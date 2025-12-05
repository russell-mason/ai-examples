namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates how to return a typed response message that contains structured data directly.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.StructuredData)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class StructuredDataExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent();

        const string prompt = """
                              List the top 5 cities in the UK by population.
                              The results should include the city name, population, and the year the data was captured.
                              Numeric values should not be formatted, i.e. have no commas or decimal separators.
                              """;

        var response = await agent.RunAsync<Populations>(prompt);

        Console.WriteLine(response.Text);
        Console.WriteLine();

        foreach (var city in response.Result.Cities.OrderByDescending(city => city.Population))
        {
            Console.WriteLine($"{city.Name,-12} - {city.Population,8} - {city.Year}");
        }
    }
}
