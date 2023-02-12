using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using CoH2XML2JSON.Blueprint.DataEntry;

namespace CoH2XML2JSON.Blueprint;

public class EBP : BP {

    public readonly struct EBPCrew {
        public string Army { get; }
        public string SBP { get; }
        public string Capture { get; }
        public EBPCrew(string army, string sbp, string capture) {
            Army = army;
            SBP = sbp;
            Capture = capture;
        }
    }

    public override string ModGUID { get; }

    public override ulong PBGID { get; }

    public override string Name { get; }

    public string Army { get; init; }

    [DefaultValue(null)]
    public UI Display { get; }

    public Cost Cost { get; }

    [DefaultValue(null)]
    public string[] Abilities { get; }

    [DefaultValue(null)]
    public EBPCrew[] Drivers { get; }

    [DefaultValue(null)]
    public string[] Hardpoints { get; }

    [DefaultValue(false)]
    public bool HasRecrewableExtension { get; }

    [DefaultValue(0.0f)]
    public float Health { get; }

    [DefaultValue(null)]
    public string[] Upgrades { get; }

    [DefaultValue(null)]
    public string[] AppliedUpgrades { get; }

    [DefaultValue(0)]
    public int UpgradeCapacity { get; }

    public EBP(XmlDocument xmlDocument, string guid, string name) {

        // Set the name
        Name = name;

        // Set mod GUID
        ModGUID = string.IsNullOrEmpty(guid) ? null : guid;

        // Load pbgid
        PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value"));

        // Load display
        Display = new(xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\ui_ext']") as XmlElement);

        // Load Cost
        Cost = new(xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\cost_ext']") as XmlElement);
        if (Cost.IsNull) {
            Cost = null;
        }

        // Load abilities
        if (xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\ability_ext']") is XmlElement abilities) {
            var nodes = abilities.SelectSubnodes("instance_reference", "ability");
            List<string> abps = new();
            foreach (XmlNode ability in nodes) {
                abps.Add(Path.GetFileNameWithoutExtension(ability.Attributes["value"].Value));
            }
            if (abps.Count > 0) {
                Abilities = abps.ToArray();
            }
        }

        // Load drivers (if any)
        if (xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\recrewable_ext']") is XmlElement recrewable) {
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
                Drivers = crews.ToArray();
            }
        }

        // Load hardpoints (if any)
        if (xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\combat_ext']") is XmlElement hardpoints) {
            var nodes = hardpoints.SelectNodes("//instance_reference[@name='weapon']");
            List<string> wpbs = new();
            foreach (XmlNode wpn in nodes) {
                var hardpoint = wpn.Attributes["value"].Value;
                if (!string.IsNullOrEmpty(hardpoint)) {
                    wpbs.Add(Path.GetFileNameWithoutExtension(hardpoint));
                }
            }
            if (wpbs.Count > 0) {
                Hardpoints = wpbs.ToArray();
            }
        }

        // Get hitpoints (if any)
        if (xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\health_ext']") is XmlElement health) {
            Health = float.Parse(health.GetValue("//float[@name='hitpoints']"));
        }


        // Load upgrades
        if (xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\upgrade_ext']") is XmlElement upgrades) {
            var nodes = upgrades.SelectSubnodes("instance_reference", "upgrade");
            List<string> ubps = new();
            foreach (XmlNode ability in nodes) {
                ubps.Add(Path.GetFileNameWithoutExtension(ability.Attributes["value"].Value));
            }
            if (ubps.Count > 0) {
                Upgrades = ubps.ToArray();
            }
            UpgradeCapacity = (int)float.Parse(upgrades.FindSubnode("float", "number_of_standard_slots").GetAttribute("value"));
        }


        // Load applied upgrades
        if (xmlDocument.SelectSingleNode(@"//template_reference[@name='exts'] [@value='ebpextensions\upgrade_apply_ext']") is XmlElement appliedUpgrades) {
            var nodes = appliedUpgrades.SelectSubnodes("instance_reference", "upgrade");
            List<string> ubps = new();
            foreach (XmlNode ability in nodes) {
                ubps.Add(Path.GetFileNameWithoutExtension(ability.Attributes["value"].Value));
            }
            if (ubps.Count > 0) {
                AppliedUpgrades = ubps.ToArray();
            }
        }

    }

}
