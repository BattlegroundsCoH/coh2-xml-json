using System.Collections.Generic;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// A consumer that retrieves blueprint data from a <see cref="RegistryProducer{T}"/> and exposes it as a read-only collection.
/// </summary>
/// <typeparam name="T">The type of the blueprint.</typeparam>
public sealed class RegistryConsumer<T> : IBlueprintHelperHandler where T : IBlueprint {

    private readonly RegistryProducer<T> _producer;

    /// <summary>
    /// Gets the read-only collection of blueprints from the registry producer.
    /// </summary>
    public IReadOnlyCollection<T> Registry => _producer.GetBlueprints();

    /// <summary>
    /// Initializes a new instance of the <see cref="RegistryConsumer{T}"/> class with the specified <see cref="RegistryProducer{T}"/>.
    /// </summary>
    /// <param name="producer">The registry producer to consume data from.</param>
    public RegistryConsumer(RegistryProducer<T> producer) {
        this._producer = producer;
    }

}
