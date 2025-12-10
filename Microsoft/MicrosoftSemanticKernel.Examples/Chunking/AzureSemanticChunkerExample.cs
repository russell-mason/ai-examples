namespace MicrosoftSemanticKernel.Examples.Chunking;

#pragma warning disable SKEXP0010

// Uses SemanticChunker.NET
// https://github.com/GregorBiswanger/SemanticChunker.NET

/// <summary>
/// Demonstrates using a semantic chunker to create embeddings for a text file using an Azure hosted model.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.02)]
public class AzureSemanticChunkerExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIEmbeddingGenerator(project.DeployedModels.TextEmbedding3Small, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var embeddingGenerator = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        var semanticChunker = new SemanticChunker(embeddingGenerator, tokenLimit: 512, thresholdAmount: 90);

        var textFileInfo = await TextFileReader.ReadAsync(@".\SourceText\TheRedHeadedLeague.txt");

        var chunks = await semanticChunker.CreateChunksAsync(textFileInfo.Text); // Includes embeddings in the results

        Console.WriteLine($"Chunks: {chunks.Count}");
        Console.WriteLine($"Chunks Total Length: {chunks.Sum(chunk => chunk.Text.Length)}");
        Console.WriteLine();
        Console.WriteLine($"Line Count: {textFileInfo.LineCount}");
        Console.WriteLine($"Blank Line Count: {textFileInfo.BlankLineCount}");
        Console.WriteLine($"Paragraph Count: {textFileInfo.ParagraphCount}");
        Console.WriteLine($"Word Count: {textFileInfo.WordCount}");
        Console.WriteLine($"Character Count: {textFileInfo.CharacterCount}");

        foreach (var chunk in chunks)
        {
            Console.WriteTitle(chunk.Id);
            Console.WriteLine(chunk.Text);
        }
    }
}

#pragma warning restore SKEXP0010
