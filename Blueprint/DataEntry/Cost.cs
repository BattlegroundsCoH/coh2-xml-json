using System.Linq;
using System.Xml;

namespace CoH2XML2JSON.Blueprint.DataEntry;

public class Cost {

    public float Manpower { get; }

    public float Munition { get; }

    public float Fuel { get; }

    public float FieldTime { get; }

    public bool IsNull => Manpower + Munition + Fuel + FieldTime == 0.0;

    public Cost(params Cost[] costs) {
        Manpower = costs.Sum(x => x.Manpower);
        Munition = costs.Sum(x => x.Munition);
        Fuel = costs.Sum(x => x.Fuel);
        FieldTime = costs.Sum(x => x.FieldTime);
    }

    public Cost(float man, float mun, float ful, float fld) {
        Manpower = man;
        Munition = mun;
        Fuel = ful;
        FieldTime = fld;
    }

    public Cost(XmlElement xmlElement) {
        if (xmlElement is not null) {
            Manpower = Program.GetFloat(xmlElement.FindSubnode("float", "manpower").GetAttribute("value"));
            Munition = Program.GetFloat(xmlElement.FindSubnode("float", "munition").GetAttribute("value"));
            Fuel = Program.GetFloat(xmlElement.FindSubnode("float", "fuel").GetAttribute("value"));
            FieldTime = Program.GetFloat(xmlElement.FindSubnode("float", "time_seconds")?.GetAttribute("value") ?? "0");
        }
    }

}
