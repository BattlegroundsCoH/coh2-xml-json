using System.ComponentModel;

namespace CoH2XML2JSON.Blueprints.DataEntry;

/// <summary>
/// Interface for UI extensions
/// </summary>
public interface IUI {

    /// <summary>
    /// Gets the localized name of the UI element.
    /// </summary>
    public string LocaleName { get; init; }

    /// <summary>
    /// Gets the long localized description of the UI element.
    /// </summary>
    public string LocaleDescriptionLong { get; init; }

    /// <summary>
    /// Gets or sets the short localized description of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? LocaleDescriptionShort { get; init; }

    /// <summary>
    /// Gets or sets the icon name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Icon { get; init; }

    /// <summary>
    /// Gets or sets the symbol icon name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Symbol { get; init; }

    /// <summary>
    /// Gets or sets the portrait name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Portrait { get; init; }

    /// <summary>
    /// Gets or sets the position of the UI element.
    /// </summary>
    [DefaultValue(0)]
    public int Position { get; set; }

}
