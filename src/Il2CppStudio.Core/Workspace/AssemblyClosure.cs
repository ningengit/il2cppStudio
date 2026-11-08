using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// The set of assemblies needed to build one exported project: the root plus everything it references, transitively.
/// Used by "Export Assembly + References" (GUI context menu) and <c>load --assembly X --with-references</c> (CLI).
/// References that are not part of the build (facades resolved elsewhere) are skipped.
/// </summary>
public static class AssemblyClosure
{
    public static IReadOnlyList<AssemblyModel> Of(MetadataModel model, AssemblyModel root)
    {
        var byName = new Dictionary<string, AssemblyModel>(StringComparer.OrdinalIgnoreCase);
        foreach (var a in model.Assemblies)
        {
            byName.TryAdd(a.Name, a);
            byName.TryAdd(Path.GetFileNameWithoutExtension(a.ImageName), a);
        }

        var result = new List<AssemblyModel>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var queue = new Queue<AssemblyModel>([root]);
        while (queue.TryDequeue(out var a))
        {
            if (!seen.Add(a.Name)) continue;
            result.Add(a);
            foreach (var r in a.References)
                if (byName.TryGetValue(SimpleName(r), out var dep) && !seen.Contains(dep.Name)) queue.Enqueue(dep);
        }
        return result;
    }

    /// <summary>"UnityEngine.CoreModule, Version=0.0.0.0, …" or "UnityEngine.CoreModule.dll" → "UnityEngine.CoreModule".</summary>
    private static string SimpleName(string reference)
    {
        var name = reference.Split(',')[0].Trim();
        return name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? name[..^4] : name;
    }

    public static Func<AssemblyModel, bool> Filter(IEnumerable<AssemblyModel> set)
    {
        var names = set.Select(a => a.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return a => names.Contains(a.Name);
    }
}
