using System.Reflection.Metadata.Ecma335;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Declarations of members found by <see cref="CrossTypeAccessScanner"/> become <c>internal</c> (from private /
/// protected) with an explanatory comment; protected virtual/override methods across the assembly widen consistently
/// to <c>protected internal</c> so base and overrides match.
/// </summary>
public static class AccessRelaxer
{
    private const Modifiers Access = Modifiers.Private | Modifiers.Protected | Modifiers.Internal | Modifiers.Public;

    public static void Apply(SyntaxTree tree, IReadOnlySet<uint> tokens)
    {
        var flags = AstWalk.ScanFlags(tree);
        Apply(tree, tokens, in flags);
    }

    internal static void Apply(SyntaxTree tree, IReadOnlySet<uint> tokens, in TreeFlags flags)
    {
        if (tokens.Count > 0) RelaxTokens(tree, tokens);
        if (flags.HasCompilerGeneratedTypes) WidenSurvivingStateMachines(tree);
        WidenReferencedNestedTypes(tree);
    }

    private static void RelaxTokens(SyntaxTree tree, IReadOnlySet<uint> tokens)
    {
        var decls = AstWalk.Collect<EntityDeclaration>(tree);
        var widenedProtectedMethodNames = new HashSet<string>(StringComparer.Ordinal);
        foreach (var decl in decls)
            if (decl is MethodDeclaration md
                && md.Modifiers.HasFlag(Modifiers.Protected) && !md.Modifiers.HasFlag(Modifiers.Override)
                && md.GetSymbol() is IEntity ent && tokens.Contains((uint)MetadataTokens.GetToken(ent.MetadataToken)))
                widenedProtectedMethodNames.Add(md.Name);

        foreach (var decl in decls)
        {
            if (decl is DestructorDeclaration || decl is ConstructorDeclaration cd && cd.Modifiers.HasFlag(Modifiers.Static)) continue;
            var symbol = decl.GetSymbol();
            if (symbol is not IEntity entity) continue;
            var token = (uint)MetadataTokens.GetToken(entity.MetadataToken);
            var inTokens = tokens.Contains(token);

            if (decl is Accessor acc)
            {
                if (acc.Parent is not PropertyDeclaration parentProp || !inTokens) continue;
                if (parentProp.Modifiers.HasFlag(Modifiers.Public))
                {
                    if (parentProp.Getter is { IsNull: false } && parentProp.Setter is { IsNull: false }
                        && (acc.Modifiers & Access) != Modifiers.None)
                        acc.Modifiers = Modifiers.Internal;
                    continue;
                }
                if (parentProp.Modifiers.HasFlag(Modifiers.Override)) continue;
                acc.Modifiers = Modifiers.None;
                var propWasProtected = parentProp.Modifiers.HasFlag(Modifiers.Protected);
                parentProp.Modifiers = (parentProp.Modifiers & ~Access) | (propWasProtected ? Modifiers.Protected | Modifiers.Internal : Modifiers.Internal);
                continue;
            }

            if (decl.Modifiers.HasFlag(Modifiers.Public)) continue;

            var isOverride = decl.Modifiers.HasFlag(Modifiers.Override);
            var baseProtectedWidened = false;
            if (isOverride)
            {
                if (decl is MethodDeclaration md2 && md2.Modifiers.HasFlag(Modifiers.Protected) && entity is IMethod m)
                    baseProtectedWidened = InheritanceHelper.GetBaseMembers(m, includeImplementedInterfaces: false)
                        .Any(bm => !bm.IsOverride && bm.ParentModule == m.ParentModule
                            && tokens.Contains((uint)MetadataTokens.GetToken(bm.MetadataToken)));
                else if (decl is PropertyDeclaration pd2 && pd2.Modifiers.HasFlag(Modifiers.Protected) && entity is IProperty prop)
                    baseProtectedWidened = InheritanceHelper.GetBaseMembers(prop, includeImplementedInterfaces: false)
                        .Any(bp => !bp.IsOverride && bp.ParentModule == prop.ParentModule
                            && bmPropTokens(bp, tokens));
                if (!baseProtectedWidened) continue;
            }
            else if (!inTokens && !(decl is MethodDeclaration md3 && md3.Modifiers.HasFlag(Modifiers.Protected)
                && widenedProtectedMethodNames.Contains(md3.Name)))
            {
                continue;
            }

            if (!isOverride && decl is MethodDeclaration or PropertyDeclaration && decl.Modifiers.HasFlag(Modifiers.Protected)
                && (decl.Modifiers.HasFlag(Modifiers.Virtual) || decl.Modifiers.HasFlag(Modifiers.Abstract))
                && entity is IMember em
                && InheritanceHelper.GetBaseMembers(em, includeImplementedInterfaces: false).Any(bm => bm.ParentModule != em.ParentModule))
                continue;

            var wasProtected = decl.Modifiers.HasFlag(Modifiers.Protected);
            decl.Modifiers = (decl.Modifiers & ~Access) | (wasProtected ? Modifiers.Protected | Modifiers.Internal : Modifiers.Internal);
            if (!isOverride && inTokens)
            {
                var comment = new Comment($" Il2CppStudio: declared {(wasProtected ? "protected" : "private")}; widened because native code inlined into other types accesses it.");
                decl.Parent?.InsertChildBefore(decl, comment, Roles.Comment);
            }
        }
    }

    private static bool bmPropTokens(IMember bp, IReadOnlySet<uint> tokens) =>
        bp is IProperty p && ((p.Getter is { } g && tokens.Contains((uint)MetadataTokens.GetToken(g.MetadataToken)))
            || (p.Setter is { } s && tokens.Contains((uint)MetadataTokens.GetToken(s.MetadataToken))));

    private static void WidenSurvivingStateMachines(SyntaxTree tree)
    {
        AstWalk.Visit<TypeDeclaration>(tree, static td =>
        {
            if (td.Parent is not TypeDeclaration) return;
            if (!td.Name.Contains(">d", StringComparison.Ordinal) && !td.Name.Contains("_d__", StringComparison.Ordinal)) return;
            for (var cur = td; cur is not null && cur.Parent is TypeDeclaration; cur = cur.Parent as TypeDeclaration)
            {
                var acc = cur.Modifiers & Access;
                if (acc is Modifiers.Private or Modifiers.None)
                    cur.Modifiers = (cur.Modifiers & ~Access) | Modifiers.Internal;
                else if (acc == Modifiers.Protected)
                    cur.Modifiers = (cur.Modifiers & ~Access) | Modifiers.Protected | Modifiers.Internal;
            }
            foreach (var fd in td.Members.OfType<FieldDeclaration>())
            {
                var acc = fd.Modifiers & Access;
                if (acc is Modifiers.Private or Modifiers.None)
                    fd.Modifiers = (fd.Modifiers & ~Access) | Modifiers.Internal;
                else if (acc == Modifiers.Protected)
                    fd.Modifiers = (fd.Modifiers & ~Access) | Modifiers.Protected | Modifiers.Internal;
            }
        });
    }

    private static void WidenReferencedNestedTypes(SyntaxTree tree)
    {
        var nestedPrivateDecls = new List<EntityDeclaration>();
        var members = new List<EntityDeclaration>();
        AstWalk.Visit<EntityDeclaration>(tree, ed =>
        {
            members.Add(ed);
            if (ed is TypeDeclaration or DelegateDeclaration && ed.Parent is TypeDeclaration
                && (ed.Modifiers & Access) is Modifiers.Private or Modifiers.None)
                nestedPrivateDecls.Add(ed);
        });
        if (nestedPrivateDecls.Count == 0) return;

        bool changed;
        do
        {
            changed = false;
            var needsInternal = new HashSet<string>(StringComparer.Ordinal);
            var needsProtected = new HashSet<string>(StringComparer.Ordinal);

            void CollectTypes(AstNode? root, bool isInternalOrPublic, bool isProtected)
            {
                if (root is null) return;
                var stack = new Stack<AstNode>(8);
                stack.Push(root);
                while (stack.Count > 0)
                {
                    var cur = stack.Pop();
                    if (cur is SimpleType st)
                    {
                        if (isInternalOrPublic) needsInternal.Add(st.Identifier);
                        if (isProtected) needsProtected.Add(st.Identifier);
                    }
                    else if (cur is MemberType mt)
                    {
                        if (isInternalOrPublic) needsInternal.Add(mt.MemberName);
                        if (isProtected) needsProtected.Add(mt.MemberName);
                    }
                    var ch = cur.LastChild;
                    while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
                }
            }

            foreach (var member in members)
            {
                var underPrivateNested = false;
                for (AstNode? a = member.Parent; a is not null; a = a.Parent)
                {
                    if (a is TypeDeclaration anc && anc.Parent is TypeDeclaration
                        && (anc.Modifiers & Access) is Modifiers.Private or Modifiers.None)
                    {
                        underPrivateNested = true;
                        break;
                    }
                }
                if (underPrivateNested) continue;

                var acc = member.Modifiers & Access;
                var isInternalOrPublic = acc.HasFlag(Modifiers.Public) || acc.HasFlag(Modifiers.Internal);
                var isProtected = acc.HasFlag(Modifiers.Protected);
                if (!isInternalOrPublic && !isProtected) continue;

                switch (member)
                {
                    case FieldDeclaration fd:
                        CollectTypes(fd.ReturnType, isInternalOrPublic, isProtected);
                        break;
                    case PropertyDeclaration pd:
                        CollectTypes(pd.ReturnType, isInternalOrPublic, isProtected);
                        break;
                    case IndexerDeclaration id:
                        CollectTypes(id.ReturnType, isInternalOrPublic, isProtected);
                        foreach (var p in id.Parameters) CollectTypes(p.Type, isInternalOrPublic, isProtected);
                        break;
                    case MethodDeclaration md:
                        CollectTypes(md.ReturnType, isInternalOrPublic, isProtected);
                        foreach (var p in md.Parameters) CollectTypes(p.Type, isInternalOrPublic, isProtected);
                        break;
                    case ConstructorDeclaration cd:
                        foreach (var p in cd.Parameters) CollectTypes(p.Type, isInternalOrPublic, isProtected);
                        break;
                    case DelegateDeclaration dd:
                        CollectTypes(dd.ReturnType, isInternalOrPublic, isProtected);
                        foreach (var p in dd.Parameters) CollectTypes(p.Type, isInternalOrPublic, isProtected);
                        break;
                    case EventDeclaration ed:
                        CollectTypes(ed.ReturnType, isInternalOrPublic, isProtected);
                        break;
                    case TypeDeclaration td:
                        foreach (var bt in td.BaseTypes) CollectTypes(bt, isInternalOrPublic, isProtected);
                        break;
                }
            }

            foreach (var nested in nestedPrivateDecls)
            {
                if (!needsInternal.Contains(nested.Name)) continue;
                var acc = nested.Modifiers & Access;
                if (acc is Modifiers.Private or Modifiers.None)
                {
                    var targetAcc = needsProtected.Contains(nested.Name) ? Modifiers.Protected | Modifiers.Internal : Modifiers.Internal;
                    nested.Modifiers = (nested.Modifiers & ~Access) | targetAcc;
                    changed = true;
                }
                else if (acc == Modifiers.Protected)
                {
                    nested.Modifiers = (nested.Modifiers & ~Access) | Modifiers.Protected | Modifiers.Internal;
                    changed = true;
                }
            }
        }
        while (changed);
    }
}
