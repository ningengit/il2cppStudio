using System.CommandLine;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Cli.Commands;

/// <summary>
/// <c>load &lt;folder&gt; [--export-all &lt;dir&gt;] [--substitute-bcl] [--no-il] [--assembly NAME]</c> and
/// <c>inspect &lt;folder&gt; --type T [--method M] --view cpp|il|csharp|all</c>.
/// </summary>
internal static class CodeCommands
{
    public static Command Load(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("folder-path") { Description = "Game folder (containing GameAssembly.dll and *_Data)." };
        var exportAll = new Option<string?>("--export-all") { Description = "Export C#, IL, C++ headers, stub DLLs and artifacts to this directory." };
        var bcl = new Option<bool>("--substitute-bcl") { Description = "Substitute BCL (Clean/Fast): use pristine reference assemblies for System.*/Microsoft.*/mscorlib." };
        var bclDir = new Option<string?>("--bcl-dir") { Description = "Folder with BCL reference assemblies (overrides auto-detection)." };
        var noIl = new Option<bool>("--no-il") { Description = "Skip per-type IL listings in the export." };
        var only = new Option<string[]>("--assembly") { Description = "Restrict export to these assemblies (repeatable).", AllowMultipleArgumentsPerToken = true };

        var cmd = new Command("load", "Load a game folder, report the workspace and optionally export everything.") { path, exportAll, bcl, bclDir, noIl, only };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            using var ws = CliHost.Load(pr.GetValue(path)!, sink, pr.GetValue(bcl), pr.GetValue(bclDir));
            WorkspaceSetup.Attach(ws);
            var s = ws.Model.Statistics;
            if (!pr.GetValue(json))
            {
                Console.WriteLine($"{ws.ModeLabel}");
                Console.WriteLine($"Unity {ws.Environment.UnityVersion ?? "?"}  metadata v{s.Version}  {s.Assemblies} assemblies, {s.Types:N0} types, {s.Methods:N0} methods");
                Console.WriteLine($"Native code: {(ws.HasNativeCode ? $"{ws.AddressMap!.ResolvedMethodCount:N0} methods + {ws.AddressMap.GenericInstanceCount:N0} generic instances" : "unavailable")}");
                Console.WriteLine($"View: {(ws.SubstituteBcl ? "Substitute BCL (Clean/Fast)" : "Pure Native View")}");
            }

            if (pr.GetValue(exportAll) is { } outDir)
            {
                var filter = pr.GetValue(only) is { Length: > 0 } names
                    ? (Func<Core.Metadata.AssemblyModel, bool>)(a => names.Any(n => a.Name.Equals(n, StringComparison.OrdinalIgnoreCase) || a.ImageName.Equals(n, StringComparison.OrdinalIgnoreCase)))
                    : null;
                var summary = ExportService.ExportAll(ws, outDir, !pr.GetValue(noIl), filter, new Progress<string>(m => Console.Error.WriteLine(m)));
                if (pr.GetValue(json)) CliHost.WriteJson(summary);
                else Console.WriteLine($"Exported {summary.Assemblies.Count} assemblies to {outDir} in {summary.Elapsed.TotalSeconds:F1}s.");
            }
            else if (pr.GetValue(json)) CliHost.WriteJson(new { ws.ModeLabel, ws.Files, s, ws.Environment });
            return sink.HasErrors ? 1 : 0;
        }));
        return cmd;
    }

    public static Command Inspect(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("folder-path");
        var type = new Option<string>("--type") { Description = "Class name (simple or full).", Required = true };
        var method = new Option<string?>("--method") { Description = "Method name (all overloads are shown)." };
        var view = new Option<string>("--view") { Description = "cpp | il | csharp | all", DefaultValueFactory = _ => "all" };
        view.AcceptOnlyFromAmong("cpp", "il", "csharp", "all");
        var bcl = new Option<bool>("--substitute-bcl") { Description = "Substitute BCL (Clean/Fast)." };

        var cmd = new Command("inspect", "Show the C/C++, IL and/or C# view of a class or method.") { path, type, method, view, bcl };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            using var ws = CliHost.Load(pr.GetValue(path)!, sink, pr.GetValue(bcl));
            WorkspaceSetup.Attach(ws);
            if (!CliHost.TryResolve(ws, pr.GetValue(type)!, pr.GetValue(method), out var t, out var methods)) return 2;

            var views = pr.GetValue(view) switch
            {
                "cpp" => new[] { CodeView.Cpp },
                "il" => [CodeView.Il],
                "csharp" => [CodeView.CSharp],
                _ => [CodeView.Cpp, CodeView.Il, CodeView.CSharp],
            };
            var targets = methods.Count == 0 ? [null] : methods.Cast<Core.Metadata.MethodModel?>().ToList();
            var results = new List<object>();
            foreach (var m in targets)
                foreach (var v in views)
                {
                    var text = ws.Render(t, m, v);
                    if (pr.GetValue(json)) { results.Add(new { Type = t.FullName, Method = m?.DisplaySignature, Rva = m is null ? 0 : ws.GetMethodRva(m), View = v.ToString(), Text = text }); continue; }
                    Console.WriteLine($"==================== {v} :: {t.FullName}{(m is null ? "" : "::" + m.DisplaySignature)}{(m is not null && ws.GetMethodRva(m) is var rva and > 0 ? $"  (RVA 0x{rva:X})" : "")}");
                    Console.WriteLine(text);
                }
            if (pr.GetValue(json)) CliHost.WriteJson(results);
            return 0;
        }));
        return cmd;
    }
}
