using CoH2XML2JSON.Blueprints.Constraints;

namespace CoH2XML2JSON.Strategy.Handlers;

public class ArmyHandler : IBlueprintPostHandler<IBlueprintOfArmy> {

    private readonly string[] racebps;

    public ArmyHandler(params string[] racebps) {
        this.racebps = racebps;
    }

    public IBlueprintOfArmy Handle(IBlueprintOfArmy blueprint, string filepath) {
        blueprint.Army = GetFactionFromPath(filepath);
        return blueprint;
    }

    private string GetFactionFromPath(string path) {
        int rid = path.IndexOf("races");
        string army = path;
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
