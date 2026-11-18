using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Finds private/protected members of a stub module that lifted code in <em>other</em> types accesses. IL2CPP inlines
/// small methods across classes, so <c>CameraScript</c> may read <c>InputManager.currentKeyStates</c> directly; such
/// members are widened to <c>internal</c> in the C# output (with a comment) so the exported project compiles.
/// </summary>
public static class CrossTypeAccessScanner
{
    public static HashSet<uint> Scan(byte[] image)
    {
        var tokens = new HashSet<uint>();
        var module = ModuleDefinition.FromBytes(image);
        foreach (var type in module.GetAllTypes())
        foreach (var method in type.Methods)
        {
            if (method.CilMethodBody is not { } body) continue;
            foreach (var ins in body.Instructions)
            {
                var (owner, token, isPrivate) = ins.Operand switch
                {
                    FieldDefinition f => (f.DeclaringType, f.MetadataToken.ToUInt32(), (f.Attributes & FieldAttributes.FieldAccessMask) is not (FieldAttributes.Public or FieldAttributes.Assembly or FieldAttributes.FamilyOrAssembly)),
                    MethodDefinition m => (m.DeclaringType, m.MetadataToken.ToUInt32(), (m.Attributes & MethodAttributes.MemberAccessMask) is not (MethodAttributes.Public or MethodAttributes.Assembly or MethodAttributes.FamilyOrAssembly)),
                    _ => (null, 0u, false),
                };
                if (owner is null || !isPrivate || Encloses(owner, type)) continue;
                tokens.Add(token);
                if (owner.DeclaringType is { } outer && ParseCoroutineMethod(owner.Name) is { } factoryName
                    && outer.Methods.FirstOrDefault(m => m.Name == factoryName) is { } factoryMethod)
                    tokens.Add(factoryMethod.MetadataToken.ToUInt32());
            }
        }
        return tokens;
    }

    private static string? ParseCoroutineMethod(string stateMachineName)
    {
        if (stateMachineName.StartsWith('<'))
        {
            var end = stateMachineName.IndexOf(">d__", StringComparison.Ordinal);
            return end > 1 ? stateMachineName[1..end] : null;
        }
        return null;
    }

    /// <summary>True when <paramref name="caller"/> is <paramref name="owner"/> or nested inside it (private access is legal).</summary>
    private static bool Encloses(TypeDefinition owner, TypeDefinition caller)
    {
        for (var t = caller; t is not null; t = t.DeclaringType)
            if (t == owner) return true;
        return false;
    }
}
