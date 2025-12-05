namespace MicrosoftAgentFramework.Examples.Tools;

/// <summary>
/// Demonstrates how to use an agent as a tool from within another agent.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Tools)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.002)]
public class AgentAsToolExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        // Tool agent can only answer people related questions
        var peopleAgent = CreatePeopleToolAgent(project);

        Console.WriteTitle("Tool Agent ...");
        Console.WriteLine(await peopleAgent.RunAsync("What is the capital of England"));
        Console.WriteLine();
        Console.WriteLine(await peopleAgent.RunAsync("What is the telephone number for Bob Smith"));


        // Main agent that includes the people agent as a tool, can answer all questions
        var mainAgent = CreateMainAgent(project);

        Console.WriteTitle("Main Agent ...");
        Console.WriteLine(await mainAgent.RunAsync("What is the capital of England"));
        Console.WriteLine();
        Console.WriteLine(await mainAgent.RunAsync("What is the telephone number for Bob Smith"));
    }

    public static AIAgent CreateMainAgent(AzureAIFoundryProjectSettings project)
    {
        var peopleAgent = CreatePeopleToolAgent(project);

        var options = new ChatClientAgentOptions
                      {
                          ChatOptions = new ChatOptions { Tools = [peopleAgent.AsAIFunction()] }
                      };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(options);

        return agent;
    }

    public static AIAgent CreatePeopleToolAgent(AzureAIFoundryProjectSettings project)
    {
        var options = new ChatClientAgentOptions
                      {
                          Instructions = """
                                         You are an Agent that provides details about staff members. 
                                         If the details are not related to staff members politely inform the user
                                         that those details are unavailable.
                                         If asked about any other subject politely inform the user that those details 
                                         are not within the scope of your knowledge.
                                         """,
                          ChatOptions = new ChatOptions { Tools = PersonalDetailsFunctions.AsAITools() }
                      };

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(options);

        return agent;
    }
}
