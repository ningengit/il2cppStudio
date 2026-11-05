using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Evaluates the state dispatch at the top of <c>MoveNext</c> for a concrete state value. Supports the shapes the
/// lifter produces: an alias local (<c>int num = &lt;&gt;1__state;</c>), comparisons, casts, !, &amp;&amp; and ||.
/// </summary>
public static class StateEvaluator
{
    /// <summary>True when the expression depends only on the state (field or aliases) and constants.</summary>
    public static bool IsStateOnly(Expression e, ISet<string> aliases) => e switch
    {
        IdentifierExpression id => aliases.Contains(id.Identifier) || id.Identifier == StateMachineInfo.StateField,
        MemberReferenceExpression { Target: ThisReferenceExpression, MemberName: StateMachineInfo.StateField } => true,
        PrimitiveExpression => true,
        ParenthesizedExpression p => IsStateOnly(p.Expression, aliases),
        CastExpression c => IsStateOnly(c.Expression, aliases),
        UnaryOperatorExpression u => IsStateOnly(u.Expression, aliases),
        BinaryOperatorExpression b => IsStateOnly(b.Left, aliases) && IsStateOnly(b.Right, aliases),
        _ => false,
    };

    /// <summary>Value of the expression for <paramref name="state"/>; null when it cannot be decided.</summary>
    public static long? Eval(Expression e, long state, ISet<string> aliases)
    {
        switch (e)
        {
            case IdentifierExpression or MemberReferenceExpression:
                return state;
            case PrimitiveExpression p when p.Value is IConvertible:
                return p.Value is bool flag ? (flag ? 1 : 0) : Convert.ToInt64(p.Value);
            case ParenthesizedExpression p:
                return Eval(p.Expression, state, aliases);
            case CastExpression c:
                var inner = Eval(c.Expression, state, aliases);
                return inner is null ? null : c.Type is PrimitiveType { Keyword: "uint" } ? (long)(uint)inner.Value : inner;
            case UnaryOperatorExpression { Operator: UnaryOperatorType.Not } u:
                return Eval(u.Expression, state, aliases) is { } v ? (v == 0 ? 1 : 0) : null;
            case BinaryOperatorExpression b:
                var l = Eval(b.Left, state, aliases);
                var r = Eval(b.Right, state, aliases);
                if (l is null || r is null) return null;
                return b.Operator switch
                {
                    BinaryOperatorType.Add => l + r,
                    BinaryOperatorType.Subtract => l - r,
                    BinaryOperatorType.Equality => l == r ? 1 : 0,
                    BinaryOperatorType.InEquality => l != r ? 1 : 0,
                    BinaryOperatorType.LessThan => l < r ? 1 : 0,
                    BinaryOperatorType.LessThanOrEqual => l <= r ? 1 : 0,
                    BinaryOperatorType.GreaterThan => l > r ? 1 : 0,
                    BinaryOperatorType.GreaterThanOrEqual => l >= r ? 1 : 0,
                    BinaryOperatorType.ConditionalAnd or BinaryOperatorType.BitwiseAnd => l != 0 && r != 0 ? 1 : 0,
                    BinaryOperatorType.ConditionalOr or BinaryOperatorType.BitwiseOr => l != 0 || r != 0 ? 1 : 0,
                    _ => null,
                };
            default:
                return null;
        }
    }
}
