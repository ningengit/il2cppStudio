using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// IL2CPP inlines iterator factories into their callers, so <c>StartCoroutine(BootTimer())</c> lifts as
/// <c>var d = new &lt;BootTimer&gt;d__14(0); d.&lt;&gt;1__state = 0; d.&lt;&gt;4__this = this; StartCoroutine(d);</c>.
/// This folds that sequence back into a call of the reconstructed iterator method.
/// </summary>
public static class FactoryCallSites
{
    public static int Collapse(TypeDeclaration outer, string machineName, MethodDeclaration factory)
    {
        var parameters = factory.Parameters.Select(p => p.Name).ToList();
        var count = 0;
        foreach (var decl in outer.Descendants.OfType<VariableDeclarationStatement>().ToList())
        {
            if (decl.Ancestors.Contains(factory) || decl.Parent is not BlockStatement block) continue;
            if (decl.Variables.Count != 1 || decl.Variables.First() is not { } v
                || v.Initializer is not ObjectCreateExpression oce || !oce.Type.ToString().Contains(machineName, StringComparison.Ordinal)) continue;

            var args = new Dictionary<string, Expression>();
            var setup = new List<Statement>();
            var siblings = block.Statements.ToList();
            var index = siblings.IndexOf(decl) + 1;
            Statement? Next() => index < siblings.Count ? siblings[index++] : null;
            var cursor = Next();
            while (cursor is ExpressionStatement { Expression: AssignmentExpression { Left: MemberReferenceExpression { Target: IdentifierExpression id } m } a }
                   && id.Identifier == v.Name)
            {
                var param = m.MemberName.StartsWith("<>3__", StringComparison.Ordinal) ? m.MemberName[5..] : m.MemberName;
                if (m.MemberName is StateMachineInfo.StateField) { }
                else if (m.MemberName is StateMachineInfo.ThisField && a.Right is ThisReferenceExpression) { }
                else if (parameters.Contains(param)) args[param] = a.Right;
                else break;
                setup.Add(cursor);
                cursor = Next();
            }
            if (!parameters.All(args.ContainsKey)) continue;

            var uses = block.Descendants.OfType<IdentifierExpression>().Where(i => i.Identifier == v.Name && !setup.Any(s => i.Ancestors.Contains(s))).ToList();
            if (uses.Count == 0 && cursor is ExpressionStatement { Expression: InvocationExpression { Arguments.Count: 1 } inv }
                && inv.Arguments.Single() is IdentifierExpression orphan
                && !block.Descendants.OfType<AssignmentExpression>().Any(a => a.Left is IdentifierExpression l && l.Identifier == orphan.Identifier))
                uses.Add(orphan);
            if (uses.Count != 1) continue;

            var call = new InvocationExpression(new IdentifierExpression(factory.Name), parameters.Select(p => args[p].Clone()));
            uses[0].ReplaceWith(call);
            foreach (var s in setup) s.Remove();
            decl.Remove();
            count++;
        }
        return count;
    }
}
