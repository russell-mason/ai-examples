namespace MachineLearning.AndGate.Example.NeuralNetworks.Activators;

public class LogisticSigmoid : IActivator
{
    public double Activate(double input) => 1 / (1 + Math.Exp(-input));

    public double ComputeDerivative(double input)
    {
        var activated = Activate(input);

        return activated * (1 - activated);
    }

    public int ApplyTargetNormalization(int input) => input;

    public int ApplyThreshold(double input) => input > 0.5 ? 1 : 0;
}
