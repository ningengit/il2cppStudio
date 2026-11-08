using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Reconstructs C# iterator methods (Unity coroutines) from their compiler-generated state machines: the factory
/// (<c>return new &lt;BootTimer&gt;d__14(0) { &lt;&gt;4__this = this };</c>) gets a <c>yield</c>-based body rebuilt from
/// <c>MoveNext</c>, and the state machine class — with its <c>&lt;&gt;1__state</c>, <c>&lt;&gt;2__current</c>,
/// explicit <c>IEnumerator.Current</c>/<c>Reset</c>/<c>Dispose</c> — is hidden. User locals keep their names.
/// Any shape that is not understood is left exactly as decompiled, and the reason is reported.
/// </summary>
public static class CoroutineTransform
{
    public static int Apply(SyntaxTree tree, Action<string, string>? report = null)
    {
        var rebuilt = 0;
        foreach (var type in tree.Descendants.OfType<TypeDeclaration>().ToList())
        {
            if (StateMachineInfo.TryCreate(type) is not { } info || type.Parent is not TypeDeclaration outer) continue;
            var factory = outer.Members.OfType<MethodDeclaration>().FirstOrDefault(m => m.Name == info.MethodName && !m.Body.IsNull
                && m.Body.Descendants.OfType<ObjectCreateExpression>().Any(o => o.Type.ToString().Contains(type.Name)));
            if (factory is null) { report?.Invoke(type.Name, "factory method not found"); continue; }

            var body = (BlockStatement)info.MoveNext.Body.Clone();
            YieldRewriter.NormalizeBoolResultReturn(body);
            var labels = new Dictionary<long, string>();
            var layout = DispatchLayout.Build(body, YieldRewriter.ResumeStates(body), labels);
            if (layout is null) { report?.Invoke(type.Name, "state dispatch at the top of MoveNext not recognized"); continue; }
            var newBody = new BlockStatement();
            foreach (var s in layout) newBody.Statements.Add(s);

            var parameters = factory.Parameters.Select(p => p.Name).ToHashSet();
            LocalCleanup.Run(newBody);
            if (!YieldRewriter.Rewrite(newBody, info, labels, parameters)) { report?.Invoke(type.Name, "yield transitions not recognized"); continue; }
            LocalCleanup.Run(newBody);
            GotoSimplifier.Simplify(newBody);
            LocalCleanup.Run(newBody);
            LocalCleanup.MergeDeclarations(newBody, info.HoistedLocals.Values);
            RemovePureDiscards(newBody);
            TempInliner.Run(newBody);
            var known = ScopeRepair.KnownLocals(info.MoveNext.Body, info.HoistedLocals.Values);
            foreach (var (n, t) in info.ClosureLocals) known.TryAdd(n, t);
            ScopeRepair.Run(newBody, known);
            if (!newBody.Descendants.OfType<YieldReturnStatement>().Any())
            {
                report?.Invoke(type.Name, "no 'yield return' recovered - the lifted MoveNext body is incomplete");
                continue;
            }

            var comment = new Comment($" Il2CppStudio: coroutine reconstructed from {type.Name}.MoveNext (state machine class hidden).");
            factory.Body = newBody;
            if (newBody.Statements.FirstOrDefault() is { } first) newBody.InsertChildBefore(first, comment, Roles.Comment);
            else newBody.AddChild(comment, Roles.Comment);
            ThisQualifier.Simplify(factory);
            foreach (var m in outer.Members.OfType<MethodDeclaration>().Where(m => m != factory && !m.Body.IsNull))
                TempInliner.Run(m.Body);
            FactoryCallSites.Collapse(outer, type.Name, factory);
            if (StillReferenced(outer, type))
                report?.Invoke(type.Name, "body reconstructed, but a call site still constructs the state machine directly - class kept so the code compiles");
            else RemoveWithComments(type);
            rebuilt++;
        }
        return rebuilt;
    }

    /// <summary><c>_ = this;</c> / <c>_ = local;</c> have no effect (leftover register reads of the outer instance).</summary>
    private static void RemovePureDiscards(BlockStatement body)
    {
        foreach (var s in body.Descendants.OfType<ExpressionStatement>().ToList())
            if (s.Expression is AssignmentExpression { Left: IdentifierExpression { Identifier: "_" }, Right: ThisReferenceExpression or IdentifierExpression })
                s.Remove();
    }

    private static bool StillReferenced(TypeDeclaration outer, TypeDeclaration machine) =>
        outer.Descendants.Any(n => !n.Ancestors.Contains(machine) && n != machine
            && n is SimpleType { Identifier: var id } && id == machine.Name);

    private static void RemoveWithComments(AstNode node)
    {
        while (node.PrevSibling is Comment c) c.Remove();
        node.Remove();
    }
}
