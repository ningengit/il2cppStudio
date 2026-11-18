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
            if (decl.Variables.Count == 1 && decl.Variables.Single() is { Initializer: DefaultValueExpression } dv
                && decl.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id, Right: DefaultValueExpression } } next
                && id.Identifier == dv.Name)
                next.Remove();
        }

        foreach (var block in tree.Descendants.OfType<BlockStatement>().ToList())
        {
            if (!block.Descendants.OfType<LabelStatement>().Any()) continue;
            var toHoist = block.Statements.OfType<VariableDeclarationStatement>()
                .Where(d => !d.Modifiers.HasFlag(Modifiers.Const) && d.Type is not SimpleType { Identifier: "var" } && d.Variables.Count > 0)
                .ToList();
            foreach (var decl in Enumerable.Reverse(toHoist))
            {
                var earlierShadow = block.Statements.TakeWhile(s => s != decl).SelectMany(s => s.Descendants)
                    .OfType<VariableInitializer>().Any(vi => decl.Variables.Any(dv => dv.Name == vi.Name));
                if (earlierShadow) continue;
                if (decl.Variables.All(v => v.Initializer is DefaultValueExpression))
                {
                    if (block.Statements.FirstOrDefault() == decl) continue;
                    decl.Remove();
                    block.Statements.InsertBefore(block.Statements.FirstOrDefault(), decl);
                }
                else if (decl.Variables.Count == 1
                    && decl.Variables.Single() is { Initializer: { IsNull: false } init } v
                    && init is not ArrayInitializerExpression)
                {
                    var hoisted = new VariableDeclarationStatement(decl.Type.Clone(), v.Name, new DefaultValueExpression(decl.Type.Clone()));
                    decl.ReplaceWith(new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(v.Name), init.Detach())));
                    block.Statements.InsertBefore(block.Statements.FirstOrDefault(), hoisted);
                }
            }
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
