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
    private static readonly Register[] Win64Volatile = [Register.RAX, Register.RCX, Register.RDX, Register.R8, Register.R9, Register.R10, Register.R11,
        Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, Register.XMM5];

    /// <summary>System V: RSI/RDI and every XMM register are caller-saved as well.</summary>
    private static readonly Register[] SysVVolatile = [Register.RAX, Register.RCX, Register.RDX, Register.RSI, Register.RDI, Register.R8, Register.R9,
        Register.R10, Register.R11, Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, Register.XMM5, Register.XMM6,
        Register.XMM7, Register.XMM8, Register.XMM9, Register.XMM10, Register.XMM11, Register.XMM12, Register.XMM13, Register.XMM14, Register.XMM15];

    /// <summary>Registers a call clobbers under the active calling convention.</summary>
    public static Register[] Volatile => ArgumentLocations.Abi == NativeAbi.SysV ? SysVVolatile : Win64Volatile;

    /// <summary>Emits the call; returns false if the target cannot be referenced (caller falls back).</summary>
    public bool Emit(EmitContext ctx, int methodIndex, int specIndex, Expr? explicitThis)
    {
        if (methodIndex < 0 && specIndex >= 0 && specIndex < reg.MethodSpecs.Count) methodIndex = reg.MethodSpecs[specIndex].MethodDefinitionIndex;
        if ((uint)methodIndex >= (uint)md.Methods.Length) return false;
        specIndex = ExactInstantiation(methodIndex, specIndex);
        var target = stubs.MethodRefFor(module, methodIndex, specIndex);
        if (target is null) return false;
        var shape = MethodShape.Create(md, reg, stubs, mem, module, methodIndex, specIndex);
        shape.EnsureAbiSupported(mem);
        ArgumentLocations.Layout(shape.FloatLayout(mem));

        var retPtr = shape.HasReturnBuffer ? ArgumentLocations.Read(regs, 0, false) : null;
        var thisValue = shape.IsStatic ? null : explicitThis ?? ArgumentLocations.Read(regs, shape.ThisPosition, false);

        // object_new(klass) followed by klass::.ctor(obj, …) → newobj .ctor(…)
        if (thisValue is NewObjExpr { IsMaterialized: false } inlined && md.GetString(md.Methods[methodIndex].NameIndex) == ".ctor"
            && DelegateCtorFor(inlined, target) is null
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
            ctx.IL.Add(CilOpCodes.Newobj, DelegateCtorFor(pending, target) ?? target);
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
        if (!shape.DeclaringIsValueType && declSig.FullName == "System.Enum" && value is StackAddrPlaceholder sp
            && regs.GetSlot(sp.Key) is MarkerExpr { Marker: MarkerKind.TypeInfo, Usage: { Kind: MetadataUsageKind.TypeInfo } eu }
            && regs.GetSlot(sp.Key + 16) is { } enumVal && stubs.ToSig(module, eu.Index) is { } enumSig)
        {
            Expr.EmitAs(ctx, enumVal, enumSig);
            ctx.IL.Add(CilOpCodes.Box, enumSig.ToTypeDefOrRef());
            return;
        }
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
        // Windows x64 passes 8-byte structs in a GPR: a Vector2 built with unpcklps arrives as two packed floats.
        if (v is PackedFloatsExpr packed && type.FullName == "UnityEngine.Vector2")
        {
            var single = ctx.Module.CorLibTypeFactory.Single;
            Expr.EmitAs(ctx, packed.Lane0, single);
            Expr.EmitAs(ctx, packed.Lane1, single);
            var ctor = new AsmResolver.DotNet.MemberReference(type.ToTypeDefOrRef(), ".ctor",
                MethodSignature.CreateInstance(ctx.Module.CorLibTypeFactory.Void, [single, single]));
            ctx.IL.Add(CilOpCodes.Newobj, ctor);
            return;
        }
        // Delegate constructors take (object target, IntPtr method): a MethodInfo* marker becomes ldftn.
        if (Expr.KindOf(type) == ValueKind.Native && v is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { } u } && FunctionPointer(u) is { } fn)
        {
            ctx.IL.Add(CilOpCodes.Ldftn, fn);
            return;
        }
        // Type.GetTypeFromHandle(RuntimeTypeHandle): an Il2CppType* marker becomes ldtoken <Type>.
        if (type.FullName == "System.RuntimeTypeHandle" && v is MarkerExpr { Marker: MarkerKind.Il2CppType, Usage: { Kind: MetadataUsageKind.Il2CppType } tu }
            && stubs.ToSig(module, tu.Index) is { } typeSig)
        {
            ctx.IL.Add(CilOpCodes.Ldtoken, typeSig.ToTypeDefOrRef());
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
    public int ReceiverTypeDef() => regs.Get(ArgumentLocations.FirstIntRegister) switch
    {
        NewObjExpr n => mem.TypeDefOf(n.Type),
        AddressExpr a => mem.TypeDefOf(a.Target.Type),
        var e => mem.TypeDefOf(e.Type),
    };

    /// <summary>
    /// MSVC folds every delegate constructor (all are <c>(object target, IntPtr method)</c>) into one function, so the
    /// call can resolve to e.g. <c>AsyncCallback..ctor</c> while the object being built is a <c>UnityAction&lt;string&gt;</c>.
    /// For delegate objects the constructor of the allocated type itself is used (same signature by definition).
    /// </summary>
    private IMethodDescriptor? DelegateCtorFor(NewObjExpr obj, IMethodDescriptor resolved)
    {
        // (object, native int) is the delegate constructor shape; no other constructor IL2CPP emits has it.
        if (resolved.Signature is not MethodSignature { ParameterTypes.Count: 2 } sig
            || sig.ParameterTypes[0].ElementType != AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Object
            || sig.ParameterTypes[1].ElementType != AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I) return null;
        if (resolved.DeclaringType is { } dt && dt.FullName == obj.Type.FullName) return null;
        return new AsmResolver.DotNet.MemberReference(obj.Type.ToTypeDefOrRef(), ".ctor", sig);
    }

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
