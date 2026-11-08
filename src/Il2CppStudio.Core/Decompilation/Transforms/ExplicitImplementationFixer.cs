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
    public static void Apply(SyntaxTree tree)
    {
        foreach (var member in tree.Descendants.OfType<EntityDeclaration>().ToList())
        {
            if (member is not (MethodDeclaration or PropertyDeclaration or EventDeclaration) || Split(member.Name) is not var (iface, name)) continue;
            if (member is MethodDeclaration { PrivateImplementationType.IsNull: false } or PropertyDeclaration { PrivateImplementationType.IsNull: false }) continue;
            member.Modifiers = Modifiers.None;
            member.Name = name;
            switch (member)
            {
                case MethodDeclaration m: m.PrivateImplementationType = new SimpleType(iface); break;
                case PropertyDeclaration p: p.PrivateImplementationType = new SimpleType(iface); break;
                case CustomEventDeclaration e: e.PrivateImplementationType = new SimpleType(iface); break;
            }
        }
        foreach (var access in tree.Descendants.OfType<MemberReferenceExpression>().ToList())
        {
            if (access.Parent is null || Split(access.MemberName) is not var (iface, name)) continue;
            var target = access.Target is ThisReferenceExpression or BaseReferenceExpression ? new ThisReferenceExpression() : access.Target.Detach();
            access.Target = new ParenthesizedExpression(new CastExpression(new SimpleType(iface), target));
            access.MemberName = name;
        }
        FixStateMachines(tree);
    }

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
