using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Declarations of members found by <see cref="CrossTypeAccessScanner"/> become <c>internal</c> (from private /
/// protected) with an explanatory comment; protected members keep <c>protected internal</c> so overrides still match.
/// </summary>
public static class AccessRelaxer
{
    private const Modifiers Access = Modifiers.Private | Modifiers.Protected | Modifiers.Internal | Modifiers.Public;

    public static void Apply(SyntaxTree tree, IReadOnlySet<uint> tokens)
    {
        if (tokens.Count == 0) return;
        foreach (var decl in tree.Descendants.OfType<EntityDeclaration>().ToList())
        {
            if (decl.GetSymbol() is not IEntity entity || !tokens.Contains((uint)System.Reflection.Metadata.Ecma335.MetadataTokens.GetToken(entity.MetadataToken))) continue;
            var wasProtected = decl.Modifiers.HasFlag(Modifiers.Protected);
            var isOverride = decl.Modifiers.HasFlag(Modifiers.Override);
            if (isOverride) continue;   // accessibility of overrides is fixed by the base declaration
            decl.Modifiers = (decl.Modifiers & ~Access) | (wasProtected ? Modifiers.Protected | Modifiers.Internal : Modifiers.Internal);
            var comment = new Comment($" Il2CppStudio: declared {(wasProtected ? "protected" : "private")}; widened because native code inlined into other types accesses it.");
            decl.Parent?.InsertChildBefore(decl, comment, Roles.Comment);
        }
    }
}
