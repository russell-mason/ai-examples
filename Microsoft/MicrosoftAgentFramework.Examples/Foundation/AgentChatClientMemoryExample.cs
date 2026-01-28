namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates the ability to intercept, extract, and capture, information provided during the chat and
/// store it in a memory state object for use across threads and agents.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.05)]
public class AgentChatClientMemoryExample(AzureAIFoundrySettings settings) : IExample
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

                                   return new UserPreferencesMemory(chatClient, serializedState, context.JsonSerializerOptions);
                               },
                               Instructions = "When providing responses, be brief.", 
                               ChatOptions = new ChatOptions { Temperature = 0, TopK = 1, TopP = 0.1f }
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

        // This is nonsense, but designed to have some ambiguity and to test if rules are followed correctly
        // For demonstration purposes only
        await Ask(agent, thread, "What is the capital of England?");
        await Ask(agent, thread, "I like bacon and Chicken.");
        await Ask(agent, thread, "I dislike fish, same for lamb.");
        await Ask(agent, thread, "Who is Winston Churchill?");
        await Ask(agent, thread, "I prefer Broccoli, but am not keen on spinach.");
        await Ask(agent, thread, "My name is Washington Hall.");
        await Ask(agent, thread, "I enjoy potato, but can't say the same for sweet potato.");
        await Ask(agent, thread, "I had a holiday in the USA recently. " +
                                 "dallas was great, but my visit to Las vegas was disappointing. " +
                                 "I also visited New York and Florida. " +
                                 "The grand canyon was the best, but still not as good as tower bridge!");
        
        return thread;
    }

    private static async Task Chat(IChatClient chatClient, JsonElement memoryJsonElement)
    {
        var agent = CreateAgent(chatClient, memoryJsonElement);
        var thread = agent.GetNewThread();

        Console.WriteHighlight("New Agent");
        Console.WriteLine();

        await Ask(agent, thread, "What is the capital of France?");
        await Ask(agent, thread, "Can you suggest a meal with multiple ingredients I like?");
        await Ask(agent, thread, "From the places I have visited, list the ones I like?");
    }

    private static async Task Ask(ChatClientAgent agent, AgentThread thread, string message)
    {
        var response = await agent.RunAsync(message, thread);
        var memory = thread.GetService<UserPreferencesMemory>()!;

        Console.WriteLine(message);
        Console.WriteLine(response.Text);
        Console.WriteInfo($"Name: [{memory.UserPreferences.Name}]");
        Console.WriteInfo($"Likes: [{string.Join(", ", memory.UserPreferences.Likes)}]");
        Console.WriteInfo($"Dislikes: [{string.Join(", ", memory.UserPreferences.Dislikes)}]");
        Console.WriteLine();
    }
}
