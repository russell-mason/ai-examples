namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates how to create a thread (chat history) so that prior request/response messages provide historical context.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class AgentChatClientThreadExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent();

        var thread = agent.GetNewThread();

        const string prompt1 = "My name is Bob Smith.";

        var response1 = await agent.RunAsync(prompt1, thread);

        const string prompt2 = "What is my name?";

        var response2 = await agent.RunAsync(prompt2);

        var response3 = await agent.RunAsync(prompt2, thread);

        Console.WriteLine(response1.Text);
        Console.WriteTitle("Without history ...");
        Console.WriteLine(response2.Text);
        Console.WriteTitle("With history ...");
        Console.WriteLine(response3.Text);
    }
}
