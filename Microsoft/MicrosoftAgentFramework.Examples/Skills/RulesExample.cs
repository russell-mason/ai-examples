namespace MicrosoftAgentFramework.Examples.Skills;

#pragma warning disable MAAI001

/// <summary>
/// Demonstrates using a set of rules to determine an approvals process.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Skills)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.001)]
public class RulesExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var skillsProvider = new AgentSkillsProvider(Path.GetFullPath(@".\\Skills\expense-report"));

        var agentOptions = new ChatClientAgentOptions
                           {
                               AIContextProviders = [skillsProvider]
                           };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .AsAIAgent(agentOptions);

        var response1 = await agent.RunAsync("Please validate a meal expense of £40 with a receipt.");

        Console.WriteLine(response1.Text);
        Console.WriteLine();

        var response2 = await agent.RunAsync("Please validate a meal expense of £60 with a receipt.");

        Console.WriteLine(response2.Text);
        Console.WriteLine();

        var response3 = await agent.RunAsync("Please validate a meal expense of £30 without a receipt.");

        Console.WriteLine(response3.Text);
    }
}

#pragma warning restore MAAI001
