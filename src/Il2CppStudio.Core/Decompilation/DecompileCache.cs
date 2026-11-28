using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Il2CppStudio.Core.Decompilation;

internal readonly record struct CachedDecompiledType(int TypeIndex, string FileName, string CSharpText, string IlText);

/// <summary>
/// Compressed per-assembly cache of decompiled C# (and optional IL) type outputs keyed by SHA-256 over the binary/metadata
/// fingerprint, assembly image name, decompilation options, and BCL reference path. Written atomically via a temporary file.
/// </summary>
internal static class DecompileCache
{
    private const uint Magic = 0x43444C49; // "ILDC"
    private const int FormatVersion = 1;

    public static bool IsDisabled => Environment.GetEnvironmentVariable("IL2CPPSTUDIO_NO_CACHE") == "1";

    public static string CacheDirectory
    {
        get
        {
            var overrideDir = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_CACHE_DIR");
            return !string.IsNullOrWhiteSpace(overrideDir)
                ? overrideDir
                : Path.Combine(Path.GetTempPath(), "Il2CppStudio", "decompile-cache");
        }
    }

    public static string ComputeKey(
        string fingerprint,
        string imageName,
        bool includeIl,
        bool substituteBcl,
        bool showTokens,
        string? bclRefDir,
        bool relaxInlinedAccess = false)
    {
        using var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        Span<byte> hdr = stackalloc byte[8];
        BinaryPrimitives.WriteInt32LittleEndian(hdr[0..4], FormatVersion);
        hdr[4] = includeIl ? (byte)1 : (byte)0;
        hdr[5] = substituteBcl ? (byte)1 : (byte)0;
        hdr[6] = showTokens ? (byte)1 : (byte)0;
        hdr[7] = relaxInlinedAccess ? (byte)1 : (byte)0;
        hasher.AppendData(hdr);

        Span<byte> mvid = stackalloc byte[16];
        typeof(DecompileCache).Assembly.ManifestModule.ModuleVersionId.TryWriteBytes(mvid);
        hasher.AppendData(mvid);

        AppendLengthPrefixedUtf8(hasher, fingerprint);
        AppendLengthPrefixedUtf8(hasher, imageName);
        AppendLengthPrefixedUtf8(hasher, bclRefDir ?? "");

        Span<byte> hash = stackalloc byte[32];
        hasher.TryGetHashAndReset(hash, out _);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static void AppendLengthPrefixedUtf8(IncrementalHash hasher, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        Span<byte> lenBuf = stackalloc byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(lenBuf, bytes.Length);
        hasher.AppendData(lenBuf);
        hasher.AppendData(bytes);
    }

    public static bool TryRead(string cacheKey, out List<CachedDecompiledType> entries, string? cacheDir = null)
    {
        entries = [];
        if (IsDisabled || string.IsNullOrEmpty(cacheKey)) return false;

        var path = Path.Combine(cacheDir ?? CacheDirectory, cacheKey + ".dcache");
        if (!File.Exists(path)) return false;

        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var hdrBr = new BinaryReader(fs, Encoding.UTF8, leaveOpen: true);
            if (fs.Length < 12) return false;
            if (hdrBr.ReadUInt32() != Magic || hdrBr.ReadInt32() != FormatVersion) return false;
            if (hdrBr.ReadString() != cacheKey) return false;

            using var brotli = new BrotliStream(fs, CompressionMode.Decompress, leaveOpen: false);
            using var buffered = new BufferedStream(brotli, 65536);
            using var br = new BinaryReader(buffered, Encoding.UTF8);
            var count = br.ReadInt32();
            if (count < 0 || count > 2_000_000) return false;
            entries = new List<CachedDecompiledType>(count);
            for (var i = 0; i < count; i++)
            {
                var typeIndex = br.ReadInt32();
                var fileName = br.ReadString();
                var cs = br.ReadString();
                var il = br.ReadString();
                entries.Add(new CachedDecompiledType(typeIndex, fileName, cs, il));
            }
            return true;
        }
        catch
        {
            entries = [];
            return false;
        }
    }

    public static void Write(string cacheKey, IReadOnlyList<CachedDecompiledType> entries, string? cacheDir = null)
    {
        if (IsDisabled || string.IsNullOrEmpty(cacheKey)) return;

        var dir = cacheDir ?? CacheDirectory;
        var path = Path.Combine(dir, cacheKey + ".dcache");
        var tmp = Path.Combine(dir, $"{cacheKey}.{Guid.NewGuid():N}.tmp");
        try
        {
            Directory.CreateDirectory(dir);
            using (var fs = new FileStream(tmp, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (var hdrBw = new BinaryWriter(fs, Encoding.UTF8, leaveOpen: true))
                {
                    hdrBw.Write(Magic);
                    hdrBw.Write(FormatVersion);
                    hdrBw.Write(cacheKey);
                }
                using var brotli = new BrotliStream(fs, new BrotliCompressionOptions { Quality = 0 }, leaveOpen: false);
                using var buffered = new BufferedStream(brotli, 65536);
                using var bw = new BinaryWriter(buffered, Encoding.UTF8);
                bw.Write(entries.Count);
                foreach (var e in entries)
                {
                    bw.Write(e.TypeIndex);
                    bw.Write(e.FileName);
                    bw.Write(e.CSharpText);
                    bw.Write(e.IlText);
                }
            }
            File.Move(tmp, path, overwrite: true);
        }
        catch
        {
            try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
        }
    }
}
