namespace AIExamples.Shared.Configuration;

public class QdrantSettings
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 0;

    public string TestCollectionName { get; set; } = string.Empty;

    public string NewsCollectionName { get; set; } = string.Empty;
}
