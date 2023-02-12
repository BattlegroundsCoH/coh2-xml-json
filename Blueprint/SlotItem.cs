using System.ComponentModel;
using System.IO;
using System.Xml;
using CoH2XML2JSON.Blueprint.DataEntry;

namespace CoH2XML2JSON.Blueprint;

public class SlotItem : BP {

    public override string ModGUID { get; }

    public override ulong PBGID { get; }

    public override string Name { get; }

    public string Army { get; init; }

    public UI Display { get; }

    [DefaultValue(null)]
    public string WPB { get; }

    [DefaultValue(0)]
    public int SlotSize { get; }

    public SlotItem(XmlDocument xmlDocument, string guid, string name) {

        // Set the name
        Name = name;

        // Set mod GUID
        ModGUID = string.IsNullOrEmpty(guid) ? null : guid;

        // Load pbgid
        PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value"));

        // Load display
        Display = new(xmlDocument.SelectSingleNode("//group[@name='ui_info']") as XmlElement);

        // Get slot size
        SlotSize = (int)Program.GetFloat((xmlDocument.SelectSingleNode("//float[@name='slot_size']") as XmlElement)?.GetAttribute("value") ?? "0");

        // Get weapon
        WPB = Path.GetFileNameWithoutExtension((xmlDocument.SelectSingleNode("//instance_reference[@name='weapon']") as XmlElement).GetAttribute("value"));
        if (string.IsNullOrEmpty(WPB)) {
            WPB = null;
        }

    }

}
