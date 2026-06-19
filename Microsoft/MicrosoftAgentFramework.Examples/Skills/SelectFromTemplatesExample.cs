namespace MicrosoftAgentFramework.Examples.Skills;

#pragma warning disable MAAI001

/// <summary>
/// Demonstrates using a skill that selects an appropriate template and fills in details based on the prompt.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Skills)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.001)]
public class SelectFromTemplatesExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var skillsProvider = new AgentSkillsProvider(Path.GetFullPath(@".\\Skills\job-interview-email-response"));

        var agentOptions = new ChatClientAgentOptions
                           {
                               AIContextProviders = [skillsProvider]
                           };

        const string offerPrompt = 
            "Create a job offer email to Mike Jones (his email is mj@somemail.com) " +
            "who has been offered the role of Assistant Manager." + 
            "Date the email 2026-02-01. His start date will be 2026-03-01 and he will report to Jane Doe.";

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .AsAIAgent(agentOptions);

        var offerResponse = await agent.RunAsync(offerPrompt);

        Console.WriteTitle("Offer");
        Console.WriteLine(offerResponse.Text);
        Console.WriteLine();

        const string rejectionPrompt = 
            "Create an email to Henry Hall (his email is henry.hall@somemail.com) " +
            "who has been turned down for the role of Assistant Manager." +
            "Date the email 2026-02-01.";

        var rejectResponse = await agent.RunAsync(rejectionPrompt);

        Console.WriteTitle("Reject");
        Console.WriteLine(rejectResponse.Text);
    }
}

#pragma warning restore MAAI001
