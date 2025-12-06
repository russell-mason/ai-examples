namespace VectorData.Examples.Persistence.CosmosDBStore;

/// <summary>
/// Demonstrates minimal code to create a database and container. Ensures the connection is correct and working.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureCosmosDB)]
[ExampleCostEstimate(0.01, 1.20, CostVisibility.Opaque)]
public class CosmosEnsureTestDatabaseAndContainerExistExample(CosmosDBSettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        using var client = new CosmosClient(settings.PrimaryConnectionString);

        Database database = await client.CreateDatabaseIfNotExistsAsync(settings.DatabaseId);
        Container container = await database.CreateContainerIfNotExistsAsync(settings.TestContainerId, settings.PartitionKeyPath);

        Console.WriteLine($"Database: {database.Id}");
        Console.WriteLine($"Container: {container.Id}");
    }
}
