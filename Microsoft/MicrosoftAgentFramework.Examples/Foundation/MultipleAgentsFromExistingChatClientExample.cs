namespace MicrosoftAgentFramework.Examples.Foundation;

public class MultipleAgentsFromExistingChatClientExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        const string instructions = """
                                    You are an AI translation service that translates English to {{$Language}}.
                                    Your role is to accept English text and echo back that text in {{$Language}}.
                                    Once you have given the response please do not make any further statements, or ask any follow up questions.
                                    """;

        var aiClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var chatClient = aiClient.GetChatClient(project.DeployedModels.Default).AsIChatClient();
        
        var frenchAgent = chatClient.CreateAIAgent(instructions.Replace("{{$Language}}", "French"));
        var germanAgent = chatClient.CreateAIAgent(instructions.Replace("{{$Language}}", "German"));

        const string prompt = "Hello, How do I get to the library?";

        var frenchResponse = await frenchAgent.RunAsync(prompt);
        var germanResponse = await germanAgent.RunAsync(prompt);

        Console.WriteTitle("French ...");
        Console.WriteLine(frenchResponse.Text);

        Console.WriteLine();
        
        Console.WriteTitle("German ...");
        Console.WriteLine(germanResponse.Text);
    }
}
