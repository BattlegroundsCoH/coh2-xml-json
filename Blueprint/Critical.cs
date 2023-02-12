using System.Xml;
using CoH2XML2JSON.Blueprint.DataEntry;

namespace CoH2XML2JSON.Blueprint;

public class Critical : BP {

    public override string ModGUID { get; }

    public override ulong PBGID { get; }

    public override string Name { get; }

    public UI Display { get; }

    public Critical(XmlDocument xmlDocument, string guid, string name) {

        // Set the name
        Name = name;

        // Set mod GUID
        ModGUID = string.IsNullOrEmpty(guid) ? null : guid;

        // Load pbgid
        PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value"));

        // Load display
        Display = new(xmlDocument.SelectSingleNode(@"//template_reference[@name='ui_info']") as XmlElement);

    }

}
