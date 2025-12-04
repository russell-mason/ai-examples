namespace MicrosoftAgentFramework.Examples.Tools;

public class ToolWithDependencyInjectionExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var services = new ServiceCollection();
        services.AddSingleton<ITaxCodeService, TaxCodeService>();
        services.AddSingleton<TaxCodeFunctions>();

        var serviceProvider = services.BuildServiceProvider();

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(services: serviceProvider,
                                   tools: serviceProvider.GetRequiredService<TaxCodeFunctions>().AsAITools());

        const string prompt = "What is the tax code for the The Big Blue Box Company?";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
