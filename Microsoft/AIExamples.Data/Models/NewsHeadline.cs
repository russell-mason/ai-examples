namespace AIExamples.Data.Models;

public class NewsHeadline
{
    [JsonPropertyName("id")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("headline")]
    public required string Headline { get; set; }

    [JsonPropertyName("link")]
    public required string Link { get; set; }

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("short_description")]
    public required string ShortDescription { get; set; }

    [JsonPropertyName("authors")]
    public required string Authors { get; set; }

    [JsonPropertyName("date")]
    public required DateTime Date { get; set; }

    [JsonPropertyName("embedding")]
    public float[] ShortDescriptionEmbedding { get; set; } = [];
}
