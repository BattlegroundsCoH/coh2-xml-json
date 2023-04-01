using CoH2XML2JSON.Blueprints;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml;
using System;

namespace CoH2XML2JSON.Strategy;

/// <summary>
/// Defines a strategy for executing a translation task from XML files to a JSON database based on the specified goal.
/// </summary>
public interface IGameStrategy {

    /// <summary>
    /// Executes a translation task from XML files to a JSON database based on the specified goal.
    /// </summary>
    /// <param name="goal">The goal object that specifies the translation task.</param>
    void Execute(Goal goal);

    /// <summary>
    /// Saves the given collection of <see cref="IBlueprint"/> objects to a JSON file with the given name in the given output directory.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IBlueprint"/> objects to save.</typeparam>
    /// <param name="goal">The Goal object containing information about the mod being built.</param>
    /// <param name="dbname">The name of the database file to create.</param>
    /// <param name="data">The collection of <see cref="IBlueprint"/> objects to save.</param>
    /// <remarks>
    /// This method uses <see cref="JsonSerializer"/> to serialize the <see cref="IBlueprint"/> objects to JSON.
    /// </remarks>
    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
    public static void GenericDatabaseSave<T>(Goal goal, string dbname, IEnumerable<T> data) where T : IBlueprint {

        // Get destination
        string fileName = Path.Combine(goal.OutPath, dbname);

        // Write
        File.WriteAllText(fileName, JsonSerializer.Serialize(data.ToArray(), Program.SerializerOptions));
        Console.WriteLine($"Created database: {fileName}");


    }

    /// <summary>
    /// Creates a JSON database file with the given name containing <see cref="IBlueprint"/> objects parsed from the XML files located in the given path(s).
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IBlueprint"/> objects to create the database for.</typeparam>
    /// <param name="goal">The Goal object containing information about the mod being built.</param>
    /// <param name="dbname">The name of the database file to create.</param>
    /// <param name="lookpath">The path to the XML file(s) to parse.</param>
    /// <param name="reader">The <see cref="IBlueprintReader{T}"/> used to parse the XML files.</param>
    /// <param name="handlers">The <see cref="IBlueprintHandler"/>(s) used to modify the <see cref="IBlueprint"/> objects after parsing.</param>
    /// <remarks>
    /// This method calls <see cref="GenericDatabaseGet{T}(Goal, string, string, IBlueprintReader{T}, IBlueprintHandler[])"/> to parse the XML files and then calls <see cref="GenericDatabaseSave{T}(Goal, string, IEnumerable{T})"/> to save the parsed <see cref="IBlueprint"/> objects to JSON.
    /// </remarks>
    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
    public static void CreateDatabase<T>(Goal goal, string dbname, string lookpath, IBlueprintReader<T> reader, params IBlueprintHandler[] handlers) where T : IBlueprint {

        // Get collection
        var bps = GenericDatabaseGet(goal, dbname, lookpath, reader, handlers);

        // Save it
        GenericDatabaseSave(goal, dbname, bps);

    }

    /// <summary>
    /// Creates a JSON database file with the given name containing <see cref="IBlueprint"/> objects parsed from the XML files located in the given path(s).
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IBlueprint"/> objects to create the database for.</typeparam>
    /// <param name="goal">The Goal object containing information about the mod being built.</param>
    /// <param name="dbname">The name of the database file to create.</param>
    /// <param name="lookpath">The list of paths to the XML file(s) to parse.</param>
    /// <param name="reader">The <see cref="IBlueprintReader{T}"/> used to parse the XML files.</param>
    /// <param name="handlers">The <see cref="IBlueprintHandler"/>(s) used to modify the <see cref="IBlueprint"/> objects after parsing.</param>
    /// <remarks>
    /// This method calls <see cref="GenericDatabaseGet{T}(Goal, string, string, IBlueprintReader{T}, IBlueprintHandler[])"/> to parse the XML files and then calls <see cref="GenericDatabaseSave{T}(Goal, string, IEnumerable{T})"/> to save the parsed <see cref="IBlueprint"/> objects to JSON.
    /// </remarks>
    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
    public static void CreateDatabase<T>(Goal goal, string dbname, IList<string> lookpath, IBlueprintReader<T> reader, params IBlueprintHandler[] handlers) where T : IBlueprint {
        List<T> bps = new List<T>();
        foreach (var path in lookpath) {
            bps.AddRange(GenericDatabaseGet(goal, dbname, path, reader, handlers));
        }
        GenericDatabaseSave(goal, dbname, bps);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
    private static IList<T> GenericDatabaseGet<T>(Goal goal, string dbname, string lookpath, IBlueprintReader<T> reader, params IBlueprintHandler[] handlers) where T : IBlueprint {

        // Get destination
        string fileName = Path.Combine(goal.OutPath, dbname);

        // Get handlers
        var helpers = new Helpers(handlers.Where(x => x is IBlueprintHelperHandler).Cast<IBlueprintHelperHandler>().ToArray());
        var xmlHandlers = handlers.Where(x => x is IBlueprintXmlHandler).Cast<IBlueprintXmlHandler>();
        var postHandlers = handlers.Where(x => x is IBlueprintPostHandler).Cast<IBlueprintPostHandler>();
        var postDbHandlers = handlers.Where(x => x is IBlueprintPostDatabaseHandler).Cast<IBlueprintPostDatabaseHandler>();

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
                    T? bpXml = reader.FromXml(document, path, name, helpers);
                    if (bpXml is null) {
                        continue;
                    }

                    T postXmlBp = xmlHandlers.Aggregate(bpXml, (x, y) => y.Handle(document, x, helpers));
                    T bp = postHandlers.Aggregate(postXmlBp, (x, y) => y.Handle(x, path));
                    string sbpsJson = JsonSerializer.Serialize(bp, Program.SerializerOptions);

                    bps.Add(bp);

                }

                // Get post DB
                IList<T> handledBps = postDbHandlers.Aggregate((IList<T>)bps, (x, y) => y.Handle(x));

                // Return found values
                return handledBps;

            }
        } catch (Exception e) {

            // Log error and wait for user to exit
            Console.WriteLine(e.ToString());
            Console.ReadLine();

        }

        // Return something
        return Array.Empty<T>();

    }

}
