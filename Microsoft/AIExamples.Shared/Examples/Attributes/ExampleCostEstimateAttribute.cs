namespace AIExamples.Shared.Examples.Attributes;

/// <summary>
/// Specifies an estimated cost for running the example.
/// </summary>
/// <param name="estimateInUSD">
/// A very rough estimate of the raw compute cost associated with running the example. In U.S. dollars.
/// </param>
/// <param name="dailyOverheadInUSD">
/// A very rough estimate of additional daily charges associated with the resource itself, i.e. on top of the raw compute cost. In U.S. dollars.
/// </param>
/// <param name="costVisibility">
/// Whether the cost is simple to determine from the pricing pace, or whether the total cost will turn out to be a surprise (and probably not in a good way).
/// </param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ExampleCostEstimateAttribute(double estimateInUSD, double dailyOverheadInUSD, CostVisibility costVisibility) : Attribute
{
    /// <summary>
    /// Specifies an estimated cost for running the example. Assumes pricing is transparent.
    /// </summary>
    /// <param name="estimateInUSD">
    /// A very rough estimate of the raw compute cost associated with running the example. In U.S. dollars.
    /// </param>
    /// <param name="dailyOverheadInUSD">
    /// A very rough estimate of additional daily charges associated with the resource itself, i.e. on top of the raw compute cost. In U.S. dollars.
    /// </param>
    public ExampleCostEstimateAttribute(double estimateInUSD, double dailyOverheadInUSD) : this(estimateInUSD, dailyOverheadInUSD, CostVisibility.Transparent)
    {
    }

    /// <summary>
    /// Specifies an estimated cost for running the example. Assumes pricing is transparent and there are no overhead charges.
    /// </summary>
    /// <param name="estimateInUSD">
    /// A very rough estimate of additional daily charges associated with the resource itself, i.e. on top of the raw compute cost. In U.S. dollars.
    /// </param>
    public ExampleCostEstimateAttribute(double estimateInUSD) : this(estimateInUSD, 0)
    {
    }

    /// <summary>
    /// A very rough estimate of the raw compute cost associated with running the example. In U.S. dollars.
    /// </summary>
    public double Estimate { get; } = estimateInUSD;

    /// <summary>
    /// A very rough estimate of additional daily charges associated with the resource itself, i.e. on top of the raw compute cost. In U.S. dollars.
    /// </summary>
    public double DailyOverhead { get; } = dailyOverheadInUSD;

    /// <summary>
    /// Whether the cost is simple to determine from the pricing pace, or whether the total cost will turn out to be a surprise (and probably not in a good way).
    /// </summary>
    public CostVisibility CostVisibility { get; } = costVisibility;
}