using System.Collections.Concurrent;
using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Reference set for a generated project: pristine Unity BCL assemblies (mscorlib, System, System.Core …) from the
/// editor install, and stub DLLs (exact signatures from metadata) for every other dependency, written once into
/// <c>&lt;export&gt;/_references</c>. Works for a single-assembly export as well as an export-all.
/// </summary>
public static class ProjectReferences
{
    /// <summary>Materializes every unique reference across all target assemblies once in a flat parallel pass.</summary>
    public static void WarmAll(
        Il2CppWorkspace ws,
        IEnumerable<AssemblyModel> assemblies,
        string referencesDir,
        BclReferenceProvider sharedBcl,
        ConcurrentDictionary<string, Lazy<string?>> materialized)
    {
        var fullRefDir = Path.GetFullPath(referencesDir);
        var uniqueDeps = new Dictionary<string, AssemblyModel>(StringComparer.OrdinalIgnoreCase);
        foreach (var asm in assemblies)
        {
            foreach (var dep in AssemblyClosure.Of(ws.Model, asm))
                if (dep.Name != asm.Name)
                    uniqueDeps.TryAdd(dep.Name, dep);
        }

        var tasks = new List<Lazy<string?>>();
        foreach (var name in new[] { "mscorlib", "System", "System.Core" })
        {
            if (sharedBcl.Find(name) is { } p)
                tasks.Add(GetOrCreateBclLazy(p, fullRefDir, materialized));
        }

        foreach (var dep in uniqueDeps.Values)
        {
            if (BclReferenceProvider.IsBclAssembly(dep.Name) && sharedBcl.Find(dep.Name) is { } pristine)
            {
                tasks.Add(GetOrCreateBclLazy(pristine, fullRefDir, materialized));
                continue;
            }
            var target = Path.GetFullPath(Path.Combine(fullRefDir, dep.ImageName));
            tasks.Add(materialized.GetOrAdd(target, _ => new Lazy<string?>(
                () => WriteStubReference(ws, dep, fullRefDir, target),
                LazyThreadSafetyMode.ExecutionAndPublication)));
        }

        if (tasks.Count > 1)
            Parallel.ForEach(tasks, static lazy => _ = lazy.Value);
        else if (tasks.Count == 1)
            _ = tasks[0].Value;
    }

    public static IReadOnlyList<string> Materialize(
        Il2CppWorkspace ws,
        AssemblyModel asm,
        string referencesDir,
        BclReferenceProvider? sharedBcl = null,
        ConcurrentDictionary<string, Lazy<string?>>? materialized = null)
    {
        var bcl = sharedBcl ?? new BclReferenceProvider(
            ws.Options.BclReferenceDirectory, ws.Environment.UnityVersion, null, Diagnostics.NullDiagnosticSink.Instance);
        var fullRefDir = Path.GetFullPath(referencesDir);
        var paths = new List<string>();
        var coreBcl = new List<(string Pristine, Lazy<string?>? Lazy)>();
        foreach (var name in new[] { "mscorlib", "System", "System.Core" })
        {
            if (bcl.Find(name) is not { } p) continue;
            if (materialized is not null)
                coreBcl.Add((p, GetOrCreateBclLazy(p, fullRefDir, materialized)));
            else
                paths.Add(PublicizedCopyCore(p, fullRefDir, Path.GetFullPath(Path.Combine(fullRefDir, Path.GetFileName(p)))));
        }

        var depEntries = new List<(string? PristineFallback, Lazy<string?> Lazy)>();
        var uncachedStubs = new List<(AssemblyModel Dep, string Target)>();
        foreach (var dep in AssemblyClosure.Of(ws.Model, asm).Where(d => d.Name != asm.Name))
        {
            if (BclReferenceProvider.IsBclAssembly(dep.Name) && bcl.Find(dep.Name) is { } pristine)
            {
                if (materialized is not null)
                    depEntries.Add((pristine, GetOrCreateBclLazy(pristine, fullRefDir, materialized)));
                else
                    paths.Add(PublicizedCopyCore(pristine, fullRefDir, Path.GetFullPath(Path.Combine(fullRefDir, Path.GetFileName(pristine)))));
                continue;
            }
            var target = Path.GetFullPath(Path.Combine(fullRefDir, dep.ImageName));
            if (materialized is not null)
            {
                var lazy = materialized.GetOrAdd(target, _ => new Lazy<string?>(
                    () => WriteStubReference(ws, dep, fullRefDir, target),
                    LazyThreadSafetyMode.ExecutionAndPublication));
                depEntries.Add((null, lazy));
            }
            else
            {
                uncachedStubs.Add((dep, target));
            }
        }

        if (materialized is not null)
        {
            var pending = new List<Lazy<string?>>();
            foreach (var (_, lazy) in coreBcl)
                if (lazy is { IsValueCreated: false }) pending.Add(lazy);
            foreach (var (_, lazy) in depEntries)
                if (!lazy.IsValueCreated) pending.Add(lazy);
            if (pending.Count > 1)
                Parallel.ForEach(pending, lazy => _ = lazy.Value);

            foreach (var (pristine, lazy) in coreBcl)
                paths.Add(lazy?.Value ?? pristine);
            foreach (var (fallback, lazy) in depEntries)
                if ((lazy.Value ?? fallback) is { } resolved)
                    paths.Add(resolved);
        }
        else if (uncachedStubs.Count > 0)
        {
            var resolved = new string?[uncachedStubs.Count];
            if (uncachedStubs.Count > 1)
                Parallel.For(0, uncachedStubs.Count, i => resolved[i] = WriteStubReference(ws, uncachedStubs[i].Dep, fullRefDir, uncachedStubs[i].Target));
            else
                resolved[0] = WriteStubReference(ws, uncachedStubs[0].Dep, fullRefDir, uncachedStubs[0].Target);
            foreach (var r in resolved)
                if (r is not null) paths.Add(r);
        }
        return paths;
    }

    private static Lazy<string?> GetOrCreateBclLazy(string pristine, string fullRefDir, ConcurrentDictionary<string, Lazy<string?>> materialized)
    {
        var target = Path.GetFullPath(Path.Combine(fullRefDir, Path.GetFileName(pristine)));
        return materialized.GetOrAdd(target, _ => new Lazy<string?>(
            () => PublicizedCopyCore(pristine, fullRefDir, target),
            LazyThreadSafetyMode.ExecutionAndPublication));
    }

    private static string? WriteStubReference(Il2CppWorkspace ws, AssemblyModel dep, string referencesDir, string target)
    {
        var image = ws.Stubs.FindImage(dep.ImageName);
        if (image >= 0 && ws.Decompiler.GetStubImage(image) is { } bytes)
        {
            WriteAtomic(target, referencesDir, ReferencePublicizer.Publicize(bytes));
            return target;
        }
        return File.Exists(target) ? target : null;
    }

    /// <summary>Publicized copy of a pristine BCL assembly (IL2CPP also inlines BCL internals such as DateTime._dateData).</summary>
    private static string PublicizedCopy(string pristine, string referencesDir, ConcurrentDictionary<string, Lazy<string?>>? materialized)
    {
        var target = Path.GetFullPath(Path.Combine(referencesDir, Path.GetFileName(pristine)));
        if (materialized is not null)
        {
            return materialized.GetOrAdd(target, _ => new Lazy<string?>(() => PublicizedCopyCore(pristine, referencesDir, target), LazyThreadSafetyMode.ExecutionAndPublication)).Value
                ?? pristine;
        }
        return PublicizedCopyCore(pristine, referencesDir, target);
    }

    private static string PublicizedCopyCore(string pristine, string referencesDir, string target)
    {
        if (File.Exists(target)) return target;
        try
        {
            WriteAtomic(target, referencesDir, ReferencePublicizer.Publicize(File.ReadAllBytes(pristine)));
            return target;
        }
        catch (Exception ex) when (ex is BadImageFormatException or InvalidOperationException) { return pristine; }
    }

    private static void WriteAtomic(string target, string referencesDir, byte[] content)
    {
        Directory.CreateDirectory(referencesDir);
        var tmp = Path.Combine(referencesDir, $"{Path.GetFileName(target)}.{Guid.NewGuid():N}.tmp");
        try
        {
            File.WriteAllBytes(tmp, content);
            File.Move(tmp, target, overwrite: true);
        }
        catch (IOException) when (File.Exists(target))
        {
            // Another worker already materialized this reference concurrently.
        }
        finally
        {
            try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
        }
    }
}
