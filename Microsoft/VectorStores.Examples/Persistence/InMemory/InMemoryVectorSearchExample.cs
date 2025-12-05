namespace VectorData.Examples.Persistence.InMemory;

/// <summary>
/// Demonstrates using a simple List&lt;T&gt; to store a series of embeddings that can then be used to perform a 
/// similarity text search.  
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleCategory(Category.VectorSearch)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleCostEstimate(0.01)]
public class InMemoryVectorSearchExample(AzureAIFoundrySettings azureSettings) : IExample
{
    public async Task ExecuteAsync()
    {
        // Create clients

        var project = azureSettings.Projects.Default;

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var embeddingClient = openAIClient.GetEmbeddingClient(project.DeployedModels.TextEmbedding3Small);
        var embeddingOptions = new EmbeddingGenerationOptions { Dimensions = 512 };

        List<NewsHeadline> headlinesStore = [];

        var headlines = await NewsHeadlinesJsonReader.ReadAsync(100);

        foreach (var headline in headlines)
        {
            // Generate Embedding
            var embeddingResult = await embeddingClient.GenerateEmbeddingAsync(headline.ShortDescription, embeddingOptions);
            var embedding = embeddingResult.Value.ToFloats().ToArray();

            headline.ShortDescriptionEmbedding = embedding;

            // Store Embedding
            headlinesStore.Add(headline);
        }

        // Search

        const string prompt = "Provide headlines relating to extreme weather conditions";

        var searchEmbeddingResult = await embeddingClient.GenerateEmbeddingAsync(prompt, embeddingOptions);
        var searchEmbedding = searchEmbeddingResult.Value.ToFloats().ToArray();

        var results = headlinesStore
                      .Select(headline => new
                                          {
                                              Headline = headline,
                                              SimilarityScore = searchEmbedding.Zip(headline.ShortDescriptionEmbedding,
                                                                                    (queryVector, headlineVector) => queryVector * headlineVector)
                                                                               .Sum()
                                          })
                      .OrderByDescending(similarity => similarity.SimilarityScore)
                      .Take(5)
                      .ToList();

        foreach (var result in results)
        {
            Console.WriteLine(result.Headline.Headline);
            Console.WriteLine(result.Headline.Link);
            Console.WriteLine(result.Headline.ShortDescription);
            Console.WriteLine($"Score: {result.SimilarityScore}");
            Console.WriteLine();
        }
    }
}
