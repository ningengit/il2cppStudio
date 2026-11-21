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
        AstWalk.Visit<MethodDeclaration>(tree, static m =>
        {
            if (m.Body.IsNull) return;
            RemoveSelfAliases(m.Body);
            RemoveRefLocals(m.Body);
            RemoveZeroObjectLocals(m.Body);
            SinkPastElseReturn(m.Body);
        });
    }

    private sealed class BodyIndex
    {
        public readonly List<VariableDeclarationStatement> Decls = [];
        public readonly Dictionary<string, List<IdentifierExpression>> IdsByName = new(StringComparer.Ordinal);
        public readonly Dictionary<string, List<AssignmentExpression>> AssignsByVar = new(StringComparer.Ordinal);
        public readonly HashSet<string> AddrTakenVars = new(StringComparer.Ordinal);
        public readonly HashSet<string> IncDecVars = new(StringComparer.Ordinal);
        public readonly List<ExpressionStatement> ExprStmts = [];
        public readonly List<IfElseStatement> BlockIfs = [];

        public BodyIndex(BlockStatement body)
        {
            AstWalk.ForEachNode(body, n =>
            {
                switch (n)
                {
                    case VariableDeclarationStatement { Variables.Count: 1 } d:
                        Decls.Add(d);
                        break;
                    case IdentifierExpression id:
                        if (!IdsByName.TryGetValue(id.Identifier, out var list))
                            IdsByName[id.Identifier] = list = [];
                        list.Add(id);
                        break;
                    case AssignmentExpression { Left: IdentifierExpression lhs } ae:
                        if (!AssignsByVar.TryGetValue(lhs.Identifier, out var alist))
                            AssignsByVar[lhs.Identifier] = alist = [];
                        alist.Add(ae);
                        break;
                    case DirectionExpression { Expression: IdentifierExpression did }:
                        AddrTakenVars.Add(did.Identifier);
                        break;
                    case UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement, Expression: IdentifierExpression uid }:
                        IncDecVars.Add(uid.Identifier);
                        break;
                    case ExpressionStatement es:
                        ExprStmts.Add(es);
                        break;
                    case IfElseStatement ifs when ifs.Parent is BlockStatement:
                        BlockIfs.Add(ifs);
                        break;
                }
            });
        }
    }

    private static void RemoveZeroObjectLocals(BlockStatement body)
    {
        List<VariableDeclarationStatement>? cands = null;
        AstWalk.Visit<VariableDeclarationStatement>(body, d =>
        {
            if (d.Variables.Count == 1 && d.Type is PrimitiveType { Keyword: "int" or "long" })
            {
                var init = d.Variables.Single().Initializer;
                if (init.IsNull || init is DefaultValueExpression or PrimitiveExpression { Value: 0 or 0L })
                    (cands ??= []).Add(d);
            }
        });
        if (cands is null) return;

        var idx = new BodyIndex(body);
        foreach (var decl in cands)
        {
            if (!AstWalk.IsAttachedTo(decl, body)) continue;
            var v = decl.Variables.Single();
            if (idx.AddrTakenVars.Contains(v.Name) || idx.IncDecVars.Contains(v.Name)) continue;
            var writes = idx.AssignsByVar.TryGetValue(v.Name, out var rawWrites)
                ? rawWrites.Where(a => AstWalk.IsAttachedTo(a, body)).ToList()
                : [];
            if (writes.Count == 0
                || writes.Any(a => a.Operator != AssignmentOperatorType.Assign || a.Right is not (PrimitiveExpression { Value: 0 or 0L } or DefaultValueExpression))) continue;
            var writeIds = writes.Select(w => w.Left).ToHashSet();
            var reads = idx.IdsByName.TryGetValue(v.Name, out var allIds)
                ? allIds.Where(i => AstWalk.IsAttachedTo(i, body) && !writeIds.Contains(i)).ToList()
                : [];
            if (!reads.Any(r => r.Parent is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } m }
                && m.TypeArguments.FirstOrDefault() is PrimitiveType { Keyword: "object" })) continue;
            foreach (var r in reads)
            {
                if (!AstWalk.IsAttachedTo(r, body)) continue;
                if (r.Parent is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } m } inv
                    && inv.Parent is not null
                    && m.TypeArguments.FirstOrDefault() is PrimitiveType { Keyword: "object" })
                    inv.ReplaceWith(new NullReferenceExpression());
                else if (r.Parent is not null)
                    r.ReplaceWith(new PrimitiveExpression(0));
            }
            foreach (var w in writes)
                if (w.Parent is ExpressionStatement wes && AstWalk.IsAttachedTo(wes, body)) wes.Remove();
            if (AstWalk.IsAttachedTo(decl, body)) decl.Remove();
        }
    }

    private static void RemoveRefLocals(BlockStatement body)
    {
        List<VariableDeclarationStatement>? refDecls = null;
        List<ExpressionStatement>? redundantDefaults = null;
        AstWalk.ForEachNode(body, n =>
        {
            if (n is VariableDeclarationStatement { Variables.Count: 1 } decl
                && decl.Variables.Single().Initializer is DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression })
                (refDecls ??= []).Add(decl);
            else if (n is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id, Right: DefaultValueExpression } } es
                && es.PrevSibling is VariableDeclarationStatement { Variables.Count: 1 } prev
                && prev.Variables.Single().Name == id.Identifier && prev.Variables.Single().Initializer is DefaultValueExpression)
                (redundantDefaults ??= []).Add(es);
        });
        if (refDecls is not null)
        {
            var ids = AstWalk.Collect<IdentifierExpression>(body);
            foreach (var decl in refDecls)
            {
                if (!AstWalk.IsAttachedTo(decl, body)) continue;
                var v = decl.Variables.Single();
                var target = ((DirectionExpression)v.Initializer).Expression;
                foreach (var u in ids)
                    if (u.Identifier == v.Name && AstWalk.IsAttachedTo(u, body))
                        u.ReplaceWith(target.Clone());
                if (AstWalk.IsAttachedTo(decl, body)) decl.Remove();
            }
        }
        if (redundantDefaults is not null)
            foreach (var exprStmt in redundantDefaults)
                if (AstWalk.IsAttachedTo(exprStmt, body)) exprStmt.Remove();
    }

    private static void RemoveSelfAliases(BlockStatement body)
    {
        List<VariableDeclarationStatement>? cands = null;
        AstWalk.Visit<VariableDeclarationStatement>(body, d =>
        {
            if (d.Variables.Count == 1)
            {
                var init = d.Variables.Single().Initializer;
                if (init.IsNull || init is DefaultValueExpression or ThisReferenceExpression)
                    (cands ??= []).Add(d);
            }
        });
        if (cands is null) return;

        var idx = new BodyIndex(body);
        HashSet<string>? localNames = null;
        foreach (var decl in cands)
        {
            if (!AstWalk.IsAttachedTo(decl, body)) continue;
            var v = decl.Variables.Single();
            if (idx.AddrTakenVars.Contains(v.Name)) continue;
            var writes = idx.AssignsByVar.TryGetValue(v.Name, out var rawWrites)
                ? rawWrites.Where(a => AstWalk.IsAttachedTo(a, body)).ToList()
                : [];
            if (writes.Count == 0
                || writes.Any(a => a.Operator != AssignmentOperatorType.Assign || a.Right is not ThisReferenceExpression)) continue;

            foreach (var w in writes)
                if (w.Parent is ExpressionStatement wes && AstWalk.IsAttachedTo(wes, body)) wes.Remove();
            if (localNames is null)
            {
                localNames = new HashSet<string>(StringComparer.Ordinal);
                AstWalk.Visit<VariableInitializer>(body, vi => localNames.Add(vi.Name));
                if (body.Parent is MethodDeclaration md)
                    foreach (var p in md.Parameters) localNames.Add(p.Name);
            }
            if (idx.IdsByName.TryGetValue(v.Name, out var uses))
            {
                foreach (var u in uses)
                {
                    if (!AstWalk.IsAttachedTo(u, body)) continue;
                    if (u.Parent is MemberReferenceExpression m && m.Target == u && m.Parent is not null && !localNames.Contains(m.MemberName))
                    {
                        var id = new IdentifierExpression(m.MemberName);
                        foreach (var ta in m.TypeArguments.ToList()) id.TypeArguments.Add(ta.Detach());
                        m.ReplaceWith(id);
                    }
                    else if (u.Parent is not null)
                    {
                        u.ReplaceWith(new ThisReferenceExpression());
                    }
                }
            }
            if (AstWalk.IsAttachedTo(decl, body)) decl.Remove();
        }
    }

    private static void SinkPastElseReturn(BlockStatement body)
    {
        foreach (var top in AstWalk.Collect<IfElseStatement>(body, static i => i.Parent is BlockStatement))
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
