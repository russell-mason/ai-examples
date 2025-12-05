namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates getting a streamed response from an agent, and  outputting response tokens as they're received.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class AgentChatClientStreamingExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent();

        const string prompt = "What is a large language model?";

        var response = agent.RunStreamingAsync(prompt);

        await foreach (var update in response)
        {
            Console.Write(update);
        }

        Console.WriteLine();
    }
}
