namespace VectorData.Examples.Embeddings;

/// <summary>
/// Demonstrates using a text-embedding-3-small embedding client to take text and turn it into a vector/embedding 
/// capable for use with a vector store.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleCostEstimate(0.01)]
public class TextEmbedding3SmallExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var embeddingClient = openAIClient.GetEmbeddingClient(project.DeployedModels.TextEmbedding3Small);
        var options = new EmbeddingGenerationOptions { Dimensions = 512 };

        const string sentence = "The quick brown fox jumps over the lazy dog";

        var result = await embeddingClient.GenerateEmbeddingAsync(sentence, options);

        var embedding = result.Value.ToFloats();

        Console.WriteLine($"Embedding for {sentence}:");
        Console.Write(string.Join(", ", embedding.ToArray()));
        Console.WriteLine();
    }
}
