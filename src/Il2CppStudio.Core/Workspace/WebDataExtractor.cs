using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Unpacks Unity WebGL data containers across all Unity generations:
/// <list type="bullet">
/// <item><c>Build/*.data</c>, <c>*.data.br</c>, <c>*.data.gz</c>, <c>*.data.unityweb</c> (<c>UnityWebData1.0\0</c>)</item>
/// <item>Unity 5.3–5.5 <c>Release/*.datagz</c> / <c>Compressed/*.datagz</c> / <c>Development/*.data</c> (Emscripten
/// <c>file_packager.py</c> concatenated data blobs with manifest in sibling <c>UnityLoader.js</c>, or raw fallback)</item>
/// </list>
/// Files are extracted to a per-build cache directory under the user's temp folder (the game folder is never modified).
/// </summary>
public static partial class WebDataExtractor
{
    private static readonly byte[] Magic = "UnityWebData1.0\0"u8.ToArray();

    /// <summary>Standard Unity WebGL output subdirectories across Unity 5.x (Release/Compressed/Development) and 5.6+ (Build).</summary>
    public static readonly string[] SearchSubdirs = ["Build", "Release", "Compressed", "Development"];

    public static bool IsWebGlSubdir(string dirName) =>
        SearchSubdirs.Contains(dirName, StringComparer.OrdinalIgnoreCase);

    public static bool IsDataFilePath(string path) =>
        path.EndsWith(".data", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".data.br", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".data.gz", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".data.unityweb", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".datagz", StringComparison.OrdinalIgnoreCase);

    public static bool IsLegacyAsmJsPath(string path) =>
        path.EndsWith(".jsgz", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".js.memgz", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".memgz", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".asm.code.unityweb", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".asm.memory.unityweb", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".asm.js", StringComparison.OrdinalIgnoreCase)
        || path.EndsWith(".js.mem", StringComparison.OrdinalIgnoreCase);

    /// <summary>Finds the WebGL data file of a build folder (Build/, Release/, Compressed/, Development/, or root), or null.</summary>
    public static string? FindDataFile(string root) =>
        SearchSubdirs.Select(s => Path.Combine(root, s)).Append(root)
            .Where(Directory.Exists)
            .SelectMany(Directory.EnumerateFiles)
            .FirstOrDefault(IsDataFilePath);

    /// <summary>Finds a legacy pre-Wasm WebGL asm.js code or memory-init artifact under the build folder, or null.</summary>
    public static string? FindLegacyAsmJs(string root)
    {
        var files = SearchSubdirs.Select(s => Path.Combine(root, s)).Append(root)
            .Where(Directory.Exists)
            .SelectMany(Directory.EnumerateFiles)
            .ToList();

        static bool IsHelperScript(string file)
        {
            var name = Path.GetFileName(file);
            return name.StartsWith("UnityLoader", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("UnityProgress", StringComparison.OrdinalIgnoreCase)
                || name.Contains(".framework", StringComparison.OrdinalIgnoreCase);
        }

        var primary = files.FirstOrDefault(f => !IsHelperScript(f) && (
            f.EndsWith(".jsgz", StringComparison.OrdinalIgnoreCase)
            || f.EndsWith(".asm.code.unityweb", StringComparison.OrdinalIgnoreCase)
            || f.EndsWith(".asm.js", StringComparison.OrdinalIgnoreCase)));
        if (primary is not null) return primary;

        var memInit = files.FirstOrDefault(f =>
            f.EndsWith(".js.memgz", StringComparison.OrdinalIgnoreCase)
            || f.EndsWith(".memgz", StringComparison.OrdinalIgnoreCase)
            || f.EndsWith(".js.mem", StringComparison.OrdinalIgnoreCase)
            || f.EndsWith(".mem", StringComparison.OrdinalIgnoreCase)
            || f.EndsWith(".asm.memory.unityweb", StringComparison.OrdinalIgnoreCase));
        if (memInit is null) return null;

        var siblingJs = files.FirstOrDefault(f => !IsHelperScript(f) && f.EndsWith(".js", StringComparison.OrdinalIgnoreCase));
        return siblingJs ?? memInit;
    }

    /// <summary>Extracts the container and returns the cache directory, or null when the format is unrecognized.</summary>
    public static string? Extract(string dataFile, IDiagnosticSink sink)
    {
        var info = new FileInfo(dataFile);
        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "webdata-v2", $"{Path.GetFileName(dataFile)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}");
        if (File.Exists(Path.Combine(cache, ".complete"))) return cache;

        byte[] bytes;
        try { bytes = Decompress(File.ReadAllBytes(dataFile), dataFile); }
        catch (Exception ex)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound, $"WebGL data could not be decompressed: {ex.Message}", new() { File = dataFile });
            return null;
        }

        if (TryExtractUnityWebData(bytes, cache, dataFile, sink))
            return cache;

        if (TryExtractEmscriptenManifest(bytes, cache, dataFile, sink))
            return cache;

        if (TryExtractRawMetadataFallback(bytes, cache, dataFile, sink))
            return cache;

        sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound,
            "File is neither a UnityWebData1.0 container nor a recognized legacy WebGL data package.", new() { File = dataFile });
        return null;
    }

    private static bool TryExtractUnityWebData(byte[] bytes, string cache, string dataFile, IDiagnosticSink sink)
    {
        if (bytes.Length < 20 || !bytes.AsSpan(0, Magic.Length).SequenceEqual(Magic))
            return false;

        var headerSize = BitConverter.ToInt32(bytes, Magic.Length);
        var pos = Magic.Length + 4;
        var count = 0;
        while (pos + 12 <= headerSize && pos + 12 <= bytes.Length)
        {
            var offset = BitConverter.ToInt32(bytes, pos);
            var size = BitConverter.ToInt32(bytes, pos + 4);
            var nameLen = BitConverter.ToInt32(bytes, pos + 8);
            if (nameLen < 0 || pos + 12 + nameLen > bytes.Length) break;
            var name = Encoding.UTF8.GetString(bytes, pos + 12, nameLen);
            pos += 12 + nameLen;
            if ((uint)offset > (uint)bytes.Length || (uint)size > (uint)(bytes.Length - offset)) break;
            if (WriteExtractedEntry(cache, name, bytes.AsSpan(offset, size)))
                count++;
        }

        if (count == 0) return false;
        FinalizeCache(cache, dataFile, count, "UnityWebData1.0", sink);
        return true;
    }

    private static bool TryExtractEmscriptenManifest(byte[] bytes, string cache, string dataFile, IDiagnosticSink sink)
    {
        var dir = Path.GetDirectoryName(dataFile);
        if (dir is null || !Directory.Exists(dir)) return false;

        var candidates = Directory.EnumerateFiles(dir, "*.js")
            .Where(f => !Path.GetFileName(f).Equals("UnityProgress.js", StringComparison.OrdinalIgnoreCase)
                     && !Path.GetFileName(f).Contains(".framework", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(f => Path.GetFileName(f).Equals("UnityLoader.js", StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var jsFile in candidates)
        {
            var fi = new FileInfo(jsFile);
            if (fi.Length > 16 * 1024 * 1024) continue;

            string text;
            try { text = File.ReadAllText(jsFile); }
            catch { continue; }

            var entries = new Dictionary<string, (int Start, int End)>(StringComparer.Ordinal);
            foreach (Match m in DataRequestRegex().Matches(text))
            {
                if (int.TryParse(m.Groups[1].Value, out var start)
                    && int.TryParse(m.Groups[2].Value, out var end))
                {
                    entries.TryAdd(m.Groups[3].Value, (start, end));
                }
            }

            if (entries.Count == 0 && text.Contains("global-metadata.dat", StringComparison.Ordinal))
            {
                foreach (Match block in LoadPackageEntryRegex().Matches(text))
                {
                    var fnMatch = JsonFilenameRegex().Match(block.Value);
                    var startMatch = JsonStartRegex().Match(block.Value);
                    var endMatch = JsonEndRegex().Match(block.Value);
                    if (fnMatch.Success && startMatch.Success && endMatch.Success
                        && int.TryParse(startMatch.Groups[1].Value, out var start)
                        && int.TryParse(endMatch.Groups[1].Value, out var end))
                    {
                        entries.TryAdd(fnMatch.Groups[1].Value, (start, end));
                    }
                }
            }

            if (entries.Count == 0) continue;

            var count = 0;
            foreach (var (name, (start, end)) in entries)
            {
                if (start < 0 || end < start || end > bytes.Length) continue;
                if (WriteExtractedEntry(cache, name, bytes.AsSpan(start, end - start)))
                    count++;
            }

            if (count > 0 && File.Exists(Directory.EnumerateFiles(cache, "global-metadata.dat", SearchOption.AllDirectories).FirstOrDefault() ?? ""))
            {
                FinalizeCache(cache, dataFile, count, $"Emscripten manifest ({Path.GetFileName(jsFile)})", sink);
                return true;
            }
        }
        return false;
    }

    private static bool TryExtractRawMetadataFallback(byte[] bytes, string cache, string dataFile, IDiagnosticSink sink)
    {
        var span = bytes.AsSpan();
        ReadOnlySpan<byte> metaMagic = [0xAF, 0x1B, 0xB1, 0xFA];

        var searchFrom = 0;
        while (searchFrom <= span.Length - 8)
        {
            var rel = span[searchFrom..].IndexOf(metaMagic);
            if (rel < 0) break;
            var offset = searchFrom + rel;
            searchFrom = offset + 4;

            var slice = span[offset..];
            if (TryResolveRawVersion(slice) is not { } version) continue;

            MetadataHeader hdr;
            try { hdr = MetadataHeader.Read(slice, version); }
            catch { continue; }

            var validSections = 0;
            long maxEnd = hdr.HeaderSize;
            var invalid = false;
            foreach (var s in hdr.Sections)
            {
                if (s.Offset < 0 || s.Size < 0 || s.End > slice.Length)
                {
                    invalid = true;
                    break;
                }
                if (s.Size > 0)
                {
                    if (s.Offset < hdr.HeaderSize)
                    {
                        invalid = true;
                        break;
                    }
                    validSections++;
                    if (s.End > maxEnd) maxEnd = s.End;
                }
            }
            if (invalid || validSections < 4 || maxEnd > slice.Length) continue;

            var count = 0;
            if (WriteExtractedEntry(cache, "Il2CppData/Metadata/global-metadata.dat", slice[..(int)maxEnd]))
                count++;

            if (offset > 0 && (span.StartsWith("UnityFS\0"u8) || span.StartsWith("UnityWeb\0"u8) || span.StartsWith("UnityRaw\0"u8)))
            {
                if (WriteExtractedEntry(cache, "data.unity3d", span[..offset]))
                    count++;
            }

            if (count > 0)
            {
                FinalizeCache(cache, dataFile, count, "raw WebGL blob fallback", sink);
                return true;
            }
        }

        return false;
    }

    private static MetadataVersion? TryResolveRawVersion(ReadOnlySpan<byte> blob)
    {
        if (blob.Length < 8) return null;
        var raw = BinaryPrimitives.ReadInt32LittleEndian(blob[4..]);
        MetadataVersion version = raw switch
        {
            16 or 19 or 20 or 21 or 22 or 23 or 27 or 29 or 31 => new(raw),
            24 => blob.Length >= 12 && BinaryPrimitives.ReadInt32LittleEndian(blob[8..]) == MetadataHeader.SizeFor(new(24, 2))
                ? new(24, 2)
                : new(24, 0),
            _ => default,
        };
        if (version.Major == 0 || blob.Length < MetadataHeader.SizeFor(version)) return null;
        return version;
    }

    private static bool WriteExtractedEntry(string cache, string rawName, ReadOnlySpan<byte> payload)
    {
        var relative = rawName.TrimStart('/', '\\');
        if (string.IsNullOrWhiteSpace(relative)) return false;

        var fullCache = Path.GetFullPath(cache);
        if (!fullCache.EndsWith(Path.DirectorySeparatorChar))
            fullCache += Path.DirectorySeparatorChar;

        var target = Path.GetFullPath(Path.Combine(fullCache, relative));
        if (!target.StartsWith(fullCache, StringComparison.OrdinalIgnoreCase)) return false;   // no path traversal

        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        File.WriteAllBytes(target, payload);
        return true;
    }

    private static void FinalizeCache(string cache, string dataFile, int count, string kind, IDiagnosticSink sink)
    {
        File.WriteAllText(Path.Combine(cache, ".complete"), dataFile);
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.WorkspaceLoadStarted,
            $"WebGL: extracted {count} files ({kind}) from '{Path.GetFileName(dataFile)}' to '{cache}'.", new() { File = dataFile });
    }

    /// <summary>
    /// Gzip (<c>1F 8B</c>), raw uncompressed <c>*.data</c> / <c>UnityWebData1.0\0</c>, or Brotli decompression.
    /// </summary>
    private static byte[] Decompress(byte[] data, string dataFile)
    {
        if (data.AsSpan().StartsWith(Magic)) return data;
        if (data.Length > 2 && data[0] == 0x1F && data[1] == 0x8B)
        {
            using var gzInput = new MemoryStream(data);
            using var gz = new GZipStream(gzInput, CompressionMode.Decompress);
            using var gzOutput = new MemoryStream();
            gz.CopyTo(gzOutput);
            return gzOutput.ToArray();
        }
        if (dataFile.EndsWith(".data", StringComparison.OrdinalIgnoreCase))
            return data;

        using var input = new MemoryStream(data);
        using var dec = new BrotliStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        dec.CopyTo(output);
        return output.ToArray();
    }

    [GeneratedRegex(@"new\s+[A-Za-z_$][\w$]*\(\s*(\d+)\s*,\s*(\d+)(?:\s*,\s*\d+)*\s*\)\s*\.open\(\s*['""]GET['""]\s*,\s*['""]([^'""]+)['""]\s*\)", RegexOptions.CultureInvariant)]
    private static partial Regex DataRequestRegex();

    [GeneratedRegex(@"\{[^{}]*\}", RegexOptions.CultureInvariant)]
    private static partial Regex LoadPackageEntryRegex();

    [GeneratedRegex(@"[""']filename[""']\s*:\s*[""']([^""']+)[""']", RegexOptions.CultureInvariant)]
    private static partial Regex JsonFilenameRegex();

    [GeneratedRegex(@"[""']start[""']\s*:\s*(\d+)", RegexOptions.CultureInvariant)]
    private static partial Regex JsonStartRegex();

    [GeneratedRegex(@"[""']end[""']\s*:\s*(\d+)", RegexOptions.CultureInvariant)]
    private static partial Regex JsonEndRegex();
}
