using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.CoH2;
using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON.Strategy;

public class CoH2Strategy : IGameStrategy {

    private readonly IBlueprintReader<AbilityBlueprint> abilityReader = new CoH2AbilityReader();
    private readonly IBlueprintReader<CriticalBlueprint> criticalReader = new CoH2CriticalReader();

    private static readonly string[] racebps = new string[] {
        "racebps\\soviet",
        "racebps\\aef",
        "racebps\\british",
        "racebps\\german",
        "racebps\\west_german",
    };

    private readonly ArmyHandler armyHandler = new ArmyHandler(racebps);

    public void Execute(Goal goal) {

        GenericDatabase(goal, $"{goal.ModName}-abp-db.json", "abilities", abilityReader, armyHandler);

    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
    private static IList<T> GenericDatabaseGet<T>(Goal goal, string dbname, string lookpath, IBlueprintReader<T> reader, params IBlueprintPostHandler<T>[] postHandlers) where T : IBlueprint {

        // Get destination
        string fileName = Path.Combine(goal.OutPath, dbname);

        try {

            // If file already exists, delete it.
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }

            // Get folder to search and read .xml files from
            string searchDir = Path.Combine(goal.InstancePath, lookpath);

            // Make sure there's a folder to read
            if (!Directory.Exists(searchDir)) {

                Console.WriteLine($"INFO: \"{lookpath}\" folder not found - the database creation will be skipped.");

            } else {

                var files = Directory.GetFiles(searchDir, "*.xml", SearchOption.AllDirectories);
                List<T> bps = new();

                foreach (string path in files) {

                    XmlDocument document = new XmlDocument();
                    document.Load(path);

                    string name = path[(path.LastIndexOf(@"\") + 1)..^4];
                    T bp = postHandlers.Aggregate(reader.FromXml(document, path, name), (x, y) => y.Handle(x));
                    string sbpsJson = JsonSerializer.Serialize(bp, Program.SerializerOptions);

                    bps.Add(bp);

                }

                // Return found values
                return bps;

            }
        } catch (Exception e) {

            // Log error and wait for user to exit
            Console.WriteLine(e.ToString());
            Console.ReadLine();

        }

        // Return something
        return Array.Empty<T>();

    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
    public static void GenericDatabaseSave<T>(Goal goal, string dbname, IEnumerable<T> data) where T : IBlueprint {

        // Get destination
        string fileName = Path.Combine(goal.OutPath, dbname);

        // Write
        File.WriteAllText(fileName, JsonSerializer.Serialize(data.ToArray(), Program.SerializerOptions));
        Console.WriteLine($"Created database: {fileName}");


    }

}
