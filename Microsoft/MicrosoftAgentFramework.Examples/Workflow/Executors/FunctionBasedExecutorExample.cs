namespace MicrosoftAgentFramework.Examples.Workflow.Executors;

/// <summary>
/// Demonstrates a simple workflow using a function based executor. 
/// <para>
/// This is only intended to demonstrate the executor itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class FunctionBasedExecutorExample : IExample
{
    public async Task ExecuteAsync()
    {
        var executor = ((Func<string, string>) ExecutorFunction).BindAsExecutor("ExecutorFunction");
        var workflowBuilder = new WorkflowBuilder(executor);
        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunAsync(workflow, "START");

        var lastEvent = run.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent.Data}");
    }

    private static string ExecutorFunction(string input)
    {
        Console.WriteLine($"STEP 1 - Input: {input}");

        return "END";
    }
}
