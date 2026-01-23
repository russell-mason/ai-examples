namespace MachineLearning.AndGate.Example.NeuralNetworks.Activators;

public interface IActivator
{
    double Activate(double input);

    double ComputeDerivative(double input);

    int ApplyTargetNormalization(int input);

    int ApplyThreshold(double input);
}
