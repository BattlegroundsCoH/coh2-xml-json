using System.ComponentModel;
using System.Text.Json.Serialization;

using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints.Relations;
using CoH2XML2JSON.Converter;

namespace CoH2XML2JSON.Blueprints;

/// <summary>
/// Represents the category of a weapon.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponCategory {
    undefined,
    ballistic,
    explosive,
    flame,
    smallarms,
}

/// <summary>
/// Represents the type of small arms weapon.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponSmallArmsType {
    invalid,
    heavymachinegun,
    lightmachinegun,
    submachinegun,
    pistol,
    rifle
}

/// <summary>
/// Represents the type of ballistic weapon.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponBalisticType {
    invalid,
    antitankgun,
    tankgun,
    infantryatgun,
}

/// <summary>
/// Represents the type of explosive weapon.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponExplosiveType {
    invalid,
    grenade,
    artillery,
    mine,
    mortar
}

/// <summary>
/// Represents a blueprint for a weapon.
/// </summary>
public sealed class WeaponBlueprint : BaseBlueprint<WeaponBlueprint>, IBlueprint {

    /// <inheritdoc/>
    public string? ModGUID { get; init; }

    /// <inheritdoc/>
    public ulong PBGID { get; init; }

    /// <inheritdoc/>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The user interface of the weapon.
    /// </summary>
    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

    /// <summary>
    /// The range of the weapon.
    /// </summary>
    [DefaultValue(0.0f)]
    public float Range {
        get => GetValue<float>();
        set => SetValue(value);
    }

    /// <summary>
    /// The damage of the weapon.
    /// </summary>
    [DefaultValue(0.0f)]
    public float Damage {
        get => GetValue<float>();
        set => SetValue(value);
    }

    /// <summary>
    /// The size of the magazine of the weapon.
    /// </summary>
    public int MagazineSize {
        get => GetValue<int>();
        set => SetValue(value);
    }

    /// <summary>
    /// The fire rate of the weapon.
    /// </summary>
    public float FireRate {
        get => GetValue<float>();
        set => SetValue(value);
    }

    /// <summary>
    /// The category of the weapon.
    /// </summary>
    public WeaponCategory Category {
        get => GetValue<WeaponCategory>();
        set => SetValue(value);
    }

    /// <summary>
    /// The small arms type of the weapon.
    /// </summary>
    public WeaponSmallArmsType SmallArmsType {
        get => GetValue<WeaponSmallArmsType>();
        set => SetValue(value);
    }

    /// <summary>
    /// The ballistic type of the weapon.
    /// </summary>
    public WeaponBalisticType BalisticType {
        get => GetValue<WeaponBalisticType>();
        set => SetValue(value);
    }

    /// <summary>
    /// The explosive type of the weapon.
    /// </summary>
    public WeaponExplosiveType ExplosiveType {
        get => GetValue<WeaponExplosiveType>();
        set => SetValue(value);
    }

    /// <summary>
    /// The callback type of the weapon.
    /// </summary>
    public string? CallbackType {
        get => GetValue<string>();
        set => SetValue(value);
    }

}
