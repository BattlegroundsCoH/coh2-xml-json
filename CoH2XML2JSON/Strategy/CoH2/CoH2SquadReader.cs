using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

using CoH2XML2JSON.Blueprints.DataEntry;
using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.Handlers;

using static CoH2XML2JSON.Blueprints.SquadBlueprint;

namespace CoH2XML2JSON.Strategy.CoH2;

public sealed class CoH2SquadReader : IBlueprintReader<SquadBlueprint> {

    /// <inheritdoc/>
    public SquadBlueprint FromXml(XmlDocument xmlDocument, string modGuid, string filename, Helpers helpers) {

        // Get helper
        RegistryConsumer<EntityBlueprint> entityConsumer = helpers.GetHelper<RegistryConsumer<EntityBlueprint>>();

        SquadBlueprint SBP = new SquadBlueprint() { 
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value")),
            Display = new(xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_ui_ext']") as XmlElement)
        };

        // Load squad types
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_type_ext']") is XmlNode squadTypeList) {
            var typeList = squadTypeList.SelectSingleNode(@".//list[@name='squad_type_list']");
            var tmpList = new List<string>();
            foreach (XmlNode type in typeList) {
                tmpList.Add(type.Attributes["value"].Value);
            }
            SBP.Types = tmpList.ToArray();
        } else {
            SBP.Types = Array.Empty<string>();
        }

        // Load squad loadout
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_loadout_ext']") is XmlElement squadLoadout) {
            var nodes = squadLoadout.SelectNodes(".//group[@name='loadout_data']");
            List<Cost> costList = new();
            List<Loadout> squadLoadoutData = new();
            List<EntityBlueprint> tmpEbpCollect = new();
            foreach (XmlElement loadout_data in nodes) {
                int num = int.Parse(loadout_data.FindSubnode("float", "num").GetAttribute("value"));
                string entity = loadout_data.FindSubnode("instance_reference", "type").GetAttribute("value");
                EntityBlueprint ebp = entityConsumer.Registry.FirstOrDefault(x => entity.EndsWith(x.Name));
                tmpEbpCollect.Add(ebp);
                Cost[] dups = new Cost[num];
                Array.Fill(dups, ebp?.Cost ?? new Cost(0, 0, 0, 0));
                costList.AddRange(dups);
                squadLoadoutData.Add(new(ebp?.Name ?? Path.GetFileNameWithoutExtension(entity), num));
            }
            SBP.SquadCost = new(costList.ToArray());
            if (SBP.SquadCost.IsNull) {
                SBP.SquadCost = null;
            }
            SBP.Entities = squadLoadoutData.ToArray();
            var temp = squadLoadout.GetValue(".//float [@name='squad_female_chance']");
            SBP.FemaleChance = float.Parse(squadLoadout.GetValue(".//float [@name='squad_female_chance']"), CultureInfo.InvariantCulture) / 10.0f;
            SBP.HasCrew = tmpEbpCollect.Any(x => x?.Drivers?.Length > 0);
        }

        // Load squad abilities
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_ability_ext']") is XmlElement squadAbilities) {
            var nodes = squadAbilities.SelectSubnodes("instance_reference", "ability");
            List<string> abps = new();
            foreach (XmlNode ability in nodes) {
                abps.Add(Path.GetFileNameWithoutExtension(ability.Attributes["value"].Value));
            }
            if (abps.Count > 0) {
                SBP.Abilities = abps.ToArray();
            }
        }

        // Load squad veterancy
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_veterancy_ext']") is XmlElement squadVet) {
            var ranks = squadVet.SelectNodes(".//group[@name='veterancy_rank']");
            var ranks_data = new List<VeterancyLevel>();
            foreach (XmlElement rank in ranks) {
                ranks_data.Add(new(
                    rank.FindSubnode("locstring", "screen_name").GetAttribute("value"),
                    (float)double.Parse(rank.FindSubnode("float", "experience_value").GetAttribute("value"))
                    ));
            }
            SBP.Veterancy = ranks_data.ToArray();
        }

        // Load pickup data
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_item_slot_ext']") is XmlElement squadItmes) {
            SBP.CanPickupItems = squadItmes.FindSubnode("bool", "can_pick_up").GetAttribute("value") == bool.TrueString;
            SBP.SlotPickupCapacity = int.Parse(squadItmes.FindSubnode("float", "num_slots").GetAttribute("value"));
        }

        // Determine if syncweapon (has team_weapon extension)
        SBP.IsSyncWeapon = xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_team_weapon_ext']") is not null;

        // Load squad upgrades
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_upgrade_ext']") is XmlElement squadUpgrades) {
            var nodes = squadUpgrades.SelectSubnodes("instance_reference", "upgrade");
            List<string> ubps = new();
            foreach (XmlNode upgrade in nodes) {
                ubps.Add(Path.GetFileNameWithoutExtension(upgrade.Attributes["value"].Value));
            }
            if (ubps.Count > 0) {
                SBP.Upgrades = ubps.ToArray();
            }
            SBP.UpgradeCapacity = (int)float.Parse(squadUpgrades.FindSubnode("float", "number_of_slots").GetAttribute("value"));
        }

        // Load squad pre-applied upgrades
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='squadexts'] [@value='sbpextensions\squad_upgrade_apply_ext']") is XmlElement squadAppliedUpgrades) {
            var nodes = squadAppliedUpgrades.SelectSubnodes("instance_reference", "upgrade");
            List<string> ubps = new();
            foreach (XmlNode upgrade in nodes) {
                ubps.Add(Path.GetFileNameWithoutExtension(upgrade.Attributes["value"].Value));
            }
            if (ubps.Count > 0) {
                SBP.AppliedUpgrades = ubps.ToArray();
            }
        }

        return SBP;

    }

}
