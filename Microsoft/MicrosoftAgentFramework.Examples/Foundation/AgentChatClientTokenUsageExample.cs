namespace MicrosoftAgentFramework.Examples.Foundation;

public class AgentChatClientTokenUsageExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent();

        const string prompt = "What is the capital of England?";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
        Console.WriteLine($"Input: {response.Usage?.InputTokenCount}");
        Console.WriteLine($"Output: {response.Usage?.OutputTokenCount}");
        Console.WriteLine($"Total: {response.Usage?.TotalTokenCount}");
    }
}
