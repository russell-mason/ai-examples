namespace MicrosoftAgentFramework.Examples.Foundation;

public class AgentChatClientExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        const string instructions = """
                                    You are an AI translation service that translates English to French.
                                    Your role is to accept English text and echo back that text in French.
                                    Once you have given the response please do not make any further statements, or ask any follow up questions.
                                    """;

        // This is a simplified version of creating the AI client, chat client, and agent, as separate objects
        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(instructions);

        const string prompt = "Hello, How do I get to the library?";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
