using System.Xml;

using CoH2XML2JSON.Blueprint.DataEntry;
using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.CoH2;

public class CoH2AbilityReader : IBlueprintReader<AbilityBlueprint> {
    
    public AbilityBlueprint FromXml(XmlDocument xmlDocument, string modGuid, string filename) {

        // Set the name
        var name = filename;

        // Set mod GUID
        var modGUID = string.IsNullOrEmpty(modGuid) ? null : modGuid;

        // Load pbgid
        var pBGID = ulong.Parse(xmlDocument["instance"]["uniqueid"].GetAttribute("value"));

        // Load display
        var display = new UI(xmlDocument.SelectSingleNode(@"//group[@name='ui_info']") as XmlElement);

        // Load UI position
        if (int.TryParse((xmlDocument.SelectSingleNode(@"//int[@name='ui_position']") as XmlElement).GetAttribute("value"), out int ui_pos)) {
            display.Position = ui_pos;
        }

        // Load Cost
        var cost = new Cost(xmlDocument.SelectSingleNode(@"//group[@name='cost']") as XmlElement);
        if (cost.IsNull) {
            cost = null;
        }

        // Get activation
        var activation = (xmlDocument.SelectSingleNode(@"//enum[@name='activation']") as XmlElement).GetAttribute("value");

        // Load Requirements
        var requirements = Requirement.GetRequirements(xmlDocument.SelectSingleNode(@"//list[@name='requirements']") as XmlElement);

        // Create ability
        return new AbilityBlueprint() {
            Name = name,
            ModGUID = modGUID,
            PBGID = pBGID,
            Display = display,
            Cost = cost,
            Activation = activation,
            Requirements = requirements,
        };

    }

}
