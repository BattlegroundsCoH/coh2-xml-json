using System.Xml;

namespace CoH2XML2JSON.Blueprints.DataEntry;

public class UICoH3 : UI {

    public UICoH3(XmlElement? xmlElement) : base(xmlElement) {
        if (xmlElement is not null) {

            LocaleName = GetStr(xmlElement, "locstring", "screen_name") ?? string.Empty;
            LocaleDescriptionLong = GetStr(xmlElement, "locstring", "brief_text") ?? (GetStr(xmlElement, "locstring", "extra_help_text") ?? string.Empty);
            LocaleDescriptionShort = GetStr(xmlElement, "locstring", "help_text");

            // Read portrait
            Portrait = xmlElement.FindSubnode("icon", "portrait_name")?.GetAttribute("value") ?? null;
            if (string.IsNullOrEmpty(Portrait)) {
                Portrait = xmlElement.FindSubnode("file", "portrait_name") is XmlElement coh3FileRef ? coh3FileRef.GetAttribute("value") : null;
            }

        }
    }

}
