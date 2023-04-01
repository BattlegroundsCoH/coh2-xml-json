using System.Globalization;
using System.Linq;
using System.Xml;
using CoH2XML2JSON.Blueprints.Relations;

namespace CoH2XML2JSON.Blueprints.DataEntry;

/// <summary>
/// Represents the cost of a blueprint, including manpower, munition, fuel, and field time.
/// </summary>
public sealed class Cost : Extendable<Cost> {

    /// <summary>
    /// Gets the manpower cost.
    /// </summary>
    public float Manpower {
        get => GetValue<float>();
        init => SetValue(value);
    }

    /// <summary>
    /// Gets the munition cost.
    /// </summary>
    public float Munition {
        get => GetValue<float>();
        init => SetValue(value);
    }

    /// <summary>
    /// Gets the fuel cost.
    /// </summary>
    public float Fuel {
        get => GetValue<float>();
        init => SetValue(value);
    }

    /// <summary>
    /// Gets the field time cost.
    /// </summary>
    public float FieldTime {
        get => GetValue<float>();
        init => SetValue(value);
    }

    /// <summary>
    /// Gets a value indicating whether this cost is null (all values are 0).
    /// </summary>
    public bool IsNull => Manpower + Munition + Fuel + FieldTime == 0.0;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cost"/> class with the sum of the specified costs.
    /// </summary>
    /// <param name="costs">The costs to sum.</param>
    public Cost(params Cost[] costs) {
        Manpower = costs.Sum(x => x.Manpower);
        Munition = costs.Sum(x => x.Munition);
        Fuel = costs.Sum(x => x.Fuel);
        FieldTime = costs.Sum(x => x.FieldTime);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cost"/> class with the specified costs.
    /// </summary>
    /// <param name="man">The manpower cost.</param>
    /// <param name="mun">The munition cost.</param>
    /// <param name="ful">The fuel cost.</param>
    /// <param name="fld">The field time cost.</param>
    public Cost(float man, float mun, float ful, float fld) {
        Manpower = man;
        Munition = mun;
        Fuel = ful;
        FieldTime = fld;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cost"/> class from the specified XML element.
    /// </summary>
    /// <param name="xmlElement">The XML element containing the cost data.</param>
    public Cost(XmlElement? xmlElement) {
        if (xmlElement is not null) {
            Manpower = float.Parse(xmlElement.FindSubnode("float", "manpower")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
            Munition = float.Parse(xmlElement.FindSubnode("float", "munition")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
            Fuel = float.Parse(xmlElement.FindSubnode("float", "fuel")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
            FieldTime = float.Parse(xmlElement.FindSubnode("float", "time_seconds")?.GetAttribute("value") ?? "0", CultureInfo.InvariantCulture);
        }
    }

}
