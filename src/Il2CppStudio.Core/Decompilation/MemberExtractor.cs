using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Cuts a single method out of a fully transformed type tree. Used for single-method views whose output depends on
/// type-level rewrites (coroutines are rebuilt from a nested state machine class that a method-only decompile never sees).
/// </summary>
public static class MemberExtractor
{
    /// <summary>
    /// Returns a tree holding the type's using declarations, the method's leading comments and the method itself,
    /// or <c>null</c> when no unique match exists (caller falls back to a method-only decompile).
    /// </summary>
    public static SyntaxTree? Extract(SyntaxTree tree, string typeName, string methodName, IReadOnlyList<string> parameterNames)
    {
        var owner = tree.Descendants.OfType<TypeDeclaration>().FirstOrDefault(t => t.Name == StripArity(typeName));
        if (owner is null) return null;
        var matches = owner.Members.OfType<MethodDeclaration>()
            .Where(m => m.Name == methodName && m.Parameters.Select(p => p.Name).SequenceEqual(parameterNames))
            .ToList();
        if (matches.Count != 1) return null;

        var result = new SyntaxTree();
        foreach (var u in tree.Children.OfType<UsingDeclaration>()) result.AddChild((UsingDeclaration)u.Clone(), SyntaxTree.MemberRole);

        var method = matches[0];
        var leading = new List<Comment>();
        for (var n = method.PrevSibling; n is Comment c; n = n.PrevSibling) leading.Insert(0, c);
        foreach (var c in leading) result.AddChild((Comment)c.Clone(), Roles.Comment);
        result.AddChild((MethodDeclaration)method.Clone(), SyntaxTree.MemberRole);
        return result;
    }

    private static string StripArity(string n) => n.IndexOf('`') is var i and >= 0 ? n[..i] : n;
}
