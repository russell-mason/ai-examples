namespace AIExamples.Shared.Examples.Attributes;

/// <summary>
/// Specifies an estimated cost for running the example.
/// </summary>
/// <param name="estimateInUSD">
/// A very rough estimate of what the cost associated with running the example might be. In U.S. dollars.
/// </param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ExampleCostEstimateAttribute(double estimateInUSD) : Attribute
{
    /// <summary>
    /// A very rough estimate of what the cost associated with running the example might be. In U.S. dollars.
    /// </summary>
    public double Estimate { get; } = estimateInUSD;
}
