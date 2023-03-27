using System.Xml;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Provides functionality to deserialize an XML document into an object of type T that implements IBlueprint.
/// </summary>
/// <typeparam name="T">The type of the object to deserialize.</typeparam>
public interface IBlueprintReader<T> where T : IBlueprint {

    /// <summary>
    /// Deserializes an XML document into an object of type T that implements IBlueprint.
    /// </summary>
    /// <param name="xml">The XML document to deserialize.</param>
    /// <param name="modGuid">The mod GUID that the XML document belongs to.</param>
    /// <param name="filename">The name of the XML document.</param>
    /// <returns>An object of type T that represents the deserialized XML document.</returns>
    T FromXml(XmlDocument xml, string modGuid, string filename); 

}
