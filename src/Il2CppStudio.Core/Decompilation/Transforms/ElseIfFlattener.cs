using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Flattens nested conditionals into <c>if … else if … else if …</c> chains. IL2CPP code evaluates each test into
/// temporaries before the branch (<c>bool flag2 = string.Equals(…); if (… || !flag2)</c>), which blocks C#'s
/// <c>else if</c>. Per <see cref="IfElseStatement"/>, innermost first:
/// <list type="number">
/// <item>Fold single-use temporaries declared right before an inner <c>if</c> into its condition, keeping native
/// evaluation order: the boolean chain becomes non-short-circuit (&amp; / |, valid because every leaf after the
/// first is pure) and temp-bearing leaves are ordered first, in declaration order.</item>
/// <item>If the then-branch is the continuing chain and the else-branch is the action, negate and swap them.</item>
/// <item>Unwrap <c>else { if … }</c> into <c>else if</c>.</item>
/// </list>
/// Every rewrite is semantics-preserving; anything that would reorder side effects is left untouched.
/// </summary>
public static class ElseIfFlattener
{
    public static int Apply(SyntaxTree tree)
    {
        var changes = 0;
        foreach (var body in tree.Descendants.OfType<EntityDeclaration>().Select(e => e.GetChildByRole(Roles.Body)).Where(b => !b.IsNull).ToList())
        {
            foreach (var ifs in body.Descendants.OfType<IfElseStatement>().Reverse().ToList())
            {
                if (ifs.Parent is null) continue;
                changes += Hoist(ifs.TrueStatement, body) + Hoist(ifs.FalseStatement, body);
                if (TrySwap(ifs)) changes++;
                if (UnwrapElse(ifs)) changes++;
            }
        }
        return changes;
    }

    /// <summary>[temp decls…; if (…) …] → [if (folded) …].</summary>
    private static int Hoist(Statement branch, BlockStatement body)
    {
        if (branch is BlockStatement guarded) NormalizeGuard(guarded);
        if (branch is not BlockStatement block || block.Statements.Count < 2 || block.Statements.Last() is not IfElseStatement inner) return 0;
        var temps = block.Statements.Take(block.Statements.Count - 1).ToList();
        if (!temps.All(t => t is VariableDeclarationStatement { Variables.Count: 1 } v && !v.Variables.Single().Initializer.IsNull)) return 0;
        var decls = temps.Cast<VariableDeclarationStatement>().ToList();
        if (!TempFolder.TryFold(decls, inner.Condition, body)) return 0;
        foreach (var d in decls) d.Remove();
        return 1;
    }

    /// <summary>
    /// [temps…; if (G) { …; return; } rest…] → [temps…; if (!G) { rest… } else { …; return; }]. Equivalent because the
    /// guard body never falls through; it turns a trailing guard clause into a chain element.
    /// </summary>
    private static void NormalizeGuard(BlockStatement block)
    {
        var stmts = block.Statements.ToList();
        var g = stmts.FindIndex(s => s is IfElseStatement { FalseStatement.IsNull: true });
        if (g < 0 || g == stmts.Count - 1 || stmts.Take(g).Any(s => s is not VariableDeclarationStatement)) return;
        var guard = (IfElseStatement)stmts[g];
        var exit = guard.TrueStatement is BlockStatement b ? b.Statements.LastOrDefault() : guard.TrueStatement;
        if (exit is not (ReturnStatement or ThrowStatement or GotoStatement or BreakStatement or ContinueStatement or YieldBreakStatement)) return;
        var rest = new BlockStatement();
        foreach (var s in stmts.Skip(g + 1)) rest.Statements.Add(s.Detach());
        var guardBody = guard.TrueStatement.Detach();
        guard.ReplaceWith(new IfElseStatement(ConditionTools.Negate(guard.Condition.Detach()), rest, guardBody));
    }

    private static bool IsChain(Statement s) => s is IfElseStatement || s is BlockStatement { Statements.Count: 1 } b && b.Statements.First() is IfElseStatement;

    /// <summary>if (!c) { chain } else { action } → if (c) { action } else chain.</summary>
    private static bool TrySwap(IfElseStatement ifs)
    {
        if (ifs.FalseStatement.IsNull) return false;
        var swap = IsChain(ifs.TrueStatement) && !IsChain(ifs.FalseStatement)
            || !IsChain(ifs.TrueStatement) && !IsChain(ifs.FalseStatement) && ifs.Condition is UnaryOperatorExpression { Operator: UnaryOperatorType.Not };
        if (!swap) return false;
        var condition = ConditionTools.Negate(ifs.Condition.Detach());
        var trueStmt = ifs.TrueStatement.Detach();
        var falseStmt = ifs.FalseStatement.Detach();
        ifs.Condition = condition;
        ifs.TrueStatement = falseStmt;
        ifs.FalseStatement = trueStmt;
        return true;
    }

    private static bool UnwrapElse(IfElseStatement ifs)
    {
        if (ifs.FalseStatement is not BlockStatement { Statements.Count: 1 } block || block.Statements.First() is not IfElseStatement inner) return false;
        inner.Remove();
        ifs.FalseStatement = inner;
        return true;
    }
}
