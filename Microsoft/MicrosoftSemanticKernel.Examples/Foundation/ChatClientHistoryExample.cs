using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates how to create a chat history so that prior request/response messages provide historical context.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatClientHistoryExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatClient(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatClient>();

        var chatHistory = new List<ChatMessage>();

        const string prompt1 = "My name is Bob Smith.";
        chatHistory.Add(new ChatMessage(ChatRole.User, prompt1));

        var response1 = await chatCompletionService.GetResponseAsync(chatHistory);
        chatHistory.Add(new ChatMessage(ChatRole.Assistant, response1.Text));

        const string prompt2 = "What is my name?";
        chatHistory.Add(new ChatMessage(ChatRole.User, prompt2));

        var response2 = await chatCompletionService.GetResponseAsync(prompt2);

        var response3 = await chatCompletionService.GetResponseAsync(chatHistory);

        Console.WriteLine(response1.Text);
        Console.WriteTitle("Without history ...");
        Console.WriteLine(response2.Text);
        Console.WriteTitle("With history ...");
        Console.WriteLine(response3.Text);
    }
}
