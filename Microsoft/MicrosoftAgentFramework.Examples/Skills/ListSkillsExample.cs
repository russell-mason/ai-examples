namespace MicrosoftAgentFramework.Examples.Skills;

#pragma warning disable MAAI001

/// <summary>
/// Demonstrates that multiple skills can be automatically loaded from skills based subdirectories.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Skills)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.001)]
public class ListSkillsExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var skillsProvider = new AgentSkillsProvider(Path.GetFullPath(@".\\Skills"));

        var agentOptions = new ChatClientAgentOptions
                           {
                               AIContextProviders = [skillsProvider]
                           };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .AsAIAgent(agentOptions);

        var response = await agent.RunAsync("Provide a list of all skills you have available.");

        Console.WriteLine(response.Text);
    }
}

#pragma warning restore MAAI001
