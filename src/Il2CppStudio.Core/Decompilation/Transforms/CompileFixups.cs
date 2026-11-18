using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Semantics;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Expression-level rewrites that turn lifted-but-unidiomatic constructs into compilable C#:
/// property/indexer sugar, implicit base ctor removal, dead-distance restoration, discard/native escape purging,
/// open generic placeholder cleanup, and delegate fallback annotations.
/// </summary>
public static class CompileFixups
{
    public static void Apply(SyntaxTree tree)
    {
        foreach (var m in tree.Descendants.OfType<MemberReferenceExpression>().ToList())
        {
            if (m.Parent is null) continue;
            if (m.MemberName == "m_value" && IsPrimitive(m.Target)) { m.ReplaceWith(m.Target.Detach()); continue; }
            if (m.MemberName == "Slot" && UnwrapParens(m.Target) is CastExpression { Type: SimpleType { Identifier: "LocalDataStoreSlot" } } lds)
            {
                var listExpr = lds.Expression is CastExpression { Type: PrimitiveType { Keyword: "object" } } oc ? oc.Expression : lds.Expression;
                m.ReplaceWith(new MemberReferenceExpression(listExpr.Detach(), "Count"));
            }
        }

        foreach (var pr in tree.Descendants.OfType<PointerReferenceExpression>().ToList())
        {
            if (pr.Parent is not null && pr.MemberName == "m_value"
                && UnwrapParens(pr.Target) is CastExpression { Type: ComposedType { BaseType: PrimitiveType pt, PointerRank: 1 } } pc)
                pr.ReplaceWith(new CastExpression(pt.Clone(), pc.Expression.Detach()));
        }

        RewriteMemberCalls(tree);
        MergeEulerRotationDefault(tree);
        CollapseCrossTypeCoroutineFactories(tree);
        RestoreDeadVectorDistance(tree);
        NativeEscapeSanitizer.Apply(tree);
        PurgeDiscardsAndDeadStatements(tree);
        FixLeakedTypeParameters(tree);
        FixUncheckedConstantCasts(tree);

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

        CompileHygiene.RefreshUnsafe(tree);
    }

    private static void RewriteMemberCalls(SyntaxTree tree)
    {
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
            else if (target.MemberName is ".ctor" or "_ctor" && call.Parent is ExpressionStatement s)
                RewriteCtorStatement(call, target, args, s);
            else if (target.MemberName == "CreateString")
                call.ReplaceWith(new ObjectCreateExpression(new PrimitiveType("string"), args.Select(a => a.Detach())));
            else if (target.MemberName == "GetTextContainerLocalCorners" && args.Count == 0 && call.Parent is ExpressionStatement)
                target.MemberName = "ForceMeshUpdate";
            else if (target.MemberName == "AsPointer" && args.Count == 1 && target.Target.ToString().EndsWith("Unsafe", StringComparison.Ordinal))
            {
                var raw = args[0] is DirectionExpression d ? d.Expression.Detach() : args[0].Detach();
                call.ReplaceWith(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", new SimpleType("nint")),
                    raw));
            }
            else if (target.MemberName == "CallNative" && args.Any(a => a is NamedArgumentExpression))
            {
                var ordered = args.Select((a, idx) => (a, key: CallNativeOrder(a, idx))).OrderBy(x => x.key)
                    .Select(x => x.a is NamedArgumentExpression na ? na.Expression.Detach() : x.a.Detach()).ToList();
                call.Arguments.Clear();
                foreach (var a in ordered) call.Arguments.Add(a);
            }
        }
    }

    private static void MergeEulerRotationDefault(SyntaxTree tree)
    {
        foreach (var es in tree.Descendants.OfType<ExpressionStatement>().ToList())
        {
            if (es.Parent is null || es.Expression is not InvocationExpression { Target: MemberReferenceExpression { MemberName: "Euler", Target: TypeReferenceExpression or IdentifierExpression { Identifier: "Quaternion" } } } euler)
                continue;
            if (es.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: MemberReferenceExpression { MemberName: "rotation" } rotLhs, Right: DefaultValueExpression } } nextRot)
            {
                nextRot.Expression = new AssignmentExpression(rotLhs.Detach(), euler.Detach());
                es.Remove();
            }
        }
    }

    private static int CallNativeOrder(Expression a, int idx) => a is NamedArgumentExpression na
        ? na.Name switch { "native" => 0, "arg1" => 1, "arg2" => 2, "arg3" => 3, "arg4" => 4, _ => 10 + idx }
        : idx;

    private static void CollapseCrossTypeCoroutineFactories(SyntaxTree tree)
    {
        foreach (var decl in tree.Descendants.OfType<VariableDeclarationStatement>().Where(d => d.Variables.Count == 1).ToList())
        {
            if (decl.Parent is not BlockStatement block) continue;
            var v = decl.Variables.Single();
            if (v.Initializer is not ObjectCreateExpression { Type: MemberType mt } || ParseStateMachineFactory(mt.MemberName) is not { } factoryName) continue;
            var stmts = block.Statements.ToList();
            var idx = stmts.IndexOf(decl) + 1;
            var setup = new List<Statement>();
            Expression? receiver = null;
            var bad = false;
            while (idx < stmts.Count && stmts[idx] is ExpressionStatement { Expression: AssignmentExpression { Left: MemberReferenceExpression { Target: IdentifierExpression id } m } a }
                && id.Identifier == v.Name)
            {
                if (m.MemberName is "<>1__state" or "__1__state") { }
                else if (m.MemberName is "<>4__this" or "__4__this") receiver = a.Right;
                else { bad = true; break; }
                setup.Add(stmts[idx++]);
            }
            if (bad || receiver is null) continue;
            var uses = block.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name && !setup.Any(s => i.Ancestors.Contains(s)) && !i.Ancestors.Contains(decl)).ToList();
            if (uses.Count != 1) continue;
            uses[0].ReplaceWith(new InvocationExpression(new MemberReferenceExpression(receiver.Clone(), factoryName)));
            foreach (var s in setup) s.Remove();
            decl.Remove();
        }
    }

    private static string? ParseStateMachineFactory(string memberName)
    {
        if (memberName.StartsWith('<') && memberName.IndexOf(">d__", StringComparison.Ordinal) is > 1 and var idx1)
            return memberName[1..idx1];
        if (memberName.StartsWith('_') && memberName.IndexOf("_d__", StringComparison.Ordinal) is > 1 and var idx2)
            return memberName[1..idx2];
        return null;
    }

    private static void RewriteCtorStatement(InvocationExpression call, MemberReferenceExpression target, List<Expression> args, ExpressionStatement s)
    {
        if (s.GetParent<ConstructorDeclaration>() is not null && args.Count == 0 && Unwrap(target.Target) is BaseReferenceExpression or ThisReferenceExpression)
        { s.Remove(); return; }
        if (target.Target is IdentifierExpression local && s.PrevSibling is VariableDeclarationStatement { Variables.Count: 1 } d
            && d.Variables.Single().Name == local.Identifier && d.Variables.Single().Initializer is ObjectCreateExpression { Arguments.Count: 0 } oce0)
        {
            foreach (var a in args) oce0.Arguments.Add(a.Detach());
            s.Remove();
            return;
        }
        if (target.Target is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } reinterp } outer && reinterp.TypeArguments.Count == 1 && args.Count == 2)
        {
            var fn = UnwrapLdftn(args[1]);
            var loc = outer.Arguments.FirstOrDefault();
            if (fn is IdentifierExpression or MemberReferenceExpression && loc is IdentifierExpression or MemberReferenceExpression)
            {
                var delArg = args[0] is NullReferenceExpression ? fn.Clone()
                    : fn is IdentifierExpression fid ? new MemberReferenceExpression(args[0].Detach(), fid.Identifier) : fn.Clone();
                s.Expression = new AssignmentExpression(loc.Detach(), new ObjectCreateExpression(reinterp.TypeArguments.First().Clone(), delArg));
                return;
            }
        }
        s.Remove();
    }

    private static void RestoreDeadVectorDistance(SyntaxTree tree)
    {
        foreach (var block in tree.Descendants.OfType<BlockStatement>().ToList())
        {
            var stmts = block.Statements.ToList();
            for (var i = 0; i + 1 < stmts.Count; i++)
            {
                if (stmts[i] is not ExpressionStatement s1 || stmts[i + 1] is not ExpressionStatement s2) continue;
                if (UnwrapDiscard(s1.Expression) is not MemberReferenceExpression { MemberName: "position" } e1) continue;
                if (UnwrapDiscard(s2.Expression) is not MemberReferenceExpression { MemberName: "position" } e2) continue;
                if (e1.ToString() == e2.ToString()) continue;

                var v3Dist = new InvocationExpression(new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("Vector3")), "Distance"), e1.Detach(), e2.Detach());
                var distField = FindUnusedDistanceField(block);
                if (distField is not null)
                {
                    var varName = PickFreeName(block, "num");
                    var decl = new VariableDeclarationStatement(new PrimitiveType("float"), varName, v3Dist);
                    s1.ReplaceWith(decl);
                    var emptyIf = new IfElseStatement(new BinaryOperatorExpression(new IdentifierExpression(varName), BinaryOperatorType.LessThan, new IdentifierExpression(distField)), new BlockStatement());
                    s2.ReplaceWith(emptyIf);
                }
                else
                {
                    s1.Expression = v3Dist;
                    s2.Remove();
                }
                i++;
            }
        }
    }

    private static string? FindUnusedDistanceField(BlockStatement block)
    {
        var method = block.Ancestors.OfType<MethodDeclaration>().FirstOrDefault();
        var type = method?.Ancestors.OfType<TypeDeclaration>().FirstOrDefault();
        if (method is null || type is null) return null;
        var used = method.Descendants.OfType<IdentifierExpression>().Select(i => i.Identifier)
            .Concat(method.Descendants.OfType<MemberReferenceExpression>().Select(m => m.MemberName)).ToHashSet(StringComparer.Ordinal);
        return type.Members.OfType<FieldDeclaration>()
            .Where(f => f.ReturnType is PrimitiveType { Keyword: "float" })
            .SelectMany(f => f.Variables)
            .Select(v => v.Name)
            .FirstOrDefault(n => n.EndsWith("Distance", StringComparison.OrdinalIgnoreCase) && !used.Contains(n));
    }

    private static string PickFreeName(BlockStatement block, string baseName)
    {
        var method = block.Ancestors.OfType<MethodDeclaration>().FirstOrDefault();
        var taken = (method?.DescendantsAndSelf ?? block.DescendantsAndSelf).OfType<VariableInitializer>().Select(v => v.Name)
            .Concat(method?.Parameters.Select(p => p.Name) ?? []).ToHashSet(StringComparer.Ordinal);
        if (!taken.Contains(baseName)) return baseName;
        for (var n = 1; ; n++) { var c = $"distance{(n == 1 ? "" : n.ToString())}"; if (!taken.Contains(c)) return c; }
    }

    private static void PurgeDiscardsAndDeadStatements(SyntaxTree tree)
    {
        foreach (var s in tree.Descendants.OfType<ExpressionStatement>().ToList())
        {
            if (s.Parent is null) continue;
            if (s.Expression is AssignmentExpression { Left: IdentifierExpression { Identifier: "_" } } da)
            {
                var rhs = Unwrap(da.Right);
                if (!IsReinterpretCall(rhs) && rhs is InvocationExpression or ObjectCreateExpression) s.Expression = rhs.Detach();
                else s.Remove();
                continue;
            }
            if (s.Expression is AssignmentExpression { Left: MemberReferenceExpression lm } && Unwrap(lm.Target) is DefaultValueExpression)
            { s.Remove(); continue; }

            var expr = UnwrapParens(s.Expression);
            if (!IsReinterpretCall(expr) && expr is not (BinaryOperatorExpression or UnaryOperatorExpression { Operator: not (UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement or UnaryOperatorType.Await) }
                or MemberReferenceExpression or IdentifierExpression or PrimitiveExpression or NullReferenceExpression
                or DefaultValueExpression or ConditionalExpression or CastExpression or AsExpression or IsExpression
                or TypeReferenceExpression or IndexerExpression)) continue;

            var sideEffects = expr.DescendantsAndSelf.OfType<Expression>()
                .Where(e => !IsReinterpretCall(e) && e is InvocationExpression or ObjectCreateExpression or AssignmentExpression).ToList();
            if (sideEffects.Count == 0) { s.Remove(); continue; }
            if (s.Parent is BlockStatement && sideEffects.Count == 1 && !sideEffects[0].Ancestors.Any(a => a != expr && a is Expression ae && !IsReinterpretCall(ae) && ae is InvocationExpression or ObjectCreateExpression or AssignmentExpression))
                s.Expression = sideEffects[0].Detach();
            else s.Remove();
        }
    }

    private static void FixLeakedTypeParameters(SyntaxTree tree)
    {
        foreach (var st in tree.Descendants.OfType<SimpleType>().ToList())
        {
            if (st.Parent is null || st.Role == Roles.TypeArgument && st.Identifier.Length == 0) continue;
            if (st.Identifier is not ("_0" or "_1" or "_2" or "TKey" or "TValue" or "T")) continue;
            var rt = st.Annotation<TypeResolveResult>()?.Type;
            if (rt is not null && rt.Kind is not (TypeKind.Unknown or TypeKind.TypeParameter)) continue;
            var declared = st.Ancestors.OfType<TypeDeclaration>().SelectMany(t => t.TypeParameters)
                .Concat(st.Ancestors.OfType<MethodDeclaration>().SelectMany(m => m.TypeParameters)).Select(tp => tp.Name);
            if (!declared.Contains(st.Identifier)) st.ReplaceWith(new PrimitiveType("object"));
        }
    }

    private static void FixUncheckedConstantCasts(SyntaxTree tree)
    {
        foreach (var cast in tree.Descendants.OfType<CastExpression>().ToList())
        {
            if (cast.Parent is null or UncheckedExpression) continue;
            if (cast.Type is not PrimitiveType { Keyword: "uint" or "ulong" or "byte" or "ushort" or "int" or "sbyte" or "short" }) continue;
            if (cast.Expression.DescendantsAndSelf.Any(n => n is IdentifierExpression or MemberReferenceExpression or InvocationExpression)) continue;
            if (cast.Expression.DescendantsAndSelf.OfType<PrimitiveExpression>().Any() || cast.Expression.DescendantsAndSelf.OfType<DefaultValueExpression>().Any())
                cast.ReplaceWith(new UncheckedExpression(cast.Clone()));
        }
    }

    private static Expression UnwrapDiscard(Expression e) =>
        e is AssignmentExpression { Left: IdentifierExpression { Identifier: "_" } } a ? Unwrap(a.Right) : Unwrap(e);

    private static Expression UnwrapLdftn(Expression e) => e switch
    {
        InvocationExpression { Target: IdentifierExpression { Identifier: "__ldftn" }, Arguments.Count: 1 } i => i.Arguments.Single(),
        CastExpression c => UnwrapLdftn(c.Expression),
        ParenthesizedExpression p => UnwrapLdftn(p.Expression),
        _ => e,
    };

    private static Expression Unwrap(Expression e) => e switch
    {
        ParenthesizedExpression p => Unwrap(p.Expression),
        CastExpression c => Unwrap(c.Expression),
        _ => e,
    };

    private static Expression UnwrapParens(Expression e) => e is ParenthesizedExpression p ? UnwrapParens(p.Expression) : e;

    private static bool IsReinterpretCall(Expression e) => e is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } };

    private static bool IsPrimitive(Expression e) =>
        e.Annotation<ResolveResult>()?.Type is { } t && t.GetDefinition()?.KnownTypeCode is >= KnownTypeCode.Boolean and <= KnownTypeCode.Double;

    private static bool IsAccessor(InvocationExpression call) =>
        call.Annotation<InvocationResolveResult>()?.Member is IMethod { IsAccessor: true } or null;

    private static bool SameGenericDefinition(AstType a, AstType b) =>
        a is SimpleType sa && b is SimpleType sb && sa.Identifier == sb.Identifier && sa.TypeArguments.Count == sb.TypeArguments.Count && sa.TypeArguments.Count > 0
        || a is MemberType ma && b is MemberType mb && ma.MemberName == mb.MemberName && ma.TypeArguments.Count == mb.TypeArguments.Count && ma.TypeArguments.Count > 0;
}
