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
public sealed class ScalarIdioms(RegisterTracker regs, OperandReader ops, OperandWriter writer, CorLibTypeFactory f, VirtualReader reader)
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
                writer.Write(ctx, ins, new MathCallExpr("Abs", f.Single, ops.Read(ins, 0))); return true;
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

    private static bool IsSub(BinaryExpr e) => e.Op.Code == CilCode.Sub;

    private static bool IsSign(BinaryExpr sign, Expr x) =>
        ReferenceEquals(sign.Left, x) || sign.Left is LocalExpr a && x is LocalExpr b && a.Local == b.Local
        || sign.Left is ConvExpr { Operand: var inner } && (ReferenceEquals(inner, x) || inner is LocalExpr c && x is LocalExpr d && c.Local == d.Local)
        || x is ConvExpr { Operand: var xi } && ReferenceEquals(sign.Left is ConvExpr { Operand: var si } ? si : sign.Left, xi);
}
