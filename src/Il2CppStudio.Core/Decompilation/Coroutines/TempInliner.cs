using ICSharpCode.Decompiler.CSharp.Syntax;
using Il2CppStudio.Core.Decompilation.Transforms;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Inlines single-use temporary locals with side-effect-free initializers across intervening pure declarations
/// into their sole use site (e.g. <c>Vector3 p = player.position; if (Vector3.Distance(p, …) &lt; 10f)</c>).
/// </summary>
public static class TempInliner
{
    public static bool Run(BlockStatement body)
    {
        var everChanged = false;
        bool changed;
        do
        {
            changed = false;
            List<VariableDeclarationStatement>? decls = null;
            Dictionary<string, List<IdentifierExpression>>? usesByName = null;
            AstWalk.ForEachNode(body, n =>
            {
                if (n is VariableDeclarationStatement { Variables.Count: 1 } d && d.Parent is BlockStatement)
                {
                    var v = d.Variables.Single();
                    if (!v.Initializer.IsNull)
                        (decls ??= []).Add(d);
                }
                else if (n is IdentifierExpression id)
                {
                    usesByName ??= new Dictionary<string, List<IdentifierExpression>>(StringComparer.Ordinal);
                    if (!usesByName.TryGetValue(id.Identifier, out var list))
                        usesByName[id.Identifier] = list = [];
                    list.Add(id);
                }
            });
            if (decls is null || usesByName is null) break;

            foreach (var decl in decls)
            {
                if (decl.Parent is not BlockStatement || !AstWalk.IsAttachedTo(decl, body)) continue;
                var v = decl.Variables.Single();
                var attachedUses = usesByName.TryGetValue(v.Name, out var rawUses)
                    ? rawUses.Where(u => AstWalk.IsAttachedTo(u, body)).ToList()
                    : null;
                if (attachedUses is null || attachedUses.Count != 1) continue;
                var use = attachedUses[0];
                if (IsWriteTarget(use)) continue;

                var directAssign = decl.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign } da }
                    && da.Right == use && (da.Left is IdentifierExpression || da.Left is MemberReferenceExpression { Target: ThisReferenceExpression or IdentifierExpression });
                var directIf = decl.NextSibling is IfElseStatement ifs
                    && (ifs.Condition == use || ifs.Condition is UnaryOperatorExpression { Operator: UnaryOperatorType.Not } u && u.Expression == use);
                if (!directAssign && !directIf && !IsPure(v.Initializer)) continue;

                var names = Names(v.Initializer);
                Statement? cursor = decl.NextSibling as Statement;
                while (cursor is not null && !IsAncestor(use, cursor))
                {
                    if (!IsPureDecl(cursor) || Writes(cursor, names)) { cursor = null; break; }
                    cursor = cursor.NextSibling as Statement;
                }
                if (cursor is null || WritesBefore(ScopeOfUse(cursor, use), use, names)) continue;
                var replacement = v.Initializer.Detach();
                if (replacement is BinaryOperatorExpression && use.Parent is BinaryOperatorExpression or UnaryOperatorExpression)
                    replacement = new ParenthesizedExpression(replacement);
                use.ReplaceWith(replacement);
                decl.Remove();
                changed = true;
                everChanged = true;
            }
        } while (changed);
        return everChanged;
    }

    private static bool IsAncestor(AstNode node, AstNode potentialAncestor)
    {
        for (AstNode? a = node; a is not null; a = a.Parent)
            if (a == potentialAncestor) return true;
        return false;
    }

    private static bool IsPureDecl(Statement s) =>
        s is VariableDeclarationStatement { Variables.Count: 1 } d && !d.Variables.Single().Initializer.IsNull && IsPure(d.Variables.Single().Initializer);

    private static AstNode ScopeOfUse(Statement s, AstNode use) =>
        s is IfElseStatement ifs && IsAncestor(use, ifs.Condition) ? ifs.Condition : s;

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

    private static HashSet<string> Names(Expression e)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        AddNames(e, names);
        return names;
    }

    private static void AddNames(AstNode root, HashSet<string> names)
    {
        var stack = new Stack<AstNode>(8);
        stack.Push(root);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is IdentifierExpression i) names.Add(i.Identifier);
            else if (n is MemberReferenceExpression m) names.Add(m.MemberName);
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
    }

    private static bool OverlapsNames(AstNode root, HashSet<string> names)
    {
        if (names.Count == 0) return false;
        var stack = new Stack<AstNode>(8);
        stack.Push(root);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is IdentifierExpression i && names.Contains(i.Identifier)) return true;
            if (n is MemberReferenceExpression m && names.Contains(m.MemberName)) return true;
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
        return false;
    }

    private static bool Writes(AstNode node, HashSet<string> names)
    {
        var stack = new Stack<AstNode>(8);
        stack.Push(node);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is DirectionExpression) return true;
            if (n is AssignmentExpression a && OverlapsNames(a.Left, names)) return true;
            if (n is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                    or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement } u && OverlapsNames(u.Expression, names)) return true;
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
        return false;
    }

    private static bool WritesBefore(AstNode scope, AstNode use, HashSet<string> names)
    {
        var stack = new Stack<AstNode>(16);
        stack.Push(scope);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n == use) break;
            if (n is DirectionExpression) return true;
            if (n is AssignmentExpression a && OverlapsNames(a.Left, names)) return true;
            if (n is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                    or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement } u && OverlapsNames(u.Expression, names)) return true;
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
        return false;
    }

    private static bool IsWriteTarget(IdentifierExpression id) =>
        id.Parent is AssignmentExpression a && a.Left == id
        || id.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                                                   or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement }
        || id.Parent is DirectionExpression;
}
