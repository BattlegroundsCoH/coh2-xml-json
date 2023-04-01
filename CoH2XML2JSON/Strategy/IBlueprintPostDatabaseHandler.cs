using System.Collections.Generic;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Defines a handler for post-database processing of blueprints.
/// </summary>
public interface IBlueprintPostDatabaseHandler : IBlueprintHandler {

    /// <summary>
    /// Handles post-database processing of a list of blueprints.
    /// </summary>
    /// <typeparam name="T">The type of blueprint to handle.</typeparam>
    /// <param name="source">The list of blueprints to handle.</param>
    /// <returns>The list of blueprints after post-database processing.</returns>
    IList<T> Handle<T>(IList<T> source) where T : IBlueprint;

}
