using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.Syntax;
using Il2CppStudio.Core.Decompilation.Coroutines;

namespace Il2CppStudio.Tests;

public sealed class CoroutineTransformTests
{
    [Fact]
    public void Reconstructs_SwitchWithTrailingStateReturn_LikeAfterFollow()
    {
        var tree = new SyntaxTree();

        var smType = new TypeDeclaration
        {
            Name = "<afterFollow>d__115",
            ClassType = ClassType.Class,
            Members =
            {
                new FieldDeclaration
                {
                    ReturnType = new PrimitiveType("int"),
                    Variables = { new VariableInitializer("<>1__state") },
                },
                new FieldDeclaration
                {
                    ReturnType = new PrimitiveType("object"),
                    Variables = { new VariableInitializer("<>2__current") },
                },
                new MethodDeclaration
                {
                    Name = "MoveNext",
                    ReturnType = new PrimitiveType("bool"),
                    Body = new BlockStatement
                    {
                        new VariableDeclarationStatement(
                            new PrimitiveType("int"),
                            "num",
                            new IdentifierExpression("<>1__state")),
                        new SwitchStatement
                        {
                            Expression = new IdentifierExpression("num"),
                            SwitchSections =
                            {
                                new SwitchSection
                                {
                                    CaseLabels = { new CaseLabel(new PrimitiveExpression(1)) },
                                    Statements =
                                    {
                                        new ExpressionStatement(new AssignmentExpression(
                                            new IdentifierExpression("<>1__state"),
                                            new PrimitiveExpression(-1))),
                                        new ExpressionStatement(new InvocationExpression(
                                            new MemberReferenceExpression(new IdentifierExpression("Debug"), "Log"),
                                            new PrimitiveExpression("Now Move On!!"))),
                                        new BreakStatement(),
                                    },
                                },
                                new SwitchSection
                                {
                                    CaseLabels = { new CaseLabel(new PrimitiveExpression(0)) },
                                    Statements =
                                    {
                                        new ExpressionStatement(new AssignmentExpression(
                                            new IdentifierExpression("<>1__state"),
                                            new PrimitiveExpression(-1))),
                                        new ExpressionStatement(new AssignmentExpression(
                                            new IdentifierExpression("<>2__current"),
                                            new ObjectCreateExpression(new SimpleType("WaitForSeconds"), new PrimitiveExpression(8f)))),
                                        new ExpressionStatement(new AssignmentExpression(
                                            new IdentifierExpression("<>1__state"),
                                            new PrimitiveExpression(1))),
                                        new BreakStatement(),
                                    },
                                },
                            },
                        },
                        new ReturnStatement(new BinaryOperatorExpression(
                            new IdentifierExpression("num"),
                            BinaryOperatorType.Equality,
                            new PrimitiveExpression(0))),
                    },
                },
            },
        };

        var outer = new TypeDeclaration
        {
            Name = "AIfollow",
            ClassType = ClassType.Class,
            Members =
            {
                smType,
                new MethodDeclaration
                {
                    Name = "afterFollow",
                    ReturnType = new SimpleType("IEnumerator"),
                    Body = new BlockStatement
                    {
                        new ReturnStatement(new ObjectCreateExpression(
                            new SimpleType("<afterFollow>d__115"),
                            new PrimitiveExpression(0))),
                    },
                },
            },
        };

        tree.Members.Add(outer);

        var reasons = new List<(string Type, string Reason)>();
        var rebuilt = CoroutineTransform.Apply(tree, (t, r) => reasons.Add((t, r)));

        Assert.Equal(1, rebuilt);
        Assert.Empty(reasons);

        using var sw = new StringWriter();
        tree.AcceptVisitor(new CSharpOutputVisitor(sw, FormattingOptionsFactory.CreateAllman()));
        var output = sw.ToString();
        Assert.Empty(outer.Members.OfType<TypeDeclaration>());
        Assert.Contains("yield return new WaitForSeconds(8f);", output);
        Assert.Contains("Debug.Log(\"Now Move On!!\");", output);
        Assert.DoesNotContain("class <afterFollow>d__115", output);
    }
}
