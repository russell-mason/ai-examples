namespace MicrosoftAgentFramework.Examples.Foundation;

#pragma warning disable MEAI001

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
                               ChatMessageStoreFactory = context =>
                                   new InMemoryChatMessageStore(chatReducer,
                                                                context.SerializedState,
                                                                context.JsonSerializerOptions,
                                                                InMemoryChatMessageStore.ChatReducerTriggerEvent.AfterMessageAdded)
                           };

        // If you want to inspect the message store, comment out the above "var agentOptions = ..." and uncomment the below

        //InMemoryChatMessageStore messageStore;

        //var agentOptions = new ChatClientAgentOptions
        //                   {
        //                       ChatMessageStoreFactory = context =>
        //                       {
        //                           messageStore = new InMemoryChatMessageStore(chatReducer,
        //                                                                       context.SerializedState,
        //                                                                       context.JsonSerializerOptions,
        //                                                                       InMemoryChatMessageStore.ChatReducerTriggerEvent.AfterMessageAdded);

        //                           return messageStore;
        //                       }
        //                   };

        var agent = chatClient.CreateAIAgent(agentOptions);

        var thread = agent.GetNewThread();

        const string prompt1 = "My name is Bob Smith. I am 35 years old.";
        var response1 = await agent.RunAsync(prompt1, thread);

        const string prompt2 = "What is my name?";
        var response2 = await agent.RunAsync(prompt2, thread);

        const string prompt3 = "What is my age?";
        var response3 = await agent.RunAsync(prompt3, thread);

        const string prompt4 = "What is my name? ";
        var response4 = await agent.RunAsync(prompt4, thread);

        const string prompt5 = "What is my age? ";
        var response5 = await agent.RunAsync(prompt5, thread);

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
