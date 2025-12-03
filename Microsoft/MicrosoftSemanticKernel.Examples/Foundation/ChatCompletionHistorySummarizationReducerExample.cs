namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates how to take a chat history and condense it to a summary of the messages thus far. This will create 
/// a single message from a set of messages and replace them. This should provide enough context without having to 
/// still have all the literal messages available.  
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatCompletionHistorySummarizationReducerExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();
        var summarizationReducer = new ChatHistorySummarizationReducer(chatCompletionService, 2);

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

        var reducedHistory = await summarizationReducer.ReduceAsync(chatHistory); // Reduces messages from 6 to 2

        if (reducedHistory != null)
        {
            chatHistory = new ChatHistory(reducedHistory);
            // Summary is added to front of history, so the history now contains 3 messages, the summary, then the 2 most recent messages

            // In this example the summary is longer than the original information but this is a contrived example
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
