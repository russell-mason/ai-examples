namespace MicrosoftAgentFramework.Examples.Foundation;

public class AgentChatClientOptionsExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agentOptions = new ChatClientAgentOptions
                           {
                               Instructions = "You are an AI Assistant designed for use by very young children.",
                               ChatOptions = new ChatOptions
                                             {
                                                 Temperature = 0.5f,
                                                 TopP = 0.5f,
                                                 MaxOutputTokens = 1000
                                             }
                           };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(agentOptions);

        const string prompt = "Explain the concept of a sphere";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
