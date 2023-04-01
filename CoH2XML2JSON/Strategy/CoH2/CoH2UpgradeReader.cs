using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.CoH2;

public class CoH2UpgradeReader : IBlueprintReader<UpgradeBlueprint> {

    /// <inheritdoc/>
    public UpgradeBlueprint FromXml(XmlDocument xmlDocument, string modGuid, string filename, Helpers helpers) {

        UpgradeBlueprint UBP = new UpgradeBlueprint() {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value")),
            Display = new(xmlDocument.SelectSingleNode("//group[@name='ui_info']") as XmlElement),
            Cost = new(xmlDocument.SelectSingleNode("//group[@name='time_cost']") as XmlElement)
        };

        // Load cost
        if (UBP.Cost.IsNull) {
            UBP.Cost = null;
        }

        // Get ownertype
        UBP.OwnerType = (xmlDocument.SelectSingleNode("//enum[@name='owner_type']") as XmlElement).GetAttribute("value");
        if (string.IsNullOrEmpty(UBP.OwnerType)) {
            UBP.OwnerType = null;
        }

        // Load slot items
        var slot_items = xmlDocument.SelectNodes("//instance_reference[@name='slot_item']");
        List<string> items = new();
        foreach (XmlElement item in slot_items) {
            items.Add(Path.GetFileNameWithoutExtension(item.GetAttribute("value")));
        }
        if (items.Count > 0) {
            UBP.SlotItems = items.ToArray();
        }

        // Load Requirements
        UBP.Requirements = Requirement.GetRequirements(xmlDocument.SelectSingleNode(@"//list[@name='requirements']") as XmlElement);

        // Return created UBP
        return UBP;

    }

}
