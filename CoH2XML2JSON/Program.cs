﻿using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

using CoH2XML2JSON.Strategy;
using CoH2XML2JSON.Strategy.Listeners;
using System.Collections.Generic;

namespace CoH2XML2JSON;

public class Program {

    public static readonly JsonSerializerOptions SerializerOptions = new() { 
        WriteIndented = true, 
        IgnoreReadOnlyFields = false,
        IgnoreReadOnlyProperties = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
    };

    public const string DirPathArg = "--dir";
    public const string InstancesArg = "--instances";
    public const string ModGuidArg = "--guid";
    public const string ModNameArg = "--mod";

    public static Dictionary<string, string> ReadArguments(string[] args, params string[] keys) {
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        for (int i = 0; i < keys.Length; i++) {
            int idx = Array.IndexOf(args, keys[i]);
            if (idx != -1 && idx + 1 < args.Length) {
                keyValuePairs[keys[i]] = args[idx+1];
            }
        }
        return keyValuePairs;
    }

    private static string FromArgOrInput(string arg, Dictionary<string, string> kvArgs) {
        if (kvArgs.TryGetValue(arg, out string ?value)) {
            Console.WriteLine(value);
            return kvArgs[arg];
        }
        return Console.ReadLine()!;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public static void Main(string[] args) {

        string dirPath = string.Empty;
        string instancesPath = string.Empty;
        string modguid = string.Empty;
        string modname = string.Empty;

        var kvArgs = ReadArguments(args, DirPathArg, InstancesArg, ModGuidArg, ModNameArg);
        bool doLastIgnoreInput = args.Contains("-do_last");
        bool isCoH3 = args.Contains("-coh3");
        bool silent = args.Contains("-silent");
        bool ignoreLast = args.Contains("-skip_last");

        Console.WriteLine(string.Join(" ", args));

        Goal? last = null;
        if (File.Exists("last.json") && !ignoreLast) {
            last = JsonSerializer.Deserialize<Goal>(File.ReadAllText("last.json"));
            if (last is not null && !silent) {
                Console.WriteLine("Use settings from last execution?");
                Console.WriteLine("Output Directory: " + last.OutPath);
                Console.WriteLine("Instance Directory: " + last.InstancePath);
                Console.WriteLine("ModGUID: " + last.ModGuid);
                Console.WriteLine();
                if (doLastIgnoreInput) {
                    dirPath = last.OutPath;
                    modguid = last.ModGuid;
                    instancesPath = last.InstancePath;
                    modname = last.ModName;
                } else {
                    Console.Write("(Y/N): ");
                    if (Console.ReadLine()!.ToLower() is not "y") {
                        last = null;
                    } else {
                        dirPath = last.OutPath;
                        modguid = last.ModGuid;
                        instancesPath = last.InstancePath;
                        modname = last.ModName;
                    }
                }
            }
        }

        if (last is null) {
            Console.Write("Set path where you want the files to be created to: ");
            dirPath = FromArgOrInput(DirPathArg, kvArgs);

            while (!Directory.Exists(dirPath)) {
                if (string.IsNullOrEmpty(dirPath)) { // Because I'm lazy - this is a quick method to simply use the directory of the .exe
                    Console.WriteLine($"Using: {Environment.CurrentDirectory}");
                    dirPath = Environment.CurrentDirectory;
                    break;
                }
                if (kvArgs.ContainsKey(DirPathArg)) {
                    Console.Error.WriteLine("Output Directory not found");
                    return;
                }
                Console.Write("Invalid path! Try again: ");
                dirPath = Console.ReadLine()!;
            }

            Console.Write("Set path to your \"instances\" folder: ");
            instancesPath = FromArgOrInput(InstancesArg, kvArgs);

            while (!Directory.Exists(instancesPath) && !instancesPath.EndsWith(@"\instances")) {
                if (kvArgs.ContainsKey(InstancesArg)) {
                    Console.Error.WriteLine("Instances Directory not found");
                    return;
                }
                Console.Write("Invalid path! Try again: ");
                instancesPath = Console.ReadLine()!;
            }

            Console.Write("Mod GUID (Leave empty if not desired/available):");
            modguid = FromArgOrInput(ModGuidArg, kvArgs).Replace("-", "");
            if (modguid.Length != 32) {
                modguid = string.Empty;
            }

            Console.Write("Mod Name (Leave empty for vanilla):");
            modname = FromArgOrInput(ModNameArg, kvArgs);
            if (string.IsNullOrEmpty(modname)) {
                modname = "vcoh";
            }

        }

        Goal goal = new() { InstancePath = instancesPath, ModGuid = modguid, OutPath = dirPath, ModName = modname };
        File.WriteAllText("last.json", JsonSerializer.Serialize(goal));

        IGameStrategy strategy = isCoH3 ? new CoH3Strategy() : new CoH2Strategy();
        strategy.Execute(goal, new NullListener());

        Console.WriteLine();

        if (!doLastIgnoreInput && !silent) {
            Console.WriteLine("Created databases - Press any key to exit");
            Console.Read();
        }

    }

}
