namespace MachineLearning.AndGate.Example.Statistics;

public static class Metrics
{
    public static MetricsScores Score(IEnumerable<Sample> samples, IEnumerable<int> outputs)
    {
        var sampleList = samples.ToList();
        var outputList = outputs.ToList();

        var totalPredicted = sampleList.Count;
        var totalPredictedCorrectly = sampleList.Zip(outputList).Count(pair => pair.First.Output == pair.Second);

        var accuracy = (float) totalPredictedCorrectly / totalPredicted;

        return new MetricsScores(accuracy);
    }
}
