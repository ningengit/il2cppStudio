using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Restores natural C# comparison order: flips literal-on-left (<c>0.5f &gt; x</c> → <c>x &lt; 0.5f</c>) and simplifies
/// negated relational expressions produced by x64 float/unsigned branch inversion (<c>!(a &lt; b)</c> → <c>a &gt;= b</c>).
/// </summary>
public static class ComparisonNormalizer
{
    public static void Apply(SyntaxTree tree)
    {
        var negations = new List<UnaryOperatorExpression>();
        var binaries = new List<BinaryOperatorExpression>();
        AstWalk.ForEachNode(tree, n =>
        {
            if (n is UnaryOperatorExpression { Operator: UnaryOperatorType.Not } u)
                negations.Add(u);
            else if (n is BinaryOperatorExpression b && IsLiteral(b.Left) && !IsLiteral(b.Right))
                binaries.Add(b);
        });

        foreach (var u in negations)
        {
            if (u.Parent is null) continue;
            var inner = u.Expression is ParenthesizedExpression p ? p.Expression : u.Expression;
            if (inner is not BinaryOperatorExpression b) continue;
            var inv = b.Operator switch
            {
                BinaryOperatorType.LessThan => BinaryOperatorType.GreaterThanOrEqual,
                BinaryOperatorType.LessThanOrEqual => BinaryOperatorType.GreaterThan,
                BinaryOperatorType.GreaterThan => BinaryOperatorType.LessThanOrEqual,
                BinaryOperatorType.GreaterThanOrEqual => BinaryOperatorType.LessThan,
                _ => (BinaryOperatorType?)null,
            };
            if (inv is null) continue;
            var replaced = new BinaryOperatorExpression(b.Left.Detach(), inv.Value, b.Right.Detach());
            u.ReplaceWith(replaced);
            if (IsLiteral(replaced.Left) && !IsLiteral(replaced.Right))
                binaries.Add(replaced);
        }

        foreach (var b in binaries)
        {
            if (b.Parent is null || !IsLiteral(b.Left) || IsLiteral(b.Right)) continue;
            var flipped = b.Operator switch
            {
                BinaryOperatorType.LessThan => BinaryOperatorType.GreaterThan,
                BinaryOperatorType.LessThanOrEqual => BinaryOperatorType.GreaterThanOrEqual,
                BinaryOperatorType.GreaterThan => BinaryOperatorType.LessThan,
                BinaryOperatorType.GreaterThanOrEqual => BinaryOperatorType.LessThanOrEqual,
                BinaryOperatorType.Equality => BinaryOperatorType.Equality,
                BinaryOperatorType.InEquality => BinaryOperatorType.InEquality,
                _ => (BinaryOperatorType?)null,
            };
            if (flipped is null) continue;
            var l = b.Left.Detach();
            var r = b.Right.Detach();
            b.Left = r;
            b.Right = l;
            b.Operator = flipped.Value;
        }
    }

    private static bool IsLiteral(Expression e) => e switch
    {
        PrimitiveExpression => true,
        UnaryOperatorExpression { Operator: UnaryOperatorType.Minus or UnaryOperatorType.Plus, Expression: PrimitiveExpression } => true,
        _ => false,
    };
}
