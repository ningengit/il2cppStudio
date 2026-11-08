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
    public static void Apply(SyntaxTree tree)
    {
        foreach (var sqrt in tree.Descendants.OfType<CastExpression>().ToList())
        {
            if (sqrt.Parent is null || sqrt.Type is not PrimitiveType { Keyword: "float" } || Unparen(sqrt.Expression) is not InvocationExpression
                { Target: MemberReferenceExpression { MemberName: "Sqrt" }, Arguments.Count: 1 } call) continue;
            var body = sqrt.GetParent<BlockStatement>();
            while (body?.GetParent<BlockStatement>() is { } outer) body = outer;
            if (body is null) continue;
            var helpers = new List<Statement>();
            List<string>? locals;
            if (call.Arguments.Single() is IdentifierExpression dRef)
            {
                if (Decl(body, dRef.Identifier) is not { } dDecl || Uses(body, dRef.Identifier) != 2) continue;
                locals = Squares(dDecl.Variables.Single().Initializer);
                helpers.Add(dDecl);
            }
            else locals = Squares(call.Arguments.Single());
            if (locals is not { Count: 3 }) continue;

            Expression? a = null, b = null;
            var components = new HashSet<string>();
            var ok = true;
            foreach (var l in locals)
            {
                var sub = body.Descendants.OfType<ExpressionStatement>().FirstOrDefault(s => s.Expression is AssignmentExpression
                    { Operator: AssignmentOperatorType.Subtract, Left: IdentifierExpression id } && id.Identifier == l);
                if (sub is not null)
                {
                    var (init, initValue) = Init(sub, l);
                    if (initValue is not MemberReferenceExpression ma || sub.Expression is not AssignmentExpression { Right: MemberReferenceExpression mb }
                        || ma.MemberName != mb.MemberName || !components.Add(ma.MemberName)
                        || UsesAfter(body, init!, l) != 1 + 2) { ok = false; break; }
                    if (a is null) { a = ma.Target; b = mb.Target; }
                    else if (!a.IsMatch(ma.Target) || !b!.IsMatch(mb.Target)) { ok = false; break; }
                    helpers.Add(init!); helpers.Add(sub);
                }
                else if (Decl(body, l) is { } diffDecl && diffDecl.Variables.Single().Initializer is BinaryOperatorExpression
                    { Operator: BinaryOperatorType.Subtract, Left: MemberReferenceExpression ma2, Right: MemberReferenceExpression mb2 }
                    && ma2.MemberName == mb2.MemberName && components.Add(ma2.MemberName) && Uses(body, l) == 1 + 2)
                {
                    if (a is null) { a = ma2.Target; b = mb2.Target; }
                    else if (!a.IsMatch(ma2.Target) || !b!.IsMatch(mb2.Target)) { ok = false; break; }
                    helpers.Add(diffDecl);
                }
                else { ok = false; break; }
            }
            if (!ok || a is null || !components.SetEquals(["x", "y", "z"])) continue;

            sqrt.ReplaceWith(new InvocationExpression(new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Vector3")), "Distance"), a.Clone(), b!.Clone()));
            foreach (var h in helpers) h.Remove();
        }
        FoldVectorEquality(tree);
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

    /// <summary>The statement giving <paramref name="l"/> its value right before the <c>-=</c> (declaration or plain assignment, same block).</summary>
    private static (Statement? Stmt, Expression? Value) Init(Statement? sub, string l)
    {
        for (var s = sub?.PrevSibling as Statement; s is not null; s = s.PrevSibling as Statement)
        {
            if (s is VariableDeclarationStatement { Variables.Count: 1 } d && d.Variables.Single().Name == l) return (s, d.Variables.Single().Initializer);
            if (s is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id } a } && id.Identifier == l)
                return (s, a.Right);
            if (s.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == l)) return (null, null);
        }
        return (null, null);
    }

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
