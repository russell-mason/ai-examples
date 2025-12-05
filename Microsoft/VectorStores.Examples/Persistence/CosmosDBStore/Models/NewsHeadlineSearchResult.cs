namespace VectorData.Examples.Persistence.CosmosDBStore.Models;

public class NewsHeadlineSearchResult : NewsHeadline
{
    [JsonPropertyName("similarity_score")]
    public float SimilarityScore { get; set; }
}
