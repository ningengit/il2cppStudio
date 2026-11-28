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

    private static readonly Register[] Win32Volatile = [Register.RAX, Register.RCX, Register.RDX,
        Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, Register.XMM5, Register.XMM6, Register.XMM7];

    /// <summary>Declaring typedef of the method currently being lifted (used to distinguish <c>base.M()</c> from <c>this.M()</c>).</summary>
    public int CurrentTypeDef { get; init; } = -1;

    internal MemoryResolver Mem => mem;
    internal StructSlotTable Slots => slots;

    /// <summary>Registers a call clobbers under the active calling convention.</summary>
    public static Register[] Volatile => ArgumentLocations.Abi switch
    {
        NativeAbi.SysV => SysVVolatile,
        NativeAbi.Win32 => Win32Volatile,
        _ => Win64Volatile,
    };

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
        ArgumentLocations.Layout(shape.FloatLayout(mem),
            ArgumentLocations.Abi == NativeAbi.SysV ? shape.SysVLayout(mem) : null,
            ArgumentLocations.Abi == NativeAbi.Win32 ? shape.Win32Layout(mem) : null);

        var retPtr = shape.HasReturnBuffer ? ArgumentLocations.Read(regs, 0, false) : null;
        var thisValue = shape.IsStatic ? null : explicitThis ?? ArgumentLocations.Read(regs, shape.ThisPosition, false);

        if (RebindReceiverGeneric(ref target, ref shape, thisValue, md, reg, stubs, mem, module, methodIndex, specIndex))
        {
            shape.EnsureAbiSupported(mem);
            ArgumentLocations.Layout(shape.FloatLayout(mem),
                ArgumentLocations.Abi == NativeAbi.SysV ? shape.SysVLayout(mem) : null,
                ArgumentLocations.Abi == NativeAbi.Win32 ? shape.Win32Layout(mem) : null);
            retPtr = shape.HasReturnBuffer ? ArgumentLocations.Read(regs, 0, false) : null;
            thisValue = shape.IsStatic ? null : explicitThis ?? ArgumentLocations.Read(regs, shape.ThisPosition, false);
        }

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
            var ctorTarget = DelegateCtorFor(pending, target) ?? target;
            if (pending.Type is GenericInstanceTypeSignature pgi
                && (ctorTarget.DeclaringType is not TypeSpecification { Signature: var cts } || !SignatureComparer.Default.Equals(cts, pgi)))
            {
                ctorTarget = new MemberReference(new TypeSpecification(pgi), ctorTarget.Name, ctorTarget.Signature as MethodSignature);
            }
            ctx.IL.Add(CilOpCodes.Newobj, ctorTarget);
            regs.ClobberVolatiles(Volatile);
            var obj = regs.NewLocal(pending.Type);
            ctx.IL.Add(CilOpCodes.Stloc, obj);
            regs.ReplaceAll(pending, new LocalExpr(obj));
            return true;
        }

        if (thisValue is not null) EmitThis(ctx, shape, target, thisValue);
        for (var i = 0; i < shape.Parameters.Count; i++) EmitParameter(ctx, shape.Parameters[i], shape.FirstParameterPosition + i);

        var isBaseCall = explicitThis is null
            && thisValue is ArgExpr { Parameter.Index: -1 }
            && !shape.IsStatic && !shape.DeclaringIsValueType
            && CurrentTypeDef >= 0
            && md.Methods[methodIndex].DeclaringType != CurrentTypeDef
            && (md.Methods[methodIndex].Flags & 0x0040) != 0
            && mem.IsStrictDerivedFrom(CurrentTypeDef, md.Methods[methodIndex].DeclaringType);
        var useCallvirt = !shape.IsStatic && !shape.DeclaringIsValueType && !isBaseCall;
        ctx.IL.Add(useCallvirt ? CilOpCodes.Callvirt : CilOpCodes.Call, target);
        regs.ClobberVolatiles(Volatile);
        StoreResult(ctx, shape, retPtr);
        return true;
    }

    private void EmitThis(EmitContext ctx, MethodShape shape, IMethodDescriptor target, Expr value)
    {
        var declTd = md.Methods[shape.MethodIndex].DeclaringType;
        var declRef = stubs.TypeRefFor(module, declTd);
        if (declRef is null) { value.Emit(ctx); return; }
        var targetDecl = target is MethodSpecification ms ? ms.Method?.DeclaringType : target.DeclaringType;
        var declSig = targetDecl is TypeSpecification { Signature: { } specSig }
            ? specSig
            : declRef.ToTypeSignature(shape.DeclaringIsValueType);
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
        if (ArgumentLocations.Abi == NativeAbi.SysV && type is not ByReferenceTypeSignature && Expr.KindOf(type) == ValueKind.Struct)
        {
            var spec = MethodShape.ClassifySysVEightbytes(type, mem);
            var argSlots = ArgumentLocations.LocateAll(position, spec);
            var chunks = argSlots.Select(s => ArgumentLocations.ReadSlot(regs, s, position)).ToList();
            if (chunks.Count == 1 && chunks[0] is StackAddrPlaceholder sp && spec.StackOnly)
                binder.BindValue(ctx, sp, type).Emit(ctx);
            else
                EmitStructFromChunks(ctx, type, chunks, mem, regs);
            return;
        }
        if (ArgumentLocations.Abi == NativeAbi.Win32 && type is not ByReferenceTypeSignature && Expr.KindOf(type) == ValueKind.Struct)
        {
            var argSlots = ArgumentLocations.LocateAll(position);
            var wordIdx = argSlots.Count > 0 ? argSlots[0].StackIndex : position;
            var bound = binder.BindValue(ctx, new StackAddrPlaceholder(regs.SlotKey(4L * wordIdx)), type);
            Expr.EmitAs(ctx, bound, type);
            return;
        }
        var v = ArgumentLocations.Read(regs, position, MethodShape.IsFloat(type));
        if (v is StructChunkExpr { Offset: 0 } c && Expr.KindOf(type) == ValueKind.Struct) v = c.Source;
        // Windows x64 passes small structs (<= 8 bytes) in a GPR: Vector2 via unpcklps/movsd, InputBinding/SimulatedKeyState via packed ints or constants.
        if (Expr.KindOf(type) == ValueKind.Struct && !MethodShape.PassedByPointer(type, mem)
            && (v is PackedFloatsExpr or PackedIntsExpr
                || (v is ConstExpr && mem.ValueSize(type) <= 8)
                || (v.Kind is ValueKind.I4 or ValueKind.I8 && mem.ValueSize(type) <= 8 && (v.Type is null || !SignatureComparer.Default.Equals(v.Type, type)) && CanUnpackScalarStruct(type, mem))))
        {
            EmitStructFromChunks(ctx, type, [v], mem, regs);
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
            ctx.IL.Add(CilOpCodes.Ldtoken, Expr.TypeToken(typeSig));
            return;
        }
        // RuntimeHelpers.InitializeArray(..., RuntimeFieldHandle): FieldInfo/FieldRva marker becomes ldtoken <Field>.
        if (type.FullName == "System.RuntimeFieldHandle"
            && v is MarkerExpr { Marker: MarkerKind.FieldInfo, Usage: { Kind: MetadataUsageKind.FieldInfo or MetadataUsageKind.FieldRva } fu }
            && mem.ResolveFieldRefDescriptor(fu.Index) is { } fieldRef)
        {
            ctx.IL.Add(CilOpCodes.Ldtoken, fieldRef);
            return;
        }
        if (Expr.KindOf(type) is ValueKind.I4 or ValueKind.R4 && v.Kind == ValueKind.Struct && v.Type is not null
            && mem.TypeDefOf(v.Type) is var argTd and >= 0
            && mem.ValueSize(type) is var targetSize
            && mem.StructField(v, argTd, 0, desiredSize: targetSize) is { Type: not null } argF0
            && mem.ValueSize(argF0.Type) == targetSize)
        {
            v = argF0;
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
        var baseShape = MethodShape.Create(md, reg, stubs, mem, module, methodIndex, specIndex);
        ArgumentLocations.Layout(baseShape.FloatLayout(mem),
            ArgumentLocations.Abi == NativeAbi.SysV ? baseShape.SysVLayout(mem) : null,
            ArgumentLocations.Abi == NativeAbi.Win32 ? baseShape.Win32Layout(mem) : null);
        var position = baseShape.FirstParameterPosition + baseShape.Parameters.Count;
        if (ArgumentLocations.Read(regs, position, false) is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { Kind: Registration.MetadataUsageKind.MethodRef } u }
            && (uint)u.Index < (uint)reg.MethodSpecs.Count && reg.MethodSpecs[u.Index].MethodDefinitionIndex == methodIndex)
            return u.Index;
        return specIndex;
    }

    internal StubAssemblyBuilder Stubs => stubs;
    internal ModuleDefinition Module => module;

    /// <summary>Static type of the receiver of an instance call, or null.</summary>
    public TypeSignature? ReceiverType()
    {
        var expr = ArgumentLocations.Abi == NativeAbi.Win32 ? Win32ThisCandidate() : regs.Get(ArgumentLocations.FirstIntRegister);
        return expr switch
        {
            NewObjExpr n => n.Type,
            AddressExpr a => a.Target.Type,
            var e => e.Type,
        };
    }

    internal static bool RebindReceiverGeneric(
        ref IMethodDescriptor target, ref MethodShape shape, Expr? thisValue,
        GlobalMetadata md, Il2CppRegistration reg, StubAssemblyBuilder stubs, MemoryResolver mem,
        ModuleDefinition module, int methodIndex, int specIndex)
    {
        if (shape.IsStatic || thisValue is null) return false;
        var baseMr = target as MemberReference ?? (target as MethodSpecification)?.Method as MemberReference;
        if (baseMr?.Parent is not TypeSpecification { Signature: GenericInstanceTypeSignature targetGen }) return false;

        var declTd = md.Methods[methodIndex].DeclaringType;
        var recvType = thisValue switch
        {
            NewObjExpr n => n.Type,
            AddressExpr a => a.Target.Type,
            _ => thisValue.Type,
        };
        if (recvType is null) return false;

        GenericInstanceTypeSignature? matchingRecvGen = null;
        if (recvType is GenericInstanceTypeSignature gi && mem.TypeDefOf(gi) == declTd)
            matchingRecvGen = gi;
        else
        {
            var curTd = mem.TypeDefOf(recvType);
            for (var hop = 0; hop < 16 && (uint)curTd < (uint)md.TypeDefinitions.Length; hop++)
            {
                var parentIdx = md.TypeDefinitions[curTd].ParentIndex;
                if (parentIdx < 0 || !reg.TryGetType(parentIdx, out var parentRec)) break;
                var parentSig = stubs.ToSig(module, parentRec);
                if (parentSig is GenericInstanceTypeSignature pGi && mem.TypeDefOf(pGi) == declTd)
                {
                    matchingRecvGen = pGi;
                    break;
                }
                curTd = mem.TypeDefOf(parentSig);
                if (curTd == declTd) break;
            }
        }

        if (matchingRecvGen is null
            || (HasOpenGenericParam(matchingRecvGen) && thisValue is not (ArgExpr { Parameter.Index: -1 } or NewObjExpr))
            || matchingRecvGen.TypeArguments.Count != targetGen.TypeArguments.Count
            || SignatureComparer.Default.Equals(targetGen, matchingRecvGen))
            return false;

        var rebound = new MemberReference(new TypeSpecification(matchingRecvGen), baseMr.Name, baseMr.Signature as MemberSignature);
        target = target is MethodSpecification ms
            ? new MethodSpecification(rebound, ms.Signature)
            : rebound;
        shape = MethodShape.Create(md, reg, stubs, mem, module, methodIndex, specIndex, classArgsOverride: [.. matchingRecvGen.TypeArguments]);
        return true;
    }

    private static bool HasOpenGenericParam(TypeSignature sig) => sig switch
    {
        GenericParameterSignature => true,
        GenericInstanceTypeSignature gi => gi.TypeArguments.Any(HasOpenGenericParam),
        TypeSpecificationSignature ts => HasOpenGenericParam(ts.BaseType),
        _ => false,
    };

    /// <summary>Static type of the receiver of an instance call, as a typedef index, or -1.</summary>
    public int ReceiverTypeDef()
    {
        var expr = ArgumentLocations.Abi == NativeAbi.Win32 ? Win32ThisCandidate() : regs.Get(ArgumentLocations.FirstIntRegister);
        return expr switch
        {
            NewObjExpr n => mem.TypeDefOf(n.Type),
            AddressExpr a => mem.TypeDefOf(a.Target.Type),
            var e => mem.TypeDefOf(e.Type),
        };
    }

    internal Expr Win32ThisCandidate()
    {
        var s0 = regs.GetSlot(regs.SlotKey(0));
        if (s0 is StackAddrPlaceholder or AddressExpr or null && regs.GetSlot(regs.SlotKey(4)) is { } s1)
            return s1;
        return s0 ?? new NativeExpr("win32_this");
    }

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
                    var retSize = mem.ValueSize(ret);
                    for (var k = p.Key; k < p.Key + retSize; k++) regs.ClearSlot(k);
                    var local = slots.GetOrCreate(p.Key, ret, retSize);
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
        if (ArgumentLocations.Abi == NativeAbi.SysV && Expr.KindOf(ret) == ValueKind.Struct)
        {
            var retRegs = MethodShape.SysVReturnRegs(ret, mem);
            var classes = MethodShape.ClassifySysVEightbytes(ret, mem).Classes;
            var localExpr = new LocalExpr(result);
            for (var c = 0; c < retRegs.Length; c++)
                regs.Set(retRegs[c], MethodShape.SysVChunkExpr(localExpr, ret, c, classes[c], mem, module.CorLibTypeFactory));
            return;
        }
        regs.Set(MethodShape.IsFloat(ret) ? Register.XMM0 : Register.RAX, new LocalExpr(result));
    }

    internal static void EmitStructFromChunks(EmitContext ctx, TypeSignature type, IReadOnlyList<Expr> chunks, MemoryResolver mem, RegisterTracker regs)
    {
        if (chunks.Count > 0 && chunks[0] is StructChunkExpr { Offset: 0 } c0
            && c0.Source.Type is { } st && SignatureComparer.Default.Equals(st, type))
        {
            c0.Source.Emit(ctx);
            return;
        }
        if (chunks.Count > 0 && chunks[0] is PackedFloatsExpr { WholeSource: { } ws }
            && ws.Type is { } wt && SignatureComparer.Default.Equals(wt, type))
        {
            ws.Emit(ctx);
            return;
        }
        if (chunks.Count == 1 && chunks[0] is PackedFloatsExpr p0 && type.FullName == "UnityEngine.Vector2")
        {
            var single = ctx.Module.CorLibTypeFactory.Single;
            Expr.EmitAs(ctx, p0.Lane0, single);
            Expr.EmitAs(ctx, p0.Lane1, single);
            var ctor = new MemberReference(type.ToTypeDefOrRef(), ".ctor",
                MethodSignature.CreateInstance(ctx.Module.CorLibTypeFactory.Void, [single, single]));
            ctx.IL.Add(CilOpCodes.Newobj, ctor);
            return;
        }
        if (chunks.Count == 1 && chunks[0].Type is { } ct && SignatureComparer.Default.Equals(ct, type))
        {
            chunks[0].Emit(ctx);
            return;
        }
        var size = mem.ValueSize(type);
        var local = regs.NewLocal(type);
        var addr = new AddressExpr(new LocalExpr(local));
        for (var i = 0; i < chunks.Count; i++)
        {
            var off = i * 8;
            if (off >= size) break;
            var cSize = Math.Min(8, size - off);
            var ch = chunks[i];
            if (ch is PackedFloatsExpr pf)
            {
                mem.Store(ctx, addr, null, 0, off, 4, pf.Lane0);
                if (off + 4 < size) mem.Store(ctx, addr, null, 0, off + 4, 4, pf.Lane1);
            }
            else if (ch is PackedIntsExpr pi)
            {
                mem.Store(ctx, addr, null, 0, off, 4, pi.Lo);
                if (off + 4 < size) mem.Store(ctx, addr, null, 0, off + 4, 4, pi.Hi);
            }
            else if (ch is VectorConstExpr vc)
            {
                var lanes = Math.Min(4, (cSize + 3) / 4);
                for (var lane = 0; lane < lanes; lane++) mem.Store(ctx, addr, null, 0, off + lane * 4, 4, vc.Lane(lane));
            }
            else if (!mem.Store(ctx, addr, null, 0, off, cSize, ch) && ch is ConstExpr ce && cSize >= 8 && off + 4 < size)
            {
                mem.Store(ctx, addr, null, 0, off, 4, new ConstExpr(unchecked((int)ce.Value), ValueKind.I4));
                mem.Store(ctx, addr, null, 0, off + 4, 4, new ConstExpr(unchecked((int)(ce.Value >> 32)), ValueKind.I4));
            }
        }
        ctx.IL.Add(CilOpCodes.Ldloc, local);
    }

    internal static bool CanUnpackScalarStruct(TypeSignature type, MemoryResolver mem)
    {
        var td = mem.TypeDefOf(type);
        if (td < 0) return false;
        var fields = mem.FieldsOf(td, type).Where(f => !f.IsStatic).OrderBy(f => f.Offset).ToList();
        if (fields.Count == 0 || fields.Count > 4) return false;
        foreach (var f in fields)
        {
            var ft = mem.FieldType(f, type);
            if (Expr.KindOf(ft) is not (ValueKind.I4 or ValueKind.I8 or ValueKind.R4 or ValueKind.R8))
                return false;
        }
        return true;
    }
}
