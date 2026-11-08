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
    public static IReadOnlyList<string> Materialize(Il2CppWorkspace ws, AssemblyModel asm, string referencesDir)
    {
        var bcl = new BclReferenceProvider(ws.Options.BclReferenceDirectory, ws.Environment.UnityVersion, null, Diagnostics.NullDiagnosticSink.Instance);
        var paths = new List<string>();
        foreach (var name in new[] { "mscorlib", "System", "System.Core" })
            if (bcl.Find(name) is { } p) paths.Add(PublicizedCopy(p, referencesDir));

        foreach (var dep in AssemblyClosure.Of(ws.Model, asm).Where(d => d.Name != asm.Name))
        {
            if (BclReferenceProvider.IsBclAssembly(dep.Name) && bcl.Find(dep.Name) is { } pristine) { paths.Add(PublicizedCopy(pristine, referencesDir)); continue; }
            var target = Path.Combine(referencesDir, dep.ImageName);
            if (!File.Exists(target))
            {
                var image = ws.Stubs.FindImage(dep.ImageName);
                if (image < 0 || ws.Decompiler.GetStubImage(image) is not { } bytes) continue;
                Directory.CreateDirectory(referencesDir);
                File.WriteAllBytes(target, ReferencePublicizer.Publicize(bytes));
            }
            paths.Add(Path.GetFullPath(target));
        }
        return paths;
    }

    /// <summary>Publicized copy of a pristine BCL assembly (IL2CPP also inlines BCL internals such as DateTime._dateData).</summary>
    private static string PublicizedCopy(string pristine, string referencesDir)
    {
        var target = Path.GetFullPath(Path.Combine(referencesDir, Path.GetFileName(pristine)));
        if (File.Exists(target)) return target;
        Directory.CreateDirectory(referencesDir);
        try { File.WriteAllBytes(target, ReferencePublicizer.Publicize(File.ReadAllBytes(pristine))); }
        catch (Exception ex) when (ex is BadImageFormatException or InvalidOperationException) { return pristine; }
        return target;
    }
}
