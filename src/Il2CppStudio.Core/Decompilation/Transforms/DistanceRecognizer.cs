using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.CSharp.Syntax.PatternMatching;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Re-folds an inlined <c>Vector3.Distance(a, b)</c>:
/// <c>float y = a.y; float x = a.x; float z = a.z; y -= b.y; x -= b.x; z -= b.z; double d = y*y + x*x + z*z; … (float)Math.Sqrt(d)</c>
/// → <c>Vector3.Distance(a, b)</c>. Unity computes exactly this (float differences, float sum, sqrt, float cast).
/// Matching requires each component local to be written only by its <c>= a.c</c> / <c>-= b.c</c> pair and used only in
/// the sum; all helper statements are removed afterwards.
/// </summary>
public static class DistanceRecognizer
{
    public static void Apply(SyntaxTree tree) => Apply(tree, AstWalk.ScanFlags(tree));

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        if (!flags.HasSqrt) return;
        foreach (var sqrt in AstWalk.Collect<CastExpression>(tree))
        {
            if (sqrt.Parent is null || sqrt.Type is not PrimitiveType { Keyword: "float" } || Unparen(sqrt.Expression) is not InvocationExpression
                { Target: MemberReferenceExpression { MemberName: "Sqrt" }, Arguments.Count: 1 } call) continue;
            var rawArg = Unparen(call.Arguments.Single());
            var body = FindScopeBlock(sqrt.GetParent<BlockStatement>(), rawArg);
            var sqrtStmt = body is null ? null : StatementInBlock(sqrt, body);
            if (body is null || sqrtStmt is null) continue;
            var helpers = new List<Statement>();
            var postDeclarations = new HashSet<string>(StringComparer.Ordinal);
            List<string>? locals;
            VariableDeclarationStatement? dDecl = null;
            if (rawArg is IdentifierExpression dRef)
            {
                if (Decl(body, dRef.Identifier) is not { } foundDecl || Uses(body, dRef.Identifier) != 2) continue;
                dDecl = foundDecl;
                locals = Squares(dDecl.Variables.Single().Initializer);
                helpers.Add(dDecl);
            }
            else locals = Squares(rawArg);
            if (locals is not { Count: 3 }) continue;

            var sumStmt = (Statement?)dDecl ?? sqrtStmt;
            Expression? a = null, b = null;
            var components = new HashSet<string>();
            var aVecReads = new List<(Statement ReadStmt, MemberReferenceExpression Access)>();
            var bVecReads = new List<(Statement ReadStmt, MemberReferenceExpression Access)>();
            var allDiffDecls = true;
            var ok = true;
            foreach (var l in locals)
            {
                var sub = FindSub(body, sumStmt, l);
                if (sub is not null)
                {
                    allDiffDecls = false;
                    var (init, initValue) = Init(sub, l);
                    if (init is null || initValue is not MemberReferenceExpression ma) { ok = false; break; }

                    MemberReferenceExpression? mb = null;
                    Statement readStmtB = sub;
                    if (sub.Expression is AssignmentExpression { Right: MemberReferenceExpression directMb })
                    {
                        mb = directMb;
                    }
                    else if (sub.Expression is AssignmentExpression { Right: IdentifierExpression rhsId })
                    {
                        var (rhsInit, rhsVal) = Init(sub, rhsId.Identifier);
                        if (rhsInit is not null && rhsVal is MemberReferenceExpression rhsMem && IsScalarLocalValidForRange(body, rhsInit, sub, rhsId.Identifier, out var rhsReused))
                        {
                            mb = rhsMem;
                            readStmtB = rhsInit;
                            helpers.Add(rhsInit);
                            if (rhsReused && rhsInit is VariableDeclarationStatement) postDeclarations.Add(rhsId.Identifier);
                        }
                    }

                    if (mb is null || ma.MemberName != mb.MemberName || !components.Add(ma.MemberName)
                        || !IsComponentLocalValid(body, init, sub, sumStmt, l, out var reusedAfter))
                    {
                        ok = false; break;
                    }
                    aVecReads.Add((init, ma));
                    bVecReads.Add((readStmtB, mb));
                    helpers.Add(init);
                    helpers.Add(sub);
                    if (reusedAfter && init is VariableDeclarationStatement) postDeclarations.Add(l);
                }
                else if (DeclBefore(body, sumStmt, l) is { } diffDecl && diffDecl.Variables.Single().Initializer is BinaryOperatorExpression
                    { Operator: BinaryOperatorType.Subtract, Left: MemberReferenceExpression ma2, Right: MemberReferenceExpression mb2 }
                    && ma2.MemberName == mb2.MemberName && components.Add(ma2.MemberName)
                    && IsComponentLocalValid(body, diffDecl, diffDecl, sumStmt, l, out var reusedDiff))
                {
                    aVecReads.Add((diffDecl, ma2));
                    bVecReads.Add((diffDecl, mb2));
                    helpers.Add(diffDecl);
                    if (reusedDiff && diffDecl is VariableDeclarationStatement) postDeclarations.Add(l);
                }
                else { ok = false; break; }
            }
            if (!ok || !components.SetEquals(["x", "y", "z"])) continue;

            if (!TryResolveVectorSource(body, sqrtStmt, aVecReads, helpers, postDeclarations, out a, out var vecStmtA)
                || !TryResolveVectorSource(body, sqrtStmt, bVecReads, helpers, postDeclarations, out b, out var vecStmtB)
                || a is null || b is null)
                continue;
            if (vecStmtA is not null && ReferenceEquals(vecStmtA, vecStmtB)) continue;
            if (vecStmtA is not null) helpers.Add(vecStmtA);
            if (vecStmtB is not null) helpers.Add(vecStmtB);

            var useMagnitude = (allDiffDecls && sqrt.Ancestors.OfType<WhileStatement>().Any())
                || (sqrt.Ancestors.OfType<MethodDeclaration>().FirstOrDefault() is { } md
                    && md.Descendants.OfType<MemberReferenceExpression>().Any(m => m.MemberName == "magnitude"));
            Expression distExpr = useMagnitude
                ? new MemberReferenceExpression(new ParenthesizedExpression(new BinaryOperatorExpression(a.Clone(), BinaryOperatorType.Subtract, b.Clone())), "magnitude")
                : new InvocationExpression(new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Vector3")), "Distance"), a.Clone(), b.Clone());
            sqrt.ReplaceWith(distExpr);
            var distinctHelpers = helpers.Distinct().ToList();
            var helperSet = new HashSet<Statement>(distinctHelpers);
            foreach (var h in distinctHelpers)
            {
                if (h is VariableDeclarationStatement vds && vds.Variables.Count == 1)
                {
                    var varName = vds.Variables.Single().Name;
                    if (HasReferenceOutside(body, varName, helperSet))
                    {
                        if (!TryPromoteLaterAssignment(body, vds, varName, helperSet))
                            vds.Variables.Single().Initializer = Expression.Null;
                        continue;
                    }
                }
                h.Remove();
            }
        }
        FoldVectorEquality(tree);
    }

    private static BlockStatement? FindScopeBlock(BlockStatement? inner, Expression arg)
    {
        if (inner is null) return null;
        if (arg is IdentifierExpression dRef)
        {
            for (var b = inner; b is not null; b = b.GetParent<BlockStatement>())
                if (Decl(b, dRef.Identifier) is not null) return b;
            return inner;
        }
        if (Squares(arg) is { Count: 3 } sq)
        {
            for (var b = inner; b is not null; b = b.GetParent<BlockStatement>())
                if (b.Statements.Any(s => s is VariableDeclarationStatement d && d.Variables.Count == 1 && d.Variables.Single().Name == sq[0]
                    || s is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Subtract, Left: IdentifierExpression id } } && id.Identifier == sq[0]))
                    return b;
        }
        return inner;
    }

    private static Statement? StatementInBlock(AstNode node, BlockStatement block)
    {
        for (var cur = node; cur is not null; cur = cur.Parent)
            if (cur is Statement st && cur.Parent == block) return st;
        return null;
    }

    private static ExpressionStatement? FindSub(BlockStatement body, Statement sqrtStmt, string local)
    {
        ExpressionStatement? match = null;
        foreach (var s in body.Statements)
        {
            if (s == sqrtStmt) break;
            if (s is ExpressionStatement es && es.Expression is AssignmentExpression
                { Operator: AssignmentOperatorType.Subtract, Left: IdentifierExpression id } && id.Identifier == local)
                match = es;
        }
        return match;
    }

    private static VariableDeclarationStatement? DeclBefore(BlockStatement body, Statement sqrtStmt, string name)
    {
        foreach (var s in body.Statements)
        {
            if (s == sqrtStmt) break;
            if (s is VariableDeclarationStatement { Variables.Count: 1 } d && d.Variables.Single().Name == name) return d;
        }
        return null;
    }

    private static bool IsComponentLocalValid(BlockStatement body, Statement init, Statement sub, Statement sqrtStmt, string name, out bool reusedAfter)
    {
        reusedAfter = false;
        var seenInit = false;
        foreach (var s in body.Statements)
        {
            if (s == init) { seenInit = true; continue; }
            if (!seenInit) continue;
            if (s == sub) continue;
            if (s == sqrtStmt)
            {
                var sqrtNodeUses = sqrtStmt.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == name).ToList();
                var condCount = 0;
                foreach (var u in sqrtNodeUses)
                {
                    var insideChildStmt = false;
                    for (AstNode? a = u; a is not null && a != sqrtStmt; a = a.Parent)
                    {
                        if (sqrtStmt is IfElseStatement ifs2 && (a == ifs2.TrueStatement || a == ifs2.FalseStatement)
                            || a.Role == Roles.EmbeddedStatement && a is Statement) { insideChildStmt = true; break; }
                    }
                    if (!insideChildStmt) condCount++;
                }
                if (condCount != 2) return false;
                if (sqrtStmt is IfElseStatement ifs)
                {
                    if (!IsSafeInBranchAfterSqrt(ifs.TrueStatement, name, ref reusedAfter)) return false;
                    if (!IsSafeInBranchAfterSqrt(ifs.FalseStatement, name, ref reusedAfter)) return false;
                }
                else
                {
                    if (sqrtNodeUses.Count != 2) return false;
                }
                return IsDeadOrReassignedAfter(body, sqrtStmt, name, ref reusedAfter);
            }
            if (s.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == name))
                return false;
        }
        return false;
    }

    private static bool IsScalarLocalValidForRange(BlockStatement body, Statement init, Statement useStmt, string name, out bool reusedAfter)
    {
        reusedAfter = false;
        var seenInit = false;
        foreach (var s in body.Statements)
        {
            if (s == init) { seenInit = true; continue; }
            if (!seenInit) continue;
            if (s == useStmt)
            {
                var useCount = useStmt.DescendantsAndSelf.OfType<IdentifierExpression>().Count(i => i.Identifier == name);
                if (useCount != 1) return false;
                return IsDeadOrReassignedAfter(body, useStmt, name, ref reusedAfter);
            }
            if (s.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == name))
                return false;
        }
        return false;
    }

    private static bool IsDeadOrReassignedAfter(BlockStatement body, Statement afterStmt, string name, ref bool reusedAfter)
    {
        var past = false;
        foreach (var s in body.Statements)
        {
            if (s == afterStmt) { past = true; continue; }
            if (!past) continue;
            if (IsUnconditionalAssign(s, name)) { reusedAfter = true; return true; }
            if (s.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == name)) return false;
        }
        return true;
    }

    private static bool IsSafeInBranchAfterSqrt(Statement branch, string name, ref bool reusedAfter)
    {
        if (branch.IsNull) return true;
        if (branch is BlockStatement block)
        {
            foreach (var s in block.Statements)
            {
                if (IsUnconditionalAssign(s, name)) { reusedAfter = true; return true; }
                if (s.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == name)) return false;
            }
            return true;
        }
        if (IsUnconditionalAssign(branch, name)) { reusedAfter = true; return true; }
        return !branch.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == name);
    }

    private static bool IsUnconditionalAssign(Statement s, string name) =>
        s is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id } a }
        && id.Identifier == name
        && !a.Right.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == name);

    private static bool TryResolveVectorSource(
        BlockStatement body,
        Statement sqrtStmt,
        List<(Statement ReadStmt, MemberReferenceExpression Access)> reads,
        List<Statement> knownHelpers,
        HashSet<string> postDeclarations,
        out Expression? source,
        out Statement? vecStmt)
    {
        source = null;
        vecStmt = null;
        if (reads.Count != 3) return false;

        var firstTarget = reads[0].Access.Target;
        if (firstTarget is not IdentifierExpression id)
        {
            for (var i = 1; i < reads.Count; i++)
                if (!firstTarget.IsMatch(reads[i].Access.Target)) return false;
            source = firstTarget;
            return true;
        }

        Statement? commonDef = null;
        Expression? commonVal = null;
        foreach (var (stmt, access) in reads)
        {
            if (access.Target is not IdentifierExpression ri || ri.Identifier != id.Identifier) return false;
            var (def, val) = Init(stmt, id.Identifier, stopOnAnyRead: false);
            if (def is null || val is null) return false;
            if (commonDef is null) { commonDef = def; commonVal = val; }
            else if (!ReferenceEquals(commonDef, def)) return false;
        }
        if (commonDef is null || commonVal is null) return false;

        // Verify no other statement uses this definition of id.Identifier before the next overwrite or sqrtStmt's condition.
        var pastDef = false;
        foreach (var s in body.Statements)
        {
            if (s == commonDef) { pastDef = true; continue; }
            if (!pastDef) continue;
            if (IsUnconditionalAssign(s, id.Identifier))
            {
                if (commonDef is VariableDeclarationStatement) postDeclarations.Add(id.Identifier);
                break;
            }
            var allowedReads = reads.Count(r => r.ReadStmt == s);
            var totalRefs = s.DescendantsAndSelf.OfType<IdentifierExpression>().Count(i => i.Identifier == id.Identifier);
            if (s == sqrtStmt)
            {
                if (sqrtStmt is IfElseStatement ifs)
                {
                    var branchUnused = false;
                    if (!IsSafeInBranchAfterSqrt(ifs.TrueStatement, id.Identifier, ref branchUnused)
                        || !IsSafeInBranchAfterSqrt(ifs.FalseStatement, id.Identifier, ref branchUnused))
                        return false;
                    if (branchUnused && commonDef is VariableDeclarationStatement) postDeclarations.Add(id.Identifier);
                    totalRefs = s.Descendants.OfType<IdentifierExpression>().Count(i => i.Identifier == id.Identifier && !i.Ancestors.Contains(ifs.TrueStatement) && !i.Ancestors.Contains(ifs.FalseStatement));
                }
                if (totalRefs != 0) return false;
                break;
            }
            if (totalRefs != allowedReads) return false;
        }

        // If the vector temp is still read later in the block (before any overwrite), abort resolving it.
        var afterSqrtReused = false;
        if (!IsDeadOrReassignedAfter(body, sqrtStmt, id.Identifier, ref afterSqrtReused)) return false;
        if (afterSqrtReused && commonDef is VariableDeclarationStatement) postDeclarations.Add(id.Identifier);

        source = commonVal;
        vecStmt = commonDef;
        return true;
    }

    private static bool HasReferenceOutside(BlockStatement body, string varName, HashSet<Statement> removed) =>
        body.Descendants.OfType<IdentifierExpression>().Any(i => i.Identifier == varName && !i.Ancestors.OfType<Statement>().Any(removed.Contains));

    private static bool TryPromoteLaterAssignment(BlockStatement body, VariableDeclarationStatement decl, string varName, HashSet<Statement> removed)
    {
        var remaining = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == varName && !i.Ancestors.OfType<Statement>().Any(removed.Contains)).ToList();
        if (remaining.Count == 0) return false;
        var firstStmt = remaining[0].Ancestors.OfType<Statement>().FirstOrDefault(s => s.Parent is BlockStatement);
        if (firstStmt is not ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression lhs } assign }
            || lhs.Identifier != varName || firstStmt.Parent is not BlockStatement targetBlock)
            return false;
        if (!remaining.All(r => r.Ancestors.Contains(targetBlock))) return false;
        var newDecl = new VariableDeclarationStatement(decl.Type.Clone(), varName, assign.Right.Detach());
        firstStmt.ReplaceWith(newDecl);
        decl.Remove();
        return true;
    }

    private static void FoldVectorEquality(SyntaxTree tree)
    {
        foreach (var cmp in tree.Descendants.OfType<BinaryOperatorExpression>().ToList())
        {
            if (cmp.Parent is null) continue;
            BinaryOperatorType op;
            Expression sum;
            if (IsV3Epsilon(cmp.Left) && cmp.Operator is BinaryOperatorType.GreaterThan or BinaryOperatorType.LessThanOrEqual)
            {
                op = cmp.Operator == BinaryOperatorType.GreaterThan ? BinaryOperatorType.Equality : BinaryOperatorType.InEquality;
                sum = cmp.Right;
            }
            else if (IsV3Epsilon(cmp.Right) && cmp.Operator is BinaryOperatorType.LessThan or BinaryOperatorType.GreaterThanOrEqual)
            {
                op = cmp.Operator == BinaryOperatorType.LessThan ? BinaryOperatorType.Equality : BinaryOperatorType.InEquality;
                sum = cmp.Left;
            }
            else continue;

            var terms = new List<Expression>();
            void SplitAdd(Expression x)
            {
                x = Unparen(x);
                if (x is BinaryOperatorExpression { Operator: BinaryOperatorType.Add } add) { SplitAdd(add.Left); SplitAdd(add.Right); }
                else terms.Add(x);
            }
            SplitAdd(sum);
            if (terms.Count != 3) continue;

            Expression? a = null, b = null;
            var comps = new HashSet<string>();
            var match = true;
            foreach (var t in terms)
            {
                if (t is not BinaryOperatorExpression { Operator: BinaryOperatorType.Multiply, Left: var l, Right: var r }
                    || !Unparen(l).IsMatch(Unparen(r))
                    || Unparen(l) is not BinaryOperatorExpression { Operator: BinaryOperatorType.Subtract, Left: MemberReferenceExpression ma, Right: MemberReferenceExpression mb }
                    || ma.MemberName != mb.MemberName || !comps.Add(ma.MemberName))
                {
                    match = false;
                    break;
                }
                if (a is null) { a = ma.Target; b = mb.Target; }
                else if (!a.IsMatch(ma.Target) || !b!.IsMatch(mb.Target)) { match = false; break; }
            }
            if (!match || a is null || !comps.SetEquals(["x", "y", "z"])) continue;

            var body = cmp.GetParent<BlockStatement>();
            var stmt = cmp.GetParent<Statement>();
            var lhs = a.Clone();
            var rhs = b!.Clone();
            cmp.ReplaceWith(new BinaryOperatorExpression(lhs, op, rhs));
            if (body is not null && stmt?.PrevSibling is VariableDeclarationStatement { Variables.Count: 1 } vds)
            {
                var v = vds.Variables.Single();
                if (lhs is IdentifierExpression id && id.Identifier == v.Name && !v.Initializer.IsNull && Uses(body, v.Name) == 2)
                {
                    lhs.ReplaceWith(v.Initializer.Clone());
                    vds.Remove();
                }
            }
        }
    }

    private static bool IsV3Epsilon(Expression e) =>
        Unparen(e) is PrimitiveExpression { Value: float f } && MathF.Abs(f - 1e-10f) < 1e-12f;

    /// <summary>The statement giving <paramref name="l"/> its value right before <paramref name="sub"/> (declaration or plain assignment, same block).</summary>
    private static (Statement? Stmt, Expression? Value) Init(Statement? sub, string l, bool stopOnAnyRead = true)
    {
        for (var s = sub?.PrevSibling as Statement; s is not null; s = s.PrevSibling as Statement)
        {
            if (s is VariableDeclarationStatement { Variables.Count: 1 } d && d.Variables.Single().Name == l) return (s, d.Variables.Single().Initializer);
            if (s is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id } a } && id.Identifier == l)
                return (s, a.Right);
            if (stopOnAnyRead)
            {
                if (s.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == l)) return (null, null);
            }
            else if (WritesIdentifier(s, l))
            {
                return (null, null);
            }
        }
        return (null, null);
    }

    private static bool WritesIdentifier(Statement s, string l) =>
        s.DescendantsAndSelf.OfType<AssignmentExpression>().Any(a => a.Left.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == l))
        || s.DescendantsAndSelf.OfType<DirectionExpression>().Any(d => d.Expression.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == l))
        || s.DescendantsAndSelf.OfType<UnaryOperatorExpression>().Any(u =>
            u.Operator is UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement
            && u.Expression.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == l));

    /// <summary>References to <paramref name="name"/> in statements after <paramref name="from"/> (document order).</summary>
    private static int UsesAfter(BlockStatement body, Statement from, string name)
    {
        var after = false;
        var count = 0;
        foreach (var n in body.Descendants)
        {
            if (n == from) { after = true; continue; }
            if (after && n is IdentifierExpression i && i.Identifier == name && !i.Ancestors.Contains(from)) count++;
        }
        return count;
    }

    private static Expression Unparen(Expression e) => e is ParenthesizedExpression p ? Unparen(p.Expression) : e;

    private static VariableDeclarationStatement? Decl(BlockStatement body, string name) =>
        body.Descendants.OfType<VariableDeclarationStatement>().FirstOrDefault(d => d.Variables.Count == 1 && d.Variables.Single().Name == name);

    /// <summary>Uses of a name: declarator counts once, each identifier reference once.</summary>
    private static int Uses(BlockStatement body, string name) =>
        body.Descendants.OfType<IdentifierExpression>().Count(i => i.Identifier == name) + (Decl(body, name) is null ? 0 : 1);

    /// <summary>"y*y + x*x + z*z" (optionally cast to double) → [y, x, z].</summary>
    private static List<string>? Squares(Expression e)
    {
        var result = new List<string>();
        void Walk(Expression x)
        {
            x = Unparen(x);
            if (x is CastExpression c) { Walk(c.Expression); return; }
            if (x is BinaryOperatorExpression { Operator: BinaryOperatorType.Add } add) { Walk(add.Left); Walk(add.Right); return; }
            if (x is BinaryOperatorExpression { Operator: BinaryOperatorType.Multiply, Left: IdentifierExpression l, Right: IdentifierExpression r } && l.Identifier == r.Identifier)
                result.Add(l.Identifier);
            else result.Add("\0");
        }
        Walk(e);
        return result.Contains("\0") ? null : result;
    }
}
