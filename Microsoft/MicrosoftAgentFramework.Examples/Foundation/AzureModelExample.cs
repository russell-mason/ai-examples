namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates using an Azure hosted grok model with an agent.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.Grok420NonReasoning)]
[ExampleCostEstimate(0.001)]
public class AzureModelExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.ForDeployedModel(nameof(AzureAIFoundryModelDeploymentSettings.Grok420NonReasoning));

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Grok420NonReasoning)
                    .AsAIAgent();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
