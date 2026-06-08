namespace MicrosoftAgentFramework.Examples.Workflow.Edges;

/// <summary>
/// Demonstrates a simple workflow using a switch-case edge. 
/// <para>
/// A -- 1 --> B <br/>
/// A -- 2 --> C <br/>
/// A -- 3 --> D <br/>
/// A -- else --> E
/// </para>
/// <para>
/// This is only intended to demonstrate the workflow itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class SwitchCaseEdgeExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, int>) StepA).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<int, string>) StepB).BindAsExecutor("StepBExecutor");
        var stepCExecutor = ((Func<int, string>) StepC).BindAsExecutor("StepCExecutor");
        var stepDExecutor = ((Func<int, string>) StepD).BindAsExecutor("StepDExecutor");
        var stepEExecutor = ((Func<int, string>) StepE).BindAsExecutor("StepEExecutor");

        Func<int, bool> conditionAToB = result => result == 1;
        Func<int, bool> conditionAToC = result => result == 2;
        Func<int, bool> conditionAToD = result => result == 3; 

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);

        workflowBuilder.AddSwitch(stepAExecutor, 
                                  switchBuilder => switchBuilder.AddCase(conditionAToB, stepBExecutor)
                                                                .AddCase(conditionAToC, stepCExecutor)
                                                                .AddCase(conditionAToD, stepDExecutor)
                                                                .WithDefault(stepEExecutor));

        var workflow1 = workflowBuilder.Build();
        var workflow2 = workflowBuilder.Build();
        var workflow3 = workflowBuilder.Build();
        var workflow4 = workflowBuilder.Build();

        await using var run1 = await InProcessExecution.RunAsync(workflow1, "START 1");

        var lastEvent1 = run1.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent1.Data}");

        Console.WriteLine();
        
        await using var run2 = await InProcessExecution.RunAsync(workflow2, "START 2");

        var lastEvent2 = run2.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent2.Data}");

        Console.WriteLine();
        
        await using var run3 = await InProcessExecution.RunAsync(workflow3, "START 3");

        var lastEvent3 = run3.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent3.Data}");

        Console.WriteLine();
        
        await using var run4 = await InProcessExecution.RunAsync(workflow4, "START 4");

        var lastEvent4 = run4.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent4.Data}");
    }

    private static int StepA(string input)
    {
        Console.WriteLine($"STEP A - Input: {input}");

        return int.Parse(input[^1].ToString());
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

    private static string StepD(int input)
    {
        Console.WriteLine($"STEP D - Input: {input}");

        return "END D";
    }

    private static string StepE(int input)
    {
        Console.WriteLine($"STEP E - Input: {input}");

        return "END E";
    }
}
