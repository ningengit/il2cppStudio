using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Eliminates redundant upcasts on reference types (e.g. <c>((Renderer)meshRenderer).SetMaterial(...)</c> or
/// <c>((TMP_Text)m_TextMeshPro).textInfo</c>) when the expression's static type already inherits from the cast target.
/// </summary>
public static class RedundantCastRemover
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var cast in AstWalk.Collect<CastExpression>(tree, SafeParentForUpcastRemoval))
        {
            if (cast.Parent is null) continue;
            var targetType = cast.Annotation<ResolveResult>()?.Type;
            var sourceType = cast.Expression.Annotation<ResolveResult>()?.Type;
            if (targetType is null || sourceType is null) continue;
            if (targetType.IsReferenceType != true || sourceType.IsReferenceType != true) continue;
            if (targetType.Kind == TypeKind.Interface || targetType.IsKnownType(KnownTypeCode.Object)) continue;
            if (!sourceType.GetAllBaseTypes().Any(b => b.Equals(targetType))) continue;
            if (!SafeParentForUpcastRemoval(cast)) continue;

            var inner = cast.Expression.Detach();
            if (cast.Parent is ParenthesizedExpression paren && paren.Parent is MemberReferenceExpression or IndexerExpression)
                paren.ReplaceWith(inner);
            else
                cast.ReplaceWith(inner);
        }
    }

    private static bool SafeParentForUpcastRemoval(CastExpression cast)
    {
        var p = cast.Parent is ParenthesizedExpression ? cast.Parent.Parent : cast.Parent;
        return p is MemberReferenceExpression or AssignmentExpression or VariableInitializer or ReturnStatement or IndexerExpression or ArrayInitializerExpression;
    }
}
