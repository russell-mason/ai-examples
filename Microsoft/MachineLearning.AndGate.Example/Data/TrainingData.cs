namespace MachineLearning.AndGate.Example.Data;

public static class TrainingData
{
    private static readonly Random _random = new();

    public static IEnumerable<Sample> CreatePermutationsSamples()
    {
        int[][] twoInputsOneOutput = [[0, 0, 0], [0, 1, 0], [1, 0, 0], [1, 1, 1]];

        return twoInputsOneOutput.Select(sample => new Sample(sample[0], sample[1], sample[2]));
    }

    public static IEnumerable<Sample> CreateRandomSamples(int quantity) =>
        Enumerable.Range(0, quantity - 1)
                  .Select(_ =>
                  {
                      var input1 = _random.Next(2);
                      var input2 = _random.Next(2);
                      var output = (input1 == 1) && (input2 == 1) ? 1 : 0;

                      return new Sample(input1, input2, output);
                  });
}
