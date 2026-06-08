namespace MicrosoftAgentFramework.Examples.Workflow.Events;

/// <summary>
/// Demonstrates raising custom events raised during a simple workflow run. 
/// <para>
/// This is only intended to demonstrate the executor itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class CustomEventsExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, IWorkflowContext, ValueTask<string>>) StepA).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<string, IWorkflowContext, ValueTask<string>>) StepB).BindAsExecutor("StepBExecutor");
        var stepCExecutor = ((Func<string, IWorkflowContext, ValueTask<string>>) StepC).BindAsExecutor("StepCExecutor");

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);
        workflowBuilder.AddEdge(stepAExecutor, stepBExecutor);
        workflowBuilder.AddEdge(stepBExecutor, stepCExecutor);

        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunStreamingAsync(workflow, "START");

        await foreach (var @event in run.WatchStreamAsync())
        {
            switch (@event)
            {
                case CustomEvent custom:
                    Console.WriteLine($"Custom Event: {custom.Data}");
                    break;

                case CustomEventWithString withString:
                    Console.WriteLine($"Custom Event With String: {withString.Data}");
                    break;

                case CustomEventWithObject withObject:
                    var obj = (CustomObject) withObject.Data;
                    Console.WriteLine($"Custom Event With Object: Id={obj.Id}, Description={obj.Description}");
                    break;
            }
        }
    }

    private static async ValueTask<string> StepA(string input, IWorkflowContext context)
    {
        Console.WriteLine("STEP A");

        await context.AddEventAsync(new CustomEvent());

        return "END A";
    }

    private static async ValueTask<string> StepB(string input, IWorkflowContext context)
    {
        Console.WriteLine("STEP B");

        await context.AddEventAsync(new CustomEventWithString("StepB Data"));

        return "END B";
    }

    private static async ValueTask<string> StepC(string input, IWorkflowContext context)
    {
        Console.WriteLine("STEP C");

        await context.AddEventAsync(new CustomEventWithObject(new CustomObject(1, "StepC Data")));

        return "END C";
    }
}

public class CustomObject(int id, string description)
{
    public int Id { get; } = id;
    public string Description { get; } = description;
}

public class CustomEvent() : WorkflowEvent() { }

public class CustomEventWithString(string data) : WorkflowEvent(data) { }

public class CustomEventWithObject(CustomObject data) : WorkflowEvent(data) { }
