namespace MicrosoftSemanticKernel.Examples.Plugins.MicrosoftCore;

/// <summary>
/// Demonstrates how HttpPlugin, a pre-built Microsoft plugin, to retrieve some JSON using an HTTP call.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleResourceUse(Resource.HTTP)]
[ExampleCostEstimate(0.002)]
public class HttpPluginUsageExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        // Add logging to show the plugin functions being invoked
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Information));

        var kernel = builder.Build();

        kernel.Plugins.AddFromType<HttpPlugin>();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt = "Get the json from this url: https://swapi.dev/api/people/1";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);

        Console.WriteLine();
        Console.WriteLine(response.Content);
    }
}
