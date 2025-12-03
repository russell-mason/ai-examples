namespace MicrosoftSemanticKernel.Examples.Embeddings;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable CS0618

[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleCategory(Category.VectorSearch)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleCostEstimate(0.003)]
public class InMemoryTextSearchExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAITextEmbeddingGeneration(project.DeployedModels.TextEmbedding3Small, project.OpenAIEndpoint, project.ApiKey)
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        builder.Services.AddInMemoryVectorStore();

        var kernel = builder.Build();

        var definition = new VectorStoreCollectionDefinition
                         {
                             Properties = new List<VectorStoreProperty>
                                          {
                                              new VectorStoreKeyProperty("Slug", typeof(string)),
                                              new VectorStoreDataProperty("Headline", typeof(string)) { IsIndexed = true },
                                              new VectorStoreDataProperty("ShortDescription", typeof(string)) { IsFullTextIndexed = true },
                                              new VectorStoreVectorProperty("ShortDescriptionEmbedding", typeof(float[]), dimensions: 1536)
                                              {
                                                  DistanceFunction = DistanceFunction.CosineSimilarity,
                                                  IndexKind = IndexKind.Hnsw
                                              }
                                          }
                         };

        var embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        var vectorStore = kernel.GetRequiredService<InMemoryVectorStore>();
        var collection = vectorStore.GetCollection<string, NewsHeadline>("headlines", definition);

        await collection.EnsureCollectionExistsAsync();

        var headlines = await NewsHeadlinesJsonReader.ReadAsync(100);

        foreach (var headline in headlines)
        {
            // Generate Embedding
            var embeddingResult = await embeddingService.GenerateEmbeddingAsync(headline.ShortDescription);
            var embedding = embeddingResult.ToArray();

            headline.ShortDescriptionEmbedding = embedding;

            // Store Embedding
            await collection.UpsertAsync(headline);
        }

        // Search

        Console.WriteTitle("Sporting News (text search) ...");
        Console.WriteLine();

        var stringMapper = new TextSearchStringMapper();
        var resultMapper = new TextSearchResultMapper();
        var vectorTextSearch = new VectorStoreTextSearch<NewsHeadline>(collection, embeddingService, stringMapper, resultMapper);
        var vectorResults = await vectorTextSearch.GetTextSearchResultsAsync("Give me some sporting news headlines");

        await foreach (var result in vectorResults.Results)
        {
            Console.WriteLine(result.Name);
            Console.WriteLine(result.Value);
            Console.WriteLine(result.Link);
            Console.WriteLine();
        }
    }
}

public class TextSearchStringMapper : ITextSearchStringMapper
{
    public string MapFromResultToString(object result) =>
        result is NewsHeadline headline
            ? headline.ShortDescription
            : throw new ArgumentException("Invalid result type.");
}

public class TextSearchResultMapper : ITextSearchResultMapper
{
    public TextSearchResult MapFromResultToTextSearchResult(object result) =>
        result is NewsHeadline headline
            ? new TextSearchResult(headline.ShortDescription) { Name = headline.Headline, Link = headline.Link }
            : throw new ArgumentException("Invalid result type.");
}

#pragma warning restore CS0618
#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0001
