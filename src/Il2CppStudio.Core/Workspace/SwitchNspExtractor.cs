using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts IL2CPP binaries (<c>main</c> NSO0), <c>global-metadata.dat</c>, and Unity data headers from an unencrypted
/// Nintendo Switch <c>.nsp</c> (<c>PFS0</c>/<c>HFS0</c>) container, including nested unencrypted PartitionFS
/// (<c>exefs</c>) and Switch <c>RomFS</c> images, into a cached temp folder without embedded proprietary keys.
/// </summary>
public static class SwitchNspExtractor
{
    private const uint Pfs0Magic = 0x30534650; // "PFS0"
    private const uint Hfs0Magic = 0x30534648; // "HFS0"
    private const uint Nso0Magic = 0x304F534E; // "NSO0"
    private const uint RomfsNone = 0xFFFFFFFF;
    private const ulong RomfsHeaderSize = 0x50;

    private static readonly HashSet<string> WantedNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "global-metadata.dat",
        "globalgamemanagers",
        "data.unity3d",
        "boot.config",
        "ScriptingAssemblies.json",
        "app.info",
    };

    public static bool IsNspPath(string path) =>
        Path.GetExtension(path).Equals(".nsp", StringComparison.OrdinalIgnoreCase);

    public static string? Extract(string nspPath, IDiagnosticSink sink)
    {
        var info = new FileInfo(nspPath);
        if (!info.Exists)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{nspPath}' does not exist.", new() { File = nspPath });
            return null;
        }

        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "nsp",
            $"{Path.GetFileName(nspPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}");
        if (File.Exists(Path.Combine(cache, ".complete")) && HasMetadata(cache))
            return cache;

        try
        {
            using var fs = new FileStream(nspPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length < 0x20)
                throw new InvalidDataException("File is too small to be a Switch .nsp container.");

            Span<byte> hdr = stackalloc byte[4];
            fs.ReadExactly(hdr);
            var magic = BinaryPrimitives.ReadUInt32LittleEndian(hdr);
            if (magic is not (Pfs0Magic or Hfs0Magic))
                throw new InvalidDataException("Missing Switch PFS0/HFS0 header magic in .nsp.");

            Directory.CreateDirectory(cache);
            var sawNca = false;
            var count = ExtractPartitionFs(fs, 0, fs.Length, cache, prefix: string.Empty, ref sawNca, depth: 0);
            if (count > 0 && HasMetadata(cache))
            {
                File.WriteAllText(Path.Combine(cache, ".complete"), nspPath);
                sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
                    $"NSP: extracted {count} IL2CPP files from '{Path.GetFileName(nspPath)}' to '{cache}'.",
                    new() { File = nspPath });
                return cache;
            }

            if (sawNca)
            {
                sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                    $"Switch package '{Path.GetFileName(nspPath)}' contains encrypted Nintendo .nca partitions without unencrypted metadata. " +
                    "Open the unpacked Switch title directory containing '0/main' (or 'exefs/main') and '1/Data/Managed/Metadata/global-metadata.dat'.",
                    new() { File = nspPath });
            }
            else
            {
                sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound,
                    $"No global-metadata.dat found inside Switch package '{Path.GetFileName(nspPath)}'.",
                    new() { File = nspPath });
            }
            return null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidDataException)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract Switch package '{Path.GetFileName(nspPath)}': {ex.Message}", new() { File = nspPath }, ex);
            return null;
        }
    }

    private static int ExtractPartitionFs(Stream stream, long baseOffset, long containerSize, string cache, string prefix, ref bool sawNca, int depth)
    {
        if (depth > 3 || containerSize < 16 || baseOffset + 16 > stream.Length)
            return 0;

        stream.Position = baseOffset;
        Span<byte> hdr = stackalloc byte[16];
        stream.ReadExactly(hdr);
        var magic = BinaryPrimitives.ReadUInt32LittleEndian(hdr);
        if (magic is not (Pfs0Magic or Hfs0Magic))
            return 0;

        var fileCount = BinaryPrimitives.ReadUInt32LittleEndian(hdr.Slice(4, 4));
        var strTableSize = BinaryPrimitives.ReadUInt32LittleEndian(hdr.Slice(8, 4));
        var entrySize = magic == Pfs0Magic ? 24 : 64;
        if (fileCount == 0 || fileCount > 100_000 || strTableSize > 16 * 1024 * 1024)
            return 0;

        var tableBytes = checked((long)fileCount * entrySize);
        var dataStart = 16L + tableBytes + strTableSize;
        if (dataStart > containerSize || baseOffset + dataStart > stream.Length)
            return 0;

        var entries = new byte[tableBytes];
        stream.ReadExactly(entries);
        var names = new byte[strTableSize];
        stream.ReadExactly(names);

        var fullPrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var extracted = 0;

        for (var i = 0; i < (int)fileCount; i++)
        {
            var e = entries.AsSpan(i * entrySize, entrySize);
            var relOff = BinaryPrimitives.ReadUInt64LittleEndian(e.Slice(0, 8));
            var size = BinaryPrimitives.ReadUInt64LittleEndian(e.Slice(8, 8));
            var nameOff = BinaryPrimitives.ReadUInt32LittleEndian(e.Slice(16, 4));
            if (nameOff >= (uint)names.Length || size == 0) continue;

            var nameSpan = names.AsSpan((int)nameOff);
            var nul = nameSpan.IndexOf((byte)0);
            if (nul >= 0) nameSpan = nameSpan[..nul];
            var name = Encoding.UTF8.GetString(nameSpan).Trim('/', '\\');
            if (name.Length == 0 || name.Contains("..", StringComparison.Ordinal)) continue;

            if (name.EndsWith(".nca", StringComparison.OrdinalIgnoreCase))
            {
                sawNca = true;
                continue;
            }

            if (relOff > (ulong)long.MaxValue || size > (ulong)long.MaxValue) continue;
            var absOff = baseOffset + dataStart + (long)relOff;
            if (absOff < 0 || absOff + (long)size > baseOffset + containerSize || absOff + (long)size > stream.Length)
                continue;

            var relPath = prefix.Length == 0 ? name : $"{prefix}/{name}";

            // Inspect first 80 bytes for nested unencrypted PFS0/HFS0/RomFS containers or NSO0 main binary.
            Span<byte> peek = stackalloc byte[80];
            var peekLen = (int)Math.Min((long)size, 80L);
            stream.Position = absOff;
            stream.ReadExactly(peek[..peekLen]);

            if (peekLen >= 16 && BinaryPrimitives.ReadUInt32LittleEndian(peek) is Pfs0Magic or Hfs0Magic)
            {
                var subPrefix = Path.GetFileNameWithoutExtension(name);
                if (string.IsNullOrEmpty(subPrefix)) subPrefix = name;
                var nestedPrefix = prefix.Length == 0 ? subPrefix : $"{prefix}/{subPrefix}";
                extracted += ExtractPartitionFs(stream, absOff, (long)size, cache, nestedPrefix, ref sawNca, depth + 1);
                continue;
            }

            if (LooksLikeRomFs(peek[..peekLen], (long)size))
            {
                var subPrefix = name.Contains("romfs", StringComparison.OrdinalIgnoreCase) ? "romfs" : Path.GetFileNameWithoutExtension(name);
                extracted += ExtractRomFs(stream, absOff, (long)size, cache, string.IsNullOrEmpty(subPrefix) ? "romfs" : subPrefix);
                continue;
            }

            var isMainNso = name.Equals("main", StringComparison.OrdinalIgnoreCase)
                && peekLen >= 4 && BinaryPrimitives.ReadUInt32LittleEndian(peek) == Nso0Magic;
            if (isMainNso || WantedNames.Contains(Path.GetFileName(name)))
            {
                var target = Path.GetFullPath(Path.Combine(cache, relPath.Replace('/', Path.DirectorySeparatorChar)));
                if (target.StartsWith(fullPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    CopyRangeToFile(stream, absOff, (long)size, target);
                    extracted++;
                }
            }
        }

        return extracted;
    }

    private static bool LooksLikeRomFs(ReadOnlySpan<byte> hdr, long size)
    {
        if (hdr.Length < 0x50 || size < 0x60) return false;
        var hdrSize = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x00, 8));
        var dirHashOff = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x08, 8));
        var dirHashSize = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x10, 8));
        var dirTableOff = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x18, 8));
        var dirTableSize = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x20, 8));
        var fileTableOff = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x38, 8));
        var fileTableSize = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x40, 8));
        var dataOffset = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x48, 8));

        return hdrSize == RomfsHeaderSize
            && dirTableOff == dirHashOff + dirHashSize
            && dataOffset >= hdrSize
            && dirTableOff + dirTableSize <= (ulong)size
            && fileTableOff + fileTableSize <= (ulong)size
            && dataOffset <= (ulong)size;
    }

    private static int ExtractRomFs(Stream stream, long romfsBase, long romfsSize, string cache, string rootPrefix)
    {
        stream.Position = romfsBase;
        Span<byte> hdr = stackalloc byte[0x50];
        stream.ReadExactly(hdr);

        var dirTableOff = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x18, 8));
        var dirTableSize = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x20, 8));
        var fileTableOff = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x38, 8));
        var fileTableSize = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x40, 8));
        var dataOffset = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x48, 8));
        if (dirTableSize < 0x18 || dirTableSize > 32 * 1024 * 1024 || fileTableSize > 64 * 1024 * 1024)
            return 0;

        var dirTable = new byte[dirTableSize];
        stream.Position = romfsBase + (long)dirTableOff;
        stream.ReadExactly(dirTable);

        var fileTable = new byte[fileTableSize];
        if (fileTableSize > 0)
        {
            stream.Position = romfsBase + (long)fileTableOff;
            stream.ReadExactly(fileTable);
        }

        var fullPrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var visitedDirs = new HashSet<uint>();
        var visitedFiles = new HashSet<uint>();
        var extracted = 0;

        void WalkDir(uint dirOff, string relDir)
        {
            while (dirOff != RomfsNone && (ulong)dirOff + 0x18 <= (ulong)dirTable.Length && visitedDirs.Add(dirOff))
            {
                var d = dirTable.AsSpan((int)dirOff);
                var sibling = BinaryPrimitives.ReadUInt32LittleEndian(d.Slice(0x04, 4));
                var child = BinaryPrimitives.ReadUInt32LittleEndian(d.Slice(0x08, 4));
                var firstFile = BinaryPrimitives.ReadUInt32LittleEndian(d.Slice(0x0C, 4));
                var nameLen = BinaryPrimitives.ReadUInt32LittleEndian(d.Slice(0x14, 4));

                var name = dirOff == 0 || nameLen == 0 || (ulong)dirOff + 0x18 + nameLen > (ulong)dirTable.Length
                    ? string.Empty
                    : Encoding.UTF8.GetString(d.Slice(0x18, (int)nameLen));
                var currentPath = string.IsNullOrEmpty(name) ? relDir
                    : string.IsNullOrEmpty(relDir) ? name
                    : $"{relDir}/{name}";

                var fileOff = firstFile;
                while (fileOff != RomfsNone && (ulong)fileOff + 0x20 <= (ulong)fileTable.Length && visitedFiles.Add(fileOff))
                {
                    var f = fileTable.AsSpan((int)fileOff);
                    var fSibling = BinaryPrimitives.ReadUInt32LittleEndian(f.Slice(0x04, 4));
                    var fDataOff = BinaryPrimitives.ReadUInt64LittleEndian(f.Slice(0x08, 8));
                    var fSize = BinaryPrimitives.ReadUInt64LittleEndian(f.Slice(0x10, 8));
                    var fNameLen = BinaryPrimitives.ReadUInt32LittleEndian(f.Slice(0x1C, 4));

                    if (fNameLen > 0 && (ulong)fileOff + 0x20 + fNameLen <= (ulong)fileTable.Length)
                    {
                        var fName = Encoding.UTF8.GetString(f.Slice(0x20, (int)fNameLen));
                        if (WantedNames.Contains(fName)
                            && fDataOff <= (ulong)romfsSize - dataOffset
                            && fSize <= (ulong)romfsSize - dataOffset - fDataOff)
                        {
                            var fileRel = string.IsNullOrEmpty(currentPath) ? fName : $"{currentPath}/{fName}";
                            var fullRel = string.IsNullOrEmpty(rootPrefix) ? fileRel : $"{rootPrefix}/{fileRel}";
                            var target = Path.GetFullPath(Path.Combine(cache, fullRel.Replace('/', Path.DirectorySeparatorChar)));
                            if (target.StartsWith(fullPrefix, StringComparison.OrdinalIgnoreCase))
                            {
                                CopyRangeToFile(stream, romfsBase + (long)dataOffset + (long)fDataOff, (long)fSize, target);
                                extracted++;
                            }
                        }
                    }

                    fileOff = fSibling;
                }

                if (child != RomfsNone)
                    WalkDir(child, currentPath);

                // Root directory (offset 0) should not follow siblings.
                if (dirOff == 0) break;
                dirOff = sibling;
            }
        }

        WalkDir(0, string.Empty);
        return extracted;
    }

    private static void CopyRangeToFile(Stream src, long offset, long size, string targetPath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
        src.Position = offset;
        using var dst = File.Create(targetPath);
        var buffer = new byte[Math.Min(81920, (int)Math.Max(1, size))];
        var remaining = size;
        while (remaining > 0)
        {
            var read = src.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
            if (read <= 0) break;
            dst.Write(buffer, 0, read);
            remaining -= read;
        }
    }

    private static bool HasMetadata(string dir) =>
        Directory.Exists(dir) && Directory.EnumerateFiles(dir, "global-metadata.dat", SearchOption.AllDirectories).Any();
}
