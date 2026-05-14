namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates the ability to intercept, extract, and capture, information provided during the chat and
/// store it in a memory state object for use across sessions and agents.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.05)]
public class AgentChatClientMemoryExample(AzureAIFoundrySettings settings) : IExample
{
    private const string DemoUserId = "1";

    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var aiClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var chatClient = aiClient.GetChatClient(project.DeployedModels.Default).AsIChatClient();
        var dataStore = new DataStore<UserPreferences>();

        // Create an original conversation that captures the user's name, likes, and dislikes
        await InitialChat(chatClient, dataStore);

        // Create a new conversation that can use the captured information from the original conversation
        // to provide more personalized responses
        await SubsequentChat(chatClient, dataStore);
    }

    private static ChatClientAgent CreateAgent(IChatClient chatClient, DataStore<UserPreferences> dataStore)
    {
        var agentOptions = new ChatClientAgentOptions
        {
            AIContextProviders = [new UserPreferencesMemory(chatClient, dataStore)],
            ChatOptions = new ChatOptions
            {
                Instructions = "When providing responses, be brief.",
                Temperature = 0,
                TopK = 1,
                TopP = 0.1f
            }
        };

        var agent = chatClient.AsAIAgent(agentOptions);

        return agent;
    }

    // Simulates setting a user ID against the session in order to correlate independent conversations for
    // the same user.
    private static void SetSessionIdentifier(AgentSession session) => 
        session.StateBag.SetValue(UserPreferencesMemory.UserIdStateKey, DemoUserId);

    private static async Task InitialChat(IChatClient chatClient,
                                          DataStore<UserPreferences> dataStore)
    {
        var agent = CreateAgent(chatClient, dataStore);
        var session = await agent.CreateSessionAsync();

        SetSessionIdentifier(session);

        Console.WriteHighlight("Original Agent");
        Console.WriteLine();

        // This is nonsense, but designed to have some ambiguity and to test if rules are followed correctly
        // For demonstration purposes only
        await Ask(agent, session, dataStore, "What is the capital of England?");
        await Ask(agent, session, dataStore, "I like bacon and Chicken.");
        await Ask(agent, session, dataStore, "I dislike fish, same for lamb.");
        await Ask(agent, session, dataStore, "Who is Winston Churchill?");
        await Ask(agent, session, dataStore, "I prefer Broccoli, but am not keen on spinach.");
        await Ask(agent, session, dataStore, "My name is Washington Hall.");
        await Ask(agent, session, dataStore, "I enjoy potato, but can't say the same for sweet potato.");
        await Ask(agent, session, dataStore, "I had a holiday in the USA recently. " +
                                             "dallas was great, but my visit to Las vegas was disappointing. " + 
                                             "I also visited New York and Florida. " + 
                                             "The grand canyon was the best, but still not as good as tower bridge!");
    }

    private static async Task SubsequentChat(IChatClient chatClient,
                                             DataStore<UserPreferences> dataStore)
    {
        var agent = CreateAgent(chatClient, dataStore);
        var session = await agent.CreateSessionAsync();

        SetSessionIdentifier(session);

        Console.WriteHighlight("New Agent");
        Console.WriteLine();

        await Ask(agent, session, dataStore, "What is the capital of France?");
        await Ask(agent, session, dataStore, "What is my name?");
        await Ask(agent, session, dataStore, "Can you suggest a meal with multiple ingredients I like?");
        await Ask(agent, session, dataStore, "From the places I have visited, list the ones I like?");
    }

    private static async Task Ask(ChatClientAgent agent, 
                                  AgentSession session,
                                  DataStore<UserPreferences> dataStore,
                                  string message)
    {
        var response = await agent.RunAsync(message, session);
        var userPreferences = dataStore.Get(DemoUserId);

        Console.WriteLine(message);
        Console.WriteLine(response.Text);
        Console.WriteInfo($"Name: [{userPreferences.Name}]");
        Console.WriteInfo($"Likes: [{string.Join(", ", userPreferences.Likes)}]");
        Console.WriteInfo($"Dislikes: [{string.Join(", ", userPreferences.Dislikes)}]");
        Console.WriteLine();
    }
}
