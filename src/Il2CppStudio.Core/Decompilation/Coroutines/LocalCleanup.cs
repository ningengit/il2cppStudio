using ICSharpCode.Decompiler.CSharp.Syntax;
using Il2CppStudio.Core.Decompilation.Transforms;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Local clean-ups for reconstructed coroutine bodies: copy-propagation of never-reassigned aliases
/// (<c>PlayerScript p = this;</c>, <c>float n = time;</c> when <c>time</c> is unchanged up to every use), inlining of a
/// single-use temporary into the immediately following statement when nothing evaluated before it has side
/// effects, <c>x = x op e</c> → <c>x op= e</c>, and merging a hoisted local's declaration with its first assignment.
/// </summary>
public static class LocalCleanup
{
    public static void Run(BlockStatement body)
    {
        for (var pass = 0; pass < 16 && (PropagateAliases(body) | InlineAdjacent(body)); pass++) { }
        CompoundAssignments(body);
    }

    private static List<AstNode> Order(BlockStatement body) => [.. body.Descendants];

    private static bool IsAssigned(BlockStatement body, string name) =>
        body.Descendants.Any(n => n is AssignmentExpression { Left: IdentifierExpression id } && id.Identifier == name
            || n is UnaryOperatorExpression { Expression: IdentifierExpression u } un && u.Identifier == name
               && un.Operator is UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement
            || n is DirectionExpression { Expression: IdentifierExpression d } && d.Identifier == name);

    private static bool PropagateAliases(BlockStatement body)
    {
        var changed = false;
        foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (v.Initializer is not (ThisReferenceExpression or IdentifierExpression) || IsAssigned(body, v.Name)) continue;
            var uses = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
            if (v.Initializer is IdentifierExpression src)
            {
                // The source must not change between the alias and any use of it.
                var order = Order(body);
                var start = order.IndexOf(decl);
                var lastUse = uses.Count == 0 ? start : uses.Max(order.IndexOf);
                var writes = order.Skip(start + 1).Take(lastUse - start).Any(n => n is AssignmentExpression { Left: IdentifierExpression w } && w.Identifier == src.Identifier);
                if (writes) continue;
            }
            foreach (var u in uses) u.ReplaceWith(v.Initializer.Clone());
            decl.Remove();
            changed = true;
        }
        return changed;
    }

    private static bool InlineAdjacent(BlockStatement body)
    {
        var changed = false;
        foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (v.Initializer.IsNull || decl.NextSibling is not Statement next) continue;
            var uses = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
            if (uses.Count != 1 || !uses[0].Ancestors.Contains(next)) continue;
            // Everything in the next statement other than the temp must be pure: then evaluating the initializer at the
            // temp's position instead of before the statement cannot change any observable order.
            var probe = (Statement)next.Clone();
            var probeUse = probe.Descendants.OfType<IdentifierExpression>().First(i => i.Identifier == v.Name);
            probeUse.ReplaceWith(new PrimitiveExpression(0));
            if (!StatementIsPure(probe)) continue;
            uses[0].ReplaceWith(v.Initializer.Detach());
            decl.Remove();
            changed = true;
        }
        return changed;
    }

    private static bool StatementIsPure(Statement s) => s switch
    {
        ExpressionStatement { Expression: AssignmentExpression { Left: IdentifierExpression } a } => ConditionTools.IsPure(a.Right),
        IfElseStatement i => ConditionTools.IsPure(i.Condition),
        WhileStatement w => ConditionTools.IsPure(w.Condition),
        YieldReturnStatement y => ConditionTools.IsPure(y.Expression),
        ReturnStatement r => r.Expression.IsNull || ConditionTools.IsPure(r.Expression),
        _ => false,
    };

    private static void CompoundAssignments(BlockStatement body)
    {
        foreach (var a in body.Descendants.OfType<AssignmentExpression>().Where(a => a.Operator == AssignmentOperatorType.Assign).ToList())
        {
            if (a.Left is not IdentifierExpression target || a.Right is not BinaryOperatorExpression { Left: IdentifierExpression same } bin
                || same.Identifier != target.Identifier) continue;
            var op = bin.Operator switch
            {
                BinaryOperatorType.Add => AssignmentOperatorType.Add,
                BinaryOperatorType.Subtract => AssignmentOperatorType.Subtract,
                BinaryOperatorType.Multiply => AssignmentOperatorType.Multiply,
                BinaryOperatorType.Divide => AssignmentOperatorType.Divide,
                _ => (AssignmentOperatorType?)null,
            };
            if (op is null) continue;
            a.ReplaceWith(new AssignmentExpression(a.Left.Detach(), op.Value, bin.Right.Detach()));
        }
    }

    /// <summary>`T x;` … first statement `x = e;` (x not read before) → `T x = e;`.</summary>
    public static void MergeDeclarations(BlockStatement body, IEnumerable<(string Name, AstType Type)> locals)
    {
        foreach (var (name, type) in locals)
        {
            if (body.Descendants.OfType<VariableDeclarationStatement>().Any(d => d.Variables.Any(v => v.Name == name))) continue;
            var uses = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == name).ToList();
            if (uses.Count == 0) continue;
            var first = body.Statements.FirstOrDefault(s => s.Descendants.OfType<IdentifierExpression>().Any(i => i.Identifier == name));
            if (first is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression l } a }
                && l.Identifier == name && !a.Right.Descendants.OfType<IdentifierExpression>().Any(i => i.Identifier == name))
                first.ReplaceWith(new VariableDeclarationStatement(type.Clone(), name, a.Right.Detach()));
            else
                body.Statements.InsertBefore(body.Statements.First(), new VariableDeclarationStatement(type.Clone(), name));
        }
    }
}
