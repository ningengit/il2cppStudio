using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
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

        void AddNestedOwners(TypeDefinition? owner)
        {
            for (var n = owner; n is not null && n.IsNested; n = n.DeclaringType)
                tokens.Add(n.MetadataToken.ToUInt32());
        }

        void ScanMethod(MethodDefinition method, TypeDefinition? callerType)
        {
            if (method.CilMethodBody is not { } body) return;
            foreach (var ins in body.Instructions)
            {
                var operand = ins.Operand is MethodSpecification ms ? (object?)ms.Method : ins.Operand;
                if (operand is MemberReference mr)
                {
                    var parentTd = mr.Parent switch
                    {
                        TypeDefinition td0 => td0,
                        TypeSpecification { Signature: GenericInstanceTypeSignature { GenericType: TypeDefinition gtd } } => gtd,
                        _ => null,
                    };
                    if (parentTd is not null)
                    {
                        if (mr.Signature is FieldSignature)
                            operand = parentTd.Fields.FirstOrDefault(f => f.Name == mr.Name);
                        else if (mr.Signature is MethodSignature msig)
                            operand = parentTd.Methods.FirstOrDefault(m => m.Name == mr.Name && (m.Signature?.ParameterTypes.Count ?? 0) == msig.ParameterTypes.Count);
                    }
                }

                if (operand is TypeDefinition or TypeSpecification)
                {
                    var td = operand switch
                    {
                        TypeDefinition t0 => t0,
                        TypeSpecification { Signature: GenericInstanceTypeSignature { GenericType: TypeDefinition gtd } } => gtd,
                        _ => null,
                    };
                    if (td is not null && td.IsNested && (callerType is null || !Encloses(td, callerType)))
                        AddNestedOwners(td);
                    continue;
                }

                var (owner, token, isPrivate) = operand switch
                {
                    FieldDefinition f => (f.DeclaringType, f.MetadataToken.ToUInt32(),
                        (f.Attributes & FieldAttributes.FieldAccessMask) is not (FieldAttributes.Public or FieldAttributes.Assembly or FieldAttributes.FamilyOrAssembly)),
                    MethodDefinition m when m.Name != "Finalize" => (m.DeclaringType, m.MetadataToken.ToUInt32(),
                        (m.Attributes & MethodAttributes.MemberAccessMask) is not (MethodAttributes.Public or MethodAttributes.Assembly or MethodAttributes.FamilyOrAssembly)),
                    _ => (null, 0u, false),
                };
                if (owner is null || !isPrivate || (callerType is not null && Encloses(owner, callerType))) continue;
                tokens.Add(token);
                AddNestedOwners(owner);
                if (owner.DeclaringType is { } outer && ParseCoroutineMethod(owner.Name) is { } factoryName
                    && outer.Methods.FirstOrDefault(m => m.Name == factoryName) is { } factoryMethod
                    && (factoryMethod.Attributes & MethodAttributes.MemberAccessMask) is not (MethodAttributes.Public or MethodAttributes.Assembly or MethodAttributes.FamilyOrAssembly))
                    tokens.Add(factoryMethod.MetadataToken.ToUInt32());
            }
        }

        foreach (var type in module.GetAllTypes())
            foreach (var method in type.Methods)
                ScanMethod(method, type);

        foreach (var type in module.GetAllTypes())
            foreach (var method in type.Methods)
                if (tokens.Contains(method.MetadataToken.ToUInt32())
                    && method.Name?.Value is { } mn
                    && (mn.Contains(">b__", StringComparison.Ordinal) || mn.Contains(">g__", StringComparison.Ordinal)))
                    ScanMethod(method, callerType: null);

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
