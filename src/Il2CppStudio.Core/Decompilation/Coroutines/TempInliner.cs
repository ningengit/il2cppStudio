using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Inlines single-use temporary locals with side-effect-free initializers across intervening pure declarations
/// into their sole use site (e.g. <c>Vector3 p = player.position; if (Vector3.Distance(p, …) &lt; 10f)</c>).
/// </summary>
public static class TempInliner
{
    public static void Run(BlockStatement body)
    {
        bool changed;
        do
        {
            changed = false;
            foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().ToList())
            {
                if (decl.Parent is not BlockStatement || decl.Variables.Count != 1) continue;
                var v = decl.Variables.Single();
                if (v.Initializer.IsNull) continue;
                var uses = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
                if (uses.Count != 1 || IsWriteTarget(uses[0])) continue;
                var directAssign = decl.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign } da }
                    && da.Right == uses[0] && (da.Left is IdentifierExpression || da.Left is MemberReferenceExpression { Target: ThisReferenceExpression or IdentifierExpression });
                var directIf = decl.NextSibling is IfElseStatement ifs
                    && (ifs.Condition == uses[0] || ifs.Condition is UnaryOperatorExpression { Operator: UnaryOperatorType.Not } u && u.Expression == uses[0]);
                if (!directAssign && !directIf && !IsPure(v.Initializer)) continue;

                var names = Names(v.Initializer);
                Statement? cursor = decl.NextSibling as Statement;
                while (cursor is not null && !uses[0].Ancestors.Contains(cursor))
                {
                    if (!IsPureDecl(cursor) || Writes(cursor, names)) { cursor = null; break; }
                    cursor = cursor.NextSibling as Statement;
                }
                if (cursor is null || WritesBefore(ScopeOfUse(cursor, uses[0]), uses[0], names)) continue;
                var replacement = v.Initializer.Detach();
                if (replacement is BinaryOperatorExpression && uses[0].Parent is BinaryOperatorExpression or UnaryOperatorExpression)
                    replacement = new ParenthesizedExpression(replacement);
                uses[0].ReplaceWith(replacement);
                decl.Remove();
                changed = true;
            }
        } while (changed);
    }

    private static bool IsPureDecl(Statement s) =>
        s is VariableDeclarationStatement { Variables.Count: 1 } d && !d.Variables.Single().Initializer.IsNull && IsPure(d.Variables.Single().Initializer);

    private static AstNode ScopeOfUse(Statement s, AstNode use) =>
        s is IfElseStatement ifs && use.Ancestors.Contains(ifs.Condition) ? ifs.Condition : s;

    private static bool IsPure(Expression e) => e switch
    {
        PrimitiveExpression or NullReferenceExpression or ThisReferenceExpression or IdentifierExpression or TypeReferenceExpression => true,
        MemberReferenceExpression m => IsPure(m.Target),
        ParenthesizedExpression p => IsPure(p.Expression),
        UnaryOperatorExpression { Operator: UnaryOperatorType.Not or UnaryOperatorType.Minus or UnaryOperatorType.Plus } u => IsPure(u.Expression),
        BinaryOperatorExpression b => IsPure(b.Left) && IsPure(b.Right),
        CastExpression c => IsPure(c.Expression),
        _ => false,
    };

    private static HashSet<string> Names(Expression e) =>
        e.DescendantsAndSelf.Select(n => n switch { IdentifierExpression i => i.Identifier, MemberReferenceExpression m => m.MemberName, _ => null })
            .OfType<string>().ToHashSet();

    private static bool Writes(AstNode node, HashSet<string> names) =>
        node.DescendantsAndSelf.OfType<AssignmentExpression>().Any(a => Names(a.Left).Overlaps(names))
        || node.DescendantsAndSelf.OfType<UnaryOperatorExpression>().Any(u => u.Operator is UnaryOperatorType.Increment or UnaryOperatorType.Decrement
               or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement && Names(u.Expression).Overlaps(names))
        || node.DescendantsAndSelf.OfType<DirectionExpression>().Any();

    private static bool WritesBefore(AstNode scope, AstNode use, HashSet<string> names)
    {
        foreach (var n in scope.DescendantsAndSelf)
        {
            if (n == use) break;
            if (n is AssignmentExpression a && Names(a.Left).Overlaps(names)) return true;
            if (n is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                    or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement } u && Names(u.Expression).Overlaps(names)) return true;
            if (n is DirectionExpression) return true;
        }
        return false;
    }

    private static bool IsWriteTarget(IdentifierExpression id) =>
        id.Parent is AssignmentExpression a && a.Left == id
        || id.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                                                   or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement }
        || id.Parent is DirectionExpression;
}
