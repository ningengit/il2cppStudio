using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>Extra structuring rules for <see cref="GotoSimplifier"/> (kept separate to stay small).</summary>
public static class GotoRules
{
    /// <summary>
    /// <c>…; goto X; A: S1; …; Sn(no fall-through)</c> where A has exactly one jump: the segment is only reachable through
    /// that jump, so it is moved to replace it. Segments with declarations or other labels are left alone.
    /// </summary>
    public static bool InlineSingleUseSegment(BlockStatement body)
    {
        foreach (var label in body.Descendants.OfType<LabelStatement>().ToList())
        {
            if (label.Parent is not BlockStatement block || label.PrevSibling is not Statement prev || DispatchLayout.CanFallThrough(prev)) continue;
            var jumps = body.Descendants.OfType<GotoStatement>().Where(g => g.Label == label.Label).ToList();
            if (jumps.Count != 1 || jumps[0].Parent is not BlockStatement) continue;

            var segment = new List<Statement>();
            for (var s = label.NextSibling as Statement; s is not null; s = s.NextSibling as Statement)
            {
                if (s is LabelStatement) { segment.Clear(); break; }
                segment.Add(s);
                if (!DispatchLayout.CanFallThrough(s)) break;
            }
            if (segment.Count == 0 || DispatchLayout.CanFallThrough(segment[^1]) || segment.Any(s => jumps[0].Ancestors.Contains(s))
                || DeclaresEscapingLocal(body, segment)) continue;

            var site = jumps[0];
            foreach (var s in segment) site.Parent!.InsertChildBefore(site, s.Detach(), BlockStatement.StatementRole);
            site.Remove();
            label.Remove();
            return true;
        }
        return false;
    }

    /// <summary>True when a local declared inside the segment is referenced anywhere outside it (moving it would break scope).</summary>
    private static bool DeclaresEscapingLocal(BlockStatement body, List<Statement> segment)
    {
        var names = segment.OfType<VariableDeclarationStatement>().SelectMany(d => d.Variables.Select(v => v.Name)).ToHashSet();
        if (names.Count == 0) return false;
        return body.Descendants.OfType<IdentifierExpression>()
            .Any(i => names.Contains(i.Identifier) && !segment.Any(s => i.Ancestors.Contains(s)));
    }

    /// <summary><c>A: B:</c> → every <c>goto A</c> becomes <c>goto B</c> and <c>A</c> is removed.</summary>
    public static bool MergeAdjacentLabels(BlockStatement body)
    {
        var changed = false;
        foreach (var label in body.Descendants.OfType<LabelStatement>().ToList())
        {
            if (label.Parent is null || label.NextSibling is not LabelStatement next) continue;
            foreach (var g in body.Descendants.OfType<GotoStatement>().Where(g => g.Label == label.Label).ToList())
                g.ReplaceWith(new GotoStatement(next.Label));
            label.Remove();
            changed = true;
        }
        return changed;
    }

    /// <summary>
    /// <c>L: S1; …; Sn; goto L;</c> (the only jump to L, and the last statement of L's block) → <c>while (true) { S1; …; Sn; }</c>.
    /// Valid because nothing after the goto in that block is reachable except through other labels, which must be absent.
    /// </summary>
    public static bool TryMakeInfiniteLoop(LabelStatement label, List<GotoStatement> jumps)
    {
        if (jumps.Count != 1 || label.Parent is not BlockStatement block || block.Statements.LastOrDefault() != jumps[0]) return false;
        var siblings = block.Statements.ToList();
        var start = siblings.IndexOf(label);
        var inner = siblings.Skip(start + 1).Take(siblings.Count - start - 2).ToList();
        if (inner.Count == 0 || inner.Any(s => s is LabelStatement)) return false;

        var loopBody = new BlockStatement();
        foreach (var s in inner) loopBody.Statements.Add(s.Detach());
        jumps[0].Remove();
        label.ReplaceWith(new WhileStatement { Condition = new PrimitiveExpression(true), EmbeddedStatement = loopBody });
        return true;
    }
}
