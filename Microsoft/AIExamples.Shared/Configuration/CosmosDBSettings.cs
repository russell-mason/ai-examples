namespace AIExamples.Shared.Configuration;

public class CosmosDBSettings
{
    public string PrimaryConnectionString { get; set; } = string.Empty;

    public string DatabaseId { get; set; } = string.Empty;

    public string TestContainerId { get; set; } = string.Empty;

    public string NewsContainerId { get; set; } = string.Empty;

    public string PartitionKeyPath { get; set; } = string.Empty;
}
