namespace MicrosoftSemanticKernel.Examples.Embeddings;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010

/// <summary>
/// Demonstrates populating an in-memory vector store with a set of objects with embeddings, and performing a search using text.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleCategory(Category.VectorSearch)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleCostEstimate(0.003)]
public class InMemoryVectorSearchExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIEmbeddingGenerator(project.DeployedModels.TextEmbedding3Small, project.OpenAIEndpoint, project.ApiKey)
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

        var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        var vectorStore = kernel.GetRequiredService<InMemoryVectorStore>();
        var collection = vectorStore.GetCollection<string, NewsHeadline>("headlines", definition);

        await collection.EnsureCollectionExistsAsync();

        var headlines = await NewsHeadlinesJsonReader.ReadAsync(100);

        foreach (var headline in headlines)
        {
            // Generate Embedding
            var embedding = await embeddingGenerator.GenerateAsync(headline.ShortDescription);

            headline.ShortDescriptionEmbedding = embedding.Vector.ToArray();

            // Store Embedding
            await collection.UpsertAsync(headline);
        }

        // Search

        Console.WriteTitle("Weather News (vector search) ...");
        Console.WriteLine();

        const string prompt = "Extreme weather conditions";
        var promptVector = await embeddingGenerator.GenerateAsync(prompt);

        var vectorSearchResults = collection.SearchAsync(promptVector, 10);

        await foreach (var result in vectorSearchResults)
        {
            Console.WriteLine($"{result.Record.Headline}");
            Console.WriteLine($"{result.Record.ShortDescription}");
            Console.WriteLine($"{result.Score}");
            Console.WriteLine();
        }
    }
}

#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0001
