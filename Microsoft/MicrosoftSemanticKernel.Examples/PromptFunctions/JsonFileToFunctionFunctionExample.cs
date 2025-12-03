namespace MicrosoftSemanticKernel.Examples.PromptFunctions;

/// <summary>
/// Demonstrates how to import a plugin containing a prompt function via config.json and skprompt.txt files, and then executing 
/// the function from within that plugin.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleCategory(Category.SemanticFunctions)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class JsonFileToFunctionFunctionExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var plugin = kernel.ImportPluginFromPromptDirectory(@".\PromptFunctions\Plugins\Books");
        var function = plugin["FindAuthor"];
        var arguments = new KernelArguments { ["title"] = "The Lord of the Rings" };

        var result = await kernel.InvokeAsync(function, arguments);

        Console.WriteLine($"{arguments["title"]}: {result}");
    }
}
