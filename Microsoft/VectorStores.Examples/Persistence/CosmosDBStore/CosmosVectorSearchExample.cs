namespace VectorData.Examples.Persistence.CosmosDBStore;

// This example assumes that "Vector Search for NoSQL API" has been enabled
// https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/vector-search

/// <summary>
/// Demonstrates creating an embedding from search text, then running a vector search over a set of embedding previously
/// stored in the Cosmos DB vector store. The search results represent the most similar matches, and include the original 
/// typed objects.  
/// N.B. This relies on data previously stored via CosmosStoreNewsHeadlinesEmbeddingsExample.
/// </summary>
public class CosmosVectorSearchExample(AzureAIFoundrySettings azureSettings, CosmosDBSettings cosmosSettings) : IExample
{
    public async Task ExecuteAsync()
    {
        // Create clients

        var project = azureSettings.Projects.Default;

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var embeddingClient = openAIClient.GetEmbeddingClient(project.DeployedModels.TextEmbedding3Small);
        var embeddingOptions = new EmbeddingGenerationOptions { Dimensions = 512 };

        var cosmosOptions = new CosmosClientOptions
                            {
                                UseSystemTextJsonSerializerWithOptions = new JsonSerializerOptions
                                                                         {
                                                                             DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                                         }
                            };

        using var cosmosClient = new CosmosClient(cosmosSettings.PrimaryConnectionString, cosmosOptions);

        const string prompt = "Provide headlines relating to extreme weather conditions";

        var searchEmbeddingResult = await embeddingClient.GenerateEmbeddingAsync(prompt, embeddingOptions);
        var searchEmbedding = searchEmbeddingResult.Value.ToFloats().ToArray();

        var container = cosmosClient.GetContainer(cosmosSettings.DatabaseId, cosmosSettings.NewsContainerId);

        var queryDefinition = new QueryDefinition(
                """
                SELECT TOP 10 c.id, c.headline, c.link, c.category, c.short_description, c.authors, c.date, 
                              VectorDistance(c.embedding, @embedding) AS similarity_score
                FROM c
                ORDER BY VectorDistance(c.embedding, @embedding)
                """)
            .WithParameter("@embedding", searchEmbedding);

        var feed = container.GetItemQueryIterator<NewsHeadlineSearchResult>(queryDefinition);

        while (feed.HasMoreResults)
        {
            foreach (var item in await feed.ReadNextAsync())
            {
                Console.WriteLine(item.Headline);
                Console.WriteLine(item.Link);
                Console.WriteLine(item.ShortDescription);
                Console.WriteLine($"Score: {item.SimilarityScore}");
                Console.WriteLine();
            }
        }
    }
}
