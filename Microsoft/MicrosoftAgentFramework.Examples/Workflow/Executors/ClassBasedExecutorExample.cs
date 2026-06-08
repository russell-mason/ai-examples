namespace MicrosoftAgentFramework.Examples.Workflow.Executors;

/// <summary>
/// Demonstrates a simple workflow using a class based executor. 
/// <para>
/// This is only intended to demonstrate the executor itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class ClassBasedExecutorExample : IExample
{
    public async Task ExecuteAsync()
    {
        var workflowBuilder = new WorkflowBuilder(new ExecutorClass());
        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunAsync(workflow, "START");

        var lastEvent = run.OutgoingEvents.OfType<ExecutorCompletedEvent>().Last();

        Console.WriteLine($"RESULT - Output: {lastEvent.Data}");
    }
}

public partial class ExecutorClass() : Executor("ExecutorClass")
{
    [MessageHandler]
    private ValueTask<string> HandleAsync(string input, IWorkflowContext context)
    {
        Console.WriteLine($"STEP 1 - Input: {input}");

        return ValueTask.FromResult("END");
    }
}
