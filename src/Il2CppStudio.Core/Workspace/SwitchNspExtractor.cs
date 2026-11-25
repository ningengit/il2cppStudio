using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts IL2CPP binaries (<c>main</c> NSO0), <c>global-metadata.dat</c>, and Unity data headers from a Nintendo
/// Switch <c>.nsp</c> (<c>PFS0</c>/<c>HFS0</c>) container. Handles both unencrypted PartitionFS/RomFS containers
/// and encrypted <c>.nca</c> (NCA2/NCA3) + <c>.tik</c> containers using user-supplied keys (<c>prod.keys</c> /
/// <c>dev.keys</c>) via <see cref="SwitchKeySet"/>. Contains ZERO embedded proprietary keys.
/// </summary>
public static class SwitchNspExtractor
{
    private const uint Pfs0Magic = 0x30534650; // "PFS0"
    private const uint Hfs0Magic = 0x30534648; // "HFS0"
    private const uint Nca3Magic = 0x3341434E; // "NCA3"
    private const uint Nca2Magic = 0x3241434E; // "NCA2"
    private const uint IvfcMagic = 0x43465649; // "IVFC"
    private const uint Nso0Magic = 0x304F534E; // "NSO0"
    private const uint RomfsNone = 0xFFFFFFFF;
    private const ulong RomfsHeaderSize = 0x50;
    private const int NcaSectorSize = 0x200;

    private static readonly HashSet<string> WantedNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "global-metadata.dat",
        "globalgamemanagers",
        "data.unity3d",
        "boot.config",
        "ScriptingAssemblies.json",
        "app.info",
    };

    private readonly record struct PartitionEntry(string Name, long AbsOffset, long Size);

    public static bool IsNspPath(string path) =>
        Path.GetExtension(path).Equals(".nsp", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns <c>true</c> when the <c>.nsp</c> contains <c>.nca</c> archives and no directly readable unencrypted
    /// IL2CPP files at top level.
    /// </summary>
    public static bool RequiresKeyset(string nspPath)
    {
        if (!File.Exists(nspPath))
            return false;

        try
        {
            using var fs = new FileStream(nspPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var entries = ReadPartitionEntries(fs, 0, fs.Length);
            if (entries.Count == 0)
                return false;

            var hasNca = false;
            var hasPlainMeta = false;
            foreach (var e in entries)
            {
                if (e.Name.EndsWith(".nca", StringComparison.OrdinalIgnoreCase))
                    hasNca = true;
                else if (WantedNames.Contains(Path.GetFileName(e.Name))
                    || e.Name.Contains("romfs", StringComparison.OrdinalIgnoreCase))
                    hasPlainMeta = true;
            }

            return hasNca && !hasPlainMeta;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidDataException)
        {
            return false;
        }
    }

    public static string? Extract(string nspPath, IDiagnosticSink sink) =>
        Extract(nspPath, sink, keysetPath: null);

    public static string? Extract(string nspPath, IDiagnosticSink sink, string? keysetPath)
    {
        var info = new FileInfo(nspPath);
        if (!info.Exists)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{nspPath}' does not exist.", new() { File = nspPath });
            return null;
        }

        var resolvedKeyPath = SwitchKeySet.ResolveDefaultPath(keysetPath);
        if (!string.IsNullOrWhiteSpace(keysetPath) && resolvedKeyPath is null)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Nintendo Switch keyset file not found: '{keysetPath}'.", new() { File = nspPath });
            return null;
        }

        var keySuffix = resolvedKeyPath is null
            ? string.Empty
            : "-" + ComputeKeyCacheTag(resolvedKeyPath);
        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "nsp",
            $"{Path.GetFileName(nspPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}{keySuffix}");
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

            if (count == 0 && sawNca && resolvedKeyPath is not null)
            {
                var keySet = SwitchKeySet.LoadFromFile(resolvedKeyPath);
                var topEntries = ReadPartitionEntries(fs, 0, fs.Length);
                foreach (var tik in topEntries.Where(e => e.Name.EndsWith(".tik", StringComparison.OrdinalIgnoreCase) && e.Size is > 0 and <= 65536))
                {
                    var tikBytes = new byte[tik.Size];
                    fs.Position = tik.AbsOffset;
                    fs.ReadExactly(tikBytes);
                    keySet.ImportTicket(tikBytes);
                }

                if (!keySet.HasHeaderKey)
                {
                    sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                        $"Switch keyset '{resolvedKeyPath}' does not contain 'header_key' needed to decrypt .nca containers.",
                        new() { File = nspPath });
                    return null;
                }

                foreach (var nca in topEntries.Where(e => e.Name.EndsWith(".nca", StringComparison.OrdinalIgnoreCase) && e.Size >= 0xC00))
                    count += ExtractNca(fs, nca, keySet, cache, sink, nspPath);
            }

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
                    resolvedKeyPath is null
                        ? $"Switch package '{Path.GetFileName(nspPath)}' contains encrypted Nintendo .nca partitions. " +
                          "Provide your own prod.keys (--keyset <path>, IL2CPPSTUDIO_SWITCH_KEYS, or ~/.switch/prod.keys) or open the unpacked Switch title directory containing '0/main' and '1/Data/Managed/Metadata/global-metadata.dat'."
                        : $"Could not decrypt .nca IL2CPP partitions in '{Path.GetFileName(nspPath)}' with keyset '{resolvedKeyPath}' (verify master_key/key_area_key and title keys).",
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
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidDataException or CryptographicException)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract Switch package '{Path.GetFileName(nspPath)}': {ex.Message}", new() { File = nspPath }, ex);
            return null;
        }
    }

    private static string ComputeKeyCacheTag(string keyPath)
    {
        var fi = new FileInfo(keyPath);
        var payload = Encoding.UTF8.GetBytes($"{keyPath}|{fi.Length}|{fi.LastWriteTimeUtc.Ticks}");
        return Convert.ToHexString(SHA256.HashData(payload)).AsSpan(0, 8).ToString();
    }

    private static List<PartitionEntry> ReadPartitionEntries(Stream stream, long baseOffset, long containerSize)
    {
        if (containerSize < 16 || baseOffset + 16 > stream.Length)
            return [];

        stream.Position = baseOffset;
        Span<byte> hdr = stackalloc byte[16];
        stream.ReadExactly(hdr);
        var magic = BinaryPrimitives.ReadUInt32LittleEndian(hdr);
        if (magic is not (Pfs0Magic or Hfs0Magic))
            return [];

        var fileCount = BinaryPrimitives.ReadUInt32LittleEndian(hdr.Slice(4, 4));
        var strTableSize = BinaryPrimitives.ReadUInt32LittleEndian(hdr.Slice(8, 4));
        var entrySize = magic == Pfs0Magic ? 24 : 64;
        if (fileCount == 0 || fileCount > 100_000 || strTableSize > 16 * 1024 * 1024)
            return [];

        var tableBytes = checked((long)fileCount * entrySize);
        var dataStart = 16L + tableBytes + strTableSize;
        if (dataStart > containerSize || baseOffset + dataStart > stream.Length)
            return [];

        var entries = new byte[tableBytes];
        stream.ReadExactly(entries);
        var names = new byte[strTableSize];
        stream.ReadExactly(names);

        var result = new List<PartitionEntry>((int)fileCount);
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
            if (relOff > (ulong)long.MaxValue || size > (ulong)long.MaxValue) continue;

            var absOff = baseOffset + dataStart + (long)relOff;
            if (absOff < 0 || absOff + (long)size > baseOffset + containerSize || absOff + (long)size > stream.Length)
                continue;

            result.Add(new PartitionEntry(name, absOff, (long)size));
        }

        return result;
    }

    private static int ExtractNca(Stream fs, PartitionEntry nca, SwitchKeySet keySet, string cache, IDiagnosticSink sink, string nspPath)
    {
        var headerBuf = new byte[0xC00];
        fs.Position = nca.AbsOffset;
        fs.ReadExactly(headerBuf);

        var headerKey = keySet.HeaderKey;
        var k1 = headerKey[..16];
        var k2 = headerKey.Slice(16, 16);

        // Decrypt sectors 0..1 (main NCA header 0x000..0x3FF) using Nintendo AES-128-XTS (big-endian sector tweak).
        DecryptNintendoXts(headerBuf.AsSpan(0, 0x400), startSector: 0, k1, k2);
        var magic = BinaryPrimitives.ReadUInt32LittleEndian(headerBuf.AsSpan(0x200, 4));
        if (magic != Nca3Magic && magic != Nca2Magic)
            return 0;

        if (magic == Nca3Magic)
        {
            DecryptNintendoXts(headerBuf.AsSpan(0x400, 0x800), startSector: 2, k1, k2);
        }
        else
        {
            // NCA2 encrypts each 0x200 FsHeader individually with sector index 0.
            for (var s = 0; s < 4; s++)
            {
                var fsEntry = headerBuf.AsSpan(0x240 + s * 16, 16);
                if (BinaryPrimitives.ReadUInt32LittleEndian(fsEntry.Slice(4, 4)) > BinaryPrimitives.ReadUInt32LittleEndian(fsEntry[..4]))
                    DecryptNintendoXts(headerBuf.AsSpan(0x400 + s * 0x200, 0x200), startSector: 0, k1, k2);
            }
        }

        var contentType = headerBuf[0x205]; // 0 = Program, 4 = Data
        if (contentType is not (0 or 4))
            return 0;

        var rawGen = Math.Max(headerBuf[0x206], headerBuf[0x220]);
        var keyGen = rawGen > 0 ? rawGen - 1 : 0;
        var rightsId = headerBuf.AsSpan(0x230, 16);

        byte[]? ctrKey = null;
        if (HasNonZeroBytes(rightsId))
        {
            if (!keySet.TryGetTitleKey(rightsId, out ctrKey))
            {
                sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                    $"Missing Switch title key for RightsId {Convert.ToHexStringLower(rightsId)} in '{nca.Name}'.",
                    new() { File = nspPath });
                return 0;
            }
        }
        else
        {
            var kakSlot = headerBuf[0x207];
            if (keySet.TryGetKeyAreaKey(kakSlot, keyGen, out var kak))
            {
                ctrKey = new byte[16];
                // Slot 2 in the NCA KeyArea holds the AES-CTR section key.
                SwitchKeySet.AesEcbDecrypt(kak, headerBuf.AsSpan(0x300 + 2 * 16, 16), ctrKey);
            }
        }

        var extracted = 0;
        for (var s = 0; s < 4; s++)
        {
            var fsEntry = headerBuf.AsSpan(0x240 + s * 16, 16);
            var startSector = BinaryPrimitives.ReadUInt32LittleEndian(fsEntry[..4]);
            var endSector = BinaryPrimitives.ReadUInt32LittleEndian(fsEntry.Slice(4, 4));
            if (endSector <= startSector) continue;

            var secOff = (long)startSector * NcaSectorSize;
            var secLen = (long)(endSector - startSector) * NcaSectorSize;
            if (secOff < 0xC00 || secOff + secLen > nca.Size) continue;

            var fsHdr = headerBuf.AsSpan(0x400 + s * 0x200, 0x200);
            var fsType = fsHdr[0x02];   // 0 = RomFS, 1 = PartitionFS
            var hashType = fsHdr[0x03]; // 0 = None, 2 = HierarchicalSha256, 3 = HierarchicalIntegrity (IVFC)
            var encType = fsHdr[0x04];  // 1 = None, 3 = AesCtr

            if (encType == 3 && ctrKey is null)
            {
                sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                    $"Missing Nintendo key_area_key (slot={headerBuf[0x207]}, gen={keyGen:X2}) for '{nca.Name}'.",
                    new() { File = nspPath });
                return extracted;
            }

            if (encType is not (1 or 3)) continue;

            if (!TryGetSectionPayloadBounds(fsHdr, hashType, secLen, out var payloadOff, out var payloadLen))
                continue;

            using Stream secStream = encType == 3
                ? new NcaAesCtrStream(fs, nca.AbsOffset, secOff, secLen, ctrKey!, fsHdr.Slice(0x140, 8))
                : new SubRangeStream(fs, nca.AbsOffset + secOff, secLen);

            if (fsType == 1)
            {
                var dummySawNca = false;
                extracted += ExtractPartitionFs(secStream, payloadOff, payloadLen, cache, prefix: "exefs", ref dummySawNca, depth: 1);
            }
            else if (fsType == 0)
            {
                extracted += ExtractRomFs(secStream, payloadOff, payloadLen, cache, rootPrefix: "romfs");
            }
        }

        return extracted;
    }

    private static bool TryGetSectionPayloadBounds(ReadOnlySpan<byte> fsHdr, byte hashType, long sectionSize, out long offset, out long size)
    {
        offset = 0;
        size = sectionSize;

        if (hashType == 0)
            return true;

        if (hashType == 2) // HierarchicalSha256 (ExeFS)
        {
            var layerCount = BinaryPrimitives.ReadUInt32LittleEndian(fsHdr.Slice(0x2C, 4));
            if (layerCount is 0 or > 5) return false;
            var regionOff = 0x30 + ((int)layerCount - 1) * 16;
            offset = (long)BinaryPrimitives.ReadUInt64LittleEndian(fsHdr.Slice(regionOff, 8));
            size = (long)BinaryPrimitives.ReadUInt64LittleEndian(fsHdr.Slice(regionOff + 8, 8));
            return offset >= 0 && size > 0 && offset + size <= sectionSize;
        }

        if (hashType == 3) // HierarchicalIntegrity / IVFC (RomFS)
        {
            if (BinaryPrimitives.ReadUInt32LittleEndian(fsHdr.Slice(0x08, 4)) != IvfcMagic)
                return false;
            var maxLayers = BinaryPrimitives.ReadUInt32LittleEndian(fsHdr.Slice(0x14, 4));
            var preferredIdx = Math.Clamp((int)maxLayers - 2, 0, 5);
            offset = (long)BinaryPrimitives.ReadUInt64LittleEndian(fsHdr.Slice(0x18 + preferredIdx * 0x18, 8));
            size = (long)BinaryPrimitives.ReadUInt64LittleEndian(fsHdr.Slice(0x18 + preferredIdx * 0x18 + 8, 8));
            if (size == 0)
            {
                for (var idx = 5; idx >= 0; idx--)
                {
                    var off = (long)BinaryPrimitives.ReadUInt64LittleEndian(fsHdr.Slice(0x18 + idx * 0x18, 8));
                    var sz = (long)BinaryPrimitives.ReadUInt64LittleEndian(fsHdr.Slice(0x18 + idx * 0x18 + 8, 8));
                    if (sz > 0)
                    {
                        offset = off;
                        size = sz;
                        break;
                    }
                }
            }
            return offset >= 0 && size > 0 && offset + size <= sectionSize;
        }

        return false;
    }

    internal static void DecryptNintendoXts(Span<byte> buffer, long startSector, ReadOnlySpan<byte> dataKey, ReadOnlySpan<byte> tweakKey)
    {
        using var aesTweak = Aes.Create();
        aesTweak.Key = tweakKey.ToArray();
        using var aesData = Aes.Create();
        aesData.Key = dataKey.ToArray();

        Span<byte> tweakInput = stackalloc byte[16];
        Span<byte> tweak = stackalloc byte[16];
        Span<byte> tmp = stackalloc byte[16];

        for (var secOff = 0; secOff + NcaSectorSize <= buffer.Length; secOff += NcaSectorSize)
        {
            var sector = startSector + secOff / NcaSectorSize;
            tweakInput.Clear();
            BinaryPrimitives.WriteUInt64BigEndian(tweakInput.Slice(8, 8), (ulong)sector);
            aesTweak.EncryptEcb(tweakInput, tweak, PaddingMode.None);

            for (var b = 0; b < NcaSectorSize; b += 16)
            {
                var slice = buffer.Slice(secOff + b, 16);
                for (var i = 0; i < 16; i++) slice[i] ^= tweak[i];
                aesData.DecryptEcb(slice, tmp, PaddingMode.None);
                for (var i = 0; i < 16; i++) slice[i] = (byte)(tmp[i] ^ tweak[i]);

                var carry = 0;
                for (var i = 0; i < 16; i++)
                {
                    var nextCarry = (tweak[i] >> 7) & 1;
                    tweak[i] = (byte)((tweak[i] << 1) | carry);
                    carry = nextCarry;
                }
                if (carry != 0) tweak[0] ^= 0x87;
            }
        }
    }

    private static bool HasNonZeroBytes(ReadOnlySpan<byte> span)
    {
        foreach (var b in span)
            if (b != 0)
                return true;
        return false;
    }

    private static int ExtractPartitionFs(Stream stream, long baseOffset, long containerSize, string cache, string prefix, ref bool sawNca, int depth)
    {
        if (depth > 3 || containerSize < 16 || baseOffset + 16 > stream.Length)
            return 0;

        var entries = ReadPartitionEntries(stream, baseOffset, containerSize);
        if (entries.Count == 0)
            return 0;

        var fullPrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var extracted = 0;
        Span<byte> peek = stackalloc byte[80];

        foreach (var entry in entries)
        {
            if (entry.Name.EndsWith(".nca", StringComparison.OrdinalIgnoreCase))
            {
                sawNca = true;
                continue;
            }

            var relPath = prefix.Length == 0 ? entry.Name : $"{prefix}/{entry.Name}";

            var peekLen = (int)Math.Min(entry.Size, 80L);
            stream.Position = entry.AbsOffset;
            stream.ReadExactly(peek[..peekLen]);

            if (peekLen >= 16 && BinaryPrimitives.ReadUInt32LittleEndian(peek) is Pfs0Magic or Hfs0Magic)
            {
                var subPrefix = Path.GetFileNameWithoutExtension(entry.Name);
                if (string.IsNullOrEmpty(subPrefix)) subPrefix = entry.Name;
                var nestedPrefix = prefix.Length == 0 ? subPrefix : $"{prefix}/{subPrefix}";
                extracted += ExtractPartitionFs(stream, entry.AbsOffset, entry.Size, cache, nestedPrefix, ref sawNca, depth + 1);
                continue;
            }

            if (LooksLikeRomFs(peek[..peekLen], entry.Size))
            {
                var subPrefix = entry.Name.Contains("romfs", StringComparison.OrdinalIgnoreCase) ? "romfs" : Path.GetFileNameWithoutExtension(entry.Name);
                extracted += ExtractRomFs(stream, entry.AbsOffset, entry.Size, cache, string.IsNullOrEmpty(subPrefix) ? "romfs" : subPrefix);
                continue;
            }

            var isMainNso = entry.Name.Equals("main", StringComparison.OrdinalIgnoreCase)
                && peekLen >= 4 && BinaryPrimitives.ReadUInt32LittleEndian(peek) == Nso0Magic;
            if (isMainNso || WantedNames.Contains(Path.GetFileName(entry.Name)))
            {
                var target = Path.GetFullPath(Path.Combine(cache, relPath.Replace('/', Path.DirectorySeparatorChar)));
                if (target.StartsWith(fullPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    CopyRangeToFile(stream, entry.AbsOffset, entry.Size, target);
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
        if (romfsSize < 0x50 || romfsBase + 0x50 > stream.Length)
            return 0;

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

    private sealed class SubRangeStream(Stream inner, long baseOffset, long length) : Stream
    {
        private long _pos;
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => Math.Min(length, Math.Max(0, inner.Length - baseOffset));
        public override long Position { get => _pos; set => _pos = Math.Max(0, value); }
        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_pos >= Length || count <= 0) return 0;
            var avail = (int)Math.Min(count, Length - _pos);
            inner.Position = baseOffset + _pos;
            var read = inner.Read(buffer, offset, avail);
            _pos += read;
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin) => Position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _pos + offset,
            SeekOrigin.End => Length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    /// <summary>Random-access AES-128-CTR section stream per Horizon OS / <c>hactool</c> NCA specification.</summary>
    private sealed class NcaAesCtrStream : Stream
    {
        private const int ChunkSize = 0x10000;
        private readonly Stream _fs;
        private readonly long _ncaBaseOffset;
        private readonly long _sectionOffset;
        private readonly long _length;
        private readonly Aes _aes;
        private readonly byte[] _baseCtrUpper = new byte[8];
        private readonly byte[] _chunk = new byte[ChunkSize];
        private long _cachedChunkIdx = -1;
        private long _pos;

        public NcaAesCtrStream(Stream fs, long ncaBaseOffset, long sectionOffset, long length, byte[] key, ReadOnlySpan<byte> sectionCtr)
        {
            _fs = fs;
            _ncaBaseOffset = ncaBaseOffset;
            _sectionOffset = sectionOffset;
            _length = length;
            _aes = Aes.Create();
            _aes.Key = key;
            for (var j = 0; j < 8; j++)
                _baseCtrUpper[j] = sectionCtr[7 - j];
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => _length;
        public override long Position { get => _pos; set => _pos = Math.Max(0, value); }
        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_pos >= _length || count <= 0) return 0;
            var remaining = (int)Math.Min(count, _length - _pos);
            var total = 0;
            while (remaining > 0)
            {
                var chunkIdx = _pos / ChunkSize;
                EnsureChunk(chunkIdx);
                var inChunk = (int)(_pos % ChunkSize);
                var avail = Math.Min(remaining, ChunkSize - inChunk);
                Buffer.BlockCopy(_chunk, inChunk, buffer, offset + total, avail);
                _pos += avail;
                total += avail;
                remaining -= avail;
            }
            return total;
        }

        private void EnsureChunk(long chunkIdx)
        {
            if (_cachedChunkIdx == chunkIdx) return;
            var relStart = chunkIdx * ChunkSize;
            var avail = (int)Math.Min(ChunkSize, Math.Max(0, _length - relStart));
            if (avail < ChunkSize) Array.Clear(_chunk);
            if (avail > 0)
            {
                _fs.Position = _ncaBaseOffset + _sectionOffset + relStart;
                _fs.ReadExactly(_chunk.AsSpan(0, avail));
            }

            Span<byte> ctr = stackalloc byte[16];
            Span<byte> keystream = stackalloc byte[16];
            _baseCtrUpper.CopyTo(ctr);
            for (var off = 0; off < avail; off += 16)
            {
                var blockCounter = (ulong)((_sectionOffset + relStart + off) >> 4);
                BinaryPrimitives.WriteUInt64BigEndian(ctr.Slice(8, 8), blockCounter);
                _aes.EncryptEcb(ctr, keystream, PaddingMode.None);
                var blockLen = Math.Min(16, avail - off);
                for (var b = 0; b < blockLen; b++)
                    _chunk[off + b] ^= keystream[b];
            }
            _cachedChunkIdx = chunkIdx;
        }

        public override long Seek(long offset, SeekOrigin origin) => Position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _pos + offset,
            SeekOrigin.End => _length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing) _aes.Dispose();
            base.Dispose(disposing);
        }
    }
}
