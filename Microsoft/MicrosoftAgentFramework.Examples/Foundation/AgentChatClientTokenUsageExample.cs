namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates how to get the number of input and output tokens used by agent request/response messages.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class AgentChatClientTokenUsageExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent();

        const string prompt = "What is the capital of England?";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
        Console.WriteLine($"Input: {response.Usage?.InputTokenCount}");
        Console.WriteLine($"Output: {response.Usage?.OutputTokenCount}");
        Console.WriteLine($"Total: {response.Usage?.TotalTokenCount}");
    }
}
