using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Rewrites iterator plumbing inside the laid-out body:
/// <c>&lt;&gt;2__current = V; …; &lt;&gt;1__state = k; return true;</c> → <c>…; yield return V; goto resume_k;</c>,
/// <c>return false;</c> → <c>yield break;</c>, other state stores are dropped, and field accesses become the
/// user's names (<c>&lt;&gt;4__this</c> → <c>this</c>, <c>&lt;time&gt;5__2</c> → <c>time</c>, parameter copies → parameters).
/// </summary>
public static class YieldRewriter
{
    public static bool Rewrite(BlockStatement body, StateMachineInfo info, IReadOnlyDictionary<long, string> labels, ISet<string> parameters)
    {
        InlineSharedReturns(body);
        foreach (var ret in body.Descendants.OfType<ReturnStatement>().ToList())
        {
            if (ret.Expression is PrimitiveExpression { Value: false }) { ret.ReplaceWith(new YieldBreakStatement()); continue; }
            if (ret.Expression is not PrimitiveExpression { Value: true } || ret.Parent is not BlockStatement block) return false;
            var siblings = block.Statements.ToList();
            var at = siblings.IndexOf(ret);
            var stateIdx = siblings.FindLastIndex(at, s => StateStore(s) is not null);
            var currentIdx = stateIdx < 0 ? -1 : siblings.FindLastIndex(stateIdx, s => IsFieldStore(s, StateMachineInfo.CurrentField, out _));
            if (stateIdx < 0 || currentIdx < 0 || StateStore(siblings[stateIdx]) is not { } k || !labels.TryGetValue(k, out var label)) return false;

            IsFieldStore(siblings[currentIdx], StateMachineInfo.CurrentField, out var value);
            var intervening = siblings.Skip(currentIdx + 1).Take(stateIdx - currentIdx - 1).Any();
            Expression yielded;
            if (!intervening || value is PrimitiveExpression or NullReferenceExpression or IdentifierExpression)
            {
                yielded = value!.Detach();
                siblings[currentIdx].Remove();
            }
            else
            {
                // Keep evaluation order: compute the yielded value where the original stored it.
                var temp = new VariableDeclarationStatement(new PrimitiveType("object"), "current", value!.Detach());
                siblings[currentIdx].ReplaceWith(temp);
                yielded = new IdentifierExpression("current");
            }
            siblings[stateIdx].Remove();
            ret.ReplaceWith(new YieldReturnStatement { Expression = yielded });
            block.Statements.InsertAfter(block.Statements.OfType<YieldReturnStatement>().Last(), new GotoStatement(label));
        }
        foreach (var s in body.Descendants.OfType<ExpressionStatement>().Where(s => StateStore(s) is not null || IsFieldStore(s, StateMachineInfo.StateField, out _)).ToList())
            s.Remove();
        return RewriteFields(body, info, parameters);
    }

    public static void NormalizeBoolResultReturn(BlockStatement body)
    {
        foreach (var finalRet in body.Descendants.OfType<ReturnStatement>().Where(r => r.Expression is BinaryOperatorExpression { Operator: BinaryOperatorType.InEquality, Right: PrimitiveExpression { Value: 0 } }).ToList())
        {
            var bin = (BinaryOperatorExpression)finalRet.Expression;
            var resId = (bin.Left is CastExpression c ? c.Expression : bin.Left) as IdentifierExpression;
            if (resId is null) continue;
            var name = resId.Identifier;
            if (finalRet.PrevSibling is LabelStatement retLbl)
            {
                foreach (var g in body.Descendants.OfType<GotoStatement>().Where(g => g.Label == retLbl.Label).ToList())
                {
                    if (g.PrevSibling is ExpressionStatement { Expression: AssignmentExpression { Left: IdentifierExpression l1, Right: PrimitiveExpression { Value: 1 } } } s1 && l1.Identifier == name)
                    {
                        s1.Remove();
                        g.ReplaceWith(new ReturnStatement(new PrimitiveExpression(true)));
                    }
                    else g.ReplaceWith(new ReturnStatement(new PrimitiveExpression(false)));
                }
                retLbl.Remove();
            }
            if (finalRet.PrevSibling is IfElseStatement { TrueStatement: BlockStatement tb, FalseStatement: BlockStatement fb })
            {
                foreach (var b in new[] { tb, fb })
                {
                    var lastWrite = b.Statements.OfType<ExpressionStatement>().LastOrDefault(s => s.Expression is AssignmentExpression { Left: IdentifierExpression l, Right: PrimitiveExpression { Value: int } } && l.Identifier == name);
                    if (lastWrite?.Expression is AssignmentExpression { Right: PrimitiveExpression { Value: int v } })
                    {
                        if (DispatchLayout.CanFallThrough(b)) b.Statements.Add(new ReturnStatement(new PrimitiveExpression(v != 0)));
                        lastWrite.Remove();
                    }
                }
            }
            foreach (var s in body.Descendants.OfType<ExpressionStatement>().Where(s => s.Expression is AssignmentExpression { Left: IdentifierExpression l } && l.Identifier == name).ToList()) s.Remove();
            foreach (var d in body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1 && d.Variables.Single().Name == name).ToList()) d.Remove();
            finalRet.Remove();
        }
    }

    private static void InlineSharedReturns(BlockStatement body)
    {
        foreach (var s in body.Descendants.OfType<ExpressionStatement>().Where(s => IsFieldStore(s, StateMachineInfo.CurrentField, out _)).ToList())
        {
            if (s.NextSibling is IfElseStatement { TrueStatement: BlockStatement tb } ifs
                && tb.Descendants.OfType<ReturnStatement>().Any(r => r.Expression is PrimitiveExpression { Value: true })
                && !tb.Descendants.OfType<ExpressionStatement>().Any(x => IsFieldStore(x, StateMachineInfo.CurrentField, out _)))
                tb.Statements.InsertBefore(tb.Statements.First(), s.Clone());
        }
        foreach (var lbl in body.Descendants.OfType<LabelStatement>().ToList())
        {
            if (lbl.NextSibling is not ReturnStatement { Expression: PrimitiveExpression { Value: bool } } ret) continue;
            var gotos = body.Descendants.OfType<GotoStatement>().Where(g => g.Label == lbl.Label).ToList();
            if (gotos.Count == 0) continue;
            var fallsIn = DispatchLayout.CanFallThrough(lbl.PrevSibling as Statement);
            foreach (var g in gotos) g.ReplaceWith(ret.Clone());
            lbl.Remove();
            if (!fallsIn) ret.Remove();
        }
        foreach (var ret in body.Descendants.OfType<ReturnStatement>().Where(r => r.Expression is PrimitiveExpression { Value: true }).ToList())
        {
            if (ret.PrevSibling is IfElseStatement { FalseStatement.IsNull: false } ifs
                && DispatchLayout.CanFallThrough(ifs.TrueStatement) && DispatchLayout.CanFallThrough(ifs.FalseStatement)
                && ifs.TrueStatement is BlockStatement tb && ifs.FalseStatement is BlockStatement fb)
            {
                tb.Statements.Add(ret.Clone());
                fb.Statements.Add(ret.Clone());
                ret.Remove();
            }
            else if (ret.PrevSibling is WhileStatement { Condition: PrimitiveExpression { Value: true }, EmbeddedStatement: BlockStatement wb })
            {
                var breaks = wb.Descendants.OfType<BreakStatement>().Where(b => b.Ancestors.First(a => a is WhileStatement or DoWhileStatement or ForStatement or ForeachStatement or SwitchStatement) == wb.Parent).ToList();
                if (breaks.Count > 0) { foreach (var b in breaks) b.ReplaceWith(ret.Clone()); ret.Remove(); }
            }
        }
    }

    private static bool RewriteFields(BlockStatement body, StateMachineInfo info, ISet<string> parameters)
    {
        foreach (var node in body.Descendants.Where(n => n is IdentifierExpression or MemberReferenceExpression { Target: ThisReferenceExpression }).ToList())
        {
            var name = node is IdentifierExpression id ? id.Identifier : ((MemberReferenceExpression)node).MemberName;
            if (name == StateMachineInfo.ThisField) node.ReplaceWith(new ThisReferenceExpression());
            else if (info.HoistedLocals.TryGetValue(name, out var local)) node.ReplaceWith(new IdentifierExpression(local.Name));
            else if (name.StartsWith("<>3__", StringComparison.Ordinal) && parameters.Contains(name[5..])) node.ReplaceWith(new IdentifierExpression(name[5..]));
            else if (node is MemberReferenceExpression && parameters.Contains(name)) node.ReplaceWith(new IdentifierExpression(name));
            else if (StateMachineInfo.IsPlumbing(name)) return false;   // plumbing used in an unsupported way
        }
        return true;
    }

    private static long? StateStore(Statement s) =>
        IsFieldStore(s, StateMachineInfo.StateField, out var v) && v is PrimitiveExpression { Value: int k } && k >= 0 ? k : null;

    private static bool IsFieldStore(Statement s, string field, out Expression? value)
    {
        value = null;
        if (s is not ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign } a }) return false;
        var target = a.Left switch
        {
            IdentifierExpression id => id.Identifier,
            MemberReferenceExpression { Target: ThisReferenceExpression } m => m.MemberName,
            _ => null,
        };
        if (target != field) return false;
        value = a.Right;
        return true;
    }

    /// <summary>All non-negative states the iterator suspends in.</summary>
    public static IReadOnlyCollection<long> ResumeStates(BlockStatement moveNext) =>
        [.. moveNext.Descendants.OfType<ExpressionStatement>().Select(StateStore).OfType<long>().Distinct()];
}
