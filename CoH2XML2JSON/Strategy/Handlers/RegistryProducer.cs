using System.Collections.Generic;
using System.Collections.Immutable;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// A blueprint post handler that collects blueprints of a specific type <typeparamref name="TBlueprint"/> and makes them available through the <see cref="GetBlueprints"/> method.
/// </summary>
/// <typeparam name="TBlueprint">The type of blueprint to collect.</typeparam>
public class RegistryProducer<TBlueprint> : IBlueprintPostHandler where TBlueprint : IBlueprint {
    
    private readonly ICollection<TBlueprint> _blueprints;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegistryProducer{TBlueprint}"/> class with an empty collection of blueprints.
    /// </summary>
    public RegistryProducer() {
        _blueprints = new List<TBlueprint>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RegistryProducer{TBlueprint}"/> class with the specified collection of blueprints.
    /// </summary>
    /// <param name="blueprints">The collection of blueprints to use.</param>
    public RegistryProducer(ICollection<TBlueprint> blueprints) {
        _blueprints = blueprints;
    }

    /// <summary>
    /// Handles a blueprint of type <typeparamref name="TBlueprint"/> and adds it to the collection of blueprints.
    /// </summary>
    /// <typeparam name="T">The type of the blueprint.</typeparam>
    /// <param name="blueprint">The blueprint to handle.</param>
    /// <param name="filepath">The path to the file containing the blueprint.</param>
    /// <returns>The same blueprint that was passed in.</returns>
    public virtual T Handle<T>(T blueprint, string filepath) where T : IBlueprint {
        if (blueprint is TBlueprint bp) {
            _blueprints.Add(bp);
        }
        return blueprint;
    }

    /// <summary>
    /// Retrieves a read-only collection of blueprints of type <typeparamref name="TBlueprint"/>.
    /// </summary>
    /// <returns>A read-only collection of blueprints of type <typeparamref name="TBlueprint"/>.</returns>
    public IReadOnlyCollection<TBlueprint> GetBlueprints() => _blueprints.ToImmutableList();

    /// <summary>
    /// Creates a new instance of <see cref="RegistryConsumer{T}"/> that consumes the blueprints produced by this <see cref="RegistryProducer{TBlueprint}"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="RegistryConsumer{T}"/> for this <see cref="RegistryProducer{TBlueprint}"/>.</returns>
    public RegistryConsumer<TBlueprint> CreateConsumer() => new RegistryConsumer<TBlueprint>(this);

}
