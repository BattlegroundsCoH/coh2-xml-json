using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    /// <summary>
    /// Returns a specific helper from the collection and if not found, returns a <typeparamref name="THelperImpl"/> instance.
    /// </summary>
    /// <typeparam name="THelper">The type of the helper to retrieve.</typeparam>
    /// <typeparam name="THelperImpl">The type of the helper to return if <typeparamref name="THelper"/> cannot be returned.</typeparam>
    /// <returns>The helper of the specified type.</returns>
    /// <exception cref="InvalidProgramException"></exception>
    [SuppressMessage("Trimming", "IL2087:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The generic parameter of the source method or type does not have matching annotations.", Justification = "<Pending>")]
    public THelper GetHelper<THelper, THelperImpl>() where THelper : IBlueprintHelperHandler where THelperImpl : THelper {
        if (Handlers.FirstOrDefault(x => x is THelper) is THelper helper) {
            return helper;
        }
        return Activator.CreateInstance(typeof(THelperImpl)) is not THelperImpl impl ? throw new InvalidProgramException() : impl;
    }

}
