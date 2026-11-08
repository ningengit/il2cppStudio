using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Unity call shapes the C++ compiler inlined, restored to their source form:
/// <list type="bullet">
/// <item><c>Quaternion.Internal_FromEulerRad(new Vector3(a * k, b * k, c * k))</c> with <c>k = π/180</c> → <c>Quaternion.Euler(a, b, c)</c>
/// (Quaternion.Euler(x, y, z) is exactly <c>Internal_FromEulerRad(new Vector3(x, y, z) * Mathf.Deg2Rad)</c>).</item>
/// <item>A float temp <c>float z = V.z;</c> (pure static vector property, single later use) is inlined, so
/// <c>new Vector3(V.x, V.y, z)</c> can collapse to <c>V</c>.</item>
/// <item><c>T x = default(T); x = default(T);</c> → the first declaration only.</item>
/// </list>
/// </summary>
public static class UnityInlineRecognizer
{
    public static void Apply(SyntaxTree tree)
    {
        InlinePureComponentTemps(tree);
        RemoveRepeatedDefault(tree);
        foreach (var inv in tree.Descendants.OfType<InvocationExpression>().ToList()) TryEuler(inv);
    }

    private static void TryEuler(InvocationExpression inv)
    {
        if (inv.Target is not MemberReferenceExpression { MemberName: "Internal_FromEulerRad", Target: TypeReferenceExpression or IdentifierExpression { Identifier: "Quaternion" } } t
            || inv.Arguments.Count != 1) return;
        var arg = inv.Arguments.Single();
        VariableDeclarationStatement? tempDecl = null;
        if (arg is IdentifierExpression id && FindSingleUseTemp(inv, id.Identifier) is { } d) { tempDecl = d; arg = d.Variables.Single().Initializer; }
        if (arg is not ObjectCreateExpression { Type: SimpleType { Identifier: "Vector3" } } create || create.Arguments.Count != 3) return;
        var stripped = create.Arguments.Select(StripDeg2Rad).ToList();
        if (stripped.Any(s => s is null)) return;
        var euler = new InvocationExpression(new MemberReferenceExpression(t.Target.Clone(), "Euler"), stripped.Select(s => s!.Clone()));
        inv.ReplaceWith(euler);
        tempDecl?.Remove();
    }

    /// <summary><c>a * ((float)Math.PI / 180f)</c> → <c>a</c>.</summary>
    private static Expression? StripDeg2Rad(Expression e) =>
        e is BinaryOperatorExpression { Operator: BinaryOperatorType.Multiply } m && IsDeg2Rad(m.Right) ? m.Left : null;

    private static bool IsDeg2Rad(Expression e)
    {
        var s = e.ToString().Replace(" ", "");
        return s is "((float)Math.PI/180f)" or "(float)Math.PI/180f" or "Mathf.Deg2Rad" or "0.0174532924f" or "0.017453292f";
    }

    private static VariableDeclarationStatement? FindSingleUseTemp(AstNode use, string name)
    {
        var stmt = use.Ancestors.OfType<Statement>().FirstOrDefault(s => s.Parent is BlockStatement);
        if (stmt?.Parent is not BlockStatement block) return null;
        var decl = block.Statements.OfType<VariableDeclarationStatement>().FirstOrDefault(d => d.Variables.Count == 1 && d.Variables.Single().Name == name);
        if (decl is null || block.Descendants.OfType<IdentifierExpression>().Count(i => i.Identifier == name) != 1) return null;
        return decl;
    }

    private static void InlinePureComponentTemps(SyntaxTree tree)
    {
        foreach (var decl in tree.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (v.Initializer is not MemberReferenceExpression { MemberName: "x" or "y" or "z" or "w", Target: MemberReferenceExpression { Target: TypeReferenceExpression or IdentifierExpression { Identifier: "Vector3" or "Vector2" or "Quaternion" } } inner }
                || inner.MemberName is not ("zero" or "one" or "up" or "down" or "left" or "right" or "forward" or "back" or "identity")
                || decl.Parent is not BlockStatement block) continue;
            var uses = block.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
            if (uses.Count != 1) continue;
            uses[0].ReplaceWith(v.Initializer.Detach());   // constant static property: reordering the read is unobservable
            decl.Remove();
        }
    }

    private static void RemoveRepeatedDefault(SyntaxTree tree)
    {
        foreach (var decl in tree.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (v.Initializer is not DefaultValueExpression) continue;
            if (decl.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression l, Right: DefaultValueExpression } } next
                && l.Identifier == v.Name)
                next.Remove();
        }
    }
}
