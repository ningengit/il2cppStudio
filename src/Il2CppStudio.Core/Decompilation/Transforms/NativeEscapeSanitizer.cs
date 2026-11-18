using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Eliminates remaining native escape constructs (<c>NativeIntrinsics.Asm</c>, <c>NativeIntrinsics.Stmt</c>,
/// <c>out *(T*)...</c> stack-slot dereferences) so every generated method compiles cleanly in a safe C# context.
/// </summary>
internal static class NativeEscapeSanitizer
{
    public static void Apply(SyntaxTree tree)
    {
        var slot = 0;
        ReplacePointerDirections(tree, ref slot);
        SanitizePointerExpressions(tree, ref slot);
        RemoveAsmStatements(tree);
        ReplaceInlineAsmCalls(tree);
    }

    private static void ReplacePointerDirections(SyntaxTree tree, ref int slot)
    {
        foreach (var dir in tree.Descendants.OfType<DirectionExpression>().ToList())
        {
            if (Unwrap(dir.Expression) is not UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference } deref) continue;
            if (Unwrap(deref.Expression) is not CastExpression { Type: ComposedType { PointerRank: > 0 } ptrType }) continue;
            if (dir.Ancestors.OfType<Statement>().FirstOrDefault(s => s.Parent is BlockStatement) is not { } insertBefore) continue;

            var baseType = ptrType.BaseType.Clone();
            var localName = $"_outSlot{++slot}";
            var decl = new VariableDeclarationStatement(baseType, localName, new DefaultValueExpression(baseType.Clone()));
            insertBefore.Parent!.InsertChildBefore(insertBefore, decl, BlockStatement.StatementRole);
            dir.Expression = new IdentifierExpression(localName);
        }
    }

    private static void SanitizePointerExpressions(SyntaxTree tree, ref int slot)
    {
        foreach (var p in tree.Descendants.OfType<PointerReferenceExpression>().ToList())
        {
            if (p.Parent is null || Unwrap(p.Target) is not CastExpression { Type: ComposedType { PointerRank: > 0 } ct } cast) continue;
            if (!IsAsmCall(Unwrap(cast.Expression), out var arg)) continue;
            if (arg is not null && arg.Contains("ThisStructExpr", StringComparison.Ordinal))
            {
                p.ReplaceWith(new MemberReferenceExpression(new ThisReferenceExpression(), p.MemberName));
                continue;
            }
            if (p.Parent is AssignmentExpression a && a.Left == p && p.Ancestors.OfType<Statement>().FirstOrDefault(s => s.Parent is BlockStatement) is { } stmt)
            {
                var baseType = ct.BaseType.Clone();
                var localName = $"_ptrSlot{++slot}";
                stmt.Parent!.InsertChildBefore(stmt, new VariableDeclarationStatement(baseType, localName, new DefaultValueExpression(baseType.Clone())), BlockStatement.StatementRole);
                p.ReplaceWith(new MemberReferenceExpression(new IdentifierExpression(localName), p.MemberName));
                continue;
            }
            p.ReplaceWith(new MemberReferenceExpression(new DefaultValueExpression(ct.BaseType.Clone()), p.MemberName));
        }

        foreach (var u in tree.Descendants.OfType<UnaryOperatorExpression>().Where(u => u.Operator == UnaryOperatorType.Dereference).ToList())
        {
            if (u.Parent is null || Unwrap(u.Expression) is not CastExpression { Type: ComposedType { PointerRank: > 0 } ct } cast) continue;
            if (!IsAsmCall(Unwrap(cast.Expression), out _)) continue;
            if (u.Parent is AssignmentExpression a && a.Left == u && a.Parent is ExpressionStatement es)
            {
                if (a.Right is InvocationExpression or ObjectCreateExpression) a.ReplaceWith(a.Right.Detach());
                else es.Remove();
                continue;
            }
            u.ReplaceWith(new DefaultValueExpression(ct.BaseType.Clone()));
        }
    }

    private static void RemoveAsmStatements(SyntaxTree tree)
    {
        foreach (var es in tree.Descendants.OfType<ExpressionStatement>().ToList())
        {
            if (es.Parent is null) continue;
            var inner = es.Expression is AssignmentExpression { Left: IdentifierExpression { Identifier: "_" } } da ? Unwrap(da.Right) : Unwrap(es.Expression);
            if (IsAsmCall(inner, out _)) es.Remove();
        }
    }

    private static void ReplaceInlineAsmCalls(SyntaxTree tree)
    {
        foreach (var inv in tree.Descendants.OfType<InvocationExpression>().ToList())
        {
            if (inv.Parent is null || !IsAsmCall(inv, out _)) continue;
            AstNode anchor = inv;
            while (anchor.Parent is ParenthesizedExpression paren) anchor = paren;
            if (anchor.Parent is CastExpression cast)
            {
                if (cast.Type is not ComposedType { PointerRank: > 0 })
                    cast.ReplaceWith(new DefaultValueExpression(cast.Type.Clone()));
                else
                    inv.ReplaceWith(new PrimitiveExpression(0L));
                continue;
            }
            if (anchor.Parent is InvocationExpression outerCall && outerCall.Target is MemberReferenceExpression { MemberName: "Reinterpret" } mre && mre.TypeArguments.Count == 1)
            {
                outerCall.ReplaceWith(new DefaultValueExpression(mre.TypeArguments.First().Clone()));
                continue;
            }
            inv.ReplaceWith(new PrimitiveExpression(0L));
        }
    }

    internal static bool IsAsmCall(Expression e, out string? argText)
    {
        argText = null;
        if (e is not InvocationExpression { Target: MemberReferenceExpression { MemberName: "Asm" or "Stmt" } mre } inv) return false;
        if (!mre.Target.ToString().EndsWith("NativeIntrinsics", StringComparison.Ordinal)) return false;
        argText = inv.Arguments.FirstOrDefault() is PrimitiveExpression p ? p.Value?.ToString() : inv.Arguments.FirstOrDefault()?.ToString();
        return true;
    }

    private static Expression Unwrap(Expression e) => e is ParenthesizedExpression p ? Unwrap(p.Expression) : e;
}
