﻿using System.Diagnostics.CodeAnalysis;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.CoH2;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Class implementing the <see cref="IGameStrategy"/> interface for the CoH2 reading strategy.
/// </summary>
public sealed class CoH2Strategy : IGameStrategy {

    private readonly IBlueprintReader<AbilityBlueprint> abilityReader = new CoH2AbilityReader();
    private readonly IBlueprintReader<CriticalBlueprint> criticalReader = new CoH2CriticalReader();
    private readonly IBlueprintReader<EntityBlueprint> entityReader = new CoH2EntityReader();
    private readonly IBlueprintReader<SquadBlueprint> squadReader = new CoH2SquadReader();
    private readonly IBlueprintReader<SlotItemBlueprint> slotItemReader = new CoH2SlotItemReader();
    private readonly IBlueprintReader<UpgradeBlueprint> upgradeReader = new CoH2UpgradeReader();
    private readonly IBlueprintReader<WeaponBlueprint> weaponReader = new CoH2WeaponReader();

    private static readonly string[] racebps = new string[] {
        "racebps\\soviet",
        "racebps\\aef",
        "racebps\\british",
        "racebps\\german",
        "racebps\\west_german",
    };

    private readonly ArmyHandler armyHandler = new ArmyHandler(racebps);

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public void Execute(Goal goal) {

        // Create entity producer
        RegistryProducer<EntityBlueprint> entityRegistry = new RegistryProducer<EntityBlueprint>();

        // Create databases
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-abp-db.json", "abilities", abilityReader, armyHandler);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-cbp-db.json", "critical", criticalReader);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-ebp-db.json", new[] { "ebps\\races", "ebps\\gameplay" }, entityReader, armyHandler, entityRegistry);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-ebp-db.json", "sbps\\races", squadReader, armyHandler, entityRegistry.CreateConsumer());
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-cbp-db.json", "slot_item", slotItemReader, armyHandler);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-cbp-db.json", "upgrade", upgradeReader);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-cbp-db.json", "weapon", weaponReader);

    }

}