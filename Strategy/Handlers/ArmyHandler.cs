using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Blueprints.Constraints;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// A handler used for assigning the army/faction of a blueprint based on its file path.
/// </summary>
public sealed class ArmyHandler : IBlueprintPostHandler {

    private readonly string[] racebps;

    /// <summary>
    /// Creates a new instance of the ArmyHandler class with the specified race blueprints.
    /// </summary>
    /// <param name="racebps">The list of race blueprints used to determine the army/faction of a blueprint based on its file path.</param>
    public ArmyHandler(params string[] racebps) {
        this.racebps = racebps;
    }

    /// <summary>
    /// Assigns the army/faction of the specified blueprint based on its file path.
    /// </summary>
    /// <typeparam name="T">The type of the blueprint.</typeparam>
    /// <param name="blueprint">The blueprint to modify.</param>
    /// <param name="filepath">The file path of the blueprint.</param>
    /// <returns>The modified blueprint.</returns>
    public T Handle<T>(T blueprint, string filepath) where T : IBlueprint {
        if (blueprint is IBlueprintOfArmy bp) {
            bp.Army = GetFactionFromPath(filepath);
        }
        return blueprint;
    }

    /// <summary>
    /// Determines the army/faction of a blueprint based on its file path.
    /// </summary>
    /// <param name="path">The file path of the blueprint.</param>
    /// <returns>The army/faction of the blueprint.</returns>
    public string GetFactionFromPath(string path) {
        int rid = path.IndexOf("races");
        string army;
        if (rid != -1) {
            army = path.Substring(rid + 6, path.Length - rid - 6).Split("\\")[0];
        } else {
            for (int i = 0; i < racebps.Length; i++) {
                string k = racebps[i][8..];
                if (path.Contains(k)) {
                    return k;
                }
            }
            army = "NULL";
        }
        if (army == "soviets") {
            army = "soviet";
        } else if (army == "brits") {
            army = "british";
        }
        return army;
    }

}
