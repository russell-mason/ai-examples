namespace MicrosoftAgentFramework.Examples.Foundation;

#pragma warning disable MEAI001

/// <summary>
/// Demonstrates using a chat reducer with an agent to summarize messages thus far. This will create a single 
/// message from a set of messages and replace them. This should provide enough context without having to still have 
/// all the literal messages available.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.002)]
public class SummarizingChatReducerExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var chatClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
            .GetChatClient(project.DeployedModels.Default);

        var chatReducer = new SummarizingChatReducer(chatClient.AsIChatClient(), 2, 0);

        var agentOptions = new ChatClientAgentOptions
                           {
                               ChatHistoryProviderFactory = (context, _) =>
                                   new ValueTask<ChatHistoryProvider>(new InMemoryChatHistoryProvider(
                                                                          chatReducer,
                                                                          context.SerializedState,
                                                                          context.JsonSerializerOptions,
                                                                          InMemoryChatHistoryProvider.ChatReducerTriggerEvent.AfterMessageAdded))
                           };

        // If you want to inspect the message history, comment out the above "var agentOptions = ..." and uncomment the below

        //InMemoryChatHistoryProvider historyProvider;

        //var agentOptions = new ChatClientAgentOptions
        //                   {
        //                       ChatHistoryProviderFactory = (context, _) =>
        //                       {
        //                           historyProvider =
        //                               new InMemoryChatHistoryProvider(chatReducer,
        //                                                               context.SerializedState,
        //                                                               context.JsonSerializerOptions,
        //                                                               InMemoryChatHistoryProvider.ChatReducerTriggerEvent.AfterMessageAdded);

        //                           return new ValueTask<ChatHistoryProvider>(historyProvider);
        //                       }
        //                   };

        var agent = chatClient.AsAIAgent(agentOptions);

        var session = await agent.GetNewSessionAsync();

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

        Console.WriteLine(response1.Text);
        Console.WriteLine();
        Console.WriteLine(response2.Text);
        Console.WriteLine();
        Console.WriteLine(response3.Text);
        Console.WriteLine();
        Console.WriteLine(response4.Text);
        Console.WriteLine();
        Console.WriteLine(response5.Text);
    }
}

#pragma warning restore MEAI001
