namespace MicrosoftSemanticKernel.Examples.ModelProviders;

/// <summary>
/// Demonstrates using a gpt-4o-mini model via Azure and the OpenAI chat client.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class AzureOpenAIExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt);

        Console.WriteLine(response.Content);
    }
}
