using System.Collections.Concurrent;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>Per-target call-site statistics gathered by <see cref="CallSiteScanner"/>.</summary>
public sealed class CallSiteStats
{
    public int Sites, WriteBarrierShape, FollowedByInt3, FollowedByTrap, RcxSetJustBefore, RdxFromTypeInfo, RcxLeaUsage, RaxUsedAfter, FollowedByIndirectCallAtRax, RcxLeaStack, RcxFromTypeInfo;
    public readonly ConcurrentDictionary<ulong, int> NextCalls = new();
    public void Inc(ref int field) => Interlocked.Increment(ref field);
}

/// <summary>
/// Scans the code of every metadata method and records, for each call target that is not itself a metadata
/// method, how it is called. Thresholds over these counts classify helpers on any Unity version.
/// </summary>
public static class CallSiteScanner
{
    public static ConcurrentDictionary<ulong, CallSiteStats> Scan(NativeSymbolizer sym, Disassembly.X64Disassembler disasm, NativeAbi abi = NativeAbi.Win64)
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
                Record(stats.GetOrAdd(target, _ => new CallSiteStats()), ins, i, sym, ArgumentLocations.IntArg(0, abi), ArgumentLocations.IntArg(1, abi));
            }
        });
        return stats;
    }

    private static void Record(CallSiteStats s, IReadOnlyList<Instruction> ins, int i, NativeSymbolizer sym, Register a0, Register a1)
    {
        s.Inc(ref s.Sites);
        if (i + 1 < ins.Count && ins[i + 1].Mnemonic == Mnemonic.Int3) s.Inc(ref s.FollowedByInt3);
        // Cold trap blocks chain several no-return calls back to back (call A; call B; int3).
        // Clang emits no int3 after a no-return call: the next bytes are another trap call or nop padding.
        if (i + 1 >= ins.Count || ins[i + 1].Mnemonic is Mnemonic.Int3 or Mnemonic.Call or Mnemonic.Nop or Mnemonic.Ud2) s.Inc(ref s.FollowedByTrap);
        if (i > 0 && ins[i - 1].Op0Kind == OpKind.Register && ins[i - 1].Op0Register.GetFullRegister() == a0) s.Inc(ref s.RcxSetJustBefore);

        // Write barrier: `mov [B+D], r` and `lea rcx, [B+D]` among the preceding instructions.
        Instruction? lea = null;
        for (var k = i - 1; k >= Math.Max(0, i - 4); k--)
            if (ins[k].Mnemonic == Mnemonic.Lea && ins[k].Op0Register == a0 && !ins[k].IsIPRelativeMemoryOperand) { lea = ins[k]; break; }
        if (lea is { } l)
            for (var k = i - 1; k >= Math.Max(0, i - 5); k--)
                if (ins[k].Mnemonic == Mnemonic.Mov && ins[k].Op0Kind == OpKind.Memory && ins[k].MemoryBase == l.MemoryBase && ins[k].MemorySize.GetSize() == 8
                    && ins[k].MemoryIndex == l.MemoryIndex && ins[k].MemoryDisplacement64 == l.MemoryDisplacement64 + LeaAdjust(ins, i, a0)) { s.Inc(ref s.WriteBarrierShape); break; }

        if (SourceUsage(ins, i, a1, sym) is { Kind: MetadataUsageKind.TypeInfo }) s.Inc(ref s.RdxFromTypeInfo);
        if (SourceUsage(ins, i, a0, sym) is { Kind: MetadataUsageKind.TypeInfo }) s.Inc(ref s.RcxFromTypeInfo);
        if (i > 0 && ins[i - 1].Mnemonic == Mnemonic.Lea && ins[i - 1].Op0Register == a0 && ins[i - 1].MemoryBase == Register.RSP)
            s.Inc(ref s.RcxLeaStack);
        for (var k = i - 1; k >= Math.Max(0, i - 3); k--)
            if (ins[k].Op0Register == a0 && ins[k].IsIPRelativeMemoryOperand
                && (ins[k].Mnemonic == Mnemonic.Lea && sym.TryGetUsage(ins[k].IPRelativeMemoryAddress, out _)
                    || ins[k].Mnemonic == Mnemonic.Mov && sym.TryGetUsage(sym.Registration.Reader.ReadPointer(ins[k].IPRelativeMemoryAddress), out _)))   // PIC: GOT slot → usage
            { s.Inc(ref s.RcxLeaUsage); break; }

        var raxSeen = false;
        for (var k = i + 1; k < Math.Min(ins.Count, i + 10); k++)
        {
            if (!raxSeen && (ins[k].Op1Kind == OpKind.Register && ins[k].Op1Register.GetFullRegister() == Register.RAX
                || ins[k].Mnemonic == Mnemonic.Test && ins[k].Op0Register.GetFullRegister() == Register.RAX)) { s.Inc(ref s.RaxUsedAfter); raxSeen = true; }
            if (ins[k].Mnemonic == Mnemonic.Call)
            {
                if (ins[k].Op0Kind == OpKind.Memory && ins[k].MemoryBase == Register.RAX && ins[k].MemoryDisplacement64 == 0)
                    s.Inc(ref s.FollowedByIndirectCallAtRax);
                if (ins[k].Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32) s.NextCalls.AddOrUpdate(ins[k].NearBranchTarget, 1, (_, n) => n + 1);
                break;
            }
        }
    }

    /// <summary>Clang splits the barrier address: <c>lea rdi, [rax+rcx*8]; add rdi, 20h</c>. Sum of such adds before the call.</summary>
    private static ulong LeaAdjust(IReadOnlyList<Instruction> ins, int call, Register a0)
    {
        ulong sum = 0;
        for (var k = call - 1; k >= Math.Max(0, call - 4); k--)
        {
            if (ins[k].Mnemonic == Mnemonic.Lea && ins[k].Op0Register == a0) break;
            if (ins[k].Mnemonic == Mnemonic.Add && ins[k].Op0Kind == OpKind.Register && ins[k].Op0Register == a0 && ins[k].Op1Kind != OpKind.Register)
                sum += ins[k].GetImmediate(1);
        }
        return sum;
    }

    /// <summary>Runtime-metadata usage a register was loaded from shortly before index <paramref name="i"/> (one reg→reg hop).</summary>
    public static MetadataUsage? SourceUsage(IReadOnlyList<Instruction> ins, int i, Register reg, NativeSymbolizer sym, int window = 8)
    {
        var deref = false;   // PIC: `mov rdi, [rip+GOT]; mov rdi, [rdi]` - the GOT slot points at the usage global
        for (var k = i - 1; k >= Math.Max(0, i - window); k--)
        {
            var x = ins[k];
            if (x.Op0Kind != OpKind.Register || x.Op0Register.GetFullRegister() != reg) continue;
            if (x.Mnemonic == Mnemonic.Mov && x.IsIPRelativeMemoryOperand)
            {
                if (!deref && sym.TryGetUsage(x.IPRelativeMemoryAddress, out var u)) return u;
                if (deref && sym.TryGetUsage(sym.Registration.Reader.ReadPointer(x.IPRelativeMemoryAddress), out var gu)) return gu;
                return null;
            }
            if (x.Mnemonic == Mnemonic.Mov && x.Op1Kind == OpKind.Register) { reg = x.Op1Register.GetFullRegister(); continue; }
            if (!deref && x.Mnemonic == Mnemonic.Mov && x.Op1Kind == OpKind.Memory && x.MemoryIndex == Register.None && x.MemoryDisplacement64 == 0
                && x.MemoryBase != Register.RIP) { reg = x.MemoryBase.GetFullRegister(); deref = true; continue; }
            return null;
        }
        return null;
    }
}
