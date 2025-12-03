namespace MicrosoftSemanticKernel.Examples.ModelProviders;

/// <summary>
/// Demonstrates using a grok-3-mini model via Azure and the OpenAI chat client.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.Grok3Mini)]
[ExampleCostEstimate(0.001)]
public class AzureGrokExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.ForDeployedModel(nameof(AzureAIFoundryModelDeploymentSettings.Grok3Mini));

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Grok3Mini, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt);

        Console.WriteLine(response.Content);
    }
}
