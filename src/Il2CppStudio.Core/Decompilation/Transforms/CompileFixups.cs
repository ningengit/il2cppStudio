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
        var flags = AstWalk.ScanFlags(tree);
        Apply(tree, in flags);
    }

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        var mrs = new List<MemberReferenceExpression>();
        var prs = flags.HasPointerSyntax ? new List<PointerReferenceExpression>() : null;
        AstWalk.ForEachNode(tree, n =>
        {
            if (n is MemberReferenceExpression m && m.MemberName is "m_value" or "value__" or "Value" or "Slot")
                mrs.Add(m);
            else if (prs is not null && n is PointerReferenceExpression pr && pr.MemberName is "m_value" or "value__")
                prs.Add(pr);
        });

        foreach (var m in mrs)
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

        if (prs is not null)
        {
            foreach (var pr in prs)
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
        }

        RewriteMemberCalls(tree);

        var eulerStmts = new List<ExpressionStatement>();
        var coroutineDecls = new List<VariableDeclarationStatement>();
        var distBlocks = new List<BlockStatement>();
        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case ExpressionStatement { Expression: InvocationExpression { Target: MemberReferenceExpression { MemberName: "Euler", Target: TypeReferenceExpression or IdentifierExpression { Identifier: "Quaternion" } } } } es:
                    eulerStmts.Add(es);
                    break;
                case VariableDeclarationStatement { Variables.Count: 1 } vds
                    when vds.Variables.FirstOrDefault() is { Initializer: ObjectCreateExpression { Type: MemberType } }:
                    coroutineDecls.Add(vds);
                    break;
                case BlockStatement bs when bs.Statements.Count >= 2:
                    distBlocks.Add(bs);
                    break;
            }
        });
        MergeEulerRotationDefault(eulerStmts);
        CollapseCrossTypeCoroutineFactories(coroutineDecls);
        RestoreDeadVectorDistance(distBlocks);

        NativeEscapeSanitizer.Apply(tree, in flags);
        PurgeDiscardsAndDeadStatements(tree);
        FixLeakedAndConstantPatterns(tree);
        FoldDoubleUnaryMinus(tree);

        var emptyTypeArgs = new List<SimpleType>();
        var boxedCtorCasts = new List<CastExpression>();
        var delegOces = new List<ObjectCreateExpression>();
        var refDirs = new List<DirectionExpression>();
        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case SimpleType st when st.Identifier.Length == 0 && st.Role == Roles.TypeArgument:
                    emptyTypeArgs.Add(st);
                    break;
                case CastExpression { Expression: CastExpression { Type: PrimitiveType { Keyword: "object" }, Expression: ObjectCreateExpression } } cast:
                    boxedCtorCasts.Add(cast);
                    break;
                case ObjectCreateExpression { Arguments.Count: 2 } oce:
                    delegOces.Add(oce);
                    break;
                case DirectionExpression dir when dir.Parent is ParenthesizedExpression or CastExpression or BinaryOperatorExpression or UnaryOperatorExpression:
                    refDirs.Add(dir);
                    break;
            }
        });

        foreach (var st in emptyTypeArgs)
            if (st.Parent is not null)
                st.ReplaceWith(ReplacementForTypeArgument(st));

        foreach (var cast in boxedCtorCasts)
            if (cast.Parent is not null && cast.Expression is CastExpression { Type: PrimitiveType { Keyword: "object" }, Expression: ObjectCreateExpression oce }
                && SameGenericDefinition(cast.Type, oce.Type) && oce.Arguments.Count == 1)
                cast.ReplaceWith(new ObjectCreateExpression(cast.Type.Clone(), oce.Arguments.Single().Detach()));

        // new D(target, (IntPtr)0) / new D(target, default(nint)): the delegate's target method could not be recovered. Keep it compilable and explicit.
        foreach (var oce in delegOces)
            if (oce.Parent is not null && oce.Arguments.Count == 2 && IsUnresolvedDelegatePtr(oce.Arguments.First(), oce.Arguments.Last()))
                oce.ReplaceWith(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", oce.Type.Clone()),
                    new PrimitiveExpression("unresolved delegate target")));

        foreach (var dir in refDirs)
            if (dir.Parent is ParenthesizedExpression or CastExpression or BinaryOperatorExpression or UnaryOperatorExpression)
                dir.ReplaceWith(dir.Expression.Detach());

        FixRemainingCompilePatterns(tree, in flags);
        FixLeakedTypeParameters(tree);
        PurgeDiscardsAndDeadStatements(tree);
        EmptyIfRemover.Apply(tree);
        CompileHygiene.RefreshUnsafe(tree);
    }

    private static void RewriteMemberCalls(SyntaxTree tree)
    {
        foreach (var call in AstWalk.Collect<InvocationExpression>(tree))
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
            {
                Expression rep = new IndexerExpression(target.Target.Detach(), args.Select(a => a.Detach()));
                if (call.Annotation<ResolveResult>() is { } rr) rep.AddAnnotation(rr);
                call.ReplaceWith(rep);
            }
            else if (target.MemberName == "set_Item" && args.Count >= 2 && call.Annotation<InvocationResolveResult>()?.Member is IMethod { IsAccessor: true })
                call.ReplaceWith(new AssignmentExpression(new IndexerExpression(target.Target.Detach(), args.Take(args.Count - 1).Select(a => a.Detach())), args[^1].Detach()));
            else if (target.MemberName.StartsWith("get_", StringComparison.Ordinal) && args.Count == 0 && IsAccessor(call))
            {
                Expression rep = new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]);
                if (call.Annotation<ResolveResult>() is { } rr) rep.AddAnnotation(rr);
                call.ReplaceWith(rep);
            }
            else if (target.MemberName.StartsWith("set_", StringComparison.Ordinal) && args.Count == 1 && IsAccessor(call))
                call.ReplaceWith(new AssignmentExpression(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]), args[0].Detach()));
            else if (target.MemberName.StartsWith("add_", StringComparison.Ordinal) && args.Count == 1 && IsAccessor(call))
                call.ReplaceWith(new AssignmentExpression(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[4..]), AssignmentOperatorType.Add, args[0].Detach()));
            else if (target.MemberName.StartsWith("remove_", StringComparison.Ordinal) && args.Count == 1 && IsAccessor(call))
                call.ReplaceWith(new AssignmentExpression(new MemberReferenceExpression(target.Target.Detach(), target.MemberName[7..]), AssignmentOperatorType.Subtract, args[0].Detach()));
            else if (target.MemberName is ".ctor" or "_ctor" && call.Parent is ExpressionStatement s)
                RewriteCtorStatement(call, target, args, s);
            else if (target.MemberName is ".cctor" or "_cctor" && call.Parent is ExpressionStatement sCctor)
                sCctor.Remove();
            else if (target.MemberName == "CreateString")
                call.ReplaceWith(new ObjectCreateExpression(new PrimitiveType("string"), args.Select(a => a.Detach())));
            else if (target.MemberName == "GetTextContainerLocalCorners" && args.Count == 0 && call.Parent is ExpressionStatement)
                target.MemberName = "ForceMeshUpdate";
            else if (target.MemberName == "ComputeStringHash" && args.Count == 1)
                call.ReplaceWith(new CastExpression(new PrimitiveType("uint"), new InvocationExpression(new MemberReferenceExpression(args[0].Detach(), "GetHashCode"))));
            else if (target.MemberName == "AsPointer" && args.Count == 1 && IsUnsafeTarget(target.Target))
            {
                var raw = args[0] is DirectionExpression d ? d.Expression.Detach() : args[0].Detach();
                AstNode anchor = call;
                while (anchor.Parent is ParenthesizedExpression paren) anchor = paren;
                while (anchor.Parent is CastExpression midCast && IsNintAstType(midCast.Type))
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
                && IsUnsafeTarget(target.Target)
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
            else if (target.MemberName == "Write" && args.Count == 2 && IsUnsafeTarget(target.Target))
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

    private static void MergeEulerRotationDefault(List<ExpressionStatement> candidates)
    {
        foreach (var es in candidates)
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

    private static void CollapseCrossTypeCoroutineFactories(List<VariableDeclarationStatement> candidates)
    {
        foreach (var decl in candidates)
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
            var uses = new List<IdentifierExpression>();
            AstWalk.Collect<IdentifierExpression>(block, i =>
            {
                if (i.Identifier != v.Name) return false;
                for (AstNode? a = i.Parent; a is not null && a != block; a = a.Parent)
                    if (a == decl || setup.Contains(a)) return false;
                return true;
            }, uses);
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
        if (target.Target is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } reinterp } outer
            && reinterp.TypeArguments.Count == 1
            && outer.Arguments.FirstOrDefault() is IdentifierExpression or MemberReferenceExpression)
        {
            var ctorType = reinterp.TypeArguments.First();
            var loc = outer.Arguments.First();
            var isDelegate = args.Count == 2 && (ctorType.Annotation<TypeResolveResult>()?.Type.Kind == TypeKind.Delegate
                || !ReferenceEquals(UnwrapLdftn(args[1]), args[1]));
            Expression? rhs = null;
            if (isDelegate)
            {
                var fn = UnwrapLdftn(args[1]);
                if (fn is IdentifierExpression or MemberReferenceExpression)
                {
                    var recv = args[0];
                    while (true)
                    {
                        while (recv is ParenthesizedExpression pe) recv = pe.Expression;
                        if (recv is CastExpression { Type: PrimitiveType { Keyword: "object" } } objCast) { recv = objCast.Expression; continue; }
                        if (recv is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } mr, Arguments.Count: 1 }
                            && mr.TypeArguments.FirstOrDefault() is PrimitiveType { Keyword: "object" })
                        {
                            recv = mr.Parent is InvocationExpression inv ? inv.Arguments.First() : recv;
                            continue;
                        }
                        break;
                    }
                    if (recv is not NullReferenceExpression and not DefaultValueExpression and not PrimitiveExpression { Value: 0 or 0L }
                        && recv.Annotation<ResolveResult>()?.Type is { IsReferenceType: false })
                    {
                        rhs = new InvocationExpression(
                            new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                                "Reinterpret", ctorType.Clone()),
                            new DefaultValueExpression(new SimpleType("nint")));
                    }
                    else
                    {
                        var delArg = recv is NullReferenceExpression or DefaultValueExpression or PrimitiveExpression { Value: 0 or 0L }
                            ? fn.Clone()
                            : fn is IdentifierExpression fid ? new MemberReferenceExpression(recv.Detach(), fid.Identifier)
                            : fn is MemberReferenceExpression fmr && fmr.Target is TypeReferenceExpression ? new MemberReferenceExpression(recv.Detach(), fmr.MemberName)
                            : fn.Clone();
                        rhs = new ObjectCreateExpression(ctorType.Clone(), delArg);
                    }
                }
            }
            else if (ctorType.Annotation<TypeResolveResult>()?.Type.GetDefinition()?.IsAbstract == true
                || (s.GetParent<SyntaxTree>() is { } stTree && IsAbstractTypeInTree(stTree, ctorType)))
            {
                rhs = new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", ctorType.Clone()),
                    new ObjectCreateExpression(new PrimitiveType("object")));
            }
            else
            {
                var oce = new ObjectCreateExpression(ctorType.Clone());
                foreach (var a in args) oce.Arguments.Add(a.Detach());
                rhs = oce;
            }
            if (rhs is not null)
            {
                var locType = loc.Annotation<ResolveResult>()?.Type;
                var ctorResolved = ctorType.Annotation<TypeResolveResult>()?.Type;
                var needsReinterp = locType is not null
                    && !locType.IsKnownType(KnownTypeCode.Object)
                    && locType.Kind != TypeKind.Unknown
                    && !(isDelegate && locType.Kind == TypeKind.Delegate)
                    && !(ctorResolved is not null && locType.Equals(ctorResolved));
                if (needsReinterp)
                {
                    var astBuilder = new TypeSystemAstBuilder();
                    rhs = new InvocationExpression(
                        new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                            "Reinterpret", astBuilder.ConvertType(locType!)),
                        rhs);
                }
                s.Expression = new AssignmentExpression(loc.Detach(), rhs);
                return;
            }
        }
        s.Remove();
    }

    private static void RestoreDeadVectorDistance(List<BlockStatement> blocks)
    {
        foreach (var block in blocks)
        {
            for (Statement? s = block.Statements.FirstOrDefault(); s is not null && s.NextSibling is Statement next;)
            {
                var nextNext = next.NextSibling as Statement;
                if (s is ExpressionStatement s1 && next is ExpressionStatement s2
                    && UnwrapDiscard(s1.Expression) is MemberReferenceExpression { MemberName: "position" } e1
                    && UnwrapDiscard(s2.Expression) is MemberReferenceExpression { MemberName: "position" } e2
                    && e1.ToString() != e2.ToString())
                {
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
                    s = nextNext;
                    continue;
                }
                s = next;
            }
        }
    }

    private static string? FindUnusedDistanceField(BlockStatement block)
    {
        var method = block.GetParent<MethodDeclaration>();
        var type = method?.GetParent<TypeDeclaration>();
        if (method is null || type is null) return null;
        var used = new HashSet<string>(StringComparer.Ordinal);
        AstWalk.ForEachNode(method, n =>
        {
            if (n is IdentifierExpression id) used.Add(id.Identifier);
            else if (n is MemberReferenceExpression mr) used.Add(mr.MemberName);
        });
        return type.Members.OfType<FieldDeclaration>()
            .Where(f => f.ReturnType is PrimitiveType { Keyword: "float" })
            .SelectMany(f => f.Variables)
            .Select(v => v.Name)
            .FirstOrDefault(n => n.EndsWith("Distance", StringComparison.OrdinalIgnoreCase) && !used.Contains(n));
    }

    private static string PickFreeName(BlockStatement block, string baseName)
    {
        var method = block.GetParent<MethodDeclaration>();
        var taken = new HashSet<string>(StringComparer.Ordinal);
        AstWalk.Visit<VariableInitializer>(method ?? (AstNode)block, v => taken.Add(v.Name));
        if (method is not null)
            foreach (var p in method.Parameters) taken.Add(p.Name);
        if (!taken.Contains(baseName)) return baseName;
        for (var n = 1; ; n++) { var c = $"distance{(n == 1 ? "" : n.ToString())}"; if (!taken.Contains(c)) return c; }
    }

    private static void PurgeDiscardsAndDeadStatements(SyntaxTree tree)
    {
        foreach (var s in AstWalk.Collect<ExpressionStatement>(tree))
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
            if (s.Expression is AssignmentExpression daRval && HasRvalueRoot(daRval.Left))
            {
                var rhs = Unwrap(daRval.Right);
                if (!IsReinterpretCall(rhs) && rhs is InvocationExpression or ObjectCreateExpression && !IsDelegateCreation(rhs))
                    s.Expression = rhs.Detach();
                else
                    s.Remove();
                continue;
            }

            var expr = UnwrapParens(s.Expression);
            if (!IsReinterpretCall(expr) && expr is not (BinaryOperatorExpression or UnaryOperatorExpression { Operator: not (UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement or UnaryOperatorType.Await) }
                or MemberReferenceExpression or IdentifierExpression or PrimitiveExpression or NullReferenceExpression
                or DefaultValueExpression or ConditionalExpression or CastExpression or AsExpression or IsExpression
                or TypeReferenceExpression or IndexerExpression)) continue;

            var sideEffects = new List<Expression>();
            if (!IsReinterpretCall(expr) && expr is InvocationExpression or ObjectCreateExpression or AssignmentExpression)
                sideEffects.Add(expr);
            AstWalk.Collect<Expression>(expr, e => !IsReinterpretCall(e) && e is InvocationExpression or ObjectCreateExpression or AssignmentExpression, sideEffects);
            if (sideEffects.Count == 0) { s.Remove(); continue; }
            var se0 = sideEffects[0];
            var hasEnclosingSideEffect = false;
            if (s.Parent is BlockStatement && sideEffects.Count == 1)
            {
                for (AstNode? a = se0.Parent; a is not null && a != expr; a = a.Parent)
                {
                    if (a is Expression ae && !IsReinterpretCall(ae) && ae is InvocationExpression or ObjectCreateExpression or AssignmentExpression)
                    {
                        hasEnclosingSideEffect = true;
                        break;
                    }
                }
            }
            if (s.Parent is BlockStatement && sideEffects.Count == 1 && !hasEnclosingSideEffect && !IsDelegateCreation(se0))
                s.Expression = se0.Detach();
            else s.Remove();
        }
    }

    private static bool HasRvalueRoot(Expression e)
    {
        while (true)
        {
            e = Unwrap(e);
            if (e is DefaultValueExpression or NullReferenceExpression) return true;
            if (e is MemberReferenceExpression mre) { e = mre.Target; continue; }
            if (e is IndexerExpression ie) { e = ie.Target; continue; }
            return false;
        }
    }

    private static void FixLeakedTypeParameters(SyntaxTree tree)
    {
        var leaked = new List<SimpleType>();
        AstWalk.Collect<SimpleType>(tree, static st =>
            st.Identifier.Length > 0 && !(st.Role == Roles.TypeArgument && st.Identifier.Length == 0)
            && (st.Identifier[0] == '!' || st.Identifier is "_0" or "_1" or "_2" or "_3" or "__0" or "__1" or "__2" or "__3" or "TKey" or "TValue" or "T"),
            leaked);
        FixLeakedTypeParametersCore(leaked);
    }

    private static void FixLeakedTypeParametersCore(List<SimpleType> leaked)
    {
        foreach (var st in leaked)
        {
            if (st.Parent is null) continue;
            var rt = st.Annotation<TypeResolveResult>()?.Type;
            if (rt is not null && rt.Kind is not (TypeKind.Unknown or TypeKind.TypeParameter)) continue;
            var isDeclared = false;
            for (AstNode? a = st.Parent; a is not null && !isDeclared; a = a.Parent)
            {
                switch (a)
                {
                    case TypeDeclaration td:
                        foreach (var tp in td.TypeParameters) if (tp.Name == st.Identifier) { isDeclared = true; break; }
                        break;
                    case MethodDeclaration md:
                        foreach (var tp in md.TypeParameters) if (tp.Name == st.Identifier) { isDeclared = true; break; }
                        break;
                    case DelegateDeclaration dd:
                        foreach (var tp in dd.TypeParameters) if (tp.Name == st.Identifier) { isDeclared = true; break; }
                        break;
                }
            }
            if (!isDeclared)
                st.ReplaceWith(st.Role == Roles.TypeArgument ? ReplacementForTypeArgument(st) : new PrimitiveType("object"));
        }
    }

    private static void FixLeakedAndConstantPatterns(SyntaxTree tree)
    {
        var leaked = new List<SimpleType>();
        var arrays = new List<ArrayCreateExpression>();
        var casts = new List<CastExpression>();
        var bins = new List<BinaryOperatorExpression>();
        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case SimpleType st when st.Parent is not null && st.Identifier.Length > 0
                    && (st.Identifier[0] == '!' || st.Identifier is "_0" or "_1" or "_2" or "_3" or "__0" or "__1" or "__2" or "__3" or "TKey" or "TValue" or "T"):
                    leaked.Add(st);
                    break;
                case ArrayCreateExpression ace when ace.Arguments.Count > 0:
                    arrays.Add(ace);
                    break;
                case CastExpression c when c.Type is PrimitiveType { Keyword: "uint" or "ulong" or "byte" or "ushort" or "int" or "sbyte" or "short" }:
                    casts.Add(c);
                    break;
                case BinaryOperatorExpression b when b.Parent is not (BinaryOperatorExpression or ParenthesizedExpression or CastExpression or UncheckedExpression):
                    bins.Add(b);
                    break;
            }
        });

        FixLeakedTypeParametersCore(leaked);

        foreach (var ace in arrays)
        {
            foreach (var arg in ace.Arguments.ToList())
            {
                if (!HasIdentMemberOrCall(arg) && TryEvalConstInt(arg, out var len) && len < 0)
                    arg.ReplaceWith(new PrimitiveExpression(0));
            }
        }

        for (var i = casts.Count - 1; i >= 0; i--)
        {
            var cast = casts[i];
            if (cast.Parent is null or UncheckedExpression || HasUncheckedAncestor(cast)) continue;
            if (HasIdentMemberOrCall(cast.Expression)) continue;
            if (HasPrimOrDefault(cast.Expression))
                cast.ReplaceWith(new UncheckedExpression(cast.Clone()));
        }

        for (var i = bins.Count - 1; i >= 0; i--)
        {
            var bin = bins[i];
            if (bin.Parent is null or BinaryOperatorExpression or ParenthesizedExpression or CastExpression or UncheckedExpression) continue;
            if (!HasIdentMemberOrCall(bin) && TryEvalDefaultComparison(bin, out var constBool))
            {
                bin.ReplaceWith(new PrimitiveExpression(constBool));
                continue;
            }
            if (HasUncheckedAncestor(bin)) continue;
            if (!AstWalk.HasAny<DefaultValueExpression>(bin)) continue;
            if (HasIdentMemberOrCall(bin)) continue;
            bin.ReplaceWith(new UncheckedExpression(bin.Clone()));
        }

        foreach (var ifs in AstWalk.Collect<IfElseStatement>(tree, static s => s.Condition is PrimitiveExpression { Value: bool } && s.Parent is BlockStatement))
        {
            if (ifs.Parent is not BlockStatement block || ifs.Condition is not PrimitiveExpression { Value: bool condVal }) continue;
            if (!condVal)
            {
                if (ifs.FalseStatement.IsNull)
                    ifs.Remove();
                else if (ifs.FalseStatement is BlockStatement fb && !fb.Statements.OfType<VariableDeclarationStatement>().Any())
                {
                    foreach (var st in fb.Statements.ToList())
                        block.InsertChildBefore(ifs, st.Detach(), BlockStatement.StatementRole);
                    ifs.Remove();
                }
                else
                {
                    var rep = ifs.FalseStatement.Detach();
                    if (rep is BlockStatement || !rep.Descendants.OfType<VariableDeclarationStatement>().Any())
                        ifs.ReplaceWith(rep);
                }
            }
            else if (ifs.TrueStatement is BlockStatement tb && !tb.Statements.OfType<VariableDeclarationStatement>().Any())
            {
                foreach (var st in tb.Statements.ToList())
                    block.InsertChildBefore(ifs, st.Detach(), BlockStatement.StatementRole);
                ifs.Remove();
            }
        }

        static bool TryEvalDefaultComparison(BinaryOperatorExpression b, out bool result)
        {
            result = false;
            if (b.Operator is not (BinaryOperatorType.Equality or BinaryOperatorType.InEquality
                or BinaryOperatorType.LessThan or BinaryOperatorType.LessThanOrEqual
                or BinaryOperatorType.GreaterThan or BinaryOperatorType.GreaterThanOrEqual))
                return false;
            if (!TryZeroConst(b.Left, out var lIsDef, out var lv) || !TryZeroConst(b.Right, out var rIsDef, out var rv))
                return false;
            if (!lIsDef && !rIsDef) return false;
            result = b.Operator switch
            {
                BinaryOperatorType.Equality => lv == rv,
                BinaryOperatorType.InEquality => lv != rv,
                BinaryOperatorType.LessThan => lv < rv,
                BinaryOperatorType.LessThanOrEqual => lv <= rv,
                BinaryOperatorType.GreaterThan => lv > rv,
                BinaryOperatorType.GreaterThanOrEqual => lv >= rv,
                _ => false,
            };
            return true;
        }

        static bool TryZeroConst(Expression e, out bool isDefault, out long val)
        {
            e = UnwrapParens(e);
            if (e is DefaultValueExpression dve && dve.Type is PrimitiveType
                {
                    Keyword: "int" or "uint" or "long" or "ulong" or "byte" or "sbyte" or "short" or "ushort" or "bool" or "char"
                })
            {
                isDefault = true;
                val = 0;
                return true;
            }
            isDefault = false;
            return TryEvalConstInt(e, out val);
        }

        static bool HasUncheckedAncestor(AstNode n)
        {
            for (AstNode? a = n.Parent; a is not null; a = a.Parent)
                if (a is UncheckedExpression) return true;
            return false;
        }
        static bool HasIdentMemberOrCall(AstNode n) =>
            n is IdentifierExpression or MemberReferenceExpression or InvocationExpression
            || AstWalk.HasAny(n, static x => x is IdentifierExpression or MemberReferenceExpression or InvocationExpression);
        static bool HasPrimOrDefault(AstNode n) =>
            n is PrimitiveExpression or DefaultValueExpression
            || AstWalk.HasAny(n, static x => x is PrimitiveExpression or DefaultValueExpression);
    }

    private static void FixRemainingCompilePatterns(SyntaxTree tree, in TreeFlags flags)
    {
        var derefs = new List<UnaryOperatorExpression>();
        var ptrs = new List<ComposedType>();
        var abstractTypeNames = new HashSet<string>(StringComparer.Ordinal);
        var oces = new List<ObjectCreateExpression>();
        var ctors = new List<ConstructorDeclaration>();
        var baseInits = new List<ConstructorInitializer>();
        var zeroOrNullCasts = new List<CastExpression>();
        var asExprs = new List<AsExpression>();
        var readonlyPropAssigns = new List<AssignmentExpression>();
        var refRhsAssigns = new List<AssignmentExpression>();
        var invocs = new List<InvocationExpression>();
        var refReturns = new List<ReturnStatement>();
        var stringFieldMrs = new List<MemberReferenceExpression>();

        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case TypeDeclaration td when td.Modifiers.HasFlag(Modifiers.Abstract):
                    abstractTypeNames.Add(td.Name);
                    break;
                case UnaryOperatorExpression { Operator: UnaryOperatorType.Dereference } u:
                    derefs.Add(u);
                    break;
                case ComposedType { PointerRank: > 0 } ct:
                    ptrs.Add(ct);
                    break;
                case ObjectCreateExpression oce:
                    oces.Add(oce);
                    break;
                case ConstructorDeclaration cd when !cd.Modifiers.HasFlag(Modifiers.Static) && cd.Initializer.IsNull:
                    ctors.Add(cd);
                    break;
                case ConstructorInitializer ci when ci.ConstructorInitializerType == ConstructorInitializerType.Base && ci.Arguments.Count > 0:
                    baseInits.Add(ci);
                    break;
                case CastExpression c when c.Expression is NullReferenceExpression || UnwrapParens(c.Expression) is PrimitiveExpression { Value: 0 or 0L }:
                    zeroOrNullCasts.Add(c);
                    break;
                case AsExpression ae:
                    asExprs.Add(ae);
                    break;
                case AssignmentExpression assign:
                    if (assign.Left is MemberReferenceExpression mr && mr.Target is not BaseReferenceExpression)
                        readonlyPropAssigns.Add(assign);
                    if (assign.Right is DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression })
                        refRhsAssigns.Add(assign);
                    break;
                case InvocationExpression inv:
                    invocs.Add(inv);
                    break;
                case ReturnStatement { Expression: DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression } } ret:
                    refReturns.Add(ret);
                    break;
                case MemberReferenceExpression { MemberName: "m_stringLength" or "m_firstChar" } smr:
                    stringFieldMrs.Add(smr);
                    break;
            }
        });

        if (derefs is not null)
        {
            foreach (var u in derefs)
            {
                if (u.Parent is null || UnwrapParens(u.Expression) is not CastExpression { Type: ComposedType { PointerRank: > 0, BaseType: SimpleType bst } }) continue;
                if (bst.Identifier == "_" || bst.Identifier.Length == 0 || IdentifierEscaper.Escape(bst.Identifier) == "_")
                    u.ReplaceWith(new PrimitiveExpression(0));
            }
        }

        if (ptrs is not null)
        {
            foreach (var ct in ptrs)
                if (ct.PointerRank > 0 && ct.BaseType is SimpleType bst && (bst.Identifier == "_" || bst.Identifier.Length == 0 || IdentifierEscaper.Escape(bst.Identifier) == "_"))
                    ct.BaseType = new PrimitiveType("void");
        }

        foreach (var oce in oces)
        {
            if (oce.Parent is null) continue;
            var def = oce.Annotation<ResolveResult>()?.Type.GetDefinition() ?? oce.Type.Annotation<TypeResolveResult>()?.Type.GetDefinition();
            var isAbstract = def?.IsAbstract == true || IsTypeNameInSet(abstractTypeNames, oce.Type);
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
        foreach (var ctor in ctors)
        {
            if (ctor.Modifiers.HasFlag(Modifiers.Static) || !ctor.Initializer.IsNull) continue;
            var td = ctor.GetParent<TypeDeclaration>();
            var resolvedType = td?.Annotation<TypeResolveResult>()?.Type ?? td?.GetSymbol() as IType;
            var baseType = resolvedType?.DirectBaseTypes.FirstOrDefault(b => b.Kind == TypeKind.Class && !b.IsKnownType(KnownTypeCode.Object));
            if (baseType is null) continue;
            var ctorDefs = baseType.GetConstructors().Where(c => !c.IsStatic).ToList();
            if (ctorDefs.Count == 0 || ctorDefs.Any(c => c.Accessibility != Accessibility.Private && c.Parameters.All(p => p.IsOptional))) continue;
            var chosen = ctorDefs
                .Where(c => c.Accessibility != Accessibility.Private && c.Parameters.All(p => p.ReferenceKind == ReferenceKind.None))
                .OrderBy(c => c.Parameters.Count)
                .FirstOrDefault()
                ?? ctorDefs.Where(c => c.Accessibility != Accessibility.Private).OrderBy(c => c.Parameters.Count).FirstOrDefault();
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

        foreach (var ci in baseInits)
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

        foreach (var cast in zeroOrNullCasts)
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

        foreach (var ae in asExprs)
        {
            if (ae.Parent is null) continue;
            var srcT = ae.Expression.Annotation<ResolveResult>()?.Type
                ?? (ae.Expression is ThisReferenceExpression ? ae.GetParent<TypeDeclaration>()?.GetSymbol() as IType : null);
            var dstT = ae.Type.Annotation<TypeResolveResult>()?.Type;
            if (srcT is { Kind: TypeKind.Class } && dstT is { Kind: TypeKind.Class }
                && !srcT.IsKnownType(KnownTypeCode.Object) && !dstT.IsKnownType(KnownTypeCode.Object)
                && !srcT.Equals(dstT) && !IsStrictDerivedType(srcT, dstT) && !IsStrictDerivedType(dstT, srcT))
            {
                ae.Expression = new CastExpression(new PrimitiveType("object"), ae.Expression.Detach());
            }
        }

        foreach (var assign in readonlyPropAssigns)
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

        foreach (var inv in invocs)
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
            var hasNoThis = false;
            if (enclosingTdDecl is not null)
            {
                for (AstNode? a = inv.Parent; a is not null && a != enclosingTdDecl; a = a.Parent)
                {
                    if (a is EntityDeclaration ed && (ed.Modifiers.HasFlag(Modifiers.Static) || ed is FieldDeclaration))
                    {
                        hasNoThis = true;
                        break;
                    }
                }
            }
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

        foreach (var ret in refReturns)
        {
            if (ret.Expression is not DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression id }) continue;
            AstNode? ownerFunc = null;
            for (AstNode? a = ret.Parent; a is not null; a = a.Parent)
                if (a is MethodDeclaration or LocalFunctionDeclarationStatement or Accessor or IndexerDeclaration) { ownerFunc = a; break; }
            if (ownerFunc is null) continue;
            VariableDeclarationStatement? localDecl = null;
            AstWalk.Visit<VariableDeclarationStatement>(ownerFunc, d =>
            {
                if (localDecl is null && d.Variables.Any(v => v.Name == id.Identifier))
                    localDecl = d;
            });
            if (localDecl is null || localDecl.Type is ComposedType { HasRefSpecifier: true } || localDecl.Type is SimpleType { Identifier: "var" }) continue;
            ret.Expression.ReplaceWith(MakeRefDefaultExpr(localDecl.Type));
        }

        foreach (var assign in refRhsAssigns)
        {
            if (assign.Right is not DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IdentifierExpression rhsId }) continue;
            AstNode? ownerMethod = null;
            for (AstNode? a = assign.Parent; a is not null; a = a.Parent)
                if (a is MethodDeclaration or ConstructorDeclaration or IndexerDeclaration or LocalFunctionDeclarationStatement) { ownerMethod = a; break; }
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

        foreach (var m in stringFieldMrs)
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

        static bool IsSystemNamespace(string ns) =>
            ns == "System" || ns.StartsWith("System.", StringComparison.Ordinal);

        static bool HasAccessibleMember(IType tType, string memberName, ITypeDefinition? enclosingTd)
        {
            if (tType.GetDefinition()?.ParentModule?.Compilation is { } comp)
            {
                foreach (var mem in TypeSystemCache.For(comp).GetMembers(tType))
                {
                    if (mem.Name != memberName) continue;
                    if (mem.Accessibility == Accessibility.Public) return true;
                    if (mem.Accessibility is Accessibility.Protected or Accessibility.ProtectedOrInternal
                        && enclosingTd is not null && tType.GetDefinition() is { } td
                        && (enclosingTd.Equals(td) || IsStrictDerivedType(enclosingTd, td)))
                        return true;
                }
                return false;
            }
            return tType.GetMembers(mem => mem.Name == memberName
                && (mem.Accessibility == Accessibility.Public
                    || (mem.Accessibility is Accessibility.Protected or Accessibility.ProtectedOrInternal
                        && enclosingTd is not null && tType.GetDefinition() is { } td
                        && (enclosingTd.Equals(td) || IsStrictDerivedType(enclosingTd, td))))).Any();
        }

        IType? InferExprType(Expression e)
        {
            var unwrapped = UnwrapParens(e);
            var direct = (e.Annotation<ResolveResult>() ?? unwrapped.Annotation<ResolveResult>())?.Type
                ?? (unwrapped as CastExpression)?.Type.Annotation<TypeResolveResult>()?.Type
                ?? (unwrapped as DefaultValueExpression)?.Type.Annotation<TypeResolveResult>()?.Type
                ?? (unwrapped as TypeReferenceExpression)?.Type.Annotation<TypeResolveResult>()?.Type;
            if (direct is not null && direct.Kind != TypeKind.Unknown) return direct;
            if (unwrapped is MemberReferenceExpression innerMr && InferExprType(innerMr.Target) is { } parentT)
            {
                if (parentT.GetDefinition()?.ParentModule?.Compilation is { } pcomp)
                    return TypeSystemCache.For(pcomp).TryGetMemberReturnType(parentT, innerMr.MemberName, out var rt) ? rt : null;
                return parentT.GetMembers(m => m.Name == innerMr.MemberName).FirstOrDefault()?.ReturnType;
            }
            return null;
        }

        static bool IsBooleanContext(Expression e) => e.Parent switch
        {
            IfElseStatement ifs when ifs.Condition == e => true,
            WhileStatement ws when ws.Condition == e => true,
            DoWhileStatement dws when dws.Condition == e => true,
            ConditionalExpression ce when ce.Condition == e => true,
            UnaryOperatorExpression uo when uo.Operator == UnaryOperatorType.Not && uo.Expression == e => true,
            BinaryOperatorExpression boe when boe.Operator is BinaryOperatorType.ConditionalAnd or BinaryOperatorType.ConditionalOr => true,
            _ => false,
        };

        static IType? ArgumentType(InvocationExpression inv, IMethod m, Expression arg)
        {
            var idx = inv.Arguments.TakeWhile(a => a != arg).Count();
            if ((uint)idx >= (uint)m.Parameters.Count) return null;
            var pt = m.Parameters[idx].Type;
            return pt is ByReferenceType br ? br.ElementType : pt;
        }

        var bclAssignStmts = new List<ExpressionStatement>();
        var allMrs = new List<MemberReferenceExpression>();
        var staticVarDecls = new List<VariableDeclarationStatement>();
        var staticDefaults = new List<DefaultValueExpression>();
        var bitNots = new List<UnaryOperatorExpression>();
        var boolCasts = new List<CastExpression>();
        var hasCustomEvents = false;
        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case ExpressionStatement { Expression: AssignmentExpression { Left: MemberReferenceExpression } } bs:
                    bclAssignStmts.Add(bs);
                    break;
                case MemberReferenceExpression mr:
                    allMrs.Add(mr);
                    break;
                case VariableDeclarationStatement vds when vds.Type.Annotation<TypeResolveResult>()?.Type.GetDefinition()?.IsStatic == true:
                    staticVarDecls.Add(vds);
                    break;
                case DefaultValueExpression dve when dve.Type.Annotation<TypeResolveResult>()?.Type.GetDefinition()?.IsStatic == true:
                    staticDefaults.Add(dve);
                    break;
                case UnaryOperatorExpression { Operator: UnaryOperatorType.BitNot } u:
                    bitNots.Add(u);
                    break;
                case CastExpression c when c.Expression is IndexerExpression || c.Expression.Annotation<ResolveResult>()?.Type.IsKnownType(KnownTypeCode.Boolean) == true:
                    boolCasts.Add(c);
                    break;
                case CustomEventDeclaration:
                    hasCustomEvents = true;
                    break;
            }
        });

        foreach (var s in bclAssignStmts)
        {
            if (s.Parent is null || s.Expression is not AssignmentExpression { Left: MemberReferenceExpression lm2 } bclAssign) continue;
            var unwrappedTarget = UnwrapParens(lm2.Target);
            var tType = (lm2.Target.Annotation<ResolveResult>() ?? unwrappedTarget.Annotation<ResolveResult>())?.Type
                ?? (unwrappedTarget as CastExpression)?.Type.Annotation<TypeResolveResult>()?.Type;
            if (tType is not null && tType.Kind != TypeKind.Unknown
                && IsSystemNamespace(tType.Namespace)
                && !HasMemberCached(tType, lm2.MemberName))
            {
                if (AstWalk.HasAny(bclAssign.Right, static n => n is InvocationExpression or ObjectCreateExpression)
                    || bclAssign.Right is InvocationExpression or ObjectCreateExpression)
                    s.Expression = bclAssign.Right.Detach();
                else
                    s.Remove();
            }
        }

        foreach (var mr in allMrs)
        {
            if (mr.Parent is null || (mr.Parent is AssignmentExpression a0 && a0.Left == mr)
                || (mr.Parent is InvocationExpression invB && invB.Target == mr))
                continue;
            var enclosingTd0 = mr.GetParent<TypeDeclaration>()?.GetSymbol() as ITypeDefinition;
            var tType = InferExprType(mr.Target);
            if (tType is null || tType.Kind == TypeKind.Unknown
                || !IsSystemNamespace(tType.Namespace)
                || HasAccessibleMember(tType, mr.MemberName, enclosingTd0)
                || tType.GetNestedTypes().Any(nt => nt.Name == mr.MemberName))
                continue;
            if (mr.Parent is DirectionExpression dir)
            {
                AstType dirType = dir.Parent is InvocationExpression invD && invD.Annotation<InvocationResolveResult>()?.Member is IMethod mD
                    && ArgumentType(invD, mD, dir) is { Kind: not (TypeKind.Unknown or TypeKind.Null) } argTD
                    ? astBuilder.ConvertType(argTD)
                    : new PrimitiveType("int");
                dir.ReplaceWith(MakeRefDefaultExpr(dirType));
                continue;
            }
            Expression replacement = mr.Parent switch
            {
                _ when IsBooleanContext(mr) => new PrimitiveExpression(false),
                CastExpression ce when ce.Expression == mr
                    && ce.Type.Annotation<TypeResolveResult>()?.Type is { Kind: not (TypeKind.Unknown or TypeKind.Null) } ct =>
                    new DefaultValueExpression(astBuilder.ConvertType(ct)),
                InvocationExpression invArg when invArg.Annotation<InvocationResolveResult>()?.Member is IMethod invM
                    && ArgumentType(invArg, invM, mr) is { Kind: not (TypeKind.Unknown or TypeKind.Null) } argT =>
                    new DefaultValueExpression(astBuilder.ConvertType(argT)),
                AssignmentExpression ae when ae.Right == mr
                    && ae.Left.Annotation<ResolveResult>()?.Type is { Kind: not (TypeKind.Unknown or TypeKind.Null) } lt =>
                    new DefaultValueExpression(astBuilder.ConvertType(lt)),
                VariableInitializer vi when vi.Parent is VariableDeclarationStatement vds
                    && vds.Type is not SimpleType { Identifier: "var" } =>
                    new DefaultValueExpression(vds.Type.Clone()),
                BinaryOperatorExpression boe
                    when (boe.Left == mr ? boe.Right : boe.Left).Annotation<ResolveResult>()?.Type.Kind == TypeKind.Null =>
                    new NullReferenceExpression(),
                BinaryOperatorExpression boe
                    when (boe.Left == mr ? boe.Right : boe.Left).Annotation<ResolveResult>()?.Type is { Kind: not (TypeKind.Unknown or TypeKind.Null) } ot =>
                    new DefaultValueExpression(astBuilder.ConvertType(ot)),
                _ => new PrimitiveExpression(0),
            };
            mr.ReplaceWith(replacement);
        }

        foreach (var mr in allMrs)
        {
            if (mr.Parent is null) continue;
            var parentInv = mr.Parent is InvocationExpression inv0 && inv0.Target == mr ? inv0 : null;
            var member = parentInv?.Annotation<InvocationResolveResult>()?.Member
                ?? mr.Annotation<MemberResolveResult>()?.Member;
            if (member is null || member.DeclaringTypeDefinition is not { } declTd) continue;
            var enclosingTd = mr.GetParent<TypeDeclaration>()?.GetSymbol() as ITypeDefinition;
            if (enclosingTd is null) continue;

            var crossAsm = member.ParentModule != enclosingTd.ParentModule;
            // ReferencePublicizer publicizes all fields and non-virtual members in cross-assembly references;
            // valid-identifier cross-assembly fields remain bindable, while unspeakable names (<...>) get escaped by
            // IdentifierEscaper in C# source and no longer match metadata in referenced DLLs.
            if (crossAsm && member is IField
                && string.Equals(IdentifierEscaper.Escape(member.Name), member.Name, StringComparison.Ordinal))
                continue;
            var lockedProtectedOverride = member.Accessibility == Accessibility.Protected && member.IsOverride
                && InheritanceHelper.GetBaseMembers(member, includeImplementedInterfaces: false).Any(bm => bm.ParentModule != enclosingTd.ParentModule);
            if ((crossAsm || lockedProtectedOverride) && member.Accessibility != Accessibility.Public)
            {
                var derivedOk = !lockedProtectedOverride && member.Accessibility is Accessibility.Protected or Accessibility.ProtectedOrInternal
                    && (enclosingTd.Equals(declTd) || IsStrictDerivedType(enclosingTd, declTd));
                if (!derivedOk)
                {
                    if (parentInv is not null)
                    {
                        var nativeCall = new InvocationExpression(
                            new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")), "CallNative"),
                            new PrimitiveExpression($"inaccessible:{member.DeclaringType.Name}.{member.Name}"),
                            new PrimitiveExpression(0L), new PrimitiveExpression(0L), new PrimitiveExpression(0L), new PrimitiveExpression(0L));
                        if (parentInv.Parent is ExpressionStatement)
                            parentInv.ReplaceWith(nativeCall);
                        else if (!member.ReturnType.IsKnownType(KnownTypeCode.Void))
                            parentInv.ReplaceWith(new InvocationExpression(
                                new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                                    "Reinterpret", astBuilder.ConvertType(member.ReturnType)),
                                nativeCall));
                        else
                            parentInv.ReplaceWith(nativeCall);
                    }
                    else if (mr.Parent is AssignmentExpression ae && ae.Left == mr && ae.Parent is ExpressionStatement es)
                    {
                        if (ae.Right is InvocationExpression or ObjectCreateExpression || AstWalk.HasAny(ae.Right, static n => n is InvocationExpression or ObjectCreateExpression))
                            es.Expression = ae.Right.Detach();
                        else
                            es.Remove();
                    }
                    else if (mr.Parent is not DirectionExpression && !member.ReturnType.IsKnownType(KnownTypeCode.Void))
                    {
                        mr.ReplaceWith(new DefaultValueExpression(astBuilder.ConvertType(member.ReturnType)));
                    }
                    continue;
                }
            }

            if (mr.Target is ThisReferenceExpression or BaseReferenceExpression) continue;
            if (member.IsStatic || !(member.Accessibility == Accessibility.Protected || (crossAsm && member.Accessibility == Accessibility.ProtectedOrInternal))) continue;
            if (!IsStrictDerivedType(enclosingTd, declTd)) continue;
            var tgtT = mr.Target.Annotation<ResolveResult>()?.Type
                ?? (mr.Target as DefaultValueExpression)?.Type.Annotation<TypeResolveResult>()?.Type;
            if (tgtT is null || tgtT.Equals(enclosingTd) || IsStrictDerivedType(tgtT, enclosingTd)) continue;
            mr.Target = new ParenthesizedExpression(new CastExpression(
                astBuilder.ConvertType(enclosingTd),
                new CastExpression(new PrimitiveType("object"), mr.Target.Detach())));
        }

        foreach (var vds in staticVarDecls)
        {
            if (vds.Parent is null) continue;
            vds.Type = new PrimitiveType("object");
            foreach (var v in vds.Variables)
                if (v.Initializer is DefaultValueExpression) v.Initializer = new NullReferenceExpression();
        }
        foreach (var dve in staticDefaults)
            if (dve.Parent is not null)
                dve.ReplaceWith(new NullReferenceExpression());

        foreach (var u in bitNots)
        {
            if (u.Parent is null) continue;
            var ut = u.Expression.Annotation<ResolveResult>()?.Type;
            if (ut is { Kind: TypeKind.Struct }
                && ut.GetDefinition()?.KnownTypeCode is null or KnownTypeCode.None
                && !HasOnesComplementOperator(ut))
            {
                u.Expression = new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", new PrimitiveType("long")),
                    u.Expression.Detach());
            }
        }

        foreach (var c in boolCasts)
        {
            if (c.Parent is null) continue;
            var isBoolOrBoolIdx = c.Expression.Annotation<ResolveResult>()?.Type.IsKnownType(KnownTypeCode.Boolean) == true
                || (c.Expression is IndexerExpression ie && InferExprType(ie.Target) is { } colT
                    && ((colT as ArrayType)?.ElementType ?? (colT.TypeArguments.Count == 1 ? colT.TypeArguments[0] : null))?.IsKnownType(KnownTypeCode.Boolean) == true);
            if (isBoolOrBoolIdx
                && (c.Type.Annotation<TypeResolveResult>()?.Type ?? c.Annotation<ResolveResult>()?.Type) is { Kind: TypeKind.Class } dst
                && !dst.IsKnownType(KnownTypeCode.Object) && !dst.IsKnownType(KnownTypeCode.ValueType))
            {
                c.ReplaceWith(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new MemberType(new SimpleType("Il2CppStudio"), "Native"), "NativeIntrinsics")),
                        "Reinterpret", c.Type.Clone()),
                    c.Expression.Detach()));
            }
        }

        if (hasCustomEvents)
            RenameCollidingEventBackingFields(tree);
    }

    private static void RenameCollidingEventBackingFields(SyntaxTree tree)
    {
        foreach (var td in AstWalk.Collect<TypeDeclaration>(tree))
        {
            var customEvents = td.Members.OfType<CustomEventDeclaration>().Select(e => e.Name).ToHashSet(StringComparer.Ordinal);
            if (customEvents.Count == 0) continue;

            var taken = td.Members.SelectMany(m => m switch
            {
                FieldDeclaration f => f.Variables.Select(v => v.Name),
                _ => [m.Name],
            }).Where(n => !string.IsNullOrEmpty(n)).ToHashSet(StringComparer.Ordinal);

            void RewriteEventFieldRefs(string evName, string backing)
            {
                var ids = new List<IdentifierExpression>();
                var mrs = new List<MemberReferenceExpression>();
                AstWalk.ForEachNode(td, n =>
                {
                    if (n is IdentifierExpression id && id.Identifier == evName) ids.Add(id);
                    else if (n is MemberReferenceExpression mr && mr.MemberName == evName) mrs.Add(mr);
                });

                foreach (var id in ids)
                {
                    if (id.GetParent<TypeDeclaration>() != td) continue;
                    if (HasLocalOrParam(id, evName)) continue;
                    var inAccessor = id.GetParent<CustomEventDeclaration>() is { } ced && ced.Name == evName;
                    if (!inAccessor && id.Parent is AssignmentExpression ae && ae.Left == id
                        && ae.Operator is AssignmentOperatorType.Add or AssignmentOperatorType.Subtract)
                        continue;
                    id.Identifier = backing;
                }

                foreach (var mr in mrs)
                {
                    if (mr.GetParent<TypeDeclaration>() != td) continue;
                    var tdSym = td.GetSymbol() as ITypeDefinition;
                    var isOwnTarget = mr.Target is ThisReferenceExpression
                        || (mr.Target is TypeReferenceExpression tre
                            && (tre.Type is SimpleType st && st.Identifier == td.Name
                                || tre.Type is MemberType mt && mt.MemberName == td.Name))
                        || (mr.Target.Annotation<ResolveResult>()?.Type.GetDefinition() is { } tgtDef
                            && (tdSym is not null ? Equals(tgtDef, tdSym) : tgtDef.Name == td.Name));
                    if (!isOwnTarget) continue;
                    var inAccessor = mr.GetParent<CustomEventDeclaration>() is { } ced && ced.Name == evName;
                    if (!inAccessor && mr.Parent is AssignmentExpression ae && ae.Left == mr
                        && ae.Operator is AssignmentOperatorType.Add or AssignmentOperatorType.Subtract)
                        continue;
                    mr.MemberName = backing;
                }
            }

            foreach (var fd in td.Members.OfType<FieldDeclaration>().ToList())
            {
                foreach (var variable in fd.Variables)
                {
                    var name = variable.Name;
                    if (customEvents.Contains(name))
                    {
                        var backing = $"_backing_{name}";
                        while (!taken.Add(backing)) backing = "_" + backing;
                        variable.Name = backing;
                        RewriteEventFieldRefs(name, backing);
                    }
                    else if (name.StartsWith("m_", StringComparison.Ordinal) && customEvents.Contains(name[2..]))
                    {
                        RewriteEventFieldRefs(name[2..], name);
                    }
                }
            }
        }
    }

    private static bool HasLocalOrParam(IdentifierExpression id, string name)
    {
        for (AstNode? cur = id.Parent; cur is not null && cur is not TypeDeclaration; cur = cur.Parent)
        {
            if (cur is MethodDeclaration m && m.Parameters.Any(p => p.Name == name)) return true;
            if (cur is ConstructorDeclaration c && c.Parameters.Any(p => p.Name == name)) return true;
            if (cur is Accessor acc && name == "value") return true;
            if (cur is LambdaExpression lam && lam.Parameters.Any(p => p.Name == name)) return true;
            if (cur is AnonymousMethodExpression anon && anon.Parameters.Any(p => p.Name == name)) return true;
            if (cur is BlockStatement bs && bs.Statements.OfType<VariableDeclarationStatement>().Any(v => v.Variables.Any(vi => vi.Name == name)))
                return true;
        }
        return false;
    }

    private static bool IsStringTarget(Expression target)
    {
        var u = UnwrapParens(target);
        if ((target.Annotation<ResolveResult>() ?? u.Annotation<ResolveResult>())?.Type.IsKnownType(KnownTypeCode.String) == true)
            return true;
        if (u is CastExpression { Type: PrimitiveType { Keyword: "string" } })
            return true;
        if (u is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } rmr }
            && IsStringAstType(rmr.TypeArguments.FirstOrDefault()))
            return true;
        return false;
    }

    private static bool IsStrictDerivedType(IType derived, IType baseType) =>
        !derived.Equals(baseType) && derived.GetAllBaseTypes().Any(b => b.Equals(baseType));

    private static bool HasMemberCached(IType type, string name) =>
        type.GetDefinition()?.ParentModule?.Compilation is { } comp
            ? TypeSystemCache.For(comp).HasMember(type, name)
            : type.GetMembers(mem => mem.Name == name).Any();

    private static bool HasOnesComplementOperator(IType type)
    {
        if (type.GetDefinition()?.ParentModule?.Compilation is { } comp)
        {
            foreach (var m in TypeSystemCache.For(comp).GetMembers(type))
                if (m is IMethod && m.Name == "op_OnesComplement") return true;
            return false;
        }
        return type.GetMethods(m => m.Name == "op_OnesComplement").Any();
    }

    private static bool IsNonNullableValueTarget(Expression target)
    {
        var vt = target.Annotation<ResolveResult>()?.Type;
        if (vt is not null && vt.Kind is not (TypeKind.Unknown or TypeKind.TypeParameter))
            return vt.FullName != "System.Nullable" && !HasMemberCached(vt, "Value");
        if (target is DefaultValueExpression dve && dve.Type is not ComposedType { HasNullableSpecifier: true }
            && dve.Type is not SimpleType { Identifier: "Nullable" } && dve.Type is not MemberType { MemberName: "Nullable" })
        {
            var dt = dve.Type.Annotation<TypeResolveResult>()?.Type;
            return dt is not null && dt.Kind is not (TypeKind.Unknown or TypeKind.TypeParameter)
                && dt.FullName != "System.Nullable" && !HasMemberCached(dt, "Value");
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
        if (string.IsNullOrEmpty(name)) return false;
        var found = false;
        AstWalk.Visit<TypeDeclaration>(tree, td =>
        {
            if (!found && td.Modifiers.HasFlag(Modifiers.Abstract) && td.Name == name) found = true;
        });
        return found;
    }

    private static bool IsTypeNameInSet(HashSet<string> names, AstType type)
    {
        if (names.Count == 0) return false;
        var name = type switch { SimpleType s => s.Identifier, MemberType m => m.MemberName, _ => "" };
        return !string.IsNullOrEmpty(name) && names.Contains(name);
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
        DefaultValueExpression d => IsNintAstType(d.Type),
        CastExpression { Expression: PrimitiveExpression { Value: 0 } } c =>
            IsNintAstType(c.Type) || first is NullReferenceExpression,
        InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } mr } =>
            IsNintAstType(mr.TypeArguments.FirstOrDefault()),
        _ => false,
    };

    private static bool IsNintAstType(AstType? type) => type switch
    {
        PrimitiveType { Keyword: "nint" } => true,
        SimpleType { TypeArguments.Count: 0, Identifier: "nint" or "IntPtr" } => true,
        MemberType { TypeArguments.Count: 0, MemberName: "IntPtr", Target: SimpleType { TypeArguments.Count: 0, Identifier: "System" or "global::System" } or MemberType { MemberName: "System", Target: SimpleType { Identifier: "global" } } } => true,
        _ => false,
    };

    private static bool IsStringAstType(AstType? type) => type switch
    {
        PrimitiveType { Keyword: "string" } => true,
        SimpleType { TypeArguments.Count: 0, Identifier: "String" or "string" } => true,
        MemberType { TypeArguments.Count: 0, MemberName: "String", Target: SimpleType { TypeArguments.Count: 0, Identifier: "System" or "global::System" } or MemberType { MemberName: "System", Target: SimpleType { Identifier: "global" } } } => true,
        _ => false,
    };

    private static bool IsUnsafeTarget(Expression expr) => expr switch
    {
        TypeReferenceExpression { Type: SimpleType { TypeArguments.Count: 0, Identifier: var id } } => id.EndsWith("Unsafe", StringComparison.Ordinal),
        TypeReferenceExpression { Type: MemberType { TypeArguments.Count: 0, MemberName: var mn } } => mn.EndsWith("Unsafe", StringComparison.Ordinal),
        IdentifierExpression { TypeArguments.Count: 0, Identifier: var id } => id.EndsWith("Unsafe", StringComparison.Ordinal),
        MemberReferenceExpression { TypeArguments.Count: 0, MemberName: var mn } => mn.EndsWith("Unsafe", StringComparison.Ordinal),
        _ => expr.ToString().EndsWith("Unsafe", StringComparison.Ordinal),
    };

    private static bool IsReinterpretCall(Expression e) => e is InvocationExpression { Target: MemberReferenceExpression { MemberName: "Reinterpret" } };

    private static bool IsPrimitive(Expression e) =>
        e.Annotation<ResolveResult>()?.Type is { } t && t.GetDefinition()?.KnownTypeCode is >= KnownTypeCode.Boolean and <= KnownTypeCode.Double;

    private static bool IsAccessor(InvocationExpression call) =>
        call.Annotation<InvocationResolveResult>()?.Member is IMethod { IsAccessor: true } or null;

    private static bool SameGenericDefinition(AstType a, AstType b) =>
        a is SimpleType sa && b is SimpleType sb && sa.Identifier == sb.Identifier && sa.TypeArguments.Count == sb.TypeArguments.Count && sa.TypeArguments.Count > 0
        || a is MemberType ma && b is MemberType mb && ma.MemberName == mb.MemberName && ma.TypeArguments.Count == mb.TypeArguments.Count && ma.TypeArguments.Count > 0;

    private static void FoldDoubleUnaryMinus(SyntaxTree tree)
    {
        foreach (var u in AstWalk.Collect<UnaryOperatorExpression>(tree, static e => e.Operator == UnaryOperatorType.Minus))
        {
            if (u.Parent is null) continue;
            var rawInner = UnwrapParens(u.Expression);
            if (rawInner is PrimitiveExpression pe)
            {
                Expression? folded = pe.Value switch
                {
                    sbyte v when v < 0 && v != sbyte.MinValue => new PrimitiveExpression(-v),
                    short v when v < 0 && v != short.MinValue => new PrimitiveExpression(-v),
                    int v when v < 0 && v != int.MinValue => new PrimitiveExpression(-v),
                    long v when v < 0 && v != long.MinValue => new PrimitiveExpression(-v),
                    float v when v < 0f => new PrimitiveExpression(-v),
                    double v when v < 0d => new PrimitiveExpression(-v),
                    _ => null,
                };
                if (folded is not null)
                {
                    u.ReplaceWith(folded);
                    continue;
                }
                if (u.Expression is not ParenthesizedExpression && pe.Value is int.MinValue or long.MinValue)
                    u.Expression = new ParenthesizedExpression(u.Expression.Detach());
            }
            else if (rawInner is UnaryOperatorExpression { Operator: UnaryOperatorType.Minus } innerMinus)
            {
                if (UnwrapParens(innerMinus.Expression) is PrimitiveExpression innerPe)
                    u.ReplaceWith(innerPe.Detach());
                else if (u.Expression is not ParenthesizedExpression)
                    u.Expression = new ParenthesizedExpression(innerMinus.Detach());
            }
        }
    }
}
