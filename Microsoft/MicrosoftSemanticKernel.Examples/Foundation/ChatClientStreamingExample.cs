namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates getting a streamed response from a chat client, and outputting response tokens as they're received.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatClientStreamingExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatClient(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatClient>();

        const string prompt = "What is a large language model?";

        var response = chatCompletionService.GetStreamingResponseAsync(prompt);

        await foreach (var content in response)
        {
            Console.Write(content);
        }

        Console.WriteLine();
    }
}
