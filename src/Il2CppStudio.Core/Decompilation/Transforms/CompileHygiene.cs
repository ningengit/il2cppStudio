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
                    || a.Left is IdentifierExpression l && a.Right is IdentifierExpression r && l.Identifier == r.Identifier))
                s.Remove();
    }

    private static void MarkUnsafe(SyntaxTree tree)
    {
        foreach (var member in tree.Descendants.OfType<EntityDeclaration>().Where(m => m is MethodDeclaration or ConstructorDeclaration or PropertyDeclaration or OperatorDeclaration))
        {
            // Iterator bodies are always a safe context (unsafe modifier does not apply inside): use the safe escape.
            if (member.Descendants.Any(n => n is YieldReturnStatement or YieldBreakStatement)) { SafeIteratorReads(member); continue; }
            var usesPointers = member.Descendants.Any(n =>
                n is PointerReferenceExpression || n is ComposedType { PointerRank: > 0 }
                || n is UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference or UnaryOperatorType.AddressOf });
            if (usesPointers && !member.GetParent<TypeDeclaration>()!.Modifiers.HasFlag(Modifiers.Unsafe)) member.Modifiers |= Modifiers.Unsafe;
        }
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
            if (id.Parent is MemberReferenceExpression m && m.Target == id && KnownClashes.TryGetValue(id.Identifier, out var cns)
                && namespaces.Contains(cns) && namespaces.Contains("System")
                && (id.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type is not { } it || it.Namespace == cns))
                id.ReplaceWith(new TypeReferenceExpression(new MemberType(QualifierFor(cns), id.Identifier)));
        foreach (var st in tree.Descendants.OfType<SimpleType>().ToList())
        {
            var resolved = st.Annotation<TypeResolveResult>()?.Type ?? (st.Parent as TypeReferenceExpression)?.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type;
            if (KnownClashes.TryGetValue(st.Identifier, out var ns) && (resolved is null || resolved.Namespace == ns) && namespaces.Contains(ns) && namespaces.Contains("System"))
            {
                st.ReplaceWith(new MemberType(QualifierFor(ns), st.Identifier));   // unannotated reference: Unity's type is the one IL2CPP code uses
                continue;
            }
            if (resolved is not { } type || type.GetDefinition() is not { } def || def.DeclaringTypeDefinition is not null) continue;
            var compilation = def.ParentModule?.Compilation;
            if (compilation is null || string.IsNullOrEmpty(def.Namespace)) continue;
            var clash = namespaces.Any(ns => ns != def.Namespace
                && compilation.FindType(new FullTypeName(new TopLevelTypeName(ns, def.Name, def.TypeParameterCount))).Kind != TypeKind.Unknown);
            if (!clash) continue;
            var qualified = new MemberType(QualifierFor(def.Namespace), st.Identifier) { IsDoubleColon = false };
            foreach (var arg in st.TypeArguments.ToList()) qualified.TypeArguments.Add(arg.Detach());
            st.ReplaceWith(qualified);
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
