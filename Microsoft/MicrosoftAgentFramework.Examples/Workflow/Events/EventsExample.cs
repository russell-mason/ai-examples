namespace MicrosoftAgentFramework.Examples.Workflow.Events;

/// <summary>
/// Demonstrates the basic events raised during a simple workflow run. 
/// <para>
/// This is only intended to demonstrate the executor itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class EventsExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, IWorkflowContext, ValueTask<string>>) StepA).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<string, string>) StepB).BindAsExecutor("StepBExecutor");
        var stepCExecutor = ((Func<string, string>) StepC).BindAsExecutor("StepCExecutor");

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);
        workflowBuilder.AddEdge(stepAExecutor, stepBExecutor);
        workflowBuilder.AddEdge(stepBExecutor, stepCExecutor);
        workflowBuilder.WithOutputFrom(stepAExecutor);

        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunStreamingAsync(workflow, "START");

        await foreach (var @event in run.WatchStreamAsync())
        {
            switch (@event)
            {
                case WorkflowStartedEvent start:
                    Console.WriteLine($"Workflow Started: {start.Data}");
                    break;

                case WorkflowOutputEvent output:
                    Console.WriteLine($"Workflow Output: {output.ExecutorId} - {output.Data}");
                    break;

                case SuperStepStartedEvent superStepStarted:
                    Console.WriteLine($"Super Step Started - Step : {superStepStarted.StepNumber}");
                    break;

                case SuperStepCompletedEvent superStepCompleted:
                    Console.WriteLine($"Super Step Completed - Step : {superStepCompleted.StepNumber}");
                    break;

                case ExecutorInvokedEvent invoke:
                    Console.WriteLine($"Executor Invoked: {invoke.ExecutorId}");
                    break;

                case ExecutorCompletedEvent complete:
                    Console.WriteLine($"Executor Completed: {complete.ExecutorId} - {complete.Data}");
                    break;
            }
        }

        Console.WriteLine("Workflow Completed");
    }

    private static async ValueTask<string> StepA(string input, IWorkflowContext context)
    {
        Console.WriteLine($"STEP A - Input: {input}");

        await context.YieldOutputAsync("STEP A - OUTPUT");

        return "END A";
    }

    private static string StepB(string input)
    {
        Console.WriteLine($"STEP B - Input: {input}");

        return "END B";
    }

    private static string StepC(string input)
    {
        Console.WriteLine($"STEP C - Input: {input}");

        return "END C";
    }
}
