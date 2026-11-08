using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Reconstructs <c>foreach</c> loops from <c>IEnumerator e = coll.GetEnumerator(); while (e.MoveNext()) { T x = (T)e.Current; … }</c>.
/// </summary>
public static class ForeachRestorer
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var ws in tree.Descendants.OfType<WhileStatement>().ToList())
        {
            if (ws.Parent is not BlockStatement parent) continue;
            if (ws.PrevSibling is not VariableDeclarationStatement { Variables.Count: 1 } enumDecl) continue;
            var enumVar = enumDecl.Variables.Single();
            if (enumVar.Initializer is not InvocationExpression { Arguments.Count: 0, Target: MemberReferenceExpression { MemberName: "GetEnumerator" } getEnum })
                continue;
            if (ws.Condition is not InvocationExpression { Arguments.Count: 0, Target: MemberReferenceExpression { MemberName: "MoveNext", Target: IdentifierExpression moveId } }
                || moveId.Identifier != enumVar.Name) continue;
            if (ws.EmbeddedStatement is not BlockStatement loopBody
                || loopBody.Statements.FirstOrDefault() is not VariableDeclarationStatement { Variables.Count: 1 } itemDecl)
                continue;
            var itemVar = itemDecl.Variables.Single();
            Expression curExpr = itemVar.Initializer;
            if (curExpr is CastExpression cast) curExpr = cast.Expression;
            if (curExpr is not MemberReferenceExpression { MemberName: "Current", Target: IdentifierExpression curId }
                || curId.Identifier != enumVar.Name) continue;

            var allUses = parent.Descendants.OfType<IdentifierExpression>().Count(i => i.Identifier == enumVar.Name);
            if (allUses != 2) continue;

            itemDecl.Remove();
            var fe = new ForeachStatement
            {
                VariableType = itemDecl.Type.Clone(),
                VariableDesignation = new SingleVariableDesignation { Identifier = itemVar.Name },
                InExpression = getEnum.Target.Detach(),
                EmbeddedStatement = loopBody.Detach(),
            };
            ws.ReplaceWith(fe);
            enumDecl.Remove();
        }
        RestoreForLoops(tree);
    }

    private static void RestoreForLoops(SyntaxTree tree)
    {
        foreach (var ws in tree.Descendants.OfType<WhileStatement>().ToList())
        {
            if (ws.Parent is not BlockStatement parent) continue;
            if (ws.PrevSibling is not VariableDeclarationStatement { Variables.Count: 1 } initDecl) continue;
            var loopVar = initDecl.Variables.Single();
            if (loopVar.Initializer.IsNull) continue;
            if (!ws.Condition.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == loopVar.Name)) continue;
            if (ws.EmbeddedStatement is not BlockStatement body || body.Statements.Count < 2) continue;
            if (body.Descendants.OfType<ContinueStatement>().Any()) continue;
            if (body.Statements.LastOrDefault() is not ExpressionStatement stepStmt) continue;
            if (!IsLoopStep(stepStmt.Expression, loopVar.Name)) continue;
            if (UsedAfter(ws, loopVar.Name)) continue;

            stepStmt.Remove();
            var forStmt = new ForStatement
            {
                Condition = ws.Condition.Detach(),
                EmbeddedStatement = body.Detach(),
            };
            forStmt.Initializers.Add(initDecl.Detach());
            forStmt.Iterators.Add(stepStmt);
            ws.ReplaceWith(forStmt);
        }
    }

    private static bool IsLoopStep(Expression expr, string name) => expr switch
    {
        AssignmentExpression { Left: IdentifierExpression l } when l.Identifier == name => true,
        UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement, Expression: IdentifierExpression u } when u.Identifier == name => true,
        _ => false,
    };

    private static bool UsedAfter(Statement stmt, string name)
    {
        for (var next = stmt.NextSibling; next is not null; next = next.NextSibling)
            if (next.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == name))
                return true;
        return false;
    }
}
