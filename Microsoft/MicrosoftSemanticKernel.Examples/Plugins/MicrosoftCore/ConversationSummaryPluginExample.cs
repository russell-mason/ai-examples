namespace MicrosoftSemanticKernel.Examples.Plugins.MicrosoftCore;

/// <summary>
/// Demonstrates how ConversationSummaryPlugin, a pre-built Microsoft plugin, can take a block of text and create a summary from it.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class ConversationSummaryPluginExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        // Add logging to show the plugin functions being invoked
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Information));

        var kernel = builder.Build();

        kernel.Plugins.AddFromType<ConversationSummaryPlugin>();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt = """
                              Summarize the following conversation between a customer and a support agent into key topics discussed and any action items agreed upon.
                              Customer: Hi, I'm having trouble with my internet connection. It keeps dropping every few minutes.
                              Support Agent: I'm sorry to hear that. Let's run through some troubleshooting steps. Have you tried restarting your router?
                              Customer: Yes, I have restarted it a couple of times, but the issue persists.
                              Support Agent: Understood. I'll check if there are any outages in your area. Meanwhile, could you please provide me with your account number?
                              Customer: Sure, it's 123456789.
                              Support Agent: Thank you. I see there are no outages reported. I'll escalate this issue to our technical team who will contact you within 24 hours.
                              """;

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);

        Console.WriteLine();
        Console.WriteLine(response.Content);
    }
}
