namespace MicrosoftAgentFramework.Examples.Foundation;

public class AgentFromExistingChatClientExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        // This allows you to authenticate and connect to Azure Open AI
        var aiClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));

        // This allows you to chat to a specific model
        var chatClient = aiClient.GetChatClient(project.DeployedModels.Default).AsIChatClient();
        
        // This provides a higher abstraction allowing you to use advanced tooling and workflows
        var agent = chatClient.CreateAIAgent("You are a very terse agent and answer questions as briefly as possible, and one word if possible.");

        var response = await agent.RunAsync("What is the capital of England?");

        Console.WriteLine(response.Text);
    }
}
