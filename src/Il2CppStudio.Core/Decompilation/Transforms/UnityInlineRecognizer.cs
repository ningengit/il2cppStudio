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
        if (flags.HasTruncate)
            CollapseBankersRound(tree);
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

    // ------------------------------------------------------------------ inlined IEEE banker's rounding (Mathf.RoundToInt / Math.Round)

    private static void CollapseBankersRound(SyntaxTree tree)
    {
        var useMathf = HasUnityEngineContext(tree);
        foreach (var entity in AstWalk.Collect<EntityDeclaration>(tree))
        {
            var methodBody = entity switch
            {
                MethodDeclaration m => m.Body,
                ConstructorDeclaration c => c.Body,
                Accessor a => a.Body,
                _ => BlockStatement.Null,
            };
            if (methodBody.IsNull) continue;

            var progress = true;
            while (progress)
            {
                progress = false;
                var blocks = new List<BlockStatement> { methodBody };
                AstWalk.CollectInto(methodBody, blocks);
                foreach (var block in blocks)
                {
                    if (!ReferenceEquals(block, methodBody) && !AstWalk.IsAttachedTo(block, methodBody)) continue;
                    if (TryCollapseBlockBankersRound(methodBody, block, useMathf))
                    {
                        progress = true;
                        break;
                    }
                }
            }
        }
    }

    private static bool HasUnityEngineContext(SyntaxTree tree)
    {
        foreach (var u in tree.Children.OfType<UsingDeclaration>())
            if (u.Namespace == "UnityEngine") return true;
        return AstWalk.HasAny(tree, static n =>
            n is IdentifierExpression { Identifier: "UnityEngine" or "Mathf" }
            || n is SimpleType { Identifier: "UnityEngine" or "Mathf" });
    }

    private static bool TryCollapseBlockBankersRound(BlockStatement methodBody, BlockStatement block, bool useMathf)
    {
        var stmts = block.Statements.ToList();
        for (var i = 0; i + 2 < stmts.Count; i++)
        {
            if (!TryMatchAssignOrDecl(stmts[i], out var truncVar, out var truncRhs)
                || !TryMatchMathCall(truncRhs, "Truncate", out var srcExpr)
                || Unparen(srcExpr) is not IdentifierExpression srcId)
                continue;

            var skippedDecls = new HashSet<string>(StringComparer.Ordinal);
            var idx = i + 1;
            SkipEmptyDecls(stmts, ref idx, skippedDecls);
            if (idx >= stmts.Count
                || !TryMatchAssignOrDecl(stmts[idx], out var diffVar, out var diffRhs)
                || !IsDiffSub(diffRhs, srcId.Identifier, truncVar))
                continue;
            var diffStmt = stmts[idx];
            var diffDeclaredRes = diffStmt is VariableDeclarationStatement;

            idx++;
            SkipEmptyDecls(stmts, ref idx, skippedDecls);
            Statement? copyStmt = null;
            string? copyVar = null;
            var copyRefName = srcId.Identifier;
            if (idx < stmts.Count
                && TryMatchAssignOrDecl(stmts[idx], out var cVar, out var cRhs)
                && Unparen(cRhs) is IdentifierExpression cId && cId.Identifier == srcId.Identifier)
            {
                copyStmt = stmts[idx];
                copyVar = cVar;
                copyRefName = cVar;
                idx++;
                SkipEmptyDecls(stmts, ref idx, skippedDecls);
            }

            if (idx >= stmts.Count || stmts[idx] is not IfElseStatement ifs)
                continue;
            if (!TryMatchBankersBranch(ifs, diffVar, copyRefName, out var resVar, out var tieLabel))
                continue;
            diffDeclaredRes &= resVar == diffVar;

            var tieLabelStmt = AstWalk.Collect<LabelStatement>(methodBody, ls => ls.Label == tieLabel).FirstOrDefault();
            if (tieLabelStmt is null || !TryMatchTieLabelBlock(tieLabelStmt, resVar, out var tieAssignStmt, out var tieGotoStmt, out var joinLabel))
                continue;

            var deadCandidates = new HashSet<string>(skippedDecls, StringComparer.Ordinal) { truncVar, diffVar };
            if (copyVar is not null) deadCandidates.Add(copyVar);
            CollectAssignedLocals(ifs, deadCandidates);
            CollectAssignedLocals(tieAssignStmt, deadCandidates);

            var postIfGoto = ifs.NextSibling as GotoStatement;
            if (postIfGoto is not null && joinLabel is not null && postIfGoto.Label != joinLabel)
                postIfGoto = null;

            // Inline preceding `srcVar = <rhs>;` if srcVar is overwritten or never read again before overwrite.
            var removedDecls = new Dictionary<string, AstType>(StringComparer.Ordinal);
            Expression argExpr = srcId.Clone();
            Statement? prevSrcAssign = null;
            var inLoop = IsLoopBody(block);
            if (i > 0 && TryMatchPureSrcAssign(stmts[i - 1], srcId.Identifier, out var prevRhs)
                && IsVarDeadAfter(stmts[idx], methodBody, srcId.Identifier, joinLabel)
                && (!inLoop || !ContainsIdentifierBefore(block, i - 1, srcId.Identifier)))
            {
                prevSrcAssign = stmts[i - 1];
                argExpr = prevRhs.Clone();
                deadCandidates.Add(srcId.Identifier);
            }
            // One-step unwrap for `int t = Random.Range(...); float src = t;`
            if (Unparen(argExpr) is IdentifierExpression innerSrc
                && prevSrcAssign?.PrevSibling is Statement innerPrev
                && TryMatchPureSrcAssign(innerPrev, innerSrc.Identifier, out var innerRhs)
                && IsVarDeadAfter(stmts[idx], methodBody, innerSrc.Identifier, joinLabel)
                && (!inLoop || !ContainsIdentifierBefore(block, i - 2, innerSrc.Identifier)))
            {
                RecordRemovedDecls(innerPrev, removedDecls);
                innerPrev.Remove();
                argExpr = innerRhs.Clone();
                deadCandidates.Add(innerSrc.Identifier);
            }

            var srcIsDouble = IsDeclaredAsDouble(methodBody, srcId.Identifier)
                || (Unparen(argExpr) is IdentifierExpression argIdCheck && IsDeclaredAsDouble(methodBody, argIdCheck.Identifier));

            var anchorNext = ifs.NextSibling;
            var insertAnchor = stmts[i];

            // Record declarations before removing statements so any surviving reference elsewhere gets hoisted safely.
            RecordRemovedDecls(prevSrcAssign, removedDecls);
            RecordRemovedDecls(insertAnchor, removedDecls);
            RecordRemovedDecls(diffStmt, removedDecls);
            RecordRemovedDecls(copyStmt, removedDecls);
            RecordRemovedDecls(ifs, removedDecls);

            // Remove the inlined rounding block.
            prevSrcAssign?.Remove();
            diffStmt.Remove();
            copyStmt?.Remove();
            ifs.Remove();

            // Remove tieLabel + parity check + trailing goto if no other goto jumps to tieLabel.
            if (!HasGotoTarget(methodBody, tieLabel))
            {
                RecordRemovedDecls(tieAssignStmt, removedDecls);
                tieAssignStmt.Remove();
                tieGotoStmt?.Remove();
                tieLabelStmt.Remove();
            }

            // If postIfGoto falls directly through to joinLabel, drop the goto and check the label.
            var fellThroughJoin = false;
            if (postIfGoto is not null && joinLabel is not null
                && postIfGoto.NextSibling is LabelStatement nextLbl && nextLbl.Label == joinLabel)
            {
                anchorNext = nextLbl.NextSibling as Statement;
                postIfGoto.Remove();
                postIfGoto = null;
                fellThroughJoin = true;
            }
            else if (postIfGoto is null && anchorNext is LabelStatement adjLbl && adjLbl.Label == joinLabel)
            {
                anchorNext = adjLbl.NextSibling as Statement;
                fellThroughJoin = true;
            }

            LabelStatement? joinLabelStmt = joinLabel is not null
                ? AstWalk.Collect<LabelStatement>(methodBody, ls => ls.Label == joinLabel).FirstOrDefault()
                : null;
            if (joinLabelStmt is not null && !HasGotoTarget(methodBody, joinLabel!))
            {
                if (ReferenceEquals(anchorNext, joinLabelStmt))
                    anchorNext = joinLabelStmt.NextSibling as Statement;
                joinLabelStmt.Remove();
                joinLabelStmt = null;
            }

            // Decide whether we can fold directly into the immediately following consumer statement
            // (ignoring any trailing uninitialized temp declarations), or emit `resVar = Mathf.RoundToInt(argExpr);` / `Math.Round(argExpr);`.
            var anchorStmt = anchorNext as Statement;
            while (anchorStmt is not null && IsEmptyDecl(anchorStmt))
                anchorStmt = anchorStmt.NextSibling as Statement;
            var nextNonEmpty = insertAnchor.NextSibling as Statement;
            while (nextNonEmpty is not null && IsEmptyDecl(nextNonEmpty))
                nextNonEmpty = nextNonEmpty.NextSibling as Statement;

            var wantMathfInt = useMathf && !srcIsDouble && HasIntConsumer(block, anchorStmt, joinLabelStmt, resVar);
            Expression roundCall = wantMathfInt
                ? new InvocationExpression(new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Mathf")), "RoundToInt"), argExpr)
                : new InvocationExpression(new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Math")), "Round"), argExpr);

            var canInlineConsumer = wantMathfInt
                && postIfGoto is null
                && nextNonEmpty is not null
                && ReferenceEquals(nextNonEmpty, anchorStmt)
                && IsSoleIntConsumer(nextNonEmpty, resVar, methodBody, block);

            if (canInlineConsumer && FoldOverflowGuardsInNode(anchorStmt!, resVar, roundCall))
            {
                deadCandidates.Add(resVar);
                insertAnchor.Remove();
            }
            else
            {
                var trailingResDecl = false;
                if (!diffDeclaredRes)
                {
                    for (var s = insertAnchor.NextSibling as Statement; s is not null && IsEmptyDecl(s); s = s.NextSibling as Statement)
                    {
                        if (s is VariableDeclarationStatement vd && vd.Variables.Count == 1 && vd.Variables.Single().Name == resVar)
                        {
                            RecordRemovedDecls(vd, removedDecls);
                            vd.Remove();
                            trailingResDecl = true;
                            break;
                        }
                    }
                }
                var declareLocally = (diffDeclaredRes || trailingResDecl) && !HasRefOutside(methodBody, block, resVar);
                if (!declareLocally && (diffDeclaredRes || trailingResDecl))
                    removedDecls.TryAdd(resVar, new PrimitiveType("double"));
                Statement replacement = declareLocally
                    ? new VariableDeclarationStatement(new PrimitiveType("double"), resVar, roundCall)
                    : new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(resVar), roundCall));
                block.Statements.InsertBefore(insertAnchor, replacement);
                insertAnchor.Remove();

                // Fold overflow guards on resVar in the remaining statements of this block and at joinLabel.
                for (var s = replacement.NextSibling as Statement; s is not null; s = s.NextSibling as Statement)
                {
                    FoldOverflowGuardsInNode(s, resVar, null);
                    if (ContainsUnconditionalAssign(s, resVar) || s is ReturnStatement or ThrowStatement) break;
                }
                if (!fellThroughJoin && joinLabelStmt is not null)
                {
                    for (var s = joinLabelStmt.NextSibling as Statement; s is not null && s is not LabelStatement; s = s.NextSibling as Statement)
                    {
                        FoldOverflowGuardsInNode(s, resVar, null);
                        if (ContainsUnconditionalAssign(s, resVar) || s is ReturnStatement or ThrowStatement or GotoStatement) break;
                    }
                }
            }

            PruneDeadLocalsAndLabels(methodBody, deadCandidates);
            EnsureReferencedDeclsPresent(methodBody, removedDecls);
            return true;
        }
        return false;
    }

    private static Expression Unparen(Expression e)
    {
        while (e is ParenthesizedExpression p) e = p.Expression;
        return e;
    }

    private static bool IsEmptyDecl(Statement s) =>
        s is VariableDeclarationStatement vd && vd.Variables.All(v => v.Initializer.IsNull);

    private static void SkipEmptyDecls(List<Statement> stmts, ref int idx, HashSet<string> deadCandidates)
    {
        while (idx < stmts.Count && stmts[idx] is VariableDeclarationStatement vd && vd.Variables.All(v => v.Initializer.IsNull))
        {
            foreach (var v in vd.Variables) deadCandidates.Add(v.Name);
            idx++;
        }
    }

    private static bool IsLoopBody(BlockStatement block) =>
        block.Parent is WhileStatement or DoWhileStatement or ForStatement or ForeachStatement;

    private static bool TryMatchAssignOrDecl(Statement stmt, out string name, out Expression rhs)
    {
        switch (stmt)
        {
            case ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id, Right: { } r } }:
                name = id.Identifier;
                rhs = r;
                return true;
            case VariableDeclarationStatement { Variables.Count: 1 } vd when !vd.Variables.Single().Initializer.IsNull:
                var v = vd.Variables.Single();
                name = v.Name;
                rhs = v.Initializer;
                return true;
            default:
                name = "";
                rhs = null!;
                return false;
        }
    }

    private static bool TryMatchPureSrcAssign(Statement stmt, string expectedVar, out Expression rhs)
    {
        if (TryMatchAssignOrDecl(stmt, out var name, out rhs)
            && name == expectedVar
            && !AstWalk.HasAny(rhs, n => n is IdentifierExpression id && id.Identifier == expectedVar))
            return true;
        rhs = null!;
        return false;
    }

    private static bool TryMatchMathCall(Expression expr, string memberName, out Expression arg)
    {
        if (Unparen(expr) is InvocationExpression { Arguments.Count: 1, Target: MemberReferenceExpression mr } inv
            && mr.MemberName == memberName
            && mr.Target is TypeReferenceExpression or IdentifierExpression { Identifier: "Math" })
        {
            arg = inv.Arguments.Single();
            return true;
        }
        arg = null!;
        return false;
    }

    private static bool IsDiffSub(Expression rhs, string srcVar, string truncVar)
    {
        if (Unparen(rhs) is not BinaryOperatorExpression { Operator: BinaryOperatorType.Subtract } sub) return false;
        var left = StripDoubleCast(Unparen(sub.Left));
        return left is IdentifierExpression lid && lid.Identifier == srcVar
            && Unparen(sub.Right) is IdentifierExpression rid && rid.Identifier == truncVar;
    }

    private static Expression StripDoubleCast(Expression e) =>
        Unparen(e) is CastExpression { Type: PrimitiveType { Keyword: "double" } } ce ? Unparen(ce.Expression) : Unparen(e);

    private static IfElseStatement? UnwrapSingleIf(Statement? stmt) => stmt switch
    {
        IfElseStatement ifs => ifs,
        BlockStatement { Statements.Count: 1 } bs => bs.Statements.Single() as IfElseStatement,
        _ => null,
    };

    private static bool TryMatchBankersBranch(
        IfElseStatement ifs, string diffVar, string copyVar, out string resVar, out string tieLabel)
    {
        resVar = "";
        tieLabel = "";
        var tIf = UnwrapSingleIf(ifs.TrueStatement);
        var fIf = UnwrapSingleIf(ifs.FalseStatement);
        if (tIf is null || fIf is null) return false;

        if (TryMatchHalfBranch(tIf, diffVar, copyVar, isCeiling: true, out var r1, out var l1)
            && TryMatchHalfBranch(fIf, diffVar, copyVar, isCeiling: false, out var r2, out var l2)
            && r1 == r2 && l1 == l2)
        {
            resVar = r1;
            tieLabel = l1;
            return true;
        }
        if (TryMatchHalfBranch(tIf, diffVar, copyVar, isCeiling: false, out r1, out l1)
            && TryMatchHalfBranch(fIf, diffVar, copyVar, isCeiling: true, out r2, out l2)
            && r1 == r2 && l1 == l2)
        {
            resVar = r1;
            tieLabel = l1;
            return true;
        }
        return false;
    }

    private static bool TryMatchHalfBranch(
        IfElseStatement inner, string diffVar, string copyVar, bool isCeiling, out string resVar, out string tieLabel)
    {
        resVar = "";
        tieLabel = "";
        if (Unparen(inner.Condition) is not BinaryOperatorExpression { Operator: BinaryOperatorType.InEquality } ne) return false;
        var hasDiff = Unparen(ne.Left) is IdentifierExpression d && d.Identifier == diffVar;
        if (!hasDiff) return false;
        var expectedHalf = isCeiling ? -0.5 : 0.5;
        if (!TryGetDoubleLiteral(ne.Right, out var halfVal) || !halfVal.Equals(expectedHalf)) return false;

        var roundStmt = inner.TrueStatement switch
        {
            BlockStatement { Statements.Count: 1 } bs => bs.Statements.Single() as ExpressionStatement,
            ExpressionStatement es => es,
            _ => null,
        };
        if (roundStmt?.Expression is not AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression rId, Right: { } callExpr })
            return false;
        if (!TryMatchMathCall(callExpr, isCeiling ? "Ceiling" : "Floor", out var innerArg)) return false;
        if (Unparen(innerArg) is not BinaryOperatorExpression { Operator: BinaryOperatorType.Add } add
            || Unparen(add.Left) is not IdentifierExpression cId || cId.Identifier != copyVar
            || !TryGetDoubleLiteral(add.Right, out var addHalf) || !addHalf.Equals(expectedHalf))
            return false;

        if (inner.FalseStatement is not BlockStatement { Statements.Count: >= 2 } tieBlock) return false;
        if (tieBlock.Statements.LastOrDefault() is not GotoStatement gs) return false;
        resVar = rId.Identifier;
        tieLabel = gs.Label;
        return true;
    }

    private static bool TryMatchTieLabelBlock(
        LabelStatement labelStmt, string resVar, out ExpressionStatement tieAssignStmt, out GotoStatement? tieGotoStmt, out string? joinLabel)
    {
        tieAssignStmt = null!;
        tieGotoStmt = null;
        joinLabel = null;
        if (labelStmt.NextSibling is not ExpressionStatement
            {
                Expression: AssignmentExpression
                {
                    Operator: AssignmentOperatorType.Assign,
                    Left: IdentifierExpression lid,
                    Right: var rawRight
                }
            } es || lid.Identifier != resVar || Unparen(rawRight) is not ConditionalExpression condExpr)
            return false;
        if (!AstWalk.HasAny(condExpr.Condition, static n => n is BinaryOperatorExpression { Operator: BinaryOperatorType.BitwiseAnd } ba && Unparen(ba.Right) is PrimitiveExpression { Value: 1 or 1L or 1u or 1UL }))
            return false;
        tieAssignStmt = es;
        if (es.NextSibling is GotoStatement gs)
        {
            tieGotoStmt = gs;
            joinLabel = gs.Label;
        }
        else if (es.NextSibling is LabelStatement nextLbl)
        {
            joinLabel = nextLbl.Label;
        }
        return true;
    }

    private static void CollectAssignedLocals(AstNode root, HashSet<string> dest)
    {
        AstWalk.Visit<AssignmentExpression>(root, a =>
        {
            if (a.Left is IdentifierExpression id) dest.Add(id.Identifier);
        });
    }

    private static bool HasGotoTarget(BlockStatement methodBody, string label) =>
        AstWalk.HasAny(methodBody, n => n is GotoStatement g && g.Label == label);

    private static bool ContainsIdentifierBefore(BlockStatement block, int beforeExclusive, string name)
    {
        var idx = 0;
        foreach (var s in block.Statements)
        {
            if (idx >= beforeExclusive) break;
            if (AstWalk.HasAny(s, n => n is IdentifierExpression id && id.Identifier == name))
                return true;
            idx++;
        }
        return false;
    }

    private static bool IsVarDeadAfter(Statement afterStmt, BlockStatement methodBody, string varName, string? joinLabel)
    {
        // Walk forward in enclosing block(s) and optional joinLabel; safe iff we hit an unconditional assign/return before any read.
        for (var s = afterStmt.NextSibling as Statement; s is not null; s = s.NextSibling as Statement)
        {
            if (ContainsUnconditionalAssign(s, varName)) return true;
            if (AstWalk.HasAny(s, n => n is IdentifierExpression id && id.Identifier == varName)) return false;
            if (s is ReturnStatement or ThrowStatement) return true;
            if (s is GotoStatement) break;
        }
        if (afterStmt.Parent is BlockStatement parentBlock && !ReferenceEquals(parentBlock, methodBody))
        {
            var outer = parentBlock.Parent as Statement;
            while (outer is BlockStatement) outer = outer.Parent as Statement;
            if (outer is IfElseStatement ifs && outer.Parent is BlockStatement)
                return IsVarDeadAfter(ifs, methodBody, varName, joinLabel);
            return false;
        }
        if (joinLabel is not null)
        {
            var lbl = AstWalk.Collect<LabelStatement>(methodBody, ls => ls.Label == joinLabel).FirstOrDefault();
            if (lbl is not null)
            {
                for (var s = lbl.NextSibling as Statement; s is not null; s = s.NextSibling as Statement)
                {
                    if (ContainsUnconditionalAssign(s, varName)) return true;
                    if (AstWalk.HasAny(s, n => n is IdentifierExpression id && id.Identifier == varName)) return false;
                    if (s is ReturnStatement or ThrowStatement) return true;
                }
            }
        }
        return true;
    }

    private static bool ContainsUnconditionalAssign(Statement s, string varName) =>
        s is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id, Right: var rhs } }
        && id.Identifier == varName
        && !AstWalk.HasAny(rhs, n => n is IdentifierExpression r && r.Identifier == varName);

    private static bool IsDeclaredAsDouble(BlockStatement methodBody, string varName)
    {
        foreach (var vd in AstWalk.Collect<VariableDeclarationStatement>(methodBody))
        {
            if (vd.Type is PrimitiveType { Keyword: "double" }
                && vd.Variables.Any(v => v.Name == varName))
                return true;
        }
        return false;
    }

    private static bool HasIntConsumer(BlockStatement block, Statement? afterStmt, LabelStatement? joinLabelStmt, string resVar)
    {
        for (var s = afterStmt; s is not null; s = s.NextSibling as Statement)
        {
            if (HasIntCastOrGuard(s, resVar)) return true;
            if (ContainsUnconditionalAssign(s, resVar) || s is ReturnStatement or ThrowStatement) break;
        }
        if (joinLabelStmt is not null)
        {
            for (var s = joinLabelStmt.NextSibling as Statement; s is not null && s is not LabelStatement; s = s.NextSibling as Statement)
            {
                if (HasIntCastOrGuard(s, resVar)) return true;
                if (ContainsUnconditionalAssign(s, resVar) || s is ReturnStatement or ThrowStatement or GotoStatement) break;
            }
        }
        return false;
    }

    private static bool HasIntCastOrGuard(AstNode scope, string resVar) =>
        AstWalk.HasAny(scope, n =>
            n is CastExpression { Type: PrimitiveType { Keyword: "int" } } ce && Unparen(ce.Expression) is IdentifierExpression id && id.Identifier == resVar);

    private static bool IsSoleIntConsumer(Statement consumer, string resVar, BlockStatement methodBody, BlockStatement block)
    {
        if (!HasIntCastOrGuard(consumer, resVar)) return false;
        // Require exactly one occurrence of resVar inside consumer (apart from the +Inf guard condition itself).
        var restHasRef = false;
        for (var s = consumer.NextSibling as Statement; s is not null; s = s.NextSibling as Statement)
        {
            if (ContainsUnconditionalAssign(s, resVar)) break;
            if (AstWalk.HasAny(s, n => n is IdentifierExpression id && id.Identifier == resVar)) { restHasRef = true; break; }
            if (s is ReturnStatement or ThrowStatement) break;
        }
        if (restHasRef) return false;
        // Also ensure resVar is not read elsewhere outside this block (unless overwritten at block/other branch).
        if (!ReferenceEquals(block, methodBody))
        {
            var outer = block.Parent as Statement;
            while (outer is not null && outer.Parent is not BlockStatement) outer = outer.Parent as Statement;
            if (outer is IfElseStatement ifs)
                return IsVarDeadAfter(ifs, methodBody, resVar, null);
            return false;
        }
        return true;
    }

    private static bool FoldOverflowGuardsInNode(AstNode scope, string resVar, Expression? inlineIntExpr)
    {
        var folded = false;
        foreach (var cond in AstWalk.Collect<ConditionalExpression>(scope))
        {
            if (!AstWalk.IsAttachedTo(cond, scope)) continue;
            if (!IsResNotPosInf(cond.Condition, resVar)) continue;
            if (!IsMinIntFallback(cond.FalseExpression)) continue;
            var replacement = InlineCastInReplacement(Unparen(cond.TrueExpression), resVar, inlineIntExpr, out var ok);
            if (!ok) continue;
            ReplaceUnwrappingParens(cond, replacement, scope);
            folded = true;
        }
        foreach (var bin in AstWalk.Collect<BinaryOperatorExpression>(scope))
        {
            if (!AstWalk.IsAttachedTo(bin, scope)) continue;
            if (bin.Operator == BinaryOperatorType.ConditionalOr)
            {
                Expression? keep = IsResEqPosInf(bin.Left, resVar) ? bin.Right
                                 : IsResEqPosInf(bin.Right, resVar) ? bin.Left : null;
                if (keep is null) continue;
                var replacement = InlineCastInReplacement(Unparen(keep), resVar, inlineIntExpr, out var ok);
                if (!ok) continue;
                ReplaceUnwrappingParens(bin, replacement, scope);
                folded = true;
            }
            else if (bin.Operator == BinaryOperatorType.ConditionalAnd)
            {
                Expression? keep = IsResNotPosInf(bin.Left, resVar) ? bin.Right
                                 : IsResNotPosInf(bin.Right, resVar) ? bin.Left : null;
                if (keep is null) continue;
                var replacement = InlineCastInReplacement(Unparen(keep), resVar, inlineIntExpr, out var ok);
                if (!ok) continue;
                ReplaceUnwrappingParens(bin, replacement, scope);
                folded = true;
            }
        }
        return folded;
    }

    private static Expression InlineCastInReplacement(Expression expr, string resVar, Expression? inlineIntExpr, out bool ok)
    {
        ok = true;
        if (inlineIntExpr is null) return expr;
        if (expr is CastExpression selfCast && selfCast.Type is PrimitiveType { Keyword: "int" }
            && Unparen(selfCast.Expression) is IdentifierExpression selfId && selfId.Identifier == resVar)
            return inlineIntExpr.Clone();
        var intCasts = AstWalk.Collect<CastExpression>(expr, c =>
            c.Type is PrimitiveType { Keyword: "int" } && Unparen(c.Expression) is IdentifierExpression id && id.Identifier == resVar);
        if (intCasts.Count != 1)
        {
            ok = false;
            return expr;
        }
        ReplaceUnwrappingParens(intCasts[0], inlineIntExpr.Clone(), expr);
        return expr;
    }

    private static void ReplaceUnwrappingParens(AstNode target, Expression replacement, AstNode stopAt)
    {
        AstNode outer = target;
        while (outer.Parent is ParenthesizedExpression paren && !ReferenceEquals(paren, stopAt))
            outer = paren;
        outer.ReplaceWith(Unparen(replacement).Detach());
    }

    private static bool IsResNotPosInf(Expression cond, string resVar) =>
        Unparen(cond) is BinaryOperatorExpression { Operator: BinaryOperatorType.InEquality } ne
        && ((Unparen(ne.Left) is IdentifierExpression l && l.Identifier == resVar && IsPosInfinityExpr(ne.Right))
            || (Unparen(ne.Right) is IdentifierExpression r && r.Identifier == resVar && IsPosInfinityExpr(ne.Left)));

    private static bool IsResEqPosInf(Expression cond, string resVar) =>
        Unparen(cond) is BinaryOperatorExpression { Operator: BinaryOperatorType.Equality } eq
        && ((Unparen(eq.Left) is IdentifierExpression l && l.Identifier == resVar && IsPosInfinityExpr(eq.Right))
            || (Unparen(eq.Right) is IdentifierExpression r && r.Identifier == resVar && IsPosInfinityExpr(eq.Left)));

    private static bool IsPosInfinityExpr(Expression e) =>
        Unparen(e) is MemberReferenceExpression
        {
            MemberName: "PositiveInfinity",
            Target: TypeReferenceExpression { Type: PrimitiveType { Keyword: "double" } or SimpleType { Identifier: "Double" } }
                or IdentifierExpression { Identifier: "double" or "Double" }
        }
        || (Unparen(e) is InvocationExpression { Arguments.Count: 1, Target: MemberReferenceExpression { MemberName: "Reinterpret" } } inv
            && Unparen(inv.Arguments.Single()) is PrimitiveExpression { Value: 9218868437227405312L });

    private static bool IsMinIntFallback(Expression e)
    {
        e = Unparen(e);
        if (e is MemberReferenceExpression { MemberName: "MinValue" }) return true;
        if (TryGetDoubleLiteral(e, out var d) && d < -2e9) return true;
        return false;
    }

    private static bool TryGetDoubleLiteral(Expression e, out double value)
    {
        switch (Unparen(e))
        {
            case PrimitiveExpression { Value: double d }: value = d; return true;
            case PrimitiveExpression { Value: float f }: value = f; return true;
            case UnaryOperatorExpression { Operator: UnaryOperatorType.Minus } u when TryGetDoubleLiteral(u.Expression, out var inner):
                value = -inner;
                return true;
            default:
                value = 0;
                return false;
        }
    }

    private static void PruneDeadLocalsAndLabels(BlockStatement methodBody, HashSet<string> deadCandidates)
    {
        var remainingIds = new HashSet<string>(StringComparer.Ordinal);
        AstWalk.Visit<IdentifierExpression>(methodBody, id => remainingIds.Add(id.Identifier));
        foreach (var d in AstWalk.Collect<VariableDeclarationStatement>(methodBody))
        {
            if (d.Variables.Count != 1) continue;
            var v = d.Variables.Single();
            if (deadCandidates.Contains(v.Name)
                && (v.Initializer.IsNull || v.Initializer is DefaultValueExpression)
                && !remainingIds.Contains(v.Name))
                d.Remove();
        }
        foreach (var lbl in AstWalk.Collect<LabelStatement>(methodBody))
        {
            if (!HasGotoTarget(methodBody, lbl.Label))
                lbl.Remove();
        }
    }

    private static void RecordRemovedDecls(AstNode? root, Dictionary<string, AstType> map)
    {
        if (root is null) return;
        if (root is VariableDeclarationStatement selfVd)
            RecordDeclVars(selfVd, map);
        AstWalk.Visit<VariableDeclarationStatement>(root, vd => RecordDeclVars(vd, map));
    }

    private static void RecordDeclVars(VariableDeclarationStatement vd, Dictionary<string, AstType> map)
    {
        if (vd.Type.IsNull) return;
        foreach (var v in vd.Variables)
            map.TryAdd(v.Name, vd.Type.Clone());
    }

    private static bool HasRefOutside(BlockStatement methodBody, BlockStatement innerBlock, string name)
    {
        var found = false;
        AstWalk.Visit<IdentifierExpression>(methodBody, id =>
        {
            if (!found && id.Identifier == name && !IsDescendantOf(id, innerBlock))
                found = true;
        });
        return found;
    }

    private static bool IsDescendantOf(AstNode node, AstNode ancestor)
    {
        for (AstNode? a = node; a is not null; a = a.Parent)
            if (ReferenceEquals(a, ancestor)) return true;
        return false;
    }

    private static void EnsureReferencedDeclsPresent(BlockStatement methodBody, Dictionary<string, AstType> removedDecls)
    {
        if (removedDecls.Count == 0) return;
        foreach (var (name, type) in removedDecls)
        {
            var hasUncoveredUse = false;
            AstWalk.Visit<IdentifierExpression>(methodBody, id =>
            {
                if (!hasUncoveredUse && id.Identifier == name && !IsIdentifierDeclaredInScope(id, methodBody, name))
                    hasUncoveredUse = true;
            });
            if (!hasUncoveredUse) continue;

            // Remove/demote any nested declaration of `name` before hoisting to methodBody so we never introduce CS0136 shadowing.
            foreach (var vd in AstWalk.Collect<VariableDeclarationStatement>(methodBody))
            {
                if (ReferenceEquals(vd.Parent, methodBody) || vd.Variables.Count != 1) continue;
                var v = vd.Variables.Single();
                if (v.Name != name) continue;
                if (v.Initializer.IsNull || v.Initializer is DefaultValueExpression)
                    vd.Remove();
                else
                    vd.ReplaceWith(new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(name), v.Initializer.Detach())));
            }

            var hasTopDecl = methodBody.Statements.OfType<VariableDeclarationStatement>()
                .Any(vd => vd.Variables.Any(v => v.Name == name));
            if (!hasTopDecl)
            {
                var hoisted = new VariableDeclarationStatement(type.Clone(), name, new DefaultValueExpression(type.Clone()));
                if (methodBody.Statements.FirstOrDefault() is { } first)
                    methodBody.Statements.InsertBefore(first, hoisted);
                else
                    methodBody.Statements.Add(hoisted);
            }
        }
    }

    private static bool IsIdentifierDeclaredInScope(IdentifierExpression id, BlockStatement methodBody, string name)
    {
        AstNode? cur = id;
        while (cur is not null)
        {
            AstNode? stmt = cur;
            while (stmt is not null && stmt.Parent is not BlockStatement)
                stmt = stmt.Parent;
            if (stmt?.Parent is not BlockStatement block) break;

            foreach (var s in block.Statements)
            {
                if (s is VariableDeclarationStatement vd && vd.Variables.Any(v => v.Name == name))
                    return true;
                if (ReferenceEquals(s, stmt)) break;
            }
            if (ReferenceEquals(block, methodBody)) break;
            cur = block.Parent;
        }
        return false;
    }
}
