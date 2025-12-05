namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates creating the individual elements required to obtain a chat client, and getting a simple response from a prompt. 
/// No agent involved.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatClientExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        // This allows you to authenticate and connect to Azure Open AI
        var aiClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));

        // This allows you to chat to a specific model
        var chatClient = aiClient.GetChatClient(project.DeployedModels.Default).AsIChatClient();

        var response = await chatClient.GetResponseAsync("What is the capital of England?");

        Console.WriteLine(response);
    }
}
