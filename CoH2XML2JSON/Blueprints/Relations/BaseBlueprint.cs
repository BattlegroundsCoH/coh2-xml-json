namespace CoH2XML2JSON.Blueprints.Relations;

/// <summary>
/// Base class for blueprint classes that can be extended by another blueprint.
/// </summary>
/// <typeparam name="T">The type of the blueprint that can be extended.</typeparam>
public abstract class BaseBlueprint<T> : Extendable<BaseBlueprint<T>>, IExtendableBlueprint<T> where T : IBlueprint {

    /// <inheritdoc/>
    public string? ParentFilepath { get; set; }

    /// <inheritdoc/>
    public T? GetParent() => this.Extends is T parent ? parent : default;

}
