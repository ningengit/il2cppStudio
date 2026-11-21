using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Collapses an inlined C++ <c>bankers_round</c> (<c>Mathf.RoundToInt</c>) expansion inside a loop or method body:
/// <c>modf(x, &amp;i)</c> followed by <c>±0.5</c> even/odd parity checks (<c>&amp; 1</c>) and <c>Math.Floor(x + 0.5)</c> /
/// <c>Math.Ceiling(x - 0.5)</c> branches assigning to the rounded integer local.
/// </summary>
public static class BankersRoundRecognizer
{
    public static void Apply(SyntaxTree tree) => Apply(tree, AstWalk.ScanFlags(tree));

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        if (!flags.HasCallNative) return;
        foreach (var block in AstWalk.Collect<BlockStatement>(tree))
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
                if (!AstWalk.HasAny(ifs, n => n is AssignmentExpression a && a.Left is IdentifierExpression id && id.Identifier == resVar.Identifier)) continue;

                Expression arg = floatArg.Clone();
                Statement? argDecl = i > 0 && stmts[i - 1] is VariableDeclarationStatement { Variables.Count: 1 } prev
                    && prev.Variables.Single().Name == floatArg.Identifier ? prev : null;
                if (argDecl is VariableDeclarationStatement vd) arg = vd.Variables.Single().Initializer.Clone();

                var roundCall = new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Mathf")), "RoundToInt"), arg);
                castAssign.Expression = new AssignmentExpression(targetVar.Clone(), roundCall);

                var deadTemps = new HashSet<string>(StringComparer.Ordinal);
                AstWalk.Visit<IdentifierExpression>(ifs, id => deadTemps.Add(id.Identifier));
                stmts[i].Remove();
                dblDecl.Remove();
                ifs.Remove();
                argDecl?.Remove();
                if (block.GetParent<MethodDeclaration>()?.Body is { } methodBody)
                {
                    var remainingIds = new HashSet<string>(StringComparer.Ordinal);
                    AstWalk.Visit<IdentifierExpression>(methodBody, id => remainingIds.Add(id.Identifier));
                    foreach (var d in AstWalk.Collect<VariableDeclarationStatement>(methodBody))
                        if (d.Variables.Count == 1 && deadTemps.Contains(d.Variables.Single().Name)
                            && !remainingIds.Contains(d.Variables.Single().Name))
                            d.Remove();
                }
            }
        }
    }

    private static bool IsStackOutNativeCall(InvocationExpression call)
    {
        if (call.Target is not MemberReferenceExpression { MemberName: "CallNative" }) return false;
        foreach (var a in call.Arguments)
            if (AstWalk.HasAny(a, static n => n is UnaryOperatorExpression { Operator: UnaryOperatorType.AddressOf, Expression: IndexerExpression { Target: IdentifierExpression { Identifier: "stack" } } }))
                return true;
        return false;
    }

    private static bool IsBankersRoundBranch(IfElseStatement ifs, string dName)
    {
        var hasFloor = false;
        var hasCeiling = false;
        var hasD = false;
        var hasMask1 = false;
        AstWalk.ForEachNode(ifs, n =>
        {
            switch (n)
            {
                case InvocationExpression { Target: MemberReferenceExpression mr }:
                    if (mr.MemberName == "Floor") hasFloor = true;
                    else if (mr.MemberName == "Ceiling") hasCeiling = true;
                    break;
                case IdentifierExpression id when id.Identifier == dName:
                    hasD = true;
                    break;
                case BinaryOperatorExpression { Operator: BinaryOperatorType.BitwiseAnd, Right: PrimitiveExpression { Value: 1 } }:
                    hasMask1 = true;
                    break;
            }
        });
        return hasFloor && hasCeiling && hasD && hasMask1;
    }
}
