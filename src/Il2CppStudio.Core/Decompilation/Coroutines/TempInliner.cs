using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// <c>string label = label02; m_textMeshPro.text = label;</c> → <c>m_textMeshPro.text = label02;</c>.
/// Only for side-effect-free initializers (identifiers, field/member reads, constants) used exactly once, in the
/// immediately following statement; register spills from the lifter produce many of these.
/// </summary>
public static class TempInliner
{
    public static void Run(BlockStatement body)
    {
        foreach (var decl in body.Descendants.OfType<VariableDeclarationStatement>().ToList())
        {
            if (decl.Parent is not BlockStatement || decl.Variables.Count != 1) continue;
            var v = decl.Variables.Single();
            if (v.Initializer.IsNull || !IsPure(v.Initializer) || decl.NextSibling is not Statement next) continue;
            var uses = body.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name).ToList();
            if (uses.Count != 1 || !uses[0].Ancestors.Contains(next) || IsWriteTarget(uses[0]) || Writes(next, Names(v.Initializer))) continue;
            uses[0].ReplaceWith(v.Initializer.Detach());
            decl.Remove();
        }
    }

    private static bool IsPure(Expression e) => e switch
    {
        PrimitiveExpression or NullReferenceExpression or ThisReferenceExpression or IdentifierExpression => true,
        MemberReferenceExpression m => IsPure(m.Target),
        TypeReferenceExpression => true,
        _ => false,
    };

    private static HashSet<string> Names(Expression e) =>
        e.DescendantsAndSelf.Select(n => n switch { IdentifierExpression i => i.Identifier, MemberReferenceExpression m => m.MemberName, _ => null })
            .OfType<string>().ToHashSet();

    /// <summary>True when the statement may write one of the names (moving the read past it would change its value).</summary>
    private static bool Writes(Statement s, HashSet<string> names) =>
        s.Descendants.OfType<AssignmentExpression>().Any(a => Names(a.Left).Overlaps(names))
        || s.Descendants.OfType<UnaryOperatorExpression>().Any(u => u.Operator is UnaryOperatorType.Increment or UnaryOperatorType.Decrement
               or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement && Names(u.Expression).Overlaps(names))
        || s.Descendants.OfType<InvocationExpression>().Any();   // calls may write fields

    private static bool IsWriteTarget(IdentifierExpression id) =>
        id.Parent is AssignmentExpression a && a.Left == id
        || id.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement
                                                   or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement }
        || id.Parent is DirectionExpression;
}
