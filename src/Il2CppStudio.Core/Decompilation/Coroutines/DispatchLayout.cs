using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Turns the state dispatch at the top of <c>MoveNext</c> into straight-line code with resume labels:
/// the entry arm (state 0) first, then every arm a resume state jumps to (labelled), then the shared remainder
/// (labelled when a state resumes there). Arms that can fall through get an explicit jump to the remainder, so the
/// layout is exactly equivalent; the simplifier removes jumps that land on the next statement.
/// </summary>
public static class DispatchLayout
{
    public const string RestLabel = "resume_rest";

    public static List<Statement>? Build(BlockStatement body, IReadOnlyCollection<long> resumeStates, Dictionary<long, string> labels)
    {
        var stmts = body.Statements.ToList();
        var aliases = new HashSet<string>();
        var i = 0;
        while (i < stmts.Count && stmts[i] is VariableDeclarationStatement { Variables.Count: 1 } v && !v.Variables.Single().Initializer.IsNull)
        {
            var init = v.Variables.Single().Initializer;
            if (IsThisField(init)) { InlineThisAlias(body, v); stmts.RemoveAt(i); continue; }
            if (!StateEvaluator.IsStateOnly(init, aliases)) break;
            aliases.Add(v.Variables.Single().Name);
            i++;
        }
        DispatchNormalizer.Normalize(body, aliases, nested: false);
        stmts = body.Statements.ToList();
        MergeSequentialArms(stmts, i, aliases);
        var chain = i < stmts.Count && stmts[i] is IfElseStatement ifs && StateEvaluator.IsStateOnly(ifs.Condition, aliases) ? ifs : null;
        var rest = stmts.Skip(chain is null ? i : i + 1).Where(s => s.Parent == body).ToList();
        // State aliases must not be used after the dispatch (they are dropped).
        if (rest.Any(s => s.Descendants.OfType<IdentifierExpression>().Any(id => aliases.Contains(id.Identifier)))) return null;

        var armOf = new Dictionary<long, Statement?>();
        foreach (var state in resumeStates.Prepend(0).Distinct())
        {
            if (chain is null) { armOf[state] = null; continue; }
            if (!TrySelect(chain, state, aliases, out var arm)) return null;
            armOf[state] = arm;
        }
        if (!SubstituteAliases(armOf, aliases)) return null;

        var output = new List<Statement>();
        var restUsed = false;
        var emitted = new List<Statement>();
        foreach (var arm in armOf.Values.Where(a => a is not null).Distinct().OrderBy(a => a == armOf[0] ? 0 : 1))
        {
            var label = $"resume_arm{emitted.Count}";
            foreach (var (state, a) in armOf) if (a == arm && state != 0) labels[state] = label;
            if (armOf.Any(kv => kv.Value == arm && kv.Key != 0)) output.Add(new LabelStatement { Label = label });
            var armStmts = arm is BlockStatement b ? b.Statements.ToList() : [arm!];
            foreach (var s in armStmts) output.Add(s.Detach());
            if (CanFallThrough(armStmts.LastOrDefault())) { output.Add(new GotoStatement(RestLabel)); restUsed = true; }
            emitted.Add(arm!);
        }
        foreach (var (state, a) in armOf) if (a is null && state != 0) { labels[state] = RestLabel; restUsed = true; }
        if (armOf[0] is null && !restUsed) { /* entry falls into the remainder directly */ }
        if (restUsed) output.Add(new LabelStatement { Label = RestLabel });
        foreach (var s in rest) output.Add(s.Detach());
        return output;
    }

    /// <summary>
    /// <c>if (s == 0) { …; return …; } if (s == 1) { … }</c> is an else-if chain in disguise: an arm that cannot fall
    /// through makes the following state test its else branch. Nests such tests so <see cref="TrySelect"/> sees one chain.
    /// </summary>
    private static void MergeSequentialArms(List<Statement> stmts, int start, ISet<string> aliases)
    {
        if (start >= stmts.Count || stmts[start] is not IfElseStatement head || !StateEvaluator.IsStateOnly(head.Condition, aliases)) return;
        var tail = head;
        for (var k = start + 1; k < stmts.Count; k++)
        {
            if (!tail.FalseStatement.IsNull || CanFallThrough(LastOf(tail.TrueStatement))) return;
            if (stmts[k] is not IfElseStatement next || !StateEvaluator.IsStateOnly(next.Condition, aliases)) return;
            tail.FalseStatement = next.Detach();
            tail = next;
            while (tail.FalseStatement is IfElseStatement deeper) tail = deeper;
        }
    }

    private static Statement? LastOf(Statement s) => s is BlockStatement b ? b.Statements.LastOrDefault() : s;

    /// <summary>Replaces state aliases inside an arm by the single state that reaches it; fails for shared arms.</summary>
    private static bool SubstituteAliases(Dictionary<long, Statement?> armOf, ISet<string> aliases)
    {
        foreach (var group in armOf.Where(kv => kv.Value is not null).GroupBy(kv => kv.Value!))
        {
            var uses = group.Key.Descendants.OfType<IdentifierExpression>().Where(id => aliases.Contains(id.Identifier)).ToList();
            if (uses.Count == 0) continue;
            if (group.Count() != 1) return false;
            foreach (var u in uses)
            {
                var constant = new PrimitiveExpression((int)group.First().Key);
                u.ReplaceWith(constant);
                ConstantFolder.FoldUpwards(constant);
            }
        }
        return true;
    }

    private static bool IsThisField(Expression e) =>
        e is IdentifierExpression { Identifier: StateMachineInfo.ThisField }
          or MemberReferenceExpression { Target: ThisReferenceExpression, MemberName: StateMachineInfo.ThisField };

    /// <summary><c>TeleType teleType = &lt;&gt;4__this;</c> → every <c>teleType</c> reads the outer instance directly.</summary>
    private static void InlineThisAlias(BlockStatement body, VariableDeclarationStatement decl)
    {
        var name = decl.Variables.Single().Name;
        foreach (var id in body.Descendants.OfType<IdentifierExpression>().Where(x => x.Identifier == name).ToList())
            id.ReplaceWith(new IdentifierExpression(StateMachineInfo.ThisField));
        decl.Remove();
    }

    private static bool TrySelect(IfElseStatement chain, long state, ISet<string> aliases, out Statement? arm)
    {
        arm = null;
        for (IfElseStatement? cur = chain; cur is not null;)
        {
            if (StateEvaluator.Eval(cur.Condition, state, aliases) is not { } v) return false;
            var next = v != 0 ? cur.TrueStatement : cur.FalseStatement;
            if (next.IsNull) return true;   // falls through to the remainder
            if (InnerChain(next, aliases) is { } nested) { cur = nested; continue; }
            arm = next;
            return true;
        }
        return true;
    }

    /// <summary>A branch that is itself a state test (directly, or as the only statement of a block).</summary>
    private static IfElseStatement? InnerChain(Statement s, ISet<string> aliases)
    {
        var candidate = s is BlockStatement { Statements.Count: 1 } b ? b.Statements.Single() : s;
        return candidate is IfElseStatement ifs && StateEvaluator.IsStateOnly(ifs.Condition, aliases) ? ifs : null;
    }

    public static bool CanFallThrough(Statement? last) =>
        last is not (ReturnStatement or GotoStatement or ThrowStatement or YieldBreakStatement or BreakStatement or ContinueStatement);
}
