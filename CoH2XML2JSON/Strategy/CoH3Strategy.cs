using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.CoH3;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Class implementing the <see cref="IGameStrategy"/> interface for the CoH3 reading strategy.
/// </summary>
public sealed class CoH3Strategy : IGameStrategy {

    private readonly IBlueprintReader<AbilityBlueprint> abilityReader = new CoH3AbilityReader();

    private static readonly string[] racebps = new string[] {
        "racebps\\americans",
        "racebps\\british_africa",
        "racebps\\afrika_korps",
        "racebps\\germans"
    };

    private readonly ArmyHandler armyHandler = new ArmyHandler(racebps);

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public void Execute(Goal goal) {

        // Create entity producer
        RegistryProducer<EntityBlueprint> entityRegistry = new RegistryProducer<EntityBlueprint>();

        // Create database
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-abp-db-coh3.json", "abilities", abilityReader, armyHandler);

    }

}
