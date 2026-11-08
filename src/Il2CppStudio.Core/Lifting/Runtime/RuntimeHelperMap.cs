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

    public static RuntimeHelperMap Build(NativeSymbolizer sym, X64Disassembler disasm, IEnumerable<(string Name, ulong Va)> exports, IDiagnosticSink sink, NativeAbi abi = NativeAbi.Win64)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var reader = sym.Registration.Reader;
        var exportNames = new Dictionary<ulong, string>();
        foreach (var (name, eva) in exports) exportNames.TryAdd(eva, name);
        var anchors = RuntimeHelperAnchors.Build(reader, exportNames.Select(e => (e.Value, e.Key)));
        var stats = CallSiteScanner.Scan(sym, disasm, abi);
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
            else if (s.Sites >= 2 && Ratio(s.FollowedByIndirectCallAtRax, s) >= 0.5)
                map.Add(va, RuntimeHelperKind.InterfaceLookup, $"{s.FollowedByIndirectCallAtRax}/{s.Sites} sites followed by call [rax]", s.Sites);
            else if (s.Sites >= 2 && Ratio(s.RcxLeaStack, s) >= 0.9 && s.RaxUsedAfter == 0 && s.FollowedByInt3 == 0 && IsScopeGuardDestructor(disasm, va))
                map.Add(va, RuntimeHelperKind.ScopeCleanup, $"{s.RcxLeaStack}/{s.Sites} sites pass lea rcx,[rsp+d] to scope-guard destructor", s.Sites);
            else if (Agrees(s.RdxFromTypeInfo, s, 0.4) && Ratio(s.RaxUsedAfter, s) >= 0.8 && s.FollowedByInt3 == 0)
                map.Add(va, RuntimeHelperKind.Cast, $"{s.RdxFromTypeInfo}/{s.Sites} sites pass a TypeInfo in rdx, result used {s.RaxUsedAfter}/{s.Sites}", s.Sites);
            else if (abi == Lifting.NativeAbi.SysV && Agrees(s.RcxFromTypeInfo, s, 0.5) && Ratio(s.RaxUsedAfter, s) >= 0.8 && Ratio(s.RdxFromTypeInfo, s) < 0.1)
                // Clang/ELF: the codegen allocator is not reached through the exported il2cpp_object_new thunk chain.
                map.Add(va, RuntimeHelperKind.ObjectNew, $"{s.RcxFromTypeInfo}/{s.Sites} sites pass a TypeInfo as the only argument, result used {s.RaxUsedAfter}/{s.Sites}", s.Sites);
            else if (s.Sites >= 10 && Ratio(s.RaxUsedAfter, s) < 0.1 && NeverReturns(disasm, va))
                map.Add(va, Ratio(s.RcxSetJustBefore, s) >= 0.5 ? RuntimeHelperKind.Raise : RuntimeHelperKind.ThrowHelper,
                    $"body has no return path (no ret / indirect tail jump); result unused at {s.Sites - s.RaxUsedAfter}/{s.Sites} sites", s.Sites);
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

    /// <summary>
    /// No-return helper (raise / throw null-reference / index-out-of-range): following direct jumps (thunks), the body
    /// contains no <c>ret</c> and no indirect jump, and ends in calls. Compilers without trap padding (Clang) need this,
    /// since call sites are not followed by <c>int3</c>.
    /// </summary>
    private static bool NeverReturns(X64Disassembler disasm, ulong va)
    {
        for (var hop = 0; hop < 3; hop++)
        {
            var fn = disasm.Disassemble(va, 0x400).Instructions;
            if (fn.Count == 0) return false;
            if (fn.Count <= 2 && fn[0].Mnemonic == Iced.Intel.Mnemonic.Jmp && fn[0].Op0Kind is Iced.Intel.OpKind.NearBranch64 or Iced.Intel.OpKind.NearBranch32)
            { va = fn[0].NearBranchTarget; continue; }
            var calls = 0;
            ulong furthestTarget = 0;
            for (var k = 0; k < fn.Count; k++)
            {
                var i = fn[k];
                if (i.Mnemonic is Iced.Intel.Mnemonic.Ret or Iced.Intel.Mnemonic.Retf) return false;
                if (i.Mnemonic == Iced.Intel.Mnemonic.Jmp && i.Op0Kind is not (Iced.Intel.OpKind.NearBranch64 or Iced.Intel.OpKind.NearBranch32)) return false;
                if (i.FlowControl is Iced.Intel.FlowControl.ConditionalBranch or Iced.Intel.FlowControl.UnconditionalBranch
                    && i.Op0Kind is Iced.Intel.OpKind.NearBranch64 or Iced.Intel.OpKind.NearBranch32)
                {
                    if (i.NearBranchTarget < fn[0].IP || i.NearBranchTarget > fn[^1].IP) return false;   // tail jump elsewhere may return
                    furthestTarget = Math.Max(furthestTarget, i.NearBranchTarget);
                }
                if (i.Mnemonic != Iced.Intel.Mnemonic.Call) continue;
                calls++;
                // Function end without a function table: a call followed by padding with no branch beyond it.
                var next = k + 1 < fn.Count ? fn[k + 1] : default;
                if ((k + 1 >= fn.Count || next.Mnemonic is Iced.Intel.Mnemonic.Nop or Iced.Intel.Mnemonic.Int3 or Iced.Intel.Mnemonic.Ud2) && furthestTarget <= i.IP)
                    return true;
            }
            return calls > 0 && fn.Count < 200;
        }
        return false;
    }

    private static bool IsScopeGuardDestructor(X64Disassembler disasm, ulong va)
    {
        var ins = disasm.Disassemble(va, 0x200).Instructions;
        return ins.Any(i => i.Mnemonic == Iced.Intel.Mnemonic.Movzx && i.Op1Kind == Iced.Intel.OpKind.Memory && (long)i.MemoryDisplacement64 == 0x12E);
    }
}
