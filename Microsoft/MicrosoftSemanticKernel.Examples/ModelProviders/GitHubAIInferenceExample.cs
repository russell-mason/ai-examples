namespace MicrosoftSemanticKernel.Examples.ModelProviders;

/// <summary>
/// Demonstrates using a meta/Meta-Llama-3.1-405B-Instruct model via GitHub and the Azure AI Inference chat client.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.GitHub, AIModel.MetaLlama3dot1)]
[ExampleCostEstimate(0.00)]
public class GitHubAIInferenceExample(GitHubAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var kernel = Kernel.CreateBuilder()
                           .AddAzureAIInferenceChatCompletion(settings.ModelId, settings.ApiKey, new Uri(settings.Endpoint)) // Inference not OpenAI
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt);

        Console.WriteLine(response.Content);
    }
}
