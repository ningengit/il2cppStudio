using System.Collections.Concurrent;
using System.Diagnostics;
using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

public sealed record ExportedAssembly(string Name, bool Substituted, int Types, int MethodsLifted, string? StubPath, string CSharpFolder, string? CppHeader);

public sealed record ExportSummary(
    string OutputDirectory, string Mode, bool SubstituteBcl, IReadOnlyList<ExportedAssembly> Assemblies, SerializationSummary Artifacts,
    int Warnings, int Errors, TimeSpan Elapsed);

/// <summary>
/// "Export All": for each assembly writes the lifted stub DLL, C# per top-level type, IL per type and one C++
/// header, plus the workspace artifacts (resources, strings, metadata report) and a diagnostics log. With BCL
/// substitution enabled, BCL assemblies are not lifted (Clean/Fast); their C++ headers are still written.
/// The CLI and GUI both call this, so the outputs are identical.
/// </summary>
public static class ExportService
{
    public static ExportSummary ExportAll(Il2CppWorkspace ws, string outDir, bool includeIl = true, Func<AssemblyModel, bool>? filter = null,
        IProgress<string>? progress = null, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        Directory.CreateDirectory(outDir);

        _ = ws.Stubs;
        _ = ws.Decompiler;
        _ = ws.Cpp;
        var sharedBcl = new BclReferenceProvider(ws.Options.BclReferenceDirectory, ws.Environment.UnityVersion, null, NullDiagnosticSink.Instance);
        var materializedRefs = new ConcurrentDictionary<string, Lazy<string?>>(StringComparer.OrdinalIgnoreCase);
        var refsDir = Path.GetFullPath(Path.Combine(outDir, "_references"));
        var progressGate = new Lock();

        var assemblies = ws.Model.Assemblies
            .OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
            .Where(a => filter is null || filter(a))
            .ToList();

        // Pre-lift all selected non-BCL assemblies in one pass so StubModules stay immutable during parallel export.
        var preLifted = false;
        if (ws.BodyProvider is { } provider)
        {
            var allMethods = new List<int>();
            void Collect(TypeModel t) { allMethods.AddRange(t.Methods.Select(m => m.Index)); foreach (var n in t.NestedTypes) Collect(n); }
            foreach (var asm in assemblies)
            {
                if (ws.SubstituteBcl && BclReferenceProvider.IsBclAssembly(asm.Name)) continue;
                foreach (var ns in asm.Namespaces)
                    foreach (var t in ns.Types)
                        Collect(t);
            }
            if (allMethods.Count > 0)
                provider.EnsureLifted(allMethods);
            preLifted = true;
        }

        var results = new List<ExportedAssembly>(assemblies.Count);
        foreach (var asm in assemblies)
        {
            ct.ThrowIfCancellationRequested();
            progress?.Report($"Exporting {asm.Name}…");
            results.Add(ExportAssemblyCore(ws, asm, Path.Combine(outDir, WorkspaceSerializer.Sanitize(asm.Name)),
                refsDir, sharedBcl, materializedRefs, includeIl, preLifted, maxDecompileDegree: null, ct));
        }

        var artifacts = ws.ExportArtifacts(Path.Combine(outDir, "_artifacts"), includeRawSections: true);
        foreach (var log in ws.Decompiler.ErrorLog.WriteAll(outDir))
            ws.Sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExportCompleted, $"Error log written: {log}", new() { File = log });
        var collector = ws.Sink as DiagnosticCollector;
        if (collector is not null)
            WorkspaceSerializer.WriteJson(Path.Combine(outDir, "diagnostics.json"), collector.Snapshot());

        var summary = new ExportSummary(outDir, ws.ModeLabel, ws.SubstituteBcl, results, artifacts,
            collector?.CountOf(DiagnosticSeverity.Warning) ?? 0, (collector?.CountOf(DiagnosticSeverity.Error) ?? 0) + (collector?.CountOf(DiagnosticSeverity.Fatal) ?? 0),
            sw.Elapsed);
        WorkspaceSerializer.WriteJson(Path.Combine(outDir, "export-summary.json"), summary);
        ws.Sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExportCompleted,
            $"Exported {results.Count} assemblies to '{outDir}' in {sw.Elapsed.TotalSeconds:F1}s.", new() { File = outDir });
        return summary;
    }

    public static ExportedAssembly ExportAssembly(Il2CppWorkspace ws, AssemblyModel asm, string dir, bool includeIl, CancellationToken ct = default) =>
        ExportAssemblyCore(ws, asm, dir, Path.Combine(Path.GetDirectoryName(Path.GetFullPath(dir))!, "_references"), null, null, includeIl, preLifted: false, maxDecompileDegree: null, ct);

    private static ExportedAssembly ExportAssemblyCore(
        Il2CppWorkspace ws,
        AssemblyModel asm,
        string dir,
        string refsDir,
        BclReferenceProvider? sharedBcl,
        ConcurrentDictionary<string, Lazy<string?>>? materializedRefs,
        bool includeIl,
        bool preLifted,
        int? maxDecompileDegree,
        CancellationToken ct)
    {
        Directory.CreateDirectory(dir);
        var timer = new PhaseTimer();
        var image = timer.Measure("stubs", () => ws.Stubs.FindImage(asm.ImageName));
        var substituted = ws.SubstituteBcl && BclReferenceProvider.IsBclAssembly(asm.Name);
        var topLevel = asm.Namespaces.SelectMany(n => n.Types).ToList();

        // C++ header (always available — derived from metadata + registration).
        string? cppPath = null;
        try
        {
            cppPath = Path.Combine(dir, WorkspaceSerializer.Sanitize(asm.Name) + ".h");
            timer.Measure("c++ header", () => File.WriteAllText(cppPath, ws.Cpp.GenerateImageHeader(asm)));
        }
        catch (Exception ex)
        {
            ws.Sink.Warn(DiagnosticCategory.Extraction, DiagnosticCodes.ExtractionEntryFailed, $"C++ header for {asm.Name}: {ex.Message}", new() { AssemblyName = asm.Name }, ex);
        }

        var lifted = 0;
        if (!substituted && ws.BodyProvider is { } provider)
        {
            var all = new List<int>();
            void Collect(TypeModel t) { all.AddRange(t.Methods.Select(m => m.Index)); foreach (var n in t.NestedTypes) Collect(n); }
            topLevel.ForEach(Collect);
            lifted = timer.Measure("lift", () =>
                preLifted && provider is Lifting.LiftingBodyProvider lbp
                    ? all.Distinct().Count(m => lbp.Results.TryGetValue(m, out var r) && r.Success)
                    : provider.EnsureLifted(all));
        }

        string? stubPath = null;
        if (!substituted && image >= 0 && timer.Measure("stub dll", () => ws.Decompiler.GetStubImage(image)) is { } bytes)
        {
            stubPath = Path.Combine(dir, asm.ImageName);
            File.WriteAllBytes(stubPath, bytes);
        }

        var csDir = Path.Combine(dir, "csharp");
        var ilDir = Path.Combine(dir, "il");
        if (image >= 0)
        {
            var createdDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var type in topLevel)
            {
                var nsDir = type.Namespace.Length == 0 ? "-" : WorkspaceSerializer.Sanitize(type.Namespace);
                var cDir = Path.Combine(csDir, nsDir);
                if (createdDirs.Add(cDir)) Directory.CreateDirectory(cDir);
                if (includeIl && !substituted)
                {
                    var iDir = Path.Combine(ilDir, nsDir);
                    if (createdDirs.Add(iDir)) Directory.CreateDirectory(iDir);
                }
            }

            var decompileWatch = Stopwatch.StartNew();
            ws.Decompiler.DecompileImage(image, topLevel, includeIl && !substituted, (type, cs, il) =>
            {
                var rel = Path.Combine(type.Namespace.Length == 0 ? "-" : WorkspaceSerializer.Sanitize(type.Namespace), WorkspaceSerializer.Sanitize(type.Name));
                File.WriteAllText(Path.Combine(csDir, rel + ".cs"), cs);
                if (il.Length > 0) File.WriteAllText(Path.Combine(ilDir, rel + ".il"), il);
            }, ct, maxDecompileDegree);
            timer.Add("decompile+write", decompileWatch.Elapsed);
            if (!substituted)
            {
                try
                {
                    var refs = ProjectReferences.Materialize(ws, asm, refsDir, sharedBcl, materializedRefs);
                    ProjectFileWriter.Write(dir, asm.Name, refs);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    ws.Sink.Warn(DiagnosticCategory.Extraction, DiagnosticCodes.ExtractionEntryFailed, $"Project file for {asm.Name}: {ex.Message}", new() { AssemblyName = asm.Name }, ex);
                }
            }
        }
        if (!substituted) ws.Sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExportCompleted, $"{asm.Name}: {timer}", new() { AssemblyName = asm.Name });
        return new ExportedAssembly(asm.Name, substituted, asm.TypeCount, lifted, stubPath, csDir, cppPath);
    }
}
