using System.Globalization;
using System.Xml;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.CoH2;

public sealed class CoH2WeaponReader : IBlueprintReader<WeaponBlueprint> {

    /// <inheritdoc/>
    public WeaponBlueprint FromXml(XmlDocument xmlDocument, string modGuid, string filename, Helpers helpers) {

        WeaponBlueprint WBP = new WeaponBlueprint() {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value")),
            Range = float.Parse((xmlDocument.SelectSingleNode(".//group[@name='range']") as XmlElement).FindSubnode("float", "max")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture),
            Damage = float.Parse((xmlDocument.FirstChild as XmlElement).FindSubnode("group", "damage").FindSubnode("float", "max")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture)
        };

        // Get additional reload data
        XmlElement reloadData = xmlDocument.SelectSingleNode(@".//group[@name='reload']") as XmlElement;
        XmlElement reloadFreqData = reloadData.FindSubnode("group", "frequency");
        XmlElement burstData = xmlDocument.SelectSingleNode(@".//group[@name='burst']") as XmlElement;
        XmlElement burstRateOfFireData = burstData.FindSubnode("group", "rate_of_fire");

        // Get the min number of shots before reloading
        int reloadAfterShots = (int)float.Parse(reloadFreqData.FindSubnode("float", "min").GetAttribute("value"), CultureInfo.InvariantCulture);
        if (reloadAfterShots < 1) {
            reloadAfterShots = 1;
        }

        // Get if can burst
        bool canBurst = bool.Parse(burstData.FindSubnode("bool", "can_burst").GetAttribute("value"));

        // Get burst data
        float burstMin = float.Parse(burstRateOfFireData.FindSubnode("float", "min").GetAttribute("value"), CultureInfo.InvariantCulture);
        float burstMax = float.Parse(burstRateOfFireData.FindSubnode("float", "max").GetAttribute("value"), CultureInfo.InvariantCulture);
        float rate_of_fire = (burstMin + burstMax) / 2.0f;

        // Set properties
        WBP.FireRate = canBurst ? rate_of_fire : 1;
        WBP.MagazineSize = canBurst ? (int)(reloadAfterShots * rate_of_fire) : reloadAfterShots;

        // Get callback type
        XmlElement fireData = xmlDocument.SelectSingleNode(@".//group[@name='fire']") as XmlElement;
        XmlElement onFireActions = fireData.FindSubnode("list", "on_fire_actions");
        foreach (XmlElement action in onFireActions) {
            if (action.Name == "template_reference" && action.GetAttribute("value") == "action\\scar_function_call") {
                if (action.FindSubnode("string", "function_name") is XmlElement func && func.GetAttribute("value").StartsWith("ScarEvent_")) {
                    WBP.CallbackType = func.GetAttribute("value");
                }
            }
        }

        return WBP;

    }

}
