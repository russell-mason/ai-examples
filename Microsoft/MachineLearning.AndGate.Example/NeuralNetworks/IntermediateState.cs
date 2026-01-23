namespace MachineLearning.AndGate.Example.NeuralNetworks;

public class IntermediateState(Sample sample)
{
    public Sample Sample { get; } = sample;

    public double PreActivation { get; set; }

    public double PostActivation { get; set; }

    public double Error { get; set; }

    public double Loss { get; set; }

    public Gradient Gradient { get; set; } = new();

    public override string ToString() =>
        $"Sample: [{Sample}], PreActivation: {PreActivation}, PostActivation: {PostActivation}, " +
        $"Error: {Error}, Loss: {Loss}, Gradient: [{Gradient}]";
}
