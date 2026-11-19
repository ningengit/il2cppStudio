using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
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
        TypeNameService names, Action<string>? log = null, bool receiverIsNewObject = false,
        TypeSignature? receiverType = null, StubAssemblyBuilder? stubs = null, ModuleDefinition? module = null)
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
            var mdef = md.Methods[c.MethodIndex];
            var decl = mdef.DeclaringType;
            var mname = md.GetString(mdef.NameIndex);
            var depth = chain.IndexOf(decl);
            var rank = depth >= 0 ? depth
                : names.TypeDefName(decl, true) == "System.Object" ? 1000
                : c.MethodSpecIndex < 0 ? 2000 : 3000;
            if (depth == 0 && receiverType is GenericInstanceTypeSignature recvGen && stubs is not null && module is not null
                && c.MethodSpecIndex >= 0 && (uint)c.MethodSpecIndex < (uint)reg.MethodSpecs.Count)
            {
                var spec = reg.MethodSpecs[c.MethodSpecIndex];
                if (spec.ClassInstIndex >= 0 && spec.ClassInstIndex < reg.GenericInstVas.Count)
                {
                    var ci = reg.ReadGenericInst(reg.GenericInstVas[spec.ClassInstIndex]);
                    if (ci.TypeArgVas.Count == recvGen.TypeArguments.Count)
                    {
                        var allMatch = true;
                        for (var k = 0; k < ci.TypeArgVas.Count; k++)
                        {
                            if (!SignatureComparer.Default.Equals(stubs.ToSig(module, reg.GetTypeAt(ci.TypeArgVas[k])), recvGen.TypeArguments[k]))
                            {
                                allMatch = false;
                                break;
                            }
                        }
                        rank += allMatch ? -500 : 500;
                    }
                }
            }
            if (depth < 0)
            {
                if (!mname.StartsWith('.') && mname.Contains('.')) continue;
                var declDef = md.TypeDefinitions[decl];
                var declName = md.GetString(declDef.NameIndex);
                if (declName.Contains('<') || mname.Contains('<')) rank += 20_000;
                if ((declDef.Flags & 0x7) is not (1 or 2)) rank += 500;
                if (declDef.IsValueType) rank += 5000;
                if (mdef.ParameterCount == 0) rank -= 200;
            }
            // The first call on a freshly allocated object is its constructor chain.
            if (receiverIsNewObject) rank += mname == ".ctor" ? -10_000 : 0;
            if (rank < bestRank) { best = c; bestRank = rank; }
        }
        if (log is not null && best is not null && bestRank >= 1000 && !receiverIsNewObject)
            log($"identical-code folding: {candidates.Count} methods share 0x{best.Va:X}; chose {best.DisplayName} (receiver type unrelated to candidates)");
        return best ?? candidates[0];
    }
}
