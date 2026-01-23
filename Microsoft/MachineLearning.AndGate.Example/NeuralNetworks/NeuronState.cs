namespace MachineLearning.AndGate.Example.NeuralNetworks;

public class NeuronState
{
    private static readonly Random _random = new();

    public double Weight1 { get; private set; } = CreateInitialValue();

    public double Weight2 { get; private set; } = CreateInitialValue();

    public double Bias { get; private set; } = 0;

    public void Update(double weight1, double weight2, double bias)
    {
        Weight1 = weight1;
        Weight2 = weight2;
        Bias = bias;
    }

    public override string ToString() => $"Weight1: {Weight1}, Weight2: {Weight2}, Bias: {Bias}";

    private static double CreateInitialValue() => .01 * _random.NextDouble() * (_random.Next(2) == 0 ? -1 : 1);
}
