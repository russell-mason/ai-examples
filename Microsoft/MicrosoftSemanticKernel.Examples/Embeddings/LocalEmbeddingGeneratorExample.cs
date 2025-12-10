namespace MicrosoftSemanticKernel.Examples.Embeddings;

#pragma warning disable SKEXP0010

// LM Studio
// https://lmstudio.ai/
// load text-embedding-e5-base-v2

/// <summary>
/// Demonstrates how to create an embedding from text using an LM Studio hosted model. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.LocalLMStudio, AIModel.TextEmbeddingE5BaseV2)]
[ExampleCostEstimate(0.00)]
public class LocalEmbeddingGeneratorExample(LMStudioAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var openAiClient = new OpenAIClient(new ApiKeyCredential("NOT_APPLICABLE"), 
                                            new OpenAIClientOptions { Endpoint = new Uri(settings.Endpoint) });

        var kernel = Kernel.CreateBuilder()
                           .AddOpenAIEmbeddingGenerator(settings.E5BaseV2ModelId, openAiClient)
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
