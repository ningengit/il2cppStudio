using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Runtime;

public enum HelperOutcome { NotHandled, Handled, Terminated }

/// <summary>
/// Emits the CIL meaning of an identified runtime helper call: write barriers, metadata/class init vanish;
/// object_new becomes a pending <c>newobj</c>; array_new → <c>newarr</c>; box → <c>box</c>; casts →
/// <c>isinst</c>/<c>castclass</c>; raise → <c>throw</c>. Each handled call is counted for the lift statistics.
/// </summary>
public sealed class RuntimeHelperEmitter(RuntimeHelperMap map, StubAssemblyBuilder stubs, ModuleDefinition module,
    Il2CppRegistration reg, RegisterTracker regs, StructBinder binder)
{
    public int Handled { get; private set; }

    public HelperOutcome Emit(EmitContext ctx, ulong target, bool resultNullTested)
    {
        var kind = map.KindOf(target);
        if (kind == RuntimeHelperKind.Unknown) return HelperOutcome.NotHandled;
        var rcx = regs.Get(ArgumentLocations.IntArg(0));
        var rdx = regs.Get(ArgumentLocations.IntArg(1));
        Handled++;
        switch (kind)
        {
            case RuntimeHelperKind.WriteBarrier or RuntimeHelperKind.MetadataInit or RuntimeHelperKind.ClassInit or RuntimeHelperKind.ScopeCleanup:
                regs.ClobberVolatiles(CallEmitter.Volatile);
                // Some init helpers return the initialized metadata (lea rcx,[usage]; call; mov rcx,rax): RAX = that value.
                if (kind == RuntimeHelperKind.MetadataInit && rcx is MarkerExpr or StringExpr) regs.Set(Register.RAX, rcx);
                return HelperOutcome.Handled;
            case RuntimeHelperKind.ObjectNew when TypeOf(rcx) is { } t:
                regs.ClobberVolatiles(CallEmitter.Volatile);
                regs.Set(Register.RAX, new NewObjExpr(t, TypeDefOf(rcx), regs));
                return HelperOutcome.Handled;
            case RuntimeHelperKind.ArrayNew when TypeOf(rcx) is SzArrayTypeSignature arr:
                Expr.EmitAs(ctx, rdx, module.CorLibTypeFactory.Int32);
                ctx.IL.Add(CilOpCodes.Newarr, arr.BaseType.ToTypeDefOrRef());
                return Result(ctx, arr);
            case RuntimeHelperKind.Box when TypeOf(rcx) is { } t:
                Expr.EmitAs(ctx, BoxedValue(ctx, rdx, t), t);
                ctx.IL.Add(CilOpCodes.Box, t.ToTypeDefOrRef());
                return Result(ctx, module.CorLibTypeFactory.Object);
            case RuntimeHelperKind.Unbox:
                var unboxType = TypeOf(regs.Get(ArgumentLocations.IntArg(2))) ?? TypeOf(rdx) ?? module.CorLibTypeFactory.Int32;
                CompareExpr.EmitOperand(ctx, rcx, ValueKind.Ref);
                ctx.IL.Add(CilOpCodes.Unbox_Any, unboxType.ToTypeDefOrRef());
                var unboxedLocal = regs.NewLocal(unboxType);
                ctx.IL.Add(CilOpCodes.Stloc, unboxedLocal);
                regs.ClobberVolatiles(CallEmitter.Volatile);
                regs.Set(Register.RAX, new AddressExpr(new LocalExpr(unboxedLocal)));
                return HelperOutcome.Handled;
            case RuntimeHelperKind.Cast when rdx is MarkerExpr { Marker: MarkerKind.ElementClass }:
                // `if (v && !IsInst(v, arr->klass->element_class)) throw ArrayTypeMismatch` - part of stelem.ref in CIL.
                regs.ClobberVolatiles(CallEmitter.Volatile);
                regs.Set(Register.RAX, rcx);
                return HelperOutcome.Handled;
            case RuntimeHelperKind.Cast when TypeOf(rdx) is { } t:
                CompareExpr.EmitOperand(ctx, rcx, ValueKind.Ref);
                ctx.IL.Add(resultNullTested ? CilOpCodes.Isinst : CilOpCodes.Castclass, t.ToTypeDefOrRef());
                return Result(ctx, t);
            case RuntimeHelperKind.InterfaceLookup when rdx is MarkerExpr { Marker: MarkerKind.TypeInfo } iface:
                var slot = regs.Get(ArgumentLocations.IntArg(2)) is ConstExpr c ? (int)c.Value : 0;
                regs.ClobberVolatiles(CallEmitter.Volatile);
                regs.Set(Register.RAX, new MarkerExpr(MarkerKind.VTableEntry, iface.Index, $"iface_vtable[{slot}]", rcx, new(0, MetadataUsageKind.MethodDef, slot)));
                return HelperOutcome.Handled;
            case RuntimeHelperKind.ExceptionFactory:
                regs.ClobberVolatiles(CallEmitter.Volatile);
                regs.Set(Register.RAX, new NativeExpr("runtime_exception"));
                return HelperOutcome.Handled;
            case RuntimeHelperKind.Raise or RuntimeHelperKind.ThrowHelper:
                if (kind == RuntimeHelperKind.Raise && rcx.Kind == ValueKind.Ref) rcx.Emit(ctx);
                else ctx.IL.Add(CilOpCodes.Ldnull);   // runtime-raised exception (null/bounds/cast) — implicit in CIL
                ctx.IL.Add(CilOpCodes.Throw);
                return HelperOutcome.Terminated;
            default:
                Handled--;
                return HelperOutcome.NotHandled;   // operand shape not recognised: fall back to a visible native call
        }
    }

    private HelperOutcome Result(EmitContext ctx, TypeSignature type)
    {
        regs.ClobberVolatiles(CallEmitter.Volatile);
        var local = regs.NewLocal(type);
        ctx.IL.Add(CilOpCodes.Stloc, local);
        regs.Set(Register.RAX, new LocalExpr(local));
        return HelperOutcome.Handled;
    }

    /// <summary>Exact type of a TypeInfo marker, from its runtime-metadata usage (Il2CppType index).</summary>
    private TypeSignature? TypeOf(Expr e) =>
        e is MarkerExpr { Marker: MarkerKind.TypeInfo, Usage: { Kind: MetadataUsageKind.TypeInfo } u } ? stubs.ToSig(module, u.Index) : null;

    private TypeDefinition? TypeDefOf(Expr e) => e is MarkerExpr { Marker: MarkerKind.TypeInfo } m ? stubs.GetTypeDef(m.Index) : null;

    /// <summary>il2cpp_value_box receives a pointer to the value; recover the value itself.</summary>
    private Expr BoxedValue(EmitContext ctx, Expr pointer, TypeSignature t) => pointer switch
    {
        AddressExpr a => a.Target,
        StackAddrPlaceholder p when Expr.KindOf(t) == ValueKind.Struct => binder.BindValue(ctx, p, t),
        StackAddrPlaceholder p => regs.GetSlot(p.Key) ?? new NativeExpr($"stack[0x{p.Key:X}]"),
        _ => new NativeExpr($"*({pointer.Describe()})"),
    };
}
