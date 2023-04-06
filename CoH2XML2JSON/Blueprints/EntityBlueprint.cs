using System.ComponentModel;

using CoH2XML2JSON.Blueprints.Constraints;
using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints.Relations;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents a blueprint for an entity that can be deployed by an army in the game.
/// </summary>
public sealed class EntityBlueprint : BaseBlueprint<EntityBlueprint>, IBlueprintOfArmy {

    /// <summary>
    /// Represents the crew of an <see cref="EntityBlueprint"/>.
    /// </summary>
    public readonly struct EBPCrew {

        /// <summary>
        /// The army of the crew.
        /// </summary>
        public string Army { get; }

        /// <summary>
        /// The SBP of the crew.
        /// </summary>
        public string SBP { get; }

        /// <summary>
        /// The capture type of the crew.
        /// </summary>
        public string Capture { get; }
        public EBPCrew(string army, string sbp, string capture) {
            Army = army;
            SBP = sbp;
            Capture = capture;
        }
    }

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
    /// The user interface (UI) display for this entity blueprint.
    /// </summary>
    [DefaultValue(null)]
    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

    /// <summary>
    /// The cost required to deploy this entity blueprint.
    /// </summary>
    public Cost? Cost { 
        get => GetValue<Cost>();
        set => SetValue(value);
    }

    /// <summary>
    /// The abilities that can be used by this entity blueprint.
    /// </summary>
    [DefaultValue(null)]
    public string[]? Abilities {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// The drivers of this entity blueprint.
    /// </summary>
    [DefaultValue(null)]
    public EBPCrew[]? Drivers {
        get => GetValue<EBPCrew[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// The hardpoints of this entity blueprint.
    /// </summary>
    [DefaultValue(null)]
    public string[]? Hardpoints {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// Indicates whether this entity blueprint has a recrewable extension.
    /// </summary>
    [DefaultValue(false)]
    public bool HasRecrewableExtension {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    /// <summary>
    /// The health of this entity blueprint.
    /// </summary>
    [DefaultValue(0.0f)]
    public float Health {
        get => GetValue<float>();
        set => SetValue(value);
    }

    /// <summary>
    /// The upgrades available for this entity blueprint.
    /// </summary>
    [DefaultValue(null)]
    public string[]? Upgrades {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// The upgrades that have been applied to this entity blueprint.
    /// </summary>
    [DefaultValue(null)]
    public string[]? AppliedUpgrades {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// The upgrade capacity of this entity blueprint.
    /// </summary>
    [DefaultValue(0)]
    public int UpgradeCapacity {
        get => GetValue<int>();
        set => SetValue(value);
    }

    /// <summary>
    /// The types that apply to this entity blueprint.
    /// </summary>
    [DefaultValue(null)]
    public string[]? Types {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    /// <summary>
    /// Defines if the entity is an inventory item
    /// </summary>
    [DefaultValue(false)]
    public bool IsInventoryItem {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    /// <summary>
    /// The amount of invenctory space required to pickup with item
    /// </summary>
    [DefaultValue(0)]
    public int InventoryRequiredCapacity {
        get => GetValue<int>();
        set => SetValue(value);
    }

    /// <summary>
    /// The chance of this item dropping when the owning entity dies.
    /// </summary>
    [DefaultValue(0.0f)]
    public float InventoryDropOnDeathChance {
        get => GetValue<float>();
        set => SetValue(value);
    }

}
