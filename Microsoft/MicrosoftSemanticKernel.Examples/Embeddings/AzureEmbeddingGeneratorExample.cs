namespace MicrosoftSemanticKernel.Examples.Embeddings;

#pragma warning disable SKEXP0010

/// <summary>
/// Demonstrates how to create an embedding from text using an Azure hosted model. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleCostEstimate(0.001)]
public class AzureEmbeddingGeneratorExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIEmbeddingGenerator(project.DeployedModels.TextEmbedding3Small, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

        const string sentence = "The quick brown fox jumps over the lazy dog";

        var embedding = await embeddingGenerator.GenerateAsync(sentence);

        Console.WriteLine($"Embedding for {sentence}:");
        Console.Write(string.Join(", ", embedding.Vector.ToArray().Select(value => value)));
        Console.WriteLine();
    }
}

#pragma warning restore SKEXP0010
