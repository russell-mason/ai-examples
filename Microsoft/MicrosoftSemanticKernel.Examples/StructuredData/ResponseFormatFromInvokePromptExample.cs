namespace MicrosoftSemanticKernel.Examples.StructuredData;

/// <summary>
/// Demonstrates how to return a message that contains JSON by specifying the response format via settings, and then
/// deserializing the JSON to a typed object.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.StructuredData)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class ResponseFormatFromInvokePromptExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        const string prompt = """
                              List the top 5 cities in the UK by population.
                              The results should include the city name, population, and the year the data was captured.
                              Numeric values should not be formatted, i.e. have no commas or decimal separators.
                              """;

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 Temperature = 0,
                                 TopP = 0,
                                 ResponseFormat = typeof(Populations)
                             };

        var arguments = new KernelArguments(promptSettings);

        var result = await kernel.InvokePromptAsync(prompt, arguments);

        var populations = JsonSerializer.Deserialize<Populations>(result.ToString())!;

        foreach (var city in populations.Cities.OrderByDescending(city => city.Population))
        {
            Console.WriteLine($"{city.Name,-12} - {city.Population,8} - {city.Year}");
        }
    }
}
