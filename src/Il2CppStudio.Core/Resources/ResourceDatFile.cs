using System.Buffers.Binary;
using System.Resources;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Resources;

public enum ManagedResourceKind
{
    /// <summary>A System.Resources .resources container (magic 0xBEEFCACE).</summary>
    ResourcesContainer,
    Xml,
    Text,
    Binary,
}

public sealed record ResourceEntry(string Name, int DataOffset, int Size, ManagedResourceKind Kind);

/// <summary>
/// Reader for il2cpp_data/Resources/&lt;Assembly&gt;.dll-resources.dat, the file IL2CPP writes for each
/// assembly's embedded manifest resources. Layout (libil2cpp RuntimeAssembly.cpp, GetResourceRecords):
///   int32 headerSize            -- bytes of the record table that follows
///   int32 count
///   count x { int32 dataSize, int32 nameSize, byte[nameSize] name }
///   resource data, back to back, starting at 4 + headerSize
/// </summary>
public sealed class ResourceDatFile
{
    public string Path { get; }
    public string AssemblyName { get; }
    public IReadOnlyList<ResourceEntry> Entries { get; }
    private readonly byte[] _data;

    private ResourceDatFile(string path, byte[] data, List<ResourceEntry> entries)
    {
        Path = path;
        _data = data;
        Entries = entries;
        var file = System.IO.Path.GetFileName(path);
        AssemblyName = file.EndsWith("-resources.dat", StringComparison.OrdinalIgnoreCase) ? file[..^"-resources.dat".Length] : file;
    }

    public static ResourceDatFile? Load(string path, IDiagnosticSink sink)
    {
        byte[] data;
        try { data = File.ReadAllBytes(path); }
        catch (Exception ex)
        {
            sink.Error(DiagnosticCategory.FileSystem, DiagnosticCodes.FileAccessFailed, $"Cannot read resource file: {ex.Message}", new() { File = path }, ex);
            return null;
        }

        var loc = new DiagnosticLocation { File = path };
        if (data.Length < 8)
        {
            sink.Error(DiagnosticCategory.Resources, DiagnosticCodes.ResourceFileCorrupt, $"File is only {data.Length} bytes.", loc);
            return null;
        }

        var span = data.AsSpan();
        var headerSize = BinaryPrimitives.ReadInt32LittleEndian(span);
        var count = BinaryPrimitives.ReadInt32LittleEndian(span[4..]);
        var dataStart = 4L + headerSize;
        if (headerSize < 4 || dataStart > data.Length || count < 0 || count > 100_000)
        {
            sink.Error(DiagnosticCategory.Resources, DiagnosticCodes.ResourceFileCorrupt,
                $"Implausible header (headerSize {headerSize}, count {count}, file {data.Length} bytes).", loc);
            return null;
        }

        var entries = new List<ResourceEntry>(count);
        var pos = 8;
        var dataOffset = (int)dataStart;
        for (var i = 0; i < count; i++)
        {
            if (pos + 8 > dataStart)
            {
                sink.Error(DiagnosticCategory.Resources, DiagnosticCodes.ResourceFileCorrupt, $"Record {i} runs past the header.", loc with { FileOffset = pos });
                break;
            }
            var size = BinaryPrimitives.ReadInt32LittleEndian(span[pos..]);
            var nameLen = BinaryPrimitives.ReadInt32LittleEndian(span[(pos + 4)..]);
            pos += 8;
            if (nameLen < 0 || pos + nameLen > dataStart)
            {
                sink.Error(DiagnosticCategory.Resources, DiagnosticCodes.ResourceFileCorrupt, $"Record {i} name length {nameLen} is invalid.", loc with { FileOffset = pos });
                break;
            }
            var name = Encoding.UTF8.GetString(span.Slice(pos, nameLen));
            pos += nameLen;

            if (size < 0 || (long)dataOffset + size > data.Length)
            {
                sink.Warn(DiagnosticCategory.Resources, DiagnosticCodes.ResourceEntryOutOfBounds,
                    $"Resource '{name}' ({size} bytes at 0x{dataOffset:X}) exceeds the file; skipped.", loc with { FileOffset = dataOffset });
                break;
            }
            entries.Add(new ResourceEntry(name, dataOffset, size, Classify(span.Slice(dataOffset, size))));
            dataOffset += size;
        }

        if (dataOffset != data.Length && entries.Count == count)
            sink.Warn(DiagnosticCategory.Resources, DiagnosticCodes.ResourceFileCorrupt,
                $"{data.Length - dataOffset} trailing byte(s) after the last resource.", loc);

        sink.Info(DiagnosticCategory.Resources, DiagnosticCodes.ResourceFileParsed,
            $"{System.IO.Path.GetFileName(path)}: {entries.Count} resource(s), {data.Length:N0} bytes.", loc);
        return new ResourceDatFile(path, data, entries);
    }

    public ReadOnlySpan<byte> GetData(ResourceEntry entry) => _data.AsSpan(entry.DataOffset, entry.Size);

    private static ManagedResourceKind Classify(ReadOnlySpan<byte> d)
    {
        if (d.Length >= 4 && BinaryPrimitives.ReadUInt32LittleEndian(d) == 0xBEEFCACE) return ManagedResourceKind.ResourcesContainer;
        var probe = d[..Math.Min(d.Length, 512)];
        if (probe.Length >= 3 && probe[0] == 0xEF && probe[1] == 0xBB && probe[2] == 0xBF) probe = probe[3..];
        var trimmed = probe.TrimStart(" \t\r\n"u8);
        if (trimmed.Length > 0 && trimmed[0] == (byte)'<') return ManagedResourceKind.Xml;
        foreach (var b in probe)
            if (b < 0x09 || (b > 0x0D && b < 0x20)) return ManagedResourceKind.Binary;
        return ManagedResourceKind.Text;
    }

    /// <summary>
    /// Decodes a .resources container into (key, value-description) pairs. Strings are returned verbatim;
    /// other types are reported by type name and size so nothing is silently skipped.
    /// </summary>
    public static IReadOnlyList<KeyValuePair<string, string>> DecodeResourcesContainer(ReadOnlySpan<byte> data, string sourceName, IDiagnosticSink sink)
    {
        var result = new List<KeyValuePair<string, string>>();
        try
        {
            using var reader = new ResourceReader(new MemoryStream(data.ToArray(), writable: false));
            var e = reader.GetEnumerator();
            while (e.MoveNext())
            {
                var key = e.Key?.ToString() ?? "<null>";
                reader.GetResourceData(key, out var typeName, out var raw);
                string value;
                try { value = e.Value is string s ? s : $"<{typeName}, {raw.Length} bytes>"; }
                catch { value = $"<{typeName}, {raw.Length} bytes, not deserialized>"; }
                result.Add(new(key, value));
            }
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.Resources, DiagnosticCodes.ManagedResourcesDecodeFailed,
                $"Could not decode .resources container '{sourceName}': {ex.Message}", new() { File = sourceName }, ex);
        }
        return result;
    }
}
