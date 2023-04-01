namespace CoH2XML2JSON.Blueprints.Constraints;

/// <summary>
/// Represents an <see cref="IBlueprint"/> that is associated with an army.
/// </summary>
public interface IBlueprintOfArmy : IBlueprint {

    /// <summary>
    /// Gets or sets the army associated with this blueprint.
    /// </summary>
    /// <value>The army associated with this blueprint.</value>
    public string? Army { get; set; }

}
