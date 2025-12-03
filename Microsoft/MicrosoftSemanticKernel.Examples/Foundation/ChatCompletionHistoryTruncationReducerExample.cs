namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates how to take a chat history and condense it to only include a set number of request/response messages. 
/// This will result in older messages being removed so context from further back will no longer be available.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class ChatCompletionHistoryTruncationReducerExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();
        var truncationReducer = new ChatHistoryTruncationReducer(2);

        const string prompt1 = "My name is Bob Smith. I am 35 years old.";
        chatHistory.AddUserMessage(prompt1); // 1

        var response1 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
        chatHistory.AddAssistantMessage(response1.Content!); // 2

        const string prompt2 = "What is my name?";
        chatHistory.AddUserMessage(prompt2); // 3

        var response2 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
        chatHistory.AddAssistantMessage(response2.Content!); // 4

        const string prompt3 = "What is my age?";
        chatHistory.AddUserMessage(prompt3); // 5

        var response3 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
        chatHistory.AddAssistantMessage(response3.Content!); // 6

        var reducedHistory = await truncationReducer.ReduceAsync(chatHistory); // Reduces messages from 6 to 2

        if (reducedHistory != null)
        {
            chatHistory = new ChatHistory(reducedHistory);
            // History no longer contains name
            // Although the last message still contains the age, the context is lost so this is still unknown in future responses
        }

        const string prompt4 = "What is my name? ";
        chatHistory.AddUserMessage(prompt4);

        var response4 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
        chatHistory.AddAssistantMessage(response4.Content!);

        const string prompt5 = "What is my age? ";
        chatHistory.AddUserMessage(prompt5);

        var response5 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
        chatHistory.AddAssistantMessage(response5.Content!);

        Console.WriteLine(response1.Content);
        Console.WriteLine();
        Console.WriteLine(response2.Content);
        Console.WriteLine();
        Console.WriteLine(response3.Content);
        Console.WriteLine();
        Console.WriteLine(response4.Content);
        Console.WriteLine();
        Console.WriteLine(response5.Content);
    }
}
