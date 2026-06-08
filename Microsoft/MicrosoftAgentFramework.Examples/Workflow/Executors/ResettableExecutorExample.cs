namespace MicrosoftAgentFramework.Examples.Workflow.Executors;

/// <summary>
/// Demonstrates a simple workflow using a class based executor that can be reset to clear internal state. 
/// <para>
/// This is only intended to demonstrate the executor itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class ResettableExecutorExample : IExample
{
    public async Task ExecuteAsync()
    {
        Console.WriteTitle("Without IResettableExecutor");

        var nonResettableExecutor = new NonResettableExecutorClass();
        var workflowBuilder1 = new WorkflowBuilder(nonResettableExecutor);

        await RunWorkflow(workflowBuilder1, nonResettableExecutor, "START 1");
        await RunWorkflow(workflowBuilder1, nonResettableExecutor, "START 2");
        await RunWorkflow(workflowBuilder1, nonResettableExecutor, "START 3");
        await RunWorkflow(workflowBuilder1, nonResettableExecutor, "START 4");

        Console.WriteTitle("With IResettableExecutor");

        var resettableExecutor = new ResettableExecutorClass();
        var workflowBuilder2 = new WorkflowBuilder(resettableExecutor);

        await RunWorkflow(workflowBuilder2, resettableExecutor, "START 1");
        await RunWorkflow(workflowBuilder2, resettableExecutor, "START 2");
        await RunWorkflow(workflowBuilder2, resettableExecutor, "START 3");
        await RunWorkflow(workflowBuilder2, resettableExecutor, "START 4");
    }

    private static async Task RunWorkflow(WorkflowBuilder workflowBuilder, Executor executor, string prompt)
    {
        await using (await InProcessExecution.RunAsync(workflowBuilder.Build(), prompt))
        {
            if (executor is IStateful stateful)
            {
                Console.WriteLine($"State: {string.Join(", ", stateful.State)}");
            }
        }

        Console.WriteLine();
    }
}

public interface IStateful
{
    List<string> State { get; }
}

public partial class NonResettableExecutorClass() : Executor("NonResettableExecutorClass"), IStateful   
{
    public List<string> State { get; } = [];

    [MessageHandler]
    private ValueTask HandleAsync(string input, IWorkflowContext context)
    {
        State.Add(input);

        Console.WriteLine($"Step 1 - Input: {input}");

        return ValueTask.CompletedTask;
    }
}

public partial class ResettableExecutorClass() : Executor("ResettableExecutorClass"), IStateful, IResettableExecutor
{
    public List<string> State { get; } = [];

    [MessageHandler]
    private ValueTask HandleAsync(string input, IWorkflowContext context)
    {
        State.Add(input);

        Console.WriteLine($"Step 1 - Input: {input}");

        return ValueTask.CompletedTask;
    }

    public ValueTask ResetAsync()
    {
        Console.WriteLine("Reset - Clear List");

        State.Clear();

        return ValueTask.CompletedTask;
    }
}
