namespace MicrosoftAgentFramework.Examples.Foundation;

public class AzureModelExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.ForDeployedModel(nameof(AzureAIFoundryModelDeploymentSettings.Grok3Mini));

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Grok3Mini)
                    .CreateAIAgent();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
