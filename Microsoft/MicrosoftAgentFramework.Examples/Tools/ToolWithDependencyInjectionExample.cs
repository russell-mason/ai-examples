namespace MicrosoftAgentFramework.Examples.Tools;

/// <summary>
/// Demonstrates how a service collection can be passed to the agent so that tools can have services injected into them.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Tools)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
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
