namespace AIExamples.Shared.Examples.Attributes;

public enum CostVisibility
{
    /// <summary>
    /// When visiting pricing pages, you should be able to work out the approximate cost.
    /// </summary>
    Transparent = 0,

    /// <summary>
    /// When visiting pricing pages, the cost may not match your expectations as the costs are not obvious and do not
    /// clearly match the actual cost incurred.
    /// </summary>
    Opaque = 1
}
