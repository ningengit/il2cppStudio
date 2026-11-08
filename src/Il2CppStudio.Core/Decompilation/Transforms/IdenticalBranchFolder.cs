using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.CSharp.Syntax.PatternMatching;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// <c>c ? X : X</c> with a side-effect-free condition → <c>X</c>. MSVC's inlined <c>sqrt</c> keeps a NaN/negative
/// slow path calling the same function (<c>(0.0 &gt; d) ? Math.Sqrt(d) : Math.Sqrt(d)</c>), which is exactly <c>Math.Sqrt(d)</c>.
/// </summary>
public static class IdenticalBranchFolder
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var c in tree.Descendants.OfType<ConditionalExpression>().ToList())
            if (c.Parent is not null && EmptyIfRemover.IsPure(c.Condition) && c.TrueExpression.IsMatch(c.FalseExpression))
                c.ReplaceWith(c.TrueExpression.Detach());
    }
}
