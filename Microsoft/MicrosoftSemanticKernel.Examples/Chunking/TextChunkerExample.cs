namespace MicrosoftSemanticKernel.Examples.Chunking;

#pragma warning disable SKEXP0050

/// <summary>
/// Demonstrates using a text chunker to create embeddings for paragraphs in a text file.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.00)]
public class TextChunkerExample() : IExample
{
    public async Task ExecuteAsync()
    {
        var textFileInfo = await TextFileReader.ReadAsync(@".\SourceText\TheRedHeadedLeague.txt");

        var chunks = TextChunker.SplitPlainTextParagraphs(textFileInfo.GetTextAsLines(),  512);

        Console.WriteLine($"Chunks: {chunks.Count}");
        Console.WriteLine($"Chunks Total Length: {chunks.Sum(chunk => chunk.Length)}");
        Console.WriteLine();
        Console.WriteLine($"Line Count: {textFileInfo.LineCount}");
        Console.WriteLine($"Blank Line Count: {textFileInfo.BlankLineCount}");
        Console.WriteLine($"Paragraph Count: {textFileInfo.ParagraphCount}");
        Console.WriteLine($"Word Count: {textFileInfo.WordCount}");
        Console.WriteLine($"Character Count: {textFileInfo.CharacterCount}");

        for (var index = 0; index < chunks.Count; index++)
        {
            Console.WriteTitle((index + 1).ToString());
            Console.WriteLine(chunks[index]);
        }
    }
}

#pragma warning restore SKEXP0050
