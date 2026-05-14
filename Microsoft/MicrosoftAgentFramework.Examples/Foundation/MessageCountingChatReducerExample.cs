namespace MicrosoftAgentFramework.Examples.Foundation;

#pragma warning disable MEAI001

/// <summary>
/// Demonstrates using a chat reducer with an agent to only include a set number of request/response messages. 
/// This will result in older messages being removed so context from further back will no longer be available.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.001)]
public class MessageCountingChatReducerExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        const int reducerTargetCount = 4;

        var project = settings.Projects.Default;

        var chatReducer = new MessageCountingChatReducer(reducerTargetCount);

        var historyProvider = new InMemoryChatHistoryProvider(new InMemoryChatHistoryProviderOptions
                                                              {
                                                                  ChatReducer = chatReducer,
                                                                  ReducerTriggerEvent = InMemoryChatHistoryProviderOptions.ChatReducerTriggerEvent.AfterMessageAdded
                                                              });

        var agentOptions = new ChatClientAgentOptions
                           {
                               ChatHistoryProvider = historyProvider
                           };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .AsAIAgent(agentOptions);

        var session = await agent.CreateSessionAsync();

        const string prompt1 = "My name is Bob Smith. I am 35 years old.";
        var response1 = await agent.RunAsync(prompt1, session);

        const string prompt2 = "What is my name?";
        var response2 = await agent.RunAsync(prompt2, session);

        const string prompt3 = "What is my age?";
        var response3 = await agent.RunAsync(prompt3, session);

        const string prompt4 = "What is my name? ";
        var response4 = await agent.RunAsync(prompt4, session);

        const string prompt5 = "What is my age? ";
        var response5 = await agent.RunAsync(prompt5, session);

        var reducedMessages = historyProvider.GetMessages(session);

        Console.WriteLine(response1.Text);
        Console.WriteLine();
        Console.WriteLine(response2.Text);
        Console.WriteLine();
        Console.WriteLine(response3.Text);
        Console.WriteLine();
        Console.WriteLine(response4.Text);
        Console.WriteLine();
        Console.WriteLine(response5.Text);

        Console.WriteTitle($"After Reduction (with a reducer target count of {reducerTargetCount}):");

        foreach (var message in reducedMessages)
        {
            Console.WriteLine();
            Console.WriteLine(message);
        }
    }
}

#pragma warning restore MEAI001
