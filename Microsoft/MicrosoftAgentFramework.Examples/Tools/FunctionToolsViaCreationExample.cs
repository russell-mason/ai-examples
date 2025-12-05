namespace MicrosoftAgentFramework.Examples.Tools;

/// <summary>
/// Demonstrates how to create tools from static functions and include them when creating the agent.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Tools)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class FunctionToolsViaCreationExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        List<AITool> tools =
        [
            AIFunctionFactory.Create(PersonalDetailsFunctions.GetPeople),
            AIFunctionFactory.Create(PersonalDetailsFunctions.GetAreaCode)
        ];

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(tools: tools);

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
