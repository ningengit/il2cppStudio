using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Call to a System.Math method (Sqrt, Abs, Max, Min) reconstructed from an SSE instruction.</summary>
public sealed class MathCallExpr(string name, TypeSignature type, params Expr[] args) : Expr
{
    public string Name => name;
    public IReadOnlyList<Expr> Args => args;
    public override TypeSignature Type => type;
    public override ValueKind Kind => KindOf(type);
    public override string Describe() => $"Math.{name}(…)";

    public override void Emit(EmitContext ctx)
    {
        foreach (var a in args) EmitAs(ctx, a, type, numeric: true);
        var module = ctx.Module;
        var math = new TypeReference(module, module.CorLibTypeFactory.CorLibScope, "System", "Math");
        var sig = MethodSignature.CreateStatic(type, Enumerable.Repeat(type, args.Length));
        ctx.IL.Add(CilOpCodes.Call, new MemberReference(math, name, sig));
    }
}
