namespace MachineLearning.AndGate.Example;

/// <summary>
/// Demonstrates how to create a simple one neuron AND logic gate.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.MachineLearning)]
[ExampleCostEstimate(0.00)]
public class AndGateExample : IExample
{
    public Task ExecuteAsync()
    {
        const int epochs = 1000;
        const double learningRate = 0.1;

        var activator = new LogisticSigmoid();

        var neuralNetwork = new NeuralNetwork(activator);

        // Train

        neuralNetwork.EpochProgress = DisplayEpochProgress;
        neuralNetwork.SampleProgress = DisplaySampleProgress;

        var trainingSamples = TrainingData.CreatePermutationsSamples().ToList();

        neuralNetwork.Fit(trainingSamples, epochs, learningRate);

        // Test

        var testSamples = TrainingData.CreateRandomSamples(500).ToList();

        var testPredictions = neuralNetwork.Predict(testSamples).ToList();
        var scores = Metrics.Score(testSamples, testPredictions);

        // Output

        DisplayPredictions(testSamples, testPredictions);
        DisplayScores(scores);

        return Task.CompletedTask;
    }

    private static void DisplayEpochProgress(Neuron neuron, int epoch, int epochs, double learningRate, double loss)
    {
        if (epoch == 0)
        {
            Console.WriteLineInColor($"Epochs: {epochs}", ConsoleColor.Yellow);
            Console.WriteLineInColor($"Activation Function: {neuron.Activator.GetType().Name}", ConsoleColor.Yellow);
            Console.WriteLineInColor($"Learning Rate: {learningRate}", ConsoleColor.Yellow);
        }

        Console.WriteLine();
        Console.WriteLine($"Epoch: {epoch}");
        Console.WriteLine($"Neuron State: {neuron.State}");
        Console.WriteLine($"Loss: {loss}");
    }

    private static void DisplaySampleProgress(int index, int count, IntermediateState intermediateState)
    {
        if (index == 0)
        {
            Console.WriteLine();
        }

        Console.WriteLineInColor($"{index} - Intermediate State: {intermediateState}", ConsoleColor.DarkGray);
    }

    private static void DisplayPredictions(List<Sample> testSamples, List<int> testPredictions)
    {
        Console.WriteLine();
        Console.WriteTitle("Sample Predictions:");
        Console.WriteLine();

        for (var index = 0; index < testSamples.Count; index++)
        {
            var sample = testSamples[index];
            var predicted = testPredictions[index];
            var match = predicted == sample.Output;
            var matchText = match ? "Correct" : "Wrong";
            var color = match ? ConsoleColor.Green : ConsoleColor.Red;

            Console.WriteLineInColor($"{sample.Input1} AND {sample.Input2} = {sample.Output} ... {predicted}  [{matchText}]", color);
        }
    }

    private static void DisplayScores(MetricsScores scores)
    {
        Console.WriteLine();
        Console.WriteTitle($"Accuracy: {scores.Accuracy * 100:F2}%");
    }
}
