using System.Diagnostics.CodeAnalysis;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.CoH3;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Class implementing the <see cref="IGameStrategy"/> interface for the CoH3 reading strategy.
/// </summary>
public sealed class CoH3Strategy : IGameStrategy {

    private readonly IBlueprintReader<AbilityBlueprint> abilityReader = new CoH3AbilityReader();
    private readonly IBlueprintReader<SquadBlueprint> squadReader = new CoH3SquadReader();
    private readonly IBlueprintReader<EntityBlueprint> entityReader = new CoH3EntityReader();
    private readonly IBlueprintReader<UpgradeBlueprint> upgradeReader = new CoH3UpgradeReader();
    private readonly IBlueprintReader<WeaponBlueprint> weaponReader = new CoH3WeaponReader();

    private static readonly string[] racebps = new string[] {
        "racebps\\americans",
        "racebps\\british_africa",
        "racebps\\afrika_korps",
        "racebps\\germans"
    };

    private readonly ArmyHandler armyHandler = new ArmyHandler(racebps);
    private readonly ParentHandler parentHandler = new ParentHandler();

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public void Execute(Goal goal) {

        // Create variant helper
        VariantSelector variant = new VariantSelector(goal.Variant);

        // Create entity producer
        RegistryProducer<EntityBlueprint> entityRegistry = new RegistryProducer<EntityBlueprint>();

        // Create database
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-abp-db-coh3.json", "abilities", abilityReader, variant, armyHandler, parentHandler);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-sbp-db-coh3.json", "ebps\\races", squadReader, variant, armyHandler, entityRegistry, parentHandler);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-ebp-db-coh3.json", "sbps\\races", entityReader, variant, armyHandler, entityRegistry.CreateConsumer(), parentHandler);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-ubp-db-coh3.json", "upgrade", upgradeReader, variant, parentHandler);
        IGameStrategy.CreateDatabase(goal, $"{goal.ModName}-wbp-db-coh3.json", "weapon", weaponReader, variant, parentHandler);

    }

}
