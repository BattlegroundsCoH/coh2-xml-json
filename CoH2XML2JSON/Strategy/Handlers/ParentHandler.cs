using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Blueprints.Relations;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// Handles assigning parent blueprints to extendable blueprints during post-database and XML handling stages.
/// Implements the <see cref="IBlueprintPostDatabaseHandler"/> and <see cref="IBlueprintXmlHandler"/> interfaces.
/// </summary>
public sealed class ParentHandler : IBlueprintPostDatabaseHandler, IBlueprintXmlHandler {

    private readonly IPathHandler pathHandler;

    public ParentHandler(IPathHandler pathHandler) {
        this.pathHandler = pathHandler;
    }

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
                if ((string.IsNullOrEmpty(parentpbg.GetAttribute("value")) ? null : parentpbg.GetAttribute("value")) is string parentPath) {
                    extendable.ParentFilepath = pathHandler.GetNameFromPath(parentPath);
                }
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
        Dictionary<string, T> common = new Dictionary<string, T>();
        for (int i = 0; i < source.Count; i++) {
            if (source[i] is BaseBlueprint<T> extendable && !string.IsNullOrEmpty(extendable.ParentFilepath)) {
                if (common.TryGetValue(extendable.ParentFilepath, out T? parent)) {
                    extendable.ExtendWith(parent);
                } else if (source.FirstOrDefault(x => x.Name == extendable.ParentFilepath) is T foundParent) {
                    extendable.ExtendWith(common[extendable.ParentFilepath] = foundParent);
                } else {
                    Console.WriteLine("Failed finding parent blueprint '' to assign to ''");
                }
            }
        }
        return source;
    }

}
