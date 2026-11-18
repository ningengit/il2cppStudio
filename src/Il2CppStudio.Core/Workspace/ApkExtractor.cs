using System.IO.Compression;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts the IL2CPP binary (<c>lib/&lt;abi&gt;/libil2cpp.so</c>), <c>global-metadata.dat</c>, and Unity data
/// headers (<c>data.unity3d</c>, <c>globalgamemanagers</c>, etc.) from an <c>.apk</c> or split <c>.xapk</c> archive
/// into a cached temp folder without modifying the source archive.
/// </summary>
public static class ApkExtractor
{
    private static readonly string[] WantedExactNames =
    [
        "global-metadata.dat",
        "data.unity3d",
        "globalgamemanagers",
        "mainData",
        "ScriptingAssemblies.json",
        "app.info",
        "boot.config",
    ];

    public static bool IsApkPath(string path)
    {
        var ext = Path.GetExtension(path);
        return ext.Equals(".apk", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".xapk", StringComparison.OrdinalIgnoreCase);
    }

    public static string? Extract(string apkPath, IDiagnosticSink sink)
    {
        var info = new FileInfo(apkPath);
        if (!info.Exists)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{apkPath}' does not exist.", new() { File = apkPath });
            return null;
        }

        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "apk",
            $"{Path.GetFileName(apkPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}");
        if (File.Exists(Path.Combine(cache, ".complete")))
            return cache;

        Directory.CreateDirectory(cache);
        var fullCachePrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                              + Path.DirectorySeparatorChar;

        var count = 0;
        try
        {
            using var outer = ZipFile.OpenRead(apkPath);
            count += ExtractFromArchive(outer, cache, fullCachePrefix);

            if (Path.GetExtension(apkPath).Equals(".xapk", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var entry in outer.Entries)
                {
                    if (!entry.Name.EndsWith(".apk", StringComparison.OrdinalIgnoreCase) || entry.Length == 0)
                        continue;

                    // Copy nested split APK to a temporary delete-on-close file so ZipArchive has a seekable stream.
                    var tempSplitPath = Path.Combine(cache, $"_split_{Guid.NewGuid():N}.tmp");
                    try
                    {
                        using (var src = entry.Open())
                        using (var dst = new FileStream(tempSplitPath, FileMode.Create, FileAccess.ReadWrite,
                                   FileShare.None, 81920, FileOptions.DeleteOnClose))
                        {
                            src.CopyTo(dst);
                            dst.Position = 0;
                            using var inner = new ZipArchive(dst, ZipArchiveMode.Read, leaveOpen: true);
                            count += ExtractFromArchive(inner, cache, fullCachePrefix);
                        }
                    }
                    catch
                    {
                        if (File.Exists(tempSplitPath))
                            try { File.Delete(tempSplitPath); } catch { /* best-effort cleanup */ }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract '{Path.GetFileName(apkPath)}': {ex.Message}", new() { File = apkPath }, ex);
            return null;
        }

        File.WriteAllText(Path.Combine(cache, ".complete"), apkPath);
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
            $"APK: extracted {count} IL2CPP files from '{Path.GetFileName(apkPath)}' to '{cache}'.",
            new() { File = apkPath });
        return cache;
    }

    private static int ExtractFromArchive(ZipArchive archive, string cache, string fullCachePrefix)
    {
        var count = 0;
        foreach (var entry in archive.Entries)
        {
            var norm = entry.FullName.Replace('\\', '/').TrimStart('/');
            if (norm.Length == 0 || norm.EndsWith('/')) continue;
            if (!IsWantedEntry(norm)) continue;

            var target = Path.GetFullPath(Path.Combine(cache, norm.Replace('/', Path.DirectorySeparatorChar)));
            if (!target.StartsWith(fullCachePrefix, StringComparison.OrdinalIgnoreCase)) continue;
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            entry.ExtractToFile(target, overwrite: true);
            count++;
        }
        return count;
    }

    private static bool IsWantedEntry(string norm)
    {
        if (norm.StartsWith("lib/", StringComparison.OrdinalIgnoreCase) &&
            norm.EndsWith("/libil2cpp.so", StringComparison.OrdinalIgnoreCase))
            return true;

        var fileName = Path.GetFileName(norm);
        if (!WantedExactNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
            return false;

        // Avoid pulling unrelated files from deep asset packs unless under assets/bin/Data/
        return norm.StartsWith("assets/bin/Data/", StringComparison.OrdinalIgnoreCase)
            || !norm.Contains('/');
    }
}
