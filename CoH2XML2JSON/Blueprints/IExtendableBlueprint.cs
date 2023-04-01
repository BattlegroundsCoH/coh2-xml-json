namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents an extendable blueprint interface that provides information about a game object and can extend another blueprint.
/// </summary>
/// <typeparam name="T">The type of the blueprint being extended.</typeparam>
public interface IExtendableBlueprint<T> where T : IBlueprint {

    /// <summary>
    /// Gets or sets the path to the blueprint being extended.
    /// </summary>
    string? ParentFilepath { get; set; }

    /// <summary>
    /// Gets the parent blueprint.
    /// </summary>
    /// <returns>The <typeparamref name="T"/> being extended by this <typeparamref name="T"/> instance.</returns>
    T? GetParent();

}
