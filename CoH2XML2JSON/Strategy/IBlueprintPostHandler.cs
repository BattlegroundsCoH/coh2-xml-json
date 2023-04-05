using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Represents a handler for post-processing <see cref="IBlueprint"/> objects after they have been read from an input source.
/// </summary>
public interface IBlueprintPostHandler : IBlueprintHandler {

    /// <summary>
    /// Handles a single <typeparamref name="T"/> blueprint object after it has been read from an input source.
    /// </summary>
    /// <typeparam name="T">The type of the blueprint to handle.</typeparam>
    /// <param name="blueprint">The blueprint object to handle.</param>
    /// <param name="filepath">The path of the file from which the blueprint was read.</param>
    /// <returns>The blueprint object, possibly modified or replaced by the handler.</returns>
    public T Handle<T>(T blueprint, string filepath) where T : IBlueprint;

}
