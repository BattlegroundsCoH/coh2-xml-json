using System.ComponentModel;

using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints.Relations;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents an upgrade blueprint that can be applied to an SBP or EBP.
/// </summary>
public sealed class UpgradeBlueprint : BaseBlueprint<UpgradeBlueprint>, IBlueprint {

    /// <inheritdoc/>
    public string? ModGUID { get; init; }

    /// <inheritdoc/>
    public ulong PBGID { get; init; }

    /// <inheritdoc/>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the UI display settings for this upgrade blueprint.
    /// </summary>
    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the cost of this upgrade blueprint.
    /// </summary>
    public Cost? Cost {
        get => GetValue<Cost>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the owner type of this upgrade blueprint.
    /// </summary>
    [DefaultValue(null)]
    public string? OwnerType {
        get => GetValue<string>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the array of slot items for this upgrade blueprint.
    /// </summary>
    [DefaultValue(null)]
    public string[]? SlotItems {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the requirements for this upgrade blueprint.
    /// </summary>
    [DefaultValue(null)]
    public Requirement[]? Requirements {
        get => GetValue<Requirement[]>();
        set => SetValue(value);
    }

}
