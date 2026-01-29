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

        var originalSession = await originalAgent.GetNewSessionAsync();

        const string originalPrompt1 = "My name is Bob Smith.";

        var originalResponse1 = await originalAgent.RunAsync(originalPrompt1, originalSession);

        var persistedSessionJson = originalSession.Serialize(JsonSerializerOptions.Web).GetRawText();

        const string originalPrompt2 = "What is my name?";

        var originalResponse2 = await originalAgent.RunAsync(originalPrompt2, originalSession);

        // Simulate persistence by restoring in a new agent instance

        var newAgent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .AsAIAgent();

        var restoredJsonElement = JsonSerializer.Deserialize<JsonElement>(persistedSessionJson, JsonSerializerOptions.Web);
        var restoredSession = await newAgent.DeserializeSessionAsync(restoredJsonElement, JsonSerializerOptions.Web);
        
        const string newPrompt1 = "What is my name?";

        var newResponse1 = await newAgent.RunAsync(newPrompt1, restoredSession);

        Console.WriteLine(originalResponse1.Text);
        Console.WriteLine();

        Console.WriteLine("Original Agent's response:");
        Console.WriteLine(originalResponse2.Text);
        Console.WriteLine();

        Console.WriteLine("New Agent's response after Session restored:");
        Console.WriteLine(newResponse1.Text);
    }
}
