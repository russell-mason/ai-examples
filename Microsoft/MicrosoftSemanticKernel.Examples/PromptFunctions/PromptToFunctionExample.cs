namespace MicrosoftSemanticKernel.Examples.PromptFunctions;

/// <summary>
/// Demonstrates how to turn a parameterized prompt into a kernel function, then execute it passing the parameter.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleCategory(Category.SemanticFunctions)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class PromptToFunctionExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        const string prompt = """Who was the author of the book titled "{{$title}}"? Provide the author's name only.""";

        var function = kernel.CreateFunctionFromPrompt(prompt);
        var arguments = new KernelArguments { ["title"] = "Dune" };

        var result = await kernel.InvokeAsync(function, arguments);

        Console.WriteLine($"{arguments["title"]}: {result}");
    }
}
