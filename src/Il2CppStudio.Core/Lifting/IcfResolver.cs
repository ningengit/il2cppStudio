using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Chooses among methods folded to one address by identical-code folding. The receiver's static type decides:
/// a candidate declared on the receiver's type or a base type (closest first) wins; otherwise System.Object
/// members (the canonical empty .ctor), then non-generic, then the first. The choice is logged.
/// </summary>
public static class IcfResolver
{
    public static NativeSymbol Choose(IReadOnlyList<NativeSymbol> candidates, int receiverTypeDef, GlobalMetadata md, Il2CppRegistration reg,
        TypeNameService names, Action<string>? log = null, bool receiverIsNewObject = false)
    {
        if (candidates.Count == 1) return candidates[0];
        var chain = new List<int>();
        for (var t = receiverTypeDef; (uint)t < (uint)md.TypeDefinitions.Length && chain.Count < 32 && !chain.Contains(t);)
        {
            chain.Add(t);
            var parent = md.TypeDefinitions[t].ParentIndex;
            t = parent >= 0 && reg.TryGetType(parent, out var p) ? names.ResolveTypeDefIndex(p) : -1;
        }

        NativeSymbol? best = null;
        var bestRank = int.MaxValue;
        foreach (var c in candidates)
        {
            var decl = md.Methods[c.MethodIndex].DeclaringType;
            var depth = chain.IndexOf(decl);
            var rank = depth >= 0 ? depth
                : names.TypeDefName(decl, true) == "System.Object" ? 1000
                : c.MethodSpecIndex < 0 ? 2000 : 3000;
            // The first call on a freshly allocated object is its constructor chain.
            if (receiverIsNewObject) rank += md.GetString(md.Methods[c.MethodIndex].NameIndex) == ".ctor" ? -10_000 : 0;
            if (rank < bestRank) { best = c; bestRank = rank; }
        }
        if (log is not null && best is not null && bestRank >= 1000 && !receiverIsNewObject)
            log($"identical-code folding: {candidates.Count} methods share 0x{best.Va:X}; chose {best.DisplayName} (receiver type unrelated to candidates)");
        return best ?? candidates[0];
    }
}
