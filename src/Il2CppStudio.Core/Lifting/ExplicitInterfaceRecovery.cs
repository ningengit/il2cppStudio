using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Metadata-only recovery of generic interfaces (no binary: WebGL, iOS project, 32-bit images). Explicit interface
/// implementations keep the interface in their metadata name (<c>System.Collections.Generic.IEnumerator&lt;System.Object&gt;.get_Current</c>),
/// so the interface instance is rebuilt from that name: definition <c>Ns.Name`N</c> plus type arguments by full name.
/// Without this the explicit members would reference an interface the class does not list (CS0540).
/// </summary>
internal static class ExplicitInterfaceRecovery
{
    public static IEnumerable<ITypeDefOrRef> Recover(GlobalMetadata md, Il2CppTypeDefinition def, ModuleDefinition module,
        Func<string, int> typeDefByFullName, Func<ModuleDefinition, int, ITypeDefOrRef?> typeRef)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var m = def.MethodStart; m < def.MethodStart + def.MethodCount && m < md.Methods.Length; m++)
        {
            var name = md.GetString(md.Methods[m].NameIndex);
            var lt = name.IndexOf('<');
            var gt = name.LastIndexOf('>');
            if (lt <= 0 || gt < lt || gt + 1 >= name.Length || name[gt + 1] != '.') continue;
            var iface = name[..(gt + 1)];
            if (!seen.Add(iface)) continue;
            var args = name[(lt + 1)..gt].Split(',', StringSplitOptions.TrimEntries);
            if (args.Any(a => a.Contains('<'))) continue;   // nested generic arguments: not resolvable by name alone
            var defIndex = typeDefByFullName($"{name[..lt]}`{args.Length}");
            if (defIndex < 0 || typeRef(module, defIndex) is not { } open) continue;
            var sigs = new List<TypeSignature>();
            foreach (var a in args)
            {
                if (a.StartsWith("System.", StringComparison.Ordinal) && module.CorLibTypeFactory.FromName("System", a["System.".Length..]) is { } c) { sigs.Add(c); continue; }
                var ai = typeDefByFullName(a);
                if (ai < 0 || typeRef(module, ai) is not { } ar) { sigs.Clear(); break; }
                sigs.Add(new TypeDefOrRefSignature(ar, md.TypeDefinitions[ai].IsValueType));
            }
            if (sigs.Count == args.Length)
                yield return new GenericInstanceTypeSignature(open, md.TypeDefinitions[defIndex].IsValueType, [.. sigs]).ToTypeDefOrRef();
        }
    }
}
