using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Collapses an inlined C++ <c>bankers_round</c> (<c>Mathf.RoundToInt</c>) expansion inside a loop or method body:
/// <c>modf(x, &amp;i)</c> followed by <c>±0.5</c> even/odd parity checks (<c>&amp; 1</c>) and <c>Math.Floor(x + 0.5)</c> /
/// <c>Math.Ceiling(x - 0.5)</c> branches assigning to the rounded integer local.
/// </summary>
public static class BankersRoundRecognizer
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var block in tree.Descendants.OfType<BlockStatement>().ToList())
        {
            var stmts = block.Statements.ToList();
            for (var i = 0; i + 2 < stmts.Count; i++)
            {
                if (stmts[i] is not ExpressionStatement { Expression: InvocationExpression modf } || !IsStackOutNativeCall(modf)) continue;
                if (stmts[i + 1] is not VariableDeclarationStatement { Variables.Count: 1 } dblDecl
                    || dblDecl.Type is not PrimitiveType { Keyword: "double" }
                    || dblDecl.Variables.Single().Initializer is not IdentifierExpression floatArg) continue;
                var dName = dblDecl.Variables.Single().Name;
                if (stmts[i + 2] is not IfElseStatement ifs || !IsBankersRoundBranch(ifs, dName)) continue;

                // Find the final `(int)res` assignment after the branch.
                var castAssign = stmts.Skip(i + 3).OfType<ExpressionStatement>().FirstOrDefault(s =>
                    s.Expression is AssignmentExpression { Operator: AssignmentOperatorType.Assign, Right: CastExpression { Type: PrimitiveType { Keyword: "int" }, Expression: IdentifierExpression } });
                if (castAssign?.Expression is not AssignmentExpression { Left: IdentifierExpression targetVar, Right: CastExpression { Expression: IdentifierExpression resVar } }) continue;
                if (!ifs.Descendants.OfType<AssignmentExpression>().Any(a => a.Left is IdentifierExpression id && id.Identifier == resVar.Identifier)) continue;

                Expression arg = floatArg.Clone();
                Statement? argDecl = i > 0 && stmts[i - 1] is VariableDeclarationStatement { Variables.Count: 1 } prev
                    && prev.Variables.Single().Name == floatArg.Identifier ? prev : null;
                if (argDecl is VariableDeclarationStatement vd) arg = vd.Variables.Single().Initializer.Clone();

                var roundCall = new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Mathf")), "RoundToInt"), arg);
                castAssign.Expression = new AssignmentExpression(targetVar.Clone(), roundCall);

                var deadTemps = ifs.Descendants.OfType<IdentifierExpression>()
                    .Select(id => id.Identifier).ToHashSet();
                stmts[i].Remove();
                dblDecl.Remove();
                ifs.Remove();
                argDecl?.Remove();
                if (block.GetParent<MethodDeclaration>()?.Body is { } methodBody)
                    foreach (var d in methodBody.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1 && deadTemps.Contains(d.Variables.Single().Name)).ToList())
                        if (!methodBody.Descendants.OfType<IdentifierExpression>().Any(id => id.Identifier == d.Variables.Single().Name))
                            d.Remove();
            }
        }
    }

    private static bool IsStackOutNativeCall(InvocationExpression call) =>
        call.Target is MemberReferenceExpression { MemberName: "CallNative" }
        && call.Arguments.Any(a => a.ToString().Contains("&stack[", StringComparison.Ordinal));

    private static bool IsBankersRoundBranch(IfElseStatement ifs, string dName)
    {
        var calls = ifs.Descendants.OfType<InvocationExpression>()
            .Select(c => (c.Target as MemberReferenceExpression)?.MemberName).OfType<string>().ToHashSet();
        return calls.Contains("Floor") && calls.Contains("Ceiling")
            && ifs.Descendants.OfType<IdentifierExpression>().Any(id => id.Identifier == dName)
            && ifs.Descendants.OfType<BinaryOperatorExpression>().Any(b => b.Operator == BinaryOperatorType.BitwiseAnd && b.Right is PrimitiveExpression { Value: 1 });
    }
}
