using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Undoes getter inlining: a read of a private/internal field of another class (<c>button.m_OnClick</c>,
/// <c>Vector3.forwardVector</c>, <c>text.m_textInfo</c>) becomes the public property that exposes it
/// (<c>button.onClick</c>, <c>Vector3.forward</c>, <c>text.textInfo</c>). The property must be on the same type, have
/// a getter, the same type, and a name matching the field once <c>m_</c>/<c>s_</c>/<c>_</c> prefixes and a
/// <c>Vector</c>/<c>Field</c> suffix are ignored. Writes are left alone unless the field is an external auto-property
/// backing field (<c>&lt;Prop&gt;k__BackingField</c>).
/// </summary>
public static class InlinedGetterRestorer
{
    public static void Apply(SyntaxTree tree) => Apply(tree, AstWalk.ScanFlags(tree));

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        if (!flags.HasBodies) return;
        IModule? curModule = null;
        var accesses = new List<MemberReferenceExpression>();
        var invocations = new List<InvocationExpression>();
        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case TypeDeclaration td when curModule is null && td.GetSymbol() is ITypeDefinition tdef:
                    curModule = tdef.ParentModule;
                    break;
                case MemberReferenceExpression mr:
                    accesses.Add(mr);
                    break;
                case InvocationExpression inv:
                    invocations.Add(inv);
                    break;
            }
        });

        foreach (var access in accesses)
        {
            if (access.Annotation<MemberResolveResult>()?.Member is IField { Accessibility: not Accessibility.Public } field)
            {
                var isBacking = field.Name.StartsWith('<') && field.Name.EndsWith(">k__BackingField", StringComparison.Ordinal);
                var isExternal = curModule is not null && field.DeclaringTypeDefinition?.ParentModule != curModule;
                var isWrite = IsWrite(access);
                if (isWrite)
                {
                    if (!(isBacking && isExternal && access.Parent is AssignmentExpression a && a.Left == access)) continue;
                    var wKey = Normalize(field.Name);
                    var wProp = field.DeclaringTypeDefinition?.Properties.FirstOrDefault(p =>
                        p.CanSet && p.IsStatic == field.IsStatic
                        && Normalize(p.Name) == wKey && p.ReturnType.Equals(field.ReturnType));
                    if (wProp is null) continue;
                    access.MemberName = wProp.Name;
                    access.RemoveAnnotations<MemberResolveResult>();
                    continue;
                }
                if (SameType(access, field.DeclaringTypeDefinition)) continue;
                var key = Normalize(field.Name);
                var property = field.DeclaringTypeDefinition?.Properties.FirstOrDefault(p =>
                    p.CanGet && (p.Getter!.Accessibility == Accessibility.Public || (isBacking && isExternal))
                    && p.IsStatic == field.IsStatic && Normalize(p.Name) == key && p.ReturnType.Equals(field.ReturnType));
                if (property is null) continue;
                access.MemberName = property.Name;
                access.RemoveAnnotations<MemberResolveResult>();
                continue;
            }

            if (!IsWrite(access))
            {
                if (access.Annotation<MemberResolveResult>() is { IsError: false }) continue;
                if (access.Parent is InvocationExpression parentInv && parentInv.Target == access
                    && parentInv.Annotation<InvocationResolveResult>() is { IsError: false }) continue;

                var targetType = access.Target.Annotation<ResolveResult>()?.Type
                    ?? (access.Target is ParenthesizedExpression pe ? pe.Expression.Annotation<ResolveResult>()?.Type : null);
                if (targetType?.GetDefinition() is { Kind: not TypeKind.Unknown } targetDef
                    && !(targetDef.ParentModule?.Compilation is { } comp
                        ? TypeSystemCache.For(comp).HasMember(targetDef, access.MemberName)
                        : targetDef.GetMembers(m => m.Name == access.MemberName).Any()))
                {
                    var key = Normalize(access.MemberName);
                    var isStatic = access.Target is TypeReferenceExpression;
                    var matches = targetDef.GetProperties(p =>
                        p.CanGet && p.Getter!.Accessibility == Accessibility.Public && !p.IsIndexer
                        && p.IsStatic == isStatic && Normalize(p.Name) == key).ToList();
                    if (matches.Count == 1)
                    {
                        access.MemberName = matches[0].Name;
                        access.RemoveAnnotations<MemberResolveResult>();
                    }
                }
            }
        }
        foreach (var inv in invocations)
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
        if (n.StartsWith('<') && n.EndsWith(">k__BackingField", StringComparison.Ordinal))
            n = n[1..^16];
        foreach (var prefix in new[] { "m_", "s_", "k_", "_" })
            if (n.StartsWith(prefix, StringComparison.Ordinal)) { n = n[prefix.Length..]; break; }
        foreach (var suffix in new[] { "Vector", "Field" })
            if (n.Length > suffix.Length && n.EndsWith(suffix, StringComparison.Ordinal)) { n = n[..^suffix.Length]; break; }
        return n.ToLowerInvariant();
    }

    private static bool IsWrite(MemberReferenceExpression e)
    {
        if (e.Parent is AssignmentExpression a && a.Left == e) return true;
        if (e.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement or UnaryOperatorType.AddressOf }) return true;
        if (e.Parent is DirectionExpression) return true;
        if (e.Annotation<MemberResolveResult>()?.Type.IsReferenceType == false)
        {
            AstNode cur = e;
            while (cur.Parent is MemberReferenceExpression outer && outer.Target == cur)
            {
                if (outer.Parent is AssignmentExpression oa && oa.Left == outer) return true;
                if (outer.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement or UnaryOperatorType.AddressOf }) return true;
                if (outer.Parent is DirectionExpression) return true;
                cur = outer;
            }
        }
        return false;
    }

    private static bool SameType(AstNode node, ITypeDefinition? declaring)
    {
        if (declaring is null) return true;
        for (var t = node.GetParent<TypeDeclaration>(); t is not null; t = t.GetParent<TypeDeclaration>())
            if (t.Name == declaring.Name) return true;
        return false;
    }
}
