namespace VectorData.Examples.Persistence.CosmosDBStore;

// This example assumes that "Vector Search for NoSQL API" has been enabled
// https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/vector-search

/// <summary>
/// Demonstrates taking a set of news headlines from a JSON file, creating typed objects, adding embeddings, and saving 
/// them to the Cosmos DB vector store. This provides permanent storage. This is intended to be the first part of an 
/// example that can then be used to perform vector searches against existing data.  
/// N.B. Performing a vector search against this data is done in CosmosVectorSearchExample.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleResourceUse(Resource.AzureCosmosDB)]
[ExampleCostEstimate(0.01, 1.20, CostVisibility.Opaque)]
public class CosmosStoreNewsHeadlinesEmbeddingsExample(AzureAIFoundrySettings azureSettings, CosmosDBSettings cosmosSettings) : IExample
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

        Collection<Embedding> vectorEmbeddings =
        [
            new()
            {
                Path = "/embedding",
                Dimensions = 512,
                DistanceFunction = DistanceFunction.Cosine
            }
        ];

        var vectorPolicy = new VectorEmbeddingPolicy(vectorEmbeddings);

        var containerProperties = new ContainerProperties
                                  {
                                      Id = cosmosSettings.NewsContainerId,
                                      PartitionKeyPath = cosmosSettings.PartitionKeyPath,
                                      VectorEmbeddingPolicy = vectorPolicy
                                  };

        using var cosmosClient = new CosmosClient(cosmosSettings.PrimaryConnectionString, cosmosOptions);

        Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosSettings.DatabaseId);
        Container container = await database.CreateContainerIfNotExistsAsync(containerProperties);

        var headlines = await NewsHeadlinesJsonReader.ReadAsync(100);

        foreach (var headline in headlines)
        {
            // Generate Embedding
            var embeddingResult = await embeddingClient.GenerateEmbeddingAsync(headline.ShortDescription, embeddingOptions);
            var embedding = embeddingResult.Value.ToFloats().ToArray();

            headline.ShortDescriptionEmbedding = embedding;

            // Store Embedding
            await container.UpsertItemAsync(headline);
        }

        Console.WriteLine($"{headlines.Count} embeddings stored.");
        Console.WriteLine();
    }
}
