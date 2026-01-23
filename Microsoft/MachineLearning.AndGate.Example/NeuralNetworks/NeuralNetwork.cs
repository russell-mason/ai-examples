namespace MachineLearning.AndGate.Example.NeuralNetworks;

public class NeuralNetwork(IActivator? activator = null)
{
    public Neuron Neuron { get; } = new(new NeuronState(), activator ?? new LogisticSigmoid());

    public Action<Neuron, int, int, double, double>? EpochProgress { get; set; }

    public Action<int, int, IntermediateState>? SampleProgress { get; set; }

    public void Fit(IEnumerable<Sample> samples, int epochs = 1000, double learningRate = 0.1)
    {
        var intermediateStates = samples.Select(sample => new IntermediateState(sample)).ToList();

        EpochProgress?.Invoke(Neuron, 0, epochs, learningRate, double.MaxValue);

        for (var epoch = 0; epoch < epochs; epoch++)
        {
            for (var index = 0; index < intermediateStates.Count; index++)
            {
                var intermediateState = intermediateStates[index];

                Feedforward(Neuron, intermediateState);
                ComputeError(Neuron, intermediateState);
                Backpropagation(Neuron, intermediateState);
                UpdateNeuronState(Neuron, learningRate, intermediateState);

                SampleProgress?.Invoke(index, intermediateStates.Count, intermediateState);
            }

            var epochLoss = intermediateStates.Average(state => state.Loss);

            EpochProgress?.Invoke(Neuron, epoch + 1, epochs, learningRate, epochLoss);
        }
    }

    public int Predict(Sample sample) => Predict([sample]).First();

    public IEnumerable<int> Predict(IEnumerable<Sample> samples) =>
        samples.Select(sample => Neuron.Activator.ApplyThreshold(Neuron.Activate(sample.Input1, sample.Input2).Output));

    private static void Feedforward(Neuron neuron, IntermediateState intermediateState)
    {
        var (preActivation, postActivation) = neuron.Activate(intermediateState.Sample.Input1, intermediateState.Sample.Input2);

        intermediateState.PreActivation = preActivation;
        intermediateState.PostActivation = postActivation;
    }

    private static void ComputeError(Neuron neuron, IntermediateState intermediateState)
    {
        var scaledOutput = neuron.Activator.ApplyTargetNormalization(intermediateState.Sample.Output);

        intermediateState.Error = scaledOutput - intermediateState.PostActivation;
        intermediateState.Loss = Math.Pow(intermediateState.Error, 2);
    }

    private static void Backpropagation(Neuron neuron, IntermediateState intermediateState)
    {
        var derivative = neuron.Activator.ComputeDerivative(intermediateState.PreActivation);

        intermediateState.Gradient.Weight1 = intermediateState.Error * derivative * intermediateState.Sample.Input1;
        intermediateState.Gradient.Weight2 = intermediateState.Error * derivative * intermediateState.Sample.Input2;
        intermediateState.Gradient.Bias = intermediateState.Error * derivative;
    }

    private static void UpdateNeuronState(Neuron neuron, double learningRate, IntermediateState intermediateState) =>
        neuron.State.Update(neuron.State.Weight1 + learningRate * intermediateState.Gradient.Weight1,
                            neuron.State.Weight2 + learningRate * intermediateState.Gradient.Weight2,
                            neuron.State.Bias + learningRate * intermediateState.Gradient.Bias);
}
