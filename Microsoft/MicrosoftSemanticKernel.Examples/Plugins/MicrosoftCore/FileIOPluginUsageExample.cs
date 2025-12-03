namespace MicrosoftSemanticKernel.Examples.Plugins.MicrosoftCore;

/// <summary>
/// Demonstrates how FileIOPlugin, a pre-built Microsoft plugin, provides the ability to read the contents of a local file and use
/// that as an additional source, from within a prompt.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.002)]
public class FileIOPluginUsageExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        // Add logging to show the plugin functions being invoked
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Information));

        var kernel = builder.Build();

        kernel.Plugins.AddFromType<FileIOPlugin>();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt = """
                              Read the text file from the directory '.\Plugins\MicrosoftCore\SourceText\CharacterQuotes.txt' and determine the name of 
                              all people that are mentioned. Provide them as a numbered list.
                              Save the list to a new text file in the same directory called 'CharacterNames.txt'.
                              Please do not ask and follow up questions.
                              """;

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);

        Console.WriteLine();
        Console.WriteLine(response.Content);
    }
}
