using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// <c>string.Equals(a, b)</c> → <c>a == b</c> and <c>!string.Equals(a, b)</c> → <c>a != b</c>. Exact: the C# compiler
/// lowers <c>string == string</c> to <c>String.op_Equality</c>, which IL2CPP inlines into the same <c>String.Equals</c>
/// call. Only the static two-argument overload is rewritten (no StringComparison argument).
/// </summary>
public static class StringEqualitySugar
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var call in tree.Descendants.OfType<InvocationExpression>().ToList())
        {
            if (call.Parent is null || call.Arguments.Count != 2 || call.Arguments.Any(a => a is NamedArgumentExpression)) continue;
            if (call.Target is not MemberReferenceExpression { MemberName: "Equals", Target: var t } || !IsStringType(t)) continue;
            var args = call.Arguments.ToList();
            var negated = call.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Not } not;
            var op = negated ? BinaryOperatorType.InEquality : BinaryOperatorType.Equality;
            var replacement = new BinaryOperatorExpression(args[0].Detach(), op, args[1].Detach());
            if (negated) call.Parent!.ReplaceWith(replacement);
            else call.ReplaceWith(replacement);
        }
    }

    private static bool IsStringType(Expression e) => e switch
    {
        TypeReferenceExpression { Type: PrimitiveType { Keyword: "string" } } => true,
        TypeReferenceExpression { Type: SimpleType { Identifier: "String" } } => true,
        TypeReferenceExpression { Type: MemberType { MemberName: "String" } } => true,
        _ => false,
    };
}
