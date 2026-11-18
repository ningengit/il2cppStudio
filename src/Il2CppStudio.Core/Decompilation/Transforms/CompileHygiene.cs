using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Compile hygiene for the exported project:
/// <list type="bullet">
/// <item>members whose bodies use pointer syntax (native fallbacks such as <c>out *(T*)…</c>) get the <c>unsafe</c> modifier;</item>
/// <item>a type name that exists in more than one imported namespace (<c>Random</c>: UnityEngine and System) is fully
/// qualified, using the decompiler's own resolution of which type was meant.</item>
/// </list>
/// </summary>
public static class CompileHygiene
{
    public static void Apply(SyntaxTree tree)
    {
        RemoveSelfAssignments(tree);
        UnwrapConditionParentheses(tree);
        MarkUnsafe(tree);
        QualifyAmbiguous(tree);
    }

    /// <summary><c>if ((x &gt; 0f))</c> → <c>if (x &gt; 0f)</c>: the statement already brackets its condition.</summary>
    private static void UnwrapConditionParentheses(SyntaxTree tree)
    {
        foreach (var p in tree.Descendants.OfType<ParenthesizedExpression>().ToList())
            if (p.Parent is IfElseStatement or WhileStatement or DoWhileStatement && p.Role == Roles.Condition)
                p.ReplaceWith(p.Expression.Detach());
    }

    /// <summary><c>this = this;</c> / <c>x = x;</c>: register round-trips with no effect (and <c>this = …</c> is illegal in classes).</summary>
    private static void RemoveSelfAssignments(SyntaxTree tree)
    {
        foreach (var s in tree.Descendants.OfType<ExpressionStatement>().ToList())
            if (s.Expression is AssignmentExpression { Operator: AssignmentOperatorType.Assign } a
                && (a.Left is ThisReferenceExpression && a.Right is ThisReferenceExpression
                    || a.Left is IdentifierExpression l && a.Right is IdentifierExpression r && l.Identifier == r.Identifier
                    || a.Left is IdentifierExpression l2 && a.Right is DirectionExpression { Expression: IdentifierExpression r2 } && l2.Identifier == r2.Identifier))
                s.Remove();

        foreach (var decl in tree.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            var v = decl.Variables.Single();
            if (v.Initializer is not IdentifierExpression id || id.Identifier != v.Name) continue;
            var owner = decl.Ancestors.OfType<TypeDeclaration>().FirstOrDefault();
            var hasMember = owner?.Members.Any(m =>
                m is FieldDeclaration f && f.Variables.Any(fv => fv.Name == v.Name)
                || m is PropertyDeclaration p && p.Name == v.Name) == true;
            v.Initializer = hasMember
                ? new MemberReferenceExpression(new ThisReferenceExpression(), v.Name)
                : new DefaultValueExpression(decl.Type.Clone());
        }
    }

    private static void MarkUnsafe(SyntaxTree tree)
    {
        foreach (var member in tree.Descendants.OfType<EntityDeclaration>().Where(m => m is MethodDeclaration or ConstructorDeclaration or PropertyDeclaration or OperatorDeclaration or IndexerDeclaration or DestructorDeclaration))
        {
            // Iterator bodies are always a safe context (unsafe modifier does not apply inside): use the safe escape.
            if (member.Descendants.Any(n => n is YieldReturnStatement or YieldBreakStatement)) { SafeIteratorReads(member); continue; }
            var usesPointers = UsesPointers(member);
            if (usesPointers && member.GetParent<TypeDeclaration>()?.Modifiers.HasFlag(Modifiers.Unsafe) != true) member.Modifiers |= Modifiers.Unsafe;
            else if (!usesPointers) member.Modifiers &= ~Modifiers.Unsafe;
        }
    }

    public static void RefreshUnsafe(SyntaxTree tree) => MarkUnsafe(tree);

    private static bool UsesPointers(AstNode member)
    {
        if (member.Descendants.Any(n =>
            n is PointerReferenceExpression || n is ComposedType { PointerRank: > 0 } || n is FixedStatement
            || n is UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference or UnaryOperatorType.AddressOf }
            || n is Expression e && e.Annotation<ResolveResult>()?.Type.Kind == TypeKind.Pointer))
            return true;
        if (member.GetParent<TypeDeclaration>() is not { } owner) return false;
        var ptrFields = owner.Members.OfType<FieldDeclaration>()
            .Where(f => f.ReturnType is ComposedType { PointerRank: > 0 })
            .SelectMany(f => f.Variables.Select(v => v.Name))
            .ToHashSet(StringComparer.Ordinal);
        return ptrFields.Count > 0 && member.Descendants.Any(n =>
            n is IdentifierExpression id && ptrFields.Contains(id.Identifier)
            || n is MemberReferenceExpression mr && ptrFields.Contains(mr.MemberName));
    }

    /// <summary><c>((T*)a)-&gt;f</c> → <c>NativeIntrinsics.Reinterpret&lt;T&gt;(a).f</c>; <c>*(T*)a</c> (read) → <c>NativeIntrinsics.Reinterpret&lt;T&gt;(a)</c>.</summary>
    private static void SafeIteratorReads(EntityDeclaration member)
    {
        member.Modifiers &= ~Modifiers.Unsafe;
        foreach (var p in member.Descendants.OfType<PointerReferenceExpression>().ToList())
            if (Unwrap(p.Target) is CastExpression { Type: ComposedType { PointerRank: 1 } ct } cast)
                p.ReplaceWith(new MemberReferenceExpression(Reinterpret(ct, cast.Expression.Detach()), p.MemberName));
        foreach (var u in member.Descendants.OfType<UnaryOperatorExpression>().Where(u => u.Operator == UnaryOperatorType.Dereference).ToList())
            if (Unwrap(u.Expression) is CastExpression { Type: ComposedType { PointerRank: 1 } ct } cast
                && u.Parent is not DirectionExpression && !(u.Parent is AssignmentExpression a && a.Left == u))
                u.ReplaceWith(Reinterpret(ct, cast.Expression.Detach()));
        if (member.Descendants.Any(n => n is ComposedType { PointerRank: > 0 }))
            member.Modifiers |= Modifiers.Unsafe;   // remaining pointer syntax (writes / out args): report via the compiler, unchanged
    }

    private static Expression Unwrap(Expression e) => e is ParenthesizedExpression p ? Unwrap(p.Expression) : e;

    private static Expression Reinterpret(ComposedType pointerType, Expression address)
    {
        var elem = pointerType.BaseType.Clone();
        var target = new MemberReferenceExpression(new MemberReferenceExpression(new MemberReferenceExpression(
            new IdentifierExpression("Il2CppStudio"), "Native"), "NativeIntrinsics"), "Reinterpret", elem);
        return new InvocationExpression(target, address);
    }

    private static void QualifyAmbiguous(SyntaxTree tree)
    {
        var namespaces = tree.Descendants.OfType<UsingDeclaration>().Select(u => u.Namespace).ToList();
        if (namespaces.Count < 2) return;
        foreach (var id in tree.Descendants.OfType<IdentifierExpression>().ToList())
        {
            if (id.Parent is not MemberReferenceExpression m || m.Target != id) continue;
            if (KnownClashes.TryGetValue(id.Identifier, out var cns)
                && namespaces.Contains(cns) && namespaces.Contains("System")
                && (id.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type is not { } it || it.Namespace == cns))
            {
                id.ReplaceWith(new TypeReferenceExpression(new MemberType(QualifierFor(cns), id.Identifier)));
                continue;
            }
            if (id.Annotation<TypeResolveResult>()?.Type.GetDefinition() is { DeclaringTypeDefinition: null } idDef
                && !string.IsNullOrEmpty(idDef.Namespace)
                && idDef.ParentModule?.Compilation is { } idComp)
            {
                var encNs = string.Join('.', id.Ancestors.OfType<NamespaceDeclaration>().Reverse().Select(n => n.Name));
                if (encNs != idDef.Namespace && !encNs.EndsWith("." + idDef.Namespace, StringComparison.Ordinal)
                    && namespaces.Any(ns => ns != idDef.Namespace
                        && idComp.Modules.Any(mod => mod.GetTypeDefinition(new TopLevelTypeName(ns, idDef.Name, idDef.TypeParameterCount)) is not null)))
                    id.ReplaceWith(new TypeReferenceExpression(new MemberType(QualifierFor(idDef.Namespace), id.Identifier)));
            }
        }
        foreach (var st in tree.Descendants.OfType<SimpleType>().ToList())
        {
            var resolved = st.Annotation<TypeResolveResult>()?.Type
                ?? (st.Parent as TypeReferenceExpression)?.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type
                ?? (st.Parent is FieldDeclaration fd && fd.ReturnType == st ? (fd.GetSymbol() ?? fd.Variables.FirstOrDefault()?.GetSymbol()) is IField f ? f.Type : null : null)
                ?? (st.Parent is PropertyDeclaration pd && pd.ReturnType == st && pd.GetSymbol() is IProperty p ? p.ReturnType : null)
                ?? (st.Parent is MethodDeclaration md && md.ReturnType == st && md.GetSymbol() is IMethod m ? m.ReturnType : null)
                ?? (st.Parent is ParameterDeclaration prm && prm.Type == st && prm.GetSymbol() is IParameter pr ? pr.Type : null)
                ?? (st.Parent is VariableDeclarationStatement vds && vds.Type == st && vds.Variables.FirstOrDefault()?.GetSymbol() is IVariable vr ? vr.Type : null)
                ?? (st.Parent is ObjectCreateExpression oce && oce.Type == st
                    ? oce.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type
                        ?? (oce.Parent is VariableInitializer { Parent: VariableDeclarationStatement vds2 }
                            ? vds2.Variables.FirstOrDefault()?.GetSymbol() is IVariable vr2 ? vr2.Type : vds2.Type.Annotation<TypeResolveResult>()?.Type
                            : null)
                        ?? (oce.Parent is AssignmentExpression ae2 && ae2.Right == oce ? ae2.Left.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type : null)
                    : null)
                ?? (st.Parent is DefaultValueExpression dve && dve.Type == st ? dve.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type : null)
                ?? (st.Parent is CastExpression ce && ce.Type == st ? ce.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type : null)
                ?? (st.Parent is TypeOfExpression toe && toe.Type == st ? toe.Type.Annotation<TypeResolveResult>()?.Type : null);
            if (KnownClashes.TryGetValue(st.Identifier, out var ns) && (resolved is null || resolved.Namespace == ns) && namespaces.Contains(ns) && namespaces.Contains("System"))
            {
                st.ReplaceWith(new MemberType(QualifierFor(ns), st.Identifier));   // unannotated reference: Unity's type is the one IL2CPP code uses
                continue;
            }
            if (st.Identifier == "VideoPlayer" && namespaces.Contains("UnityEngine.Video") && namespaces.Contains("UnityEngine.Switch"))
            {
                var vns = resolved?.Namespace is { Length: > 0 } rns ? rns : "UnityEngine.Video";
                st.ReplaceWith(new MemberType(QualifierFor(vns), st.Identifier));
                continue;
            }
            if (resolved is not { } type || type.GetDefinition() is not { } def || def.DeclaringTypeDefinition is not null) continue;
            var compilation = def.ParentModule?.Compilation;
            if (compilation is null || string.IsNullOrEmpty(def.Namespace)) continue;
            var enclosingNs = string.Join('.', st.Ancestors.OfType<NamespaceDeclaration>().Reverse().Select(n => n.Name));
            if (enclosingNs == def.Namespace || enclosingNs.EndsWith("." + def.Namespace, StringComparison.Ordinal)) continue;
            var clash = namespaces.Any(ns => ns != def.Namespace
                && compilation.Modules.Any(m => m.GetTypeDefinition(new TopLevelTypeName(ns, def.Name, def.TypeParameterCount)) is not null));
            if (!clash) continue;
            var qualified = new MemberType(QualifierFor(def.Namespace), st.Identifier) { IsDoubleColon = false };
            foreach (var arg in st.TypeArguments.ToList()) qualified.TypeArguments.Add(arg.Detach());
            st.ReplaceWith(qualified);
        }
        if (namespaces.Contains("System.IO") && namespaces.Contains("System.Linq"))
        {
            foreach (var inv in tree.Descendants.OfType<InvocationExpression>().ToList())
                if (inv.Parent is not null && inv.Arguments.Count == 0
                    && inv.Target is MemberReferenceExpression { MemberName: "ToArray" } mr && !mr.TypeArguments.Any())
                {
                    var linqEnum = new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("System"), "Linq"), "Enumerable"));
                    inv.ReplaceWith(new InvocationExpression(new MemberReferenceExpression(linqEnum, "ToArray"), mr.Target.Detach()));
                }
        }
    }

    private static readonly Dictionary<string, string> KnownClashes = new() { ["Random"] = "UnityEngine" };

    private static AstType QualifierFor(string ns)
    {
        var parts = ns.Split('.');
        AstType t = new SimpleType(parts[0]);
        foreach (var p in parts.Skip(1)) t = new MemberType(t, p);
        return t;
    }
}
