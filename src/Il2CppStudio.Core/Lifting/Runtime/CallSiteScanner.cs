using System.Collections.Concurrent;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>Per-target call-site statistics gathered by <see cref="CallSiteScanner"/>.</summary>
public sealed class CallSiteStats
{
    public int Sites, WriteBarrierShape, FollowedByInt3, FollowedByTrap, RcxSetJustBefore, RdxFromTypeInfo, RcxLeaUsage, RaxUsedAfter;
    public readonly ConcurrentDictionary<ulong, int> NextCalls = new();
    public void Inc(ref int field) => Interlocked.Increment(ref field);
}

/// <summary>
/// Scans the code of every metadata method and records, for each call target that is not itself a metadata
/// method, how it is called. Thresholds over these counts classify helpers on any Unity version.
/// </summary>
public static class CallSiteScanner
{
    public static ConcurrentDictionary<ulong, CallSiteStats> Scan(NativeSymbolizer sym, Disassembly.X64Disassembler disasm)
    {
        var stats = new ConcurrentDictionary<ulong, CallSiteStats>();
        var starts = sym.Map.Symbols.Values.Where(s => s.MethodSpecIndex < 0).Select(s => s.Va).Distinct().ToList();
        Parallel.ForEach(starts, va =>
        {
            var ins = disasm.Disassemble(va, 0x4000).Instructions;
            for (var i = 0; i < ins.Count; i++)
            {
                if (ins[i].Mnemonic != Mnemonic.Call || ins[i].Op0Kind is not (OpKind.NearBranch64 or OpKind.NearBranch32)) continue;
                var target = ins[i].NearBranchTarget;
                if (sym.Map.SymbolAt(target) is not null) continue;
                Record(stats.GetOrAdd(target, _ => new CallSiteStats()), ins, i, sym);
            }
        });
        return stats;
    }

    private static void Record(CallSiteStats s, IReadOnlyList<Instruction> ins, int i, NativeSymbolizer sym)
    {
        s.Inc(ref s.Sites);
        if (i + 1 < ins.Count && ins[i + 1].Mnemonic == Mnemonic.Int3) s.Inc(ref s.FollowedByInt3);
        // Cold trap blocks chain several no-return calls back to back (call A; call B; int3).
        if (i + 1 >= ins.Count || ins[i + 1].Mnemonic is Mnemonic.Int3 or Mnemonic.Call) s.Inc(ref s.FollowedByTrap);
        if (i > 0 && ins[i - 1].Op0Kind == OpKind.Register && ins[i - 1].Op0Register == Register.RCX) s.Inc(ref s.RcxSetJustBefore);

        // Write barrier: `mov [B+D], r` and `lea rcx, [B+D]` among the preceding instructions.
        Instruction? lea = null;
        for (var k = i - 1; k >= Math.Max(0, i - 4); k--)
            if (ins[k].Mnemonic == Mnemonic.Lea && ins[k].Op0Register == Register.RCX && !ins[k].IsIPRelativeMemoryOperand) { lea = ins[k]; break; }
        if (lea is { } l)
            for (var k = i - 1; k >= Math.Max(0, i - 5); k--)
                if (ins[k].Mnemonic == Mnemonic.Mov && ins[k].Op0Kind == OpKind.Memory && ins[k].MemoryBase == l.MemoryBase
                    && ins[k].MemoryDisplacement64 == l.MemoryDisplacement64 && ins[k].MemorySize.GetSize() == 8) { s.Inc(ref s.WriteBarrierShape); break; }

        if (SourceUsage(ins, i, Register.RDX, sym) is { Kind: MetadataUsageKind.TypeInfo }) s.Inc(ref s.RdxFromTypeInfo);
        for (var k = i - 1; k >= Math.Max(0, i - 3); k--)
            if (ins[k].Mnemonic == Mnemonic.Lea && ins[k].Op0Register == Register.RCX && ins[k].IsIPRelativeMemoryOperand
                && sym.TryGetUsage(ins[k].IPRelativeMemoryAddress, out _)) { s.Inc(ref s.RcxLeaUsage); break; }

        var raxSeen = false;
        for (var k = i + 1; k < Math.Min(ins.Count, i + 5); k++)
        {
            if (!raxSeen && (ins[k].Op1Kind == OpKind.Register && ins[k].Op1Register.GetFullRegister() == Register.RAX
                || ins[k].Mnemonic == Mnemonic.Test && ins[k].Op0Register.GetFullRegister() == Register.RAX)) { s.Inc(ref s.RaxUsedAfter); raxSeen = true; }
            if (ins[k].Mnemonic == Mnemonic.Call)
            {
                if (ins[k].Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32) s.NextCalls.AddOrUpdate(ins[k].NearBranchTarget, 1, (_, n) => n + 1);
                break;
            }
        }
    }

    /// <summary>Runtime-metadata usage a register was loaded from shortly before index <paramref name="i"/> (one reg→reg hop).</summary>
    public static MetadataUsage? SourceUsage(IReadOnlyList<Instruction> ins, int i, Register reg, NativeSymbolizer sym, int window = 8)
    {
        for (var k = i - 1; k >= Math.Max(0, i - window); k--)
        {
            var x = ins[k];
            if (x.Op0Kind != OpKind.Register || x.Op0Register.GetFullRegister() != reg) continue;
            if (x.Mnemonic == Mnemonic.Mov && x.IsIPRelativeMemoryOperand && sym.TryGetUsage(x.IPRelativeMemoryAddress, out var u)) return u;
            if (x.Mnemonic == Mnemonic.Mov && x.Op1Kind == OpKind.Register) { reg = x.Op1Register.GetFullRegister(); continue; }
            return null;
        }
        return null;
    }
}
