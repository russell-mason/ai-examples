namespace MicrosoftSemanticKernel.Examples.Plugins.MicrosoftCore;

/// <summary>
/// Demonstrates how TimePlugin, a pre-built Microsoft plugin, provides chat completion with access to date and time related 
/// functionality. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class TimePluginExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        // Add logging to show the plugin functions being invoked
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Information));

        var kernel = builder.Build();

        // Note that plugins run locally, i.e. where this code is running, not where the model is hosted
        kernel.Plugins.AddFromType<TimePlugin>();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt = "What is the current date and time, including the current time-zone?";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);

        Console.WriteLine();
        Console.WriteLine(response.Content);
    }
}
