using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints.Relations;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents a blueprint for a critical state that can be applied to an EBP.
/// </summary>
public sealed class CriticalBlueprint : BaseBlueprint<CriticalBlueprint>, IBlueprint {

    /// <inheritdoc/>
    public string? ModGUID { get; init; }

    /// <inheritdoc/>
    public ulong PBGID { get; init; }

    /// <inheritdoc/>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the user interface display settings for this blueprint.
    /// </summary>
    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

}
