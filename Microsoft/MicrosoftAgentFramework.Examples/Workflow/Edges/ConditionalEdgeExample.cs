namespace MicrosoftAgentFramework.Examples.Workflow.Edges;

/// <summary>
/// Demonstrates a simple workflow using a conditional edge. 
/// <para>
/// A -- 1 --> B <br/>
/// A -- 2 --> C
/// </para>
/// <para>
/// This is only intended to demonstrate the workflow itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class ConditionalEdgeExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, int>) StepA).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<int, string>) StepB).BindAsExecutor("StepBExecutor");
        var stepCExecutor = ((Func<int, string>) StepC).BindAsExecutor("StepCExecutor");

        Func<int, bool> conditionAToB = result => result == 1;
        Func<int, bool> conditionAToC = result => result == 2;

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);
        workflowBuilder.AddEdge(stepAExecutor, stepBExecutor, conditionAToB);
        workflowBuilder.AddEdge(stepAExecutor, stepCExecutor, conditionAToC);

        var workflow1 = workflowBuilder.Build();
        var workflow2 = workflowBuilder.Build();

        await using var run1 = await InProcessExecution.RunAsync(workflow1, "START 1");

        var lastEvent1 = run1.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent1.Data}");

        Console.WriteLine();
        
        await using var run2 = await InProcessExecution.RunAsync(workflow2, "START 2");

        var lastEvent2 = run2.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent2.Data}");
    }

    private static int StepA(string input)
    {
        Console.WriteLine($"STEP A - Input: {input}");

        return input.Contains('1') ? 1 : 2;
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
