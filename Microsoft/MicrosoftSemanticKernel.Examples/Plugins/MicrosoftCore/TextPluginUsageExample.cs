namespace MicrosoftSemanticKernel.Examples.Plugins.MicrosoftCore;

/// <summary>
/// Demonstrates how TextPlugin, a pre-built Microsoft plugin, provides access to text based functionality, such as converting 
/// case, and determining the length of text.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class TextPluginUsageExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        // Add logging to show the plugin functions being invoked
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Information));

        var kernel = builder.Build();

        kernel.Plugins.AddFromType<TextPlugin>();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt =
            "Convert the text 'The Adventure of the Blue Carbuncle by Sir Arthur Conan Doyle' to upper case and count how many characters it contains.";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);

        Console.WriteLine();
        Console.WriteLine(response.Content);
    }
}
