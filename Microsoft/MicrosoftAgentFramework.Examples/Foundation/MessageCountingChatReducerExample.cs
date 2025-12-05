namespace MicrosoftAgentFramework.Examples.Foundation;

#pragma warning disable MEAI001

/// <summary>
/// Demonstrates using a chat reducer with an agent to only include a set number of request/response messages. 
/// This will result in older messages being removed so context from further back will no longer be available.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class MessageCountingChatReducerExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agentOptions = new ChatClientAgentOptions
                           {
                               ChatMessageStoreFactory = context =>
                                   new InMemoryChatMessageStore(new MessageCountingChatReducer(4),
                                                                context.SerializedState,
                                                                context.JsonSerializerOptions,
                                                                InMemoryChatMessageStore.ChatReducerTriggerEvent.AfterMessageAdded)
                           };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(agentOptions);

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
