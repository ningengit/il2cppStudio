using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Rewritten <c>&lt;&gt;4__this.x</c> accesses become explicit <c>this.x</c>; drop the qualifier wherever no local
/// or parameter of the same name could capture the simple name, matching ILSpy's normal style.
/// </summary>
public static class ThisQualifier
{
    public static void Simplify(MethodDeclaration method)
    {
        var taken = method.Parameters.Select(p => p.Name)
            .Concat(method.Body.Descendants.OfType<VariableInitializer>().Select(v => v.Name))
            .Concat(method.Body.Descendants.OfType<CatchClause>().Select(c => c.VariableName))
            .ToHashSet();
        foreach (var m in method.Body.Descendants.OfType<MemberReferenceExpression>().ToList())
        {
            if (m.Target is not ThisReferenceExpression || taken.Contains(m.MemberName) || m.TypeArguments.Any()) continue;
            var id = new IdentifierExpression(m.MemberName);
            foreach (var ann in m.Annotations) id.AddAnnotation(ann);
            m.ReplaceWith(id);
        }
    }
}
