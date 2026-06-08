namespace MicrosoftAgentFramework.Examples.Workflow.Edges;

/// <summary>
/// Demonstrates a simple workflow using a fan-in edge. 
/// <para>
/// A --> B <br/>
/// A --> C <br/>
/// B --> D <br/>
/// C --> D
/// </para>
/// <para>
/// This is only intended to demonstrate the workflow itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class FanInEdgeExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, int>) StepA).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<int, int>) StepB).BindAsExecutor("StepBExecutor");
        var stepCExecutor = ((Func<int, int>) StepC).BindAsExecutor("StepCExecutor");
        var stepDExecutor = ((Func<int, string>) StepD).BindAsExecutor("StepDExecutor");

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);
        workflowBuilder.AddFanOutEdge(stepAExecutor, [stepBExecutor, stepCExecutor]);
        workflowBuilder.AddFanInBarrierEdge([stepBExecutor, stepCExecutor], stepDExecutor);

        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunAsync(workflow, "START 1");

        var lastEvents = run.OutgoingEvents
                            .OfType<ExecutorCompletedEvent>()
                            .Where(@event => (@event.Data as string)?.StartsWith("END") == true);

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

    private static int StepB(int input)
    {
        Console.WriteLine($"STEP B - Input: {input}");

        return input * 10;
    }

    private static int StepC(int input)
    {
        Console.WriteLine($"STEP C - Input: {input}");

        return input * 100;
    }

    private static string StepD(int input)
    {
        Console.WriteLine($"STEP D - Input: {input}");

        return "END D";
    }
}
