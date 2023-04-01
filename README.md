# Battlegrounds: XML to JSON

[![.NET Core](https://github.com/BattlegroundsCoH/coh2-xml-json/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/BattlegroundsCoH/coh2-xml-json/actions/workflows/dotnetcore.yml)
[![.NET Core](https://github.com/BattlegroundsCoH/coh2-xml-json/actions/workflows/dotnettest.yml/badge.svg)](https://github.com/BattlegroundsCoH/coh2-xml-json/actions/workflows/dotnettest.yml)

Handy tool for converting CoH2 and CoH3 attribute editor xml files into a json database that can be read by the [Battlegrounds Launcher](https://github.com/BattlegroundsCoH/coh-battlegrounds).


## Compile

The tool can be compiled with the command

```
dotnet build
```

The tool can be started by running

```
dotnet run
```

Alternatively, run the executable that was compiled.

## Run

The tool is very simple and will prompt the user for basic information of what to do.

If the file `last.json` exists and the application is started with the '-do_last' parameter, the application will automatically run using the settings specified in `last.json` without any user prompt.

The application can be set to target `Company of Heroes 3` by adding the `-coh3` parameter flag.

### Company of Heroes 3
Note that CoH3 does not have the same concept of critical and slot item blueprints as CoH2. These blueprint types are, therefore, not generated in the CoH3 database.

CoH3 also introduces the concept of inheritance and variants. The tool has been updated to support this. Inheritance thus picks the first defined value found. Meaning the child value is chosen if defined; Otherwise the parent value is picked. A value is always stored in the database, even when the child value is not defined in the read .xml file.

By default the CoH3 `default` variant is picked. A specific variant can be picked by specifying it with the `-variant <Variant-Name>` parameter.