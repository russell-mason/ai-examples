namespace AIExamples.Shared.Examples.Attributes;

/// <summary>
/// Specifies a category for an example, allowing examples to be grouped or filtered.
/// <para>
/// Multiple categories can be applied to the same example.
/// </para>
/// </summary>
/// <param name="category">The category to associate with the example.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ExampleCategoryAttribute(Category category) : Attribute
{
    /// <summary>
    /// The category associated with the example.
    /// </summary>
    public Category Category { get; } = category;
}
