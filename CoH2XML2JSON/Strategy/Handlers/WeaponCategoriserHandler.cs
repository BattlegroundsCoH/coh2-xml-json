using System;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.Handlers;

/// <summary>
/// A post-handler for weapon blueprints that categorizes the weapon based on the file path.
/// </summary>
public sealed class WeaponCategoriserHandler : IBlueprintPostHandler {

    /// <summary>
    /// Categorizes the weapon based on the file path and assigns the appropriate category, sub-type, and type to the blueprint.
    /// </summary>
    /// <typeparam name="T">The type of the blueprint.</typeparam>
    /// <param name="blueprint">The blueprint to handle.</param>
    /// <param name="filepath">The path to the file.</param>
    /// <returns>The handled blueprint.</returns>
    public T Handle<T>(T blueprint, string filepath) where T : IBlueprint {
        if (blueprint is WeaponBlueprint wbp) {
            wbp.Category = IfContains(filepath, x => x switch {
                "small_arms" => WeaponCategory.smallarms,
                "flame_throwers" => WeaponCategory.flame,
                "explosive_weapons" => WeaponCategory.explosive,
                "ballistic_weapon" => WeaponCategory.ballistic,
                _ => WeaponCategory.undefined
            });
            wbp.SmallArmsType = wbp.Category is WeaponCategory.smallarms ? IfContains(filepath, x => x switch {
                "rifle" => WeaponSmallArmsType.rifle,
                "pistol" => WeaponSmallArmsType.pistol,
                "sub_machine_gun" => WeaponSmallArmsType.submachinegun,
                "light_machine_gun" => WeaponSmallArmsType.lightmachinegun,
                "heavy_machine_gun" => WeaponSmallArmsType.heavymachinegun,
                _ => WeaponSmallArmsType.invalid
            }) : WeaponSmallArmsType.invalid;
            wbp.BalisticType = wbp.Category is WeaponCategory.ballistic ? IfContains(filepath, x => x switch {
                "anti_tank_gun" => WeaponBalisticType.antitankgun,
                "infantry_anti_tank_weapon" => WeaponBalisticType.infantryatgun,
                "tank_gun" => WeaponBalisticType.tankgun,
                _ => WeaponBalisticType.invalid
            }) : WeaponBalisticType.invalid;
            wbp.ExplosiveType = wbp.Category is WeaponCategory.explosive ? IfContains(filepath, x => x switch {
                "mine" => WeaponExplosiveType.mine,
                "mortar" => WeaponExplosiveType.mortar,
                "grenade" => WeaponExplosiveType.grenade,
                "light_artillery" or "heavy_artillery" => WeaponExplosiveType.artillery,
                _ => WeaponExplosiveType.invalid
            }) : WeaponExplosiveType.invalid;
        }
        return blueprint;
    }

    private static T IfContains<T>(string path, Func<string, T> typeMap) where T : Enum {
        string[] check = path.Split('\\');
        for (int i = 0; i < check.Length; i++) {
            T val = typeMap(check[i]);
            if (val.CompareTo(default(T)) != 0) {
                return val;
            }
        }
        return typeMap(path);
    }

}
