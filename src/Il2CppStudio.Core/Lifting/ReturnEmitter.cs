using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Emits the CIL return for a native <c>ret</c>: RAX / XMM0 / the hidden return-buffer local, or void.</summary>
public sealed class ReturnEmitter(MethodShape shape, RegisterTracker regs, EntryState entry)
{
    public void Emit(EmitContext ctx)
    {
        var type = shape.ReturnType;
        var kind = Expr.KindOf(type);
        if (kind == ValueKind.Void) { ctx.IL.Add(CilOpCodes.Ret); return; }
        if (shape.HasReturnBuffer && entry.ReturnBuffer is { } buffer)
        {
            ctx.IL.Add(CilOpCodes.Ldloc, buffer);
            ctx.IL.Add(CilOpCodes.Ret);
            return;
        }
        var value = regs.Get(MethodShape.IsFloat(type) ? Register.XMM0 : Register.RAX);
        // bool returns arrive in AL; narrow before converting.
        if (kind == ValueKind.I4 && value.Kind == ValueKind.I8) value = new ConvExpr(value, ctx.Module.CorLibTypeFactory.Int32);
        Expr.EmitAs(ctx, value, type);
        ctx.IL.Add(CilOpCodes.Ret);
    }

    /// <summary>Safety net when control can fall off the end: return a default value.</summary>
    public void EmitDefault(EmitContext ctx)
    {
        var type = shape.ReturnType;
        switch (Expr.KindOf(type))
        {
            case ValueKind.Void: break;
            case ValueKind.I4: ctx.IL.Add(CilOpCodes.Ldc_I4_0); break;
            case ValueKind.I8: ctx.IL.Add(CilOpCodes.Ldc_I8, 0L); break;
            case ValueKind.R4: ctx.IL.Add(CilOpCodes.Ldc_R4, 0f); break;
            case ValueKind.R8: ctx.IL.Add(CilOpCodes.Ldc_R8, 0d); break;
            case ValueKind.Ref: ctx.IL.Add(CilOpCodes.Ldnull); break;
            default:
                var temp = regs.NewLocal(type);
                ctx.IL.Add(CilOpCodes.Ldloc, temp);
                break;
        }
        ctx.IL.Add(CilOpCodes.Ret);
    }
}
