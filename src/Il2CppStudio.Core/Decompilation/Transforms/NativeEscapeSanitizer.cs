using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Eliminates remaining native escape constructs (<c>NativeIntrinsics.Asm</c>, <c>NativeIntrinsics.Stmt</c>,
/// <c>out *(T*)...</c> stack-slot dereferences) so every generated method compiles cleanly in a safe C# context.
/// </summary>
internal static class NativeEscapeSanitizer
{
    public static void Apply(SyntaxTree tree) => Apply(tree, AstWalk.ScanFlags(tree));

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        if (!flags.HasNativeIntrinsics && !flags.HasPointerSyntax) return;
        var slot = 0;
        if (flags.HasPointerSyntax)
        {
            ReplacePointerDirections(tree, ref slot);
            SanitizePointerExpressions(tree, ref slot);
        }
        if (flags.HasNativeIntrinsics)
        {
            RemoveAsmStatements(tree);
            ReplaceInlineAsmCalls(tree);
        }
    }

    private static void ReplacePointerDirections(SyntaxTree tree, ref int slot)
    {
        foreach (var dir in tree.Descendants.OfType<DirectionExpression>().ToList())
        {
            if (Unwrap(dir.Expression) is not UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference } deref) continue;
            if (Unwrap(deref.Expression) is not CastExpression { Type: ComposedType { PointerRank: > 0 } ptrType }) continue;
            if (dir.Ancestors.OfType<Statement>().FirstOrDefault(s => s.Parent is BlockStatement) is not { } insertBefore) continue;

            AstType slotType = StripOnePointer(ptrType);
            var localName = $"_outSlot{++slot}";
            var decl = new VariableDeclarationStatement(slotType, localName, new DefaultValueExpression(slotType.Clone()));
            var methodBody = dir.Ancestors.OfType<MethodDeclaration>().FirstOrDefault()?.Body;
            if (methodBody is not null && !methodBody.IsNull && methodBody.Statements.FirstOrDefault() is { } firstStmt)
                methodBody.InsertChildBefore(firstStmt, decl, BlockStatement.StatementRole);
            else
                insertBefore.Parent!.InsertChildBefore(insertBefore, decl, BlockStatement.StatementRole);
            dir.Expression = new IdentifierExpression(localName);
        }
    }

    private static AstType StripOnePointer(ComposedType ptrType)
    {
        if (ptrType.PointerRank > 1 || ptrType.HasNullableSpecifier || ptrType.ArraySpecifiers.Any())
        {
            var ct = new ComposedType
            {
                BaseType = ptrType.BaseType.Clone(),
                HasNullableSpecifier = ptrType.HasNullableSpecifier,
                PointerRank = Math.Max(0, ptrType.PointerRank - 1),
            };
            foreach (var spec in ptrType.ArraySpecifiers) ct.ArraySpecifiers.Add((ArraySpecifier)spec.Clone());
            return ct;
        }
        return ptrType.BaseType.Clone();
    }

    private static void SanitizePointerExpressions(SyntaxTree tree, ref int slot)
    {
        foreach (var p in tree.Descendants.OfType<PointerReferenceExpression>().ToList())
        {
            if (p.Parent is null || Unwrap(p.Target) is not CastExpression { Type: ComposedType { PointerRank: > 0 } ct } cast) continue;
            if (!IsAsmCall(Unwrap(cast.Expression), out var arg)) continue;
            if (arg is not null && arg.Contains("ThisStructExpr", StringComparison.Ordinal))
            {
                var rep = new MemberReferenceExpression(new ThisReferenceExpression(), p.MemberName);
                foreach (var ta in p.TypeArguments.ToList()) rep.TypeArguments.Add(ta.Detach());
                p.ReplaceWith(rep);
                continue;
            }
            if (p.Parent is AssignmentExpression a && a.Left == p && p.Ancestors.OfType<Statement>().FirstOrDefault(s => s.Parent is BlockStatement) is { } stmt)
            {
                var baseType = StripOnePointer(ct);
                var localName = $"_ptrSlot{++slot}";
                stmt.Parent!.InsertChildBefore(stmt, new VariableDeclarationStatement(baseType, localName, new DefaultValueExpression(baseType.Clone())), BlockStatement.StatementRole);
                var rep = new MemberReferenceExpression(new IdentifierExpression(localName), p.MemberName);
                foreach (var ta in p.TypeArguments.ToList()) rep.TypeArguments.Add(ta.Detach());
                p.ReplaceWith(rep);
                continue;
            }
            var rep2 = new MemberReferenceExpression(new DefaultValueExpression(StripOnePointer(ct)), p.MemberName);
            foreach (var ta in p.TypeArguments.ToList()) rep2.TypeArguments.Add(ta.Detach());
            p.ReplaceWith(rep2);
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
            u.ReplaceWith(new DefaultValueExpression(StripOnePointer(ct)));
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
        if (!IsNativeIntrinsicsTarget(mre.Target)) return false;
        argText = inv.Arguments.FirstOrDefault() is PrimitiveExpression p ? p.Value?.ToString() : inv.Arguments.FirstOrDefault()?.ToString();
        return true;
    }

    private static bool IsNativeIntrinsicsTarget(Expression t) => t switch
    {
        IdentifierExpression { Identifier: "NativeIntrinsics" } => true,
        TypeReferenceExpression { Type: SimpleType { Identifier: "NativeIntrinsics" } } => true,
        TypeReferenceExpression { Type: MemberType { MemberName: "NativeIntrinsics" } } => true,
        MemberReferenceExpression { MemberName: "NativeIntrinsics" } => true,
        _ => false,
    };

    private static Expression Unwrap(Expression e) => e is ParenthesizedExpression p ? Unwrap(p.Expression) : e;
}
