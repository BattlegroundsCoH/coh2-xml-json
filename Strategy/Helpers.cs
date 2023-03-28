using System.Collections.Generic;
using System.Linq;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Represents a collection of <see cref="IBlueprintHelperHandler"/> instances.
/// </summary>
public record struct Helpers(IList<IBlueprintHelperHandler> Handlers) {

    /// <summary>
    /// Returns a specific helper from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the helper to retrieve.</typeparam>
    /// <returns>The helper of the specified type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no helper of the specified type is found.</exception>
    public T GetHelper<T>() where T : IBlueprintHelperHandler {
        IBlueprintHelperHandler? helper = Handlers.FirstOrDefault(x => x is T);
        return helper is null ? throw new KeyNotFoundException() : (T)helper;
    }

}
