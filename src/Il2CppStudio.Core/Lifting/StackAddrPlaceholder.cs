using AsmResolver.PE.DotNet.Cil;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Address of a stack slot whose type is not known yet (resolved when it reaches a typed call argument).</summary>
public sealed class StackAddrPlaceholder(long key) : Expr
{
    public long Key => key;
    public override ValueKind Kind => ValueKind.Native;
    public override void Emit(EmitContext ctx) => ctx.Native($"&stack[0x{key:X}]");
    public override string Describe() => $"&stack[0x{key:X}]";
}
