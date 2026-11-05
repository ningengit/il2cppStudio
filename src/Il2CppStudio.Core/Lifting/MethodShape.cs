using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Calling-convention shape of a metadata method as IL2CPP compiles it on Win64:
/// <c>[ret buffer*] [this] params… [const MethodInfo*]</c>. Types are instantiated for generic method specs.
/// </summary>
public sealed record MethodShape(
    int MethodIndex, bool IsStatic, bool DeclaringIsValueType, bool HasReturnBuffer,
    TypeSignature ReturnType, IReadOnlyList<TypeSignature> Parameters)
{
    /// <summary>Position of the first metadata parameter in the native argument list.</summary>
    public int FirstParameterPosition => (HasReturnBuffer ? 1 : 0) + (IsStatic ? 0 : 1);

    public int ThisPosition => HasReturnBuffer ? 1 : 0;

    public static MethodShape Create(GlobalMetadata md, Il2CppRegistration reg, StubAssemblyBuilder stubs, MemoryResolver mem,
        ModuleDefinition ctx, int methodIndex, int specIndex)
    {
        var def = md.Methods[methodIndex];
        var classArgs = new List<TypeSignature>();
        var methodArgs = new List<TypeSignature>();
        if (specIndex >= 0 && (uint)specIndex < (uint)reg.MethodSpecs.Count)
        {
            var spec = reg.MethodSpecs[specIndex];
            if (spec.ClassInstIndex >= 0 && spec.ClassInstIndex < reg.GenericInstVas.Count)
                classArgs.AddRange(reg.ReadGenericInst(reg.GenericInstVas[spec.ClassInstIndex]).TypeArgVas.Select(v => stubs.ToSig(ctx, reg.GetTypeAt(v))));
            if (spec.MethodInstIndex >= 0 && spec.MethodInstIndex < reg.GenericInstVas.Count)
                methodArgs.AddRange(reg.ReadGenericInst(reg.GenericInstVas[spec.MethodInstIndex]).TypeArgVas.Select(v => stubs.ToSig(ctx, reg.GetTypeAt(v))));
        }

        TypeSignature Inst(TypeSignature t) => t switch
        {
            GenericParameterSignature { ParameterType: GenericParameterType.Type } g when g.Index < classArgs.Count => classArgs[g.Index],
            GenericParameterSignature { ParameterType: GenericParameterType.Method } g when g.Index < methodArgs.Count => methodArgs[g.Index],
            _ => t,
        };

        var ret = Inst(stubs.ToSig(ctx, def.ReturnType));
        var ps = Enumerable.Range(0, def.ParameterCount).Select(p => Inst(stubs.ToSig(ctx, md.Parameters[def.ParameterStart + p].TypeIndex))).ToList();
        var isValueDecl = stubs.IsValueType(def.DeclaringType);
        return new MethodShape(methodIndex, (def.Flags & 0x10) != 0, isValueDecl, NeedsReturnBuffer(ret, mem), ret, ps);
    }

    /// <summary>Win64: structs whose size is not 1, 2, 4 or 8 bytes are returned through a hidden pointer.</summary>
    public static bool NeedsReturnBuffer(TypeSignature t, MemoryResolver mem) =>
        Expr.KindOf(t) == ValueKind.Struct && mem.ValueSize(t) is not (1 or 2 or 4 or 8);

    /// <summary>Win64: structs whose size is not 1, 2, 4 or 8 bytes are passed by pointer.</summary>
    public static bool PassedByPointer(TypeSignature t, MemoryResolver mem) => NeedsReturnBuffer(t, mem);

    public static bool IsFloat(TypeSignature t) => Expr.KindOf(t) is ValueKind.R4 or ValueKind.R8;
}
