using System.Collections.Concurrent;
using System.Text;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Disassembly;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>
/// Resolves call targets that are the real bodies of value-type instance methods. The registered method pointer of
/// such a method is an adjustor thunk <c>T_M_AdjustorThunk(obj) =&gt; T_M(obj + 1)</c> (unboxing <c>this</c>), while
/// direct calls go to <c>T_M</c>. MSVC either keeps the thunk as <c>add rcx, 10h; jmp T_M</c> or inlines <c>T_M</c> into it,
/// in which case the thunk is the real body with every object-relative displacement shifted by +0x10. Matching
/// instruction forms exactly and requiring each displacement pair to be equal or differ by exactly 0x10 (with at least
/// one shifted) identifies <c>T_M</c> without names or version-specific offsets.
/// </summary>
public static class AdjustorThunkMatcher
{
    private const int ObjectHeader = 0x10;

    public static IReadOnlyList<InlineCopyMatch> Match(NativeSymbolizer sym, X64Disassembler disasm, IReadOnlyList<ulong> unmatched)
    {
        if (unmatched.Count == 0) return [];
        var byShape = new Dictionary<string, List<(ulong Va, List<long> Disps, int Count)>>(StringComparer.Ordinal);
        var wanted = unmatched.ToHashSet();
        foreach (var va in unmatched)
        {
            var fn = disasm.Disassemble(va, 0x2000);
            if (fn.Truncated || fn.Instructions.Count < 3) continue;
            var (shape, disps) = Shape(fn);
            (byShape.TryGetValue(shape, out var l) ? l : byShape[shape] = []).Add((va, disps, fn.Instructions.Count));
        }

        var matches = new ConcurrentBag<InlineCopyMatch>();
        Parallel.ForEach(sym.Map.Symbols.Values.Where(s => IsValueTypeInstance(sym, s)), s =>
        {
            var fn = disasm.Disassemble(s.Va, 0x2000);
            if (fn.Truncated || fn.Instructions.Count == 0) return;
            // Kept thunk: [sub rsp..] add rcx, 10h; jmp target
            var ins = fn.Instructions;
            for (var k = 0; k + 1 < ins.Count && k < 3; k++)
                if (ins[k].Mnemonic == Mnemonic.Add && ins[k].Op0Register == Register.RCX && ins[k].GetImmediate(1) == ObjectHeader
                    && ins[k + 1].Mnemonic == Mnemonic.Jmp && ins[k + 1].Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32
                    && wanted.Contains(ins[k + 1].NearBranchTarget))
                {
                    matches.Add(new InlineCopyMatch(ins[k + 1].NearBranchTarget, s, ins.Count));
                    return;
                }
            var (shape, disps) = Shape(fn);
            if (!byShape.TryGetValue(shape, out var cands)) return;
            foreach (var c in cands)
                if (Shifted(disps, c.Disps)) matches.Add(new InlineCopyMatch(c.Va, s, c.Count));
        });
        return [.. matches.GroupBy(m => m.Va).Where(g => g.Select(m => m.Original.MethodIndex).Distinct().Count() == 1).Select(g => g.First())];
    }

    private static bool IsValueTypeInstance(NativeSymbolizer sym, NativeSymbol s)
    {
        var mi = s.MethodIndex;
        if ((uint)mi >= (uint)sym.Metadata.Methods.Length) return false;
        var m = sym.Metadata.Methods[mi];
        if ((m.Flags & 0x10) != 0 || (uint)m.DeclaringType >= (uint)sym.Metadata.TypeDefinitions.Length) return false;
        return sym.Metadata.TypeDefinitions[m.DeclaringType].IsValueType;
    }

    /// <summary>thunk displacement = real + 0x10 or equal, for every non-stack memory operand; at least one shifted.</summary>
    private static bool Shifted(List<long> thunk, List<long> real)
    {
        if (thunk.Count != real.Count) return false;
        var any = false;
        for (var i = 0; i < thunk.Count; i++)
        {
            if (thunk[i] == real[i]) continue;
            if (thunk[i] != real[i] + ObjectHeader) return false;
            any = true;
        }
        return any;
    }

    /// <summary>Instruction forms with non-stack displacements removed and internal branches by instruction index.</summary>
    private static (string Shape, List<long> Disps) Shape(DisassembledFunction fn)
    {
        var ins = fn.Instructions;
        var index = new Dictionary<ulong, int>();
        for (var i = 0; i < ins.Count; i++) index[ins[i].IP] = i;
        var sb = new StringBuilder(ins.Count * 16);
        var disps = new List<long>();
        foreach (var i in ins)
        {
            if (i.Mnemonic == Mnemonic.Int3) break;
            sb.Append((int)i.Mnemonic).Append(':');
            for (var op = 0; op < i.OpCount; op++)
            {
                switch (i.GetOpKind(op))
                {
                    case OpKind.Register: sb.Append('r').Append((int)i.GetOpRegister(op)); break;
                    case OpKind.NearBranch16 or OpKind.NearBranch32 or OpKind.NearBranch64:
                        var t = i.NearBranchTarget;
                        sb.Append(index.TryGetValue(t, out var ti) ? $"L{ti}" : $"A{t:X}");
                        break;
                    case OpKind.Memory when i.IsIPRelativeMemoryOperand:
                        sb.Append(i.MemorySize == MemorySize.UInt8 ? "Mflag" : $"M{i.IPRelativeMemoryAddress:X}");
                        break;
                    case OpKind.Memory when i.MemoryBase is Register.RSP or Register.RBP:
                        sb.Append($"s{(int)i.MemoryBase},{(long)i.MemoryDisplacement64:X},{(int)i.MemorySize}");
                        break;
                    case OpKind.Memory:
                        sb.Append($"m{(int)i.MemoryBase},{(int)i.MemoryIndex},{i.MemoryIndexScale},{(int)i.MemorySize}");
                        disps.Add((long)i.MemoryDisplacement64);
                        break;
                    default:
                        sb.Append('i').Append(i.GetOpKind(op) is OpKind.Immediate8 or OpKind.Immediate16 or OpKind.Immediate32 or OpKind.Immediate64
                            or OpKind.Immediate8to16 or OpKind.Immediate8to32 or OpKind.Immediate8to64 or OpKind.Immediate32to64 ? i.GetImmediate(op).ToString("X") : "k");
                        break;
                }
                sb.Append(',');
            }
            sb.Append(';');
        }
        return (sb.ToString(), disps);
    }
}
