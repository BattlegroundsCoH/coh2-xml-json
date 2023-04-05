using System;
using System.Xml;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy.CoH3;

public sealed class CoH3UpgradeReader : IBlueprintReader<UpgradeBlueprint> {
    
    public UpgradeBlueprint? FromXml(XmlDocument xml, string modGuid, string filename, Helpers helpers) {

        // Get variant helper
        VariantSelector variant = helpers.GetHelper<VariantSelector>();

        // Pick
        var xmlDocument = variant.Select(xml);
        if (xmlDocument is null) {
            Console.WriteLine("Failed finding variant '{0}' for blueprint '{1}'.", variant.Variant, filename);
            return null;
        }

        UpgradeBlueprint UBP = new UpgradeBlueprint() {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["uniqueid"]?.GetAttribute("value") ?? "0"),
            Display = new(xmlDocument.SelectSingleNode(".//group[@name='ui_info']") as XmlElement),
            Cost = new(xmlDocument.SelectSingleNode(".//group[@name='time_cost']") as XmlElement),
            OwnerType = (xmlDocument.SelectSingleNode(".//enum[@name='owner_type']") as XmlElement)?.GetAttribute("value") ?? null
        };

        // Load cost
        if (UBP.Cost.IsNull) {
            UBP.Cost = null;
        }

        // Get ownertype
        if (string.IsNullOrEmpty(UBP.OwnerType)) {
            UBP.OwnerType = null;
        }

        // Return created UBP
        return UBP;

    }

}
