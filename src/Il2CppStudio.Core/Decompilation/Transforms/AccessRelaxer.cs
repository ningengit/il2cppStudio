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
        if (tokens.Count == 0) return;

        var widenedProtectedMethodNames = new HashSet<string>(StringComparer.Ordinal);
        foreach (var md in tree.Descendants.OfType<MethodDeclaration>())
            if (md.Modifiers.HasFlag(Modifiers.Protected) && md.GetSymbol() is IEntity ent
                && tokens.Contains((uint)MetadataTokens.GetToken(ent.MetadataToken)))
                widenedProtectedMethodNames.Add(md.Name);

        foreach (var decl in tree.Descendants.OfType<EntityDeclaration>().ToList())
        {
            var symbol = decl.GetSymbol();
            if (symbol is not IEntity entity) continue;
            var token = (uint)MetadataTokens.GetToken(entity.MetadataToken);
            var inTokens = tokens.Contains(token);
            var baseProtectedWidened = decl is MethodDeclaration { } md2 && md2.Modifiers.HasFlag(Modifiers.Protected)
                && (widenedProtectedMethodNames.Contains(md2.Name)
                    || (md2.Modifiers.HasFlag(Modifiers.Override) && entity is IMethod m
                        && InheritanceHelper.GetBaseMembers(m, includeImplementedInterfaces: false)
                            .Any(bm => tokens.Contains((uint)MetadataTokens.GetToken(bm.MetadataToken)))));

            if (!inTokens && !baseProtectedWidened) continue;
            var wasProtected = decl.Modifiers.HasFlag(Modifiers.Protected);
            var isOverride = decl.Modifiers.HasFlag(Modifiers.Override);
            if (isOverride && !baseProtectedWidened) continue;
            decl.Modifiers = (decl.Modifiers & ~Access) | (wasProtected ? Modifiers.Protected | Modifiers.Internal : Modifiers.Internal);
            if (!isOverride && inTokens)
            {
                var comment = new Comment($" Il2CppStudio: declared {(wasProtected ? "protected" : "private")}; widened because native code inlined into other types accesses it.");
                decl.Parent?.InsertChildBefore(decl, comment, Roles.Comment);
            }
        }
    }
}
