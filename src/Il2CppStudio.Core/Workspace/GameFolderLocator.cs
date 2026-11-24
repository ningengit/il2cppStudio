using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Plugins;

namespace Il2CppStudio.Core.Workspace;

public sealed record GameFiles(string Root, string? BinaryPath, string? MetadataPath, string? DataFolder, string? BackupFolder);

/// <summary>
/// Locates the IL2CPP binary, global-metadata.dat and (optionally) the build backup folder from a game
/// root, a *_Data folder, an APK/XAPK archive, or a direct file path.
/// </summary>
public static class GameFolderLocator
{
    private static readonly string[] BinaryNames = ["GameAssembly.dll", "GameAssembly.so", "GameAssembly.dylib", "libil2cpp.so", "Il2CppUserAssemblies.prx"];

    public static GameFiles Locate(string path, IDiagnosticSink sink) => Locate(path, sink, null);

    public static GameFiles Locate(string path, IDiagnosticSink sink, WorkspaceOptions? options) =>
        LocateCore(path, sink, explicitBinary: null, options);

    private static GameFiles LocateCore(string path, IDiagnosticSink sink, string? explicitBinary, WorkspaceOptions? options = null)
    {
        path = Path.GetFullPath(path);

        if (explicitBinary is null && File.Exists(path))
        {
            if (ApkExtractor.IsApkPath(path))
            {
                return ApkExtractor.Extract(path, sink) is { } extractedApk
                    ? LocateCore(extractedApk, sink, explicitBinary: null)
                    : new GameFiles(path, null, null, null, null);
            }

            if (IpaExtractor.IsIpaPath(path))
            {
                return IpaExtractor.Extract(path, sink) is { } extractedIpa
                    ? LocateCore(extractedIpa, sink, explicitBinary: null)
                    : new GameFiles(path, null, null, null, null);
            }

            if (Ps4PkgExtractor.IsPkgPath(path))
            {
                return Ps4PkgExtractor.Extract(path, sink, options?.Ps4PkgPasscode) is { } extractedPkg
                    ? LocateCore(extractedPkg, sink, explicitBinary: null)
                    : new GameFiles(path, null, null, null, null);
            }

            if (SwitchNspExtractor.IsNspPath(path))
            {
                return SwitchNspExtractor.Extract(path, sink, options?.SwitchKeysetPath) is { } extractedNsp
                    ? LocateCore(extractedNsp, sink, explicitBinary: null)
                    : new GameFiles(path, null, null, null, null);
            }

            var name = Path.GetFileName(path);
            if (name.Equals("global-metadata.dat", StringComparison.OrdinalIgnoreCase))
                return new GameFiles(Path.GetDirectoryName(path)!, null, path, null, null);

            if (BinaryNames.Contains(name, StringComparer.OrdinalIgnoreCase)
                || name.Equals("UnityFramework", StringComparison.OrdinalIgnoreCase)
                || ConsoleBinaryPluginRegistry.CanOpen(path)
                || MachOBinaryImage.IsMachO(path))
            {
                var root = FindAncestorGameRoot(Path.GetDirectoryName(path)!);
                return LocateCore(root, sink, explicitBinary: path);
            }

            if (WasmFiles.IsWasmPath(path) || IsWebDataPath(path))
            {
                var dir = Path.GetDirectoryName(path)!;
                var root = Path.GetFileName(dir).Equals("Build", StringComparison.OrdinalIgnoreCase)
                    ? (Directory.GetParent(dir)?.FullName ?? dir)
                    : dir;
                return LocateCore(root, sink, explicitBinary: WasmFiles.IsWasmPath(path) ? path : null);
            }
        }

        if (!Directory.Exists(path))
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound, $"'{path}' does not exist.", new() { File = path });
            return new GameFiles(path, null, null, null, null);
        }

        var binary = explicitBinary;
        binary ??= BinaryNames.Select(n => Path.Combine(path, n)).FirstOrDefault(File.Exists);
        binary ??= FindMachOBinary(path);
        binary ??= PreferAbi(Directory.EnumerateFiles(path, "libil2cpp.so", SearchOption.AllDirectories));
        binary ??= new[] { Path.Combine(path, "main"), Path.Combine(path, "exefs", "main"), Path.Combine(path, "0", "main") }
            .Concat(Directory.EnumerateFiles(path, "main", SearchOption.AllDirectories))
            .FirstOrDefault(p => File.Exists(p) && ConsoleBinaryPluginRegistry.CanOpen(p));
        binary ??= new[] { Path.Combine(path, "Media", "Modules", "Il2CppUserAssemblies.prx"), Path.Combine(path, "Il2CppUserAssemblies.prx") }
            .Concat(Directory.EnumerateFiles(path, "Il2CppUserAssemblies.prx", SearchOption.AllDirectories))
            .FirstOrDefault(p => File.Exists(p) && (ElfBinaryImage.IsElf(p) || ConsoleBinaryPluginRegistry.CanOpen(p)));

        // WebGL: no native binary; metadata lives inside the (optionally brotli/gzip-compressed) UnityWebData container.
        string? wasm = null;
        var searchRoot = path;
        if ((binary is null || WasmFiles.IsWasmPath(binary)) && WebDataExtractor.FindDataFile(path) is { } webData)
        {
            if (binary is null) wasm = WasmFiles.Find(path);
            if (!Directory.EnumerateFiles(path, "global-metadata.dat", SearchOption.AllDirectories).Any()
                && WebDataExtractor.Extract(webData, sink) is { } extracted)
                searchRoot = extracted;
        }

        var metadataCandidates = Directory.EnumerateFiles(searchRoot, "global-metadata.dat", SearchOption.AllDirectories)
            .Where(p => !p.Contains("BackUpThisFolder", StringComparison.OrdinalIgnoreCase))
            .ToList();
        // Android ships one tree per ABI: take the metadata from the same tree as the chosen binary.
        if (binary is not null && metadataCandidates.Count > 1)
            metadataCandidates = [.. metadataCandidates.OrderByDescending(m => CommonPrefix(m, binary))];
        if (metadataCandidates.Count > 1)
            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.MultipleCandidates,
                $"{metadataCandidates.Count} global-metadata.dat files found; using '{metadataCandidates[0]}'.", new() { File = path });
        var metadata = metadataCandidates.FirstOrDefault();

        var dataFolder = FindDataFolder(path, searchRoot, metadata);
        var backup = Directory.EnumerateDirectories(path, "*BackUpThisFolder_ButDontShipItWithYourGame").FirstOrDefault();

        if (binary is null && wasm is not null)
        {
            binary = wasm;   // WebGL: the WebAssembly module carries the registration data (linear-memory data segments) and the code
            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.BinaryNotFound,
                $"WebGL build: using WebAssembly module '{Path.GetFileName(wasm)}'.", new() { File = path });
        }
        else if (binary is null)
            sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.BinaryNotFound,
                "No GameAssembly.dll / libil2cpp.so found. If Managed/Assembly-CSharp.dll exists this is a Mono build, not IL2CPP.", new() { File = path });
        if (metadata is null)
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound, "global-metadata.dat not found.", new() { File = path });

        return new GameFiles(path, binary, metadata, dataFolder, backup);
    }

    private static string? FindDataFolder(string path, string searchRoot, string? metadata)
    {
        if (Directory.EnumerateDirectories(path, "*_Data").FirstOrDefault() is { } pcData)
            return pcData;
        var androidData = Path.Combine(path, "assets", "bin", "Data");
        if (Directory.Exists(androidData))
            return androidData;
        var switchData = Path.Combine(path, "romfs", "Data");
        if (Directory.Exists(switchData))
            return switchData;
        var switchTitleData = Path.Combine(path, "1", "Data");
        if (Directory.Exists(switchTitleData))
            return switchTitleData;
        var ps4Media = Path.Combine(path, "Media");
        if (Directory.Exists(ps4Media) && (File.Exists(Path.Combine(ps4Media, "globalgamemanagers")) || File.Exists(Path.Combine(ps4Media, "data.unity3d"))))
            return ps4Media;
        var macData = Path.Combine(path, "Contents", "Resources", "Data");
        if (Directory.Exists(macData))
            return macData;
        var directData = Path.Combine(path, "Data");
        if (Directory.Exists(directData))
            return directData;
        if (searchRoot != path)
            return searchRoot;
        if (metadata is not null)
        {
            for (var d = Path.GetDirectoryName(metadata); d is not null; d = Directory.GetParent(d)?.FullName)
            {
                if (File.Exists(Path.Combine(d, "data.unity3d")) || File.Exists(Path.Combine(d, "globalgamemanagers")))
                    return d;
                if (string.Equals(d, path, StringComparison.OrdinalIgnoreCase))
                    break;
            }
        }
        return null;
    }

    private static string? FindMachOBinary(string path)
    {
        var macFramework = Path.Combine(path, "Contents", "Frameworks", "UnityFramework.framework", "UnityFramework");
        if (File.Exists(macFramework) && MachOBinaryImage.IsMachO(macFramework)) return macFramework;
        var macFrameworkVersioned = Path.Combine(path, "Contents", "Frameworks", "UnityFramework.framework", "Versions", "A", "UnityFramework");
        if (File.Exists(macFrameworkVersioned) && MachOBinaryImage.IsMachO(macFrameworkVersioned)) return macFrameworkVersioned;
        var macDylib = Path.Combine(path, "Contents", "Frameworks", "GameAssembly.dylib");
        if (File.Exists(macDylib) && MachOBinaryImage.IsMachO(macDylib)) return macDylib;

        var iosFramework = Path.Combine(path, "Frameworks", "UnityFramework.framework", "UnityFramework");
        if (File.Exists(iosFramework) && MachOBinaryImage.IsMachO(iosFramework)) return iosFramework;

        var payloadDir = Path.Combine(path, "Payload");
        if (Directory.Exists(payloadDir))
        {
            foreach (var appDir in Directory.EnumerateDirectories(payloadDir, "*.app"))
            {
                var uf = Path.Combine(appDir, "Frameworks", "UnityFramework.framework", "UnityFramework");
                if (File.Exists(uf) && MachOBinaryImage.IsMachO(uf)) return uf;
                var exeName = Path.GetFileNameWithoutExtension(appDir);
                var exe = Path.Combine(appDir, exeName);
                if (File.Exists(exe) && MachOBinaryImage.IsMachO(exe)) return exe;
            }
        }

        foreach (var uf in Directory.EnumerateFiles(path, "UnityFramework", SearchOption.AllDirectories))
        {
            if (MachOBinaryImage.IsMachO(uf)) return uf;
        }

        var macOsDir = Path.Combine(path, "Contents", "MacOS");
        if (Directory.Exists(macOsDir))
        {
            foreach (var exe in Directory.EnumerateFiles(macOsDir))
                if (MachOBinaryImage.IsMachO(exe)) return exe;
        }

        if (path.EndsWith(".app", StringComparison.OrdinalIgnoreCase))
        {
            var appExe = Path.Combine(path, Path.GetFileNameWithoutExtension(path));
            if (File.Exists(appExe) && MachOBinaryImage.IsMachO(appExe)) return appExe;
        }

        return null;
    }

    private static string FindAncestorGameRoot(string dir)
    {
        var cur = dir;
        for (var i = 0; i < 5 && cur is not null; i++)
        {
            if (Directory.EnumerateDirectories(cur, "*_Data").Any()
                || Directory.Exists(Path.Combine(cur, "assets", "bin", "Data"))
                || Directory.Exists(Path.Combine(cur, "romfs", "Data"))
                || Directory.Exists(Path.Combine(cur, "1", "Data"))
                || Directory.Exists(Path.Combine(cur, "Media", "Metadata"))
                || Directory.Exists(Path.Combine(cur, "Data", "Managed", "Metadata"))
                || Directory.Exists(Path.Combine(cur, "Contents", "Resources", "Data"))
                || File.Exists(Path.Combine(cur, "Media", "globalgamemanagers"))
                || Directory.EnumerateFiles(cur, "global-metadata.dat", SearchOption.AllDirectories).Any())
                return cur;
            cur = Directory.GetParent(cur)?.FullName;
        }
        return dir;
    }

    private static bool IsWebDataPath(string path) =>
        path.EndsWith(".data", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".data.br", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".data.gz", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".data.unityweb", StringComparison.OrdinalIgnoreCase);

    /// <summary>Android multi-ABI: prefer 64-bit (arm64-v8a, x86_64) over 32-bit libraries.</summary>
    private static string? PreferAbi(IEnumerable<string> libs) =>
        libs.OrderBy(l => l.Contains("arm64", StringComparison.OrdinalIgnoreCase) ? 0 : l.Contains("x86_64", StringComparison.OrdinalIgnoreCase) ? 1 : 2).FirstOrDefault();

    private static int CommonPrefix(string a, string b)
    {
        var n = 0;
        while (n < a.Length && n < b.Length && char.ToLowerInvariant(a[n]) == char.ToLowerInvariant(b[n])) n++;
        return n;
    }
}
