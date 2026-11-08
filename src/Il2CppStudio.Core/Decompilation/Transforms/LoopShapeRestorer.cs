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
        NarrowIndexCasts(tree);
        RemoveAddressDiscards(tree);
        foreach (var ifs in tree.Descendants.OfType<IfElseStatement>().ToList()) TryRotatedFor(ifs);
    }

    private static void TryRotatedFor(IfElseStatement ifs)
    {
        if (!ifs.FalseStatement.IsNull || ifs.TrueStatement is not BlockStatement block || block.Statements.Count != 2) return;
        if (block.Statements.First() is not VariableDeclarationStatement { Variables.Count: 1 } decl) return;
        if (block.Statements.Last() is not DoWhileStatement loop || loop.EmbeddedStatement is not BlockStatement body) return;
        var v = decl.Variables.Single();
        if (v.Initializer.IsNull || body.Statements.LastOrDefault() is not ExpressionStatement step || !IsStep(step.Expression, v.Name)) return;
        if (body.Descendants.OfType<ContinueStatement>().Any()) return;
        if (!loop.Condition.DescendantsAndSelf.OfType<IdentifierExpression>().Any(i => i.Identifier == v.Name)) return;

        var substituted = loop.Condition.Clone();
        foreach (var id in substituted.DescendantsAndSelf.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList())
            id.ReplaceWith(v.Initializer.Clone());
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
        foreach (var method in tree.Descendants.OfType<EntityDeclaration>().Where(m => m is MethodDeclaration or ConstructorDeclaration))
        {
            var used = method.Descendants.OfType<Identifier>().Select(i => i.Name).ToHashSet(StringComparer.Ordinal);
            var pool = new Queue<string>(new[] { "i", "j", "k" }.Where(n => !used.Contains(n)));
            foreach (var loop in method.Descendants.OfType<ForStatement>().ToList())
            {
                if (pool.Count == 0) break;
                if (loop.Initializers.FirstOrDefault() is not VariableDeclarationStatement { Variables.Count: 1, Type: PrimitiveType { Keyword: "int" } } d) continue;
                var v = d.Variables.Single();
                if (!System.Text.RegularExpressions.Regex.IsMatch(v.Name, @"^num\d*$")) continue;
                if (method.Descendants.OfType<IdentifierExpression>().Any(id => id.Identifier == v.Name && !id.Ancestors.Contains(loop))) continue;
                var name = pool.Dequeue();
                foreach (var id in loop.Descendants.OfType<IdentifierExpression>().Where(id => id.Identifier == v.Name).ToList()) id.Identifier = name;
                v.Name = name;
            }
        }
    }

    private static void NarrowIndexCasts(SyntaxTree tree)
    {
        foreach (var cast in tree.Descendants.OfType<CastExpression>().ToList())
        {
            if (cast.Type is not PrimitiveType { Keyword: "int" } || Strip(cast.Expression) is not BinaryOperatorExpression
                { Operator: BinaryOperatorType.Add or BinaryOperatorType.Subtract, Left: IdentifierExpression id, Right: PrimitiveExpression { Value: long n } } bin) continue;
            if (n is < int.MinValue or > int.MaxValue || !IsInt32(id)) continue;
            cast.ReplaceWith(new BinaryOperatorExpression(id.Detach(), bin.Operator, new PrimitiveExpression((int)n)));
        }
    }

    private static bool IsInt32(IdentifierExpression id) =>
        id.Annotation<ResolveResult>()?.Type is { } t && t.IsKnownType(KnownTypeCode.Int32);

    private static void RemoveAddressDiscards(SyntaxTree tree)
    {
        foreach (var s in tree.Descendants.OfType<ExpressionStatement>().ToList())
            if (s.Expression is AssignmentExpression { Left: IdentifierExpression { Identifier: "_" }, Right: BinaryOperatorExpression rhs }
                && !rhs.Descendants.Any(n => n is AssignmentExpression or UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement }
                    || n is InvocationExpression inv && !inv.Target.ToString().EndsWith("Reinterpret<long>", StringComparison.Ordinal)))
                s.Remove();
    }
}
