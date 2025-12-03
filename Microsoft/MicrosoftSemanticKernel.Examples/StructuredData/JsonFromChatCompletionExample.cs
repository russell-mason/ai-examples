namespace MicrosoftSemanticKernel.Examples.StructuredData;

/// <summary>
/// Demonstrates how to return a message that contains JSON by describing the format in the prompt.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.StructuredData)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class JsonFromChatCompletionExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = """
                              List the top 5 cities in the UK by population.
                              The results should include the city name, population, and the year the data was captured.
                              Numeric values should not be formatted, i.e. have no commas or decimal separators.
                              Format the results as JSON using the following example:
                              { "Cities": [ "Name": "[city_name]", "Population": "[population]", "Year": "[year_when_data_was_recorded]" ] }
                              """;

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 Temperature = 0,
                                 TopP = 0
                             };

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings);

        Console.WriteLine(response.Content);
    }
}
