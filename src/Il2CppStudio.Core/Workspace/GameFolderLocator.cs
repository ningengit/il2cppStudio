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
        binary ??= Directory.EnumerateFiles(path, "libil2cpp.so", SearchOption.AllDirectories).FirstOrDefault();

        var metadataCandidates = Directory.EnumerateFiles(path, "global-metadata.dat", SearchOption.AllDirectories)
            .Where(p => !p.Contains("BackUpThisFolder", StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (metadataCandidates.Count > 1)
            sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.MultipleCandidates,
                $"{metadataCandidates.Count} global-metadata.dat files found; using '{metadataCandidates[0]}'.", new() { File = path });
        var metadata = metadataCandidates.FirstOrDefault();

        var dataFolder = Directory.EnumerateDirectories(path, "*_Data").FirstOrDefault();
        var backup = Directory.EnumerateDirectories(path, "*BackUpThisFolder_ButDontShipItWithYourGame").FirstOrDefault();

        if (binary is null)
            sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.BinaryNotFound,
                "No GameAssembly.dll / libil2cpp.so found. If Managed/Assembly-CSharp.dll exists this is a Mono build, not IL2CPP.", new() { File = path });
        if (metadata is null)
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound, "global-metadata.dat not found.", new() { File = path });

        return new GameFiles(path, binary, metadata, dataFolder, backup);
    }
}
