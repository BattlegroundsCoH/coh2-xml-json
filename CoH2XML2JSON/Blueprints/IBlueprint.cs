namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents a blueprint interface that provides information about a game object.
/// </summary>
public interface IBlueprint {

    /// <summary>
    /// Gets the mod GUID associated with the game object.
    /// </summary>
    /// <value>The mod GUID associated with the game object.</value>
    public string? ModGUID { get; }

    /// <summary>
    /// Gets the PBGID (Property Bag Group Identifier) of the game object.
    /// </summary>
    /// <value>The PBGID of the game object.</value>
    public ulong PBGID { get; }

    /// <summary>
    /// Gets the name of the game object.
    /// </summary>
    /// <value>The name of the game object.</value>
    public string Name { get; }

}
