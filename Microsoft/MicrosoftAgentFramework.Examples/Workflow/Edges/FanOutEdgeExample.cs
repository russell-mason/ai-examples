namespace MicrosoftAgentFramework.Examples.Workflow.Edges;

/// <summary>
/// Demonstrates a simple workflow using a fan-out edge. 
/// <para>
/// A --> B <br/>
/// A --> C
/// </para>
/// <para>
/// This is only intended to demonstrate the workflow itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class FanOutEdgeExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, int>) StepA).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<int, string>) StepB).BindAsExecutor("StepBExecutor");
        var stepCExecutor = ((Func<int, string>) StepC).BindAsExecutor("StepCExecutor");

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);
        workflowBuilder.AddFanOutEdge(stepAExecutor, [stepBExecutor, stepCExecutor]);

        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunAsync(workflow, "START 1");

        var lastEvents = run.OutgoingEvents.OfType<ExecutorCompletedEvent>().TakeLast(2);

        foreach (var lastEvent in lastEvents)
        {
            Console.WriteLine($"RESULT - Output: {lastEvent.Data}");
        }
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

    private static string StepC(int input)
    {
        Console.WriteLine($"STEP C - Input: {input}");

        return "END C";
    }
}
