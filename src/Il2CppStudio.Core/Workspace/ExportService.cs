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
        var results = new List<ExportedAssembly>();

        foreach (var asm in ws.Model.Assemblies.OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase))
        {
            ct.ThrowIfCancellationRequested();
            if (filter is not null && !filter(asm)) continue;
            progress?.Report($"Exporting {asm.Name}…");
            results.Add(ExportAssembly(ws, asm, Path.Combine(outDir, WorkspaceSerializer.Sanitize(asm.Name)), includeIl, ct));
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

    public static ExportedAssembly ExportAssembly(Il2CppWorkspace ws, AssemblyModel asm, string dir, bool includeIl, CancellationToken ct = default)
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
            lifted = timer.Measure("lift", () => provider.EnsureLifted(all));
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
            var decompileWatch = Stopwatch.StartNew();
            foreach (var (type, cs, il) in ws.Decompiler.DecompileImage(image, topLevel, includeIl && !substituted))
            {
                ct.ThrowIfCancellationRequested();
                var rel = Path.Combine(type.Namespace.Length == 0 ? "-" : WorkspaceSerializer.Sanitize(type.Namespace), WorkspaceSerializer.Sanitize(type.Name));
                Write(Path.Combine(csDir, rel + ".cs"), cs);
                if (il.Length > 0) Write(Path.Combine(ilDir, rel + ".il"), il);
            }
            timer.Add("decompile+write", decompileWatch.Elapsed);
            if (!substituted)
            {
                try
                {
                    var refs = ProjectReferences.Materialize(ws, asm, Path.Combine(Path.GetDirectoryName(Path.GetFullPath(dir))!, "_references"));
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

    private static void Write(string path, string text)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, text);
    }
}
