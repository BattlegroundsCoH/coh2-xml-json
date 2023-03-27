using CoH2XML2JSON.Blueprint.DataEntry;

namespace CoH2XML2JSON.Blueprints;

public class CriticalBlueprint : BaseBlueprint<CriticalBlueprint>, IBlueprint {

    public string? ModGUID { get; init; }

    public ulong PBGID { get; init; }

    public string Name { get; init; } = string.Empty;

    public UI? Display {
        get => GetValue<UI>(nameof(Display));
        set => SetValue(nameof(Display), value);
    }

}
