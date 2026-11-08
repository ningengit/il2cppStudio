using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Folds single-use temporaries into the condition that consumes them without changing what is evaluated or in
/// which order (see <see cref="ElseIfFlattener"/>).
/// </summary>
public static class TempFolder
{
    public static bool TryFold(IReadOnlyList<VariableDeclarationStatement> decls, Expression condition, BlockStatement body)
    {
        var names = decls.Select(d => d.Variables.Single().Name).ToList();
        var uses = new Dictionary<string, IdentifierExpression>();
        foreach (var name in names)
        {
            var all = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == name).ToList();
            if (all.Count != 1 || !(all[0] == condition || all[0].Ancestors.Contains(condition))) return false;
            uses[name] = all[0];
        }

        var leaves = ConditionTools.Leaves(condition, out var andFamily);
        int DefIndex(Expression leaf) => names.Select((n, i) => (n, i)).Where(x => leaf == uses[x.n] || uses[x.n].Ancestors.Contains(leaf))
            .Select(x => x.i).DefaultIfEmpty(int.MaxValue).Min();

        if (leaves.Count == 1)
        {
            // Single leaf: temps must be evaluated unconditionally and in declaration order.
            var order = names.OrderBy(n => uses[n].StartLocation).ToList();
            if (!order.SequenceEqual(names) || names.Any(n => ConditionTools.UnderShortCircuit(uses[n], condition))) return false;
            Substitute(decls, uses);
            return true;
        }

        // Chain: every leaf after the first must be pure (so evaluating it unconditionally is harmless), and leaves
        // holding temps must be pure apart from the temps themselves (always true: temps are locals).
        if (leaves.Skip(1).Any(l => !ConditionTools.IsPure(l))) return false;
        foreach (var n in names)
            if (leaves.FirstOrDefault(l => l == uses[n] || uses[n].Ancestors.Contains(l)) is not { } leaf || ConditionTools.UnderShortCircuit(uses[n], leaf))
                return false;

        var tempLeaves = leaves.Where(l => DefIndex(l) != int.MaxValue).OrderBy(DefIndex).ToList();
        var ordered = tempLeaves.Concat(leaves.Where(l => DefIndex(l) == int.MaxValue)).ToList();
        if (tempLeaves.SelectMany(l => names.Where(n => l == uses[n] || uses[n].Ancestors.Contains(l)).OrderBy(n => uses[n].StartLocation))
                .Select(n => names.IndexOf(n)).Zip(Enumerable.Range(0, names.Count)).Any(p => p.First != p.Second))
            return false;   // temps inside one leaf out of declaration order

        var op = andFamily == true ? BinaryOperatorType.BitwiseAnd : BinaryOperatorType.BitwiseOr;
        Expression rebuilt = ordered[0].Detach();
        foreach (var leaf in ordered.Skip(1)) rebuilt = new BinaryOperatorExpression(rebuilt, op, leaf.Detach());
        condition.ReplaceWith(rebuilt);
        Substitute(decls, uses);
        return true;
    }

    private static void Substitute(IReadOnlyList<VariableDeclarationStatement> decls, Dictionary<string, IdentifierExpression> uses)
    {
        foreach (var d in decls)
        {
            var v = d.Variables.Single();
            uses[v.Name].ReplaceWith(v.Initializer.Detach());
        }
    }
}
