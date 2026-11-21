using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.CSharp.Syntax.PatternMatching;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Folds <c>while (true) { if (cond) break; … }</c> into <c>while (!cond) { … }</c>, inlines loop-hoisted constant
/// locals (<c>float num = 20f;</c>), and collapses invariant <c>this.transform</c> / <c>this.gameObject</c> aliases.
/// </summary>
public static class WhileConditionRestorer
{
    public static bool Apply(SyntaxTree tree)
    {
        var changed = false;
        AstWalk.Visit<MethodDeclaration>(tree, m =>
        {
            var body = m.Body;
            if (body.IsNull) return;
            changed |= InlineLocalsAndProps(body, m);
            foreach (var w in AstWalk.Collect<WhileStatement>(body, static w => w.Condition is PrimitiveExpression { Value: true } && w.EmbeddedStatement is BlockStatement))
            {
                if (w.EmbeddedStatement is not BlockStatement wb) continue;
                if (wb.Statements.FirstOrDefault() is not IfElseStatement { FalseStatement.IsNull: true } ifs || !IsBreak(ifs.TrueStatement)) continue;
                w.Condition = ConditionTools.Negate(ifs.Condition.Detach());
                ifs.Remove();
                changed = true;
            }
        });
        return changed;
    }

    private static bool IsBreak(Statement s) =>
        s is BreakStatement || s is BlockStatement { Statements.Count: 1 } b && b.Statements.Single() is BreakStatement;

    private static bool InlineLocalsAndProps(BlockStatement body, MethodDeclaration method)
    {
        List<VariableDeclarationStatement>? litDecls = null;
        List<VariableDeclarationStatement>? propDecls = null;
        AstWalk.Visit<VariableDeclarationStatement>(body, d =>
        {
            if (d.Variables.Count != 1) return;
            var init = d.Variables.Single().Initializer;
            if (init is PrimitiveExpression)
                (litDecls ??= []).Add(d);
            else if (init is MemberReferenceExpression { Target: ThisReferenceExpression, MemberName: "transform" or "gameObject" })
                (propDecls ??= []).Add(d);
        });
        if (litDecls is null && propDecls is null) return false;

        var byName = new Dictionary<string, List<IdentifierExpression>>(StringComparer.Ordinal);
        Dictionary<string, int>? initCounts = null;
        AstWalk.ForEachNode(body, n =>
        {
            if (n is IdentifierExpression id)
            {
                if (!byName.TryGetValue(id.Identifier, out var list))
                    byName[id.Identifier] = list = [];
                list.Add(id);
            }
            else if (propDecls is not null && n is VariableInitializer vi)
            {
                initCounts ??= new Dictionary<string, int>(StringComparer.Ordinal);
                initCounts[vi.Name] = initCounts.GetValueOrDefault(vi.Name) + 1;
            }
        });

        void DecrementInit(string name)
        {
            if (initCounts is not null && initCounts.TryGetValue(name, out var c))
            {
                if (c <= 1) initCounts.Remove(name);
                else initCounts[name] = c - 1;
            }
        }

        var changed = false;
        if (litDecls is not null)
        {
            foreach (var decl in litDecls)
            {
                if (!AstWalk.IsAttachedTo(decl, body)) continue;
                var v = decl.Variables.Single();
                if (v.Initializer is not PrimitiveExpression lit) continue;
                var uses = byName.TryGetValue(v.Name, out var rawUses)
                    ? rawUses.Where(u => AstWalk.IsAttachedTo(u, body)).ToList()
                    : null;
                if (uses is null || uses.Count == 0 || uses.Any(IsMutated)) continue;
                foreach (var u in uses)
                    if (AstWalk.IsAttachedTo(u, body)) u.ReplaceWith(lit.Clone());
                if (AstWalk.IsAttachedTo(decl, body)) decl.Remove();
                DecrementInit(v.Name);
                changed = true;
            }
        }

        if (propDecls is not null)
        {
            foreach (var decl in propDecls)
            {
                if (!AstWalk.IsAttachedTo(decl, body)) continue;
                var v = decl.Variables.Single();
                if (v.Initializer is not MemberReferenceExpression { Target: ThisReferenceExpression, MemberName: "transform" or "gameObject" } prop) continue;
                var allUses = byName.TryGetValue(v.Name, out var rawAllUses)
                    ? rawAllUses.Where(u => AstWalk.IsAttachedTo(u, body)).ToList()
                    : null;
                if (allUses is null || allUses.Count == 0)
                {
                    decl.Remove();
                    DecrementInit(v.Name);
                    changed = true;
                    continue;
                }
                var valid = true;
                var redundantAssigns = new List<ExpressionStatement>();
                var reads = new List<IdentifierExpression>();
                foreach (var u in allUses)
                {
                    if (!AstWalk.IsAttachedTo(u, body)) continue;
                    if (u.Parent is DirectionExpression) { valid = false; break; }
                    if (u.Parent is AssignmentExpression a && a.Left == u)
                    {
                        if (a.Operator == AssignmentOperatorType.Assign && a.Right.IsMatch(prop) && a.Parent is ExpressionStatement es)
                        {
                            redundantAssigns.Add(es);
                            continue;
                        }
                        valid = false;
                        break;
                    }
                    if (u.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement })
                    {
                        valid = false;
                        break;
                    }
                    reads.Add(u);
                }
                if (!valid) continue;
                var aliveCount = initCounts?.GetValueOrDefault(prop.MemberName) ?? 0;
                var hasOtherInit = v.Name == prop.MemberName ? aliveCount > 1 : aliveCount > 0;
                var hasCollision = method.Parameters.Any(p => p.Name == prop.MemberName) || hasOtherInit;
                foreach (var r in reads)
                    if (AstWalk.IsAttachedTo(r, body)) r.ReplaceWith(hasCollision ? prop.Clone() : new IdentifierExpression(prop.MemberName));
                foreach (var es in redundantAssigns)
                    if (AstWalk.IsAttachedTo(es, body)) es.Remove();
                if (AstWalk.IsAttachedTo(decl, body)) decl.Remove();
                DecrementInit(v.Name);
                changed = true;
            }
        }
        return changed;
    }

    private static bool IsMutated(IdentifierExpression id) =>
        id.Parent is AssignmentExpression a && a.Left == id
        || id.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                                                   or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement }
        || id.Parent is DirectionExpression;
}
