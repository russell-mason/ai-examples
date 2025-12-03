namespace AIExamples.Shared.Examples.Attributes;

/// <summary>
/// Specifies a resource used by the example.
/// </summary>
/// <param name="resource">
/// The resource used by the example.
/// </param>
/// <param name="model">
/// The AI model associated with the resource.
/// </param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ExampleResourceUseAttribute(Resource resource, AIModel model) : Attribute
{
    /// <summary>
    /// Specifies a resource used by the example.
    /// </summary>
    /// <param name="resource">
    /// The resource used by the example. Assumes no LLM is associated with the resource.
    /// </param>
    public ExampleResourceUseAttribute(Resource resource) : this(resource, AIModel.None)
    {
    }

    /// <summary>
    /// The resource used by the example.
    /// </summary>
    public Resource Resource { get; } = resource;

    /// <summary>
    /// The AI model associated with the resource. This will be None if the resource does not relate to an LLM.
    /// </summary>
    public AIModel Model { get; } = model;
}
