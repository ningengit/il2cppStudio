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
    public static void Apply(SyntaxTree tree) => Apply(tree, AstWalk.ScanFlags(tree));

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        if (flags.HasVectorOps)
        {
            InlinePureComponentTemps(tree);
            CollapseIdentityVectorCreate(tree);
        }
        RemoveRepeatedDefault(tree);
        if (flags.HasVectorOps)
        {
            foreach (var inv in AstWalk.Collect<InvocationExpression>(tree, static i =>
                i.Arguments.Count == 1 && i.Target is MemberReferenceExpression { MemberName: "Internal_FromEulerRad" or "Internal_ToEulerRad" or "Internal_MakePositive" }))
                TryQuaternionInternal(inv);
        }
    }

    private static void TryQuaternionInternal(InvocationExpression inv)
    {
        if (inv.Parent is null || inv.Arguments.Count != 1
            || inv.Target is not MemberReferenceExpression { Target: TypeReferenceExpression or IdentifierExpression { Identifier: "Quaternion" } } t)
            return;
        if (t.MemberName == "Internal_MakePositive")
        {
            inv.ReplaceWith(inv.Arguments.Single().Detach());
            return;
        }
        if (t.MemberName == "Internal_ToEulerRad")
        {
            var q = inv.Arguments.Single().Detach();
            var deg2Rad = new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Mathf")), "Deg2Rad");
            inv.ReplaceWith(new BinaryOperatorExpression(
                new MemberReferenceExpression(q, "eulerAngles"),
                BinaryOperatorType.Multiply,
                deg2Rad));
            return;
        }
        TryEuler(inv);
    }

    private static void TryEuler(InvocationExpression inv)
    {
        if (inv.Target is not MemberReferenceExpression { MemberName: "Internal_FromEulerRad", Target: TypeReferenceExpression or IdentifierExpression { Identifier: "Quaternion" } } t
            || inv.Arguments.Count != 1) return;
        var origArg = inv.Arguments.Single();
        var arg = origArg;
        VariableDeclarationStatement? tempDecl = null;
        if (arg is IdentifierExpression id && FindSingleUseTemp(inv, id.Identifier) is { } d) { tempDecl = d; arg = d.Variables.Single().Initializer; }
        Expression? euler = null;
        if (arg is ObjectCreateExpression { Type: SimpleType { Identifier: "Vector3" } } create && create.Arguments.Count == 3)
        {
            var stripped = create.Arguments.Select(StripDeg2Rad).ToList();
            if (!stripped.Any(s => s is null))
                euler = new InvocationExpression(new MemberReferenceExpression(t.Target.Clone(), "Euler"), stripped.Select(s => s!.Clone()));
        }
        else if (StripDeg2Rad(arg) is { } directDeg)
        {
            euler = new InvocationExpression(new MemberReferenceExpression(t.Target.Clone(), "Euler"), directDeg.Clone());
        }
        if (euler is null)
        {
            var rad2Deg = new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Mathf")), "Rad2Deg");
            euler = new InvocationExpression(
                new MemberReferenceExpression(t.Target.Clone(), "Euler"),
                new BinaryOperatorExpression(origArg.Detach(), BinaryOperatorType.Multiply, rad2Deg));
            tempDecl = null;
        }
        if (inv.Parent is ExpressionStatement es
            && es.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: MemberReferenceExpression { MemberName: "rotation" } rotLhs, Right: DefaultValueExpression } } nextRot)
        {
            nextRot.Expression = new AssignmentExpression(rotLhs.Detach(), euler);
            es.Remove();
        }
        else
            inv.ReplaceWith(euler);
        tempDecl?.Remove();
    }

    /// <summary><c>a * ((float)Math.PI / 180f)</c> → <c>a</c>.</summary>
    private static Expression? StripDeg2Rad(Expression e) =>
        e is PrimitiveExpression { Value: 0f } ? e
        : e is BinaryOperatorExpression { Operator: BinaryOperatorType.Multiply } m && IsDeg2Rad(m.Right) ? m.Left : null;

    private static bool IsDeg2Rad(Expression e)
    {
        var s = e.ToString().Replace(" ", "");
        return s is "((float)Math.PI/180f)" or "(float)Math.PI/180f" or "(MathF.PI/180f)" or "MathF.PI/180f" or "Mathf.Deg2Rad" or "0.0174532924f" or "0.017453292f";
    }

    private static VariableDeclarationStatement? FindSingleUseTemp(AstNode use, string name)
    {
        Statement? stmt = null;
        for (AstNode? a = use.Parent; a is not null; a = a.Parent)
            if (a is Statement s && s.Parent is BlockStatement) { stmt = s; break; }
        if (stmt?.Parent is not BlockStatement block) return null;
        var decl = block.Statements.OfType<VariableDeclarationStatement>().FirstOrDefault(d => d.Variables.Count == 1 && d.Variables.Single().Name == name);
        if (decl is null) return null;
        var count = 0;
        AstWalk.Visit<IdentifierExpression>(block, i => { if (i.Identifier == name) count++; });
        return count == 1 ? decl : null;
    }

    private static void InlinePureComponentTemps(SyntaxTree tree)
    {
        var blocks = new Dictionary<BlockStatement, List<(VariableDeclarationStatement Decl, VariableInitializer Var)>>();
        AstWalk.Visit<VariableDeclarationStatement>(tree, decl =>
        {
            if (decl.Variables.Count != 1 || decl.Parent is not BlockStatement block) return;
            var v = decl.Variables.Single();
            if (v.Initializer is MemberReferenceExpression { MemberName: "x" or "y" or "z" or "w", Target: MemberReferenceExpression { Target: TypeReferenceExpression or IdentifierExpression { Identifier: "Vector3" or "Vector2" or "Quaternion" } } inner }
                && inner.MemberName is ("zero" or "one" or "up" or "down" or "left" or "right" or "forward" or "back" or "identity"))
            {
                if (!blocks.TryGetValue(block, out var list))
                    blocks[block] = list = new List<(VariableDeclarationStatement, VariableInitializer)>();
                list.Add((decl, v));
            }
        });
        foreach (var (block, list) in blocks)
        {
            var byName = new Dictionary<string, List<IdentifierExpression>>(StringComparer.Ordinal);
            AstWalk.Visit<IdentifierExpression>(block, id =>
            {
                if (!byName.TryGetValue(id.Identifier, out var uses))
                    byName[id.Identifier] = uses = new List<IdentifierExpression>();
                uses.Add(id);
            });
            foreach (var (decl, v) in list)
            {
                if (!AstWalk.IsAttachedTo(decl, block)) continue;
                var attachedUses = byName.TryGetValue(v.Name, out var rawUses)
                    ? rawUses.Where(u => AstWalk.IsAttachedTo(u, block)).ToList()
                    : null;
                if (attachedUses is null || attachedUses.Count != 1) continue;
                attachedUses[0].ReplaceWith(v.Initializer.Detach());   // constant static property: reordering the read is unobservable
                decl.Remove();
            }
        }
    }

    private static void CollapseIdentityVectorCreate(SyntaxTree tree)
    {
        var creates = AstWalk.Collect<ObjectCreateExpression>(tree, static c => c.Type is SimpleType { Identifier: "Vector3" or "Vector2" });
        foreach (var create in creates)
        {
            if (create.Parent is null || create.Type is not SimpleType { Identifier: "Vector3" or "Vector2" } st) continue;
            var expected = st.Identifier == "Vector3" ? 3 : 2;
            if (create.Arguments.Count != expected) continue;
            var args = create.Arguments.ToList();
            if (args[0] is not MemberReferenceExpression { MemberName: "x" } mx || args[1] is not MemberReferenceExpression { MemberName: "y" } my) continue;
            if (expected == 3 && args[2] is not MemberReferenceExpression { MemberName: "z" }) continue;
            var src = mx.Target.ToString();
            if (my.Target.ToString() != src || (expected == 3 && ((MemberReferenceExpression)args[2]).Target.ToString() != src)) continue;
            if (!IsPureVectorSource(mx.Target)) continue;
            create.ReplaceWith(mx.Target.Detach());
        }
    }

    private static bool IsPureVectorSource(Expression e) => e switch
    {
        IdentifierExpression => true,
        MemberReferenceExpression { Target: TypeReferenceExpression or ThisReferenceExpression or IdentifierExpression { Identifier: "Vector3" or "Vector2" or "Quaternion" } } => true,
        _ => EmptyIfRemover.IsPure(e),
    };

    private static void RemoveRepeatedDefault(SyntaxTree tree)
    {
        foreach (var decl in AstWalk.Collect<VariableDeclarationStatement>(tree, static d => d.Variables.Count == 1 && d.Parent is BlockStatement))
        {
            if (!AstWalk.IsAttachedTo(decl, tree)) continue;
            var v = decl.Variables.Single();
            if (v.Initializer is not DefaultValueExpression) continue;
            if (decl.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression l, Right: DefaultValueExpression } } next
                && l.Identifier == v.Name && AstWalk.IsAttachedTo(next, tree))
                next.Remove();
        }
    }
}
