using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Normalizes switch-lowered state dispatch (<c>num--; if (num == 0) {…} num--; if (num == 0) {…}</c>, MSVC's
/// <c>sub eax,1 / je</c> chains) into nested <c>if (num - 1 == 0) {…} else if (num - 2 == 0) {…}</c> form:
/// alias mutations are folded into the conditions, sequential arms that cannot fall through become else-branches,
/// and trailing statements after a non-falling chain become its final else.
/// </summary>
public static class DispatchNormalizer
{
    public static void Normalize(BlockStatement block, ISet<string> aliases, bool nested, int offset = 0, BlockStatement? rootBody = null, List<Statement>? liftedTails = null)
    {
        var stmts = block.Statements.ToList();
        var k = 0;
        while (k < stmts.Count && stmts[k] is VariableDeclarationStatement) k++;   // alias declarations at the top
        for (; k < stmts.Count; k++)
        {
            if (AliasDelta(stmts[k], aliases) is { } d) { offset += d; stmts[k].Remove(); continue; }
            if (stmts[k] is VariableDeclarationStatement vd
                && vd.Variables.All(v => v.Initializer.IsNull || v.Initializer is DefaultValueExpression)) continue;
            if (stmts[k] is not IfElseStatement ifs || !StateEvaluator.IsStateOnly(ifs.Condition, aliases)) break;
            Shift(ifs.Condition, aliases, offset);
            NormalizeArm(ifs.TrueStatement, aliases, offset, rootBody, liftedTails);
            if (!ifs.FalseStatement.IsNull) NormalizeArm(ifs.FalseStatement, aliases, offset, rootBody, liftedTails);
        }
        // Remaining statements may read the alias with the accumulated offset.
        for (var r = k; r < stmts.Count; r++) Shift(stmts[r], aliases, offset);
        if (nested && liftedTails is not null && k == 1 && stmts.Count > 1 && stmts[0] is IfElseStatement leadIf && StateEvaluator.IsStateOnly(leadIf.Condition, aliases))
            SplitLeadingStateIfWithTail(block, leadIf, stmts.Skip(1).ToList(), liftedTails);
    }

    private static void NormalizeArm(Statement arm, ISet<string> aliases, int offset, BlockStatement? rootBody, List<Statement>? liftedTails)
    {
        if (arm is BlockStatement b) { Normalize(b, aliases, nested: true, offset, rootBody, liftedTails); MergeNested(b, aliases); }
        else Shift(arm, aliases, offset);
    }

    private static void SplitLeadingStateIfWithTail(BlockStatement block, IfElseStatement leadIf, List<Statement> tail, List<Statement> liftedTails)
    {
        if (tail.Count == 0 || tail.All(s => s is VariableDeclarationStatement vd && vd.Variables.All(v => v.Initializer.IsNull)))
            return;
        if (leadIf.FalseStatement.IsNull && !DispatchLayout.CanFallThrough(Last(leadIf.TrueStatement)))
            return; // MergeNested will fold the remainder into leadIf.FalseStatement cleanly.
        if (!CanReachAfterIf(leadIf)) return;
        var label = $"dispatch_tail_{liftedTails.Count}";
        AppendGotoOnFallthrough(leadIf, label);
        liftedTails.Add(new LabelStatement { Label = label });
        foreach (var s in tail) liftedTails.Add(s.Detach());
        if (DispatchLayout.CanFallThrough(liftedTails[^1]))
            liftedTails.Add(new GotoStatement(DispatchLayout.RestLabel));
    }

    private static bool CanReachAfterIf(IfElseStatement ifs) =>
        ifs.FalseStatement.IsNull || DispatchLayout.CanFallThrough(ifs.TrueStatement) || DispatchLayout.CanFallThrough(ifs.FalseStatement);

    private static void AppendGotoOnFallthrough(Statement stmt, string label)
    {
        switch (stmt)
        {
            case IfElseStatement ifs:
                AppendGotoOnFallthrough(ifs.TrueStatement, label);
                if (ifs.FalseStatement.IsNull)
                    ifs.FalseStatement = new BlockStatement { Statements = { new GotoStatement(label) } };
                else
                    AppendGotoOnFallthrough(ifs.FalseStatement, label);
                break;
            case BlockStatement b:
                if (!DispatchLayout.CanFallThrough(b.Statements.LastOrDefault())) return;
                if (b.Statements.LastOrDefault() is IfElseStatement innerIf)
                    AppendGotoOnFallthrough(innerIf, label);
                else
                    b.Statements.Add(new GotoStatement(label));
                break;
            default:
                if (DispatchLayout.CanFallThrough(stmt))
                {
                    var blk = new BlockStatement();
                    stmt.ReplaceWith(blk);
                    blk.Statements.Add(stmt);
                    blk.Statements.Add(new GotoStatement(label));
                }
                break;
        }
    }

    /// <summary>Folds non-falling sequential state tests of a nested block into one else-if chain.</summary>
    public static void MergeNested(BlockStatement block, ISet<string> aliases)
    {
        var stmts = block.Statements.ToList();
        if (stmts.Count == 0 || stmts[0] is not IfElseStatement head || !StateEvaluator.IsStateOnly(head.Condition, aliases)) return;
        var tail = head;
        for (var i = 1; i < stmts.Count; i++)
        {
            while (!tail.FalseStatement.IsNull && tail.FalseStatement is IfElseStatement deeper) tail = deeper;
            if (!tail.FalseStatement.IsNull || DispatchLayout.CanFallThrough(Last(tail.TrueStatement))) return;
            if (stmts[i] is IfElseStatement next && StateEvaluator.IsStateOnly(next.Condition, aliases)) { tail.FalseStatement = next.Detach(); continue; }
            var remainder = new BlockStatement();
            foreach (var s in stmts.Skip(i)) remainder.Statements.Add(s.Detach());
            tail.FalseStatement = remainder;
            return;
        }
    }

    private static Statement? Last(Statement s) => s is BlockStatement b ? b.Statements.LastOrDefault() : s;

    private static int? AliasDelta(Statement s, ISet<string> aliases) => s is ExpressionStatement { Expression: var e } ? e switch
    {
        UnaryOperatorExpression { Operator: UnaryOperatorType.PostDecrement or UnaryOperatorType.Decrement, Expression: IdentifierExpression id }
            when aliases.Contains(id.Identifier) => -1,
        UnaryOperatorExpression { Operator: UnaryOperatorType.PostIncrement or UnaryOperatorType.Increment, Expression: IdentifierExpression id }
            when aliases.Contains(id.Identifier) => 1,
        AssignmentExpression { Operator: AssignmentOperatorType.Subtract, Left: IdentifierExpression id, Right: PrimitiveExpression { Value: int v } }
            when aliases.Contains(id.Identifier) => -v,
        AssignmentExpression { Operator: AssignmentOperatorType.Add, Left: IdentifierExpression id, Right: PrimitiveExpression { Value: int v } }
            when aliases.Contains(id.Identifier) => v,
        _ => null,
    } : null;

    /// <summary>Replaces each alias read <c>num</c> by <c>num + offset</c> (the value it has at this point).</summary>
    private static void Shift(AstNode node, ISet<string> aliases, int offset)
    {
        if (offset == 0) return;
        foreach (var id in node.DescendantsAndSelf.OfType<IdentifierExpression>().Where(i => aliases.Contains(i.Identifier)).ToList())
        {
            var op = offset < 0 ? BinaryOperatorType.Subtract : BinaryOperatorType.Add;
            id.ReplaceWith(new ParenthesizedExpression(new BinaryOperatorExpression(new IdentifierExpression(id.Identifier), op, new PrimitiveExpression(Math.Abs(offset)))));
        }
    }
}
