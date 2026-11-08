using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Expression-level rewrites that turn lifted-but-unidiomatic constructs into compilable C#, each exact:
/// <list type="bullet">
/// <item><c>num.m_value = x</c> / <c>num.m_value</c> on a primitive (inlined Int32/Single code) → <c>num = x</c> / <c>num</c>;</item>
/// <item><c>d.get_Item(k)</c> / <c>d.set_Item(k, v)</c> → <c>d[k]</c> / <c>d[k] = v</c>; other accessor calls → property syntax;</item>
/// <item><c>base..ctor();</c> inside a constructor (the implicit base call, inlined) → removed;</item>
/// <item>a value-only expression statement (<c>tag == "Notebook";</c>) → <c>_ = …;</c>;</item>
/// <item><c>(D&lt;string&gt;)(object)new D&lt;object&gt;(M)</c> (shared generic code) → <c>new D&lt;string&gt;(M)</c>;</item>
/// <item>unbound generic placeholders (<c>List&lt;&gt;</c>) → <c>List&lt;object&gt;</c>, the shared-code instantiation.</item>
/// </list>
/// </summary>
public static class CompileFixups
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var m in tree.Descendants.OfType<MemberReferenceExpression>().ToList())
            if (m.Parent is not null && m.MemberName == "m_value" && IsPrimitive(m.Target)) m.ReplaceWith(m.Target.Detach());

        foreach (var call in tree.Descendants.OfType<InvocationExpression>().ToList())
        {
            if (call.Parent is null || call.Target is not MemberReferenceExpression target) continue;
            var args = call.Arguments.ToList();
            if (target.MemberName == "get_Item" && args.Count >= 1)
                call.ReplaceWith(new IndexerExpression(target.Target.Detach(), args.Select(a => a.Detach())));
            else if (target.MemberName == "set_Item" && args.Count >= 2)
                call.ReplaceWith(new AssignmentExpression(new IndexerExpression(target.Target.Detach(), args.Take(args.Count - 1).Select(a => a.Detach())), args[^1].Detach()));
            else if (target.MemberName.StartsWith("get_", StringComparison.Ordinal) && args.Count == 0 && IsAccessor(call))
                call.ReplaceWith(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]));
            else if (target.MemberName.StartsWith("set_", StringComparison.Ordinal) && args.Count == 1 && IsAccessor(call))
                call.ReplaceWith(new AssignmentExpression(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]), args[0].Detach()));
            else if (target.MemberName == ".ctor" && call.Parent is ExpressionStatement s && s.GetParent<ConstructorDeclaration>() is not null && args.Count == 0
                     && Unwrap(target.Target) is BaseReferenceExpression or ThisReferenceExpression)
                s.Remove();   // implicit base-constructor call (inlined), possibly through a cast of `this`
            else if (target.MemberName == ".ctor" && call.Parent is ExpressionStatement s2 && target.Target is IdentifierExpression local
                     && s2.PrevSibling is VariableDeclarationStatement { Variables.Count: 1 } d && d.Variables.Single().Name == local.Identifier
                     && d.Variables.Single().Initializer is ObjectCreateExpression { Arguments.Count: 0 } oce0)
            {
                foreach (var a in args) oce0.Arguments.Add(a.Detach());   // `T x = new T(); x..ctor(a)` → `T x = new T(a)`
                s2.Remove();
            }
        }

        foreach (var s in tree.Descendants.OfType<ExpressionStatement>().ToList())
            if (s.Expression is BinaryOperatorExpression or UnaryOperatorExpression { Operator: UnaryOperatorType.Not or UnaryOperatorType.Minus }
                or MemberReferenceExpression or IdentifierExpression or PrimitiveExpression or ConditionalExpression or CastExpression)
                s.Expression = new AssignmentExpression(new IdentifierExpression("_"), s.Expression.Detach());

        foreach (var cast in tree.Descendants.OfType<CastExpression>().ToList())
            if (cast.Parent is not null && cast.Expression is CastExpression { Type: PrimitiveType { Keyword: "object" }, Expression: ObjectCreateExpression oce }
                && SameGenericDefinition(cast.Type, oce.Type) && oce.Arguments.Count == 1)
                cast.ReplaceWith(new ObjectCreateExpression(cast.Type.Clone(), oce.Arguments.Single().Detach()));

        foreach (var st in tree.Descendants.OfType<SimpleType>().ToList())
            if (st.Parent is not null && st.Identifier.Length == 0 && st.Role == Roles.TypeArgument) st.ReplaceWith(new PrimitiveType("object"));

        // new D(null, (IntPtr)0): the delegate's target method could not be recovered. Keep it compilable and explicit.
        foreach (var oce in tree.Descendants.OfType<ObjectCreateExpression>().ToList())
            if (oce.Parent is not null && oce.Arguments.Count == 2 && oce.Arguments.Last() is CastExpression { Expression: PrimitiveExpression { Value: 0 } }
                && oce.Arguments.First() is NullReferenceExpression)
                oce.ReplaceWith(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", oce.Type.Clone()),
                    new PrimitiveExpression("unresolved delegate target")));
    }

    private static Expression Unwrap(Expression e) => e switch
    {
        ParenthesizedExpression p => Unwrap(p.Expression),
        CastExpression c => Unwrap(c.Expression),
        _ => e,
    };

    private static bool IsPrimitive(Expression e) =>
        e.Annotation<ResolveResult>()?.Type is { } t && t.GetDefinition()?.KnownTypeCode is >= KnownTypeCode.Boolean and <= KnownTypeCode.Double;

    private static bool IsAccessor(InvocationExpression call) =>
        call.Annotation<InvocationResolveResult>()?.Member is IMethod { IsAccessor: true } or null;

    private static bool SameGenericDefinition(AstType a, AstType b) =>
        a is SimpleType sa && b is SimpleType sb && sa.Identifier == sb.Identifier && sa.TypeArguments.Count == sb.TypeArguments.Count && sa.TypeArguments.Count > 0
        || a is MemberType ma && b is MemberType mb && ma.MemberName == mb.MemberName && ma.TypeArguments.Count == mb.TypeArguments.Count && ma.TypeArguments.Count > 0;
}
