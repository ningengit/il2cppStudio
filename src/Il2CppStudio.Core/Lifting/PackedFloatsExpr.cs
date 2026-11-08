using AsmResolver.DotNet.Signatures;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Two floats packed into the low 64 bits of an XMM register by <c>unpcklps a, b</c> (lanes a₀, b₀). MSVC builds
/// Vector2 values and the x/y half of Vector3 this way before an 8-byte <c>movsd</c>/<c>movq</c> store.
/// As a scalar it is lane 0 (exactly what a 4-byte read of the register sees); 8-byte stores are split by
/// <see cref="OperandWriter"/> into two float stores at +0 and +4.
/// </summary>
public sealed class PackedFloatsExpr(Expr lane0, Expr lane1, TypeSignature single) : Expr
{
    public Expr Lane0 => lane0;
    public Expr Lane1 => lane1;
    public override ValueKind Kind => ValueKind.R4;
    public override TypeSignature Type => single;
    public override string Describe() => $"pack({lane0.Describe()}, {lane1.Describe()})";
    public override void Emit(EmitContext ctx) => EmitAs(ctx, lane0, single);
}
