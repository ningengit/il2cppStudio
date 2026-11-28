using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Two floats packed into the low 64 bits of an XMM register by <c>unpcklps a, b</c> (lanes a₀, b₀). MSVC builds
/// Vector2 values and the x/y half of Vector3 this way before an 8-byte <c>movsd</c>/<c>movq</c> store.
/// As a scalar it is lane 0 (exactly what a 4-byte read of the register sees); 8-byte stores are split by
/// <see cref="OperandWriter"/> into two float stores at +0 and +4.
/// </summary>
public sealed class PackedFloatsExpr(Expr lane0, Expr lane1, TypeSignature single, Expr? wholeSource = null) : Expr
{
    public Expr Lane0 => lane0;
    public Expr Lane1 => lane1;
    public Expr? WholeSource => wholeSource;
    public override ValueKind Kind => ValueKind.R4;
    public override TypeSignature Type => single;
    public override string Describe() => $"pack({lane0.Describe()}, {lane1.Describe()})";
    public override void Emit(EmitContext ctx) => EmitAs(ctx, lane0, single);
}

/// <summary>
/// Two 32-bit integers/enums packed into the low/high halves of a 64-bit register or stack slot (e.g., an 8-byte
/// two-int struct such as <c>InputBinding</c>). Scalar narrowing to 32 bits sees <see cref="Lo"/>; 8-byte stores
/// and struct parameters unpack both halves cleanly without <c>NativeIntrinsics.Reinterpret&lt;T&gt;</c>.
/// </summary>
public sealed class PackedIntsExpr(Expr lo, Expr hi) : Expr
{
    public Expr Lo => lo;
    public Expr Hi => hi;
    public override ValueKind Kind => ValueKind.I8;
    public override string Describe() => $"packed_i32({lo.Describe()}, {hi.Describe()})";

    public override void Emit(EmitContext ctx)
    {
        var f = ctx.Module.CorLibTypeFactory;
        EmitAs(ctx, lo, f.UInt32);
        ctx.IL.Add(CilOpCodes.Conv_U8);
        EmitAs(ctx, hi, f.UInt32);
        ctx.IL.Add(CilOpCodes.Conv_U8);
        ctx.IL.Add(CilOpCodes.Ldc_I4, 32);
        ctx.IL.Add(CilOpCodes.Shl);
        ctx.IL.Add(CilOpCodes.Or);
    }
}
