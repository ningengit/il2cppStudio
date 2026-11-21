using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Removes <c>if (cond) { }</c> (no else) when the condition has no side effects. Runtime checks that were elided
/// (array-store covariance, class-init guards) leave exactly this shape.
/// </summary>
public static class EmptyIfRemover
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var ifs in AstWalk.Collect<IfElseStatement>(tree, static i =>
            i.FalseStatement.IsNull && i.TrueStatement is BlockStatement { Statements.Count: 0 } && i.Parent is BlockStatement))
        {
            if (ifs.Parent is null || !ifs.FalseStatement.IsNull) continue;
            if (ifs.TrueStatement is not BlockStatement { Statements.Count: 0 } || !IsPure(ifs.Condition)) continue;
            if (ifs.Parent is BlockStatement) ifs.Remove();
        }
    }

    public static bool IsPure(Expression e) =>
        !(e is InvocationExpression or AssignmentExpression or ObjectCreateExpression
          || e is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                                            or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement })
        && !AstWalk.HasAny(e, static n =>
            n is InvocationExpression or AssignmentExpression or ObjectCreateExpression
            || n is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                                             or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement });
}
