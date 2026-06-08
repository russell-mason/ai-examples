    namespace MicrosoftAgentFramework.Examples.Workflow.Edges;

/// <summary>
/// Demonstrates a simple workflow using a direct edge. 
/// <para>
/// A --> B
/// </para>
/// <para>
/// This is only intended to demonstrate the workflow itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class DirectEdgeExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, int>) StepA).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<int, string>) StepB).BindAsExecutor("StepBExecutor");

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);
        workflowBuilder.AddEdge(stepAExecutor, stepBExecutor);

        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunAsync(workflow, "START");

        var lastEvent = run.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent.Data}");
    }

    private static int StepA(string input)
    {
        Console.WriteLine($"STEP A - Input: {input}");

        return 1;
    }

    private static string StepB(int input)
    {
        Console.WriteLine($"STEP B - Input: {input}");

        return "END B";
    }
}
