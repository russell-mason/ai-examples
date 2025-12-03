namespace MicrosoftSemanticKernel.Examples.ModelProviders;

// Ollama
// https://ollama.com/download
// ollama pull phi3:mini
// ollama run phi3:mini

/// <summary>
/// Demonstrates using the Ollama application running locally with a phi3:mini model via a standalone instance of the Ollama 
/// chat client, i.e. without using the kernal. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.LocalOllama, AIModel.MSPhi3Mini)]
[ExampleCostEstimate(0.00)]
public class OllamaStandaloneLocalPhiExample(OllamaAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        // Using Standalone Instance rather than kernel
        var chatCompletionService = new OllamaApiClient(settings.Endpoint, settings.ModelId).AsChatCompletionService();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt);

        Console.WriteLine(response.Content);
    }
}
