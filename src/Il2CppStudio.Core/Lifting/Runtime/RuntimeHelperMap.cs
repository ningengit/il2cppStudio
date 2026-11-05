using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Disassembly;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>
/// Address → runtime helper classification for one binary. API anchors win; otherwise call-site statistics
/// decide (thresholds require ≥ 80 % of sites to agree, with at least 2 sites). Every classification is logged
/// with its evidence and written to <c>runtime-helpers.json</c> by the export.
/// </summary>
public sealed class RuntimeHelperMap
{
    private readonly Dictionary<ulong, RuntimeHelperInfo> _byVa = [];
    public IReadOnlyCollection<RuntimeHelperInfo> Helpers => _byVa.Values;

    /// <summary>Raw call-site statistics of every non-metadata call target (for diagnostics).</summary>
    public IReadOnlyDictionary<ulong, CallSiteStats> Stats { get; private set; } = new Dictionary<ulong, CallSiteStats>();

    public RuntimeHelperKind KindOf(ulong va) => _byVa.TryGetValue(va, out var h) ? h.Kind : RuntimeHelperKind.Unknown;

    /// <summary>Call targets that are exported il2cpp_* API functions, by name (used to label native calls).</summary>
    public Dictionary<ulong, string> ExportNames { get; } = [];

    public static RuntimeHelperMap Build(NativeSymbolizer sym, X64Disassembler disasm, IEnumerable<(string Name, ulong Va)> exports, IDiagnosticSink sink)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var reader = sym.Registration.Reader;
        var exportNames = new Dictionary<ulong, string>();
        foreach (var (name, eva) in exports) exportNames.TryAdd(eva, name);
        var anchors = RuntimeHelperAnchors.Build(reader, exportNames.Select(e => (e.Value, e.Key)));
        var stats = CallSiteScanner.Scan(sym, disasm);
        var map = new RuntimeHelperMap { Stats = stats };

        foreach (var (va, s) in stats)
        {
            var impl = RuntimeHelperAnchors.ResolveImplementation(reader, va);
            if (anchors.TryGetValue(impl, out var a) || anchors.TryGetValue(va, out a))
                map.Add(va, a.Kind, $"thunk chain reaches {a.Api} (impl 0x{impl - reader.ImageBase:X})", s.Sites);
            else if (Agrees(s.WriteBarrierShape, s, 0.5) && s.FollowedByInt3 == 0)
                map.Add(va, RuntimeHelperKind.WriteBarrier, $"{s.WriteBarrierShape}/{s.Sites} sites: store [B+D] then lea rcx,[B+D]", s.Sites);
            else if (s.Sites >= 2 && Ratio(s.RcxLeaUsage, s) >= 0.8)
                map.Add(va, RuntimeHelperKind.MetadataInit, $"{s.RcxLeaUsage}/{s.Sites} sites: lea rcx,[metadata usage]", s.Sites);
            else if (s.Sites >= 2 && Ratio(s.FollowedByTrap, s) >= 0.8 && Ratio(s.RaxUsedAfter, s) < 0.1)
                map.Add(va, Ratio(s.RcxSetJustBefore, s) >= 0.5 ? RuntimeHelperKind.Raise : RuntimeHelperKind.ThrowHelper,
                    $"{s.FollowedByTrap}/{s.Sites} sites end in int3/another trap call (no return), rcx set before {s.RcxSetJustBefore}/{s.Sites}", s.Sites);
            else if (Agrees(s.RdxFromTypeInfo, s, 0.4) && Ratio(s.RaxUsedAfter, s) >= 0.8 && s.FollowedByInt3 == 0)
                map.Add(va, RuntimeHelperKind.Cast, $"{s.RdxFromTypeInfo}/{s.Sites} sites pass a TypeInfo in rdx, result used {s.RaxUsedAfter}/{s.Sites}", s.Sites);
            if (exportNames.TryGetValue(va, out var api)) map.ExportNames[va] = api;
        }

        // Exception factories: result feeds a Raise call at most sites.
        foreach (var (va, s) in stats)
        {
            if (map._byVa.ContainsKey(va) || s.Sites < 2) continue;
            var toRaise = s.NextCalls.Where(n => map.KindOf(n.Key) == RuntimeHelperKind.Raise).Sum(n => n.Value);
            if (toRaise >= 0.8 * s.Sites) map.Add(va, RuntimeHelperKind.ExceptionFactory, $"{toRaise}/{s.Sites} sites pass the result to raise_exception", s.Sites);
        }

        foreach (var h in map._byVa.Values.OrderByDescending(h => h.CallSites))
            sink.Debug(DiagnosticCategory.Registration, DiagnosticCodes.RuntimeHelperIdentified,
                $"sub_{h.Va - reader.ImageBase:X} = {h.Kind} ({h.CallSites} call sites; {h.Evidence})", new() { Rva = h.Va - reader.ImageBase });
        sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.RuntimeHelperIdentified,
            $"Identified {map._byVa.Count} runtime helpers from {stats.Count:N0} non-metadata call targets in {sw.Elapsed.TotalSeconds:F1}s: " +
            string.Join(", ", map._byVa.Values.GroupBy(h => h.Kind).OrderBy(g => g.Key).Select(g => $"{g.Key}×{g.Count()}")));
        return map;
    }

    private void Add(ulong va, RuntimeHelperKind kind, string evidence, int sites) => _byVa[va] = new RuntimeHelperInfo(va, kind, evidence, sites);

    /// <summary>Loose threshold only with enough evidence (= 10 sites); small samples need 80 % agreement.</summary>
    private static bool Agrees(int n, CallSiteStats s, double loose) => s.Sites >= 2 && Ratio(n, s) >= (s.Sites >= 10 ? loose : 0.8);

    private static double Ratio(int n, CallSiteStats s) => s.Sites == 0 ? 0 : (double)n / s.Sites;
}
