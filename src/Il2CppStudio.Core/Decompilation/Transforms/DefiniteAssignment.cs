using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Satisfies C# definite-assignment rules where lifted control flow is correct but not provably so to the compiler:
/// locals declared without an initializer get <c>= default</c>, and every <c>out</c> parameter is set to
/// <c>default</c> on entry. Both only add a store the original code overwrites before any read.
/// </summary>
public static class DefiniteAssignment
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var decl in tree.Descendants.OfType<VariableDeclarationStatement>().ToList())
        {
            if (decl.Modifiers.HasFlag(Modifiers.Const) || decl.Type is SimpleType { Identifier: "var" }) continue;
            foreach (var v in decl.Variables)
                if (v.Initializer.IsNull) v.Initializer = new DefaultValueExpression(decl.Type.Clone());
        }

        foreach (var method in tree.Descendants.OfType<MethodDeclaration>())
        {
            if (method.Body.IsNull) continue;
            var outs = method.Parameters.Where(p => p.ParameterModifier == ICSharpCode.Decompiler.TypeSystem.ReferenceKind.Out).ToList();
            foreach (var p in Enumerable.Reverse(outs))
            {
                var init = new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(p.Name), new DefaultValueExpression(p.Type.Clone())));
                if (method.Body.Statements.FirstOrDefault() is { } first) method.Body.Statements.InsertBefore(first, init);
                else method.Body.Statements.Add(init);
            }
        }
    }
}
