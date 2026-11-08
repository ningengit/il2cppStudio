using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Folds <c>while (true) { if (cond) break; … }</c> into <c>while (!cond) { … }</c> and inlines loop-hoisted constant
/// locals (<c>float num = 20f;</c>) whose values are never modified inside the method body.
/// </summary>
public static class WhileConditionRestorer
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var body in tree.Descendants.OfType<MethodDeclaration>().Select(m => m.Body).Where(b => !b.IsNull).ToList())
        {
            InlineReadOnlyLiterals(body);
            foreach (var w in body.Descendants.OfType<WhileStatement>().ToList())
            {
                if (w.Condition is not PrimitiveExpression { Value: true } || w.EmbeddedStatement is not BlockStatement wb) continue;
                if (wb.Statements.FirstOrDefault() is not IfElseStatement { FalseStatement.IsNull: true } ifs || !IsBreak(ifs.TrueStatement)) continue;
                w.Condition = ConditionTools.Negate(ifs.Condition.Detach());
                ifs.Remove();
            }
        }
    }

    private static bool IsBreak(Statement s) =>
        s is BreakStatement || s is BlockStatement { Statements.Count: 1 } b && b.Statements.Single() is BreakStatement;

    private static void InlineReadOnlyLiterals(BlockStatement body)
    {
        foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (v.Initializer is not PrimitiveExpression lit) continue;
            var uses = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
            if (uses.Count == 0 || uses.Any(IsMutated)) continue;
            foreach (var u in uses) u.ReplaceWith(lit.Clone());
            decl.Remove();
        }
    }

    private static bool IsMutated(IdentifierExpression id) =>
        id.Parent is AssignmentExpression a && a.Left == id
        || id.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                                                   or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement }
        || id.Parent is DirectionExpression;
}
