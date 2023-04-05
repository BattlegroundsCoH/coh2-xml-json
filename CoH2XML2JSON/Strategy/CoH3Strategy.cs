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
    private readonly ScarPathHandler scarPathHandler = new ScarPathHandler();
    private readonly WeaponCategoriserHandler weaponCategoriserHandler = new WeaponCategoriserHandler();

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public void Execute(Goal goal, IStrategyListener listener) {

        // Create variant helper
        VariantSelector variant = new VariantSelector(goal.Variant);

        // Create parent handler
        ParentHandler parentHandler = new ParentHandler(scarPathHandler);

        // Create entity producer
        RegistryProducer<EntityBlueprint> entityRegistry = new RegistryProducer<EntityBlueprint>();

        // Create database
        IGameStrategy.CreateDatabase(goal, listener, $"{goal.ModName}-abp-db-coh3.json", "abilities", abilityReader, variant, scarPathHandler, armyHandler, parentHandler);
        IGameStrategy.CreateDatabase(goal, listener, $"{goal.ModName}-sbp-db-coh3.json", "ebps\\races", entityReader, variant, scarPathHandler, armyHandler, entityRegistry, parentHandler);
        IGameStrategy.CreateDatabase(goal, listener, $"{goal.ModName}-ebp-db-coh3.json", "sbps\\races", squadReader, variant, scarPathHandler, armyHandler, entityRegistry.CreateConsumer(), parentHandler);
        IGameStrategy.CreateDatabase(goal, listener, $"{goal.ModName}-ubp-db-coh3.json", "upgrade", upgradeReader, variant, scarPathHandler, parentHandler);
        IGameStrategy.CreateDatabase(goal, listener, $"{goal.ModName}-wbp-db-coh3.json", "weapon", weaponReader, variant, scarPathHandler, parentHandler, weaponCategoriserHandler);

    }

}
