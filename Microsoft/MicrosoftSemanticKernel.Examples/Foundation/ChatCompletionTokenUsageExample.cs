namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates how to get the number of input and output tokens used by chat request/response messages.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatCompletionTokenUsageExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = "What is the capital of England?";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt);
        var usage = (response.InnerContent as ChatCompletion)?.Usage;

        Console.WriteLine(response.Content);
        Console.WriteLine($"Input: {usage?.InputTokenCount}");
        Console.WriteLine($"Output: {usage?.OutputTokenCount}");
        Console.WriteLine($"Total: {usage?.TotalTokenCount}");
    }
}
