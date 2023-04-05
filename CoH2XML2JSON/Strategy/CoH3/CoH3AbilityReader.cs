using System;
using System.Xml;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy.CoH3;

public sealed class CoH3AbilityReader : IBlueprintReader<AbilityBlueprint> {
    
    public AbilityBlueprint? FromXml(XmlDocument xml, string modGuid, string filename, Helpers helpers) {

        // Get variant helper
        VariantSelector variant = helpers.GetHelper<VariantSelector>();

        // Pick
        var xmlDocument = variant.Select(xml);
        if (xmlDocument is null) {
            Console.WriteLine("Failed finding variant '{0}' for blueprint '{1}'.", variant.Variant, filename);
            return null;
        }

        // Create ability blueprint
        var ABP = new AbilityBlueprint() {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["uniqueid"]?.GetAttribute("value") ?? "0")
        };

        // Grab ability bag
        XmlElement? abilityBag = xmlDocument["group"];

        // Load cost
        ABP.Cost = new Cost(abilityBag?.SelectSingleNode(@".//enum_table[@name='cost_to_player']") as XmlElement);
        if (ABP.Cost.IsNull) {
            ABP.Cost = null;
        }

        // Load UI
        ABP.Display = new UI(abilityBag?.SelectSingleNode(@".//group[@name='ui_info']") as XmlElement);
        if (ABP.Display is not null) {
            ABP.Display.Position = UI.GetUIPosition(abilityBag?.SelectSingleNode(@".//group[@name='ui_position']") as XmlElement);
        }

        // Set activation
        ABP.Activation = (abilityBag?.SelectSingleNode(@".//enum[@name='activation']") as XmlElement)?.GetAttribute("value") ?? null;

        return ABP;

    }

}
