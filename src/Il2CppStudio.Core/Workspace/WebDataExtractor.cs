using System.IO.Compression;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Unpacks a Unity WebGL data container (<c>Build/*.data</c>, <c>*.data.br</c>, <c>*.data.gz</c>, or the legacy
/// <c>*.unityweb</c>). Format "UnityWebData1.0\0": uint32 header size, then entries (uint32 offset, uint32 size,
/// uint32 name length, UTF-8 name) up to the header size. Files are extracted to a per-build cache directory
/// under the user's temp folder (the game folder is never modified).
/// </summary>
public static class WebDataExtractor
{
    private static readonly byte[] Magic = "UnityWebData1.0\0"u8.ToArray();

    /// <summary>Finds the WebGL data file of a build folder (root or Build/), or null.</summary>
    public static string? FindDataFile(string root)
    {
        var dirs = new[] { Path.Combine(root, "Build"), root }.Where(Directory.Exists);
        return dirs.SelectMany(d => Directory.EnumerateFiles(d))
            .FirstOrDefault(f => f.EndsWith(".data", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".data.br", StringComparison.OrdinalIgnoreCase)
                              || f.EndsWith(".data.gz", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".data.unityweb", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Extracts the container and returns the cache directory, or null when it is not a UnityWebData file.</summary>
    public static string? Extract(string dataFile, IDiagnosticSink sink)
    {
        var info = new FileInfo(dataFile);
        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "webdata", $"{Path.GetFileName(dataFile)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}");
        if (File.Exists(Path.Combine(cache, ".complete"))) return cache;

        byte[] bytes;
        try { bytes = Decompress(File.ReadAllBytes(dataFile)); }
        catch (Exception ex)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound, $"WebGL data could not be decompressed: {ex.Message}", new() { File = dataFile });
            return null;
        }
        if (bytes.Length < 20 || !bytes.AsSpan(0, Magic.Length).SequenceEqual(Magic))
        {
            sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound, "File is not a UnityWebData1.0 container.", new() { File = dataFile });
            return null;
        }

        var headerSize = BitConverter.ToInt32(bytes, Magic.Length);
        var pos = Magic.Length + 4;
        var count = 0;
        while (pos + 12 <= headerSize)
        {
            var offset = BitConverter.ToInt32(bytes, pos);
            var size = BitConverter.ToInt32(bytes, pos + 4);
            var nameLen = BitConverter.ToInt32(bytes, pos + 8);
            var name = Encoding.UTF8.GetString(bytes, pos + 12, nameLen);
            pos += 12 + nameLen;
            if ((uint)offset > bytes.Length || (uint)size > bytes.Length - offset) break;
            var target = Path.GetFullPath(Path.Combine(cache, name));
            if (!target.StartsWith(Path.GetFullPath(cache), StringComparison.OrdinalIgnoreCase)) continue;   // no path traversal
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.WriteAllBytes(target, bytes.AsSpan(offset, size).ToArray());
            count++;
        }
        File.WriteAllText(Path.Combine(cache, ".complete"), dataFile);
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.WorkspaceLoadStarted, $"WebGL: extracted {count} files from '{Path.GetFileName(dataFile)}' to '{cache}'.", new() { File = dataFile });
        return cache;
    }

    /// <summary>Gzip (1F 8B) or Brotli (no magic: tried when the bytes are not already a container) decompression.</summary>
    private static byte[] Decompress(byte[] data)
    {
        if (data.AsSpan().StartsWith(Magic)) return data;
        using var input = new MemoryStream(data);
        using Stream dec = data.Length > 2 && data[0] == 0x1F && data[1] == 0x8B
            ? new GZipStream(input, CompressionMode.Decompress)
            : new BrotliStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        dec.CopyTo(output);
        return output.ToArray();
    }
}
