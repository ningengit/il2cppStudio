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

        void ScanTypeSig(TypeSignature? sig, TypeDefinition? callerType)
        {
            switch (sig)
            {
                case TypeDefOrRefSignature tdor:
                    if (tdor.Type is TypeDefinition td && td.IsNested && (callerType is null || !Encloses(td.DeclaringType ?? td, callerType)))
                        AddNestedOwners(td);
                    break;
                case GenericInstanceTypeSignature gi:
                    if (gi.GenericType is TypeDefinition gtd && gtd.IsNested && (callerType is null || !Encloses(gtd.DeclaringType ?? gtd, callerType)))
                        AddNestedOwners(gtd);
                    foreach (var ta in gi.TypeArguments)
                        ScanTypeSig(ta, callerType);
                    break;
                case TypeSpecificationSignature ts:
                    ScanTypeSig(ts.BaseType, callerType);
                    break;
            }
        }

        void ScanMethod(MethodDefinition method, TypeDefinition? callerType)
        {
            if (method.CilMethodBody is not { } body) return;
            foreach (var ins in body.Instructions)
            {
                if (ins.Operand is MethodSpecification { Signature: { } msigSpec })
                    foreach (var ta in msigSpec.TypeArguments)
                        ScanTypeSig(ta, callerType);
                var operand = ins.Operand is MethodSpecification ms ? (object?)ms.Method : ins.Operand;
                if (operand is MemberReference mr)
                {
                    if (mr.Parent is TypeSpecification mrSpec) ScanTypeSig(mrSpec.Signature, callerType);
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
                    if (operand is TypeSpecification ts)
                        ScanTypeSig(ts.Signature, callerType);
                    else if (operand is TypeDefinition td && td.IsNested && (callerType is null || !Encloses(td.DeclaringType ?? td, callerType)))
                        AddNestedOwners(td);
                    continue;
                }

                var (owner, token, isPrivate, isProtected) = operand switch
                {
                    FieldDefinition f => (f.DeclaringType, f.MetadataToken.ToUInt32(),
                        (f.Attributes & FieldAttributes.FieldAccessMask) is FieldAttributes.Private or FieldAttributes.PrivateScope,
                        (f.Attributes & FieldAttributes.FieldAccessMask) is FieldAttributes.Family or FieldAttributes.FamilyAndAssembly),
                    MethodDefinition m when m.Name != "Finalize" => (m.DeclaringType, m.MetadataToken.ToUInt32(),
                        (m.Attributes & MethodAttributes.MemberAccessMask) is MethodAttributes.Private or MethodAttributes.CompilerControlled,
                        (m.Attributes & MethodAttributes.MemberAccessMask) is MethodAttributes.Family or MethodAttributes.FamilyAndAssembly),
                    _ => (null, 0u, false, false),
                };
                if (owner is null || (!isPrivate && !isProtected)) continue;
                if (callerType is not null && Encloses(owner, callerType)) continue;
                if (isProtected && callerType is not null && InheritsFrom(callerType, owner)) continue;
                tokens.Add(token);
                if (!owner.IsNested || callerType is null || !Encloses(owner.DeclaringType!, callerType))
                    AddNestedOwners(owner);
                if (owner.DeclaringType is { } outer && owner.Name?.Value is { } ownerName && ParseCoroutineMethod(ownerName) is { } factoryName
                    && (callerType is null || !Encloses(outer, callerType))
                    && outer.Methods.FirstOrDefault(m => m.Name == factoryName) is { } factoryMethod
                    && (factoryMethod.Attributes & MethodAttributes.MemberAccessMask) is not (MethodAttributes.Public or MethodAttributes.Assembly or MethodAttributes.FamilyOrAssembly))
                    tokens.Add(factoryMethod.MetadataToken.ToUInt32());
            }
        }

        void ScanSignatures()
        {
            foreach (var type in module.GetAllTypes())
            {
                var typeVisible = !type.IsNestedPrivate || tokens.Contains(type.MetadataToken.ToUInt32());
                foreach (var field in type.Fields)
                {
                    var fVisible = (typeVisible && (field.Attributes & FieldAttributes.FieldAccessMask) != FieldAttributes.Private)
                        || tokens.Contains(field.MetadataToken.ToUInt32());
                    if (fVisible) ScanTypeSig(field.Signature?.FieldType, type);
                }
                foreach (var method in type.Methods)
                {
                    var mVisible = (typeVisible && (method.Attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Private)
                        || tokens.Contains(method.MetadataToken.ToUInt32());
                    if (!mVisible || method.Signature is not { } sig) continue;
                    ScanTypeSig(sig.ReturnType, type);
                    foreach (var p in sig.ParameterTypes) ScanTypeSig(p, type);
                }
            }
        }

        foreach (var type in module.GetAllTypes())
            foreach (var method in type.Methods)
                ScanMethod(method, type);

        ScanSignatures();

        foreach (var type in module.GetAllTypes())
            foreach (var method in type.Methods)
                if (tokens.Contains(method.MetadataToken.ToUInt32())
                    && method.Name?.Value is { } mn
                    && (mn.Contains(">b__", StringComparison.Ordinal) || mn.Contains(">g__", StringComparison.Ordinal)))
                    ScanMethod(method, callerType: null);

        ScanSignatures();

        foreach (var type in module.GetAllTypes())
        {
            foreach (var method in type.Methods)
            {
                if (!method.IsVirtual || !tokens.Contains(method.MetadataToken.ToUInt32())) continue;
                var cur = type;
                for (var hop = 0; hop < 16 && cur is not null; hop++)
                {
                    var bt = cur.BaseType switch
                    {
                        TypeDefinition btd => btd,
                        TypeSpecification { Signature: GenericInstanceTypeSignature { GenericType: TypeDefinition bgtd } } => bgtd,
                        _ => null,
                    };
                    if (bt is null) break;
                    foreach (var bm in bt.Methods)
                        if (bm.IsVirtual && bm.Name == method.Name
                            && (bm.Signature?.ParameterTypes.Count ?? 0) == (method.Signature?.ParameterTypes.Count ?? 0))
                            tokens.Add(bm.MetadataToken.ToUInt32());
                    cur = bt;
                }
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

    /// <summary>True when <paramref name="caller"/> (or an enclosing type) derives from <paramref name="owner"/> (protected access is legal).</summary>
    private static bool InheritsFrom(TypeDefinition caller, TypeDefinition owner)
    {
        for (var t = caller; t is not null; t = t.DeclaringType)
        {
            var cur = t;
            for (var hop = 0; hop < 32 && cur is not null; hop++)
            {
                var bt = cur.BaseType switch
                {
                    TypeDefinition btd => btd,
                    TypeSpecification { Signature: GenericInstanceTypeSignature { GenericType: TypeDefinition bgtd } } => bgtd,
                    _ => null,
                };
                if (bt is null) break;
                if (bt == owner) return true;
                cur = bt;
            }
        }
        return false;
    }
}
