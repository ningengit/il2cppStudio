using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Thread-safe, per-<see cref="ICompilation"/> metadata cache replacing repeated O(Modules × Types) lookups in
/// <c>CompileHygiene</c>, <c>CompileFixups</c>, and <c>InlinedGetterRestorer</c>.
/// </summary>
internal sealed class TypeSystemCache
{
    private static readonly ConditionalWeakTable<ICompilation, TypeSystemCache> ByCompilation = new();

    private readonly Dictionary<(string Name, int Arity), HashSet<string>> _namespacesByTopLevelType;
    private readonly ConcurrentDictionary<IType, MemberCacheEntry> _membersByType = new();

    private sealed class MemberCacheEntry(IMember[] allMembers, Dictionary<string, IType> firstReturnTypeByName)
    {
        public IMember[] AllMembers { get; } = allMembers;
        public Dictionary<string, IType> FirstReturnTypeByName { get; } = firstReturnTypeByName;
    }

    private TypeSystemCache(ICompilation compilation)
    {
        var map = new Dictionary<(string Name, int Arity), HashSet<string>>();
        foreach (var module in compilation.Modules)
        {
            foreach (var td in module.TopLevelTypeDefinitions)
            {
                var ns = td.Namespace;
                if (string.IsNullOrEmpty(ns)) continue;
                var key = (td.Name, td.TypeParameterCount);
                if (!map.TryGetValue(key, out var set))
                {
                    set = new HashSet<string>(StringComparer.Ordinal);
                    map[key] = set;
                }
                set.Add(ns);
            }
        }
        _namespacesByTopLevelType = map;
    }

    public static TypeSystemCache For(ICompilation compilation) =>
        ByCompilation.GetValue(compilation, static c => new TypeSystemCache(c));

    /// <summary>
    /// O(1) ambiguity check equivalent to checking whether any other imported namespace in
    /// <paramref name="importedNamespaces"/> defines a top-level type with the same name and arity.
    /// </summary>
    public bool HasAmbiguousNamespace(string typeName, int arity, string defNamespace, HashSet<string> importedNamespaces)
    {
        if (!_namespacesByTopLevelType.TryGetValue((typeName, arity), out var namespaces) || namespaces.Count <= 1)
            return false;
        foreach (var ns in namespaces)
        {
            if (ns != defNamespace && importedNamespaces.Contains(ns))
                return true;
        }
        return false;
    }

    public bool HasMember(IType type, string memberName) =>
        GetOrBuildMembers(type).FirstReturnTypeByName.ContainsKey(memberName);

    public bool TryGetMemberReturnType(IType type, string memberName, out IType? returnType) =>
        GetOrBuildMembers(type).FirstReturnTypeByName.TryGetValue(memberName, out returnType);

    public IMember[] GetMembers(IType type) => GetOrBuildMembers(type).AllMembers;

    private MemberCacheEntry GetOrBuildMembers(IType type) =>
        _membersByType.GetOrAdd(type, static t =>
        {
            var members = t.GetMembers().ToArray();
            var byName = new Dictionary<string, IType>(members.Length, StringComparer.Ordinal);
            foreach (var m in members)
                byName.TryAdd(m.Name, m.ReturnType);
            return new MemberCacheEntry(members, byName);
        });

    /// <summary>Counts all <see cref="IdentifierExpression"/> occurrences per identifier in a subtree in one pass.</summary>
    public static Dictionary<string, int> CountIdentifiers(AstNode root)
    {
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        var stack = new Stack<AstNode>(32);
        var c = root.LastChild;
        while (c is not null) { stack.Push(c); c = c.PrevSibling; }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is IdentifierExpression id)
                counts[id.Identifier] = counts.GetValueOrDefault(id.Identifier) + 1;
            var ch = n.LastChild;
            while (ch is not null) { stack.Push(ch); ch = ch.PrevSibling; }
        }
        return counts;
    }
}
