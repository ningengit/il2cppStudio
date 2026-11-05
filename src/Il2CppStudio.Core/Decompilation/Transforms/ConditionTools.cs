using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>Condition manipulation that preserves evaluation order and side effects.</summary>
public static class ConditionTools
{
    /// <summary>
    /// Side-effect-free and non-throwing: locals, literals, and arithmetic/comparison/logic over them. Member access
    /// is excluded (property getters are calls; field access on null throws), as are integer division and modulus.
    /// </summary>
    public static bool IsPure(Expression e) => e switch
    {
        IdentifierExpression or PrimitiveExpression or NullReferenceExpression => true,
        ParenthesizedExpression p => IsPure(p.Expression),
        UnaryOperatorExpression u => u.Operator is UnaryOperatorType.Not or UnaryOperatorType.Minus or UnaryOperatorType.Plus or UnaryOperatorType.BitNot
                                     && IsPure(u.Expression),
        BinaryOperatorExpression b => b.Operator is not (BinaryOperatorType.Divide or BinaryOperatorType.Modulus or BinaryOperatorType.NullCoalescing)
                                      && IsPure(b.Left) && IsPure(b.Right),
        ConditionalExpression c => IsPure(c.Condition) && IsPure(c.TrueExpression) && IsPure(c.FalseExpression),
        _ => false,
    };

    public static bool IsAndFamily(BinaryOperatorType op) => op is BinaryOperatorType.ConditionalAnd or BinaryOperatorType.BitwiseAnd;
    public static bool IsOrFamily(BinaryOperatorType op) => op is BinaryOperatorType.ConditionalOr or BinaryOperatorType.BitwiseOr;

    /// <summary>Flattens a chain of one boolean family (&amp;&amp;/&amp; or ||/|) into its leaves, left to right.</summary>
    public static List<Expression> Leaves(Expression e, out bool? andFamily)
    {
        andFamily = e is BinaryOperatorExpression b && (IsAndFamily(b.Operator) || IsOrFamily(b.Operator)) ? IsAndFamily(b.Operator) : null;
        var leaves = new List<Expression>();
        Collect(e, andFamily, leaves);
        return leaves;
    }

    private static void Collect(Expression e, bool? andFamily, List<Expression> leaves)
    {
        while (e is ParenthesizedExpression p) e = p.Expression;
        if (andFamily is { } and && e is BinaryOperatorExpression b && (and ? IsAndFamily(b.Operator) : IsOrFamily(b.Operator)))
        {
            Collect(b.Left, andFamily, leaves);
            Collect(b.Right, andFamily, leaves);
        }
        else leaves.Add(e);
    }

    /// <summary>Logical negation, pushed through !, &amp;/|, &amp;&amp;/|| and ==/!= (comparisons are not flipped: NaN).</summary>
    public static Expression Negate(Expression e)
    {
        while (e is ParenthesizedExpression p) e = p.Expression;
        switch (e)
        {
            case UnaryOperatorExpression { Operator: UnaryOperatorType.Not } u:
                return u.Expression.Detach();
            case BinaryOperatorExpression b when IsAndFamily(b.Operator) || IsOrFamily(b.Operator):
                var op = b.Operator switch
                {
                    BinaryOperatorType.ConditionalAnd => BinaryOperatorType.ConditionalOr,
                    BinaryOperatorType.ConditionalOr => BinaryOperatorType.ConditionalAnd,
                    BinaryOperatorType.BitwiseAnd => BinaryOperatorType.BitwiseOr,
                    _ => BinaryOperatorType.BitwiseAnd,
                };
                return new BinaryOperatorExpression(Negate(b.Left.Detach()), op, Negate(b.Right.Detach()));
            case BinaryOperatorExpression { Operator: BinaryOperatorType.Equality } eq:
                return new BinaryOperatorExpression(eq.Left.Detach(), BinaryOperatorType.InEquality, eq.Right.Detach());
            case BinaryOperatorExpression { Operator: BinaryOperatorType.InEquality } ne:
                return new BinaryOperatorExpression(ne.Left.Detach(), BinaryOperatorType.Equality, ne.Right.Detach());
            default:
                return new UnaryOperatorExpression(UnaryOperatorType.Not, e.Detach());
        }
    }

    /// <summary>True when <paramref name="node"/> sits under a short-circuit / conditional operator inside <paramref name="root"/>.</summary>
    public static bool UnderShortCircuit(AstNode node, AstNode root)
    {
        for (var n = node.Parent; n is not null && n != root.Parent; n = n.Parent)
        {
            if (n is ConditionalExpression) return true;
            if (n is BinaryOperatorExpression b && b.Operator is BinaryOperatorType.ConditionalAnd or BinaryOperatorType.ConditionalOr
                or BinaryOperatorType.NullCoalescing && !b.Left.Descendants.Contains(node) && b.Left != node) return true;
            if (n == root) break;
        }
        return false;
    }
}
