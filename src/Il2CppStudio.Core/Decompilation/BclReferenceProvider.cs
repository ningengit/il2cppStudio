using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Locates pristine BCL assemblies used for "Substitute BCL (Clean/Fast)" mode. Search order:
/// explicit directory → Unity install for the detected editor version (MonoBleedingEdge unityaot-win32, the
/// exact class libraries IL2CPP compiles for the .NET Framework profile, then 4.7.1-api reference assemblies) →
/// any installed Unity editor → the build backup's Managed folder → .NET Framework reference assemblies.
/// </summary>
public sealed class BclReferenceProvider
{
    private static readonly string[] BclPrefixes = ["System.", "Microsoft.", "Mono."];
    private static readonly HashSet<string> BclNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "mscorlib", "netstandard", "System", "Mono.Security", "Microsoft.CSharp",
    };

    public IReadOnlyList<string> SearchDirectories { get; }

    public BclReferenceProvider(string? explicitDirectory, string? unityVersion, string? backupManagedFolder, IDiagnosticSink sink)
    {
        var dirs = new List<string>();
        if (explicitDirectory is not null) dirs.Add(explicitDirectory);

        var hub = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Unity", "Hub", "Editor");
        var versions = new List<string>();
        if (unityVersion is not null) versions.Add(unityVersion.Split(' ')[0]);
        if (Directory.Exists(hub))
            versions.AddRange(Directory.EnumerateDirectories(hub).Select(Path.GetFileName).OfType<string>().OrderDescending());
        foreach (var v in versions.Distinct())
        {
            var mono = Path.Combine(hub, v, "Editor", "Data", "MonoBleedingEdge", "lib", "mono");
            dirs.Add(Path.Combine(mono, "unityaot-win32"));
            dirs.Add(Path.Combine(mono, "4.7.1-api"));
        }
        if (backupManagedFolder is not null) dirs.Add(backupManagedFolder);
        var refAsm = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Reference Assemblies", "Microsoft", "Framework", ".NETFramework");
        dirs.Add(Path.Combine(refAsm, "v4.8"));
        dirs.Add(Path.Combine(refAsm, "v4.7.2"));

        SearchDirectories = [.. dirs.Where(d => File.Exists(Path.Combine(d, "mscorlib.dll"))).Distinct(StringComparer.OrdinalIgnoreCase)];
        if (SearchDirectories.Count == 0)
            sink.Warn(DiagnosticCategory.Decompilation, DiagnosticCodes.BclReferenceMissing,
                "No BCL reference assemblies found; BCL types will be decompiled from generated stubs.");
        else
            sink.Info(DiagnosticCategory.Decompilation, DiagnosticCodes.BclSubstituted,
                $"BCL reference directory: {SearchDirectories[0]}");
    }

    public static bool IsBclAssembly(string assemblyName)
    {
        var name = assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? assemblyName[..^4] : assemblyName;
        return BclNames.Contains(name) || BclPrefixes.Any(p => name.StartsWith(p, StringComparison.OrdinalIgnoreCase));
    }

    public string? Find(string assemblyName)
    {
        var file = assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? assemblyName : assemblyName + ".dll";
        foreach (var d in SearchDirectories)
        {
            var p = Path.Combine(d, file);
            if (File.Exists(p)) return p;
            var facade = Path.Combine(d, "Facades", file);
            if (File.Exists(facade)) return facade;
        }
        return null;
    }
}
