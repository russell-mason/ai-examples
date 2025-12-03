namespace MicrosoftSemanticKernel.Examples.Embeddings;

#pragma warning disable SKEXP0010
#pragma warning disable CS0618

/// <summary>
/// Demonstrates how to create an embedding from text. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleCostEstimate(0.001)]
public class EmbeddingGeneratorExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAITextEmbeddingGeneration(project.DeployedModels.TextEmbedding3Small, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        const string sentence = "The quick brown fox jumps over the lazy dog";

        var embeddingResult = await embeddingService.GenerateEmbeddingAsync(sentence);
        var embedding = embeddingResult.ToArray();

        Console.WriteLine($"Embedding for {sentence}:");
        Console.Write(string.Join(", ", embedding));
        Console.WriteLine();
    }
}

#pragma warning restore CS0618
#pragma warning restore SKEXP0010
