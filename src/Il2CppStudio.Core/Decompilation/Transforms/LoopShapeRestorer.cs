using ICSharpCode.Decompiler.CSharp.Resolver;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Loop shapes produced by optimizing compilers, restored to source form:
/// <list type="bullet">
/// <item>Loop rotation: <c>if (G) { int i = a; do { …; i++; } while (C); }</c> where <c>G</c> is <c>C</c> with <c>i := a</c>
/// (also mirrored, <c>n &gt; 1</c> ≡ <c>1 &lt; n</c>) → <c>for (int i = a; C; i++) { … }</c>.</item>
/// <item>Index arithmetic widened by the native code: <c>(int)(i - 1L)</c> with an <c>int</c> <c>i</c> → <c>i - 1</c>.</item>
/// <item>Pure discards of address arithmetic (<c>_ = Reinterpret&lt;long&gt;(arr) + 32L + …;</c>), which have no effect.</item>
/// </list>
/// </summary>
public static class LoopShapeRestorer
{
    public static void Apply(SyntaxTree tree)
    {
        var casts = new List<CastExpression>();
        var discards = new List<ExpressionStatement>();
        var ifs = new List<IfElseStatement>();
        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case CastExpression c when c.Type is PrimitiveType { Keyword: "int" }:
                    casts.Add(c);
                    break;
                case ExpressionStatement { Expression: AssignmentExpression { Left: IdentifierExpression { Identifier: "_" }, Right: BinaryOperatorExpression } } s:
                    discards.Add(s);
                    break;
                case IfElseStatement ie when ie.FalseStatement.IsNull && ie.TrueStatement is BlockStatement { Statements.Count: 2 }:
                    ifs.Add(ie);
                    break;
            }
        });
        foreach (var cast in casts)
        {
            if (cast.Parent is null || Strip(cast.Expression) is not BinaryOperatorExpression
                { Operator: BinaryOperatorType.Add or BinaryOperatorType.Subtract, Left: IdentifierExpression id, Right: PrimitiveExpression { Value: long n } } bin) continue;
            if (n is < int.MinValue or > int.MaxValue || !IsInt32(id)) continue;
            cast.ReplaceWith(new BinaryOperatorExpression(id.Detach(), bin.Operator, new PrimitiveExpression((int)n)));
        }
        foreach (var s in discards)
        {
            if (s.Parent is null || s.Expression is not AssignmentExpression { Right: BinaryOperatorExpression rhs }) continue;
            if (!HasBlockingNode(rhs))
                s.Remove();
        }
        foreach (var ie in ifs) TryRotatedFor(ie);
    }

    private static bool HasBlockingNode(AstNode root) =>
        AstWalk.HasAny(root, static n => n is AssignmentExpression
            or UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement }
            || (n is InvocationExpression inv && !IsReinterpretLongTarget(inv.Target)));

    private static bool IsReinterpretLongTarget(Expression target) =>
        target is MemberReferenceExpression { MemberName: "Reinterpret", TypeArguments.Count: 1 } mr
        && mr.TypeArguments.FirstOrDefault() is PrimitiveType { Keyword: "long" };

    private static void TryRotatedFor(IfElseStatement ifs)
    {
        if (!ifs.FalseStatement.IsNull || ifs.TrueStatement is not BlockStatement block || block.Statements.Count != 2) return;
        if (block.Statements.First() is not VariableDeclarationStatement { Variables.Count: 1 } decl) return;
        if (block.Statements.Last() is not DoWhileStatement loop || loop.EmbeddedStatement is not BlockStatement body) return;
        var v = decl.Variables.Single();
        if (v.Initializer.IsNull || body.Statements.LastOrDefault() is not ExpressionStatement step || !IsStep(step.Expression, v.Name)) return;
        if (AstWalk.HasAny<ContinueStatement>(body)) return;
        if (loop.Condition is not IdentifierExpression { Identifier: var cid } || cid != v.Name)
        {
            if (!AstWalk.HasAny(loop.Condition, n => n is IdentifierExpression i && i.Identifier == v.Name)) return;
        }

        var substituted = loop.Condition.Clone();
        foreach (var id in AstWalk.Collect<IdentifierExpression>(substituted, i => i.Identifier == v.Name))
            id.ReplaceWith(v.Initializer.Clone());
        if (substituted is IdentifierExpression sid && sid.Identifier == v.Name)
            substituted = v.Initializer.Clone();
        if (!Equivalent(ifs.Condition, substituted)) return;

        step.Remove();
        var forStmt = new ForStatement { Condition = loop.Condition.Detach(), EmbeddedStatement = body.Detach() };
        forStmt.Initializers.Add(decl.Detach());
        forStmt.Iterators.Add(step);
        ifs.ReplaceWith(forStmt);
    }

    private static bool IsStep(Expression e, string name) => e switch
    {
        UnaryOperatorExpression { Operator: UnaryOperatorType.PostIncrement or UnaryOperatorType.Increment or UnaryOperatorType.PostDecrement or UnaryOperatorType.Decrement, Expression: IdentifierExpression i } => i.Identifier == name,
        AssignmentExpression { Left: IdentifierExpression l } => l.Identifier == name,
        _ => false,
    };

    /// <summary>Same comparison, possibly mirrored (<c>a &gt; b</c> ≡ <c>b &lt; a</c>), compared as text.</summary>
    private static bool Equivalent(Expression guard, Expression cond)
    {
        var g = Strip(guard); var c = Strip(cond);
        if (g.ToString() == c.ToString()) return true;
        if (g is not BinaryOperatorExpression gb || c is not BinaryOperatorExpression cb) return false;
        var mirror = gb.Operator switch
        {
            BinaryOperatorType.GreaterThan => BinaryOperatorType.LessThan, BinaryOperatorType.LessThan => BinaryOperatorType.GreaterThan,
            BinaryOperatorType.GreaterThanOrEqual => BinaryOperatorType.LessThanOrEqual, BinaryOperatorType.LessThanOrEqual => BinaryOperatorType.GreaterThanOrEqual,
            BinaryOperatorType.Equality => BinaryOperatorType.Equality, BinaryOperatorType.InEquality => BinaryOperatorType.InEquality,
            _ => (BinaryOperatorType?)null,
        };
        return mirror == cb.Operator && Strip(gb.Left).ToString() == Strip(cb.Right).ToString() && Strip(gb.Right).ToString() == Strip(cb.Left).ToString();
    }

    private static Expression Strip(Expression e) => e is ParenthesizedExpression p ? Strip(p.Expression) : e;

    /// <summary>
    /// <c>for (int num = …)</c> → <c>for (int i = …)</c> (then <c>j</c>, <c>k</c> for nested loops) when the generated
    /// name (<c>num</c>, <c>num2</c>…) is only used inside the loop and the new name is free in the method.
    /// </summary>
    public static void NameLoopCounters(SyntaxTree tree)
    {
        AstWalk.Visit<EntityDeclaration>(tree, static method =>
        {
            if (method is not (MethodDeclaration or ConstructorDeclaration)) return;
            var loops = AstWalk.Collect<ForStatement>(method, static l =>
                l.Initializers.FirstOrDefault() is VariableDeclarationStatement { Variables.Count: 1, Type: PrimitiveType { Keyword: "int" } });
            if (loops.Count == 0) return;

            var used = new HashSet<string>(StringComparer.Ordinal);
            var idExprs = new Dictionary<string, List<IdentifierExpression>>(StringComparer.Ordinal);
            AstWalk.ForEachNode(method, n =>
            {
                if (n is Identifier id) used.Add(id.Name);
                else if (n is IdentifierExpression ie)
                {
                    if (!idExprs.TryGetValue(ie.Identifier, out var list))
                        idExprs[ie.Identifier] = list = [];
                    list.Add(ie);
                }
            });

            var pool = new Queue<string>();
            if (!used.Contains("i")) pool.Enqueue("i");
            if (!used.Contains("j")) pool.Enqueue("j");
            if (!used.Contains("k")) pool.Enqueue("k");
            if (pool.Count == 0) return;

            foreach (var loop in loops)
            {
                if (pool.Count == 0) break;
                var d = (VariableDeclarationStatement)loop.Initializers.First();
                var v = d.Variables.Single();
                if (!IsNumName(v.Name)) continue;
                if (!idExprs.TryGetValue(v.Name, out var allUses))
                {
                    v.Name = pool.Dequeue();
                    continue;
                }
                var usedOutside = false;
                foreach (var u in allUses)
                {
                    var underLoop = false;
                    for (AstNode? a = u; a is not null && a != method; a = a.Parent)
                        if (a == loop) { underLoop = true; break; }
                    if (!underLoop) { usedOutside = true; break; }
                }
                if (usedOutside) continue;
                var name = pool.Dequeue();
                foreach (var u in allUses) u.Identifier = name;
                v.Name = name;
            }
        });
    }

    private static bool IsNumName(string s)
    {
        if (!s.StartsWith("num", StringComparison.Ordinal)) return false;
        for (var i = 3; i < s.Length; i++)
            if (!char.IsAsciiDigit(s[i])) return false;
        return true;
    }

    private static bool IsInt32(IdentifierExpression id) =>
        id.Annotation<ResolveResult>()?.Type is { } t && t.IsKnownType(KnownTypeCode.Int32);
}
