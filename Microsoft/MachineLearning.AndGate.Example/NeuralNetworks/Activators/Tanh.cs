namespace MachineLearning.AndGate.Example.NeuralNetworks.Activators;

public class Tanh : IActivator
{
    public double Activate(double input) => Math.Tanh(input);

    public double ComputeDerivative(double input)
    {
        var activated = Activate(input);

        return 1 - activated * activated;
    }

    public int ApplyTargetNormalization(int input) => input == 0 ? -1 : 1;

    public int ApplyThreshold(double input) => input > 0 ? 1 : 0;
}
