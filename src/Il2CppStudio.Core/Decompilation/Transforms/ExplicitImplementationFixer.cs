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
        var flags = AstWalk.ScanFlags(tree);
        Apply(tree, in flags);
    }

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        var explicitDecls = new List<(EntityDeclaration Member, string Iface, string Name)>();
        var explicitAccesses = new List<(MemberReferenceExpression Access, string Iface, string Name)>();
        var stateMachines = flags.HasCompilerGeneratedTypes || AstWalk.HasAny<TypeDeclaration>(tree)
            ? new List<(TypeDeclaration Type, string Iface)>()
            : null;

        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case SimpleType st:
                    var strippedSt = StripExternAlias(st.Identifier);
                    if (!ReferenceEquals(strippedSt, st.Identifier)) st.Identifier = strippedSt;
                    break;
                case MemberType mt:
                    var strippedMt = StripExternAlias(mt.MemberName);
                    if (!ReferenceEquals(strippedMt, mt.MemberName)) mt.MemberName = strippedMt;
                    break;
                case MethodDeclaration or PropertyDeclaration or EventDeclaration when Split(((EntityDeclaration)n).Name) is var (iface, name):
                    explicitDecls.Add(((EntityDeclaration)n, iface, name));
                    break;
                case MemberReferenceExpression mr when Split(mr.MemberName) is var (iface, name):
                    explicitAccesses.Add((mr, iface, name));
                    break;
                case TypeDeclaration td when stateMachines is not null && td.BaseTypes.Count > 0:
                    string? smIface = null;
                    foreach (var bt in td.BaseTypes)
                    {
                        var candidate = StateMachineInterfaceName(bt);
                        if (candidate is not null)
                        {
                            smIface = candidate;
                            if (candidate == "System.Runtime.CompilerServices.IAsyncStateMachine") break;
                        }
                    }
                    if (smIface is not null)
                        stateMachines.Add((td, smIface));
                    break;
            }
        });

        foreach (var (member, rawIface, name) in explicitDecls)
        {
            var iface = StripExternAlias(rawIface);
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

        foreach (var (access, rawIface, name) in explicitAccesses)
        {
            if (access.Parent is null) continue;
            var iface = StripExternAlias(rawIface);
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

        if (stateMachines is not null)
        {
            foreach (var (type, iface) in stateMachines)
            {
                var isAsync = iface.EndsWith("IAsyncStateMachine", StringComparison.Ordinal);
                foreach (var m in type.Members.OfType<MethodDeclaration>())
                {
                    var explicitName = m.Name == "MoveNext" && m.Parameters.Count == 0 || isAsync && m.Name == "SetStateMachine";
                    if (!explicitName || !m.PrivateImplementationType.IsNull || (m.Modifiers & Modifiers.Public) != 0) continue;
                    m.Modifiers = Modifiers.None;
                    m.PrivateImplementationType = new SimpleType(iface);
                }
            }
        }
    }

    private static string? StateMachineInterfaceName(AstType bt) => bt switch
    {
        SimpleType { TypeArguments.Count: 0, Identifier: "IAsyncStateMachine" } => "System.Runtime.CompilerServices.IAsyncStateMachine",
        SimpleType { TypeArguments.Count: 0, Identifier: "IEnumerator" } => "System.Collections.IEnumerator",
        MemberType { TypeArguments.Count: 0, MemberName: "IAsyncStateMachine" } => "System.Runtime.CompilerServices.IAsyncStateMachine",
        MemberType { TypeArguments.Count: 0, MemberName: "IEnumerator", Target: MemberType { MemberName: "Collections", Target: SimpleType { Identifier: "System" or "global::System" } or MemberType { MemberName: "System" } } } => "System.Collections.IEnumerator",
        _ => null,
    };

    private static bool ContainsOpenTypeParam(string iface) =>
        iface.Contains("<TKey", StringComparison.Ordinal) || iface.Contains("TValue>", StringComparison.Ordinal)
        || iface.Contains("<T>", StringComparison.Ordinal);

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
