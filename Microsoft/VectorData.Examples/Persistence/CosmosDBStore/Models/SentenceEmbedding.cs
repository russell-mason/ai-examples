namespace VectorData.Examples.Persistence.CosmosDBStore.Models;

public class TextEmbedding
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }

    [JsonPropertyName("embedding")]
    public required float[] Embedding { get; set; }
}
