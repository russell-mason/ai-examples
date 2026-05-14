namespace MicrosoftAgentFramework.Examples.Tools;

/// <summary>
/// Demonstrates the use of open telemetry for capturing agent activity including the use of AI Tools.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Tools)]
[ExampleCategory(Category.OpenTelemetry)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.001)]
public class FunctionToolsViaSelfExposedWithOpenTelemetryExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        const string serviceName = "Example With Open Telemetry";

        var resourceBuilder = ResourceBuilder
                              .CreateDefault()
                              .AddService(serviceName);

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                                      .SetResourceBuilder(resourceBuilder)
                                      .AddSource(serviceName)
                                      .AddSource("*Microsoft.Extensions.AI")
                                      .AddSource("*Microsoft.Extensions.Agents*")
                                      .AddConsoleExporter()
                                      .Build();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(resourceBuilder);
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.AddConsoleExporter();
            })
        );

        var logger = loggerFactory.CreateLogger(serviceName);

        var activitySource = new ActivitySource(serviceName);
        using var activity = activitySource.StartActivity(nameof(FunctionToolsViaSelfExposedWithOpenTelemetryExample));

        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .AsAIAgent(tools: PersonalDetailsFunctions.AsAITools())
                    .AsBuilder()
                    .UseOpenTelemetry(sourceName: serviceName, configure: config => config.EnableSensitiveData = true)
                    .Build();

        var session = await agent.CreateSessionAsync();

        const string prompt1 = "What is the telephone number for Bob Smith, and when is he available?";

        var response1 = await agent.RunAsync(prompt1, session);

        const string prompt2 = "What is the area code associated with that number?";

        var response2 = await agent.RunAsync(prompt2, session);

        Console.WriteLine(response1.Text);
        Console.WriteLine();
        Console.WriteLine(response2.Text);
        Console.WriteLine();

        logger.LogInformation("The example has completed");
    }
}
