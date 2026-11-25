using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;
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
    private sealed class AssemblyPlan(
        AssemblyModel asm,
        string dir,
        string csDir,
        string ilDir,
        int image,
        bool substituted,
        List<TypeModel> topLevel,
        int lifted)
    {
        public readonly AssemblyModel Asm = asm;
        public readonly string Dir = dir;
        public readonly string CsDir = csDir;
        public readonly string IlDir = ilDir;
        public readonly int Image = image;
        public readonly bool Substituted = substituted;
        public readonly List<TypeModel> TopLevel = topLevel;
        public readonly int Lifted = lifted;
        public string? StubPath;
        public string? CppPath;
    }

    public static ExportSummary ExportAll(Il2CppWorkspace ws, string outDir, bool includeIl = true, Func<AssemblyModel, bool>? filter = null,
        IProgress<string>? progress = null, CancellationToken ct = default)
    {
        ExportProfiler.Reset();
        var sw = Stopwatch.StartNew();
        Directory.CreateDirectory(outDir);

        _ = ws.Stubs;
        _ = ws.Decompiler;
        _ = ws.Cpp;
        var sharedBcl = ws.Decompiler.Bcl;
        var materializedRefs = new ConcurrentDictionary<string, Lazy<string?>>(StringComparer.OrdinalIgnoreCase);
        var refsDir = Path.GetFullPath(Path.Combine(outDir, "_references"));

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
            {
                var tLift = ExportProfiler.Start();
                provider.EnsureLifted(allMethods);
                ExportProfiler.RecordPreLift(tLift);
            }
            preLifted = true;
        }

        var plans = new AssemblyPlan[assemblies.Count];
        for (var i = 0; i < assemblies.Count; i++)
        {
            var asm = assemblies[i];
            var dir = Path.Combine(outDir, WorkspaceSerializer.Sanitize(asm.Name));
            var image = ws.Stubs.FindImage(asm.ImageName);
            var substituted = ws.SubstituteBcl && BclReferenceProvider.IsBclAssembly(asm.Name);
            var topLevel = asm.Namespaces.SelectMany(n => n.Types).ToList();
            var lifted = 0;
            if (!substituted && ws.BodyProvider is { } bp)
            {
                var all = new List<int>();
                void CollectAsm(TypeModel t) { all.AddRange(t.Methods.Select(m => m.Index)); foreach (var n in t.NestedTypes) CollectAsm(n); }
                topLevel.ForEach(CollectAsm);
                lifted = preLifted && bp is Lifting.LiftingBodyProvider lbp
                    ? all.Distinct().Count(m => lbp.Results.TryGetValue(m, out var r) && r.Success)
                    : bp.EnsureLifted(all);
            }
            plans[i] = new AssemblyPlan(asm, dir, Path.Combine(dir, "csharp"), Path.Combine(dir, "il"), image, substituted, topLevel, lifted);
        }

        var createdDirs = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
        void EnsureDir(string path)
        {
            if (createdDirs.TryAdd(path, 0))
                Directory.CreateDirectory(path);
        }

        foreach (var p in plans)
        {
            EnsureDir(p.Dir);
            if (p.Image < 0 || (filter is null && p.Substituted)) continue;
            foreach (var type in p.TopLevel)
            {
                var nsDir = type.Namespace.Length == 0 ? "-" : WorkspaceSerializer.Sanitize(type.Namespace);
                EnsureDir(Path.Combine(p.CsDir, nsDir));
                if (includeIl && !p.Substituted)
                    EnsureDir(Path.Combine(p.IlDir, nsDir));
            }
        }

        var writeChannel = Channel.CreateBounded<(string Path, string Content)>(new BoundedChannelOptions(512)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false,
        });
        var writerTasks = new Task[4];
        for (var w = 0; w < writerTasks.Length; w++)
        {
            writerTasks[w] = Task.Run(async () =>
            {
                await foreach (var (path, content) in writeChannel.Reader.ReadAllAsync(ct).ConfigureAwait(false))
                {
                    var tIo = ExportProfiler.Start();
                    WriteUtf8Fast(path, content);
                    ExportProfiler.RecordFileWrite(tIo);
                }
            }, CancellationToken.None);
        }

        void EnqueueWrite(string path, string content)
        {
            var item = (path, content);
            while (!writeChannel.Writer.TryWrite(item))
            {
                ct.ThrowIfCancellationRequested();
                if (!writeChannel.Writer.WaitToWriteAsync(ct).AsTask().GetAwaiter().GetResult())
                    throw new OperationCanceledException(ct);
            }
        }

        var failedInTry = false;
        try
        {
            var parallelOpts = new ParallelOptions { CancellationToken = ct };

            progress?.Report($"Generating headers and decompiling {plans.Length} assemblies…");
            Parallel.ForEach(plans, parallelOpts, p =>
            {
                try
                {
                    var cppPath = Path.Combine(p.Dir, WorkspaceSerializer.Sanitize(p.Asm.Name) + ".h");
                    var tCpp = ExportProfiler.Start();
                    var header = ws.Cpp.GenerateImageHeader(p.Asm);
                    ExportProfiler.RecordCppHeader(tCpp);
                    EnqueueWrite(cppPath, header);
                    p.CppPath = cppPath;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    ws.Sink.Warn(DiagnosticCategory.Extraction, DiagnosticCodes.ExtractionEntryFailed,
                        $"C++ header for {p.Asm.Name}: {ex.Message}", new() { AssemblyName = p.Asm.Name }, ex);
                }
            });

            Parallel.ForEach(plans, parallelOpts, p =>
            {
                if (p.Substituted || p.Image < 0) return;
                if (ws.Decompiler.GetStubImage(p.Image) is { } bytes)
                {
                    var stubPath = Path.Combine(p.Dir, p.Asm.ImageName);
                    File.WriteAllBytes(stubPath, bytes);
                    p.StubPath = stubPath;
                }
            });

            var batchPlans = new List<AssemblyPlan>(plans.Length);
            var requests = new List<DecompilerService.BatchImageRequest>(plans.Length);
            foreach (var p in plans)
            {
                if (p.Image < 0) continue;
                if (filter is null && p.Substituted) continue;
                batchPlans.Add(p);
                requests.Add(new DecompilerService.BatchImageRequest(
                    p.Image, p.Asm.ImageName, p.TopLevel, includeIl && !p.Substituted, p.Substituted));
            }
            if (requests.Count > 0)
            {
                ws.Decompiler.DecompileImagesBatch(requests, (reqIdx, type, cs, il) =>
                {
                    var plan = batchPlans[reqIdx];
                    var rel = Path.Combine(type.Namespace.Length == 0 ? "-" : WorkspaceSerializer.Sanitize(type.Namespace), WorkspaceSerializer.Sanitize(type.Name));
                    EnqueueWrite(Path.Combine(plan.CsDir, rel + ".cs"), cs);
                    if (il.Length > 0) EnqueueWrite(Path.Combine(plan.IlDir, rel + ".il"), il);
                }, ct);
            }

            // Pre-materialize the union of unique references once in parallel so csproj emission has zero lock contention.
            var tRefsPre = ExportProfiler.Start();
            ProjectReferences.WarmAll(ws, plans.Where(p => p.Image >= 0 && !p.Substituted).Select(p => p.Asm), refsDir, sharedBcl, materializedRefs);
            ExportProfiler.RecordProjectRefs(tRefsPre);

            Parallel.ForEach(plans, parallelOpts, p =>
            {
                if (p.Image < 0 || p.Substituted) return;
                try
                {
                    var tRefs = ExportProfiler.Start();
                    var refs = ProjectReferences.Materialize(ws, p.Asm, refsDir, sharedBcl, materializedRefs);
                    ProjectFileWriter.Write(p.Dir, p.Asm.Name, refs);
                    ExportProfiler.RecordProjectRefs(tRefs);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    ws.Sink.Warn(DiagnosticCategory.Extraction, DiagnosticCodes.ExtractionEntryFailed,
                        $"Project file for {p.Asm.Name}: {ex.Message}", new() { AssemblyName = p.Asm.Name }, ex);
                }
            });
        }
        catch
        {
            failedInTry = true;
            throw;
        }
        finally
        {
            writeChannel.Writer.TryComplete();
            try
            {
                Task.WhenAll(writerTasks).GetAwaiter().GetResult();
            }
            catch when (failedInTry)
            {
            }
        }

        var results = new List<ExportedAssembly>(plans.Length);
        foreach (var p in plans)
            results.Add(new ExportedAssembly(p.Asm.Name, p.Substituted, p.Asm.TypeCount, p.Lifted, p.StubPath, p.CsDir, p.CppPath));

        var tArt = ExportProfiler.Start();
        var artifacts = ws.ExportArtifacts(Path.Combine(outDir, "_artifacts"), includeRawSections: true);
        ExportProfiler.RecordArtifacts(tArt);
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
        ExportProfiler.Report(sw.Elapsed);
        return summary;
    }

    private static void WriteUtf8Fast(string path, string content)
    {
        if (content.Length == 0)
        {
            using var emptyHandle = File.OpenHandle(path, FileMode.Create, FileAccess.Write, FileShare.None, FileOptions.None);
            return;
        }

        var maxBytes = Encoding.UTF8.GetMaxByteCount(content.Length);
        var buf = ArrayPool<byte>.Shared.Rent(maxBytes);
        try
        {
            var count = Encoding.UTF8.GetBytes(content.AsSpan(), buf);
            using var handle = File.OpenHandle(path, FileMode.Create, FileAccess.Write, FileShare.None, FileOptions.None);
            if (count > 0)
                RandomAccess.Write(handle, buf.AsSpan(0, count), 0);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buf);
        }
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
            var tCpp = ExportProfiler.Start();
            timer.Measure("c++ header", () => File.WriteAllText(cppPath, ws.Cpp.GenerateImageHeader(asm)));
            ExportProfiler.RecordCppHeader(tCpp);
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
                var tIo = ExportProfiler.Start();
                File.WriteAllText(Path.Combine(csDir, rel + ".cs"), cs);
                if (il.Length > 0) File.WriteAllText(Path.Combine(ilDir, rel + ".il"), il);
                ExportProfiler.RecordFileWrite(tIo);
            }, ct, maxDecompileDegree);
            timer.Add("decompile+write", decompileWatch.Elapsed);
            if (!substituted)
            {
                try
                {
                    var tRefs = ExportProfiler.Start();
                    var refs = ProjectReferences.Materialize(ws, asm, refsDir, sharedBcl, materializedRefs);
                    ProjectFileWriter.Write(dir, asm.Name, refs);
                    ExportProfiler.RecordProjectRefs(tRefs);
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
