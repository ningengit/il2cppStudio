using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Undoes getter inlining: a read of a private/internal field of another class (<c>button.m_OnClick</c>,
/// <c>Vector3.forwardVector</c>, <c>text.m_textInfo</c>) becomes the public property that exposes it
/// (<c>button.onClick</c>, <c>Vector3.forward</c>, <c>text.textInfo</c>). The property must be on the same type, have
/// a getter, the same type, and a name matching the field once <c>m_</c>/<c>s_</c>/<c>_</c> prefixes and a
/// <c>Vector</c>/<c>Field</c> suffix are ignored. Writes are left alone (a setter may have side effects).
/// </summary>
public static class InlinedGetterRestorer
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var access in tree.Descendants.OfType<MemberReferenceExpression>().ToList())
        {
            if (access.Annotation<MemberResolveResult>()?.Member is not IField { Accessibility: not Accessibility.Public } field) continue;
            if (IsWrite(access) || SameType(access, field.DeclaringTypeDefinition)) continue;
            var key = Normalize(field.Name);
            var property = field.DeclaringTypeDefinition?.Properties.FirstOrDefault(p =>
                p.CanGet && p.Getter!.Accessibility == Accessibility.Public && p.IsStatic == field.IsStatic
                && Normalize(p.Name) == key && p.ReturnType.Equals(field.ReturnType));
            if (property is null) continue;
            access.MemberName = property.Name;
            access.RemoveAnnotations<MemberResolveResult>();
        }
        foreach (var inv in tree.Descendants.OfType<InvocationExpression>().ToList())
        {
            if (inv.Target is not MemberReferenceExpression mref) continue;
            if ((inv.Annotation<InvocationResolveResult>()?.Member ?? mref.Annotation<MemberResolveResult>()?.Member) is not IMethod { Accessibility: not Accessibility.Public } method) continue;
            if (!method.Name.EndsWith("Int", StringComparison.Ordinal) || SameType(inv, method.DeclaringTypeDefinition)) continue;
            var pubName = method.Name[..^3];
            var pub = method.DeclaringTypeDefinition?.Methods.FirstOrDefault(m =>
                m.Accessibility == Accessibility.Public && m.IsStatic == method.IsStatic && m.Name == pubName
                && m.Parameters.Count == method.Parameters.Count && m.ReturnType.Equals(method.ReturnType));
            if (pub is null) continue;
            mref.MemberName = pub.Name;
            inv.RemoveAnnotations<InvocationResolveResult>();
            mref.RemoveAnnotations<MemberResolveResult>();
        }
    }

    private static string Normalize(string name)
    {
        var n = name;
        foreach (var prefix in new[] { "m_", "s_", "k_", "_" })
            if (n.StartsWith(prefix, StringComparison.Ordinal)) { n = n[prefix.Length..]; break; }
        foreach (var suffix in new[] { "Vector", "Field" })
            if (n.Length > suffix.Length && n.EndsWith(suffix, StringComparison.Ordinal)) { n = n[..^suffix.Length]; break; }
        return n.ToLowerInvariant();
    }

    private static bool IsWrite(MemberReferenceExpression e) =>
        e.Parent is AssignmentExpression a && a.Left == e
        || e.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement or UnaryOperatorType.AddressOf }
        || e.Parent is DirectionExpression
        || e.Parent is MemberReferenceExpression { Parent: AssignmentExpression } outer && ((AssignmentExpression)outer.Parent).Left == outer;   // struct field write through it

    private static bool SameType(AstNode node, ITypeDefinition? declaring)
    {
        if (declaring is null) return true;
        for (var t = node.GetParent<TypeDeclaration>(); t is not null; t = t.GetParent<TypeDeclaration>())
            if (t.Name == declaring.Name) return true;
        return false;
    }
}
