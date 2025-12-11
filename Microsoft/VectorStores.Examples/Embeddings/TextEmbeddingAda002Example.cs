namespace VectorData.Examples.Embeddings;

/// <summary>
/// Demonstrates using a text-embedding-ada-002 embedding client to take text and turn it into a vector/embedding 
/// capable for use with a vector store. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbeddingAda002)]
[ExampleCostEstimate(0.001)]
public class TextEmbeddingAda002Example(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var embeddingClient = openAIClient.GetEmbeddingClient(project.DeployedModels.TextEmbeddingAda002);

        const string sentence = "The quick brown fox jumps over the lazy dog";

        var result = await embeddingClient.GenerateEmbeddingAsync(sentence);
        var embedding = result.Value.ToFloats().ToArray();

        Console.WriteLine($"Embedding for {sentence}:");
        Console.Write(string.Join(", ", embedding));
        Console.WriteLine();
    }
}
