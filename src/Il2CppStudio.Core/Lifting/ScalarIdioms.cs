using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;
using Il2CppStudio.Core.Binary;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Compiler idioms with an exact managed meaning:
/// <list type="bullet">
/// <item><c>cdq/cqo</c> → RDX = sign(EAX); <c>sub eax,edx; sar eax,1</c> → <c>x / 2</c> (signed division by two).</item>
/// <item><c>cvtps2pd/cvtpd2ps</c> → float↔double conversion of lane 0.</item>
/// <item><c>sqrtss/sqrtsd</c> → <c>Math.Sqrt</c>; <c>maxss/minss</c> → <c>Math.Max/Min</c>;
/// <c>andps x, [0x7FFFFFFF mask]</c> → <c>Math.Abs</c>.</item>
/// </list>
/// </summary>
public sealed class ScalarIdioms(RegisterTracker regs, OperandReader ops, OperandWriter writer, CorLibTypeFactory f, VirtualReader reader, MemoryResolver mem)
{
    public bool TryTranslate(EmitContext ctx, in Instruction ins)
    {
        switch (ins.Mnemonic)
        {
            case Mnemonic.Cdq:
                regs.Set(Register.RDX, new BinaryExpr(CilOpCodes.Shr, regs.Get(Register.EAX), new ConstExpr(31, ValueKind.I4), ValueKind.I4));
                return true;
            case Mnemonic.Cqo:
                regs.Set(Register.RDX, new BinaryExpr(CilOpCodes.Shr, regs.Get(Register.RAX), new ConstExpr(63, ValueKind.I8), ValueKind.I8));
                return true;
            case Mnemonic.Sar when ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Immediate8 && ins.Immediate8 == 1
                                   && regs.Get(ins.Op0Register) is BinaryExpr { Left: var x, Right: BinaryExpr { Right: ConstExpr { Value: 31 or 63 } } sign } sub
                                   && sub.Kind == x.Kind && IsSign(sign, x) && IsSub(sub):
                writer.Write(ctx, ins, new BinaryExpr(CilOpCodes.Div, x, new ConstExpr(2, x.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4), x.Kind));
                return true;
            case Mnemonic.Idiv or Mnemonic.Div:
                return TryDivide(ins);
            case Mnemonic.Unpcklps when ins.Op0Kind == OpKind.Register:
                writer.Write(ctx, ins, new PackedFloatsExpr(Lane0(ops.Read(ins, 0)), Lane0(ops.Read(ins, 1)), f.Single)); return true;
            case Mnemonic.Shufps or Mnemonic.Vshufps when ins.OpCount >= 3:
            {
                var imm = (int)ins.GetImmediate(ins.OpCount - 1);
                var a = ops.Read(ins, ins.OpCount == 3 ? 1 : 0);
                var b = ops.Read(ins, ins.OpCount == 3 ? 2 : 1);
                var l0 = SelectLane(a, imm & 3);
                var l1 = SelectLane(a, (imm >> 2) & 3);
                if (l0 is not null && l1 is not null)
                {
                    writer.Write(ctx, ins, new PackedFloatsExpr(l0, l1, f.Single));
                    return true;
                }
                if ((imm & 3) == 1 && b is PackedFloatsExpr sp1)
                {
                    writer.Write(ctx, ins, sp1.Lane1);
                    return true;
                }
                if (l0 is not null && (imm & 3) == 1)
                {
                    writer.Write(ctx, ins, l0);
                    return true;
                }
                return false;
            }
            case Mnemonic.Movshdup when ops.Read(ins, 1) is PackedFloatsExpr sp2:
                writer.Write(ctx, ins, sp2.Lane1); return true;
            case Mnemonic.Movlhps when ins.Op0Kind == OpKind.Register:
                return true;
            case Mnemonic.Movhlps when ins.Op0Kind == OpKind.Register && ops.Read(ins, 1) is VectorConstExpr vc && vc.Lanes.Count >= 4:
                writer.Write(ctx, ins, new PackedFloatsExpr(vc.Lane(2), vc.Lane(3), f.Single)); return true;
            case Mnemonic.Addps or Mnemonic.Subps or Mnemonic.Mulps or Mnemonic.Divps or Mnemonic.Vaddps or Mnemonic.Vsubps or Mnemonic.Vmulps or Mnemonic.Vdivps:
                var (a0, a1) = Split2(ops.Read(ins, ins.OpCount == 3 ? 1 : 0));
                var (b0, b1) = Split2(ops.Read(ins, ins.OpCount - 1));
                if (a0 is not null && a1 is not null && b0 is not null && b1 is not null)
                {
                    var bop = ins.Mnemonic is Mnemonic.Addps or Mnemonic.Vaddps ? CilOpCodes.Add
                        : ins.Mnemonic is Mnemonic.Subps or Mnemonic.Vsubps ? CilOpCodes.Sub
                        : ins.Mnemonic is Mnemonic.Mulps or Mnemonic.Vmulps ? CilOpCodes.Mul : CilOpCodes.Div;
                    writer.Write(ctx, ins, new PackedFloatsExpr(new BinaryExpr(bop, a0, b0, ValueKind.R4), new BinaryExpr(bop, a1, b1, ValueKind.R4), f.Single));
                    return true;
                }
                return false;
            case Mnemonic.Movlps or Mnemonic.Movhps:
                writer.Write(ctx, ins, ops.Read(ins, 1)); return true;
            case Mnemonic.Cvtps2pd:
                writer.Write(ctx, ins, new ConvExpr(ops.Read(ins, 1), f.Double)); return true;
            case Mnemonic.Cvtpd2ps:
                writer.Write(ctx, ins, new ConvExpr(ops.Read(ins, 1), f.Single)); return true;
            case Mnemonic.Sqrtsd:
                writer.Write(ctx, ins, new MathCallExpr("Sqrt", f.Double, ops.Read(ins, 1))); return true;
            case Mnemonic.Sqrtss:
                writer.Write(ctx, ins, new ConvExpr(new MathCallExpr("Sqrt", f.Double, new ConvExpr(ops.Read(ins, 1), f.Double)), f.Single)); return true;
            case Mnemonic.Maxss or Mnemonic.Minss or Mnemonic.Maxsd or Mnemonic.Minsd:
                var t = ins.Mnemonic is Mnemonic.Maxss or Mnemonic.Minss ? f.Single : f.Double;
                var name = ins.Mnemonic is Mnemonic.Maxss or Mnemonic.Maxsd ? "Max" : "Min";
                writer.Write(ctx, ins, new MathCallExpr(name, t, ops.Read(ins, 0), ops.Read(ins, 1))); return true;
            case Mnemonic.Andps when ins.Op1Kind == OpKind.Memory && ins.IsIPRelativeMemoryOperand && reader.ReadU32(ins.IPRelativeMemoryAddress) == 0x7FFFFFFF:
            {
                var src = ops.Read(ins, 0);
                if (src is PackedFloatsExpr pf)
                    writer.Write(ctx, ins, new PackedFloatsExpr(new MathCallExpr("Abs", f.Single, pf.Lane0), new MathCallExpr("Abs", f.Single, pf.Lane1), f.Single));
                else
                    writer.Write(ctx, ins, new MathCallExpr("Abs", f.Single, src));
                return true;
            }
            case Mnemonic.Xorps or Mnemonic.Xorpd when !(ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Register && ins.Op0Register == ins.Op1Register)
                                                       && IsFloatSignMask(ins):
            {
                var src = ops.Read(ins, 0);
                if (src is PackedFloatsExpr pf)
                    writer.Write(ctx, ins, new PackedFloatsExpr(new UnaryExpr(CilOpCodes.Neg, pf.Lane0), new UnaryExpr(CilOpCodes.Neg, pf.Lane1), f.Single));
                else
                    writer.Write(ctx, ins, new UnaryExpr(CilOpCodes.Neg, src));
                return true;
            }
            default:
                return false;
        }
    }

    /// <summary>
    /// <c>cdq; idiv r</c> → EAX = x / r, EDX = x % r (and <c>xor edx,edx; div r</c> → unsigned). Only lifted when the high
    /// half is provably the sign (or zero) extension of the dividend, so the 64/32-bit split is exact.
    /// </summary>
    private bool TryDivide(in Instruction ins)
    {
        var wide = ins.OpCount > 0 && (ins.Op0Kind == OpKind.Register ? ins.Op0Register.IsGPR64() : ins.MemorySize.GetSize() == 8);
        var kind = wide ? ValueKind.I8 : ValueKind.I4;
        var x = regs.Get(wide ? Register.RAX : Register.EAX);
        var high = regs.Get(wide ? Register.RDX : Register.EDX);
        var signed = ins.Mnemonic == Mnemonic.Idiv;
        var ok = signed
            ? high is BinaryExpr { Right: ConstExpr { Value: 31 or 63 } } sign && IsSign(sign, x)
            : high is ConstExpr { Value: 0 };
        if (!ok) return false;
        var divisor = ops.Read(ins, 0);
        regs.Set(wide ? Register.RAX : Register.EAX, new BinaryExpr(signed ? CilOpCodes.Div : CilOpCodes.Div_Un, x, divisor, kind));
        regs.Set(wide ? Register.RDX : Register.EDX, new BinaryExpr(signed ? CilOpCodes.Rem : CilOpCodes.Rem_Un, x, divisor, kind));
        return true;
    }

    private static Expr Lane0(Expr e) => e is PackedFloatsExpr p ? p.Lane0 : e is VectorConstExpr vc && vc.Lanes.Count > 0 ? vc.Lane(0) : e;

    private Expr? SelectLane(Expr e, int lane) => lane switch
    {
        0 => Lane0(e),
        1 => Split2(e).Item2,
        2 or 3 => e is VectorConstExpr vc && lane < vc.Lanes.Count ? vc.Lane(lane) : null,
        _ => null,
    };

    private (Expr?, Expr?) Split2(Expr e) => e switch
    {
        PackedFloatsExpr p => (p.Lane0, p.Lane1),
        VectorConstExpr vc when vc.Lanes.Count >= 2 => (vc.Lane(0), vc.Lane(1)),
        StructChunkExpr ch when ch.Size >= 8 && mem.TryLoadFloatPair(new AddressExpr(ch.Source), ch.Offset) is { } sp => (sp.Lane0, sp.Lane1),
        LocalExpr l when Expr.KindOf(l.Type) == ValueKind.Struct && mem.TryLoadFloatPair(new AddressExpr(l), 0) is { } lp => (lp.Lane0, lp.Lane1),
        FloatExpr { Value: 0 } => (new FloatExpr(0, true), new FloatExpr(0, true)),
        ConstExpr { Value: 0 } => (new FloatExpr(0, true), new FloatExpr(0, true)),
        FloatExpr { Kind: ValueKind.R8 } fd => Unpack64ToFloatPair(unchecked((ulong)BitConverter.DoubleToInt64Bits(fd.Value))),
        ConstExpr { Kind: ValueKind.I8 } c => Unpack64ToFloatPair(unchecked((ulong)c.Value)),
        _ => (null, null),
    };

    private static (Expr, Expr) Unpack64ToFloatPair(ulong bits) => (
        new FloatExpr(BitConverter.UInt32BitsToSingle(unchecked((uint)bits)), single: true),
        new FloatExpr(BitConverter.UInt32BitsToSingle(unchecked((uint)(bits >> 32))), single: true));

    private bool IsFloatSignMask(in Instruction ins)
    {
        if (ins.Op1Kind == OpKind.Memory && ins.IsIPRelativeMemoryOperand && reader.IsMapped(ins.IPRelativeMemoryAddress))
            return reader.ReadU32(ins.IPRelativeMemoryAddress) == 0x80000000u || reader.ReadU64(ins.IPRelativeMemoryAddress) == 0x80000000_00000000UL;
        if (ins.Op1Kind == OpKind.Register)
        {
            var e = regs.Get(ins.Op1Register);
            if (e is VectorConstExpr vc && vc.Lanes.Count > 0 && vc.Lanes[0] == 0x80000000u) return true;
            if (e is FloatExpr fe && ((uint)BitConverter.SingleToInt32Bits((float)fe.Value) == 0x80000000u || (ulong)BitConverter.DoubleToInt64Bits(fe.Value) == 0x80000000_00000000UL)) return true;
            if (e is ConstExpr ce && ((uint)ce.Value == 0x80000000u || (ulong)ce.Value == 0x80000000_00000000UL)) return true;
        }
        return false;
    }

    private static bool IsSub(BinaryExpr e) => e.Op.Code == CilCode.Sub;

    private static bool IsSign(BinaryExpr sign, Expr x) =>
        ReferenceEquals(sign.Left, x) || sign.Left is LocalExpr a && x is LocalExpr b && a.Local == b.Local
        || sign.Left is ConvExpr { Operand: var inner } && (ReferenceEquals(inner, x) || inner is LocalExpr c && x is LocalExpr d && c.Local == d.Local)
        || x is ConvExpr { Operand: var xi } && ReferenceEquals(sign.Left is ConvExpr { Operand: var si } ? si : sign.Left, xi);
}
