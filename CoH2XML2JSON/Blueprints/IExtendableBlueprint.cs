namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents an extendable blueprint interface that provides information about a game object and can extend another blueprint.
/// </summary>
/// <typeparam name="T">The type of the blueprint being extended.</typeparam>
public interface IExtendableBlueprint<T> where T : IBlueprint {

    /// <summary>
    /// Gets or sets the blueprint being extended.
    /// </summary>
    /// <value>The blueprint being extended. Can be <c>null</c>.</value>
    public T? Extends { get; set; }

}
