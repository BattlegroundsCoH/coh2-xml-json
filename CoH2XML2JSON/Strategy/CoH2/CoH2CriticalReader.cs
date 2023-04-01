using System.Xml;

using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.CoH2;

public sealed class CoH2CriticalReader : IBlueprintReader<CriticalBlueprint> {
    
    /// <inheritdoc/>
    public CriticalBlueprint FromXml(XmlDocument xmlDocument, string modGuid, string filename, Helpers helpers) {

        // Set the name
        var name = filename;

        // Set mod GUID
        var modGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid;

        // Load pbgid
        var pBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value"));

        // Load display
        var display = new UI(xmlDocument.SelectSingleNode(@"//template_reference[@name='ui_info']") as XmlElement);

        return new CriticalBlueprint() {
            Name = filename,
            ModGUID = modGUID,
            PBGID = pBGID,
            Display = display
        };

    }

}
