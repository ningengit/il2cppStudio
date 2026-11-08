using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Instruction-context typing for moves between register files. A value moved into an XMM register by
/// <c>movd/movq</c> (or a scalar SSE move) is a float/double from then on; a value moved from XMM into a GPR is raw
/// bits. Constants are re-typed eagerly so later uses (calls, stores, spills) see the right type and value.
/// </summary>
public static class MoveTyping
{
    public static Expr Retype(in Instruction ins, Expr value)
    {
        if (ins.Op0Kind != OpKind.Register) return value;
        var dest = ins.Op0Register;
        if (dest.IsXMM())
        {
            var wide = ins.Mnemonic is Mnemonic.Movq or Mnemonic.Movsd or Mnemonic.Movapd or Mnemonic.Movupd
                       || ins.Op1Kind == OpKind.Register && ins.Op1Register.IsGPR64();
            return value switch
            {
                ConstExpr c when !wide => new FloatExpr(BitConverter.Int32BitsToSingle(unchecked((int)c.Value)), single: true),
                ConstExpr c => new FloatExpr(BitConverter.Int64BitsToDouble(c.Value), single: false),
                _ => value,
            };
        }
        if (dest.IsGPR() && value is FloatExpr f)
            return dest.IsGPR64()
                ? new ConstExpr(BitConverter.DoubleToInt64Bits(f.Value), ValueKind.I8)
                : new ConstExpr(BitConverter.SingleToInt32Bits((float)f.Value), ValueKind.I4);
        return value;
    }
}
