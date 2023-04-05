using System.Collections.Generic;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Interface for a strategy <see cref="IBlueprint"/> artifact listener.
/// </summary>
public interface IStrategyListener {

    /// <summary>
    /// Notifies the listener of new <typeparamref name="T"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of blueprints to notify about.</typeparam>
    /// <param name="blueprints">The blueprints to notify listener of.</param>
    void Notify<T>(IList<T> blueprints) where T : IBlueprint;

}
