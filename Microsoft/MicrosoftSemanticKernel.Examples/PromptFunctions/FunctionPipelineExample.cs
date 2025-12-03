namespace MicrosoftSemanticKernel.Examples.PromptFunctions;

// This uses prompt functions but can be used for any function created via the kernel,
// including CreateFunctionFromPrompt, CreatePluginFromPromptDirectory, CreatePluginFromType, etc.

/// <summary>
/// Demonstrates how to compose, and execute, multiple prompt functions via a pipeline passing a value from one function
/// to the next.
/// <para>
/// This uses a custom pipeline class originating from Microsoft examples.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleCategory(Category.SemanticFunctions)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.002)]
public class FunctionPipelineExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var pipelinePlugin = kernel.ImportPluginFromPromptDirectory(@".\PromptFunctions\Plugins\Pipeline");
        var findFunction = pipelinePlugin["Find"];
        var getInterestingFactFunction = pipelinePlugin["GetInterestingFact"];
        var pipeline = KernelFunctionCombinators.Pipe([findFunction, getInterestingFactFunction]);

        var arguments1 = new KernelArguments { ["criteria"] = "cities" };
        var result1 = await kernel.InvokeAsync(pipeline, arguments1);

        var arguments2 = new KernelArguments { ["criteria"] = "authors" };
        var result2 = await kernel.InvokeAsync(pipeline, arguments2);

        Console.WriteTitle("Cities...");
        Console.WriteLine();
        Console.WriteLine(result1);

        Console.WriteTitle("Authors...");
        Console.WriteLine();
        Console.WriteLine(result2);
    }
}
