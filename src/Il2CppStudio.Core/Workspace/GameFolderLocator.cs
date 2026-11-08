using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

public sealed record GameFiles(string Root, string? BinaryPath, string? MetadataPath, string? DataFolder, string? BackupFolder);

/// <summary>
/// Locates the IL2CPP binary, global-metadata.dat and (optionally) the build backup folder from a game
/// root, a *_Data folder, or a direct file path. Windows layout first; Android/Linux names also accepted.
/// </summary>
public static class GameFolderLocator
{
    private static readonly string[] BinaryNames = ["GameAssembly.dll", "GameAssembly.so", "libil2cpp.so"];

    public static GameFiles Locate(string path, IDiagnosticSink sink)
    {
        path = Path.GetFullPath(path);

        if (File.Exists(path))
        {
            var name = Path.GetFileName(path);
            if (name.Equals("global-metadata.dat", StringComparison.OrdinalIgnoreCase))
                return new GameFiles(Path.GetDirectoryName(path)!, null, path, null, null);
            if (BinaryNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                return Locate(Path.GetDirectoryName(path)!, sink) with { BinaryPath = path };
        }

        if (!Directory.Exists(path))
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound, $"'{path}' does not exist.", new() { File = path });
            return new GameFiles(path, null, null, null, null);
        }

        string? binary = BinaryNames.Select(n => Path.Combine(path, n)).FirstOrDefault(File.Exists);
        binary ??= PreferAbi(Directory.EnumerateFiles(path, "libil2cpp.so", SearchOption.AllDirectories));

        // WebGL: no native binary; metadata lives inside the (optionally brotli/gzip-compressed) UnityWebData container.
        string? wasm = null;
        var searchRoot = path;
        if (binary is null && WebDataExtractor.FindDataFile(path) is { } webData)
        {
            wasm = WasmFiles.Find(path);
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

        var dataFolder = Directory.EnumerateDirectories(path, "*_Data").FirstOrDefault();
        var backup = Directory.EnumerateDirectories(path, "*BackUpThisFolder_ButDontShipItWithYourGame").FirstOrDefault();

        if (binary is null && wasm is not null)
        {
            binary = wasm;   // WebGL: the WebAssembly module carries the registration data (linear-memory data segments) and the code
            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.BinaryNotFound,
                $"WebGL build: using WebAssembly module '{Path.GetFileName(wasm)}' (registration + disassembly; method bodies are not lifted).", new() { File = path });
        }
        else if (binary is null)
            sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.BinaryNotFound,
                "No GameAssembly.dll / libil2cpp.so found. If Managed/Assembly-CSharp.dll exists this is a Mono build, not IL2CPP.", new() { File = path });
        if (metadata is null)
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound, "global-metadata.dat not found.", new() { File = path });

        return new GameFiles(path, binary, metadata, dataFolder, backup);
    }

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
