using System.CommandLine;
using Il2CppStudio.Core.Accuracy;
using Il2CppStudio.Core.Config;
using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Cli.Commands;

/// <summary><c>extract</c>, <c>enable-debug</c>, <c>open-hex</c> and <c>test-accuracy</c>.</summary>
internal static class ToolCommands
{
    public static Command Extract(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("folder-path");
        var output = new Option<string>("--output", "-o") { Description = "Output directory.", Required = true };
        var noRaw = new Option<bool>("--no-raw-sections") { Description = "Do not dump raw metadata sections." };
        var cmd = new Command("extract", "Serialize resources (*-resources.dat), string literals and the metadata report to JSON + binary files.") { path, output, noRaw };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            using var ws = CliHost.Load(pr.GetValue(path)!, sink, substituteBcl: true);
            var summary = ws.ExportArtifacts(pr.GetValue(output)!, !pr.GetValue(noRaw));
            if (pr.GetValue(json)) CliHost.WriteJson(summary);
            else Console.WriteLine($"{summary.Resources} resources from {summary.ResourceFiles} containers, {summary.StringLiterals:N0} string literals, {summary.Sections} sections -> {summary.OutputDirectory} ({summary.Files.Count} files)");
            return sink.HasErrors ? 1 : 0;
        }));
        return cmd;
    }

    public static Command EnableDebug(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("folder-path") { Description = "Game folder, *_Data folder or boot.config path." };
        var set = new Option<string[]>("--set") { Description = "Additional key=value pairs (repeatable).", AllowMultipleArgumentsPerToken = true };
        var restore = new Option<bool>("--restore") { Description = "Restore boot.config from the backup made on first modification." };
        var dryRun = new Option<bool>("--dry-run") { Description = "Show the changes without writing." };
        var cmd = new Command("enable-debug", "Write diagnostic flags to boot.config (player-connection-debug, wait-for-native-debugger, scripting-runtime-version, enable-native-profile-agent).")
            { path, set, restore, dryRun };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            if (pr.GetValue(restore))
            {
                var ok = BootConfigManager.RestoreBackup(pr.GetValue(path)!, sink);
                Console.WriteLine(ok ? "boot.config restored." : "No backup found; nothing restored.");
                return ok ? 0 : 1;
            }
            var cfg = BootConfigManager.Open(pr.GetValue(path)!, sink);
            var changes = cfg.ApplyDiagnosticFlags().ToList();
            foreach (var kv in pr.GetValue(set) ?? [])
            {
                var eq = kv.IndexOf('=');
                if (eq <= 0) { Console.Error.WriteLine($"Ignoring '{kv}' (expected key=value)."); continue; }
                if (cfg.Set(kv[..eq].Trim(), kv[(eq + 1)..].Trim()) is { } c) changes.Add(c);
            }
            if (!pr.GetValue(dryRun) && changes.Count > 0) cfg.Save();
            if (pr.GetValue(json)) CliHost.WriteJson(new { cfg.Path, Changes = changes, DryRun = pr.GetValue(dryRun), Values = cfg.ToDictionary() });
            else
            {
                Console.WriteLine(cfg.Path);
                foreach (var c in changes) Console.WriteLine($"  {c.Key}: {c.OldValue ?? "(unset)"} -> {c.NewValue}");
                if (changes.Count == 0) Console.WriteLine("  already configured; no changes.");
                if (pr.GetValue(dryRun)) Console.WriteLine("  (dry run, file not written)");
            }
            return 0;
        }));
        return cmd;
    }

    public static Command OpenHex(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("folder-path");
        var method = new Option<string>("--method") { Description = "Method name or Type::Method.", Required = true };
        var type = new Option<string?>("--type") { Description = "Restrict the search to this type." };
        var noLaunch = new Option<bool>("--no-launch") { Description = "Only print the file offset." };
        var hxd = new Option<string?>("--hxd") { Description = "Path to HxD.exe." };
        var cmd = new Command("open-hex", "Open GameAssembly.dll in HxD at a method's code (prints the file offset for Ctrl+G).") { path, method, type, noLaunch, hxd };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            using var ws = CliHost.Load(pr.GetValue(path)!, sink, substituteBcl: true);
            var hits = CliHost.FindMethods(ws, pr.GetValue(method)!, pr.GetValue(type))
                .Select(h => (h.Type, h.Method, Target: HexEditorLauncher.Resolve(ws, h.Method))).Where(h => h.Target is not null).ToList();
            if (hits.Count == 0) { Console.Error.WriteLine("No method with native code matched."); return 2; }
            if (pr.GetValue(json)) CliHost.WriteJson(hits.Select(h => new { Type = h.Type.FullName, Method = h.Method.DisplaySignature, h.Target!.Rva, h.Target.FileOffset, h.Target.File }));
            else foreach (var h in hits) Console.WriteLine($"{h.Type.FullName}::{h.Method.DisplaySignature}  RVA 0x{h.Target!.Rva:X}  file offset 0x{h.Target.FileOffset:X}");
            if (hits.Count > 1) Console.Error.WriteLine($"{hits.Count} matches; opening the first. Use --type or Type::Method to disambiguate.");
            if (!pr.GetValue(noLaunch))
            {
                if (!HexEditorLauncher.Open(hits[0].Target!, sink, pr.GetValue(hxd))) return 1;
                Console.WriteLine(hits[0].Target!.Instructions);
            }
            return 0;
        }));
        return cmd;
    }

    public static Command TestAccuracy(Option<bool> verbose, Option<bool> json)
    {
        var gt = new Option<string>("--ground-truth-il") { Description = "Folder with the ground-truth managed assemblies (e.g. the backup's Managed/).", Required = true };
        var target = new Option<string>("--target") { Description = "IL2CPP game folder to lift.", Required = true };
        var report = new Option<string>("--report") { Description = "Output JSON report.", DefaultValueFactory = _ => "accuracy.json" };
        var asms = new Option<string[]>("--assembly") { Description = "Assemblies to compare (default: Assembly-CSharp*).", AllowMultipleArgumentsPerToken = true };
        var cmd = new Command("test-accuracy", "Differential IL accuracy: ground-truth CIL vs lifted CIL (opcode sequence, stack evaluation, CFG).") { gt, target, report, asms };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            using var ws = CliHost.Load(pr.GetValue(target)!, sink, substituteBcl: true);
            WorkspaceSetup.Attach(ws);
            var wanted = pr.GetValue(asms) is { Length: > 0 } a ? a : null;
            bool Selected(string name) => wanted is null
                ? name.StartsWith("Assembly-CSharp", StringComparison.OrdinalIgnoreCase)
                : wanted.Any(w => name.Equals(w, StringComparison.OrdinalIgnoreCase) || name.Equals(w + ".dll", StringComparison.OrdinalIgnoreCase));

            var images = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
            foreach (var asm in ws.Model.Assemblies.Where(x => Selected(x.ImageName) && !BclReferenceProvider.IsBclAssembly(x.Name)))
            {
                var all = new List<int>();
                void Collect(Core.Metadata.TypeModel t) { all.AddRange(t.Methods.Select(m => m.Index)); foreach (var n in t.NestedTypes) Collect(n); }
                foreach (var t in asm.Namespaces.SelectMany(n => n.Types)) Collect(t);
                ws.BodyProvider?.EnsureLifted(all);
                var image = ws.Stubs.FindImage(asm.ImageName);
                if (image >= 0 && ws.Decompiler.GetStubImage(image) is { } bytes) images[asm.ImageName] = bytes;
            }

            var result = CilAccuracyScorer.Compare(pr.GetValue(gt)!, images, ws.Files.Root, ws.ModeLabel, sink);
            WorkspaceSerializer.WriteJson(pr.GetValue(report)!, result);
            if (pr.GetValue(json)) CliHost.WriteJson(result with { Methods = [] });
            else
            {
                Console.WriteLine($"Opcode Sequence Match Rate      : {result.OpcodeSequenceMatchRate,6:F2} %   (strict {result.StrictOpcodeSequenceMatchRate:F2} %)");
                Console.WriteLine($"Stack Delta / Evaluation Acc.   : {result.StackEvaluationAccuracy,6:F2} %   (strict {result.StrictStackEvaluationAccuracy:F2} %)");
                Console.WriteLine($"Branch & CFG Match Rate         : {result.ControlFlowMatchRate,6:F2} %   (strict {result.StrictControlFlowMatchRate:F2} %)");
                Console.WriteLine($"Lift coverage                   : {result.LiftCoverage,6:F2} %");
                foreach (var x in result.Assemblies)
                    Console.WriteLine($"  {x.Assembly}: {x.LiftedMethods}/{x.GroundTruthMethods} lifted, opcode {x.OpcodeSequenceMatchRate:F2}%, stack {x.StackEvaluationAccuracy:F2}%, CFG {x.ControlFlowMatchRate:F2}%");
                Console.WriteLine($"Report: {Path.GetFullPath(pr.GetValue(report)!)}");
            }
            return 0;
        }));
        return cmd;
    }
}
