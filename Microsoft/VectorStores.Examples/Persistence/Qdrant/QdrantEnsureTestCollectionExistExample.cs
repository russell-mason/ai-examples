namespace VectorData.Examples.Persistence.Qdrant;

/// <summary>
/// Demonstrates minimal code to create a collection. Ensures the connection is correct and working.
/// </summary>
public class QdrantEnsureTestCollectionExistExample(QdrantSettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var collectionName = settings.TestCollectionName;
        
        using var client = new QdrantClient(settings.Host, settings.Port);

        var collectionExists = await client.CollectionExistsAsync(collectionName);

        if (!collectionExists)
        {
            await client.CreateCollectionAsync(collectionName);
        }

        var collectionInfo = await client.GetCollectionInfoAsync(collectionName);

        Console.WriteLine($"Collection: {collectionName} ({(collectionExists ? "existed" : "created")})");
        Console.WriteLine($"Status: {collectionInfo.Status}");
    }
}
