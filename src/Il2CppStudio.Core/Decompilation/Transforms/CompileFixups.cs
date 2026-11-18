using ICSharpCode.Decompiler.CSharp;
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
            if (m.MemberName == "value__")
            {
                if (m.Parent is AssignmentExpression a && a.Left == m) m.ReplaceWith(m.Target.Detach());
                else m.ReplaceWith(new CastExpression(new PrimitiveType("int"), m.Target.Detach()));
                continue;
            }
            if (m.MemberName == "Value" && IsNonNullableValueTarget(m.Target))
            {
                m.ReplaceWith(m.Target.Detach());
                continue;
            }
            if (m.MemberName == "Slot" && UnwrapParens(m.Target) is CastExpression { Type: SimpleType { Identifier: "LocalDataStoreSlot" } } lds)
            {
                var listExpr = lds.Expression is CastExpression { Type: PrimitiveType { Keyword: "object" } } oc ? oc.Expression : lds.Expression;
                m.ReplaceWith(new MemberReferenceExpression(listExpr.Detach(), "Count"));
            }
        }

        foreach (var pr in tree.Descendants.OfType<PointerReferenceExpression>().ToList())
        {
            if (pr.Parent is null) continue;
            if (pr.MemberName == "m_value"
                && UnwrapParens(pr.Target) is CastExpression { Type: ComposedType { BaseType: PrimitiveType pt, PointerRank: 1 } } pc)
                pr.ReplaceWith(new CastExpression(pt.Clone(), pc.Expression.Detach()));
            else if (pr.MemberName == "value__")
            {
                var inner = UnwrapParens(pr.Target) is CastExpression { Type: ComposedType { PointerRank: 1 } } pc2 ? pc2.Expression : pr.Target;
                if (pr.Parent is AssignmentExpression pa && pa.Left == pr) pr.ReplaceWith(inner.Detach());
                else pr.ReplaceWith(new CastExpression(new PrimitiveType("int"), inner.Detach()));
            }
        }

        RewriteMemberCalls(tree);
        MergeEulerRotationDefault(tree);
        CollapseCrossTypeCoroutineFactories(tree);
        RestoreDeadVectorDistance(tree);
        NativeEscapeSanitizer.Apply(tree);
        PurgeDiscardsAndDeadStatements(tree);
        FixLeakedTypeParameters(tree);
        FixNegativeArraySizes(tree);
        FixUncheckedConstantCasts(tree);

        foreach (var cast in tree.Descendants.OfType<CastExpression>().ToList())
            if (cast.Parent is not null && cast.Expression is CastExpression { Type: PrimitiveType { Keyword: "object" }, Expression: ObjectCreateExpression oce }
                && SameGenericDefinition(cast.Type, oce.Type) && oce.Arguments.Count == 1)
                cast.ReplaceWith(new ObjectCreateExpression(cast.Type.Clone(), oce.Arguments.Single().Detach()));

        foreach (var st in tree.Descendants.OfType<SimpleType>().ToList())
            if (st.Parent is not null && st.Identifier.Length == 0 && st.Role == Roles.TypeArgument)
                st.ReplaceWith(ReplacementForTypeArgument(st));

        // new D(target, (IntPtr)0) / new D(target, default(nint)): the delegate's target method could not be recovered. Keep it compilable and explicit.
        foreach (var oce in tree.Descendants.OfType<ObjectCreateExpression>().ToList())
            if (oce.Parent is not null && oce.Arguments.Count == 2 && IsUnresolvedDelegatePtr(oce.Arguments.First(), oce.Arguments.Last()))
                oce.ReplaceWith(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", oce.Type.Clone()),
                    new PrimitiveExpression("unresolved delegate target")));

        foreach (var dir in tree.Descendants.OfType<DirectionExpression>().ToList())
            if (dir.Parent is ParenthesizedExpression or CastExpression or BinaryOperatorExpression or UnaryOperatorExpression)
                dir.ReplaceWith(dir.Expression.Detach());

        FixRemainingCompilePatterns(tree);
        CompileHygiene.RefreshUnsafe(tree);
    }

    private static void RewriteMemberCalls(SyntaxTree tree)
    {
        foreach (var call in tree.Descendants.OfType<InvocationExpression>().ToList())
        {
            if (call.Parent is null) continue;
            var rawMemberName = (call.Target as MemberReferenceExpression)?.MemberName
                ?? (call.Target as PointerReferenceExpression)?.MemberName;
            if (rawMemberName is not null && rawMemberName.Contains("IStructuralEquatable", StringComparison.Ordinal))
            {
                if (call.Parent is ExpressionStatement es) es.Remove();
                else if (rawMemberName.EndsWith("Equals", StringComparison.Ordinal)) call.ReplaceWith(new PrimitiveExpression(false));
                else call.ReplaceWith(new PrimitiveExpression(0));
                continue;
            }
            if (call.Target is not MemberReferenceExpression target) continue;
            var args = call.Arguments.ToList();
            if (target.MemberName == "get_Item" && args.Count >= 1 && call.Annotation<InvocationResolveResult>()?.Member is IMethod { IsAccessor: true })
                call.ReplaceWith(new IndexerExpression(target.Target.Detach(), args.Select(a => a.Detach())));
            else if (target.MemberName == "set_Item" && args.Count >= 2 && call.Annotation<InvocationResolveResult>()?.Member is IMethod { IsAccessor: true })
                call.ReplaceWith(new AssignmentExpression(new IndexerExpression(target.Target.Detach(), args.Take(args.Count - 1).Select(a => a.Detach())), args[^1].Detach()));
            else if (target.MemberName.StartsWith("get_", StringComparison.Ordinal) && args.Count == 0 && IsAccessor(call))
                call.ReplaceWith(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]));
            else if (target.MemberName.StartsWith("set_", StringComparison.Ordinal) && args.Count == 1 && IsAccessor(call))
                call.ReplaceWith(new AssignmentExpression(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]), args[0].Detach()));
            else if (target.MemberName.StartsWith("add_", StringComparison.Ordinal) && args.Count == 1 && IsAccessor(call))
                call.ReplaceWith(new AssignmentExpression(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]), AssignmentOperatorType.Add, args[0].Detach()));
            else if (target.MemberName.StartsWith("remove_", StringComparison.Ordinal) && args.Count == 1 && IsAccessor(call))
                call.ReplaceWith(new AssignmentExpression(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[7..]), AssignmentOperatorType.Subtract, args[0].Detach()));
            else if (target.MemberName is ".ctor" or "_ctor" && call.Parent is ExpressionStatement s)
                RewriteCtorStatement(call, target, args, s);
            else if (target.MemberName == "CreateString")
                call.ReplaceWith(new ObjectCreateExpression(new PrimitiveType("string"), args.Select(a => a.Detach())));
            else if (target.MemberName == "GetTextContainerLocalCorners" && args.Count == 0 && call.Parent is ExpressionStatement)
                target.MemberName = "ForceMeshUpdate";
            else if (target.MemberName == "ComputeStringHash" && args.Count == 1)
                call.ReplaceWith(new CastExpression(new PrimitiveType("uint"), new InvocationExpression(new MemberReferenceExpression(args[0].Detach(), "GetHashCode"))));
            else if (target.MemberName == "AsPointer" && args.Count == 1 && target.Target.ToString().EndsWith("Unsafe", StringComparison.Ordinal))
            {
                var raw = args[0] is DirectionExpression d ? d.Expression.Detach() : args[0].Detach();
                AstNode anchor = call;
                while (anchor.Parent is ParenthesizedExpression paren) anchor = paren;
                while (anchor.Parent is CastExpression midCast && midCast.Type.ToString() is "nint" or "IntPtr" or "System.IntPtr")
                {
                    AstNode next = midCast;
                    while (next.Parent is ParenthesizedExpression mp) next = mp;
                    if (next.Parent is CastExpression outer && outer.Type is not ComposedType { PointerRank: > 0 })
                        anchor = next;
                    else
                        break;
                }
                if (anchor.Parent is CastExpression outerCast && outerCast.Type is not ComposedType { PointerRank: > 0 })
                {
                    var reinterpType = outerCast.Type.Clone();
                    outerCast.ReplaceWith(new InvocationExpression(
                        new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                            "Reinterpret", reinterpType),
                        raw));
                }
                else
                {
                    call.ReplaceWith(new InvocationExpression(
                        new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                            "Reinterpret", new SimpleType("nint")),
                        raw));
                }
            }
            else if (target.MemberName == "As" && target.TypeArguments.Count == 2 && args.Count == 1
                && target.Target.ToString().EndsWith("Unsafe", StringComparison.Ordinal)
                && args[0] is DirectionExpression { Expression: var asInner }
                && UnwrapParens(asInner) is not (IdentifierExpression or MemberReferenceExpression or IndexerExpression or PointerReferenceExpression or UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference }))
            {
                call.ReplaceWith(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", target.TypeArguments.Last().Clone()),
                    asInner.Detach()));
            }
            else if (target.MemberName == "Reinterpret" && args.Count == 1 && target.TypeArguments.Count == 1
                && target.TypeArguments.First() is ComposedType { PointerRank: > 0 } ptrT)
            {
                call.ReplaceWith(new CastExpression(ptrT.Clone(), new InvocationExpression(
                    new MemberReferenceExpression(target.Target.Detach(), "Reinterpret", new SimpleType("nint")),
                    args[0].Detach())));
            }
            else if (target.MemberName == "Write" && args.Count == 2 && target.Target.ToString().EndsWith("Unsafe", StringComparison.Ordinal))
            {
                if (args[0] is UnaryOperatorExpression { Operator: UnaryOperatorType.AddressOf, Expression: var lhs })
                {
                    if (args[1] is NullReferenceExpression && call.Parent is ExpressionStatement s0) s0.Remove();
                    else call.ReplaceWith(new AssignmentExpression(lhs.Detach(), args[1].Detach()));
                }
                else if (call.Parent is ExpressionStatement s1)
                {
                    if (args[1] is InvocationExpression or ObjectCreateExpression) s1.Expression = args[1].Detach();
                    else s1.Remove();
                }
                else call.ReplaceWith(args[1].Detach());
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
            if (call.Annotation<InvocationResolveResult>()?.Member is IMethod ctorM
                && oce0.Annotation<ResolveResult>()?.Type.GetDefinition() is { } createdTd
                && ctorM.DeclaringTypeDefinition is { } ctorTd
                && !Equals(ctorTd, createdTd))
            {
                s.Remove();
                return;
            }
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
                    : fn is IdentifierExpression fid ? new MemberReferenceExpression(args[0].Detach(), fid.Identifier)
                    : fn is MemberReferenceExpression fmr && fmr.Target is TypeReferenceExpression ? new MemberReferenceExpression(args[0].Detach(), fmr.MemberName)
                    : fn.Clone();
                Expression rhs = new ObjectCreateExpression(reinterp.TypeArguments.First().Clone(), delArg);
                var locType = loc.Annotation<ResolveResult>()?.Type;
                if (locType is not null && locType.Kind != TypeKind.Delegate && !locType.IsKnownType(KnownTypeCode.Object))
                {
                    var astBuilder = new TypeSystemAstBuilder();
                    rhs = new InvocationExpression(
                        new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                            "Reinterpret", astBuilder.ConvertType(locType)),
                        rhs);
                }
                s.Expression = new AssignmentExpression(loc.Detach(), rhs);
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
            if (s.Expression is InvocationExpression { Arguments.Count: 0, Target: IdentifierExpression { Identifier: "Finalize" } or MemberReferenceExpression { MemberName: "Finalize" } })
            { s.Remove(); continue; }
            if (s.Expression is InvocationExpression { Target: PointerReferenceExpression ptrTarget } && ptrTarget.MemberName.Contains("IStructuralEquatable", StringComparison.Ordinal))
            { s.Remove(); continue; }
            if (IsDelegateCreation(Unwrap(s.Expression)))
            { s.Remove(); continue; }
            if (s.Expression is AssignmentExpression { Left: IdentifierExpression { Identifier: "_" } } da)
            {
                var rhs = Unwrap(da.Right);
                if (rhs is InvocationExpression { Target: PointerReferenceExpression pt } && pt.MemberName.Contains("IStructuralEquatable", StringComparison.Ordinal))
                    s.Remove();
                else if (!IsReinterpretCall(rhs) && rhs is InvocationExpression or ObjectCreateExpression && !IsDelegateCreation(rhs))
                    s.Expression = rhs.Detach();
                else s.Remove();
                continue;
            }
            if (s.Expression is AssignmentExpression { Left: MemberReferenceExpression lm } && Unwrap(lm.Target) is DefaultValueExpression)
            { s.Remove(); continue; }
            if (s.Expression is AssignmentExpression { Left: IndexerExpression li } && Unwrap(li.Target) is DefaultValueExpression)
            { s.Remove(); continue; }

            var expr = UnwrapParens(s.Expression);
            if (!IsReinterpretCall(expr) && expr is not (BinaryOperatorExpression or UnaryOperatorExpression { Operator: not (UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement or UnaryOperatorType.Await) }
                or MemberReferenceExpression or IdentifierExpression or PrimitiveExpression or NullReferenceExpression
                or DefaultValueExpression or ConditionalExpression or CastExpression or AsExpression or IsExpression
                or TypeReferenceExpression or IndexerExpression)) continue;

            var sideEffects = expr.DescendantsAndSelf.OfType<Expression>()
                .Where(e => !IsReinterpretCall(e) && e is InvocationExpression or ObjectCreateExpression or AssignmentExpression).ToList();
            if (sideEffects.Count == 0) { s.Remove(); continue; }
            if (s.Parent is BlockStatement && sideEffects.Count == 1 && !sideEffects[0].Ancestors.Any(a => a != expr && a is Expression ae && !IsReinterpretCall(ae) && ae is InvocationExpression or ObjectCreateExpression or AssignmentExpression)
                && !IsDelegateCreation(sideEffects[0]))
                s.Expression = sideEffects[0].Detach();
            else s.Remove();
        }
    }

    private static void FixLeakedTypeParameters(SyntaxTree tree)
    {
        foreach (var st in tree.Descendants.OfType<SimpleType>().ToList())
        {
            if (st.Parent is null || st.Role == Roles.TypeArgument && st.Identifier.Length == 0) continue;
            if (!(st.Identifier.StartsWith('!') || st.Identifier is "_0" or "_1" or "_2" or "_3" or "__0" or "__1" or "__2" or "__3" or "TKey" or "TValue" or "T")) continue;
            var rt = st.Annotation<TypeResolveResult>()?.Type;
            if (rt is not null && rt.Kind is not (TypeKind.Unknown or TypeKind.TypeParameter)) continue;
            var declared = st.Ancestors.OfType<TypeDeclaration>().SelectMany(t => t.TypeParameters)
                .Concat(st.Ancestors.OfType<MethodDeclaration>().SelectMany(m => m.TypeParameters))
                .Concat(st.Ancestors.OfType<DelegateDeclaration>().SelectMany(d => d.TypeParameters))
                .Select(tp => tp.Name);
            if (!declared.Contains(st.Identifier))
                st.ReplaceWith(st.Role == Roles.TypeArgument ? ReplacementForTypeArgument(st) : new PrimitiveType("object"));
        }
    }

    private static void FixNegativeArraySizes(SyntaxTree tree)
    {
        foreach (var ace in tree.Descendants.OfType<ArrayCreateExpression>().ToList())
        {
            foreach (var arg in ace.Arguments.ToList())
            {
                if (!arg.DescendantsAndSelf.Any(n => n is IdentifierExpression or MemberReferenceExpression or InvocationExpression)
                    && TryEvalConstInt(arg, out var len) && len < 0)
                    arg.ReplaceWith(new PrimitiveExpression(0));
            }
        }
    }

    private static void FixUncheckedConstantCasts(SyntaxTree tree)
    {
        foreach (var cast in tree.Descendants.OfType<CastExpression>().Reverse().ToList())
        {
            if (cast.Parent is null or UncheckedExpression || cast.Ancestors.OfType<UncheckedExpression>().Any()) continue;
            if (cast.Type is not PrimitiveType { Keyword: "uint" or "ulong" or "byte" or "ushort" or "int" or "sbyte" or "short" }) continue;
            if (cast.Expression.DescendantsAndSelf.Any(n => n is IdentifierExpression or MemberReferenceExpression or InvocationExpression)) continue;
            if (cast.Expression.DescendantsAndSelf.OfType<PrimitiveExpression>().Any() || cast.Expression.DescendantsAndSelf.OfType<DefaultValueExpression>().Any())
                cast.ReplaceWith(new UncheckedExpression(cast.Clone()));
        }

        foreach (var bin in tree.Descendants.OfType<BinaryOperatorExpression>().Reverse().ToList())
        {
            if (bin.Parent is null or BinaryOperatorExpression or ParenthesizedExpression or CastExpression or UncheckedExpression) continue;
            if (bin.Ancestors.OfType<UncheckedExpression>().Any()) continue;
            if (!bin.DescendantsAndSelf.OfType<DefaultValueExpression>().Any()) continue;
            if (bin.DescendantsAndSelf.Any(n => n is IdentifierExpression or MemberReferenceExpression or InvocationExpression)) continue;
            bin.ReplaceWith(new UncheckedExpression(bin.Clone()));
        }
    }

    private static void FixRemainingCompilePatterns(SyntaxTree tree)
    {
        foreach (var u in tree.Descendants.OfType<UnaryOperatorExpression>().Where(u => u.Operator == UnaryOperatorType.Dereference).ToList())
        {
            if (u.Parent is null || UnwrapParens(u.Expression) is not CastExpression { Type: ComposedType { PointerRank: > 0, BaseType: SimpleType bst } }) continue;
            if (bst.Identifier == "_" || bst.Identifier.Length == 0 || IdentifierEscaper.Escape(bst.Identifier) == "_")
                u.ReplaceWith(new PrimitiveExpression(0));
        }

        foreach (var ct in tree.Descendants.OfType<ComposedType>().ToList())
            if (ct.PointerRank > 0 && ct.BaseType is SimpleType bst && (bst.Identifier == "_" || bst.Identifier.Length == 0 || IdentifierEscaper.Escape(bst.Identifier) == "_"))
                ct.BaseType = new PrimitiveType("void");

        foreach (var oce in tree.Descendants.OfType<ObjectCreateExpression>().ToList())
        {
            if (oce.Parent is null) continue;
            var def = oce.Annotation<ResolveResult>()?.Type.GetDefinition();
            var isAbstract = def?.IsAbstract == true || IsAbstractTypeInTree(tree, oce.Type);
            if (!isAbstract) continue;
            AstNode a = oce;
            while (a.Parent is ParenthesizedExpression paren) a = paren;
            if (a.Parent is CastExpression outerCast && oce.Arguments.Count == 0
                && outerCast.Annotation<ResolveResult>()?.Type is { } outerT
                && outerT.GetDefinition() is { IsAbstract: false }
                && outerT.GetConstructors().Any(c => !c.IsStatic && c.Accessibility != Accessibility.Private && c.Parameters.All(p => p.IsOptional)))
            {
                outerCast.ReplaceWith(new ObjectCreateExpression(outerCast.Type.Clone()));
            }
            else
            {
                oce.ReplaceWith(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", oce.Type.Clone()),
                    new ObjectCreateExpression(new PrimitiveType("object"))));
            }
        }

        var astBuilder = new TypeSystemAstBuilder();
        foreach (var ctor in tree.Descendants.OfType<ConstructorDeclaration>().ToList())
        {
            if (ctor.Modifiers.HasFlag(Modifiers.Static) || !ctor.Initializer.IsNull) continue;
            var td = ctor.GetParent<TypeDeclaration>();
            var resolvedType = td?.Annotation<TypeResolveResult>()?.Type ?? td?.GetSymbol() as IType;
            var baseType = resolvedType?.DirectBaseTypes.FirstOrDefault(b => b.Kind == TypeKind.Class && !b.IsKnownType(KnownTypeCode.Object));
            if (baseType is null) continue;
            var ctors = baseType.GetConstructors().Where(c => !c.IsStatic).ToList();
            if (ctors.Count == 0 || ctors.Any(c => c.Accessibility != Accessibility.Private && c.Parameters.All(p => p.IsOptional))) continue;
            var chosen = ctors
                .Where(c => c.Accessibility != Accessibility.Private && c.Parameters.All(p => p.ReferenceKind == ReferenceKind.None))
                .OrderBy(c => c.Parameters.Count)
                .FirstOrDefault()
                ?? ctors.Where(c => c.Accessibility != Accessibility.Private).OrderBy(c => c.Parameters.Count).FirstOrDefault();
            if (chosen is null || chosen.Parameters.Count == 0) continue;
            var init = new ConstructorInitializer { ConstructorInitializerType = ConstructorInitializerType.Base };
            var ok = true;
            foreach (var p in chosen.Parameters)
            {
                if (p.ReferenceKind is ReferenceKind.Out or ReferenceKind.Ref) { ok = false; break; }
                var pt = p.Type is ByReferenceType br ? br.ElementType : p.Type;
                init.Arguments.Add(new DefaultValueExpression(astBuilder.ConvertType(pt)));
            }
            if (ok) ctor.Initializer = init;
        }

        foreach (var ci in tree.Descendants.OfType<ConstructorInitializer>().Where(c => c.ConstructorInitializerType == ConstructorInitializerType.Base).ToList())
        {
            if (ci.Arguments.Count == 0 || ci.Parent is not ConstructorDeclaration cd) continue;
            var td = cd.GetParent<TypeDeclaration>();
            var resolvedType = td?.Annotation<TypeResolveResult>()?.Type ?? td?.GetSymbol() as IType;
            var baseType = resolvedType?.DirectBaseTypes.FirstOrDefault(b => b.Kind == TypeKind.Class && !b.IsKnownType(KnownTypeCode.Object));
            var matchingCtors = baseType?.GetConstructors()
                .Where(c => !c.IsStatic && c.Accessibility != Accessibility.Private && c.Parameters.Count == ci.Arguments.Count)
                .ToList();
            if (matchingCtors is null || matchingCtors.Count == 0) continue;
            var argsList = ci.Arguments.ToList();
            for (var i = 0; i < argsList.Count; i++)
            {
                if (argsList[i] is not CastExpression ce) continue;
                var innerT = ce.Expression.Annotation<ResolveResult>()?.Type;
                var castT = ce.Annotation<ResolveResult>()?.Type ?? ce.Type.Annotation<TypeResolveResult>()?.Type;
                if (innerT is null || castT is null || !IsStrictDerivedType(innerT, castT)) continue;
                var unwrapAll = matchingCtors.All(c =>
                {
                    var pt = c.Parameters[i].Type;
                    return (innerT.Equals(pt) || IsStrictDerivedType(innerT, pt)) && IsStrictDerivedType(pt, castT);
                });
                if (unwrapAll) ce.ReplaceWith(ce.Expression.Detach());
            }
        }

        foreach (var cast in tree.Descendants.OfType<CastExpression>().ToList())
        {
            if (cast.Parent is null) continue;
            var ct = cast.Annotation<ResolveResult>()?.Type ?? cast.Type.Annotation<TypeResolveResult>()?.Type;
            if (cast.Expression is NullReferenceExpression)
            {
                var isValueTypeCast = (ct is { IsReferenceType: false } && ct.FullName != "System.Nullable" && cast.Type is not ComposedType { HasNullableSpecifier: true })
                    || cast.Type is PrimitiveType { Keyword: not ("string" or "object") } or SimpleType { Identifier: "nint" or "nuint" };
                if (isValueTypeCast)
                {
                    cast.ReplaceWith(new DefaultValueExpression(cast.Type.Clone()));
                    continue;
                }
            }
            if (UnwrapParens(cast.Expression) is PrimitiveExpression { Value: 0 or 0L }
                && ct is { Kind: TypeKind.Struct }
                && ct.GetDefinition()?.KnownTypeCode is null or KnownTypeCode.None
                && ct.FullName != "System.Nullable")
            {
                cast.ReplaceWith(new DefaultValueExpression(cast.Type.Clone()));
            }
        }

        foreach (var ae in tree.Descendants.OfType<AsExpression>().ToList())
        {
            if (ae.Parent is null) continue;
            var srcT = ae.Expression.Annotation<ResolveResult>()?.Type
                ?? (ae.Expression is ThisReferenceExpression ? ae.Ancestors.OfType<TypeDeclaration>().FirstOrDefault()?.GetSymbol() as IType : null);
            var dstT = ae.Type.Annotation<TypeResolveResult>()?.Type;
            if (srcT is { Kind: TypeKind.Class } && dstT is { Kind: TypeKind.Class }
                && !srcT.IsKnownType(KnownTypeCode.Object) && !dstT.IsKnownType(KnownTypeCode.Object)
                && !srcT.Equals(dstT) && !IsStrictDerivedType(srcT, dstT) && !IsStrictDerivedType(dstT, srcT))
            {
                ae.Expression = new CastExpression(new PrimitiveType("object"), ae.Expression.Detach());
            }
        }

        foreach (var assign in tree.Descendants.OfType<AssignmentExpression>().ToList())
        {
            if (assign.Parent is null || assign.Left is not MemberReferenceExpression mr || mr.Target is BaseReferenceExpression) continue;
            var targetType = mr.Target.Annotation<ResolveResult>()?.Type;
            var targetTd = targetType?.GetDefinition();
            if (targetType is null || targetTd is null) continue;
            if (!targetTd.Properties.Any(p => p.Name == mr.MemberName && !p.CanSet && !p.IsStatic)) continue;
            var baseOwner = targetType.GetAllBaseTypes()
                .Where(b => b.Kind == TypeKind.Class && b.GetDefinition() is { } bd && !bd.Equals(targetTd))
                .FirstOrDefault(b => b.GetDefinition()!.Fields.Any(f => !f.IsStatic && f.Name == mr.MemberName)
                    || b.GetDefinition()!.Properties.Any(p => !p.IsStatic && p.CanSet && p.Name == mr.MemberName));
            if (baseOwner is not null)
                mr.Target = new ParenthesizedExpression(new CastExpression(astBuilder.ConvertType(baseOwner), mr.Target.Detach()));
        }

        foreach (var inv in tree.Descendants.OfType<InvocationExpression>().ToList())
        {
            if (inv.Parent is null) continue;
            var isNullTarget = UnwrapParens(inv.Target) is NullReferenceExpression;
            var isNonDelegateTarget = !isNullTarget
                && inv.Target is IdentifierExpression or MemberReferenceExpression
                && inv.Annotation<InvocationResolveResult>() is null
                && inv.Target.Annotation<ResolveResult>()?.Type is { } tgtT
                && tgtT.Kind is TypeKind.Class or TypeKind.Struct or TypeKind.Enum or TypeKind.Array or TypeKind.Interface
                && tgtT.FullName is not ("System.Delegate" or "System.MulticastDelegate");
            if (isNullTarget || isNonDelegateTarget)
            {
                if (inv.Parent is ExpressionStatement es)
                    es.Remove();
                else if (inv.Annotation<ResolveResult>()?.Type is { Kind: not TypeKind.Unknown } rt && !rt.IsKnownType(KnownTypeCode.Void))
                    inv.ReplaceWith(new DefaultValueExpression(astBuilder.ConvertType(rt)));
                else if (inv.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Not } or IfElseStatement or WhileStatement or DoWhileStatement or BinaryOperatorExpression { Operator: BinaryOperatorType.ConditionalAnd or BinaryOperatorType.ConditionalOr })
                    inv.ReplaceWith(new PrimitiveExpression(false));
                else
                    inv.ReplaceWith(new PrimitiveExpression(0));
                continue;
            }

            if (inv.Target is not IdentifierExpression id) continue;
            if (inv.Annotation<InvocationResolveResult>()?.Member is not IMethod { IsStatic: false, DeclaringTypeDefinition: { } declTd } im) continue;
            if (declTd.Kind == TypeKind.Delegate || im.Name != id.Identifier) continue;
            var enclosingTdDecl = inv.GetParent<TypeDeclaration>();
            var enclosingTd = enclosingTdDecl?.GetSymbol() as ITypeDefinition;
            var hasNoThis = enclosingTdDecl is not null && inv.Ancestors.TakeWhile(a => a != enclosingTdDecl)
                .OfType<EntityDeclaration>()
                .Any(e => e.Modifiers.HasFlag(Modifiers.Static) || e is FieldDeclaration);
            if (hasNoThis)
            {
                var mr2 = new MemberReferenceExpression(new DefaultValueExpression(astBuilder.ConvertType(im.DeclaringType)), id.Identifier);
                foreach (var ta in id.TypeArguments.ToList()) mr2.TypeArguments.Add(ta.Detach());
                id.ReplaceWith(mr2);
            }
            else if (enclosingTd is not null && enclosingTdDecl is not null
                && !enclosingTd.Equals(declTd) && !enclosingTd.GetAllBaseTypes().Any(b => b.GetDefinition()?.Equals(declTd) == true))
            {
                var thisFieldName = enclosingTdDecl.Members.OfType<FieldDeclaration>()
                    .SelectMany(f => f.Variables.Select(v => (f, v.Name)))
                    .Where(fv => fv.Name is "<>4__this" or "__4__this"
                        && (fv.f.ReturnType.Annotation<TypeResolveResult>()?.Type is not { } ft
                            || ft.GetDefinition()?.Equals(declTd) == true
                            || ft.GetAllBaseTypes().Any(b => b.GetDefinition()?.Equals(declTd) == true)))
                    .Select(fv => fv.Name)
                    .FirstOrDefault();
                Expression receiver = thisFieldName is not null
                    ? new IdentifierExpression(thisFieldName)
                    : new ParenthesizedExpression(new CastExpression(astBuilder.ConvertType(im.DeclaringType),
                        new CastExpression(new PrimitiveType("object"), new ThisReferenceExpression())));
                var mr2 = new MemberReferenceExpression(receiver, id.Identifier);
                foreach (var ta in id.TypeArguments.ToList()) mr2.TypeArguments.Add(ta.Detach());
                id.ReplaceWith(mr2);
            }
        }

        foreach (var ret in tree.Descendants.OfType<ReturnStatement>().ToList())
        {
            if (ret.Expression is not DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression id }) continue;
            var ownerFunc = ret.Ancestors.FirstOrDefault(a => a is MethodDeclaration or LocalFunctionDeclarationStatement or Accessor or IndexerDeclaration);
            if (ownerFunc is null) continue;
            var localDecl = ownerFunc.Descendants.OfType<VariableDeclarationStatement>()
                .FirstOrDefault(d => d.Variables.Any(v => v.Name == id.Identifier));
            if (localDecl is null || localDecl.Type is ComposedType { HasRefSpecifier: true } || localDecl.Type is SimpleType { Identifier: "var" }) continue;
            ret.Expression.ReplaceWith(MakeRefDefaultExpr(localDecl.Type));
        }

        foreach (var assign in tree.Descendants.OfType<AssignmentExpression>().ToList())
        {
            if (assign.Right is not DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression rhsId }) continue;
            var ownerMethod = assign.Ancestors.FirstOrDefault(a => a is MethodDeclaration or ConstructorDeclaration or IndexerDeclaration or LocalFunctionDeclarationStatement);
            var parameters = ownerMethod switch
            {
                MethodDeclaration md => md.Parameters,
                ConstructorDeclaration cd => cd.Parameters,
                IndexerDeclaration id => id.Parameters,
                LocalFunctionDeclarationStatement lf => lf.Declaration.Parameters,
                _ => null,
            };
            if ((parameters is not null && parameters.Any(p => p.Name == rhsId.Identifier))
                || rhsId.Identifier.StartsWith("_outSlot", StringComparison.Ordinal))
                assign.Right.ReplaceWith(rhsId.Detach());
        }

        foreach (var m in tree.Descendants.OfType<MemberReferenceExpression>().ToList())
        {
            if (m.Parent is null || m.MemberName is not ("m_stringLength" or "m_firstChar")) continue;
            if (!IsStringTarget(m.Target)) continue;
            if (m.Parent is AssignmentExpression sa && sa.Left == m && sa.Parent is ExpressionStatement ses)
            {
                ses.Remove();
                continue;
            }
            if (m.MemberName == "m_stringLength")
                m.ReplaceWith(new MemberReferenceExpression(m.Target.Detach(), "Length"));
            else
                m.ReplaceWith(new IndexerExpression(m.Target.Detach(), new PrimitiveExpression(0)));
        }

        foreach (var s in tree.Descendants.OfType<ExpressionStatement>().ToList())
        {
            if (s.Parent is null || s.Expression is not AssignmentExpression { Left: MemberReferenceExpression lm2 } bclAssign) continue;
            var unwrappedTarget = UnwrapParens(lm2.Target);
            var tType = (lm2.Target.Annotation<ResolveResult>() ?? unwrappedTarget.Annotation<ResolveResult>())?.Type
                ?? (unwrappedTarget as CastExpression)?.Type.Annotation<TypeResolveResult>()?.Type;
            if (tType is not null && tType.Kind != TypeKind.Unknown
                && tType.Namespace.StartsWith("System", StringComparison.Ordinal)
                && !tType.GetMembers(mem => mem.Name == lm2.MemberName).Any())
            {
                if (bclAssign.Right.DescendantsAndSelf.Any(n => n is InvocationExpression or ObjectCreateExpression))
                    s.Expression = bclAssign.Right.Detach();
                else
                    s.Remove();
            }
        }
    }

    private static bool IsStringTarget(Expression target)
    {
        var u = UnwrapParens(target);
        if ((target.Annotation<ResolveResult>() ?? u.Annotation<ResolveResult>())?.Type.IsKnownType(KnownTypeCode.String) == true)
            return true;
        if (u is CastExpression { Type: PrimitiveType { Keyword: "string" } })
            return true;
        if (u is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } rmr }
            && rmr.TypeArguments.FirstOrDefault()?.ToString() is "string" or "System.String")
            return true;
        return false;
    }

    private static bool IsStrictDerivedType(IType derived, IType baseType) =>
        !derived.Equals(baseType) && derived.GetAllBaseTypes().Any(b => b.Equals(baseType));

    private static bool IsNonNullableValueTarget(Expression target)
    {
        var vt = target.Annotation<ResolveResult>()?.Type;
        if (vt is not null && vt.Kind is not (TypeKind.Unknown or TypeKind.TypeParameter))
            return vt.FullName != "System.Nullable" && !vt.GetMembers(mem => mem.Name == "Value").Any();
        if (target is DefaultValueExpression dve && dve.Type is not ComposedType { HasNullableSpecifier: true }
            && dve.Type is not SimpleType { Identifier: "Nullable" } && dve.Type is not MemberType { MemberName: "Nullable" })
        {
            var dt = dve.Type.Annotation<TypeResolveResult>()?.Type;
            return dt is not null && dt.Kind is not (TypeKind.Unknown or TypeKind.TypeParameter)
                && dt.FullName != "System.Nullable" && !dt.GetMembers(mem => mem.Name == "Value").Any();
        }
        return false;
    }

    private static PrimitiveType ReplacementForTypeArgument(SimpleType st)
    {
        ITypeDefinition? def = null;
        int argIndex = -1;
        if (st.Parent is SimpleType pst)
        {
            argIndex = pst.TypeArguments.TakeWhile(a => a != st).Count();
            def = pst.Annotation<TypeResolveResult>()?.Type.GetDefinition()
                ?? (pst.Parent as TypeReferenceExpression)?.Annotation<ResolveResult>()?.Type.GetDefinition()
                ?? (pst.Parent as ObjectCreateExpression)?.Annotation<ResolveResult>()?.Type.GetDefinition();
        }
        else if (st.Parent is MemberType pmt)
        {
            argIndex = pmt.TypeArguments.TakeWhile(a => a != st).Count();
            def = pmt.Annotation<TypeResolveResult>()?.Type.GetDefinition()
                ?? (pmt.Parent as TypeReferenceExpression)?.Annotation<ResolveResult>()?.Type.GetDefinition()
                ?? (pmt.Parent as ObjectCreateExpression)?.Annotation<ResolveResult>()?.Type.GetDefinition();
        }
        else if (st.Parent is MemberReferenceExpression pmr)
        {
            argIndex = pmr.TypeArguments.TakeWhile(a => a != st).Count();
            if ((pmr.Parent as InvocationExpression)?.Annotation<InvocationResolveResult>()?.Member is IMethod pm
                && argIndex >= 0 && argIndex < pm.TypeParameters.Count && pm.TypeParameters[argIndex].HasValueTypeConstraint)
                return new PrimitiveType("int");
        }
        if (def is not null && argIndex >= 0 && argIndex < def.TypeParameters.Count && def.TypeParameters[argIndex].HasValueTypeConstraint)
            return new PrimitiveType("int");
        return new PrimitiveType("object");
    }

    private static DirectionExpression MakeRefDefaultExpr(AstType type)
    {
        var arrCreate = new ArrayCreateExpression();
        if (type is ComposedType ct)
        {
            AstType baseElem = ct.BaseType.Clone();
            if (ct.PointerRank > 0) baseElem = new ComposedType { BaseType = baseElem, PointerRank = ct.PointerRank };
            arrCreate.Type = baseElem;
            arrCreate.Arguments.Add(new PrimitiveExpression(1));
            foreach (var spec in ct.ArraySpecifiers) arrCreate.AdditionalArraySpecifiers.Add((ArraySpecifier)spec.Clone());
        }
        else
        {
            arrCreate.Type = type.Clone();
            arrCreate.Arguments.Add(new PrimitiveExpression(1));
        }
        return new DirectionExpression(FieldDirection.Ref, new IndexerExpression(arrCreate, new PrimitiveExpression(0)));
    }

    private static bool IsDelegateCreation(Expression e) => e is ObjectCreateExpression oce && (
        oce.Annotation<ResolveResult>()?.Type.Kind == TypeKind.Delegate
        || oce.Type.Annotation<TypeResolveResult>()?.Type.Kind == TypeKind.Delegate
        || (oce.Arguments.Count == 1 && (oce.Arguments.Single() is AnonymousMethodExpression or LambdaExpression
            || oce.Arguments.Single().Annotation<ResolveResult>()?.GetType().Name == "MethodGroupResolveResult")));

    private static bool IsAbstractTypeInTree(SyntaxTree tree, AstType type)
    {
        var name = type switch { SimpleType s => s.Identifier, MemberType m => m.MemberName, _ => "" };
        return !string.IsNullOrEmpty(name) && tree.Descendants.OfType<TypeDeclaration>()
            .Any(td => td.Modifiers.HasFlag(Modifiers.Abstract) && td.Name == name);
    }

    private static bool TryEvalConstInt(Expression expr, out long value)
    {
        value = 0;
        switch (expr)
        {
            case ParenthesizedExpression p:
                return TryEvalConstInt(p.Expression, out value);
            case DefaultValueExpression:
                return true;
            case PrimitiveExpression { Value: byte or sbyte or short or ushort or int or uint or long } pe:
                value = Convert.ToInt64(pe.Value);
                return true;
            case UnaryOperatorExpression u when TryEvalConstInt(u.Expression, out var uv):
                if (u.Operator == UnaryOperatorType.Plus) { value = uv; return true; }
                if (u.Operator == UnaryOperatorType.Minus) { value = unchecked(-uv); return true; }
                if (u.Operator == UnaryOperatorType.BitNot) { value = ~uv; return true; }
                return false;
            case CastExpression c when TryEvalConstInt(c.Expression, out var cv) && c.Type is PrimitiveType pt:
                switch (pt.Keyword)
                {
                    case "int": value = unchecked((int)cv); return true;
                    case "uint": value = unchecked((uint)cv); return true;
                    case "long": value = cv; return true;
                    case "short": value = unchecked((short)cv); return true;
                    case "ushort": value = unchecked((ushort)cv); return true;
                    case "sbyte": value = unchecked((sbyte)cv); return true;
                    case "byte": value = unchecked((byte)cv); return true;
                }
                return false;
            case BinaryOperatorExpression b when TryEvalConstInt(b.Left, out var lv) && TryEvalConstInt(b.Right, out var rv):
                switch (b.Operator)
                {
                    case BinaryOperatorType.Add: value = unchecked(lv + rv); return true;
                    case BinaryOperatorType.Subtract: value = unchecked(lv - rv); return true;
                    case BinaryOperatorType.Multiply: value = unchecked(lv * rv); return true;
                    case BinaryOperatorType.ShiftLeft: value = unchecked(lv << (int)(rv & 0x3F)); return true;
                    case BinaryOperatorType.ShiftRight: value = unchecked(lv >> (int)(rv & 0x3F)); return true;
                    case BinaryOperatorType.UnsignedShiftRight: value = unchecked((long)((ulong)lv >>> (int)(rv & 0x3F))); return true;
                    case BinaryOperatorType.BitwiseAnd: value = lv & rv; return true;
                    case BinaryOperatorType.BitwiseOr: value = lv | rv; return true;
                    case BinaryOperatorType.ExclusiveOr: value = lv ^ rv; return true;
                    case BinaryOperatorType.Divide when rv != 0: value = unchecked(lv / rv); return true;
                    case BinaryOperatorType.Modulus when rv != 0: value = unchecked(lv % rv); return true;
                }
                return false;
            default:
                return false;
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

    private static bool IsUnresolvedDelegatePtr(Expression first, Expression last) => last switch
    {
        DefaultValueExpression d => d.Type.ToString() is "nint" or "IntPtr" or "System.IntPtr",
        CastExpression { Expression: PrimitiveExpression { Value: 0 } } c =>
            c.Type.ToString() is "nint" or "IntPtr" or "System.IntPtr" || first is NullReferenceExpression,
        InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } mr } =>
            mr.TypeArguments.FirstOrDefault()?.ToString() is "nint" or "IntPtr" or "System.IntPtr",
        _ => false,
    };

    private static bool IsReinterpretCall(Expression e) => e is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } };

    private static bool IsPrimitive(Expression e) =>
        e.Annotation<ResolveResult>()?.Type is { } t && t.GetDefinition()?.KnownTypeCode is >= KnownTypeCode.Boolean and <= KnownTypeCode.Double;

    private static bool IsAccessor(InvocationExpression call) =>
        call.Annotation<InvocationResolveResult>()?.Member is IMethod { IsAccessor: true } or null;

    private static bool SameGenericDefinition(AstType a, AstType b) =>
        a is SimpleType sa && b is SimpleType sb && sa.Identifier == sb.Identifier && sa.TypeArguments.Count == sb.TypeArguments.Count && sa.TypeArguments.Count > 0
        || a is MemberType ma && b is MemberType mb && ma.MemberName == mb.MemberName && ma.TypeArguments.Count == mb.TypeArguments.Count && ma.TypeArguments.Count > 0;
}
