using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

internal readonly record struct TreeFlags(
    bool HasNativeIntrinsics,
    bool HasCallNative,
    bool HasSqrt,
    bool HasYield,
    bool HasPointerSyntax,
    bool HasPointerField,
    bool HasGetEnumerator,
    bool HasUsingDeclarations,
    bool HasCompilerGeneratedTypes,
    bool HasBodies,
    bool HasStringEquals,
    bool HasVectorOps);

/// <summary>
/// Low-allocation pre-order AST traversal helpers walking <see cref="AstNode.FirstChild"/> and
/// <see cref="AstNode.NextSibling"/> directly without LINQ iterator or ToList overhead.
/// </summary>
internal static class AstWalk
{
    public static TreeFlags ScanFlags(SyntaxTree tree)
    {
        bool hasNativeIntrinsics = false, hasCallNative = false, hasSqrt = false, hasYield = false,
             hasPointerSyntax = false, hasPointerField = false, hasGetEnumerator = false,
             hasUsingDeclarations = false, hasCompilerGen = false, hasBodies = false,
             hasStringEquals = false, hasVectorOps = false;

        var stack = new Stack<AstNode>(64);
        stack.Push(tree);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            switch (n)
            {
                case UsingDeclaration:
                    hasUsingDeclarations = true;
                    break;
                case YieldReturnStatement or YieldBreakStatement:
                    hasYield = true;
                    break;
                case PointerReferenceExpression or FixedStatement:
                    hasPointerSyntax = true;
                    break;
                case ComposedType { PointerRank: > 0 }:
                    hasPointerSyntax = true;
                    break;
                case UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference or UnaryOperatorType.AddressOf }:
                    hasPointerSyntax = true;
                    break;
                case FieldDeclaration { ReturnType: ComposedType { PointerRank: > 0 } }:
                    hasPointerField = true;
                    hasPointerSyntax = true;
                    break;
                case IdentifierExpression id:
                    var idName = id.Identifier;
                    if (idName == "NativeIntrinsics") hasNativeIntrinsics = true;
                    else if (idName is "Vector2" or "Vector3" or "Vector4" or "Quaternion" or "Mathf" or "Color" or "Matrix4x4") hasVectorOps = true;
                    break;
                case SimpleType st:
                    if (st.Identifier == "NativeIntrinsics") hasNativeIntrinsics = true;
                    else if (st.Identifier is "Vector2" or "Vector3" or "Vector4" or "Quaternion" or "Mathf" or "Color" or "Matrix4x4") hasVectorOps = true;
                    break;
                case MemberType mt:
                    if (mt.MemberName == "NativeIntrinsics") hasNativeIntrinsics = true;
                    else if (mt.MemberName is "Vector2" or "Vector3" or "Vector4" or "Quaternion" or "Mathf" or "Color" or "Matrix4x4") hasVectorOps = true;
                    break;
                case MemberReferenceExpression mr:
                    switch (mr.MemberName)
                    {
                        case "CallNative":
                            hasCallNative = true;
                            hasNativeIntrinsics = true;
                            break;
                        case "Asm" or "Stmt" or "Reinterpret" or "NativeIntrinsics":
                            hasNativeIntrinsics = true;
                            break;
                        case "Sqrt": hasSqrt = true; break;
                        case "GetEnumerator": hasGetEnumerator = true; break;
                        case "Equals": hasStringEquals = true; break;
                        case "x" or "y" or "z" or "w" or "r" or "m00" or "sqrMagnitude" or "magnitude" or "normalized"
                            or "op_Addition" or "op_Subtraction" or "op_Multiply" or "op_Division":
                            hasVectorOps = true;
                            break;
                    }
                    break;
                case TypeDeclaration td:
                    if (td.Name.Contains("d__", StringComparison.Ordinal)
                        || td.Name.Contains(">d", StringComparison.Ordinal)
                        || td.Name.Contains("DisplayClass", StringComparison.Ordinal))
                        hasCompilerGen = true;
                    break;
                case BlockStatement bs when !bs.IsNull && bs.Statements.Count > 0
                    && bs.Parent is MethodDeclaration or ConstructorDeclaration or Accessor or DestructorDeclaration or OperatorDeclaration:
                    if (!hasBodies && !IsStubOnly(bs))
                        hasBodies = true;
                    break;
            }

            if (!hasPointerSyntax && n is Expression ex && ex.Annotation<ResolveResult>()?.Type.Kind == TypeKind.Pointer)
                hasPointerSyntax = true;

            // Push in reverse sibling order so FirstChild is visited first.
            var child = n.LastChild;
            while (child is not null)
            {
                stack.Push(child);
                child = child.PrevSibling;
            }
        }

        return new TreeFlags(
            hasNativeIntrinsics, hasCallNative, hasSqrt, hasYield,
            hasPointerSyntax, hasPointerField, hasGetEnumerator,
            hasUsingDeclarations, hasCompilerGen, hasBodies,
            hasStringEquals, hasVectorOps);
    }

    private static bool IsStubOnly(BlockStatement bs)
    {
        var first = bs.Statements.FirstOrDefault();
        if (first is null) return true;
        if (first != bs.Statements.LastOrDefault()) return false;
        if (first is ThrowStatement { Expression: NullReferenceExpression }) return true;
        return first is ReturnStatement r && (r.Expression.IsNull || r.Expression is NullReferenceExpression or DefaultValueExpression);
    }

    public static List<T> Collect<T>(AstNode root) where T : AstNode
    {
        var list = new List<T>();
        CollectInto(root, list);
        return list;
    }

    public static List<T> Collect<T>(AstNode root, Func<T, bool> predicate) where T : AstNode
    {
        var list = new List<T>();
        Collect(root, predicate, list);
        return list;
    }

    public static void CollectInto<T>(AstNode root, List<T> dest) where T : AstNode
    {
        var stack = new Stack<AstNode>(32);
        var c = root.LastChild;
        while (c is not null) { stack.Push(c); c = c.PrevSibling; }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is T match) dest.Add(match);
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
    }

    public static void Collect<T>(AstNode root, Func<T, bool> predicate, List<T> dest) where T : AstNode
    {
        var stack = new Stack<AstNode>(32);
        var c = root.LastChild;
        while (c is not null) { stack.Push(c); c = c.PrevSibling; }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is T match && predicate(match)) dest.Add(match);
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
    }

    public static bool HasAny<T>(AstNode root) where T : AstNode
    {
        var stack = new Stack<AstNode>(32);
        var c = root.LastChild;
        while (c is not null) { stack.Push(c); c = c.PrevSibling; }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is T) return true;
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
        return false;
    }

    public static bool HasAny(AstNode root, Func<AstNode, bool> predicate)
    {
        var stack = new Stack<AstNode>(32);
        var c = root.LastChild;
        while (c is not null) { stack.Push(c); c = c.PrevSibling; }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (predicate(n)) return true;
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
        return false;
    }

    public static void Visit<T>(AstNode root, Action<T> visitor) where T : AstNode
    {
        var stack = new Stack<AstNode>(32);
        var c = root.LastChild;
        while (c is not null) { stack.Push(c); c = c.PrevSibling; }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            // Snapshot children before visitor can mutate/replace n.
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
            if (n is T match) visitor(match);
        }
    }

    public static void ForEachNode(AstNode root, Action<AstNode> visitor)
    {
        var stack = new Stack<AstNode>(32);
        var c = root.LastChild;
        while (c is not null) { stack.Push(c); c = c.PrevSibling; }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
            visitor(n);
        }
    }

    public static bool IsAttachedTo(AstNode node, AstNode ancestor)
    {
        for (AstNode? a = node.Parent; a is not null; a = a.Parent)
            if (a == ancestor) return true;
        return false;
    }
}
