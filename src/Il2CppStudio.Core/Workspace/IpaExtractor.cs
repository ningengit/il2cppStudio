using System.IO.Compression;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts the iOS IL2CPP binary (<c>Payload/&lt;App&gt;.app/Frameworks/UnityFramework.framework/UnityFramework</c>,
/// falling back to the main app executable), <c>global-metadata.dat</c>, and Unity data headers from an <c>.ipa</c>
/// archive into a cached temp folder without modifying the source archive.
/// </summary>
public static class IpaExtractor
{
    private static readonly string[] WantedExactNames =
    [
        "global-metadata.dat",
        "data.unity3d",
        "globalgamemanagers",
        "mainData",
        "ScriptingAssemblies.json",
        "boot.config",
        "Info.plist",
    ];

    public static bool IsIpaPath(string path) =>
        Path.GetExtension(path).Equals(".ipa", StringComparison.OrdinalIgnoreCase);

    public static string? Extract(string ipaPath, IDiagnosticSink sink)
    {
        var info = new FileInfo(ipaPath);
        if (!info.Exists)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{ipaPath}' does not exist.", new() { File = ipaPath });
            return null;
        }

        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "ipa",
            $"{Path.GetFileName(ipaPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}");
        if (File.Exists(Path.Combine(cache, ".complete")) && HasMetadata(cache))
            return cache;

        Directory.CreateDirectory(cache);
        var fullCachePrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                              + Path.DirectorySeparatorChar;

        var count = 0;
        try
        {
            using var archive = ZipFile.OpenRead(ipaPath);
            string? appRootPrefix = null;
            string? appExeName = null;
            var hasUnityFramework = false;

            foreach (var entry in archive.Entries)
            {
                var norm = entry.FullName.Replace('\\', '/').TrimStart('/');
                if (!norm.StartsWith("Payload/", StringComparison.OrdinalIgnoreCase)) continue;
                var rest = norm["Payload/".Length..];
                var slash = rest.IndexOf('/');
                if (slash <= 4 || !rest[..slash].EndsWith(".app", StringComparison.OrdinalIgnoreCase)) continue;

                appRootPrefix ??= "Payload/" + rest[..(slash + 1)];
                appExeName ??= Path.GetFileNameWithoutExtension(rest[..slash]);
                var rel = rest[(slash + 1)..];
                if (rel.Equals("Frameworks/UnityFramework.framework/UnityFramework", StringComparison.OrdinalIgnoreCase))
                    hasUnityFramework = true;
            }

            foreach (var entry in archive.Entries)
            {
                var norm = entry.FullName.Replace('\\', '/').TrimStart('/');
                if (norm.Length == 0 || norm.EndsWith('/')) continue;
                if (!IsWantedEntry(norm, appRootPrefix, appExeName, hasUnityFramework)) continue;

                var target = Path.GetFullPath(Path.Combine(cache, norm.Replace('/', Path.DirectorySeparatorChar)));
                if (!target.StartsWith(fullCachePrefix, StringComparison.OrdinalIgnoreCase)) continue;
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                entry.ExtractToFile(target, overwrite: true);
                count++;
            }
        }
        catch (Exception ex)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract '{Path.GetFileName(ipaPath)}': {ex.Message}", new() { File = ipaPath }, ex);
            return null;
        }

        if (count > 0 && HasMetadata(cache))
            File.WriteAllText(Path.Combine(cache, ".complete"), ipaPath);
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
            $"IPA: extracted {count} IL2CPP files from '{Path.GetFileName(ipaPath)}' to '{cache}'.",
            new() { File = ipaPath });
        return cache;
    }

    public static VirtualArchiveSession? OpenZeroCopy(string ipaPath, IDiagnosticSink sink)
    {
        if (!File.Exists(ipaPath))
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{ipaPath}' does not exist.", new() { File = ipaPath });
            return null;
        }

        ZipZeroCopyReader? reader = null;
        VirtualFileBuffer? metadata = null;
        string? metadataVirtual = null;
        VirtualFileBuffer? binary = null;
        string? binaryVirtual = null;
        var dataFiles = new Dictionary<string, VirtualFileBuffer>(StringComparer.OrdinalIgnoreCase);

        try
        {
            reader = ZipZeroCopyReader.Open(ipaPath);
            string? appRootPrefix = null;
            string? appExeName = null;
            var hasUnityFramework = false;

            foreach (var entry in reader.Entries)
            {
                var norm = entry.FullName.Replace('\\', '/').TrimStart('/');
                if (!norm.StartsWith("Payload/", StringComparison.OrdinalIgnoreCase)) continue;
                var rest = norm["Payload/".Length..];
                var slash = rest.IndexOf('/');
                if (slash <= 4 || !rest[..slash].EndsWith(".app", StringComparison.OrdinalIgnoreCase)) continue;

                appRootPrefix ??= "Payload/" + rest[..(slash + 1)];
                appExeName ??= Path.GetFileNameWithoutExtension(rest[..slash]);
                var rel = rest[(slash + 1)..];
                if (rel.Equals("Frameworks/UnityFramework.framework/UnityFramework", StringComparison.OrdinalIgnoreCase))
                    hasUnityFramework = true;
            }

            foreach (var entry in reader.Entries)
            {
                var norm = entry.FullName.Replace('\\', '/').TrimStart('/');
                if (norm.Length == 0 || norm.EndsWith('/')) continue;
                if (!IsWantedEntry(norm, appRootPrefix, appExeName, hasUnityFramework)) continue;

                var vPath = $"{ipaPath}::{norm}";
                var fileName = Path.GetFileName(norm);
                if (fileName.Equals("global-metadata.dat", StringComparison.OrdinalIgnoreCase))
                {
                    metadata ??= reader.OpenBuffer(entry);
                    metadataVirtual ??= vPath;
                    continue;
                }

                if (norm.EndsWith("/Frameworks/UnityFramework.framework/UnityFramework", StringComparison.OrdinalIgnoreCase)
                    || (!hasUnityFramework && appRootPrefix is not null && !string.IsNullOrEmpty(appExeName)
                        && norm.Equals(appRootPrefix + appExeName, StringComparison.OrdinalIgnoreCase)))
                {
                    binary ??= reader.OpenBuffer(entry);
                    binaryVirtual ??= vPath;
                    continue;
                }

                if (!dataFiles.ContainsKey(fileName))
                    dataFiles[fileName] = reader.OpenBuffer(entry);
            }

            var session = new VirtualArchiveSession
            {
                PrimaryArchivePath = ipaPath,
                Metadata = metadata,
                MetadataVirtualPath = metadataVirtual,
                Binary = binary,
                BinaryVirtualPath = binaryVirtual,
                DataFiles = dataFiles,
            };
            session.AddOwner(reader);
            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
                $"IPA: mapped '{Path.GetFileName(ipaPath)}' in-memory (zero-copy metadata={metadata?.IsZeroCopy == true}, zero-copy binary={binary?.IsZeroCopy == true}).",
                new() { File = ipaPath });
            return session;
        }
        catch (Exception ex)
        {
            metadata?.Dispose();
            binary?.Dispose();
            foreach (var d in dataFiles.Values) d.Dispose();
            reader?.Dispose();
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to open '{Path.GetFileName(ipaPath)}': {ex.Message}", new() { File = ipaPath }, ex);
            return null;
        }
    }

    private static bool HasMetadata(string dir) =>
        Directory.Exists(dir) && Directory.EnumerateFiles(dir, "global-metadata.dat", SearchOption.AllDirectories).Any();

    private static bool IsWantedEntry(string norm, string? appRootPrefix, string? appExeName, bool hasUnityFramework)
    {
        if (appRootPrefix is not null && norm.StartsWith(appRootPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var rel = norm[appRootPrefix.Length..];
            if (rel.Equals("Frameworks/UnityFramework.framework/UnityFramework", StringComparison.OrdinalIgnoreCase))
                return true;
            if (!hasUnityFramework && !string.IsNullOrEmpty(appExeName) && rel.Equals(appExeName, StringComparison.OrdinalIgnoreCase))
                return true;
            if (rel.Equals("Info.plist", StringComparison.OrdinalIgnoreCase))
                return true;
            if (rel.StartsWith("Data/Managed/Metadata/global-metadata.dat", StringComparison.OrdinalIgnoreCase))
                return true;
            if (rel.StartsWith("Data/Managed/Resources/", StringComparison.OrdinalIgnoreCase)
                && rel.EndsWith("-resources.dat", StringComparison.OrdinalIgnoreCase))
                return true;
            if (rel.StartsWith("Data/", StringComparison.OrdinalIgnoreCase) && !rel["Data/".Length..].Contains('/', StringComparison.Ordinal))
            {
                var fileName = Path.GetFileName(rel);
                if (WantedExactNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        var name = Path.GetFileName(norm);
        return WantedExactNames.Contains(name, StringComparer.OrdinalIgnoreCase);
    }
}
