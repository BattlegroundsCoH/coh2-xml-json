using System;
using System.Globalization;
using System.Linq;
using System.Xml;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy.CoH3;

public sealed class CoH3EntityReader : IBlueprintReader<EntityBlueprint> {
    
    public EntityBlueprint? FromXml(XmlDocument xml, string modGuid, string filename, Helpers helpers) {

        // Get variant helper
        VariantSelector variant = helpers.GetHelper<VariantSelector>();

        // Get path helper
        IPathHandler pathHandler = helpers.GetHelper<IPathHandler>();

        // Pick
        var xmlDocument = variant.Select(xml);
        if (xmlDocument is null) {
            Console.WriteLine("Failed finding variant '{0}' for blueprint '{1}'.", variant.Variant, filename);
            return null;
        }

        // Create EBP
        EntityBlueprint EBP = new EntityBlueprint() {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["uniqueid"]?.GetAttribute("value") ?? "0"),
            Cost = new Cost(xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\cost_ext']") as XmlElement),
            Display = new UI(xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\ui_ext']") as XmlElement)
        };

        // Nullify cost
        if (EBP.Cost.IsNull) {
            EBP.Cost = null;
        }

        // Collect abilities
        EBP.Abilities = xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\ability_ext']")
            ?.SelectNodes(@".//instance_reference[@name='ability']")?.MapTo(x => pathHandler.GetNameFromPath(x.GetAttribute("value"))) ?? null;

        // Collect hardpoints
        EBP.Hardpoints = xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\combat_ext']")
            ?.SelectNodes(@".//group[@name='weapon']")?.MapTo(x =>
                 pathHandler.GetNameFromPath(((x.SelectSingleNode(@".//instance_reference[@name='ebp']") as XmlElement)?.GetAttribute("value")) ?? string.Empty))
            .Where(x => !string.IsNullOrEmpty(x)).ToArray() ?? null;

        // Collect upgrades
        EBP.Upgrades = xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\upgrade_ext']")
            ?.SelectNodes(@".//instance_reference[@name='upgrade']")?.MapTo(x => pathHandler.GetNameFromPath(x.GetAttribute("value"))) ?? null;

        // Collect upgrade capacity
        EBP.UpgradeCapacity = int.Parse((xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\entity_inventory_ext']")
            ?.SelectSingleNode(@".//int[@name='upgrade']") as XmlElement)?.GetAttribute("value") ?? "0");

        // Collect types
        EBP.Types = xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\type_ext']")
            ?.SelectNodes(@".//enum[@name='unit_type']")?.MapTo(x => x.GetAttribute("value")) ?? null;
        
        // Get hitpoints (if any)
        if (xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\health_ext']") is XmlElement health) {
            var val = (health.SelectSingleNode(@".//float[@name='hitpoints']") as XmlElement)?.GetAttribute("value") ?? "0";
            EBP.Health = float.Parse(string.IsNullOrEmpty(val) ? "0" : val, CultureInfo.InvariantCulture);
        }

        // Get if inventory item
        if (xmlDocument?.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\sim_inventory_item_ext']") is XmlElement inventoryItem) {
            EBP.IsInventoryItem = true;
            EBP.InventoryRequiredCapacity = int.Parse((inventoryItem.SelectSingleNode(@".//int[@name='capacity_required']") as XmlElement)?.GetAttribute("value") ?? "0");
            EBP.InventoryDropOnDeathChance = float.Parse(
                (inventoryItem.SelectSingleNode(@".//float[@name='drop_on_death_chance']") as XmlElement)?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
        }

        // Return entities
        return EBP;

    }

}
