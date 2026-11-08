using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Final pass over a reconstructed coroutine body. Rebuilding control flow from the state dispatch can leave a local
/// used where its declaration is not in scope (declared inside one resume arm, read in another), or drop the
/// declaration together with the dispatch prefix. For every local known from the original MoveNext (or the hoisted
/// field set) that has such a use, the declaration is hoisted to the top of the method as <c>T x = default(T);</c>
/// and inner declarations become plain assignments. Uses are only repaired, never invented.
/// </summary>
internal static class ScopeRepair
{
    public static void Run(BlockStatement body, IReadOnlyDictionary<string, AstType> known)
    {
        foreach (var (name, type) in known)
        {
            var uses = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == name).ToList();
            if (uses.Count == 0) continue;
            var decls = body.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Any(v => v.Name == name)).ToList();
            if (decls.Count == 1 && uses.All(u => InScope(u, decls[0]))) continue;

            var declType = decls.FirstOrDefault()?.Type.Clone() ?? type.Clone();
            foreach (var d in decls) Demote(d, name);
            var hoisted = new VariableDeclarationStatement(declType, name, new DefaultValueExpression(declType.Clone()));
            if (body.Statements.FirstOrDefault() is { } first) body.Statements.InsertBefore(first, hoisted);
            else body.Statements.Add(hoisted);
        }
    }

    /// <summary>A use is in scope when it follows the declaration inside the declaration's block (or a nested block).</summary>
    private static bool InScope(AstNode use, VariableDeclarationStatement decl)
    {
        if (decl.Parent is not BlockStatement block || !use.Ancestors.Contains(block)) return false;
        for (var s = decl.NextSibling; s is not null; s = s.NextSibling)
            if (s == use || use.Ancestors.Contains(s)) return true;
        return false;
    }

    /// <summary><c>T x = e;</c> → <c>x = e;</c>; <c>T x;</c> → removed (other variables in a multi-declaration are kept).</summary>
    private static void Demote(VariableDeclarationStatement d, string name)
    {
        var v = d.Variables.First(x => x.Name == name);
        Statement? replacement = v.Initializer.IsNull ? null
            : new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(name), v.Initializer.Detach()));
        if (d.Variables.Count > 1)
        {
            v.Remove();
            if (replacement is not null) d.Parent!.InsertChildAfter(d, replacement, BlockStatement.StatementRole);
        }
        else if (replacement is not null) d.ReplaceWith(replacement);
        else d.Remove();
    }

    /// <summary>Locals declared anywhere in the original MoveNext, plus hoisted-field locals by their user names.</summary>
    public static Dictionary<string, AstType> KnownLocals(BlockStatement moveNext, IEnumerable<(string Name, AstType Type)> hoisted)
    {
        var map = new Dictionary<string, AstType>(StringComparer.Ordinal);
        foreach (var d in moveNext.Descendants.OfType<VariableDeclarationStatement>())
            foreach (var v in d.Variables)
                if (d.Type is not SimpleType { Identifier: "var" }) map.TryAdd(v.Name, d.Type);
        foreach (var (name, type) in hoisted) map.TryAdd(name, type);
        return map;
    }
}
