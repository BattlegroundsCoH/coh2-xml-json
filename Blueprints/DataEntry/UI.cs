using System.ComponentModel;
using System.Xml;

namespace CoH2XML2JSON.Blueprint.DataEntry;

/// <summary>
/// UI class represents a user interface element. It contains information such as the name, description, icon, symbol, portrait and position of the UI element.
/// </summary>
public sealed class UI {

    /// <summary>
    /// Gets the localized name of the UI element.
    /// </summary>
    public string LocaleName { get; }

    /// <summary>
    /// Gets the long localized description of the UI element.
    /// </summary>
    public string LocaleDescriptionLong { get; }

    /// <summary>
    /// Gets or sets the short localized description of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? LocaleDescriptionShort { get; }

    /// <summary>
    /// Gets or sets the icon name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Icon { get; }

    /// <summary>
    /// Gets or sets the symbol icon name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Symbol { get; }

    /// <summary>
    /// Gets or sets the portrait name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Portrait { get; }

    /// <summary>
    /// Gets or sets the position of the UI element.
    /// </summary>
    [DefaultValue(0)]
    public int Position { get; set; }

    /// <summary>
    /// Initializes a new instance of the UI class with the specified XmlElement.
    /// </summary>
    /// <param name="xmlElement">The XmlElement containing UI information.</param>
    public UI(XmlElement xmlElement) {
        if (xmlElement is not null) {
            LocaleName = GetStr(xmlElement, "locstring", "screen_name") ?? string.Empty;
            LocaleDescriptionLong = GetStr(xmlElement, "locstring", "help_text") ?? string.Empty;
            LocaleDescriptionShort = GetStr(xmlElement, "locstring", "extra_text");
            Icon = xmlElement.FindSubnode("icon", "icon_name")?.GetAttribute("value") ?? null;
            if (string.IsNullOrEmpty(Icon)) {
                Icon = null;
            }
            Symbol = xmlElement.FindSubnode("icon", "symbol_icon_name")?.GetAttribute("value") ?? null;
            Portrait = xmlElement.FindSubnode("icon", "portrait_name_summer")?.GetAttribute("value") ?? null;
        } else {
            LocaleName = string.Empty; 
            LocaleDescriptionLong = string.Empty;
        }
    }

    /// <summary>
    /// Gets the value of the specified locstring element in the specified XmlElement.
    /// </summary>
    /// <param name="xmlElement">The XmlElement containing the locstring element.</param>
    /// <returns>The value of the locstring element, or null if the element does not exist.</returns>
    public static string? GetStr(XmlElement xmlElement, string tag, string name) {
        var xmlNode = xmlElement.FindSubnode(tag, name);
        if (xmlNode is not null) {
            return GetStr(xmlNode);
        }
        return null;
    }

    /// <summary>
    /// Gets the value of the specified locstring element with the specified name in the specified XmlElement.
    /// </summary>
    /// <param name="xmlElement">The XmlElement containing the locstring element.</param>
    /// <param name="tag">The tag of the locstring element.</param>
    /// <param name="name">The name of the locstring element.</param>
    /// <returns>The value of the locstring element, or null if the element does not exist.</returns>
    public static string? GetStr(XmlElement xmlElement) {
        string? val = xmlElement.GetAttribute("value") ?? null;
        if (val is not null && xmlElement.GetAttribute("mod") is string mod && !string.IsNullOrEmpty(mod)) {
            val = $"${mod}:{val}";
        }
        return val;
    }

}
