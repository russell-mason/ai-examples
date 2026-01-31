namespace MicrosoftAgentFramework.Examples.Tools;

/// <summary>
/// Demonstrates the use of an MCP Server as Agent AI Tools.
/// <para>
/// Shows which MCP tool is used based on the phrasing of each question and the best match based on the MCP tool's
/// description and parameters.
/// </para>
/// <para>
/// N.B. The HumanResourcesMcpServer should NOT be running.
/// It will be automatically started by this example using an stdio transport.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Tools)]
[ExampleCategory(Category.ModelContextProtocol)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT41Mini)]
[ExampleCostEstimate(0.001)]
public class HumanResourcesMcpToolsExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var mcpClientTransport = new StdioClientTransport(
            new StdioClientTransportOptions
            {
                Command = "dotnet run",
                Arguments = ["--project", @".\..\..\..\..\..\Microsoft\HumanResourcesMcpServer"],
                Name = "Human Resources MCP Server"
            });

        var mcpClient = await McpClient.CreateAsync(mcpClientTransport);
        var mcpTools = await mcpClient.ListToolsAsync();

        var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                    .GetChatClient(project.DeployedModels.Default)
                    .AsIChatClient()
                    .AsAIAgent(tools: [.. mcpTools]);

        var session = await agent.GetNewSessionAsync();

        const string prompt1 = "What is the telephone number for bob smith, and when is he available?";
        var response1 = await agent.RunAsync(prompt1, session);

        const string prompt2 = "What is the area code associated with that number?";
        var response2 = await agent.RunAsync(prompt2, session);

        const string prompt3 = "Which employee has the highest salary? Assume GBP, and restrict to 2 decimal places.";
        var response3 = await agent.RunAsync(prompt3, session);

        const string prompt4 = "Which employee has the lowest salary? Assume GBP, and restrict to 2 decimal places.";
        var response4 = await agent.RunAsync(prompt4, session);

        const string prompt5 = "Who is the newest employee?";
        var response5 = await agent.RunAsync(prompt5, session);

        const string prompt6 = "Who in available to be contacted after 4pm?";
        var response6 = await agent.RunAsync(prompt6, session);

        WriteResponse(response1);
        Console.WriteLine();

        WriteResponse(response2);
        Console.WriteLine();

        WriteResponse(response3);
        Console.WriteLine();

        WriteResponse(response4);
        Console.WriteLine();

        WriteResponse(response5);
        Console.WriteLine();

        WriteResponse(response6);
    }

    private static void WriteResponse(AgentResponse response)
    {
        var toolCallMessages = response.Messages
                                       .Where(message => message.Contents.Any(content => content is FunctionCallContent))
                                       .ToList();

        if (toolCallMessages.Count > 0)
        {
            WriteToolCallParameters(toolCallMessages);
            Console.WriteLine();
        }

        Console.WriteLine(response.Text);
    }

    private static void WriteToolCallParameters(IEnumerable<ChatMessage> messages)
    {
        var calls = messages.SelectMany(message => message.Contents.OfType<FunctionCallContent>(),
                                        (message, call) => (message.Role, Call: call));

        foreach (var (role, call) in calls)
        {
            Console.WriteInfo($"{role}: {call.Name}");

            var arguments = call.Arguments?.Count > 0
                ? string.Join(", ", call.Arguments.Select(pair => $"{pair.Key}: {pair.Value}"))
                : string.Empty;

            Console.WriteInfo($"  [{arguments}]");
        }
    }
}
