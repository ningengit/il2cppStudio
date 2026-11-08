using System.CommandLine;
using Il2CppStudio.Cli;
using Il2CppStudio.Cli.Commands;

// Il2CppStudio CLI. Every GUI operation maps to a verb here and runs through the same Core services
// (Il2CppWorkspace + WorkspaceSetup), so CLI and GUI produce identical results.

var verbose = new Option<bool>("--verbose", "-v") { Description = "Show debug/verbose diagnostics on the console.", Recursive = true };
var json = new Option<bool>("--json") { Description = "Write machine-readable JSON to stdout.", Recursive = true };

var root = new RootCommand("Il2CppStudio — Unity IL2CPP build inspection, verification and decompilation suite");
root.Options.Add(verbose);
root.Options.Add(json);

root.Subcommands.Add(CodeCommands.Load(verbose, json));
root.Subcommands.Add(CodeCommands.Inspect(verbose, json));
root.Subcommands.Add(ToolCommands.Extract(verbose, json));
root.Subcommands.Add(ToolCommands.EnableDebug(verbose, json));
root.Subcommands.Add(ToolCommands.TestAccuracy(verbose, json));
root.Subcommands.Add(ToolCommands.OpenHex(verbose, json));
root.Subcommands.Add(InfoCommands.Metadata(verbose, json));
root.Subcommands.Add(InfoCommands.Pe(verbose, json));
root.Subcommands.Add(InfoCommands.Disasm(verbose, json));
root.Subcommands.Add(InfoCommands.Helpers(verbose, json));

return root.Parse(args).Invoke();
