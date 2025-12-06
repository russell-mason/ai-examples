namespace VectorData.Examples.Persistence.Qdrant;

/// <summary>
/// Demonstrates taking a set of news headlines from a JSON file, creating typed objects, adding embeddings, and saving 
/// them to the Qdrant vector store. This provides permanent storage. This is intended to be the first part of an 
/// example that can then be used to perform vector searches against existing data.  
/// N.B. Performing a vector search against this data is done in QdrantVectorSearchExample.
/// </summary>
public class QdrantStoreNewsHeadlinesEmbeddingsExample(AzureAIFoundrySettings azureSettings, QdrantSettings qdrantSettings) : IExample
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

        if (collectionExists)
        {
            await client.DeleteCollectionAsync(collectionName); // Delete and rebuild
        }

        var paramsMap = new VectorParamsMap
                        {
                            Map =
                            {
                                ["text_embedding"] = new VectorParams { Size = 512, Distance = Distance.Cosine }
                            }
                        };

        await client.CreateCollectionAsync(collectionName, paramsMap);

        var headlines = await NewsHeadlinesJsonReader.ReadAsync(100);

        var points = await Task.WhenAll(headlines.Select(async headline =>
        {
            var embeddingResult = await embeddingClient.GenerateEmbeddingAsync(headline.ShortDescription, embeddingOptions);
            var embedding = embeddingResult.Value.ToFloats().ToArray();

            return CreatePoint(headline, embedding);
        }));

        await client.UpsertAsync(qdrantSettings.NewsCollectionName, points);

        var collectionInfo = await client.GetCollectionInfoAsync(collectionName);
        var collectionCount = await client.CountAsync(collectionName);

        Console.WriteLine($"{collectionCount} embeddings stored.");
        Console.WriteLine($"Status: {collectionInfo.Status}");
        Console.WriteLine();
    }

    private static PointStruct CreatePoint(NewsHeadline headline, float[] embedding) =>
        new()
        {
            Id = Guid.NewGuid(),
            Vectors = new Dictionary<string, Vector>
                      {
                          ["text_embedding"] = embedding
                      },
            Payload =
            {
                ["slug"] = headline.Slug,
                ["headline"] = headline.Headline,
                ["link"] = headline.Link,
                ["category"] = headline.Category,
                ["short_description"] = headline.ShortDescription,
                ["authors"] = headline.Authors,
                ["date"] = headline.Date.ToString("yyyy-MM-dd")
            }
        };
}
