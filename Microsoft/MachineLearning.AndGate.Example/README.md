# Getting Started

These examples can be executed via the **MachineLearning.ExamplesConsoleApp** project.

### Intent

This example is for experimental purposes only, used to aid in the learning and exploration of AI.

MNIST is often referred to as the "Hello World" of machine learning. That example can be found in the Python section,
and shows a step-by-step approach. Ultimately, however, it uses features from a library called scikit-learn.

In contrast, this example has an even simpler premise, meaning its possible to follow the whole process, demonstrating
basic principles of a neural network from scratch. The example shows how to learn from two logical inputs in order to
simulate an AND gate.

In C#, an AND gate can be demonstrated with just a bitwise operation `var output = input1 & input2;`. Writing a neural
network for this is obviously massive overkill, and completely impractical as a real-world solution. What it does do,
however, is show some of the fundamentals of a neural network, in its simplest form using a single neuron, purely as
a learning experience, reasonably easy to follow, and completely transparent.

This implementation favours simplicity and clarity over any kind of efficiency and performance. Although the code could
easily be condensed, it separates responsibilities into different classes, and tries to mimic (albeit in a very naive
way) the general pattern of a neural network.

You can experiment by passing in different activation functions, number of epochs, and learning rates.

Main points of interest:

- Weights and Bias
- Calculating Weighted Sum
- Applying an Activation function
- Calculating Error and Loss
- Calculating Gradient Descent
- Showing progress and intermediate state for additional insights
- Showing predictions and accuracy

Limitations:

- There is no short circuit when convergence is achieved, it continues until all epochs have been iterated
- Because its a single neuron, there's only one activation function, so there's no separation of layer and output,
  which would normally use different activations. Because three of the four inputs involve zero outputs, all learning
  comes from one sample. Therefore, if you use the ReLU activation, it will almost always fail and simply stall. Use
  the LeakyRelU to see the difference

**Disclaimer:**

All notes and code are provided in good faith. However, because this is a learning exercise for me, I may
have misinterpreted information. Although every effort has been made to be as accurate as possible,
this is a first pass, and is subject to change, or correction, as my understanding grows.

---

Return to the repository [README](../../) file
