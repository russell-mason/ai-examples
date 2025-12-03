namespace MicrosoftSemanticKernel.Examples.PromptFunctions;

/// <summary>
/// Demonstrates how to import a plugin containing a prompt function via config.json and skprompt.txt files, and executing 
/// it multiple times using different parameters. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleCategory(Category.SemanticFunctions)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class TranslationFunctionExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var translationsPlugin = kernel.ImportPluginFromPromptDirectory(@".\PromptFunctions\Plugins\Translations");
        var translateFunction = translationsPlugin["Translate"];

        const string englishText = "Can you tell what the time is please?";

        var englishToFrenchArguments = new KernelArguments
                                       {
                                           ["source_language"] = "English",
                                           ["target_language"] = "French",
                                           ["text"] = englishText
                                       };

        var englishToFrenchResult = await kernel.InvokeAsync(translateFunction, englishToFrenchArguments);

        var englishToGermanArguments = new KernelArguments
                                       {
                                           ["source_language"] = "English",
                                           ["target_language"] = "German",
                                           ["text"] = englishText
                                       };

        var englishToGermanResult = await kernel.InvokeAsync(translateFunction, englishToGermanArguments);

        var frenchToEnglishArguments = new KernelArguments
                                       {
                                           ["source_language"] = "French",
                                           ["target_language"] = "English",
                                           ["text"] = englishToFrenchResult
                                       };

        var frenchToEnglishResult = await kernel.InvokeAsync(translateFunction, frenchToEnglishArguments);

        var germanToEnglishArguments = new KernelArguments
                                       {
                                           ["source_language"] = "German",
                                           ["target_language"] = "English",
                                           ["text"] = englishToGermanResult
                                       };

        var germanToEnglishResult = await kernel.InvokeAsync(translateFunction, germanToEnglishArguments);

        Console.WriteLine("English Source:");
        Console.WriteLine(englishText);

        Console.WriteTitle("English to French ...");
        Console.WriteLine(englishToFrenchResult);

        Console.WriteTitle("English to German ...");
        Console.WriteLine(englishToGermanResult);

        Console.WriteTitle("French to English ...");
        Console.WriteLine(frenchToEnglishResult);

        Console.WriteTitle("German to English ...");
        Console.WriteLine(germanToEnglishResult);
    }
}
