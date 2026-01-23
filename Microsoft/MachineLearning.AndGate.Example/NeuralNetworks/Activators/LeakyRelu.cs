namespace MachineLearning.AndGate.Example.NeuralNetworks.Activators;

public class LeakyRelu : IActivator
{
    public double Activate(double input) => input >= 0 ? input : input * 0.01;

    public double ComputeDerivative(double input) => input >= 0 ? 1 : 0.01;

    public int ApplyTargetNormalization(int input) => input;

    public int ApplyThreshold(double input) => input > 0.01 ? 1 : 0;
}
