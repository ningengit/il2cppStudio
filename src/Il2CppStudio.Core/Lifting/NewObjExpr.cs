using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Result of il2cpp_codegen_object_new before its constructor runs. When the next call is a .ctor of this exact
/// type, <see cref="CallEmitter"/> fuses both into one <c>newobj</c>. When IL2CPP inlined the constructor (only a
/// base .ctor is called, or none), the object is created with the type's simplest constructor, using default
/// arguments for its parameters; the field stores of the inlined body follow as normal statements.
/// </summary>
public sealed class NewObjExpr(TypeSignature type, TypeDefinition? definition, RegisterTracker regs) : Expr
{
    private CilLocalVariable? _local;

    public override TypeSignature Type => type;
    public TypeDefinition? Definition => definition;
    public override ValueKind Kind => ValueKind.Ref;
    public bool IsMaterialized => _local is not null;
    public override string Describe() => $"new {type.Name}";

    public override void Emit(EmitContext ctx)
    {
        if (_local is null) Materialize(ctx);
        ctx.IL.Add(CilOpCodes.Ldloc, _local!);
    }

    /// <summary>Creates the object with the simplest constructor (default arguments) and stores it in a local.</summary>
    public LocalExpr Materialize(EmitContext ctx)
    {
        if (_local is not null) return new LocalExpr(_local);
        _local = regs.NewLocal(type);
        var ctor = definition?.Methods.Where(m => m.IsConstructor && !m.IsStatic).OrderBy(m => m.Parameters.Count).FirstOrDefault();
        if (ctor is not null)
        {
            foreach (var p in ctor.Signature!.ParameterTypes) PushDefault(ctx, p);
            ctx.IL.Add(CilOpCodes.Newobj, Reference(ctx.Module, ctor));
            if (ctor.Parameters.Count > 0)
                ConversionLog.Record(ctx, $"constructor of {type.Name} was inlined by IL2CPP; recreated as new {type.Name}({string.Join(", ", ctor.Parameters.Select(p => p.Name))}) with default arguments, followed by the inlined field stores");
        }
        else
        {
            ctx.Native($"il2cpp_codegen_object_new({type.FullName})");
            ctx.IL.Add(CilOpCodes.Call, new MethodSpecification(ctx.Intrinsics.Reinterpret, new GenericInstanceMethodSignature([type])));
            ConversionLog.Record(ctx, $"no constructor available for {type.FullName}; allocation kept as a native intrinsic");
        }
        ctx.IL.Add(CilOpCodes.Stloc, _local);
        var result = new LocalExpr(_local);
        regs.ReplaceAll(this, result);
        return result;
    }

    private static void PushDefault(EmitContext ctx, TypeSignature t)
    {
        switch (KindOf(t))
        {
            case ValueKind.I8: ctx.IL.Add(CilOpCodes.Ldc_I8, 0L); break;
            case ValueKind.R4: ctx.IL.Add(CilOpCodes.Ldc_R4, 0f); break;
            case ValueKind.R8: ctx.IL.Add(CilOpCodes.Ldc_R8, 0d); break;
            case ValueKind.Ref: ctx.IL.Add(CilOpCodes.Ldnull); break;
            default: ctx.IL.Add(CilOpCodes.Ldc_I4_0); break;
        }
    }

    private IMethodDescriptor Reference(ModuleDefinition module, MethodDefinition ctor)
    {
        if (ctor.DeclaringModule == module && type is TypeDefOrRefSignature) return ctor;
        var parent = (IMemberRefParent)(type is GenericInstanceTypeSignature ? new TypeSpecification(type) : type.ToTypeDefOrRef());
        return new MemberReference(parent, ".ctor", MethodSignature.CreateInstance(module.CorLibTypeFactory.Void, ctor.Signature!.ParameterTypes));
    }
}
