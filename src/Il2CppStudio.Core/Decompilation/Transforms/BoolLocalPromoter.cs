using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// <c>int num = (flag ? 1 : 0); … if (num != 0)</c> → <c>bool num = flag; … if (num)</c>. Applied only when the local is
/// never reassigned and every use sits in a 0/1 context (compared with zero, or an operand of a boolean bit operation),
/// so every rewritten use is exactly equivalent.
/// </summary>
public static class BoolLocalPromoter
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var decl in tree.Descendants.OfType<VariableDeclarationStatement>().ToList())
        {
            if (decl.Variables.Count != 1 || decl.Type is not PrimitiveType { Keyword: "int" or "uint" }) continue;
            var v = decl.Variables.Single();
            if (v.Initializer.IsNull || BoolNormalizer.TryBool(v.Initializer) is not { } value) continue;
            var scope = decl.Parent;
            if (scope is null) continue;
            var uses = scope.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
            if (uses.Count == 0 || !uses.All(IsBoolContext)) continue;

            decl.Type = new PrimitiveType("bool");
            v.Initializer = value;
            foreach (var u in uses)
            {
                var outer = Outer(u);
                // Keep the operand's integer type (uint casts stay uint) so surrounding arithmetic is unchanged.
                var unsigned = outer is CastExpression { Type: PrimitiveType { Keyword: "uint" } } || decl.Type is PrimitiveType { Keyword: "uint" };
                var asInt = new ConditionalExpression(new IdentifierExpression(v.Name),
                    new PrimitiveExpression(unsigned ? 1u : (object)1), new PrimitiveExpression(unsigned ? 0u : (object)0));
                outer.ReplaceWith(asInt);
            }
        }
    }

    /// <summary>The use including any integer casts around it.</summary>
    private static Expression Outer(Expression use)
    {
        Expression e = use;
        while (e.Parent is CastExpression { Type: PrimitiveType { Keyword: "uint" or "int" or "byte" } } or ParenthesizedExpression) e = (Expression)e.Parent;
        return e;
    }

    private static bool IsBoolContext(IdentifierExpression use)
    {
        var e = Outer(use);
        return e.Parent switch
        {
            BinaryOperatorExpression { Operator: BinaryOperatorType.Equality or BinaryOperatorType.InEquality } b =>
                BoolNormalizer.IsZero(b.Left == e ? b.Right : b.Left),
            BinaryOperatorExpression { Operator: BinaryOperatorType.BitwiseAnd or BinaryOperatorType.BitwiseOr } b =>
                BoolNormalizer.TryBool(b.Left == e ? b.Right : b.Left) is not null,
            BinaryOperatorExpression { Operator: BinaryOperatorType.ExclusiveOr } b => b.Left == e && BoolNormalizer.IsOne(b.Right),
            _ => false,
        };
    }
}
