namespace MicrosoftSemanticKernel.Examples.Plugins.Native;

/// <summary>
/// Demonstrates how to create a custom plugin that uses dependency injection to access external functionality via services.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class TaxCodeServicePluginExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        builder.Services.AddSingleton<ITaxCodeService, TaxCodeService>();
        builder.Services.AddSingleton<TaxCodeServicePlugin>();

        var kernel = builder.Build();

        var taxCodeServicePlugin = kernel.GetRequiredService<TaxCodeServicePlugin>();
        kernel.Plugins.AddFromObject(taxCodeServicePlugin);

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt = "What is the tax code for the The Big Blue Box Company?";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);

        Console.WriteLine(response.Content);
    }
}
