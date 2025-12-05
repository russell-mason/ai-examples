namespace VectorData.Examples.Persistence.CosmosDBStore;

/// <summary>
/// Demonstrates generating a couple of embeddings from text, storing them, then retrieving them again. Ensures simple 
/// save/load is working correctly. 
/// </summary>
public class CosmosStoreAndRetrieveTestEmbeddingsExample(AzureAIFoundrySettings azureSettings, CosmosDBSettings cosmosSettings) : IExample
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

        Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosSettings.DatabaseId);
        Container container = await database.CreateContainerIfNotExistsAsync(cosmosSettings.TestContainerId, cosmosSettings.PartitionKeyPath);

        // Generate Embeddings

        const string sentenceId = "CF3336ED-F1B2-4314-8743-3D78092C099B";
        const string sentenceCategory = "Sentence";
        const string sentenceText = "The quick brown fox jumps over the lazy dog";

        var sentenceResult = await embeddingClient.GenerateEmbeddingAsync(sentenceText, embeddingOptions);
        var sentenceEmbedding = sentenceResult.Value.ToFloats();

        var sentenceItem = new TextEmbedding
                           {
                               Id = sentenceId,
                               Category = sentenceCategory,
                               Text = sentenceText,
                               Embedding = sentenceEmbedding.ToArray()
                           };

        const string nameId = "EED8AD26-1084-40BC-B4F3-8091E9CBC101";
        const string nameCategory = "Name";
        const string nameText = "Ronald Aristotle Braithwaite";

        var nameResult = await embeddingClient.GenerateEmbeddingAsync(nameText, embeddingOptions);
        var nameEmbedding = nameResult.Value.ToFloats();

        var nameItem = new TextEmbedding
                       {
                           Id = nameId,
                           Category = nameCategory,
                           Text = nameText,
                           Embedding = nameEmbedding.ToArray()
                       };

        // Store Embedding

        var upsertSentence = await container.UpsertItemAsync(sentenceItem);
        var upsertName = await container.UpsertItemAsync(nameItem);

        Console.WriteLine($"Upsert Status: {upsertSentence.StatusCode}");
        Console.WriteLine($"Upsert Status: {upsertName.StatusCode}");

        // Retrieve Embedding

        var readSentence = await container.ReadItemAsync<TextEmbedding>(sentenceId, new PartitionKey(sentenceCategory));
        var readName = await container.ReadItemAsync<TextEmbedding>(nameId, new PartitionKey(nameCategory));

        Console.WriteLine($"Read Status: {readSentence.StatusCode}");
        Console.WriteLine($"Text: {readSentence.Resource.Text}");

        Console.WriteLine($"Read Status: {readName.StatusCode}");
        Console.WriteLine($"Text: {readName.Resource.Text}");
    }
}
