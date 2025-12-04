namespace MicrosoftAgentFramework.Examples.Foundation;

public class StructuredDataFromJsonSchemaExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var chatOptions = new ChatOptions
                          {
                              ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(Populations)),
                                                                                nameof(Populations),
                                                                                "Information about the population of countries")
                          };

        var agentOptions = new ChatClientAgentOptions { ChatOptions = chatOptions };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(agentOptions);

        const string prompt = """
                              List the top 5 cities in the UK by population.
                              The results should include the city name, population, and the year the data was captured.
                              Numeric values should not be formatted, i.e. have no commas or decimal separators.
                              """;

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
