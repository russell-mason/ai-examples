namespace VectorData.Examples.Tokenizers;

/// <summary>
/// Demonstrates turning text into a series of tokens using the TikToken tokenizer.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.TikToken)]
[ExampleCostEstimate(0.00)]
public class TikTokenTokenizerExample() : IExample
{
    public async Task ExecuteAsync()
    {
        const string gpt4 = "cl100k_base";
        const string gpt4o = "o200k_base";

        await Tokenize(gpt4);
        await Tokenize(gpt4o);
    }

    private static async Task Tokenize(string encodingName)
    {
        var tokenizer = await TikToken.GetEncodingAsync(encodingName);

        const string sentence = "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog.";

        var tokenIds = tokenizer.Encode(sentence);
        var decoded = tokenizer.Decode(tokenIds);

        var uniqueTokenIds = tokenIds.Distinct().ToList();

        Console.WriteTitle($"{encodingName}...");
        Console.WriteLine();

        Console.WriteLine($"Text: {sentence}");
        Console.WriteLine($"Token IDs: {string.Join(", ", tokenIds)}");
        Console.WriteLine($"Number of tokens: {tokenIds.Count}");
        Console.WriteLine($"Number of unique tokens: {uniqueTokenIds.Count}");
        Console.WriteLine("Unique Tokens: ");

        foreach (var tokenId in uniqueTokenIds)
        {
            Console.WriteLine($"[{tokenizer.Decode([tokenId])}]");
        }

        Console.WriteLine($"Decoded: {decoded}");

        Console.WriteLine();
    }
}
