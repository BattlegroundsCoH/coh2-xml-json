using System.ComponentModel;
using System.Text.Json.Serialization;

using CoH2XML2JSON.Blueprint.DataEntry;
using CoH2XML2JSON.Converter;

namespace CoH2XML2JSON.Blueprints;

[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponCategory {
    undefined,
    ballistic,
    explosive,
    flame,
    smallarms,
}

[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponSmallArmsType {
    invalid,
    heavymachinegun,
    lightmachinegun,
    submachinegun,
    pistol,
    rifle
}

[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponBalisticType {
    invalid,
    antitankgun,
    tankgun,
    infantryatgun,
}

[JsonConverter(typeof(StringEnumConverter))]
public enum WeaponExplosiveType {
    invalid,
    grenade,
    artillery,
    mine,
    mortar
}

public sealed class WeaponBlueprint : BaseBlueprint<WeaponBlueprint>, IBlueprint {

    /// <inheritdoc/>
    public string? ModGUID { get; init; }

    /// <inheritdoc/>
    public ulong PBGID { get; init; }

    /// <inheritdoc/>
    public string Name { get; init; } = string.Empty;

    public UI? Display {
        get => GetValue<UI>();
        set => SetValue(value);
    }

    [DefaultValue(0.0f)]
    public float Range {
        get => GetValue<float>();
        set => SetValue(value);
    }

    [DefaultValue(0.0f)]
    public float Damage {
        get => GetValue<float>();
        set => SetValue(value);
    }

    public int MagazineSize {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public float FireRate {
        get => GetValue<float>();
        set => SetValue(value);
    }

    public WeaponCategory Category {
        get => GetValue<WeaponCategory>();
        set => SetValue(value);
    }

    public WeaponSmallArmsType SmallArmsType {
        get => GetValue<WeaponSmallArmsType>();
        set => SetValue(value);
    }

    public WeaponBalisticType BalisticType {
        get => GetValue<WeaponBalisticType>();
        set => SetValue(value);
    }

    public WeaponExplosiveType ExplosiveType {
        get => GetValue<WeaponExplosiveType>();
        set => SetValue(value);
    }

    public string? CallbackType {
        get => GetValue<string>();
        set => SetValue(value);
    }

}
