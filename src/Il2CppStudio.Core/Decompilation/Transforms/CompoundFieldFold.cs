using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// <c>float num = F; … F = num - E;</c> → <c>… F -= E;</c> (also +, *, /). The native code loaded the field before
/// evaluating <c>E</c>; C# compound assignment reads the field first as well, so evaluation order is identical.
/// Requires: <c>F</c> is a plain field reference, the temp has exactly that one use, and nothing assigns <c>F</c> between
/// the declaration and the compound statement.
/// </summary>
public static class CompoundFieldFold
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var decl in tree.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (!IsField(v.Initializer) || decl.Parent is not BlockStatement block) continue;
            var field = v.Initializer.ToString();
            var uses = block.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
            if (uses.Count != 1 || uses[0].Parent is not BinaryOperatorExpression bin) continue;
            // `F = num - E` (any op), or commuted `F = E + num` / `F = E * num` (exact for + and *; the field was read first either way).
            var tempOnLeft = bin.Left == uses[0];
            if (!tempOnLeft && !(bin.Right == uses[0] && bin.Operator is BinaryOperatorType.Add or BinaryOperatorType.Multiply)) continue;
            if (bin.Parent is not AssignmentExpression { Operator: AssignmentOperatorType.Assign } assign || assign.Right != bin || assign.Left.ToString() != field) continue;
            var op = bin.Operator switch
            {
                BinaryOperatorType.Add => AssignmentOperatorType.Add, BinaryOperatorType.Subtract => AssignmentOperatorType.Subtract,
                BinaryOperatorType.Multiply => AssignmentOperatorType.Multiply, BinaryOperatorType.Divide => AssignmentOperatorType.Divide,
                _ => (AssignmentOperatorType?)null,
            };
            if (op is null || WritesBetween(block, decl, assign, field)) continue;
            assign.ReplaceWith(new AssignmentExpression(assign.Left.Detach(), op.Value, (tempOnLeft ? bin.Right : bin.Left).Detach()));
            decl.Remove();
        }
    }

    private static bool IsField(Expression e) =>
        e is MemberReferenceExpression { Target: ThisReferenceExpression } || e is IdentifierExpression id && id.Annotation<ICSharpCode.Decompiler.Semantics.MemberResolveResult>() is not null;

    private static bool WritesBetween(BlockStatement block, Statement decl, AssignmentExpression target, string field)
    {
        var started = false;
        foreach (var n in block.Descendants)
        {
            if (n == decl) { started = true; continue; }
            if (!started) continue;
            if (n == target) return false;
            if (n is AssignmentExpression a && a.Left.ToString() == field) return true;
            if (n is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement } u && u.Expression.ToString() == field) return true;
        }
        return true;
    }
}
