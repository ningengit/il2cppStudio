using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts the IL2CPP binary (<c>lib/&lt;abi&gt;/libil2cpp.so</c>), <c>global-metadata.dat</c>, and Unity data
/// headers (<c>data.unity3d</c>, <c>globalgamemanagers</c>, etc.) from an <c>.apk</c>, split APK bundle, or <c>.xapk</c> archive
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
            || ext.Equals(".xapk", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".apks", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".apkm", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsSplitContainerPath(string path)
    {
        var ext = Path.GetExtension(path);
        return ext.Equals(".xapk", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".apks", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".apkm", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Detects an extracted Android split-APK bundle directory (<c>base.apk</c> + <c>split_config.*.apk</c> or
    /// <c>splits/base-*.apk</c>). Requires at least 2 APKs and a split-bundle naming indicator so folders of
    /// unrelated standalone APKs are not merged.
    /// </summary>
    public static bool TryFindSplitApkBundle(string dir, out string baseApk, out string[] splitApks)
    {
        baseApk = string.Empty;
        splitApks = [];
        if (!Directory.Exists(dir)) return false;

        var apks = Directory.GetFiles(dir, "*.apk", SearchOption.TopDirectoryOnly);
        if (apks.Length < 2)
        {
            var splitsSubdir = Path.Combine(dir, "splits");
            if (Directory.Exists(splitsSubdir))
                apks = Directory.GetFiles(splitsSubdir, "*.apk", SearchOption.TopDirectoryOnly);
        }
        if (apks.Length < 2) return false;

        static bool IsBaseLike(string fileName) =>
            fileName.Equals("base.apk", StringComparison.OrdinalIgnoreCase)
            || fileName.StartsWith("base-", StringComparison.OrdinalIgnoreCase)
            || fileName.StartsWith("base.", StringComparison.OrdinalIgnoreCase);

        static bool IsSplitLike(string fileName) =>
            fileName.StartsWith("split_", StringComparison.OrdinalIgnoreCase)
            || fileName.StartsWith("split.", StringComparison.OrdinalIgnoreCase)
            || fileName.StartsWith("config.", StringComparison.OrdinalIgnoreCase);

        static bool IsAbiOrDensitySplit(string fileName) =>
            IsSplitLike(fileName)
            || fileName.Contains("arm64", StringComparison.OrdinalIgnoreCase)
            || fileName.Contains("armeabi", StringComparison.OrdinalIgnoreCase)
            || fileName.Contains("x86", StringComparison.OrdinalIgnoreCase)
            || fileName.Contains("dpi", StringComparison.OrdinalIgnoreCase);

        var hasIndicator = apks.Any(p =>
        {
            var fn = Path.GetFileName(p);
            return IsBaseLike(fn) || IsSplitLike(fn);
        });
        if (!hasIndicator) return false;

        var ordered = apks
            .OrderByDescending(p => Path.GetFileName(p).Equals("base.apk", StringComparison.OrdinalIgnoreCase))
            .ThenByDescending(p =>
            {
                var fn = Path.GetFileName(p);
                return fn.Equals("base-master.apk", StringComparison.OrdinalIgnoreCase)
                    || fn.Equals("base.master.apk", StringComparison.OrdinalIgnoreCase);
            })
            .ThenByDescending(p =>
            {
                var fn = Path.GetFileName(p);
                return IsBaseLike(fn) && !IsAbiOrDensitySplit(fn);
            })
            .ThenByDescending(p => new FileInfo(p).Length)
            .ThenBy(p => Path.GetFileName(p), StringComparer.OrdinalIgnoreCase)
            .ToArray();

        baseApk = ordered[0];
        splitApks = [.. ordered.Skip(1).OrderBy(p => Path.GetFileName(p), StringComparer.OrdinalIgnoreCase)];
        return true;
    }

    public static string? Extract(string apkPath, IDiagnosticSink sink) =>
        ExtractBundle(apkPath, null, null, sink);

    public static string? ExtractBundle(
        string baseApkPath,
        IReadOnlyList<string>? splitApkPaths,
        string? nativeLibraryDir,
        IDiagnosticSink sink)
    {
        var info = new FileInfo(baseApkPath);
        if (!info.Exists)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{baseApkPath}' does not exist.", new() { File = baseApkPath });
            return null;
        }

        var splits = (splitApkPaths ?? [])
            .Where(p => !string.IsNullOrWhiteSpace(p) && !string.Equals(p, baseApkPath, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToArray();

        var nativeSoPath = !string.IsNullOrWhiteSpace(nativeLibraryDir)
            ? Path.Combine(nativeLibraryDir, "libil2cpp.so")
            : null;
        var hasNativeOnDisk = nativeSoPath is not null && File.Exists(nativeSoPath);

        string cacheKey;
        if (splits.Length == 0 && !hasNativeOnDisk)
        {
            cacheKey = $"{Path.GetFileName(baseApkPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}";
        }
        else
        {
            var sb = new StringBuilder();
            sb.Append(baseApkPath).Append('|').Append(info.Length).Append('|').Append(info.LastWriteTimeUtc.Ticks);
            foreach (var sp in splits)
            {
                var si = new FileInfo(sp);
                sb.Append('|').Append(sp).Append(':').Append(si.Exists ? si.Length : -1).Append(':').Append(si.Exists ? si.LastWriteTimeUtc.Ticks : 0);
            }
            if (hasNativeOnDisk && nativeSoPath is not null)
            {
                var ni = new FileInfo(nativeSoPath);
                sb.Append("|nat:").Append(nativeSoPath).Append(':').Append(ni.Length).Append(':').Append(ni.LastWriteTimeUtc.Ticks);
            }
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString())))[..16];
            cacheKey = $"{Path.GetFileName(baseApkPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}-{hash}";
        }

        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "apk", cacheKey);
        if (File.Exists(Path.Combine(cache, ".complete")) && HasMetadata(cache))
            return cache;

        Directory.CreateDirectory(cache);
        var fullCachePrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                              + Path.DirectorySeparatorChar;

        var count = 0;
        var hasBinary = false;
        try
        {
            using (var outer = ZipFile.OpenRead(baseApkPath))
            {
                count += ExtractFromArchive(outer, cache, fullCachePrefix, ref hasBinary);

                if (IsSplitContainerPath(baseApkPath))
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
                                count += ExtractFromArchive(inner, cache, fullCachePrefix, ref hasBinary);
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

            foreach (var splitPath in splits)
            {
                if (!File.Exists(splitPath)) continue;
                try
                {
                    using var splitZip = ZipFile.OpenRead(splitPath);
                    count += ExtractFromArchive(splitZip, cache, fullCachePrefix, ref hasBinary);
                }
                catch
                {
                    // Skip unreadable/non-zip split entries gracefully.
                }
            }

            if (!hasBinary && hasNativeOnDisk && nativeSoPath is not null)
            {
                var abi = DetectAndroidAbiFolder(nativeLibraryDir!, nativeSoPath);
                var dest = Path.Combine(cache, "lib", abi, "libil2cpp.so");
                Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
                File.Copy(nativeSoPath, dest, overwrite: true);
                count++;
                hasBinary = true;
            }
        }
        catch (Exception ex)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract '{Path.GetFileName(baseApkPath)}': {ex.Message}", new() { File = baseApkPath }, ex);
            return null;
        }

        if (count > 0 && HasMetadata(cache))
            File.WriteAllText(Path.Combine(cache, ".complete"), baseApkPath);
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
            $"APK: extracted {count} IL2CPP files from '{Path.GetFileName(baseApkPath)}' to '{cache}'.",
            new() { File = baseApkPath });
        return cache;
    }

    internal static string DetectAndroidAbiFolder(string nativeLibDir, string soPath)
    {
        var leaf = Path.GetFileName(nativeLibDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        if (leaf.Contains("arm64", StringComparison.OrdinalIgnoreCase)) return "arm64-v8a";
        if (leaf.Equals("arm", StringComparison.OrdinalIgnoreCase) || leaf.Contains("armeabi", StringComparison.OrdinalIgnoreCase)) return "armeabi-v7a";
        if (leaf.Equals("x86_64", StringComparison.OrdinalIgnoreCase) || leaf.Equals("x64", StringComparison.OrdinalIgnoreCase)) return "x86_64";
        if (leaf.Equals("x86", StringComparison.OrdinalIgnoreCase)) return "x86";

        try
        {
            Span<byte> hdr = stackalloc byte[20];
            using var fs = File.OpenRead(soPath);
            if (fs.Read(hdr) == 20 && hdr[0] == 0x7F && hdr[1] == (byte)'E' && hdr[2] == (byte)'L' && hdr[3] == (byte)'F')
            {
                var machine = BinaryPrimitives.ReadUInt16LittleEndian(hdr.Slice(18, 2));
                return machine switch
                {
                    0xB7 => "arm64-v8a",
                    0x28 => "armeabi-v7a",
                    0x3E => "x86_64",
                    0x03 => "x86",
                    _ => "arm64-v8a",
                };
            }
        }
        catch { /* default below */ }

        return "arm64-v8a";
    }

    private static bool HasMetadata(string dir) =>
        Directory.Exists(dir) && Directory.EnumerateFiles(dir, "global-metadata.dat", SearchOption.AllDirectories).Any();

    private static int ExtractFromArchive(ZipArchive archive, string cache, string fullCachePrefix, ref bool hasBinary)
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
            if (norm.EndsWith("/libil2cpp.so", StringComparison.OrdinalIgnoreCase))
                hasBinary = true;
            count++;
        }
        return count;
    }

    public static VirtualArchiveSession? OpenZeroCopy(
        string baseApkPath,
        IReadOnlyList<string>? splitApkPaths,
        string? nativeLibraryDir,
        IDiagnosticSink sink)
    {
        if (!File.Exists(baseApkPath))
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{baseApkPath}' does not exist.", new() { File = baseApkPath });
            return null;
        }

        var splits = (splitApkPaths ?? [])
            .Where(p => !string.IsNullOrWhiteSpace(p) && !string.Equals(p, baseApkPath, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToArray();

        var owners = new List<IDisposable>();
        VirtualFileBuffer? metadata = null;
        string? metadataVirtual = null;
        VirtualFileBuffer? binary = null;
        string? binaryVirtual = null;
        var binaryScore = -1;
        var dataFiles = new Dictionary<string, VirtualFileBuffer>(StringComparer.OrdinalIgnoreCase);

        void CollectFromReader(ZipZeroCopyReader reader, string containerPath)
        {
            foreach (var entry in reader.Entries)
            {
                var norm = entry.FullName.Replace('\\', '/').TrimStart('/');
                if (norm.Length == 0 || norm.EndsWith('/')) continue;
                if (!IsWantedEntry(norm)) continue;

                var vPath = $"{containerPath}::{norm}";
                var fileName = Path.GetFileName(norm);
                if (fileName.Equals("global-metadata.dat", StringComparison.OrdinalIgnoreCase))
                {
                    if (metadata is null)
                    {
                        metadata = reader.OpenBuffer(entry);
                        metadataVirtual = vPath;
                    }
                    continue;
                }

                if (norm.StartsWith("lib/", StringComparison.OrdinalIgnoreCase)
                    && fileName.Equals("libil2cpp.so", StringComparison.OrdinalIgnoreCase))
                {
                    var score = AbiPriority(norm);
                    if (score > binaryScore)
                    {
                        binary?.Dispose();
                        binary = reader.OpenBuffer(entry);
                        binaryVirtual = vPath;
                        binaryScore = score;
                    }
                    continue;
                }

                if (!dataFiles.ContainsKey(fileName))
                    dataFiles[fileName] = reader.OpenBuffer(entry);
            }
        }

        void CollectFromCompressedNestedApk(byte[] nestedBytes, string containerPath)
        {
            using var ms = new MemoryStream(nestedBytes, writable: false);
            using var zip = new ZipArchive(ms, ZipArchiveMode.Read);
            foreach (var entry in zip.Entries)
            {
                var norm = entry.FullName.Replace('\\', '/').TrimStart('/');
                if (norm.Length == 0 || norm.EndsWith('/')) continue;
                if (!IsWantedEntry(norm)) continue;

                var vPath = $"{containerPath}::{norm}";
                var fileName = Path.GetFileName(norm);
                if (fileName.Equals("global-metadata.dat", StringComparison.OrdinalIgnoreCase))
                {
                    if (metadata is null)
                    {
                        metadata = VirtualFileBuffer.FromArray(vPath, ReadAll(entry));
                        metadataVirtual = vPath;
                    }
                    continue;
                }

                if (norm.StartsWith("lib/", StringComparison.OrdinalIgnoreCase)
                    && fileName.Equals("libil2cpp.so", StringComparison.OrdinalIgnoreCase))
                {
                    var score = AbiPriority(norm);
                    if (score > binaryScore)
                    {
                        binary?.Dispose();
                        binary = VirtualFileBuffer.FromArray(vPath, ReadAll(entry));
                        binaryVirtual = vPath;
                        binaryScore = score;
                    }
                    continue;
                }

                if (!dataFiles.ContainsKey(fileName))
                    dataFiles[fileName] = VirtualFileBuffer.FromArray(vPath, ReadAll(entry));
            }
        }

        try
        {
            var outer = ZipZeroCopyReader.Open(baseApkPath);
            owners.Add(outer);
            CollectFromReader(outer, baseApkPath);

            if (IsSplitContainerPath(baseApkPath))
            {
                foreach (var entry in outer.Entries)
                {
                    if (!entry.FullName.EndsWith(".apk", StringComparison.OrdinalIgnoreCase) || entry.UncompressedSize == 0)
                        continue;

                    if (outer.TryOpenNestedStored(entry) is { } nested)
                    {
                        owners.Add(nested);
                        CollectFromReader(nested, $"{baseApkPath}::{entry.FullName}");
                    }
                    else
                    {
                        try
                        {
                            using var buf = outer.OpenBuffer(entry);
                            CollectFromCompressedNestedApk(buf.ToByteArray(), $"{baseApkPath}::{entry.FullName}");
                        }
                        catch { /* skip corrupt/unsupported nested split apk */ }
                    }
                }
            }

            foreach (var split in splits)
            {
                if (!File.Exists(split)) continue;
                try
                {
                    var r = ZipZeroCopyReader.Open(split);
                    owners.Add(r);
                    CollectFromReader(r, split);
                }
                catch { /* skip unreadable split apk */ }
            }

            string? binaryOnDisk = null;
            if (binary is null && !string.IsNullOrWhiteSpace(nativeLibraryDir))
            {
                var candidate = Path.Combine(nativeLibraryDir, "libil2cpp.so");
                if (File.Exists(candidate))
                    binaryOnDisk = candidate;
            }

            var session = new VirtualArchiveSession
            {
                PrimaryArchivePath = baseApkPath,
                Metadata = metadata,
                MetadataVirtualPath = metadataVirtual,
                Binary = binary,
                BinaryVirtualPath = binaryVirtual ?? binaryOnDisk,
                BinaryFilePathOnDisk = binaryOnDisk,
                DataFiles = dataFiles,
            };
            foreach (var o in owners) session.AddOwner(o);

            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
                $"APK: mapped '{Path.GetFileName(baseApkPath)}' in-memory (zero-copy metadata={metadata?.IsZeroCopy == true}, zero-copy binary={binary?.IsZeroCopy == true}).",
                new() { File = baseApkPath });
            return session;
        }
        catch (Exception ex)
        {
            metadata?.Dispose();
            binary?.Dispose();
            foreach (var d in dataFiles.Values) d.Dispose();
            foreach (var o in owners) o.Dispose();
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to open '{Path.GetFileName(baseApkPath)}': {ex.Message}", new() { File = baseApkPath }, ex);
            return null;
        }
    }

    private static int AbiPriority(string norm)
    {
        if (norm.Contains("/arm64-v8a/", StringComparison.OrdinalIgnoreCase)) return 4;
        if (norm.Contains("/armeabi-v7a/", StringComparison.OrdinalIgnoreCase)) return 3;
        if (norm.Contains("/x86_64/", StringComparison.OrdinalIgnoreCase)) return 2;
        if (norm.Contains("/x86/", StringComparison.OrdinalIgnoreCase)) return 1;
        return 0;
    }

    private static byte[] ReadAll(ZipArchiveEntry entry)
    {
        using var s = entry.Open();
        using var ms = new MemoryStream((int)Math.Clamp(entry.Length, 0, int.MaxValue));
        s.CopyTo(ms);
        return ms.ToArray();
    }

    private static bool IsWantedEntry(string norm)
    {
        if (norm.StartsWith("lib/", StringComparison.OrdinalIgnoreCase) &&
            norm.EndsWith("/libil2cpp.so", StringComparison.OrdinalIgnoreCase))
            return true;

        if (norm.StartsWith("assets/bin/Data/Managed/Resources/", StringComparison.OrdinalIgnoreCase) &&
            norm.EndsWith("-resources.dat", StringComparison.OrdinalIgnoreCase))
            return true;

        var fileName = Path.GetFileName(norm);
        if (!WantedExactNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
            return false;

        // Avoid pulling unrelated files from deep asset packs unless under assets/bin/Data/
        return norm.StartsWith("assets/bin/Data/", StringComparison.OrdinalIgnoreCase)
            || !norm.Contains('/');
    }
}
