namespace VectorData.Examples.Embeddings;

/// <summary>
/// Demonstrates reducing 512-dimensional embeddings to 2-dimensional coordinates that can be used to produce a chart.
/// e.g. In Excel, the output of this example can be pasted as csv data, a scatter chart created, and data labels added.
/// You can then see a very simplistic mapping of the similarity between the words.
/// N.B. For visualization purposes only.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.VectorGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TextEmbedding3Small)]
[ExampleCostEstimate(0.001)]
public class PlotEmbeddingExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey));
        var embeddingClient = openAIClient.GetEmbeddingClient(project.DeployedModels.TextEmbedding3Small);
        var embeddingOptions = new EmbeddingGenerationOptions { Dimensions = 512 };

        string[] words =
            ["Dog", "Cat", "Daisy", "Rose", "Dobermann", "Lion", "Willow", "Blue", "Wolf", "Birch", "Shrub", "Puppy", "Tiger", "Red", "Green"];

        var vectors512 = await Task.WhenAll(words.Select(async word =>
        {
            var embeddingResult = await embeddingClient.GenerateEmbeddingAsync(word, embeddingOptions);

            return embeddingResult.Value.ToFloats().ToArray();
        }));

        var vectors2 = Reduce(vectors512);

        for (var index = 0; index < words.Length; index++)
        {
            Console.WriteLine($"{words[index]}, {vectors2[index][0]}, {vectors2[index][1]}");
        }
    }

    public static double[][] Reduce(float[][] vectors)
    {
        var data = vectors.Select(row => row.Select(value => (double) value).ToArray()).ToArray();

        var principalComponentAnalysis = new PrincipalComponentAnalysis
                                         {
                                             NumberOfOutputs = 2,
                                             Method = PrincipalComponentMethod.Center
                                         };

        principalComponentAnalysis.Learn(data);

        var reduced = principalComponentAnalysis.Transform(data);

        return reduced;
    }
}
