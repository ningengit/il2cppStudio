using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Simplifies static member accesses on an explicit base type inside a derived class
/// (e.g. <c>UnityEngine.Object.Instantiate(x)</c> inside a <c>MonoBehaviour</c> → <c>Instantiate(x)</c>),
/// only when the unqualified identifier resolves unambiguously without shadowing.
/// </summary>
public static class InheritedStaticSimplifier
{
    public static void Apply(SyntaxTree tree)
    {
        var aliases = new HashSet<string>(StringComparer.Ordinal);
        var refs = new List<MemberReferenceExpression>();
        AstWalk.ForEachNode(tree, n =>
        {
            if (n is UsingAliasDeclaration ua) aliases.Add(ua.Alias);
            else if (n is MemberReferenceExpression mr) refs.Add(mr);
        });
        if (refs.Count == 0) return;

        Dictionary<ITypeDefinition, HashSet<IType>>? baseTypeCache = null;
        Dictionary<EntityDeclaration, HashSet<string>>? scopeDeclCache = null;

        foreach (var m in refs)
        {
            if (m.Parent is null) continue;
            if (m.Parent is InvocationExpression { Target: IdentifierExpression { Identifier: "nameof" } }) continue;
            if (m.MemberName.StartsWith("op_", StringComparison.Ordinal)) continue;
            if (UnderAttributeOrUsing(m)) continue;

            IType? targetType = m.Target switch
            {
                TypeReferenceExpression tre => tre.Annotation<TypeResolveResult>()?.Type
                    ?? tre.Type.Annotation<TypeResolveResult>()?.Type,
                IdentifierExpression ie => ie.Annotation<TypeResolveResult>()?.Type,
                _ => null,
            };
            if (targetType is null || targetType.Kind == TypeKind.Interface) continue;
            if (targetType.GetDefinition() is not { } targetDef || targetDef.KnownTypeCode == KnownTypeCode.Object) continue;

            var member = m.Annotation<MemberResolveResult>()?.Member
                ?? (m.Parent is InvocationExpression inv && inv.Target == m ? inv.Annotation<InvocationResolveResult>()?.Member : null);
            if (member is null || !member.IsStatic) continue;
            if (member is not (IField or IProperty or IMethod or IEvent)) continue;

            var enclosingTd = m.GetParent<TypeDeclaration>();
            if (enclosingTd is null) continue;
            var enclosingDef = enclosingTd.Annotation<TypeResolveResult>()?.Type.GetDefinition()
                ?? enclosingTd.GetSymbol() as ITypeDefinition;
            if (enclosingDef is null || enclosingDef.Equals(targetDef)) continue;

            baseTypeCache ??= [];
            if (!baseTypeCache.TryGetValue(enclosingDef, out var bases))
            {
                bases = [];
                foreach (var b in enclosingDef.GetAllBaseTypes())
                    if (b.Kind != TypeKind.Interface)
                        bases.Add(b);
                baseTypeCache[enclosingDef] = bases;
            }
            if (!bases.Contains(targetType))
                continue;

            scopeDeclCache ??= [];
            if (IsShadowed(m, enclosingTd, enclosingDef, targetDef, m.MemberName, aliases, scopeDeclCache))
                continue;

            var simplified = new IdentifierExpression(m.MemberName);
            foreach (var ta in m.TypeArguments.ToList()) simplified.TypeArguments.Add(ta.Detach());
            if (m.Annotation<ResolveResult>() is { } rr) simplified.AddAnnotation(rr);
            m.ReplaceWith(simplified);
        }
    }

    private static bool UnderAttributeOrUsing(AstNode node)
    {
        for (var a = node.Parent; a is not null; a = a.Parent)
            if (a is ICSharpCode.Decompiler.CSharp.Syntax.Attribute or UsingDeclaration or UsingAliasDeclaration)
                return true;
        return false;
    }

    private static bool IsShadowed(
        MemberReferenceExpression site,
        TypeDeclaration enclosingTd,
        ITypeDefinition enclosingDef,
        ITypeDefinition targetDef,
        string name,
        HashSet<string> aliases,
        Dictionary<EntityDeclaration, HashSet<string>> scopeDeclCache)
    {
        if (aliases.Contains(name)) return true;

        // Locals, parameters, foreach variables, and type parameters in enclosing scopes.
        for (var anc = site.Parent; anc is not null; anc = anc.Parent)
        {
            switch (anc)
            {
                case EntityDeclaration ed:
                    if (!scopeDeclCache.TryGetValue(ed, out var declNames))
                    {
                        declNames = new HashSet<string>(StringComparer.Ordinal);
                        AstWalk.ForEachNode(ed, n =>
                        {
                            switch (n)
                            {
                                case ParameterDeclaration pd: declNames.Add(pd.Name); break;
                                case VariableInitializer vi: declNames.Add(vi.Name); break;
                                case SingleVariableDesignation svd: declNames.Add(svd.Identifier); break;
                                case TypeParameterDeclaration tp: declNames.Add(tp.Name); break;
                            }
                        });
                        scopeDeclCache[ed] = declNames;
                    }
                    if (declNames.Contains(name)) return true;
                    if (ed == enclosingTd) goto DoneAncestors;
                    break;
                case LambdaExpression le:
                    foreach (var p in le.Parameters)
                        if (p.Name == name) return true;
                    break;
                case AnonymousMethodExpression ame:
                    foreach (var p in ame.Parameters)
                        if (p.Name == name) return true;
                    break;
            }
        }
    DoneAncestors:

        // Outer type declarations & their type parameters / members in AST.
        for (TypeDeclaration? td = enclosingTd; td is not null; td = td.Parent as TypeDeclaration)
        {
            foreach (var tp in td.TypeParameters)
                if (tp.Name == name) return true;
            foreach (var decl in td.Members)
            {
                if (decl is FieldDeclaration fd && fd.Variables.Any(v => v.Name == name)) return true;
                if (decl is FixedFieldDeclaration ffd && ffd.Variables.Any(v => v.Name == name)) return true;
                if (decl.Name == name) return true;
            }
        }

        // Members declared on enclosingDef, intermediate base classes before targetDef, or outer enclosing types (and their bases).
        var cur = enclosingDef;
        while (cur is not null && !cur.Equals(targetDef))
        {
            if (DeclaresMemberNamed(cur, name)) return true;
            cur = cur.DirectBaseTypes.FirstOrDefault(b => b.Kind != TypeKind.Interface)?.GetDefinition();
        }

        for (var outer = enclosingDef.DeclaringTypeDefinition; outer is not null; outer = outer.DeclaringTypeDefinition)
        {
            for (var ob = outer; ob is not null; ob = ob.DirectBaseTypes.FirstOrDefault(b => b.Kind != TypeKind.Interface)?.GetDefinition())
                if (DeclaresMemberNamed(ob, name)) return true;
        }

        return false;
    }

    private static bool DeclaresMemberNamed(ITypeDefinition typeDef, string name) =>
        typeDef.Members.Any(m => !m.IsExplicitInterfaceImplementation && m.Name == name)
        || typeDef.NestedTypes.Any(nt => nt.Name == name);
}
