using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// <c>int num = (flag ? 1 : 0); … if (num != 0)</c> → <c>bool num = flag; … if (num)</c>. Applied only when the local is
/// never reassigned and every use sits in a 0/1 context (compared with zero, or an operand of a boolean bit operation),
/// so every rewritten use is exactly equivalent.
/// </summary>
public static class BoolLocalPromoter
{
    public static void Apply(SyntaxTree tree)
    {
        for (var pass = 0; pass < 4; pass++)
        {
            List<VariableDeclarationStatement>? decls = null;
            AstWalk.Visit<VariableDeclarationStatement>(tree, decl =>
            {
                if (decl.Parent is not null && decl.Variables.Count == 1 && decl.Type is PrimitiveType { Keyword: "int" or "uint" })
                {
                    var init = decl.Variables.Single().Initializer;
                    if (!init.IsNull && BoolNormalizer.TryBool(init) is not null)
                        (decls ??= []).Add(decl);
                }
            });
            if (decls is null) break;

            var changed = false;
            var scopeUses = new Dictionary<AstNode, Dictionary<string, List<IdentifierExpression>>>();
            foreach (var decl in decls)
            {
                if (!AstWalk.IsAttachedTo(decl, tree) || decl.Variables.Count != 1 || decl.Type is not PrimitiveType { Keyword: "int" or "uint" }) continue;
                var v = decl.Variables.Single();
                if (v.Initializer.IsNull || BoolNormalizer.TryBool(v.Initializer) is not { } value) continue;
                var scope = decl.Parent!;
                if (!scopeUses.TryGetValue(scope, out var byName))
                {
                    byName = new Dictionary<string, List<IdentifierExpression>>(StringComparer.Ordinal);
                    AstWalk.Visit<IdentifierExpression>(scope, i =>
                    {
                        if (!byName.TryGetValue(i.Identifier, out var list))
                            byName[i.Identifier] = list = [];
                        list.Add(i);
                    });
                    scopeUses[scope] = byName;
                }
                var uses = byName.TryGetValue(v.Name, out var rawUses)
                    ? rawUses.Where(u => AstWalk.IsAttachedTo(u, scope)).ToList()
                    : null;
                if (uses is null || uses.Count == 0 || !uses.All(u => IsBoolContext(u, scope))) continue;

                var originalWasUint = decl.Type is PrimitiveType { Keyword: "uint" };
                decl.Type = new PrimitiveType("bool");
                v.Initializer = value;
                foreach (var u in uses)
                {
                    if (!AstWalk.IsAttachedTo(u, scope)) continue;
                    var outer = Outer(u);
                    if (!AstWalk.IsAttachedTo(outer, scope)) continue;
                    // Keep the operand's integer type (uint casts stay uint) so surrounding arithmetic is unchanged.
                    var unsigned = outer is CastExpression { Type: PrimitiveType { Keyword: "uint" } } || originalWasUint;
                    var asInt = new ConditionalExpression(new IdentifierExpression(v.Name),
                        new PrimitiveExpression(unsigned ? 1u : (object)1), new PrimitiveExpression(unsigned ? 0u : (object)0));
                    outer.ReplaceWith(asInt);
                }
                changed = true;
            }
            if (!changed) break;
        }
    }

    /// <summary>The use including any integer casts around it.</summary>
    private static Expression Outer(Expression use)
    {
        Expression e = use;
        while (e.Parent is CastExpression { Type: PrimitiveType { Keyword: "uint" or "int" or "byte" } } or ParenthesizedExpression) e = (Expression)e.Parent;
        return e;
    }

    private static bool IsBoolContext(IdentifierExpression use, AstNode scope)
    {
        var e = Outer(use);
        return e.Parent switch
        {
            BinaryOperatorExpression { Operator: BinaryOperatorType.Equality or BinaryOperatorType.InEquality } b =>
                BoolNormalizer.IsZero(b.Left == e ? b.Right : b.Left),
            BinaryOperatorExpression { Operator: BinaryOperatorType.BitwiseAnd or BinaryOperatorType.BitwiseOr } b =>
                IsBoolSibling(b.Left == e ? b.Right : b.Left, scope),
            BinaryOperatorExpression { Operator: BinaryOperatorType.ExclusiveOr } b => b.Left == e && BoolNormalizer.IsOne(b.Right),
            _ => false,
        };
    }

    private static bool IsBoolSibling(Expression sib, AstNode scope)
    {
        if (BoolNormalizer.TryBool(sib) is not null) return true;
        while (sib is CastExpression { Type: PrimitiveType { Keyword: "uint" or "int" or "byte" or "bool" } } or ParenthesizedExpression)
            sib = sib is CastExpression c ? c.Expression : ((ParenthesizedExpression)sib).Expression;
        if (sib is not IdentifierExpression id) return false;
        VariableDeclarationStatement? decl = null;
        AstWalk.Visit<VariableDeclarationStatement>(scope, d =>
        {
            if (decl is null && d.Variables.Count == 1 && d.Variables.Single().Name == id.Identifier)
                decl = d;
        });
        if (decl is null) return false;
        if (decl.Type is PrimitiveType { Keyword: "bool" }) return true;
        if (decl.Type is not PrimitiveType { Keyword: "int" or "uint" }) return false;
        var v = decl.Variables.Single();
        if (v.Initializer.IsNull || BoolNormalizer.TryBool(v.Initializer) is null) return false;
        return !AstWalk.HasAny(scope, n => n is AssignmentExpression a && a.Left is IdentifierExpression w && w.Identifier == id.Identifier);
    }
}
