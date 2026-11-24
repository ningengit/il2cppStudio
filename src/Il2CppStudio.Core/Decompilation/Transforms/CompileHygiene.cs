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
    public static void Apply(SyntaxTree tree) => Apply(tree, AstWalk.ScanFlags(tree));

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        if (flags.HasBodies)
            CleanSelfAssignsAndParentheses(tree);
        MarkUnsafe(tree, flags);
        if (flags.HasUsingDeclarations)
            QualifyAmbiguous(tree);
    }

    private static void CleanSelfAssignsAndParentheses(SyntaxTree tree)
    {
        var parens = new List<ParenthesizedExpression>();
        var selfAssignStmts = new List<ExpressionStatement>();
        var selfInitDecls = new List<VariableDeclarationStatement>();

        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case ParenthesizedExpression p when p.Parent is IfElseStatement or WhileStatement or DoWhileStatement && p.Role == Roles.Condition:
                    parens.Add(p);
                    break;
                case ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign } a } s
                    when a.Left is ThisReferenceExpression && a.Right is ThisReferenceExpression
                        || a.Left is IdentifierExpression l && a.Right is IdentifierExpression r && l.Identifier == r.Identifier
                        || a.Left is IdentifierExpression l2 && a.Right is DirectionExpression { Expression: IdentifierExpression r2 } && l2.Identifier == r2.Identifier:
                    selfAssignStmts.Add(s);
                    break;
                case VariableDeclarationStatement { Variables.Count: 1 } decl:
                {
                    var v = decl.Variables.Single();
                    if (v.Initializer is IdentifierExpression id && id.Identifier == v.Name)
                        selfInitDecls.Add(decl);
                    break;
                }
            }
        });

        foreach (var p in parens)
            p.ReplaceWith(p.Expression.Detach());
        foreach (var s in selfAssignStmts)
            s.Remove();
        foreach (var decl in selfInitDecls)
        {
            var v = decl.Variables.Single();
            TypeDeclaration? owner = null;
            for (AstNode? a = decl.Parent; a is not null; a = a.Parent)
                if (a is TypeDeclaration td) { owner = td; break; }
            var hasMember = false;
            if (owner is not null)
            {
                foreach (var m in owner.Members)
                {
                    if (m is FieldDeclaration f && f.Variables.Any(fv => fv.Name == v.Name)
                        || m is PropertyDeclaration prop && prop.Name == v.Name)
                    {
                        hasMember = true;
                        break;
                    }
                }
            }
            v.Initializer = hasMember
                ? new MemberReferenceExpression(new ThisReferenceExpression(), v.Name)
                : new DefaultValueExpression(decl.Type.Clone());
        }
    }

    private static void MarkUnsafe(SyntaxTree tree, in TreeFlags flags)
    {
        var members = AstWalk.Collect<EntityDeclaration>(tree);
        if (members.Count == 0) return;

        Dictionary<TypeDeclaration, HashSet<string>>? ptrFieldsByType = null;
        if (flags.HasPointerField)
        {
            ptrFieldsByType = [];
            foreach (var member in members)
            {
                if (member is FieldDeclaration f && f.ReturnType is ComposedType { PointerRank: > 0 }
                    && member.Parent is TypeDeclaration owner)
                {
                    if (!ptrFieldsByType.TryGetValue(owner, out var set))
                        ptrFieldsByType[owner] = set = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var v in f.Variables) set.Add(v.Name);
                }
            }
        }

        foreach (var member in members)
        {
            if (member is not (MethodDeclaration or ConstructorDeclaration or PropertyDeclaration or OperatorDeclaration or IndexerDeclaration or DestructorDeclaration))
                continue;

            var hasUnsafeMod = member.Modifiers.HasFlag(Modifiers.Unsafe);
            if (!flags.HasPointerSyntax && !flags.HasYield && !hasUnsafeMod)
                continue;

            if (flags.HasYield && AstWalk.HasAny(member, static n => n is YieldReturnStatement or YieldBreakStatement))
            {
                SafeIteratorReads(member);
                continue;
            }

            HashSet<string>? ptrFields = null;
            if (ptrFieldsByType is not null && member.Parent is TypeDeclaration ownerTd)
                ptrFieldsByType.TryGetValue(ownerTd, out ptrFields);

            var usesPointers = UsesPointers(member, ptrFields);
            if (usesPointers && member.Parent is not TypeDeclaration { } owner2)
                member.Modifiers |= Modifiers.Unsafe;
            else if (usesPointers && !member.GetParent<TypeDeclaration>()!.Modifiers.HasFlag(Modifiers.Unsafe))
                member.Modifiers |= Modifiers.Unsafe;
            else if (!usesPointers && hasUnsafeMod)
                member.Modifiers &= ~Modifiers.Unsafe;
        }
    }

    public static void RefreshUnsafe(SyntaxTree tree) => MarkUnsafe(tree, AstWalk.ScanFlags(tree));

    private static bool UsesPointers(AstNode member, HashSet<string>? ptrFields)
    {
        return AstWalk.HasAny(member, n =>
            n is PointerReferenceExpression || n is ComposedType { PointerRank: > 0 } || n is FixedStatement
            || n is UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference or UnaryOperatorType.AddressOf }
            || (n is Expression e && e.Annotation<ResolveResult>()?.Type.Kind == TypeKind.Pointer)
            || (ptrFields is not null && (n is IdentifierExpression id && ptrFields.Contains(id.Identifier)
                                          || n is MemberReferenceExpression mr && ptrFields.Contains(mr.MemberName))));
    }

    /// <summary><c>((T*)a)-&gt;f</c> → <c>NativeIntrinsics.Reinterpret&lt;T&gt;(a).f</c>; <c>*(T*)a</c> (read) → <c>NativeIntrinsics.Reinterpret&lt;T&gt;(a)</c>.</summary>
    private static void SafeIteratorReads(EntityDeclaration member)
    {
        member.Modifiers &= ~Modifiers.Unsafe;
        var ptrRefs = AstWalk.Collect<PointerReferenceExpression>(member);
        foreach (var p in ptrRefs)
            if (Unwrap(p.Target) is CastExpression { Type: ComposedType { PointerRank: 1 } ct } cast)
                p.ReplaceWith(new MemberReferenceExpression(Reinterpret(ct, cast.Expression.Detach()), p.MemberName));
        var derefs = new List<UnaryOperatorExpression>();
        AstWalk.Collect<UnaryOperatorExpression>(member, static u => u.Operator == UnaryOperatorType.Dereference, derefs);
        foreach (var u in derefs)
            if (Unwrap(u.Expression) is CastExpression { Type: ComposedType { PointerRank: 1 } ct } cast
                && u.Parent is not DirectionExpression && !(u.Parent is AssignmentExpression a && a.Left == u))
                u.ReplaceWith(Reinterpret(ct, cast.Expression.Detach()));
        if (AstWalk.HasAny(member, static n => n is ComposedType { PointerRank: > 0 }))
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
        var nsList = AstWalk.Collect<UsingDeclaration>(tree);
        if (nsList.Count < 2) return;
        var nsSet = new HashSet<string>(nsList.Count, StringComparer.Ordinal);
        foreach (var u in nsList) nsSet.Add(u.Namespace);
        if (nsSet.Count < 2) return;

        var hasSystem = nsSet.Contains("System");
        var hasVideoClash = nsSet.Contains("UnityEngine.Video") && nsSet.Contains("UnityEngine.Switch");
        var hasIoLinq = nsSet.Contains("System.IO") && nsSet.Contains("System.Linq");

        var ids = new List<IdentifierExpression>();
        var simpleTypes = new List<SimpleType>();
        var toArrayCalls = hasIoLinq ? new List<InvocationExpression>() : null;

        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case IdentifierExpression id when id.Parent is MemberReferenceExpression mr && mr.Target == id:
                    ids.Add(id);
                    break;
                case SimpleType st:
                    simpleTypes.Add(st);
                    break;
                case InvocationExpression inv when toArrayCalls is not null && inv.Arguments.Count == 0
                    && inv.Target is MemberReferenceExpression { MemberName: "ToArray" } mr2 && !mr2.TypeArguments.Any():
                    toArrayCalls.Add(inv);
                    break;
            }
        });

        Dictionary<NamespaceDeclaration, string>? encNsCache = null;
        string GetEnclosingNamespace(AstNode node)
        {
            for (var p = node.Parent; p is not null; p = p.Parent)
            {
                if (p is not NamespaceDeclaration nsDecl) continue;
                encNsCache ??= [];
                if (encNsCache.TryGetValue(nsDecl, out var cached)) return cached;
                var parts = new List<string>(4);
                for (var cur = (AstNode?)nsDecl; cur is not null; cur = cur.Parent)
                    if (cur is NamespaceDeclaration nd) parts.Add(nd.Name);
                parts.Reverse();
                var full = string.Join('.', parts);
                encNsCache[nsDecl] = full;
                return full;
            }
            return "";
        }

        foreach (var id in ids)
        {
            if (hasSystem && KnownClashes.TryGetValue(id.Identifier, out var cns)
                && nsSet.Contains(cns)
                && (id.Annotation<ICSharpCode.Decompiler.Semantics.ResolveResult>()?.Type is not { } it || it.Namespace == cns))
            {
                id.ReplaceWith(new TypeReferenceExpression(new MemberType(QualifierFor(cns), id.Identifier)));
                continue;
            }
            if (id.Annotation<TypeResolveResult>()?.Type.GetDefinition() is { DeclaringTypeDefinition: null } idDef
                && !string.IsNullOrEmpty(idDef.Namespace)
                && idDef.ParentModule?.Compilation is { } idComp)
            {
                var encNs = GetEnclosingNamespace(id);
                if (encNs != idDef.Namespace && !encNs.EndsWith("." + idDef.Namespace, StringComparison.Ordinal)
                    && TypeSystemCache.For(idComp).HasAmbiguousNamespace(idDef.Name, idDef.TypeParameterCount, idDef.Namespace, nsSet))
                    id.ReplaceWith(new TypeReferenceExpression(new MemberType(QualifierFor(idDef.Namespace), id.Identifier)));
            }
        }

        foreach (var st in simpleTypes)
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
            if (hasSystem && KnownClashes.TryGetValue(st.Identifier, out var ns) && (resolved is null || resolved.Namespace == ns) && nsSet.Contains(ns))
            {
                st.ReplaceWith(new MemberType(QualifierFor(ns), st.Identifier));   // unannotated reference: Unity's type is the one IL2CPP code uses
                continue;
            }
            if (hasVideoClash && st.Identifier == "VideoPlayer")
            {
                var vns = resolved?.Namespace is { Length: > 0 } rns ? rns : "UnityEngine.Video";
                st.ReplaceWith(new MemberType(QualifierFor(vns), st.Identifier));
                continue;
            }
            if (resolved is not { } type || type.GetDefinition() is not { } def || def.DeclaringTypeDefinition is not null) continue;
            var compilation = def.ParentModule?.Compilation;
            if (compilation is null || string.IsNullOrEmpty(def.Namespace)) continue;
            var enclosingNs = GetEnclosingNamespace(st);
            if (enclosingNs == def.Namespace || enclosingNs.EndsWith("." + def.Namespace, StringComparison.Ordinal)) continue;
            if (!TypeSystemCache.For(compilation).HasAmbiguousNamespace(def.Name, def.TypeParameterCount, def.Namespace, nsSet)) continue;
            var qualified = new MemberType(QualifierFor(def.Namespace), st.Identifier) { IsDoubleColon = false };
            foreach (var arg in st.TypeArguments.ToList()) qualified.TypeArguments.Add(arg.Detach());
            st.ReplaceWith(qualified);
        }

        if (toArrayCalls is not null)
        {
            foreach (var inv in toArrayCalls)
            {
                if (inv.Parent is not null && inv.Target is MemberReferenceExpression mr)
                {
                    var linqEnum = new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("System"), "Linq"), "Enumerable"));
                    inv.ReplaceWith(new InvocationExpression(new MemberReferenceExpression(linqEnum, "ToArray"), mr.Target.Detach()));
                }
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

    public static void PruneUnusedNativeUsing(SyntaxTree tree)
    {
        var nativeUsings = AstWalk.Collect<UsingDeclaration>(tree, static u => u.Namespace == "Il2CppStudio.Native");
        if (nativeUsings.Count == 0) return;

        var stack = new Stack<AstNode>(64);
        stack.Push(tree);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is UsingDeclaration) continue;

            if (n is IdentifierExpression { Identifier: "NativeIntrinsics" }
                || n is SimpleType { Identifier: "NativeIntrinsics" }
                || n is MemberType { MemberName: "NativeIntrinsics" }
                || n is MemberReferenceExpression { MemberName: "NativeIntrinsics" }
                || n.Annotation<ResolveResult>()?.Type.Namespace == "Il2CppStudio.Native")
            {
                return;
            }

            var ch = n.LastChild;
            while (ch is not null)
            {
                stack.Push(ch);
                ch = ch.PrevSibling;
            }
        }

        foreach (var u in nativeUsings)
            u.Remove();
    }
}
