namespace MicrosoftAgentFramework.InteractiveChatConsoleApp.Services;

public class ChatService : BackgroundService
{
    private readonly ChatClientAgent _agent;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private AgentThread _thread;

    public ChatService(IHostApplicationLifetime hostApplicationLifetime, ChatClientAgent chatClientAgent)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _agent = chatClientAgent;
        _thread = _agent.GetNewThread();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        WriteIntroduction();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var prompt = ReadUserInput();

                if (QuitTriggered(prompt)) return;

                if (ResetTriggered(prompt)) continue;

                var response = _agent.RunStreamingAsync(prompt, _thread, cancellationToken: stoppingToken);

                await WriteResponse(response);
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }
    }

    private static string ReadUserInput()
    {
        Console.Write("> ");

        return Console.ReadLine() ?? string.Empty;
    }

    private static void WriteIntroduction()
    {
        Console.WriteTitle("Interactive Chat Example Started");
        Console.WriteLine("Type 'Reset' to clear chat history");
        Console.WriteLine("Type 'Quit' to exit");
        Console.WriteLine();
        Console.WriteLineInColor("How may I help you?", ConsoleColor.Blue);
        Console.WriteLine();
    }

    private static async Task WriteResponse(IAsyncEnumerable<AgentRunResponseUpdate> response)
    {
        Console.WriteLine();

        await foreach (var update in response) Console.WriteInColor(update.Text, ConsoleColor.Blue);

        Console.WriteLine();
        Console.WriteLine();
    }

    private static void WriteReset()
    {
        Console.WriteLine();
        Console.WriteLineInColor("The chat history has been reset", ConsoleColor.Cyan);
        Console.WriteLine();
    }

    private static void WriteError(Exception exception)
    {
        Console.WriteError("Something went wrong! Please try again.");
        Console.WriteError(exception.Message);
    }

    private static void WriteConclusion()
    {
        Console.WriteTitle("Interactive Chat Example Ended");
        Console.WriteLine();
    }

    private bool QuitTriggered(string prompt)
    {
        if (!prompt.Equals("quit", StringComparison.OrdinalIgnoreCase)) return false;

        WriteConclusion();

        _hostApplicationLifetime.StopApplication();

        return true;
    }

    private bool ResetTriggered(string prompt)
    {
        if (!prompt.Equals("reset", StringComparison.OrdinalIgnoreCase)) return false;

        _thread = _agent.GetNewThread();

        WriteReset();

        return true;
    }
}
