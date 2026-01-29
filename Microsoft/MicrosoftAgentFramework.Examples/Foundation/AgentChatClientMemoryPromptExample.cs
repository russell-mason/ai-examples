namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates the ability to intercept chat, and extract the user's name. Prevents further interaction until
/// provided. This is stored in a memory state object for use across sessions and agents.
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
        var originalSession = await Chat(chatClient);

        // Grab the serialized memory from the original conversation
        var aiContextProvider = originalSession.GetService<AIContextProvider>()!;
        var memoryJsonElement = aiContextProvider.Serialize();

        // Create a new conversation with the original memory that captured the user's name
        await Chat(chatClient, memoryJsonElement);
    }

    private static ChatClientAgent CreateAgent(IChatClient chatClient, JsonElement? memoryJsonElement = null)
    {
        var agentOptions = new ChatClientAgentOptions
                           {
                               AIContextProviderFactory = (context, _) =>
                               {
                                   var serializedState = memoryJsonElement ?? context.SerializedState;

                                   return new ValueTask<AIContextProvider>(
                                       new UserMemory(chatClient, serializedState, context.JsonSerializerOptions));
                               },
                               ChatOptions = new ChatOptions { Instructions = "When providing responses, be brief." }
                           };

        var agent = chatClient.AsAIAgent(agentOptions);

        return agent;
    }

    private static async Task<AgentSession> Chat(IChatClient chatClient)
    {
        var agent = CreateAgent(chatClient);
        var session = await agent.GetNewSessionAsync();

        Console.WriteHighlight("Original Agent");
        Console.WriteLine();

        // Uncomment this block and comment out the following block to see what happens if the user's name
        // is specified before the questions are asked.

        //await Ask(agent, session, "Hi I'm Washington Hall");
        //await Ask(agent, session, "What is the Washington Monument?");
        //await Ask(agent, session, "Who was George Washington?");

        await Ask(agent, session, "What is the Washington Monument?");
        await Ask(agent, session, "Who was George Washington?");
        await Ask(agent, session, "Washington Hall");
        await Ask(agent, session, "Please answer the questions I asked.");

        return session;
    }

    private static async Task Chat(IChatClient chatClient, JsonElement memoryJsonElement)
    {
        var agent = CreateAgent(chatClient, memoryJsonElement);
        var session = await agent.GetNewSessionAsync();

        Console.WriteHighlight("New Agent");
        Console.WriteLine();

        await Ask(agent, session, "What is my name?");
    }

    private static async Task Ask(ChatClientAgent agent, AgentSession session, string message)
    {
        var response = await agent.RunAsync(message, session);
        var memory = session.GetService<UserMemory>()!;

        Console.WriteLine(message);
        Console.WriteLine(response.Text);
        Console.WriteInfo($"Name: [{memory.User.Name}]");
        Console.WriteLine();
    }
}
