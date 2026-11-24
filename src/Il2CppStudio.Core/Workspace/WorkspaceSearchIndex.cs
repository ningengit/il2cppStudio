using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

public readonly record struct SearchMatch(AssemblyModel Assembly, TypeModel Type, MethodModel? Method);

public readonly record struct SearchResult(IReadOnlyList<SearchMatch> Matches, int TotalMatches, bool IsTruncated);

/// <summary>
/// Precomputed flat search index over a loaded <see cref="MetadataModel"/>. Searching scans precomputed names
/// without allocating UI view-models or expanding tree hierarchies and stops producing <see cref="SearchMatch"/>
/// records once <c>maxResults</c> is reached.
/// </summary>
public sealed class WorkspaceSearchIndex
{
    private readonly record struct TypeEntry(AssemblyModel Assembly, TypeModel Type, string FullName);
    private readonly record struct MethodEntry(AssemblyModel Assembly, TypeModel Type, MethodModel Method, string Name);

    private readonly TypeEntry[] _types;
    private readonly MethodEntry[] _methods;

    private WorkspaceSearchIndex(TypeEntry[] types, MethodEntry[] methods)
    {
        _types = types;
        _methods = methods;
    }

    public int TypeCount => _types.Length;
    public int MethodCount => _methods.Length;

    public static WorkspaceSearchIndex Build(MetadataModel model) =>
        Build(model.Assemblies, model.Statistics.Types, model.Statistics.Methods);

    internal static WorkspaceSearchIndex Build(IEnumerable<AssemblyModel> sourceAssemblies, int estimatedTypes = 256, int estimatedMethods = 1024)
    {
        var types = new List<TypeEntry>(estimatedTypes);
        var methods = new List<MethodEntry>(estimatedMethods);

        foreach (var asm in sourceAssemblies.OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase))
        {
            foreach (var ns in asm.Namespaces)
            {
                foreach (var t in ns.Types)
                    CollectType(asm, t, types, methods);
            }
        }

        return new WorkspaceSearchIndex([.. types], [.. methods]);
    }

    private static void CollectType(AssemblyModel asm, TypeModel type, List<TypeEntry> types, List<MethodEntry> methods)
    {
        types.Add(new TypeEntry(asm, type, type.FullName));
        foreach (var m in type.Methods)
            methods.Add(new MethodEntry(asm, type, m, m.Name));
        foreach (var nested in type.NestedTypes)
            CollectType(asm, nested, types, methods);
    }

    public SearchResult Search(string query, int maxResults = 500, CancellationToken ct = default)
    {
        var filter = query?.Trim() ?? "";
        if (filter.Length == 0 || maxResults <= 0)
            return new SearchResult([], 0, false);

        var matches = new List<SearchMatch>(Math.Min(maxResults, 256));
        var total = 0;

        for (var i = 0; i < _types.Length; i++)
        {
            if ((i & 255) == 0) ct.ThrowIfCancellationRequested();
            ref readonly var entry = ref _types[i];
            if (!entry.FullName.Contains(filter, StringComparison.OrdinalIgnoreCase)) continue;
            total++;
            if (matches.Count < maxResults)
                matches.Add(new SearchMatch(entry.Assembly, entry.Type, null));
        }

        for (var i = 0; i < _methods.Length; i++)
        {
            if ((i & 255) == 0) ct.ThrowIfCancellationRequested();
            ref readonly var entry = ref _methods[i];
            if (!entry.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)) continue;
            total++;
            if (matches.Count < maxResults)
                matches.Add(new SearchMatch(entry.Assembly, entry.Type, entry.Method));
        }

        return new SearchResult(matches, total, total > matches.Count);
    }
}
