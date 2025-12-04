namespace MicrosoftAgentFramework.Examples.Tools;

public class FunctionToolsViaSelfExposedExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(tools: PersonalDetailsFunctions.AsAITools());

        var thread = agent.GetNewThread();

        const string prompt1 = "What is the telephone number for Bob Smith, and when is he available?";

        var response1 = await agent.RunAsync(prompt1, thread);

        const string prompt2 = "What is the area code associated with that number?";

        var response2 = await agent.RunAsync(prompt2, thread);

        Console.WriteLine(response1.Text);
        Console.WriteLine();
        Console.WriteLine(response2.Text);
    }
}
