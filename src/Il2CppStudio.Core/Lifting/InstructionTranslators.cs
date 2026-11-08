using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Move, arithmetic, SSE and stack-frame instructions. Values are built as expression trees in
/// <see cref="RegisterTracker"/>; only stores to memory emit CIL. <see cref="LastResult"/> records the value of the
/// last flag-setting arithmetic op for the branch translator.
/// </summary>
public sealed class InstructionTranslators(RegisterTracker regs, OperandReader ops, OperandWriter writer, CorLibTypeFactory f, ScalarIdioms? idioms = null)
{
    public Expr? LastResult { get; private set; }

    public bool Translate(EmitContext ctx, in Instruction ins)
    {
        var m = ins.Mnemonic;
        if (TranslateFrame(ins)) return true;
        if (idioms?.TryTranslate(ctx, ins) == true) return true;
        switch (m)
        {
            case Mnemonic.Mov or Mnemonic.Movss or Mnemonic.Movsd or Mnemonic.Movaps or Mnemonic.Movups or Mnemonic.Movapd
                or Mnemonic.Movupd or Mnemonic.Movdqa or Mnemonic.Movdqu or Mnemonic.Movq or Mnemonic.Movd:
                Write(ctx, ins, MoveTyping.Retype(ins, ops.Read(ins, 1))); return true;
            case Mnemonic.Movzx or Mnemonic.Movsx or Mnemonic.Movsxd:
                Write(ctx, ins, new ConvExpr(ops.Read(ins, 1), ins.Op0Register.IsGPR64() ? f.Int64 : f.Int32)); return true;
            case Mnemonic.Cdqe: regs.Set(Register.RAX, new ConvExpr(regs.Get(Register.EAX), f.Int64)); return true;
            case Mnemonic.Cwde: regs.Set(Register.EAX, new ConvExpr(regs.Get(Register.AX), f.Int32)); return true;
            case Mnemonic.Lea:
                Write(ctx, ins, ops.AddressOf(ins)); return true;
            case Mnemonic.Xor or Mnemonic.Xorps or Mnemonic.Xorpd or Mnemonic.Pxor when IsSelf(ins):
                Write(ctx, ins, ins.Op0Register.IsXMM() ? new FloatExpr(0, true) : new ConstExpr(0, ins.Op0Register.IsGPR64() ? ValueKind.I8 : ValueKind.I4));
                return true;
            case Mnemonic.Add: return Binary(ctx, ins, CilOpCodes.Add);
            case Mnemonic.Sub: return Binary(ctx, ins, CilOpCodes.Sub);
            case Mnemonic.Imul or Mnemonic.Mul when ins.OpCount == 1:
                var mulRhs = ops.Read(ins, 0);
                var mulLhs = regs.Get(Register.EAX);
                regs.Set(Register.EAX, new BinaryExpr(CilOpCodes.Mul, mulLhs, mulRhs, ValueKind.I4));
                var wideProd = new BinaryExpr(CilOpCodes.Mul, new ConvExpr(mulLhs, f.Int64), new ConvExpr(mulRhs, f.Int64), ValueKind.I8);
                regs.Set(Register.EDX, new ConvExpr(new BinaryExpr(m == Mnemonic.Imul ? CilOpCodes.Shr : CilOpCodes.Shr_Un, wideProd, new ConstExpr(32, ValueKind.I4), ValueKind.I8), f.Int32));
                return true;
            case Mnemonic.Imul when ins.OpCount == 2: return Binary(ctx, ins, CilOpCodes.Mul);
            case Mnemonic.Imul when ins.OpCount == 3:
                Write(ctx, ins, LastResult = new BinaryExpr(CilOpCodes.Mul, ops.Read(ins, 1), ops.Read(ins, 2), IntKind(ins))); return true;
            case Mnemonic.And: return Binary(ctx, ins, CilOpCodes.And);
            case Mnemonic.Or: return Binary(ctx, ins, CilOpCodes.Or);
            case Mnemonic.Xor: return Binary(ctx, ins, CilOpCodes.Xor);
            case Mnemonic.Shl: return Binary(ctx, ins, CilOpCodes.Shl);
            case Mnemonic.Sar: return Binary(ctx, ins, CilOpCodes.Shr);
            case Mnemonic.Shr: return Binary(ctx, ins, CilOpCodes.Shr_Un);
            case Mnemonic.Inc or Mnemonic.Dec:
                Write(ctx, ins, LastResult = new BinaryExpr(m == Mnemonic.Inc ? CilOpCodes.Add : CilOpCodes.Sub, ops.Read(ins, 0), new ConstExpr(1, IntKind(ins)), IntKind(ins)));
                return true;
            case Mnemonic.Neg or Mnemonic.Not:
                Write(ctx, ins, LastResult = new UnaryExpr(m == Mnemonic.Neg ? CilOpCodes.Neg : CilOpCodes.Not, ops.Read(ins, 0))); return true;
            case Mnemonic.Addss or Mnemonic.Addsd: return Float(ctx, ins, CilOpCodes.Add);
            case Mnemonic.Subss or Mnemonic.Subsd: return Float(ctx, ins, CilOpCodes.Sub);
            case Mnemonic.Mulss or Mnemonic.Mulsd: return Float(ctx, ins, CilOpCodes.Mul);
            case Mnemonic.Divss or Mnemonic.Divsd: return Float(ctx, ins, CilOpCodes.Div);
            case Mnemonic.Cvtsi2ss or Mnemonic.Cvtsd2ss or Mnemonic.Cvtdq2ps: Write(ctx, ins, new ConvExpr(ops.Read(ins, 1), f.Single)); return true;
            case Mnemonic.Cvtsi2sd or Mnemonic.Cvtss2sd: Write(ctx, ins, new ConvExpr(ops.Read(ins, 1), f.Double)); return true;
            case Mnemonic.Cvttss2si or Mnemonic.Cvttsd2si or Mnemonic.Cvtss2si or Mnemonic.Cvtsd2si:
                Write(ctx, ins, new ConvExpr(ops.Read(ins, 1), ins.Op0Register.IsGPR64() ? f.Int64 : f.Int32)); return true;
            default: return false;
        }
    }

    /// <summary>push/pop/sub rsp/add rsp/mov reg,rsp: tracked for stack-slot canonicalization, no CIL.</summary>
    private bool TranslateFrame(in Instruction ins)
    {
        var rspDest = ins.Op0Kind == OpKind.Register && ins.Op0Register == Register.RSP;
        switch (ins.Mnemonic)
        {
            case Mnemonic.Push: regs.RspDelta += 8; return true;
            case Mnemonic.Pop: regs.RspDelta -= 8; if (ins.Op0Kind == OpKind.Register) regs.Clobber(ins.Op0Register); return true;
            case Mnemonic.Sub when rspDest && ins.Op1Kind != OpKind.Register: regs.RspDelta += (long)ins.GetImmediate(1); return true;
            case Mnemonic.Add when rspDest && ins.Op1Kind != OpKind.Register: regs.RspDelta -= (long)ins.GetImmediate(1); return true;
            case Mnemonic.Mov when ins.Op1Kind == OpKind.Register && ins.Op1Register == Register.RSP:
                regs.FrameCopy = ins.Op0Register; regs.FrameCopyDelta = regs.RspDelta;
                regs.Set(ins.Op0Register, new StackAddrPlaceholder(-regs.RspDelta)); return true;   // [reg+d] → stack slot
            case Mnemonic.Mov or Mnemonic.Lea when rspDest: return true;   // epilogue restore
            default: return false;
        }
    }

    private bool Binary(EmitContext ctx, in Instruction ins, CilOpCode op)
    {
        var left = ops.Read(ins, 0);
        var right = ops.Read(ins, 1);
        if (op == CilOpCodes.Add && right is ConstExpr { Value: > 0 and var disp } && ops.TryFieldAddress(left, disp) is { } addr)
        {
            Write(ctx, ins, LastResult = addr);
            return true;
        }
        Write(ctx, ins, LastResult = new BinaryExpr(op, left, right, IntKind(ins)));
        return true;
    }

    private bool Float(EmitContext ctx, in Instruction ins, CilOpCode op)
    {
        var single = ins.Mnemonic.ToString().EndsWith("ss", StringComparison.Ordinal);
        Write(ctx, ins, new BinaryExpr(op, ops.Read(ins, 0), ops.Read(ins, 1), single ? ValueKind.R4 : ValueKind.R8));
        return true;
    }

    private static ValueKind IntKind(in Instruction ins) =>
        (ins.Op0Kind == OpKind.Register ? ins.Op0Register.GetSize() : ins.MemorySize.GetSize()) == 8 ? ValueKind.I8 : ValueKind.I4;

    private static bool IsSelf(in Instruction ins) =>
        ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Register && ins.Op0Register == ins.Op1Register;

    private void Write(EmitContext ctx, in Instruction ins, Expr value) => writer.Write(ctx, ins, value);
}
