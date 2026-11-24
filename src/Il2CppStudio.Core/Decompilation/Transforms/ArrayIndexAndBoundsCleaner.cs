using System.Numerics;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Two targeted post-decompilation readability cleanups:
/// <list type="number">
/// <item>Folds redundant array-index byte-stride shift/divides such as <c>(x &lt;&lt; 3) / 8</c> and <c>(int)((x &lt;&lt; 3) / 8L)</c>.</item>
/// <item>Removes residual IL2CPP unsigned bounds-check branches that trap into <c>NativeIntrinsics.CallNative("sub_...", ...); throw null;</c>.</item>
/// </list>
/// </summary>
public static class ArrayIndexAndBoundsCleaner
{
    public static void Apply(SyntaxTree tree)
    {
        FoldShiftDivideStride(tree);
        StripBoundsCheckTraps(tree);
    }

    private static void FoldShiftDivideStride(SyntaxTree tree)
    {
        var divides = AstWalk.Collect<BinaryOperatorExpression>(tree, static b => b.Operator == BinaryOperatorType.Divide);
        for (var i = divides.Count - 1; i >= 0; i--)
        {
            var div = divides[i];
            if (div.Parent is null) continue;
            if (!TryGetPowerOfTwoDivisor(UnwrapParens(div.Right), out var shift, out var isLongDivisor))
                continue;
            if (UnwrapParens(div.Left) is not BinaryOperatorExpression { Operator: BinaryOperatorType.ShiftLeft } shl)
                continue;
            if (!TryGetIntLiteral(UnwrapParens(shl.Right), out var actualShift, out _) || actualShift != (ulong)shift)
                continue;

            var x = shl.Left;
            if (!isLongDivisor)
            {
                div.ReplaceWith(x.Detach());
                continue;
            }

            if (FindOuterCast(div) is { } outerCast
                && outerCast.Type is PrimitiveType { Keyword: "int" })
            {
                var unwrapped = UnwrapParens(x);
                if (unwrapped is CastExpression { Type: PrimitiveType { Keyword: "long" } } longCast)
                {
                    var inner = longCast.Expression;
                    if (inner.Annotation<ResolveResult>()?.Type.IsKnownType(KnownTypeCode.Int32) == true)
                        outerCast.ReplaceWith(inner.Detach());
                    else
                        div.ReplaceWith(inner.Detach());
                }
                else if (unwrapped.Annotation<ResolveResult>()?.Type.IsKnownType(KnownTypeCode.Int32) == true)
                {
                    outerCast.ReplaceWith(unwrapped.Detach());
                }
                else
                {
                    div.ReplaceWith(x.Detach());
                }
                continue;
            }

            var exprType = UnwrapParens(x).Annotation<ResolveResult>()?.Type;
            if (UnwrapParens(x) is CastExpression { Type: PrimitiveType { Keyword: "long" or "ulong" } }
                || exprType?.IsKnownType(KnownTypeCode.Int64) == true
                || exprType?.IsKnownType(KnownTypeCode.UInt64) == true)
            {
                div.ReplaceWith(x.Detach());
            }
            else
            {
                div.ReplaceWith(new CastExpression(new PrimitiveType("long"), x.Detach()));
            }
        }
    }

    private static CastExpression? FindOuterCast(Expression expr)
    {
        AstNode cur = expr.Parent;
        while (cur is ParenthesizedExpression) cur = cur.Parent;
        return cur as CastExpression;
    }

    private static bool TryGetPowerOfTwoDivisor(Expression expr, out int shift, out bool isLong)
    {
        shift = 0;
        if (!TryGetIntLiteral(expr, out var val, out isLong)) return false;
        if (val is not (2 or 4 or 8 or 16)) return false;
        shift = BitOperations.TrailingZeroCount(val);
        return true;
    }

    private static bool TryGetIntLiteral(Expression expr, out ulong value, out bool isLong)
    {
        value = 0;
        isLong = false;
        if (expr is not PrimitiveExpression p) return false;
        switch (p.Value)
        {
            case int i when i >= 0: value = (ulong)i; return true;
            case uint u: value = u; return true;
            case long l when l >= 0: value = (ulong)l; isLong = true; return true;
            case ulong ul: value = ul; isLong = true; return true;
            default: return false;
        }
    }

    private static Expression UnwrapParens(Expression expr)
    {
        while (expr is ParenthesizedExpression p) expr = p.Expression;
        return expr;
    }

    // ------------------------------------------------------------------ bounds-check trap stripping

    private sealed record TrapBlockEntry(LabelStatement Label, List<Statement> Statements, bool EndsInGoto);

    private static void StripBoundsCheckTraps(SyntaxTree tree)
    {
        var bodies = AstWalk.Collect<BlockStatement>(tree, static b =>
            b.Parent is MethodDeclaration or ConstructorDeclaration or Accessor or OperatorDeclaration or DestructorDeclaration);

        foreach (var body in bodies)
            CleanMethodBody(body);
    }

    private static void CleanMethodBody(BlockStatement body)
    {
        var traps = CollectTrapLabels(body);

        for (var iter = 0; iter < 64; iter++)
        {
            var changed = false;
            var ifs = AstWalk.Collect<IfElseStatement>(body, static i => i.Parent is BlockStatement);
            for (var k = ifs.Count - 1; k >= 0; k--)
            {
                var node = ifs[k];
                if (node.Parent is not BlockStatement parentBlock) continue;
                if (!IsBoundsCheckCondition(node.Condition, out var trueIsInBounds)) continue;

                if (trueIsInBounds)
                {
                    if (!node.FalseStatement.IsNull && IsTrapBranch(node.FalseStatement, traps))
                    {
                        UnwrapInto(parentBlock, node, node.TrueStatement);
                        changed = true;
                        continue;
                    }

                    if (node.FalseStatement.IsNull && AlwaysTerminates(node.TrueStatement))
                    {
                        if (TryMatchTrapSuffix(node.NextSibling, traps, out var trailingTrapStmts, out var reachesTrapLabel))
                        {
                            UnwrapInto(parentBlock, node, node.TrueStatement);
                            foreach (var ts in trailingTrapStmts)
                                ts.Remove();
                            changed = true;
                        }
                        else if (!reachesTrapLabel && node.NextSibling is null && ReachesTrapViaEnclosing(parentBlock, traps))
                        {
                            UnwrapInto(parentBlock, node, node.TrueStatement);
                            changed = true;
                        }
                    }
                }
                else
                {
                    if (IsTrapBranch(node.TrueStatement, traps))
                    {
                        if (node.FalseStatement.IsNull)
                            node.Remove();
                        else
                            UnwrapInto(parentBlock, node, node.FalseStatement);
                        changed = true;
                    }
                }
            }

            if (!changed) break;
        }

        if (traps.Count == 0) return;
        var referenced = new HashSet<string>(StringComparer.Ordinal);
        AstWalk.Visit<GotoStatement>(body, g => referenced.Add(g.Label));
        foreach (var (name, entry) in traps)
        {
            if (entry.Label.Parent is null || referenced.Contains(name)) continue;
            entry.Label.Remove();
            foreach (var s in entry.Statements)
                if (s.Parent is not null) s.Remove();
        }
    }

    private static Dictionary<string, TrapBlockEntry> CollectTrapLabels(BlockStatement body)
    {
        var result = new Dictionary<string, TrapBlockEntry>(StringComparer.Ordinal);
        foreach (var block in AstWalk.Collect<BlockStatement>(body))
        {
            CollectTrapLabelsInBlock(block, result);
        }
        CollectTrapLabelsInBlock(body, result);
        return result;
    }

    private static void CollectTrapLabelsInBlock(BlockStatement block, Dictionary<string, TrapBlockEntry> result)
    {
        for (var s = block.FirstChild as Statement; s is not null; s = s.NextSibling as Statement)
        {
            if (s is not LabelStatement lbl) continue;
            var stmts = new List<Statement>();
            var cur = lbl.NextSibling as Statement;
            var sawThrowNull = false;
            var valid = true;
            while (cur is not null && cur is not LabelStatement)
            {
                if (sawThrowNull)
                {
                    valid = false;
                    break;
                }
                if (IsCallNativeTrapStmt(cur))
                {
                    stmts.Add(cur);
                    cur = cur.NextSibling as Statement;
                    continue;
                }
                if (cur is ThrowStatement { Expression: NullReferenceExpression })
                {
                    stmts.Add(cur);
                    sawThrowNull = true;
                    cur = cur.NextSibling as Statement;
                    continue;
                }
                valid = false;
                break;
            }
            if (valid && sawThrowNull)
                result[lbl.Label] = new TrapBlockEntry(lbl, stmts, EndsInGoto: false);
        }
    }

    private static bool ReachesTrapViaEnclosing(BlockStatement block, Dictionary<string, TrapBlockEntry> traps)
    {
        AstNode? curBlock = block;
        while (curBlock is BlockStatement cb && cb.Parent is IfElseStatement enclosingIf && enclosingIf.Parent is BlockStatement outerBlock)
        {
            if (enclosingIf.TrueStatement == cb && enclosingIf.FalseStatement.IsNull)
            {
                if (TryMatchTrapSuffix(enclosingIf.NextSibling, traps, out _, out _))
                    return true;
                if (enclosingIf.NextSibling is null)
                {
                    curBlock = outerBlock;
                    continue;
                }
            }
            break;
        }
        return false;
    }

    private static bool TryMatchTrapSuffix(
        AstNode? start,
        Dictionary<string, TrapBlockEntry> traps,
        out List<Statement> stmtsToRemove,
        out bool reachesTrapLabel)
    {
        stmtsToRemove = [];
        reachesTrapLabel = false;
        var cur = start as Statement;
        if (cur is null) return false;

        if (cur is LabelStatement lbl)
        {
            reachesTrapLabel = traps.ContainsKey(lbl.Label);
            return reachesTrapLabel;
        }

        while (cur is not null)
        {
            if (cur is LabelStatement nextLbl)
            {
                if (traps.ContainsKey(nextLbl.Label) && stmtsToRemove.All(IsCallNativeTrapStmt))
                {
                    reachesTrapLabel = true;
                    return true;
                }
                stmtsToRemove.Clear();
                return false;
            }
            if (IsCallNativeTrapStmt(cur))
            {
                stmtsToRemove.Add(cur);
                cur = cur.NextSibling as Statement;
                continue;
            }
            if (cur is ThrowStatement { Expression: NullReferenceExpression }
                || cur is GotoStatement gs && traps.ContainsKey(gs.Label))
            {
                stmtsToRemove.Add(cur);
                return true;
            }
            stmtsToRemove.Clear();
            return false;
        }

        stmtsToRemove.Clear();
        return false;
    }

    private static bool IsTrapBranch(Statement stmt, Dictionary<string, TrapBlockEntry> traps)
    {
        if (stmt is BlockStatement block)
            return IsTrapStatements(block.Statements, traps);
        return IsSingleTrapStatement(stmt, traps);
    }

    private static bool IsSingleTrapStatement(Statement stmt, Dictionary<string, TrapBlockEntry> traps) =>
        stmt is ThrowStatement { Expression: NullReferenceExpression }
        || stmt is GotoStatement gs && traps.ContainsKey(gs.Label);

    private static bool IsTrapStatements(AstNodeCollection<Statement> statements, Dictionary<string, TrapBlockEntry> traps)
    {
        var terminated = false;
        foreach (var s in statements)
        {
            if (terminated) return false;
            if (IsCallNativeTrapStmt(s)) continue;
            if (IsSingleTrapStatement(s, traps)) { terminated = true; continue; }
            return false;
        }
        return terminated;
    }

    private static bool IsCallNativeTrapStmt(Statement s)
    {
        if (s is not ExpressionStatement { Expression: InvocationExpression inv }) return false;
        if (inv.Target is not MemberReferenceExpression { MemberName: "CallNative" } mr) return false;
        if (!IsNativeIntrinsicsTarget(mr.Target)) return false;
        return inv.Arguments.FirstOrDefault() is PrimitiveExpression { Value: string fn }
            && fn.StartsWith("sub_", StringComparison.Ordinal);
    }

    private static bool IsNativeIntrinsicsTarget(Expression target) => target switch
    {
        IdentifierExpression { Identifier: "NativeIntrinsics" } => true,
        MemberReferenceExpression { MemberName: "NativeIntrinsics" } => true,
        TypeReferenceExpression { Type: SimpleType { Identifier: "NativeIntrinsics" } } => true,
        TypeReferenceExpression { Type: MemberType { MemberName: "NativeIntrinsics" } } => true,
        _ => false,
    };

    private static bool AlwaysTerminates(Statement stmt) => stmt switch
    {
        ReturnStatement or ThrowStatement or GotoStatement or BreakStatement or ContinueStatement => true,
        BlockStatement bs => bs.Statements.LastOrDefault() is { } last && AlwaysTerminates(last),
        IfElseStatement ie => !ie.FalseStatement.IsNull && AlwaysTerminates(ie.TrueStatement) && AlwaysTerminates(ie.FalseStatement),
        _ => false,
    };

    private static bool IsBoundsCheckCondition(Expression cond, out bool trueIsInBounds)
    {
        trueIsInBounds = true;
        cond = UnwrapParens(cond);
        if (cond is UnaryOperatorExpression { Operator: UnaryOperatorType.Not } notExpr)
        {
            if (!IsBoundsCheckCondition(notExpr.Expression, out var inner)) return false;
            trueIsInBounds = !inner;
            return true;
        }

        if (cond is not BinaryOperatorExpression bin) return false;
        if (bin.Operator is not (BinaryOperatorType.LessThan or BinaryOperatorType.GreaterThan
            or BinaryOperatorType.LessThanOrEqual or BinaryOperatorType.GreaterThanOrEqual))
            return false;

        var leftLength = IsArrayLengthExpr(bin.Left);
        var rightLength = IsArrayLengthExpr(bin.Right);
        if (leftLength == rightLength) return false;
        if (!IsUnsignedCastOrLiteral(bin.Left) && !IsUnsignedCastOrLiteral(bin.Right))
            return false;

        trueIsInBounds = bin.Operator switch
        {
            BinaryOperatorType.LessThan => rightLength,
            BinaryOperatorType.GreaterThan => leftLength,
            BinaryOperatorType.GreaterThanOrEqual => !rightLength,
            BinaryOperatorType.LessThanOrEqual => !leftLength,
            _ => true,
        };
        return true;
    }

    private static bool IsArrayLengthExpr(Expression expr)
    {
        expr = UnwrapCastAndParens(expr);
        return expr is MemberReferenceExpression { MemberName: "Length" or "LongLength" };
    }

    private static bool IsUnsignedCastOrLiteral(Expression expr)
    {
        expr = UnwrapParens(expr);
        return expr is CastExpression { Type: PrimitiveType { Keyword: "uint" or "ulong" or "nuint" } }
            || expr is PrimitiveExpression { Value: uint or ulong };
    }

    private static Expression UnwrapCastAndParens(Expression expr)
    {
        while (true)
        {
            if (expr is ParenthesizedExpression p) { expr = p.Expression; continue; }
            if (expr is CastExpression c) { expr = c.Expression; continue; }
            return expr;
        }
    }

    private static void UnwrapInto(BlockStatement parentBlock, IfElseStatement node, Statement replacement)
    {
        if (replacement is BlockStatement block)
        {
            if (HasDeclaredVarConflict(parentBlock, block))
            {
                node.ReplaceWith(block.Detach());
                return;
            }
            var anchor = (AstNode)node;
            var toInsert = block.Statements.ToList();
            foreach (var stmt in toInsert)
            {
                stmt.Detach();
                parentBlock.InsertChildAfter(anchor, stmt, BlockStatement.StatementRole);
                anchor = stmt;
            }
            node.Remove();
            return;
        }
        node.ReplaceWith(replacement.Detach());
    }

    private static bool HasDeclaredVarConflict(BlockStatement parentBlock, BlockStatement childBlock)
    {
        var childLocals = new HashSet<string>(StringComparer.Ordinal);
        foreach (var stmt in childBlock.Statements)
            if (stmt is VariableDeclarationStatement vds)
                foreach (var v in vds.Variables)
                    childLocals.Add(v.Name);
        if (childLocals.Count == 0) return false;

        foreach (var stmt in parentBlock.Statements)
        {
            if (stmt is VariableDeclarationStatement vds)
                foreach (var v in vds.Variables)
                    if (childLocals.Contains(v.Name)) return true;
        }
        return false;
    }
}
