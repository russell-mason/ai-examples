namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates the ability to intercept chat, and extract the user's name. Prevents further interaction until
/// provided. This is stored in a memory state object for use across threads and agents.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.02)]
public class AgentChatClientMemoryPromptExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var aiClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var chatClient = aiClient.GetChatClient(project.DeployedModels.Default).AsIChatClient();

        // Create an original conversation with memory that captures the user's name
        var originalThread = await Chat(chatClient);

        // Grab the serialized memory from the original conversation
        var aiContextProvider = originalThread.GetService<AIContextProvider>()!;
        var memoryJsonElement = aiContextProvider.Serialize();

        // Create a new conversation with the original memory that captured the user's name
        await Chat(chatClient, memoryJsonElement);
    }

    private static ChatClientAgent CreateAgent(IChatClient chatClient, JsonElement? memoryJsonElement = null)
    {
        var agentOptions = new ChatClientAgentOptions
                           {
                               AIContextProviderFactory = context =>
                               {
                                   var serializedState = memoryJsonElement ?? context.SerializedState;

                                   return new UserMemory(chatClient, serializedState, context.JsonSerializerOptions);
                               },
                               Instructions = "When providing responses, be brief."
                           };

        var agent = chatClient.CreateAIAgent(agentOptions);

        return agent;
    }

    private static async Task<AgentThread> Chat(IChatClient chatClient)
    {
        var agent = CreateAgent(chatClient);
        var thread = agent.GetNewThread();

        Console.WriteHighlight("Original Agent");
        Console.WriteLine();

        // Uncomment this block and comment out the following block to see what happens if the user's name
        // is specified before the questions are asked.

        //await Ask(agent, thread, "Hi I'm Washington Hall");
        //await Ask(agent, thread, "What is the Washington Monument?");
        //await Ask(agent, thread, "Who was George Washington?");

        await Ask(agent, thread, "What is the Washington Monument?");
        await Ask(agent, thread, "Who was George Washington?");
        await Ask(agent, thread, "Washington Hall");
        await Ask(agent, thread, "Please answer the questions I asked.");

        return thread;
    }

    private static async Task Chat(IChatClient chatClient, JsonElement memoryJsonElement)
    {
        var agent = CreateAgent(chatClient, memoryJsonElement);
        var thread = agent.GetNewThread();

        Console.WriteHighlight("New Agent");
        Console.WriteLine();

        await Ask(agent, thread, "What is my name?");
    }

    private static async Task Ask(ChatClientAgent agent, AgentThread thread, string message)
    {
        var response = await agent.RunAsync(message, thread);
        var memory = thread.GetService<UserMemory>()!;

        Console.WriteLine(message);
        Console.WriteLine(response.Text);
        Console.WriteInfo($"Name: [{memory.User.Name}]");
        Console.WriteLine();
    }
}
