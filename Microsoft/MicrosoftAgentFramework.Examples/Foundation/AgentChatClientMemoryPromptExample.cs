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
    private const string DemoUserId = "1";

    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var aiClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var chatClient = aiClient.GetChatClient(project.DeployedModels.Default).AsIChatClient();
        var dataStore = new DataStore<User>();

        // Create an original conversation that captures the user's name
        await InitialChat(chatClient, dataStore);

        // Create a new conversation that can use the captured information from the original conversation
        // to provide more personalized responses
        await SubsequentChat(chatClient, dataStore);
    }

    private static ChatClientAgent CreateAgent(IChatClient chatClient, DataStore<User> dataStore)
    {
        var agentOptions = new ChatClientAgentOptions
        {
            AIContextProviders = [new UserMemory(chatClient, dataStore)],
            ChatOptions = new ChatOptions { Instructions = "When providing responses, be brief." }
        };

        var agent = chatClient.AsAIAgent(agentOptions);

        return agent;
    }

    // Simulates setting a user ID against the session in order to correlate independent conversations for
    // the same user.
    private static void SetSessionIdentifier(AgentSession session) => 
        session.StateBag.SetValue(UserPreferencesMemory.UserIdStateKey, DemoUserId);

    private static async Task InitialChat(IChatClient chatClient, DataStore<User> dataStore)
    {
        var agent = CreateAgent(chatClient, dataStore);
        var session = await agent.CreateSessionAsync();

        SetSessionIdentifier(session);

        Console.WriteHighlight("Original Agent");
        Console.WriteLine();

        // Uncomment this block and comment out the following block to see what happens if the user's name
        // is specified before the questions are asked.

        //await Ask(agent, session, dataStore, "Hi I'm Washington Hall");
        //await Ask(agent, session, dataStore, "What is the Washington Monument?");
        //await Ask(agent, session, dataStore, "Who was George Washington?");

        await Ask(agent, session, dataStore, "What is the Washington Monument?");
        await Ask(agent, session, dataStore, "Who was George Washington?");
        await Ask(agent, session, dataStore, "Washington Hall");
        await Ask(agent, session, dataStore, "Please answer the questions I asked.");
    }

    private static async Task SubsequentChat(IChatClient chatClient, DataStore<User> dataStore)
    {
        var agent = CreateAgent(chatClient, dataStore);
        var session = await agent.CreateSessionAsync();

        SetSessionIdentifier(session);

        Console.WriteHighlight("New Agent");
        Console.WriteLine();

        await Ask(agent, session, dataStore, "What is my name?");
    }

    private static async Task Ask(ChatClientAgent agent, 
                                  AgentSession session, 
                                  DataStore<User> dataStore,
                                  string message)
    {
        var response = await agent.RunAsync(message, session);
        var user = dataStore.Get(DemoUserId);

        Console.WriteLine(message);
        Console.WriteLine(response.Text);
        Console.WriteInfo($"Name: [{user.Name}]");
        Console.WriteLine();
    }
}
