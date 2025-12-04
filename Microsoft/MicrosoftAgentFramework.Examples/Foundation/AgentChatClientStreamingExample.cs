namespace MicrosoftAgentFramework.Examples.Foundation;

public class AgentChatClientStreamingExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent();

        const string prompt = "What is a large language model?";

        var response = agent.RunStreamingAsync(prompt);

        await foreach (var update in response)
        {
            Console.Write(update);
        }

        Console.WriteLine();
    }
}
