using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// IL2CPP metadata names explicit interface implementations <c>System.Collections.IEnumerator.Current</c> but the stub
/// modules carry no MethodImpl records, so they print as members with dotted names. This restores C#:
/// declarations become <c>object IEnumerator.Current =&gt; …</c> (no modifiers), and accesses
/// <c>x.System.Collections.ICollection.Count</c> become <c>((System.Collections.ICollection)x).Count</c>.
/// </summary>
public static class ExplicitImplementationFixer
{
    private static string StripExternAlias(string name) =>
        name.IndexOf("::", StringComparison.Ordinal) is > 0 and var c
        && !name.StartsWith("global::", StringComparison.Ordinal)
            ? name[(c + 2)..]
            : name;

    public static void Apply(SyntaxTree tree)
    {
        foreach (var st in tree.Descendants.OfType<SimpleType>()) st.Identifier = StripExternAlias(st.Identifier);
        foreach (var mt in tree.Descendants.OfType<MemberType>()) mt.MemberName = StripExternAlias(mt.MemberName);
        foreach (var member in tree.Descendants.OfType<EntityDeclaration>().ToList())
        {
            if (member is not (MethodDeclaration or PropertyDeclaration or EventDeclaration) || Split(member.Name) is not var (iface, name)) continue;
            iface = StripExternAlias(iface);
            if (member is MethodDeclaration { PrivateImplementationType.IsNull: false } or PropertyDeclaration { PrivateImplementationType.IsNull: false }) continue;
            if (member is MethodDeclaration m && ((name.StartsWith("get_", StringComparison.Ordinal) && m.Parameters.Count == 0)
                || (name.StartsWith("set_", StringComparison.Ordinal) && m.Parameters.Count == 1)))
            {
                var isGetter = name.StartsWith("get_", StringComparison.Ordinal);
                var propName = name[4..];
                var existing = (m.Parent as TypeDeclaration)?.Members.OfType<PropertyDeclaration>().FirstOrDefault(p =>
                    p.Name == propName && !p.PrivateImplementationType.IsNull && p.PrivateImplementationType.ToString() == iface);
                var acc = new Accessor { Body = m.Body.Detach() };
                if (isGetter)
                {
                    if (existing is not null && existing.Getter.IsNull) { existing.Getter = acc; m.Remove(); }
                    else m.ReplaceWith(new PropertyDeclaration { ReturnType = m.ReturnType.Clone(), PrivateImplementationType = new SimpleType(iface), Name = propName, Getter = acc });
                }
                else
                {
                    if (existing is not null && existing.Setter.IsNull) { existing.Setter = acc; m.Remove(); }
                    else m.ReplaceWith(new PropertyDeclaration { ReturnType = m.Parameters.Single().Type.Clone(), PrivateImplementationType = new SimpleType(iface), Name = propName, Setter = acc });
                }
                continue;
            }
            member.Modifiers = Modifiers.None;
            member.Name = name;
            switch (member)
            {
                case MethodDeclaration md: md.PrivateImplementationType = new SimpleType(iface); break;
                case PropertyDeclaration p: p.PrivateImplementationType = new SimpleType(iface); break;
                case CustomEventDeclaration e: e.PrivateImplementationType = new SimpleType(iface); break;
            }
        }
        foreach (var access in tree.Descendants.OfType<MemberReferenceExpression>().ToList())
        {
            if (access.Parent is null || Split(access.MemberName) is not var (iface, name)) continue;
            iface = StripExternAlias(iface);
            var target = access.Target is ThisReferenceExpression or BaseReferenceExpression ? new ThisReferenceExpression() : access.Target.Detach();
            if (ContainsOpenTypeParam(iface))
            {
                while (target is CastExpression or ParenthesizedExpression)
                    target = target is CastExpression c ? c.Expression.Detach() : ((ParenthesizedExpression)target).Expression.Detach();
                access.Target = target;
                access.MemberName = name;
                continue;
            }
            access.Target = new ParenthesizedExpression(new CastExpression(new SimpleType(iface), target));
            access.MemberName = name;
        }
        FixStateMachines(tree);
    }

    private static bool ContainsOpenTypeParam(string iface) =>
        iface.Contains("<TKey", StringComparison.Ordinal) || iface.Contains("TValue>", StringComparison.Ordinal)
        || iface.Contains("<T>", StringComparison.Ordinal);

    /// <summary>
    /// Compiler-generated state machines implement <c>IEnumerator.MoveNext</c> / <c>IAsyncStateMachine.MoveNext</c> and
    /// <c>SetStateMachine</c> with private methods plus MethodImpl records; in C# that is an explicit implementation.
    /// </summary>
    private static void FixStateMachines(SyntaxTree tree)
    {
        foreach (var type in tree.Descendants.OfType<TypeDeclaration>())
        {
            var bases = type.BaseTypes.Select(b => b.ToString()).ToList();
            var iface = bases.Any(b => b.EndsWith("IAsyncStateMachine", StringComparison.Ordinal)) ? "System.Runtime.CompilerServices.IAsyncStateMachine"
                : bases.Any(b => b is "IEnumerator" or "System.Collections.IEnumerator") ? "System.Collections.IEnumerator" : null;
            if (iface is null) continue;
            foreach (var m in type.Members.OfType<MethodDeclaration>())
            {
                var explicitName = m.Name == "MoveNext" && m.Parameters.Count == 0 || iface.EndsWith("IAsyncStateMachine", StringComparison.Ordinal) && m.Name == "SetStateMachine";
                if (!explicitName || !m.PrivateImplementationType.IsNull || (m.Modifiers & Modifiers.Public) != 0) continue;
                m.Modifiers = Modifiers.None;
                m.PrivateImplementationType = new SimpleType(iface);
            }
        }
    }

    /// <summary>"System.Collections.Generic.IEnumerator&lt;System.Object&gt;.Current" → (interface, "Current"); null for plain names.</summary>
    public static (string Interface, string Name)? Split(string member)
    {
        var depth = 0;
        for (var i = member.Length - 1; i > 0; i--)
        {
            var c = member[i];
            if (c == '>') depth++;
            else if (c == '<') depth--;
            else if (c == '.' && depth == 0) return (member[..i], member[(i + 1)..]);
        }
        return null;
    }
}
