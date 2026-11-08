using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.CSharp.Syntax.PatternMatching;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Collapses component-wise struct writes that IL2CPP/MSVC produce for Unity math types:
/// <c>Vector3 v = default(Vector3); v.x = A; v.y = B; v.z = C;</c> → <c>Vector3 v = new Vector3(A, B, C);</c> and
/// <c>a.x = b.x; a.y = b.y; a.z = b.z;</c> → <c>a = b;</c>. Requires every component in declaration order, a field-only
/// (getter-free) source, and component values that do not read the target, so evaluation is unchanged.
/// </summary>
public static class VectorComponentCollapse
{
    private static readonly Dictionary<string, string[]> Components = new()
    {
        ["UnityEngine.Vector2"] = ["x", "y"],
        ["UnityEngine.Vector3"] = ["x", "y", "z"],
        ["UnityEngine.Vector4"] = ["x", "y", "z", "w"],
        ["UnityEngine.Quaternion"] = ["x", "y", "z", "w"],
        ["UnityEngine.Color"] = ["r", "g", "b", "a"],
        ["UnityEngine.Matrix4x4"] = ["m00", "m10", "m20", "m30", "m01", "m11", "m21", "m31", "m02", "m12", "m22", "m32", "m03", "m13", "m23", "m33"],
    };

    public static void Apply(SyntaxTree tree)
    {
        foreach (var block in tree.Descendants.OfType<BlockStatement>().ToList())
        {
            var stmts = block.Statements.ToList();
            for (var i = 0; i < stmts.Count; i++)
            {
                if (stmts[i].Parent is null || Writes(stmts[i]) is not { } first || DeclaringType(first.Member) is not { } type
                    || !Components.TryGetValue(type, out var names) || !names.Contains(first.Member.MemberName) || i + names.Length > stmts.Count) continue;
                var group = new List<(MemberReferenceExpression Member, Expression Value)>();
                for (var k = 0; k < names.Length; k++)
                    if (Writes(stmts[i + k]) is { } w && names.Contains(w.Member.MemberName) && w.Member.Target.IsMatch(first.Member.Target)) group.Add(w);
                if (group.Count != names.Length || group.Select(g => g.Member.MemberName).Distinct().Count() != names.Length) continue;
                // Out-of-order writes are only reordered when every value is a constant (no evaluation order to keep).
                var inOrder = group.Select(g => g.Member.MemberName).SequenceEqual(names);
                if (!inOrder && !group.All(g => IsConstant(g.Value))) continue;
                group = [.. group.OrderBy(g => Array.IndexOf(names, g.Member.MemberName))];
                var target = first.Member.Target;
                if (!FieldOnly(target) || group.Select((g, k) => ReadsWritten(g.Value, target, names, k)).Any(x => x)) continue;

                // The component sets cover every instance field of these structs, so a whole-value write is exact.
                var isAllZero = group.All(g => g.Value is PrimitiveExpression { Value: 0f or 0d or 0 });
                var replacement = WholeSource(group, names)
                    ?? (type == "UnityEngine.Matrix4x4" ? (isAllZero ? new DefaultValueExpression(new SimpleType("Matrix4x4")) : null) : CtorFor(type, group.Select(g => g.Value)));
                if (replacement is null) continue;
                if (i > 0 && stmts[i - 1] is VariableDeclarationStatement { Variables.Count: 1 } decl && target is IdentifierExpression id
                    && decl.Variables.Single().Name == id.Identifier && decl.Variables.Single().Initializer is DefaultValueExpression)
                {
                    decl.Variables.Single().Initializer = replacement;
                    for (var k = 0; k < names.Length; k++) stmts[i + k].Remove();
                }
                else
                {
                    stmts[i].ReplaceWith(new ExpressionStatement(new AssignmentExpression(target.Clone(), replacement)));
                    for (var k = 1; k < names.Length; k++) stmts[i + k].Remove();
                }
                i += names.Length - 1;
            }
        }
    }

    private static (MemberReferenceExpression Member, Expression Value)? Writes(Statement s) =>
        s is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: MemberReferenceExpression m } a }
            ? (m, a.Right) : null;

    private static string? DeclaringType(MemberReferenceExpression m) =>
        m.Annotation<MemberResolveResult>()?.Member is IField f ? f.DeclaringType.FullName
        : m.MemberName is "m00" or "m10" or "m20" or "m30" or "m01" or "m11" or "m21" or "m31"
            or "m02" or "m12" or "m22" or "m32" or "m03" or "m13" or "m23" or "m33" ? "UnityEngine.Matrix4x4"
        : null;

    /// <summary>All values are <c>src.c</c> for the same field-only <c>src</c> → <c>src</c>.</summary>
    private static Expression? WholeSource(List<(MemberReferenceExpression Member, Expression Value)> group, string[] names)
    {
        if (group[0].Value is not MemberReferenceExpression { Target: var src } || !FieldOnly(src)) return null;
        for (var k = 0; k < names.Length; k++)
            if (group[k].Value is not MemberReferenceExpression m || m.MemberName != names[k] || !m.Target.IsMatch(src) || DeclaringType(m) != DeclaringType(group[0].Member)) return null;
        return src.Clone();
    }

    private static Expression CtorFor(string type, IEnumerable<Expression> values) =>
        new ObjectCreateExpression(new SimpleType(type[(type.LastIndexOf('.') + 1)..]), values.Select(v => v.Clone()));

    private static bool FieldOnly(Expression e) => e switch
    {
        IdentifierExpression or ThisReferenceExpression or TypeReferenceExpression => true,
        MemberReferenceExpression m => (m.Annotation<MemberResolveResult>()?.Member is IField || m.Annotation<MemberResolveResult>() is null) && FieldOnly(m.Target),
        _ => false,
    };

    private static bool IsConstant(Expression e) =>
        e is PrimitiveExpression || e is UnaryOperatorExpression { Operator: UnaryOperatorType.Minus, Expression: PrimitiveExpression };

    /// <summary>
    /// True when component value <paramref name="k"/> reads the target in a way the one-shot form would change: the
    /// whole target, or a component already overwritten (index &lt; k). Reading not-yet-written components is exact.
    /// </summary>
    private static bool ReadsWritten(Expression value, Expression target, string[] names, int k)
    {
        foreach (var e in value.DescendantsAndSelf.OfType<Expression>().Where(e => e.IsMatch(target)))
        {
            if (e.Parent is MemberReferenceExpression m && m.Target == e && Array.IndexOf(names, m.MemberName) is var j and >= 0 && j >= k) continue;
            return true;
        }
        return false;
    }
}
