using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Emits CIL for calls whose target is a known metadata method (direct, generic instance or vtable slot).</summary>
public sealed class CallEmitter(
    GlobalMetadata md, Il2CppRegistration reg, StubAssemblyBuilder stubs, ModuleDefinition module,
    MemoryResolver mem, RegisterTracker regs, StructSlotTable slots, StructBinder binder)
{
    public static readonly Register[] Volatile = [Register.RAX, Register.RCX, Register.RDX, Register.R8, Register.R9, Register.R10, Register.R11,
        Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, Register.XMM5];

    /// <summary>Emits the call; returns false if the target cannot be referenced (caller falls back).</summary>
    public bool Emit(EmitContext ctx, int methodIndex, int specIndex, Expr? explicitThis)
    {
        if (methodIndex < 0 && specIndex >= 0 && specIndex < reg.MethodSpecs.Count) methodIndex = reg.MethodSpecs[specIndex].MethodDefinitionIndex;
        if ((uint)methodIndex >= (uint)md.Methods.Length) return false;
        specIndex = ExactInstantiation(methodIndex, specIndex);
        var target = stubs.MethodRefFor(module, methodIndex, specIndex);
        if (target is null) return false;
        var shape = MethodShape.Create(md, reg, stubs, mem, module, methodIndex, specIndex);

        var retPtr = shape.HasReturnBuffer ? ArgumentLocations.Read(regs, 0, false) : null;
        var thisValue = shape.IsStatic ? null : explicitThis ?? ArgumentLocations.Read(regs, shape.ThisPosition, false);

        // object_new(klass) followed by klass::.ctor(obj, …) → newobj .ctor(…)
        if (thisValue is NewObjExpr { IsMaterialized: false } inlined && md.GetString(md.Methods[methodIndex].NameIndex) == ".ctor"
            && inlined.Definition is not null && stubs.GetMethodDef(methodIndex)?.DeclaringType != inlined.Definition)
        {
            // Base .ctor on a fresh object: the derived constructor was inlined. Create the object (implies base .ctor).
            inlined.Materialize(ctx);
            regs.ClobberVolatiles(Volatile);
            return true;
        }
        if (thisValue is NewObjExpr { IsMaterialized: false } pending2 && md.GetString(md.Methods[methodIndex].NameIndex) == ".ctor")
        {
            var pending = pending2;
            for (var i = 0; i < shape.Parameters.Count; i++) EmitParameter(ctx, shape.Parameters[i], shape.FirstParameterPosition + i);
            ctx.IL.Add(CilOpCodes.Newobj, target);
            regs.ClobberVolatiles(Volatile);
            var obj = regs.NewLocal(pending.Type);
            ctx.IL.Add(CilOpCodes.Stloc, obj);
            regs.ReplaceAll(pending, new LocalExpr(obj));
            return true;
        }

        if (thisValue is not null) EmitThis(ctx, shape, thisValue);
        for (var i = 0; i < shape.Parameters.Count; i++) EmitParameter(ctx, shape.Parameters[i], shape.FirstParameterPosition + i);

        var useCallvirt = !shape.IsStatic && !shape.DeclaringIsValueType;
        ctx.IL.Add(useCallvirt ? CilOpCodes.Callvirt : CilOpCodes.Call, target);
        regs.ClobberVolatiles(Volatile);
        StoreResult(ctx, shape, retPtr);
        return true;
    }

    private void EmitThis(EmitContext ctx, MethodShape shape, Expr value)
    {
        var declTd = md.Methods[shape.MethodIndex].DeclaringType;
        var declRef = stubs.TypeRefFor(module, declTd);
        if (declRef is null) { value.Emit(ctx); return; }
        var declSig = declRef.ToTypeSignature(shape.DeclaringIsValueType);
        if (shape.DeclaringIsValueType)
        {
            binder.BindAddress(ctx, value, declSig).Emit(ctx);
            return;
        }
        if (value is ConstExpr { Value: 0 }) { ctx.IL.Add(CilOpCodes.Ldnull); return; }
        value.Emit(ctx);
        if (value.Kind != ValueKind.Ref) Expr.Convert(ctx, value.Kind, ValueKind.Ref, declSig);
    }

    private void EmitParameter(EmitContext ctx, TypeSignature type, int position)
    {
        if (type is ByReferenceTypeSignature byRef)
        {
            var ptr = ArgumentLocations.Read(regs, position, false);
            var bound = Expr.KindOf(byRef.BaseType) == ValueKind.Struct ? binder.BindAddress(ctx, ptr, byRef.BaseType) : ptr;
            bound.Emit(ctx);
            return;
        }
        if (MethodShape.PassedByPointer(type, mem))
        {
            var value = binder.BindValue(ctx, ArgumentLocations.Read(regs, position, false), type);
            Expr.EmitAs(ctx, value, type);
            return;
        }
        var v = ArgumentLocations.Read(regs, position, MethodShape.IsFloat(type));
        if (v is StructChunkExpr { Offset: 0 } c && Expr.KindOf(type) == ValueKind.Struct) v = c.Source;
        // Delegate constructors take (object target, IntPtr method): a MethodInfo* marker becomes ldftn.
        if (Expr.KindOf(type) == ValueKind.Native && v is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { } u } && FunctionPointer(u) is { } fn)
        {
            ctx.IL.Add(CilOpCodes.Ldftn, fn);
            return;
        }
        Expr.EmitAs(ctx, v, type);
    }

    /// <summary>
    /// Shared generic code (e.g. GetComponent&lt;object&gt; used for every reference T) receives the exact
    /// instantiation as its hidden trailing <c>const MethodInfo*</c> argument. When that argument is a method-spec
    /// marker of the same method definition, it identifies the real type arguments (GetComponent&lt;NavMeshAgent&gt;).
    /// </summary>
    private int ExactInstantiation(int methodIndex, int specIndex)
    {
        var def = md.Methods[methodIndex];
        var position = ((def.Flags & 0x10) != 0 ? 0 : 1) + def.ParameterCount;
        if (MethodShape.NeedsReturnBuffer(stubs.ToSig(module, def.ReturnType), mem)) position++;
        if (ArgumentLocations.Read(regs, position, false) is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { Kind: Registration.MetadataUsageKind.MethodRef } u }
            && (uint)u.Index < (uint)reg.MethodSpecs.Count && reg.MethodSpecs[u.Index].MethodDefinitionIndex == methodIndex)
            return u.Index;
        return specIndex;
    }

    /// <summary>Static type of the value in RCX (the receiver of an instance call), as a typedef index, or -1.</summary>
    public int ReceiverTypeDef() => regs.Get(Register.RCX) switch
    {
        NewObjExpr n => mem.TypeDefOf(n.Type),
        AddressExpr a => mem.TypeDefOf(a.Target.Type),
        var e => mem.TypeDefOf(e.Type),
    };

    private IMethodDescriptor? FunctionPointer(Registration.MetadataUsage u) => u.Kind switch
    {
        Registration.MetadataUsageKind.MethodDef => stubs.MethodRefFor(module, u.Index),
        Registration.MetadataUsageKind.MethodRef when (uint)u.Index < (uint)reg.MethodSpecs.Count =>
            stubs.MethodRefFor(module, reg.MethodSpecs[u.Index].MethodDefinitionIndex, u.Index),
        _ => null,
    };

    private void StoreResult(EmitContext ctx, MethodShape shape, Expr? retPtr)
    {
        var ret = shape.ReturnType;
        if (Expr.KindOf(ret) == ValueKind.Void) return;
        if (shape.HasReturnBuffer)
        {
            switch (retPtr)
            {
                case StackAddrPlaceholder p:
                {
                    var local = slots.GetOrCreate(p.Key, ret, mem.ValueSize(ret));
                    ctx.IL.Add(CilOpCodes.Stloc, local);
                    regs.Set(Register.RAX, new AddressExpr(new LocalExpr(local)));
                    return;
                }
                case AddressExpr { Target: LocalExpr l }:
                    ctx.IL.Add(CilOpCodes.Stloc, l.Local);
                    regs.Set(Register.RAX, retPtr);
                    return;
                default:
                {
                    var temp = regs.NewLocal(ret);
                    ctx.IL.Add(CilOpCodes.Stloc, temp);
                    regs.Set(Register.RAX, new AddressExpr(new LocalExpr(temp)));
                    return;
                }
            }
        }
        var result = regs.NewLocal(ret);
        ctx.IL.Add(CilOpCodes.Stloc, result);
        regs.Set(MethodShape.IsFloat(ret) ? Register.XMM0 : Register.RAX, new LocalExpr(result));
    }
}
