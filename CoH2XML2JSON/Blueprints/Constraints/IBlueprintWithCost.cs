using CoH2XML2JSON.Blueprints.DataEntry;

namespace CoH2XML2JSON.Blueprints.Constraints;

/// <summary>
/// Represents an <see cref="IBlueprint"/> that has a <see cref="DataEntry.Cost"/> instance associated with it.
/// </summary>
public interface IBlueprintWithCost {

    /// <summary>
    /// Get the <see cref="DataEntry.Cost"/> instance.
    /// </summary>
    Cost? Cost { get; }

}
