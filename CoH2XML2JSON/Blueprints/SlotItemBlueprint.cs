using System.ComponentModel;

using CoH2XML2JSON.Blueprints.Constraints;
using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints.Relations;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Blueprint for a slot item that can be equipped on a unit.
/// </summary>
public sealed class SlotItemBlueprint : BaseBlueprint<SlotItemBlueprint>, IBlueprintOfArmy {

    /// <inheritdoc/>
    public string? ModGUID { get; init; }

    /// <inheritdoc/>
    public ulong PBGID { get; init; }

    /// <inheritdoc/>
    public string Name { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string? Army {
        get => GetValue<string>();
        set => SetValue(value);
    }

    /// <summary>
    /// The user interface elements for this slot item blueprint.
    /// </summary>
    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

    /// <summary>
    /// The weapon blueprint associated with this slot item, if any.
    /// </summary>
    [DefaultValue(null)]
    public string? WPB {
        get => GetValue<string>();
        set => SetValue(value);
    }

    /// <summary>
    /// The size of the slot required to equip this item.
    /// </summary>
    [DefaultValue(0)]
    public int SlotSize {
        get => GetValue<int>();
        set => SetValue(value);
    }

}
