using System.ComponentModel;

using CoH2XML2JSON.Blueprint.DataEntry;

namespace CoH2XML2JSON.Blueprints;

public sealed class UpgradeBlueprint : BaseBlueprint<UpgradeBlueprint>, IBlueprint {

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

    public Cost? Cost {
        get => GetValue<Cost>();
        set => SetValue(value);
    }

    [DefaultValue(null)]
    public string? OwnerType {
        get => GetValue<string>();
        set => SetValue(value);
    }

    [DefaultValue(null)]
    public string[]? SlotItems {
        get => GetValue<string[]>();
        set => SetValue(value);
    }

    [DefaultValue(null)]
    public Requirement[]? Requirements {
        get => GetValue<Requirement[]>();
        set => SetValue(value);
    }

}
