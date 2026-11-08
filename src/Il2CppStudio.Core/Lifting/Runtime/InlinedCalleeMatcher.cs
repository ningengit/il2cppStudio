using System.Collections.Concurrent;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Disassembly;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>
/// Matches out-of-line <c>_inline</c> method copies where the C++ compiler inlined the method's single direct
/// non-helper callee <c>H</c> (e.g. <c>Mathf.RoundToInt</c> calling <c>bankers_round</c>, or
/// <c>Vector3.get_normalized</c> calling <c>Vector3.Normalize</c>). Expanding <c>H</c>'s global/call token stream
/// into the metadata method's token stream reproduces the inlined copy's exact token signature without PDBs.
/// </summary>
public static class InlinedCalleeMatcher
{
    public static IReadOnlyList<InlineCopyMatch> Match(
        NativeSymbolizer sym, X64Disassembler disasm, RuntimeHelperMap map, IReadOnlyList<ulong> unmatched)
    {
        if (unmatched.Count == 0) return [];
        var bySig = new Dictionary<string, List<(ulong Va, int Count)>>(StringComparer.Ordinal);
        foreach (var va in unmatched)
        {
            var fn = disasm.Disassemble(va, 0x2000);
            if (fn.Truncated || fn.Instructions.Count < 8) continue;
            var (tokens, convs) = Extract(fn, null);
            if (tokens.Count < 4) continue;
            var sig = Format(tokens, convs);
            (bySig.TryGetValue(sig, out var list) ? list : bySig[sig] = []).Add((va, fn.Instructions.Count));
        }
        if (bySig.Count == 0) return [];

        var calleeCache = new ConcurrentDictionary<ulong, (List<string> Tokens, HashSet<Mnemonic> Convs)>();
        var matches = new ConcurrentBag<InlineCopyMatch>();
        Parallel.ForEach(sym.Map.Symbols.Values, s =>
        {
            var fn = disasm.Disassemble(s.Va, 0x400);
            if (fn.Truncated || fn.Instructions.Count is < 3 or > 45) return;
            var callees = NonHelperCallees(fn, map);
            if (callees.Count != 1) return;
            var h = callees[0];
            var hData = calleeCache.GetOrAdd(h, va => Extract(disasm.Disassemble(va, 0x2000), null));
            if (hData.Tokens.Count == 0) return;
            var (tokens, convs) = Extract(fn, (h, hData));
            if (tokens.Count < 4) return;
            if (bySig.TryGetValue(Format(tokens, convs), out var copies))
                foreach (var c in copies) matches.Add(new InlineCopyMatch(c.Va, s, c.Count));
        });
        return [.. matches.GroupBy(m => m.Va).Select(g => g.OrderBy(m => m.Original.MethodSpecIndex >= 0 ? 1 : 0).ThenBy(m => m.Original.DisplayName.Length).First())];
    }

    private static List<ulong> NonHelperCallees(DisassembledFunction fn, RuntimeHelperMap map)
    {
        var list = new List<ulong>();
        foreach (var i in fn.Instructions)
        {
            if (i.Mnemonic == Mnemonic.Int3) break;
            if (i.Mnemonic is not (Mnemonic.Call or Mnemonic.Jmp) || i.Op0Kind is not (OpKind.NearBranch64 or OpKind.NearBranch32)) continue;
            var t = i.NearBranchTarget;
            if (t >= fn.StartVa && t < fn.EndVa) continue;
            if (map.KindOf(t) != RuntimeHelperKind.Unknown) continue;
            if (map.ExportNames.TryGetValue(t, out var name) && CrtMath.IsKnown(name)) continue;
            if (!list.Contains(t)) list.Add(t);
        }
        return list;
    }

    private static (List<string> Tokens, HashSet<Mnemonic> Convs) Extract(
        DisassembledFunction fn, (ulong Callee, (List<string> Tokens, HashSet<Mnemonic> Convs) Data)? splice)
    {
        var tokens = new List<string>();
        var convs = new HashSet<Mnemonic>();
        foreach (var i in fn.Instructions)
        {
            if (i.Mnemonic == Mnemonic.Int3) break;
            if (i.Mnemonic is Mnemonic.Cvtss2sd or Mnemonic.Cvtsd2ss or Mnemonic.Cvttsd2si or Mnemonic.Cvttss2si or Mnemonic.Cvtsi2ss or Mnemonic.Cvtsi2sd)
                convs.Add(i.Mnemonic);
            for (var op = 0; op < i.OpCount; op++)
            {
                var k = i.GetOpKind(op);
                if (k == OpKind.Memory && i.IsIPRelativeMemoryOperand && i.MemorySize != MemorySize.UInt8)
                    tokens.Add($"M{i.IPRelativeMemoryAddress:X}");
                else if (k is OpKind.NearBranch64 or OpKind.NearBranch32 or OpKind.NearBranch16
                         && i.Mnemonic is Mnemonic.Call or Mnemonic.Jmp && (i.NearBranchTarget < fn.StartVa || i.NearBranchTarget >= fn.EndVa))
                {
                    if (splice is { } sp && i.NearBranchTarget == sp.Callee)
                    {
                        tokens.AddRange(sp.Data.Tokens);
                        convs.UnionWith(sp.Data.Convs);
                    }
                    else tokens.Add($"A{i.NearBranchTarget:X}");
                }
            }
        }
        return (tokens, convs);
    }

    private static string Format(List<string> tokens, HashSet<Mnemonic> convs) =>
        string.Join(',', tokens) + "|" + string.Join(',', convs.OrderBy(c => c));
}
