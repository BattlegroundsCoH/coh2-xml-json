using System.ComponentModel;
using CoH2XML2JSON.Blueprint.DataEntry;
using CoH2XML2JSON.Blueprints.Constraints;

namespace CoH2XML2JSON.Blueprints;

public class AbilityBlueprint : BaseBlueprint<AbilityBlueprint>, IBlueprint, IBlueprintOfArmy {

    public string Name { get; init; } = string.Empty;

    public string? ModGUID { get; init; }

    public ulong PBGID { get; init; }

    public Cost? Cost {
        get => GetValue<Cost>(nameof(Cost));
        set => SetValue(nameof(Cost), value);
    }

    public UI? Display {
        get => GetValue<UI>(nameof(Display));
        set => SetValue(nameof(Display), value);
    }

    public string? Army {
        get => GetValue<string>(nameof(Army));
        set => SetValue(nameof(Army), value);
    }

    public string? Activation {
        get => GetValue<string>(nameof(Activation));
        set => SetValue(nameof(Activation), value);
    }

    [DefaultValue(null)]
    public Requirement[]? Requirements {
        get => GetValue<Requirement[]>(nameof(Activation));
        set => SetValue(nameof(Activation), value);
    }

}
