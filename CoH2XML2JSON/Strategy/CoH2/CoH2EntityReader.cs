using System.Collections.Generic;
using System.IO;
using System.Xml;

using CoH2XML2JSON.Blueprints;

using static CoH2XML2JSON.Blueprints.EntityBlueprint;

namespace CoH2XML2JSON.Strategy.CoH2;

public sealed class CoH2EntityReader : IBlueprintReader<EntityBlueprint> {
    
    /// <inheritdoc/>
    public EntityBlueprint FromXml(XmlDocument xmlDocument, string modGuid, string filename, Helpers helpers) {

        // Create EBP
        EntityBlueprint EBP = new() { 
            Name  = filename ,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value")),
            Display = new(xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\ui_ext']") as XmlElement),
            Cost = new(xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\cost_ext']") as XmlElement)
        };

        // Load Cost
        if (EBP.Cost.IsNull) {
            EBP.Cost = null;
        }

        // Load abilities
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\ability_ext']") is XmlElement abilities) {
            var nodes = abilities.SelectSubnodes("instance_reference", "ability");
            List<string> abps = new();
            foreach (XmlNode ability in nodes) {
                abps.Add(Path.GetFileNameWithoutExtension(ability.Attributes["value"].Value));
            }
            if (abps.Count > 0) {
                EBP.Abilities = abps.ToArray();
            }
        }

        // Load drivers (if any)
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\recrewable_ext']") is XmlElement recrewable) {
            List<EBPCrew> crews = new();
            var nodes = recrewable.SelectSubnodes("group", "race_data");
            foreach (XmlElement driver in nodes) {
                var armyNode = driver.FindSubnode("instance_reference", "ext_key");
                var driverNode = driver.FindSubnode("instance_reference", "driver_squad_blueprint");
                if (!string.IsNullOrEmpty(driverNode.GetAttribute("value"))) {
                    var captureNode = driver.FindSubnode("instance_reference", "capture_squad_blueprint");
                    crews.Add(new(armyNode.GetAttribute("value"),
                        Path.GetFileNameWithoutExtension(driverNode.GetAttribute("value")),
                        Path.GetFileNameWithoutExtension(captureNode.GetAttribute("value"))));
                }
            }
            if (crews.Count > 0) {
                EBP.Drivers = crews.ToArray();
            }
        }

        // Load hardpoints (if any)
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\combat_ext']") is XmlElement hardpoints) {
            var nodes = hardpoints.SelectNodes(".//instance_reference[@name='weapon']");
            List<string> wpbs = new();
            foreach (XmlNode wpn in nodes) {
                var hardpoint = wpn.Attributes["value"].Value;
                if (!string.IsNullOrEmpty(hardpoint)) {
                    wpbs.Add(Path.GetFileNameWithoutExtension(hardpoint));
                }
            }
            if (wpbs.Count > 0) {
                EBP.Hardpoints = wpbs.ToArray();
            }
        }

        // Get hitpoints (if any)
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\health_ext']") is XmlElement health) {
            EBP.Health = float.Parse(health.GetValue(".//float[@name='hitpoints']"));
        }

        // Load upgrades
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\upgrade_ext']") is XmlElement upgrades) {
            var nodes = upgrades.SelectSubnodes("instance_reference", "upgrade");
            List<string> ubps = new();
            foreach (XmlNode ability in nodes) {
                ubps.Add(Path.GetFileNameWithoutExtension(ability.Attributes["value"].Value));
            }
            if (ubps.Count > 0) {
                EBP.Upgrades = ubps.ToArray();
            }
            EBP.UpgradeCapacity = (int)float.Parse(upgrades.FindSubnode("float", "number_of_standard_slots").GetAttribute("value"));
        }

        // Load applied upgrades
        if (xmlDocument.SelectSingleNode(@".//template_reference[@name='exts'] [@value='ebpextensions\upgrade_apply_ext']") is XmlElement appliedUpgrades) {
            var nodes = appliedUpgrades.SelectSubnodes("instance_reference", "upgrade");
            List<string> ubps = new();
            foreach (XmlNode ability in nodes) {
                ubps.Add(Path.GetFileNameWithoutExtension(ability.Attributes["value"].Value));
            }
            if (ubps.Count > 0) {
                EBP.AppliedUpgrades = ubps.ToArray();
            }
        }

        // Return the constructed instance
        return EBP;

    }

}
