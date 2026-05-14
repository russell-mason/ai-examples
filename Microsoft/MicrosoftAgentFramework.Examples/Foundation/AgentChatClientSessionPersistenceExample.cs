namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates how a session (chat history) can be serialized and deserialized in order to persist the session's
/// current state.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.001)]
public class AgentChatClientSessionPersistenceExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        // Original

        var originalAgent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                            .GetChatClient(project.DeployedModels.Default)
                            .AsAIAgent();

        var originalSession = await originalAgent.CreateSessionAsync();

        // Simulate storing a user ID in the session to correlate conversations for the same user across
        // different sessions and agents.
        originalSession.StateBag.SetValue("userId", "1");

        const string originalPrompt1 = "My name is Bob Smith.";

        var originalResponse1 = await originalAgent.RunAsync(originalPrompt1, originalSession);

        var originalAgentSessionJsonElement = await originalAgent.SerializeSessionAsync(originalSession, JsonSerializerOptions.Web);

        // Convert to a JSON string to so creating the new session is not referencing the original element object in memory,
        // i.e. simulating saving to, and retrieving from, a persistence store
        var originalAgentSessionJson = originalAgentSessionJsonElement.ToString();  

        const string originalPrompt2 = "What is my name?";

        var originalResponse2 = await originalAgent.RunAsync(originalPrompt2, originalSession);

        // Simulate persistence by restoring in a new agent instance

        var newAgent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                       .GetChatClient(project.DeployedModels.Default)
                       .AsAIAgent();

        var restoredSessionJsonElement = JsonSerializer.Deserialize<JsonElement>(originalAgentSessionJson);
        var restoredSession = await newAgent.DeserializeSessionAsync(restoredSessionJsonElement, JsonSerializerOptions.Web);

        const string newPrompt1 = "What is my name?";

        var newResponse1 = await newAgent.RunAsync(newPrompt1, restoredSession);

        Console.WriteLine(originalResponse1.Text);
        Console.WriteLine();

        Console.WriteLine("Original Agent's response:");
        Console.WriteLine(originalResponse2.Text);
        Console.WriteLine();

        Console.WriteLine("New Agent's response after Session restored:");
        Console.WriteLine(newResponse1.Text);
        Console.WriteLine();

        Console.WriteLine("Session State:");
        Console.WriteLine(originalAgentSessionJson);
    }
}
