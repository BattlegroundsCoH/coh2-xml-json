using System.ComponentModel;
using System.Xml;
using CoH2XML2JSON.Blueprints.Relations;

namespace CoH2XML2JSON.Blueprints.DataEntry;

/// <summary>
/// UI class represents a user interface element. It contains information such as the name, description, icon, symbol, portrait and position of the UI element.
/// </summary>
public sealed class UI : Extendable<UI> {

    /// <summary>
    /// Gets the localized name of the UI element.
    /// </summary>
    public string LocaleName {
        get => GetValue<string>() ?? string.Empty;
        init => SetValue(value);
    }

    /// <summary>
    /// Gets the long localized description of the UI element.
    /// </summary>
    public string LocaleDescriptionLong {
        get => GetValue<string>() ?? string.Empty;
        init => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the short localized description of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? LocaleDescriptionShort {
        get => GetValue<string>();
        init => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the icon name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Icon {
        get => GetValue<string>();
        init => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the symbol icon name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Symbol {
        get => GetValue<string>();
        init => SetValue(value);
    }

    /// <summary>
    /// Gets or sets the portrait name of the UI element.
    /// </summary>
    [DefaultValue(null)]
    public string? Portrait { 
        get => GetValue<string>(); 
        init => SetValue(value); 
    }

    /// <summary>
    /// Gets or sets the position of the UI element.
    /// </summary>
    [DefaultValue(0)]
    public int Position {
        get => GetValueWhereDefaultIs(-1);
        set => SetValue(value);
    }

    /// <summary>
    /// Initializes a new instance of the UI class with the specified XmlElement.
    /// </summary>
    /// <param name="xmlElement">The XmlElement containing UI information.</param>
    public UI(XmlElement? xmlElement) {
        if (xmlElement is not null) {
            
            LocaleName = GetStr(xmlElement, "locstring", "screen_name") ?? string.Empty;
            LocaleDescriptionLong = GetStr(xmlElement, "locstring", "help_text") ?? (GetStr(xmlElement, "locstring", "brief_text") ?? string.Empty);
            LocaleDescriptionShort = GetStr(xmlElement, "locstring", "extra_text");
            
            // Read icon
            Icon = xmlElement.FindSubnode("icon", "icon_name")?.GetAttribute("value") ?? null;
            if (string.IsNullOrEmpty(Icon)) {
                Icon = xmlElement.FindSubnode("file", "icon_name") is XmlElement coh3FileRef ? coh3FileRef.GetAttribute("value") : null;
            }

            // Read symbol
            Symbol = xmlElement.FindSubnode("icon", "symbol_icon_name")?.GetAttribute("value") ?? null;
            if (string.IsNullOrEmpty(Symbol)) {
                Symbol = xmlElement.FindSubnode("file", "symbol_icon_name") is XmlElement coh3FileRef ? coh3FileRef.GetAttribute("value") : null;
            }
            
            // Read portrait
            Portrait = xmlElement.FindSubnode("icon", "portrait_name_summer")?.GetAttribute("value") ?? null;
            if (string.IsNullOrEmpty(Portrait)) {
                Portrait = xmlElement.FindSubnode("file", "portrait_name_summer") is XmlElement coh3FileRef ? coh3FileRef.GetAttribute("value") : null;
            }

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

    /// <summary>
    /// Gets the position of a UI element in a grid by parsing the values of the "ui_position_row" and "ui_position_column" XML nodes.
    /// </summary>
    /// <param name="xmlElement">The XML element to parse.</param>
    /// <returns>The position of the UI element in the grid as an integer.</returns>
    /// <remarks>
    /// This method returns -1 if the specified XML element is null or if the "ui_position_row" or "ui_position_column" nodes are not found or their values cannot be parsed as integers.
    /// </remarks>
    public static int GetUIPosition(XmlElement? xmlElement) {
        if (xmlElement is not null) {
            int row = int.Parse(xmlElement.FindSubnode("int", "ui_position_row")?.GetAttribute("value") ?? "0");
            int column = int.Parse(xmlElement.FindSubnode("int", "ui_position_column")?.GetAttribute("value") ?? "0");
            return (row-1) * 3 + column;
        }
        return -1;
    }

}
