namespace MicrosoftSemanticKernel.Examples.ModelProviders;

// LM Studio
// https://lmstudio.ai/
// load microsoft/phi-4
// Note the /v1 in addition to the URL specified in the UI: http://localhost:1234/v1 ... [LM STUDIO SERVER] ->	GET  http://localhost:1234/v1/models

/// <summary>
/// Demonstrates using the LM Studio application running locally with a microsoft/phi-4 model via the OpenAI chat client.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.LocalLMStudio, AIModel.MSPhi4)]
[ExampleCostEstimate(0.00)]
public class LMStudioLocalPhiExample(LMStudioAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var kernel = Kernel.CreateBuilder()
                           .AddOpenAIChatCompletion(settings.Phi4ModelId, new Uri(settings.Endpoint), null)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt);

        Console.WriteLine(response.Content);
    }
}
