namespace MicrosoftAgentFramework.Examples.Tools;

#pragma warning disable MEAI001

public class FunctionToolApprovalExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        AITool[] tools =
        [
            new ApprovalRequiredAIFunction(AIFunctionFactory.Create(GetDateTime)),
            new ApprovalRequiredAIFunction(AIFunctionFactory.Create(GetCurrentLocation))
        ];

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .CreateAIAgent(tools: tools);

        var thread = agent.GetNewThread();

        const string prompt = "What is the time for my current location?";

        var response = await agent.RunAsync(prompt, thread);
        var approvals = GetApprovals(response);

        // There can be multiple approvals required for the same response
        // There can be multiple responses that require approval
        // GetCurrentLocation is requested first, then GetDateTime, as two separate responses
        while (approvals.Count > 0)
        {
            var functionNames = string.Join(", ", approvals.Select(approval => approval.FunctionCall.Name));

            Console.WriteLine($"Approval is required to execute: '{functionNames}'. Do you agree? (Y or N)");

            if (Console.ReadLine() is not "Y" and not "y")
            {
                Console.WriteLine();
                Console.WriteLine("Function execution not approved. Terminating.");

                return;
            }

            var approvalMessages = CreateApprovalMessages(approvals);

            response = await agent.RunAsync(approvalMessages, thread);
            approvals = GetApprovals(response);
        }

        Console.WriteLine();
        Console.WriteLine(response.Text);
    }

    private static List<FunctionApprovalRequestContent> GetApprovals(AgentRunResponse response) =>
        response.Messages
                .SelectMany(message => message.Contents)
                .OfType<FunctionApprovalRequestContent>()
                .ToList();

    private static List<ChatMessage> CreateApprovalMessages(List<FunctionApprovalRequestContent> approvals) =>
        approvals.Select(approval => new ChatMessage(ChatRole.User, [approval.CreateResponse(true)])).ToList();

    [Description("Get the current date and time in UTC")]
    public static DateTime GetDateTime() => DateTime.UtcNow;

    [Description("Get the current user's location")]
    public static string GetCurrentLocation() => "London";
}

#pragma warning restore MEAI001
