namespace VectorData.Examples.Persistence.Qdrant;

/// <summary>
/// Demonstrates creating an embedding from search text, then running a vector search over a set of embedding previously
/// stored in the Qdrant vector store. The search results represent the most similar matches, and include the original 
/// typed objects.  
/// N.B. This relies on data previously stored via QdrantStoreNewsHeadlinesEmbeddingsExample.
/// </summary>
public class QdrantVectorSearchExample(AzureAIFoundrySettings azureSettings, QdrantSettings qdrantSettings) : IExample
{
    public async Task ExecuteAsync()
    {
        // Create clients

        var project = azureSettings.Projects.Default;
        var collectionName = qdrantSettings.NewsCollectionName;

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var embeddingClient = openAIClient.GetEmbeddingClient(project.DeployedModels.TextEmbedding3Small);
        var embeddingOptions = new EmbeddingGenerationOptions { Dimensions = 512 };

        using var client = new QdrantClient(qdrantSettings.Host, qdrantSettings.Port);

        var collectionExists = await client.CollectionExistsAsync(collectionName);

        if (!collectionExists)
        {
            throw new InvalidOperationException("No Qdrant collection. Please ensure QdrantStoreNewsHeadlinesEmbeddingsExample has been executed.");
        }

        const string prompt = "Provide headlines relating to extreme weather conditions";

        var searchEmbeddingResult = await embeddingClient.GenerateEmbeddingAsync(prompt, embeddingOptions);
        var searchVector = searchEmbeddingResult.Value.ToFloats().ToArray();

        var points = await client.SearchAsync(collectionName, searchVector, vectorName: "text_embedding", limit: 10);

        foreach (var point in points)
        {
            var headline = CreateNewsHeadline(point);

            Console.WriteLine(headline.Headline);
            Console.WriteLine(headline.Link);
            Console.WriteLine(headline.ShortDescription);
            Console.WriteLine($"Score: {point.Score}");
            Console.WriteLine();
        }
    }

    private static NewsHeadline CreateNewsHeadline(ScoredPoint point) =>
        new()
        {
            Slug = point.Payload["slug"].StringValue,
            Headline = point.Payload["headline"].StringValue,
            Link = point.Payload["link"].StringValue,
            Category = point.Payload["category"].StringValue,
            ShortDescription = point.Payload["short_description"].StringValue,
            Authors = point.Payload["authors"].StringValue,
            Date = DateTime.Parse(point.Payload["date"].StringValue)
        };
}
