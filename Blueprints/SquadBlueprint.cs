using System;
using System.ComponentModel;

using CoH2XML2JSON.Blueprint.DataEntry;
using CoH2XML2JSON.Blueprints.Constraints;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents a blueprint for a squad.
/// </summary>
public sealed class SquadBlueprint : BaseBlueprint<SquadBlueprint>, IBlueprintOfArmy {

    /// <summary>
    /// Represents a loadout of entities with a specific EBP and count.
    /// </summary>
    public record Loadout(string EBP, int Count);

    /// <summary>
    /// Represents a veterancy level with a specific screen name and experience value.
    /// </summary>
    public record VeterancyLevel(string ScreenName, float Experience);

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
    /// Gets or sets the UI display information for the squad.
    /// </summary>
    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the squad cost.
    /// </summary>
    public Cost? SquadCost {
        get => GetValue<Cost>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the squad has a crew.
    /// </summary>
    public bool HasCrew {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the squad has synchronised weapon (Team Weapon).
    /// </summary>
    public bool IsSyncWeapon {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the entities in the squad, including their EBP and count.
    /// </summary>
    public Loadout[]? Entities {
        get => GetValue<Loadout[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the female chance for the squad.
    /// </summary>
    public float FemaleChance {
        get => GetValue<float>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the abilities of the squad.
    /// </summary>
    public string[]? Abilities {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the veterancy levels of the squad.
    /// </summary>
    public VeterancyLevel[]? Veterancy {
        get => GetValue<VeterancyLevel[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the types of the squad.
    /// </summary>
    public string[] Types {
        get => GetValue<string[]>() ?? Array.Empty<string>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the slot pickup capacity of the squad.
    /// </summary>
    [DefaultValue(0)]
    public int SlotPickupCapacity {
        get => GetValue<int>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the squad can pick up items.
    /// </summary>
    [DefaultValue(false)]
    public bool CanPickupItems {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the upgrade capacity of the squad.
    /// </summary>
    [DefaultValue(0)]
    public int UpgradeCapacity {
        get => GetValue<int>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the upgrades available for the squad.
    /// </summary>
    [DefaultValue(null)]
    public string[]? Upgrades {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the upgrades that have been applied to the squad.
    /// </summary>
    [DefaultValue(null)]
    public string[]? AppliedUpgrades {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

}
