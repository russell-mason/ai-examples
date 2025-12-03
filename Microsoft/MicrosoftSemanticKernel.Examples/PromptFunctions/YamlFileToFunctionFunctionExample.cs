namespace MicrosoftSemanticKernel.Examples.PromptFunctions;

/// <summary>
/// Demonstrates how to create a prompt function from a yaml file, then execute it passing a parameter.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleCategory(Category.SemanticFunctions)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class YamlFileToFunctionFunctionExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var yaml = await File.ReadAllTextAsync(@".\PromptFunctions\Plugins\Books\FindAuthor.yaml");
        var function = kernel.CreateFunctionFromPromptYaml(yaml);
        var arguments = new KernelArguments { ["title"] = "1984" };

        var result = await kernel.InvokeAsync(function, arguments);

        Console.WriteLine($"{arguments["title"]}: {result}");
    }
}
