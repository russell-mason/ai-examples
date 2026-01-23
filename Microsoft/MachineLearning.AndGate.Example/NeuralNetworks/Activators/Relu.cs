namespace MachineLearning.AndGate.Example.NeuralNetworks.Activators;

public class Relu : IActivator
{
    public double Activate(double input) => input >= 0 ? input : 0;

    public double ComputeDerivative(double input) => input >= 0 ? 1 : 0;

    public int ApplyTargetNormalization(int input) => input;

    public int ApplyThreshold(double input) => input > 0 ? 1 : 0;
}
