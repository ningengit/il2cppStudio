using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Turns the goto-based reconstruction into structured C#. Every rule is a local, semantics-preserving rewrite:
/// jumps to the next statement are removed; <c>L: if (c) { …; goto L; }</c> with no other jump to L becomes
/// <c>while (c) { … }</c>; jumps to a <c>yield break</c> become <c>yield break</c>; unreferenced labels and a
/// trailing <c>yield break</c> are dropped.
/// </summary>
public static class GotoSimplifier
{
    public static void Simplify(BlockStatement body)
    {
        for (var pass = 0; pass < 32; pass++)
        {
            var changed = GotoRules.MergeAdjacentLabels(body) | GotoRules.InlineSingleUseSegment(body);
            foreach (var g in body.Descendants.OfType<GotoStatement>().ToList())
            {
                if (g.Parent is null) continue;
                if (JumpsToNext(g)) { g.Remove(); changed = true; continue; }
                if (LabelOf(body, g.Label) is { } target && target.NextSibling is YieldBreakStatement)
                {
                    g.ReplaceWith(new YieldBreakStatement());
                    changed = true;
                }
            }
            foreach (var label in body.Descendants.OfType<LabelStatement>().ToList())
            {
                var jumps = body.Descendants.OfType<GotoStatement>().Where(g => g.Label == label.Label).ToList();
                if (jumps.Count == 0) { label.Remove(); changed = true; continue; }
                if (TryMakeWhile(label, jumps) || GotoRules.TryMakeInfiniteLoop(label, jumps)) changed = true;
            }
            if (body.Statements.LastOrDefault() is YieldBreakStatement trailing) { trailing.Remove(); changed = true; }
            if (!changed) break;
        }
    }

    private static LabelStatement? LabelOf(BlockStatement body, string name) =>
        body.Descendants.OfType<LabelStatement>().FirstOrDefault(l => l.Label == name);

    /// <summary>True when execution reaches the label anyway if the goto is removed.</summary>
    private static bool JumpsToNext(GotoStatement g)
    {
        for (AstNode node = g; node.Parent is not null; node = node.Parent)
        {
            for (var s = node.NextSibling; s is LabelStatement l; s = s.NextSibling)
                if (l.Label == g.Label) return true;
            if (node.NextSibling is Statement) return false;   // something else would run first
            // Last statement of an if-branch / block: control continues after the enclosing statement.
            if (node.Parent is not (BlockStatement or IfElseStatement)) return false;
            if (node.Parent is IfElseStatement ifs && node == ifs.Condition) return false;
        }
        return false;
    }

    private static bool TryMakeWhile(LabelStatement label, List<GotoStatement> jumps)
    {
        if (jumps.Count != 1 || label.NextSibling is not IfElseStatement { FalseStatement.IsNull: true } ifs) return false;
        if (ifs.TrueStatement is not BlockStatement loopBody || loopBody.Statements.LastOrDefault() != jumps[0]) return false;
        jumps[0].Remove();
        var loop = new WhileStatement { Condition = ifs.Condition.Detach(), EmbeddedStatement = loopBody.Detach() };
        ifs.ReplaceWith(loop);
        label.Remove();
        return true;
    }
}
