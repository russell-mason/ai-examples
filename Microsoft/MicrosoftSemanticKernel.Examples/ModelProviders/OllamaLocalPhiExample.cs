namespace MicrosoftSemanticKernel.Examples.ModelProviders;

// Ollama
// https://ollama.com/download
// ollama pull phi3:mini
// ollama run phi3:mini

/// <summary>
/// Demonstrates using the Ollama application running locally with a phi3:mini model via the Ollama chat client.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.LocalOllama, AIModel.MSPhi3Mini)]
[ExampleCostEstimate(0.00)]
public class OllamaLocalPhiExample(OllamaAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var kernel = Kernel.CreateBuilder()
                           .AddOllamaChatCompletion(settings.ModelId, new Uri(settings.Endpoint))
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt);

        Console.WriteLine(response.Content);
    }
}
