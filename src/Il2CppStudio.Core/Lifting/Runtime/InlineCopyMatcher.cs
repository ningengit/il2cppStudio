using System.Collections.Concurrent;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Disassembly;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>A non-metadata function proven identical to a metadata method body.</summary>
public sealed record InlineCopyMatch(ulong Va, NativeSymbol Original, int Instructions);

/// <summary>
/// Identifies out-of-line copies of managed methods (IL2CPP emits <c>Method_mXXXX_inline</c> functions into each
/// translation unit; MSVC keeps some out of line) by matching their normalized instruction stream against every
/// metadata method body. Only exact fingerprint matches are accepted, so a match is the same code, not a guess.
/// </summary>
public static class InlineCopyMatcher
{
    public static IReadOnlyList<InlineCopyMatch> Match(NativeSymbolizer sym, X64Disassembler disasm, IEnumerable<ulong> candidates, IDiagnosticSink sink, RuntimeHelperMap? map = null)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var wanted = candidates.Distinct().ToList();
        if (wanted.Count == 0) return [];

        // Fingerprint the candidates first; only metadata bodies with a matching instruction count are fingerprinted.
        var candidatePrints = new ConcurrentDictionary<ulong, (string Print, int Count)>();
        Parallel.ForEach(wanted, va =>
        {
            var fn = disasm.Disassemble(va, 0x2000);
            if (fn.Truncated || fn.Instructions.Count < 3) return;
            candidatePrints[va] = (FunctionFingerprint.Compute(fn.Instructions, fn.StartVa, fn.EndVa), fn.Instructions.Count);
        });
        var byPrint = candidatePrints.GroupBy(c => c.Value.Print).ToDictionary(g => g.Key, g => g.Select(x => x.Key).ToList());
        var counts = candidatePrints.Values.Select(v => v.Count).ToHashSet();

        var matches = new ConcurrentBag<InlineCopyMatch>();
        var originals = sym.Map.Symbols.Values.Where(s => !candidatePrints.ContainsKey(s.Va)).ToList();
        Parallel.ForEach(originals, s =>
        {
            var fn = disasm.Disassemble(s.Va, 0x2000);
            if (fn.Truncated || !counts.Contains(fn.Instructions.Count)) return;
            var print = FunctionFingerprint.Compute(fn.Instructions, fn.StartVa, fn.EndVa);
            if (byPrint.TryGetValue(print, out var copies))
                foreach (var c in copies) matches.Add(new InlineCopyMatch(c, s, fn.Instructions.Count));
        });

        // One original per copy (identical-code folding may give several equivalent originals: prefer non-generic).
        var result = matches.GroupBy(m => m.Va)
            .Select(g => g.OrderBy(m => m.Original.MethodSpecIndex >= 0 ? 1 : 0).ThenBy(m => m.Original.DisplayName.Length).First())
            .ToList();
        if (map is not null)
        {
            var matchedVas = result.Select(m => m.Va).ToHashSet();
            var unmatched = wanted.Where(va => !matchedVas.Contains(va)).ToList();
            result.AddRange(InlinedCalleeMatcher.Match(sym, disasm, map, unmatched));
            var stillUnmatched = unmatched.Except(result.Select(m => m.Va)).ToList();
            result.AddRange(AdjustorThunkMatcher.Match(sym, disasm, stillUnmatched));
        }
        var imageBase = sym.Registration.Reader.ImageBase;
        foreach (var m in result.OrderBy(m => m.Va))
            sink.Debug(DiagnosticCategory.Registration, DiagnosticCodes.InlineCopyMatched,
                $"sub_{m.Va - imageBase:X} is an out-of-line copy of {m.Original.DisplayName} ({m.Instructions} instructions, exact fingerprint match)",
                new() { Rva = m.Va - imageBase });
        sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.InlineCopyMatched,
            $"Matched {result.Count} of {wanted.Count} non-metadata call targets to managed method bodies in {sw.Elapsed.TotalSeconds:F1}s.");
        return result;
    }
}
