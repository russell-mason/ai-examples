using BERTTokenizers;

namespace VectorData.Examples.Tokenizers;

/// <summary>
/// Demonstrates turning text into a series of tokens using the BERT tokenizer.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.BERT)]
[ExampleCostEstimate(0.00)]
public class BertTokenizerExample() : IExample
{
    public async Task ExecuteAsync()
    {
        var tokenizer = new BertBaseTokenizer();

        const string sentence = "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog.";

        var tokenList = tokenizer.Tokenize(sentence);

        var tokenListX = tokenizer.Encode(50, sentence);

        var tokenIds = tokenList.Select(token => token.VocabularyIndex).ToList();
        var tokens = tokenList.Select(token => token.Token).ToList();
        var uniqueTokens = tokens.Distinct().ToList();
        var fromTokenIds = string.Concat(tokenIds.Select(id => tokenizer.IdToToken(id)));

        Console.WriteLine($"Text: {sentence}");
        Console.WriteLine($"Token IDs: {string.Join(", ", tokenIds)}");
        Console.WriteLine($"Number of tokens: {tokens.Count}");
        Console.WriteLine($"Number of unique tokens: {uniqueTokens.Count}");
        Console.WriteLine("Unique Tokens: ");

        foreach (var tokenId in tokenIds)
        {
            Console.WriteLine($"[{tokenizer.IdToToken(tokenId)}]");
        }

        Console.WriteLine($"Reconstructed (space ignorant): {fromTokenIds}");

        Console.WriteLine();

        await Task.CompletedTask; // Satisfy interface - nothing awaited
    }
}
