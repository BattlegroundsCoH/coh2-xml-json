using System.ComponentModel;

using CoH2XML2JSON.Blueprint.DataEntry;
using CoH2XML2JSON.Blueprints.Constraints;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents a blueprint for an ability that can be used by an army, squad, or entity.
/// </summary>
public sealed class AbilityBlueprint : BaseBlueprint<AbilityBlueprint>, IBlueprintOfArmy {

    /// <inheritdoc/>
    public string Name { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string? ModGUID { get; init; }

    /// <inheritdoc/>
    public ulong PBGID { get; init; }

    /// <summary>
    /// The cost of the ability.
    /// </summary>
    public Cost? Cost {
        get => GetValue<Cost>();
        set => SetValue(value);
    }

    /// <summary>
    /// The user interface settings for the ability.
    /// </summary>
    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

    /// <summary>
    /// The name of the army that can use this ability.
    /// </summary>
    public string? Army {
        get => GetValue<string>();
        set => SetValue(value);
    }

    /// <summary>
    /// The activation mode for the ability.
    /// </summary>
    public string? Activation {
        get => GetValue<string>();
        set => SetValue(value);
    }

    /// <summary>
    /// The requirements for the ability.
    /// </summary>
    [DefaultValue(null)]
    public Requirement[]? Requirements {
        get => GetValue<Requirement[]>();
        set => SetValue(value);
    }

}
