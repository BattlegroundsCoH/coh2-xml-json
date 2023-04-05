using System;
using System.Globalization;
using System.Xml;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy.CoH3;

public sealed class CoH3WeaponReader : IBlueprintReader<WeaponBlueprint> {
    
    public WeaponBlueprint? FromXml(XmlDocument xml, string modGuid, string filename, Helpers helpers) {

        // Get variant helper
        VariantSelector variant = helpers.GetHelper<VariantSelector>();

        // Pick
        var xmlDocument = variant.Select(xml);
        if (xmlDocument is null) {
            Console.WriteLine("Failed finding variant '{0}' for blueprint '{1}'.", variant.Variant, filename);
            return null;
        }

        WeaponBlueprint WBP = new WeaponBlueprint() {
            Name = filename,
            ModGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid,
            PBGID = ulong.Parse(xmlDocument["uniqueid"]?.GetAttribute("value") ?? "0"),
            Range = float.Parse((xmlDocument.SelectSingleNode(".//group[@name='range']") as XmlElement)?.FindSubnode("float", "max")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture),
            Damage = float.Parse((xmlDocument.FirstChild as XmlElement)?.FindSubnode("group", "damage")?.FindSubnode("float", "max")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture)
        };


        // Get additional reload data
        XmlElement? reloadFreqData = (xmlDocument.SelectSingleNode(@".//group[@name='reload']") as XmlElement)?.FindSubnode("group", "frequency");
        XmlElement? burstData = xmlDocument.SelectSingleNode(@".//group[@name='burst']") as XmlElement;
        XmlElement? burstRateOfFireData = burstData?.FindSubnode("group", "rate_of_fire");

        // Get the min number of shots before reloading
        int reloadAfterShots = (int)float.Parse(reloadFreqData?.FindSubnode("float", "min")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
        if (reloadAfterShots < 1) {
            reloadAfterShots = 1;
        }

        // Get if can burst
        bool canBurst = bool.Parse(burstData?.FindSubnode("bool", "can_burst")?.GetAttribute("value") ?? "False");

        // Get burst data
        float burstMin = float.Parse(burstRateOfFireData?.FindSubnode("float", "min")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
        float burstMax = float.Parse(burstRateOfFireData?.FindSubnode("float", "max")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
        float rate_of_fire = (burstMin + burstMax) / 2.0f;

        // Set properties
        WBP.FireRate = canBurst ? rate_of_fire : 1;
        WBP.MagazineSize = canBurst ? (int)(reloadAfterShots * rate_of_fire) : reloadAfterShots;

        return WBP;

    }

}
