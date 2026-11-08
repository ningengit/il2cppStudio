using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Turns setcc-style integer booleans back into C# booleans: <c>((isPlaying ? 1u : 0u) ^ 1u) != 0</c> → <c>!isPlaying</c>,
/// <c>((a ? 1u : 0u) &amp; (b ? 1u : 0u)) == 0</c> → <c>!(a &amp;&amp; b)</c>. <c>&amp;</c>/<c>|</c> become <c>&amp;&amp;</c>/<c>||</c>
/// only when the right operand has no side effects (otherwise short-circuiting would change behaviour).
/// Float comparisons are never inverted (NaN), only wrapped in <c>!</c>. Parentheses are added later by the printer.
/// </summary>
public static class BoolNormalizer
{
    public static void Apply(SyntaxTree tree)
    {
        BoolLocalPromoter.Apply(tree);
        for (var pass = 0; pass < 4; pass++)
        {
            var changed = false;
            foreach (var b in tree.Descendants.OfType<BinaryOperatorExpression>().ToList())
            {
                if (b.Parent is null || b.Operator is not (BinaryOperatorType.Equality or BinaryOperatorType.InEquality)) continue;
                var (other, zero) = IsZero(b.Right) ? (b.Left, true) : IsZero(b.Left) ? (b.Right, true) : (null, false);
                if (!zero || other is null || TryBool(other) is not { } value) continue;
                b.ReplaceWith(b.Operator == BinaryOperatorType.InEquality ? value : Not(value));
                changed = true;
            }
            foreach (var c in tree.Descendants.OfType<ConditionalExpression>().ToList())
            {
                // `x ? true : false` style left over after promotion in boolean positions (if / while conditions).
                if (c.Parent is IfElseStatement or WhileStatement && TryBool(c) is { } v) { c.ReplaceWith(v); changed = true; }
            }
            if (!changed) break;
        }
    }

    /// <summary>The boolean an integer-valued 0/1 expression stands for, or null.</summary>
    public static Expression? TryBool(Expression e)
    {
        switch (e)
        {
            case ParenthesizedExpression p: return TryBool(p.Expression);
            case CastExpression { Type: PrimitiveType { Keyword: "uint" or "int" or "byte" } } cast: return TryBool(cast.Expression);
            case ConditionalExpression c when IsOne(c.TrueExpression) && IsZero(c.FalseExpression): return c.Condition.Clone();
            case ConditionalExpression c when IsZero(c.FalseExpression) && TryBool(c.TrueExpression) is { } t:
                return new BinaryOperatorExpression(c.Condition.Clone(), BinaryOperatorType.ConditionalAnd, t);
            case BinaryOperatorExpression { Operator: BinaryOperatorType.ExclusiveOr } x when IsOne(x.Right) && TryBool(x.Left) is { } l: return Not(l);
            case BinaryOperatorExpression { Operator: BinaryOperatorType.BitwiseAnd or BinaryOperatorType.BitwiseOr } a
                when TryBool(a.Left) is { } l && TryBool(a.Right) is { } r:
                var pure = EmptyIfRemover.IsPure(a.Right);
                var op = a.Operator == BinaryOperatorType.BitwiseAnd
                    ? pure ? BinaryOperatorType.ConditionalAnd : BinaryOperatorType.BitwiseAnd
                    : pure ? BinaryOperatorType.ConditionalOr : BinaryOperatorType.BitwiseOr;
                return new BinaryOperatorExpression(l, op, r);
            default: return null;
        }
    }

    public static Expression Not(Expression e) => e switch
    {
        UnaryOperatorExpression { Operator: UnaryOperatorType.Not } u => u.Expression.Clone(),
        ParenthesizedExpression p => Not(p.Expression),
        BinaryOperatorExpression { Operator: BinaryOperatorType.Equality } b => new BinaryOperatorExpression(b.Left.Clone(), BinaryOperatorType.InEquality, b.Right.Clone()),
        BinaryOperatorExpression { Operator: BinaryOperatorType.InEquality } b => new BinaryOperatorExpression(b.Left.Clone(), BinaryOperatorType.Equality, b.Right.Clone()),
        _ => new UnaryOperatorExpression(UnaryOperatorType.Not, e.Clone()),
    };

    public static bool IsZero(Expression e) => e is PrimitiveExpression { Value: 0 or 0u or 0L or (byte)0 or false };
    public static bool IsOne(Expression e) => e is PrimitiveExpression { Value: 1 or 1u or 1L or (byte)1 or true };
}
