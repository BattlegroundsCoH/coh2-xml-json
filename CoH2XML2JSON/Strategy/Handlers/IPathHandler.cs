namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// Defines a handler that translates the path of a blueprint XML file into its uniquely defined name used by the scripting system to identify the blueprint.
/// </summary>
public interface IPathHandler : IBlueprintHelperHandler {

    /// <summary>
    /// Translate the path into a unique name.
    /// </summary>
    /// <param name="path">The relative path of the blueprint file.</param>
    /// <returns>The unique name identifying the blueprint.</returns>
    string GetNameFromPath(string path);

}
