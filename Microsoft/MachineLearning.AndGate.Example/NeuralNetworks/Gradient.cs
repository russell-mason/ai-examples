namespace MachineLearning.AndGate.Example.NeuralNetworks;

public class Gradient
{
    public double Weight1 { get; set; } = 0;

    public double Weight2 { get; set; } = 0;

    public double Bias { get; set; } = 0;

    public override string ToString() => $"Weight1: {Weight1}, Weight2: {Weight2}, Bias: {Bias}";
}
