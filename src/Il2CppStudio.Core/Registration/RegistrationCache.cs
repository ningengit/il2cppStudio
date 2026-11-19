using System.Buffers.Binary;
using System.Security.Cryptography;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Registration;

internal readonly record struct CachedRegistration(
    ulong MetadataRegistrationVa,
    ulong CodeRegistrationVa,
    ulong CodeGenModulesArrayVa,
    Dictionary<ulong, MetadataUsage> MetadataUsages);

/// <summary>
/// Binary cache of scanned IL2CPP registration VAs and runtime metadata-usage globals keyed by SHA-256 over the
/// binary and <c>global-metadata.dat</c> edge chunks, lengths, and modification timestamps. Avoids re-scanning
/// multi-megabyte data sections on repeat loads while invalidating automatically when inputs change.
/// </summary>
internal static class RegistrationCache
{
    private const uint Magic = 0x43524C49; // "ILRC"
    private const int FormatVersion = 1;
    private const int EdgeSampleBytes = 64 * 1024;

    public static string CacheDirectory
    {
        get
        {
            var overrideDir = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR");
            return !string.IsNullOrWhiteSpace(overrideDir)
                ? overrideDir
                : Path.Combine(Path.GetTempPath(), "Il2CppStudio", "reg-cache");
        }
    }

    public static string ComputeKey(IBinaryImage image, GlobalMetadata md)
    {
        using var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        Span<byte> header = stackalloc byte[16];
        BinaryPrimitives.WriteInt32LittleEndian(header[0..4], FormatVersion);
        BinaryPrimitives.WriteInt32LittleEndian(header[4..8], image.Is64Bit ? 8 : 4);
        BinaryPrimitives.WriteInt32LittleEndian(header[8..12], md.Version.Major);
        BinaryPrimitives.WriteInt32LittleEndian(header[12..16], md.Version.Minor);
        hasher.AppendData(header);

        AppendFileIdentity(hasher, image.Data.Span, image.Path);
        AppendFileIdentity(hasher, md.RawFile, md.Path);

        Span<byte> hash = stackalloc byte[32];
        hasher.TryGetHashAndReset(hash, out _);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static void AppendFileIdentity(IncrementalHash hasher, ReadOnlySpan<byte> bytes, string? path)
    {
        Span<byte> meta = stackalloc byte[16];
        BinaryPrimitives.WriteInt64LittleEndian(meta[0..8], bytes.Length);
        BinaryPrimitives.WriteInt64LittleEndian(meta[8..16], SafeMtimeUtcTicks(path));
        hasher.AppendData(meta);

        var headLen = Math.Min(EdgeSampleBytes, bytes.Length);
        if (headLen > 0) hasher.AppendData(bytes[..headLen]);
        if (bytes.Length > headLen)
        {
            var tailLen = Math.Min(EdgeSampleBytes, bytes.Length);
            hasher.AppendData(bytes[(bytes.Length - tailLen)..]);
        }
    }

    private static long SafeMtimeUtcTicks(string? path)
    {
        try
        {
            return !string.IsNullOrWhiteSpace(path) && File.Exists(path)
                ? File.GetLastWriteTimeUtc(path).Ticks
                : 0L;
        }
        catch
        {
            return 0L;
        }
    }

    public static bool TryRead(IBinaryImage image, GlobalMetadata md, out CachedRegistration cached, string? cacheDir = null)
    {
        cached = default;
        var cachePath = Path.Combine(cacheDir ?? CacheDirectory, ComputeKey(image, md) + ".bin");
        if (!File.Exists(cachePath)) return false;

        try
        {
            using var fs = new FileStream(cachePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var br = new BinaryReader(fs);
            if (fs.Length < 4 + 4 + 8 + 8 + 8 + 4) return false;
            if (br.ReadUInt32() != Magic || br.ReadInt32() != FormatVersion) return false;

            var metaVa = br.ReadUInt64();
            var codeVa = br.ReadUInt64();
            var modulesVa = br.ReadUInt64();
            var usageCount = br.ReadInt32();
            if (usageCount < 0 || usageCount > 10_000_000) return false;

            const int recordBytes = 8 + 1 + 4;
            var expectedLength = fs.Position + (long)usageCount * recordBytes;
            if (fs.Length != expectedLength) return false;

            var usages = new Dictionary<ulong, MetadataUsage>(usageCount);
            for (var i = 0; i < usageCount; i++)
            {
                var va = br.ReadUInt64();
                var kindByte = br.ReadByte();
                var idx = br.ReadInt32();
                if (kindByte > (byte)MetadataUsageKind.FieldRva) return false;
                usages[va] = new MetadataUsage(va, (MetadataUsageKind)kindByte, idx);
            }

            cached = new CachedRegistration(metaVa, codeVa, modulesVa, usages);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void TryWrite(IBinaryImage image, GlobalMetadata md, in CachedRegistration entry, string? cacheDir = null)
    {
        var dir = cacheDir ?? CacheDirectory;
        var key = ComputeKey(image, md);
        var cachePath = Path.Combine(dir, key + ".bin");
        var tmpPath = Path.Combine(dir, $"{key}.{Guid.NewGuid():N}.tmp");

        try
        {
            Directory.CreateDirectory(dir);
            using (var fs = new FileStream(tmpPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(Magic);
                bw.Write(FormatVersion);
                bw.Write(entry.MetadataRegistrationVa);
                bw.Write(entry.CodeRegistrationVa);
                bw.Write(entry.CodeGenModulesArrayVa);
                bw.Write(entry.MetadataUsages.Count);
                foreach (var u in entry.MetadataUsages.Values)
                {
                    bw.Write(u.Va);
                    bw.Write((byte)u.Kind);
                    bw.Write(u.Index);
                }
            }
            File.Move(tmpPath, cachePath, overwrite: true);
        }
        catch
        {
            try { if (File.Exists(tmpPath)) File.Delete(tmpPath); } catch { }
        }
    }
}
