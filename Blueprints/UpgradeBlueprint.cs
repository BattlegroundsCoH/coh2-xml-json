using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using CoH2XML2JSON.Blueprint.DataEntry;

namespace CoH2XML2JSON.Blueprints;

public class UpgradeBlueprint : IBlueprint {

    public string ModGUID { get; }

    public ulong PBGID { get; }

    public string Name { get; }

    public UI Display { get; }

    public Cost Cost { get; }

    [DefaultValue(null)]
    public string OwnerType { get; }

    [DefaultValue(null)]
    public string[] SlotItems { get; }

    [DefaultValue(null)]
    public Requirement[] Requirements { get; }

    public UpgradeBlueprint(XmlDocument xmlDocument, string guid, string name) {

        // Set the name
        Name = name;

        // Set mod GUID
        ModGUID = string.IsNullOrEmpty(guid) ? null : guid;

        // Load pbgid
        PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value"));

        // Load display
        Display = new(xmlDocument.SelectSingleNode("//group[@name='ui_info']") as XmlElement);

        // Load cost
        Cost = new(xmlDocument.SelectSingleNode("//group[@name='time_cost']") as XmlElement);
        if (Cost.IsNull) {
            Cost = null;
        }

        // Get ownertype
        OwnerType = (xmlDocument.SelectSingleNode("//enum[@name='owner_type']") as XmlElement).GetAttribute("value");
        if (string.IsNullOrEmpty(OwnerType)) {
            OwnerType = null;
        }

        // Load slot items
        var slot_items = xmlDocument.SelectNodes("//instance_reference[@name='slot_item']");
        List<string> items = new();
        foreach (XmlElement item in slot_items) {
            items.Add(Path.GetFileNameWithoutExtension(item.GetAttribute("value")));
        }
        if (items.Count > 0) {
            SlotItems = items.ToArray();
        }

        // Load Requirements
        Requirements = Requirement.GetRequirements(xmlDocument.SelectSingleNode(@"//list[@name='requirements']") as XmlElement);

    }

}
