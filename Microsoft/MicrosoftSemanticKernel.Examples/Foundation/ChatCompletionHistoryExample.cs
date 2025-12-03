namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates how to create a chat history so that prior request/response messages provide historical context.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatCompletionHistoryExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();

        const string prompt1 = "My name is Bob Smith.";
        chatHistory.AddUserMessage(prompt1);

        var response1 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
        chatHistory.AddAssistantMessage(response1.Content!);

        const string prompt2 = "What is my name?";
        chatHistory.AddUserMessage(prompt2);

        var response2 = await chatCompletionService.GetChatMessageContentAsync(prompt2);

        var response3 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);

        Console.WriteLine(response1.Content);
        Console.WriteTitle("Without history ...");
        Console.WriteLine(response2.Content);
        Console.WriteTitle("With history ...");
        Console.WriteLine(response3.Content);
    }
}
