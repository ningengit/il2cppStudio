using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Folds the small constant expressions left behind when a state alias is replaced by its known value,
/// e.g. <c>(byte)1 != 0</c> → <c>true</c> (the compiler reused the register holding the state as a boolean).
/// </summary>
public static class ConstantFolder
{
    public static void FoldUpwards(Expression start)
    {
        for (AstNode? node = start.Parent; node is Expression e;)
        {
            var folded = Fold(e);
            if (folded is null) return;
            e.ReplaceWith(folded);
            node = folded.Parent;
        }
    }

    private static Expression? Fold(Expression e) => e switch
    {
        ParenthesizedExpression { Expression: PrimitiveExpression p } => p.Clone(),
        CastExpression { Type: PrimitiveType t, Expression: PrimitiveExpression { Value: int v } } when IsIntegral(t.Keyword) => new PrimitiveExpression(v),
        BinaryOperatorExpression { Left: PrimitiveExpression { Value: int l }, Right: PrimitiveExpression { Value: int r } } b => b.Operator switch
        {
            BinaryOperatorType.Add => new PrimitiveExpression(l + r),
            BinaryOperatorType.Subtract => new PrimitiveExpression(l - r),
            BinaryOperatorType.Equality => new PrimitiveExpression(l == r),
            BinaryOperatorType.InEquality => new PrimitiveExpression(l != r),
            _ => null,
        },
        _ => null,
    };

    private static bool IsIntegral(string keyword) => keyword is "byte" or "sbyte" or "short" or "ushort" or "int" or "uint" or "long" or "ulong";
}
