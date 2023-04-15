using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy.CoH3;

public sealed class CoH3SquadReader : IBlueprintReader<SquadBlueprint> {

    public SquadBlueprint? FromXml(XmlDocument xml, string modGuid, string filename, Helpers helpers) {

        // Get name helper
        ScarPathHandler pathHandler = helpers.GetHelper<ScarPathHandler>();

        // Get variant helper
        VariantSelector variant = helpers.GetHelper<VariantSelector>();

        // Pick
        var xmlDocument = variant.Select(xml);
        if (xmlDocument is null) {
            Console.WriteLine("Failed finding variant '{0}' for blueprint '{1}'.", variant.Variant, filename);
            return null;
        }

        // Get registered entities
        RegistryConsumer<EntityBlueprint> entityConsumer = helpers.GetHelper<RegistryConsumer<EntityBlueprint>>();

        // Create SBP
        SquadBlueprint SBP = new SquadBlueprint() {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["uniqueid"]?.GetAttribute("value") ?? "0"),
            Display = new UI(xmlDocument?.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_ui_ext']") as XmlElement),
            Abilities = xmlDocument?.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_ability_ext']")
                ?.SelectNodes(@".//instance_reference[@name='ability']")?.MapTo(x => pathHandler.GetNameFromPath(x.GetAttribute("value"))) ?? null,
            Upgrades = xmlDocument?.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_upgrade_ext']")
                ?.SelectNodes(@".//instance_reference[@name='upgrade']")?.MapTo(x => pathHandler.GetNameFromPath(x.GetAttribute("value"))) ?? null,
            Types = xmlDocument?.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_type_ext']")
                ?.SelectNodes(@".//enum[@name='squad_type']")?.MapTo(x => x.GetAttribute("value")) ?? Array.Empty<string>(),
            IsSyncWeapon = (xmlDocument?.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_team_weapon_ext']") ?? null) is not null
        };

        // Grab the loadout
        var loadoutEntities = new List<EntityBlueprint>();
        SBP.Entities = xmlDocument?.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_loadout_ext']")
            ?.SelectNodes(@".//group[@name='loadout_data']")?.MapTo(x => {
                int num = (int)float.Parse((x.SelectSingleNode(@".//float[@name='num']") as XmlElement)?.GetAttribute("value") ?? "1", CultureInfo.InvariantCulture);
                string ebpFilename = pathHandler.GetNameFromPath((x.SelectSingleNode(@".//instance_reference[@name='type']") as XmlElement)?.GetAttribute("value") ?? string.Empty);
                if (entityConsumer.Registry.FirstOrDefault(x => x.Name == ebpFilename) is EntityBlueprint ebp) {
                    loadoutEntities.Add(ebp);
                }
                return new SquadBlueprint.Loadout(ebpFilename, num);
            }) ?? Array.Empty<SquadBlueprint.Loadout>();

        // Set cost
        SBP.SquadCost = new Cost(loadoutEntities.Select(x => x.Cost).ToArray() ?? Array.Empty<Cost>());
        if (SBP.SquadCost.IsNull) {
            SBP.SquadCost = null;
        }

        // Set veterancy
        SBP.Veterancy = xmlDocument!.SelectSingleNode(@"//template_reference [@name='squadexts'] [@value='sbpextensions\squad_veterancy_ext']")
            ?.SelectNodes(@"//group [@name='veterancy_rank']")?.MapTo(x => {
                string name = (x.SelectSingleNode(@".//locstring [@name='screen_name']") as XmlElement)?.GetAttribute("value") ?? string.Empty;
                float experience = float.Parse((x.SelectSingleNode(@".//float [@name='veterancy_value']") as XmlElement)?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
                return new SquadBlueprint.VeterancyLevel(name, experience);
            }) ?? Array.Empty<SquadBlueprint.VeterancyLevel>();

        // Determine inventory data
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_inventory_ext']") is XmlElement inventory) {
            SBP.CanPickupItems = bool.Parse((inventory.SelectSingleNode(@".//bool[@name='can_pick_up_items']") as XmlElement)?.GetAttribute("value") ?? "False");
            SBP.SlotPickupCapacity = int.Parse((inventory.SelectSingleNode(@".//int[@name='default']") as XmlElement)?.GetAttribute("value") ?? "-1");
            SBP.UpgradeCapacity = int.Parse((inventory.SelectSingleNode(@".//int[@name='upgrade']") as XmlElement)?.GetAttribute("value") ?? "-1");
        }

        // Return SBP
        return SBP;

    }

}
