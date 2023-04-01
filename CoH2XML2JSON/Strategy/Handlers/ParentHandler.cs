using System.Collections.Generic;
using System.Xml;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// Handles assigning parent blueprints to extendable blueprints during post-database and XML handling stages.
/// Implements the <see cref="IBlueprintPostDatabaseHandler"/> and <see cref="IBlueprintXmlHandler"/> interfaces.
/// </summary>
public sealed class ParentHandler : IBlueprintPostDatabaseHandler, IBlueprintXmlHandler {

    /// <summary>
    /// Handles assigning parent blueprint filepaths to an extendable blueprint.
    /// </summary>
    /// <typeparam name="T">The type of the blueprint.</typeparam>
    /// <param name="xmlDoc">The XML document to parse.</param>
    /// <param name="blueprint">The blueprint to modify.</param>
    /// <param name="helpers">The helper functions.</param>
    /// <returns>The modified blueprint.</returns>
    /// <remarks>
    /// Reads the <c>&lt;instance_reference name="parent_pbg" value="???"/&gt;</c> value from the XML document to determine the parent blueprint to assign.
    /// </remarks>
    public T Handle<T>(XmlDocument xmlDoc, T blueprint, Helpers helpers) where T : IBlueprint {
        if (blueprint is IExtendableBlueprint<T> extendable) {
            var xmlVariant = helpers.GetHelper<VariantSelector>().Select(xmlDoc);
            if (xmlVariant is not null && xmlVariant.SelectSingleNode(@"//instance_reference[@name='parent_pbg']") is XmlElement parentpbg) {
                extendable.ParentFilepath = string.IsNullOrEmpty(parentpbg.GetAttribute("value")) ? null : parentpbg.GetAttribute("value");
            }
        }
        return blueprint;
    }

    /// <summary>
    /// Handles assigning parent blueprints to extendable blueprints after XML handling.
    /// </summary>
    /// <typeparam name="T">The type of the blueprint.</typeparam>
    /// <param name="source">The list of blueprints to modify.</param>
    /// <returns>The modified list of blueprints.</returns>
    /// <remarks>
    /// Finds the parent blueprint for each extendable blueprint in the source list and assigns it based on the read XML value.
    /// </remarks>
    public IList<T> Handle<T>(IList<T> source) where T : IBlueprint {
        for (int i = 0; i < source.Count; i++) {
            if (source[i] is IExtendableBlueprint<T> extendable && !string.IsNullOrEmpty(extendable.ParentFilepath)) {
                // TODO: Find parent and assign based on the instance_reference value read by the Handle<T>(XmlDocument xmlDoc, T blueprint, Helpers helpers) function
            }
        }
        return source;
    }

}
