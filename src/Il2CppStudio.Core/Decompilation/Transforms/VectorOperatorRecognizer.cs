using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Rebuilds vector arithmetic that IL2CPP/C++ inlined component-wise:
/// <c>new Vector3(a.x * s * 0.1f + b.x, a.y * s * 0.1f + b.y, a.z * s * 0.1f + b.z)</c> → <c>a * s * 0.1f + b</c>.
/// The y/z components must equal the x component with every <c>.x</c> member access on a vector operand replaced
/// by <c>.y</c>/<c>.z</c>, and the stripped expression must be well-typed for Unity's operators (vector ± vector,
/// vector × scalar, scalar × vector, vector / scalar). Single-use vector temps declared right before are inlined
/// (they were evaluated in the same left-to-right order).
/// </summary>
public static class VectorOperatorRecognizer
{
    private static readonly string[][] Lanes = [["x", "y"], ["x", "y", "z"]];

    public static void Apply(SyntaxTree tree)
    {
        foreach (var create in tree.Descendants.OfType<ObjectCreateExpression>().ToList())
        {
            if (create.Type is not SimpleType { Identifier: "Vector3" or "Vector2" } st) continue;
            var lanes = st.Identifier == "Vector3" ? Lanes[1] : Lanes[0];
            var args = create.Arguments.ToList();
            if (args.Count != lanes.Length) continue;
            var template = args[0];
            var ok = true;
            for (var i = 1; i < lanes.Length && ok; i++)
                ok = Swap(template.Clone(), "x", lanes[i]).ToString() == args[i].ToString();
            if (!ok || Strip(template.Clone(), st.Identifier) is not { } vectorExpr || Kind(vectorExpr, st.Identifier) != OperandKind.Vector) continue;
            if (!template.Descendants.OfType<MemberReferenceExpression>().Any(m => m.MemberName == "x" && IsVector(m.Target, st.Identifier))) continue;
            create.ReplaceWith(vectorExpr);
        }
        InlineVectorTemps(tree);
    }

    private enum OperandKind { Scalar, Vector, Invalid }

    private static Expression Swap(Expression e, string from, string to)
    {
        foreach (var m in e.DescendantsAndSelf.OfType<MemberReferenceExpression>().Where(m => m.MemberName == from).ToList()) m.MemberName = to;
        return e;
    }

    /// <summary>Removes <c>.x</c> from vector operands; returns null if a non-vector <c>.x</c> would be stripped.</summary>
    private static Expression? Strip(Expression e, string vectorType)
    {
        foreach (var m in e.DescendantsAndSelf.OfType<MemberReferenceExpression>().Where(m => m.MemberName == "x").ToList())
        {
            if (!IsVector(m.Target, vectorType)) return null;
            var target = m.Target.Detach();
            if (m == e) return target;
            m.ReplaceWith(target);
        }
        return e;
    }

    private static OperandKind Kind(Expression e, string vectorType) => e switch
    {
        ParenthesizedExpression p => Kind(p.Expression, vectorType),
        BinaryOperatorExpression b => (b.Operator, Kind(b.Left, vectorType), Kind(b.Right, vectorType)) switch
        {
            (_, OperandKind.Invalid, _) or (_, _, OperandKind.Invalid) => OperandKind.Invalid,
            (BinaryOperatorType.Add or BinaryOperatorType.Subtract, var l, var r) => l == r ? l : OperandKind.Invalid,
            (BinaryOperatorType.Multiply, OperandKind.Vector, OperandKind.Vector) => OperandKind.Invalid,
            (BinaryOperatorType.Multiply, var l, var r) => l == OperandKind.Vector || r == OperandKind.Vector ? OperandKind.Vector : OperandKind.Scalar,
            (BinaryOperatorType.Divide, var l, OperandKind.Scalar) => l,
            _ => OperandKind.Invalid,
        },
        UnaryOperatorExpression { Operator: UnaryOperatorType.Minus } u => Kind(u.Expression, vectorType),
        _ => IsVector(e, vectorType) ? OperandKind.Vector : OperandKind.Scalar,
    };

    private static bool IsVector(Expression e, string vectorType) =>
        e.Annotation<ResolveResult>()?.Type is { } t && t.FullName == "UnityEngine." + vectorType;

    /// <summary><c>Vector3 a = E;</c> immediately preceding a statement that is its only use → inline <c>E</c>.</summary>
    private static void InlineVectorTemps(SyntaxTree tree)
    {
        foreach (var block in tree.Descendants.OfType<BlockStatement>().ToList())
        {
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var stmt in block.Statements.ToList())
                {
                    if (stmt is not VariableDeclarationStatement { Variables.Count: 1, Type: SimpleType { Identifier: "Vector3" or "Vector2" or "float" } } decl) continue;
                    var v = decl.Variables.Single();
                    if (v.Initializer.IsNull) continue;
                    // Uses must all be in the run of declarations + the first non-declaration statement after it.
                    var next = decl.NextSibling as Statement;
                    while (next is VariableDeclarationStatement) next = next.NextSibling as Statement;
                    if (next is null) continue;
                    var uses = block.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
                    if (uses.Count != 1 || !uses[0].Ancestors.Contains(next) || next is not ExpressionStatement) continue;
                    if (!Ordered(block, decl, next, v.Name)) continue;
                    uses[0].ReplaceWith(v.Initializer.Detach());
                    decl.Remove();
                    changed = true;
                    break;
                }
            }
        }
    }

    /// <summary>Temps between decl and the use statement are inlined in declaration order; the use must come after earlier temps' uses.</summary>
    private static bool Ordered(BlockStatement block, VariableDeclarationStatement decl, Statement use, string name)
    {
        // Every declaration between decl and use must itself be used (later) in `use`, and in source order after name.
        var names = new List<string>();
        for (var s = decl.NextSibling; s != use; s = s!.NextSibling)
            if (s is VariableDeclarationStatement d) names.Add(d.Variables.First().Name);
        var ids = use.Descendants.OfType<IdentifierExpression>().Select(i => i.Identifier).ToList();
        var at = ids.IndexOf(name);
        return at >= 0 && names.All(n => ids.IndexOf(n) > at);
    }
}
