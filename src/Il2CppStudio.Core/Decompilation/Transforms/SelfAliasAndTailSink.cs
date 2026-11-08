using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Cleans up compiler/lifter artifacts around <c>this</c> and tail-merged branches:
/// <list type="number">
/// <item>Replaces locals whose only assignments are <c>default(T)</c> and <c>this</c> with <c>this</c> (and drops the dead local).</item>
/// <item>When an <c>if … else if … else { return; }</c> chain is followed by a single statement, sinks that statement into
/// each non-exiting branch and drops the <c>else { return; }</c>.</item>
/// </list>
/// </summary>
public static class SelfAliasAndTailSink
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var body in tree.Descendants.OfType<MethodDeclaration>().Select(m => m.Body).Where(b => !b.IsNull).ToList())
        {
            RemoveSelfAliases(body);
            RemoveRefLocals(body);
            RemoveZeroObjectLocals(body);
            SinkPastElseReturn(body);
        }
    }

    private static void RemoveZeroObjectLocals(BlockStatement body)
    {
        foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1 && d.Type is PrimitiveType { Keyword: "int" or "long" }).ToList())
        {
            var v = decl.Variables.Single();
            if (!v.Initializer.IsNull && v.Initializer is not DefaultValueExpression and not PrimitiveExpression { Value: 0 or 0L }) continue;
            if (body.Descendants.OfType<DirectionExpression>().Any(d => d.Expression is IdentifierExpression i && i.Identifier == v.Name)) continue;
            if (body.Descendants.OfType<UnaryOperatorExpression>().Any(u => u.Expression is IdentifierExpression i && i.Identifier == v.Name
                && u.Operator is UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement)) continue;
            var writes = body.Descendants.OfType<AssignmentExpression>().Where(a => a.Left is IdentifierExpression id && id.Identifier == v.Name).ToList();
            if (writes.Count == 0 || writes.Any(a => a.Operator != AssignmentOperatorType.Assign || a.Right is not (PrimitiveExpression { Value: 0 or 0L } or DefaultValueExpression))) continue;
            var reads = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name && !writes.Any(w => w.Left == i)).ToList();
            if (!reads.Any(r => r.Parent is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } m }
                && m.TypeArguments.FirstOrDefault() is PrimitiveType { Keyword: "object" })) continue;
            foreach (var r in reads)
            {
                if (r.Parent is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } m } && m.TypeArguments.FirstOrDefault() is PrimitiveType { Keyword: "object" })
                    r.Parent.ReplaceWith(new NullReferenceExpression());
                else r.ReplaceWith(new PrimitiveExpression(0));
            }
            foreach (var w in writes) (w.Parent as ExpressionStatement)?.Remove();
            decl.Remove();
        }
    }

    private static void RemoveRefLocals(BlockStatement body)
    {
        foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (v.Initializer is not DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression target }) continue;
            foreach (var u in body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList())
                u.ReplaceWith(target.Clone());
            decl.Remove();
        }
        foreach (var exprStmt in body.Descendants.OfType<ExpressionStatement>().ToList())
        {
            if (exprStmt.Expression is AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id, Right: DefaultValueExpression }
                && exprStmt.PrevSibling is VariableDeclarationStatement { Variables.Count: 1 } prev
                && prev.Variables.Single().Name == id.Identifier && prev.Variables.Single().Initializer is DefaultValueExpression)
                exprStmt.Remove();
        }
    }

    private static void RemoveSelfAliases(BlockStatement body)
    {
        foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (!v.Initializer.IsNull && v.Initializer is not DefaultValueExpression and not ThisReferenceExpression) continue;
            var writes = body.Descendants.OfType<AssignmentExpression>()
                .Where(a => a.Left is IdentifierExpression id && id.Identifier == v.Name).ToList();
            if (writes.Count == 0 || writes.Any(a => a.Operator != AssignmentOperatorType.Assign || a.Right is not ThisReferenceExpression)) continue;
            if (body.Descendants.OfType<DirectionExpression>().Any(d => d.Expression is IdentifierExpression id && id.Identifier == v.Name)) continue;

            foreach (var w in writes) (w.Parent as ExpressionStatement)?.Remove();
            foreach (var u in body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList())
            {
                if (u.Parent is MemberReferenceExpression m && m.Target == u)
                    m.ReplaceWith(new IdentifierExpression(m.MemberName));
                else u.ReplaceWith(new ThisReferenceExpression());
            }
            decl.Remove();
        }
    }

    private static void SinkPastElseReturn(BlockStatement body)
    {
        foreach (var top in body.Descendants.OfType<IfElseStatement>().Where(i => i.Parent is BlockStatement).ToList())
        {
            if (top.NextSibling is not ExpressionStatement tail || tail.NextSibling is not null) continue;
            var chain = new List<BlockStatement>();
            IfElseStatement? cur = top;
            while (cur is not null)
            {
                if (cur.TrueStatement is not BlockStatement tb) { chain.Clear(); break; }
                chain.Add(tb);
                if (cur.FalseStatement is IfElseStatement next) { cur = next; continue; }
                if (cur.FalseStatement is BlockStatement { Statements.Count: 1 } eb && eb.Statements.Single() is ReturnStatement { Expression.IsNull: true })
                {
                    cur.FalseStatement = Statement.Null;
                    foreach (var b in chain) b.Statements.Add(tail.Clone());
                    tail.Remove();
                }
                break;
            }
        }
    }
}
