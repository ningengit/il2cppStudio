using AsmResolver.DotNet.Signatures;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// A 16-byte constant loaded from read-only data into an XMM register (<c>movdqa xmm0, [Color.red bits]</c>).
/// MSVC materializes Color / Vector4 / Quaternion literals this way; 16-byte stores of it are split by
/// <see cref="OperandWriter"/> into four 4-byte float stores. As a scalar it is lane 0.
/// </summary>
public sealed class VectorConstExpr(uint[] lanes, TypeSignature single) : Expr
{
    public IReadOnlyList<uint> Lanes => lanes;
    public override ValueKind Kind => ValueKind.R4;
    public override TypeSignature Type => single;
    public FloatExpr Lane(int i) => new(BitConverter.UInt32BitsToSingle(lanes[i]), single: true);
    public override string Describe() => $"<{string.Join(", ", lanes.Select(l => BitConverter.UInt32BitsToSingle(l)))}>";
    public override void Emit(EmitContext ctx) => Lane(0).Emit(ctx);
}
