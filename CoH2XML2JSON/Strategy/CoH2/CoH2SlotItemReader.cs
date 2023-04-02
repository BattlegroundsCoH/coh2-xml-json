using System.Globalization;
using System.IO;
using System.Xml;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.CoH2;

public sealed class CoH2SlotItemReader : IBlueprintReader<SlotItemBlueprint> {

    /// <inheritdoc/>
    public SlotItemBlueprint FromXml(XmlDocument xmlDocument, string modGuid, string filename, Helpers helpers) {

        SlotItemBlueprint SIB = new SlotItemBlueprint {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value")),
            Display = new(xmlDocument.SelectSingleNode(".//group[@name='ui_info']") as XmlElement),
            SlotSize = (int)float.Parse((xmlDocument.SelectSingleNode(".//float[@name='slot_size']") as XmlElement)?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture),
            // Get weapon
            WPB = Path.GetFileNameWithoutExtension((xmlDocument.SelectSingleNode(".//instance_reference[@name='weapon']") as XmlElement).GetAttribute("value"))
        };

        if (string.IsNullOrEmpty(SIB.WPB)) {
            SIB.WPB = null;
        }

        return SIB;

    }

}
