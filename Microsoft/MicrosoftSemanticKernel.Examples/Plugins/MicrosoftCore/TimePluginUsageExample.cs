namespace MicrosoftSemanticKernel.Examples.Plugins.MicrosoftCore;

/// <summary>
/// Demonstrates how to get the number of input and output tokens used by chat completion when using a plugin. This increases the 
/// number of tokens used as additional request and response messages are created between the chat completion client and the 
/// plugin.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class TimePluginUsageExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        // In this example logging shows that TimePlugin-Now and TimePlugin-TimeZoneName are invoked
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Information));

        var kernel = builder.Build();

        kernel.Plugins.AddFromType<TimePlugin>();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt = "What is the current date and time, including the current time-zone?";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);
        var usage = (response.InnerContent as ChatCompletion)?.Usage;

        Console.WriteLine();
        Console.WriteLine(response.Content);
        Console.WriteLine($"Input: {usage?.InputTokenCount}");
        Console.WriteLine($"Output: {usage?.OutputTokenCount}");
        Console.WriteLine($"Total: {usage?.TotalTokenCount}");
    }
}
