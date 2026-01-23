namespace MachineLearning.AndGate.Example.NeuralNetworks;

public class Neuron(NeuronState state, IActivator activator)
{
    public NeuronState State { get; } = state;

    public IActivator Activator { get; } = activator;

    public (double WeightedSum, double Output) Activate(double input1, double input2)
    {
        var weightedSum = CalculateWeightedSum(input1, input2);
        var output = Activator.Activate(weightedSum);

        return (weightedSum, output);
    }

    private double CalculateWeightedSum(double input1, double input2) =>
        input1 * State.Weight1 + input2 * State.Weight2 + State.Bias;
}
