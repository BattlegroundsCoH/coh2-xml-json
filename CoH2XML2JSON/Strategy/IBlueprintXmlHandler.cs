using System.Xml;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Interface for handling Blueprint objects in XML format.
/// </summary>
public interface IBlueprintXmlHandler : IBlueprintHandler {

    /// <summary>
    /// Handles an individual Blueprint object in XML format.
    /// </summary>
    /// <typeparam name="T">The type of Blueprint object to handle.</typeparam>
    /// <param name="xmlDoc">The XML document containing the Blueprint object.</param>
    /// <param name="blueprint">The Blueprint object to handle.</param>
    /// <param name="helpers">The Helpers object to use during handling.</param>
    /// <returns>The handled Blueprint object.</returns>
    T Handle<T>(XmlDocument xmlDoc, T blueprint, Helpers helpers) where T : IBlueprint;

}
