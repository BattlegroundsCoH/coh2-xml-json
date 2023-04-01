using CoH2XML2JSON.Blueprints.DataEntry;

namespace CoH2XML2JSON.Blueprints.Constraints;

/// <summary>
/// Represents an <see cref="IBlueprint"/> that has a <see cref="UI"/> instance associated with it.
/// </summary>
public interface IBlueprintWithDisplay {

    /// <summary>
    /// Get the <see cref="UI"/> instance.
    /// </summary>
    UI? Display { get; }

}
