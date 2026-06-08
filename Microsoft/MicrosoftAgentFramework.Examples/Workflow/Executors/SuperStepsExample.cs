namespace MicrosoftAgentFramework.Examples.Workflow.Executors;

/// <summary>
/// Demonstrates that a super step encapsulates several steps when using a fan-out edge. 
/// <para>
/// A --> B <br/>
/// A --> C <br/>
/// A --> D <br/>
/// B --> E <br/>
/// C --> F <br/>
/// D --> G
/// </para>
/// <para>
/// This is only intended to demonstrate the workflow itself so does not include the use of any agents.
/// </para>
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.Workflow)]
[ExampleCostEstimate(0)]
public class SuperStepsExample : IExample
{
    public async Task ExecuteAsync()
    {
        var stepAExecutor = ((Func<string, string>) Step).BindAsExecutor("StepAExecutor");
        var stepBExecutor = ((Func<string, ValueTask<string>>) DelayedStep10).BindAsExecutor("StepBExecutor");
        var stepCExecutor = ((Func<string, ValueTask<string>>) DelayedStep1000).BindAsExecutor("StepCExecutor");
        var stepDExecutor = ((Func<string, ValueTask<string>>) DelayedStep2000).BindAsExecutor("StepDExecutor");
        var stepEExecutor = ((Func<string, string>) Step).BindAsExecutor("StepEExecutor");
        var stepFExecutor = ((Func<string, string>) Step).BindAsExecutor("StepFExecutor");
        var stepGExecutor = ((Func<string, string>) Step).BindAsExecutor("StepGExecutor");

        var workflowBuilder = new WorkflowBuilder(stepAExecutor);
        workflowBuilder.AddFanOutEdge(stepAExecutor, [stepBExecutor, stepCExecutor, stepDExecutor]);
        workflowBuilder.AddEdge(stepBExecutor, stepEExecutor);
        workflowBuilder.AddEdge(stepCExecutor, stepFExecutor);
        workflowBuilder.AddEdge(stepDExecutor, stepGExecutor);

        var workflow = workflowBuilder.Build();

        await using var run = await InProcessExecution.RunStreamingAsync(workflow, "START");
        
        await foreach (var @event in run.WatchStreamAsync())
        {
            switch (@event)
            {
                case SuperStepStartedEvent:
                    Console.WriteLine($"Super Step Started - {DateTime.UtcNow}");
                    break;

                case SuperStepCompletedEvent:
                    Console.WriteLine($"Super Step Completed - {DateTime.UtcNow}");
                    break;
            }
        }
    }

    private static string Step(string input)
    {
        Console.WriteLine($"STEP - {DateTime.UtcNow}");

        return input + " ... ";
    }

    private static async ValueTask<string> DelayedStep10(string input)
    {
        Console.WriteLine($"STEP 10 - {DateTime.UtcNow}");

        await Task.Delay(10);

        return input + " ... ";
    }

    private static async ValueTask<string> DelayedStep1000(string input)
    {
        Console.WriteLine($"STEP 1000 - {DateTime.UtcNow}");

        await Task.Delay(1000);

        return input + " ... ";
    }

    private static async ValueTask<string> DelayedStep2000(string input)
    {
        Console.WriteLine($"STEP 2000 - {DateTime.UtcNow}");

        await Task.Delay(2000);

        return input + " ... ";
    }
}
