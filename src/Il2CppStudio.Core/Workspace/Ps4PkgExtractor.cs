using System.Buffers.Binary;
using System.Diagnostics;
using System.IO.Compression;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Il2CppStudio.Core.Diagnostics;
using Microsoft.Win32.SafeHandles;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts IL2CPP binaries (<c>Media/Modules/Il2CppUserAssemblies.prx</c>), <c>global-metadata.dat</c>, and Unity
/// data headers from a PlayStation 4 <c>.pkg</c> container natively in C#.
/// Ports the MIT/Apache-2.0 PKG/PFS/PFSC logic from <c>orbis-pkg</c>/<c>orbis-pfs</c>: supports Fake-PKG RSA key
/// unwinding (<c>EntryKeys[3]</c> + <c>PfsImageKey</c>), optional user-supplied EKPFS/passcode, signed
/// (<c>dinodeS32</c>) and unsigned (<c>dinodeD32</c>) inodes, contiguous/direct/indirect block maps, <c>PFSC</c>
/// zlib decompression, and <c>uroot</c> directory unwrapping.
/// </summary>
public static class Ps4PkgExtractor
{
    private const uint CntMagicBe = 0x7F434E54;
    private const uint PfsMagic = 0x01332A0B;
    private const uint PfscMagic = 0x43534650; // "PFSC"
    private const uint PkgEntryIdKeys = 0x00000010;
    private const uint PkgEntryIdPfsImageKey = 0x00000020;
    private const int Dinode32Size = 0xA8;
    private const int DinodeS32Size = 0x2C8;

    private static readonly HashSet<string> WantedNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Il2CppUserAssemblies.prx",
        "global-metadata.dat",
        "globalgamemanagers",
        "data.unity3d",
        "boot.config",
        "ScriptingAssemblies.json",
        "app.info",
        "eboot.bin",
    };

    private static readonly Lazy<RSAParameters> PkgKey3Params = new(() => BuildRsaParameters(
        "d212fc335f6ddb831609628b0356273782d477853529392d526b8c4c8cfb06c1" +
        "845be7d4f7bcd24e6245cd2abbd77776453655273fb3f5f98eda4befaa59aeb3" +
        "9bea5498d206326a58312ae0d44f90b50a7decf43a9c52672d99318e0c43e682" +
        "fe0746e12e50d41f2d2f7ed908ba06b3bf2e203f4e3ffe44ffaa504357916994" +
        "49158282e40f4c8d9d2cc95b1d64bf888bd4c594e76547841ee57910fb989347" +
        "b97d8512a640982cf792bc951932ede890560d65c1aa78c62e54fd5f54a1f67e" +
        "e5e05f61c120b4b9b4330870e4df8956ed012946775f8cb8a9f51e2eb3b9bfe0" +
        "09b78d28d4a6c3b81e1f07ebb4120b95b88530fddc3913d07cdc8fedf9c9a3c1",
        "32d903908fbdb08f572b285e0b8db3ea5cd17ea890888cdd6a80bbb1dfc1f70d" +
        "aa32f0b77ccb88800e8b64b0be4cd60e9b8c1e2a64e1f35cd77601415e935c94" +
        "fedd4662c31b5ae2a0bc2debc3980aa7b7856970682b644ab31fcc7ddc7c26f4" +
        "77f65cf2ae5a442dd3ab16620419bafb90ffe23050896ecb56b2ebc09116925e" +
        "308eaec7945dfd35e120f8ad3ebc08bfc036749fd5bb5208fd0666f37ab304f4" +
        "75295de95faa1030b20f5a1ac12ab3fecb21ad80ec8f20091cdbc55894c29cc6" +
        "ce82653e5790bca98b06b4f072f677df9864f1ecfe372dbcae8c08811fc3c989" +
        "1ac742824b2edc8e8d73ceb1ccc001d90870873c4408ec498f815ae240ff77fc0d",
        "f967ad9912310c56a22e161c46b34d5b43be42a2f686968042c3c73fc342f587" +
        "49339f075d6e2c04fde3e1b2ae0a0cf0c7a61ca16350c8099c5124526c5e5ebd" +
        "1e2706bbbc9e94e135d46db3cb3c68dd68b3fe6ccb8d8220762363b7e9681001" +
        "4edcba275d01c12d805e2baf826bd884b6105286a7898eae9ae289c6f7d587fb",
        "d7a10f9a8bf2c91195329a8cf0d94047f568a00dbdc1fc432f65f9c3610f2577" +
        "54add758ac8440608d3ff3658975b5c62c511a2f1f22e4431154bec9b4c7b51b" +
        "050bbc569acd4ad973685e5cfb92b78b0dfff507cab4c89b963c079e3e6b2a11" +
        "f28ab18ad72e1ba5532406ed50b89067b1e241c69201ee10f061bbfbb27d4a73"));

    private static readonly Lazy<RSAParameters> FakePfsKeyParams = new(() => BuildRsaParameters(
        "c6cf71e7e59af0d12a2c458bf92a0ec143058bc37117801dcd497dde359d259b" +
        "a0d7a0f27d6c087eaa5502682b23c644b84418eb56cf16a24803c9e74f87eb3d" +
        "30c31588bf20e79dff770cde1d241e63a94f8abf5bbe601968333bfced9f474e" +
        "5ff8eacb3d00bd6701f92c6dc6ac1364e76714f3dc52696ab9832c4230131bb2" +
        "d8a5020d79ed96b10df8cc0cdf81954f035809570e80692efeff5277ea7528a8" +
        "fbc9bebf9fbbb7798e1805e180bd50349481d353c269a2d24ccf6cf4572c104a" +
        "3ffb22fd8b97e2c95ba62bcdd61b6bdb687f4bc2a05034c005e58def2467ff93" +
        "40cf2d62a2a050b1f13aa83dfd80d1f9b80522afc8354590588ee33a7cbd3e27",
        "7f76cd0ee2d4de051cc6d9a80e8dfa7bca1eaa271a40f8f1228735dddbfdeef8" +
        "c2bcbd01fb8be23e63b2b1225c56496e11be07440b9a2666d1492c8fd31bcfa4" +
        "a1b8d1fba49ed2212883098af6a00ba3d60f9b6368ccbc0c4e145b27a4a9f42b" +
        "b9b87bc0e651ad1d77d46bb9ce20d126667e5e9ea2e96b90f373b8528f441103" +
        "0c1397393d132258d5438249da6e7ca1c58ca5b009e0ce3ddff49d3c9715e26a" +
        "c72b3c509323dbba4a226644ac78bb0e1a2743b57167aff4ab48469373d042ab" +
        "9363e56c9ade5024c0237d99793f2207e0c148561bdf830912b42d456bc9c068" +
        "85999079961ad7f54d1f3783404aec3937a680927dc580c7d66ffe8a7989c6b1",
        "fef6bf1d69ab16250847556b86e43588722ab13df8b644cab3ab19d10424280a" +
        "7455b8154509cc131cf2ba37a903908f0210ff257986cc18509a105f5b4c1c4e" +
        "b0a7e359b12da0c6b0202c213312b3af723483cd522faf0f205a1bc0e2a37634" +
        "0fd7fcc141c9f979401742213e9dfdc7c150de445ac931896a7805be65b4e82d",
        "c79e4758007d6282b0d22281d4a8971b790c3ab0d7c930e3c3538e57eff09b9f" +
        "b39052c6942236aae64a5f721d70e87658c8b291ce9cc3e9097f2e4797cc9039" +
        "153531de1f0c8c0dc1c292be97bf2f91a18c7d50a8212fd7a29a7eb5a72a9002" +
        "d9f33dd1ebb8e05a799e7d8dca186dbd9ea180286b2afe51249b6f4d84778023"));

    private static readonly byte[] RsaExponent = [0x01, 0x00, 0x01];

    public static bool IsPkgPath(string path) =>
        Path.GetExtension(path).Equals(".pkg", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns <c>true</c> only when the package has an encrypted PFS image that cannot be decrypted automatically
    /// with Fake-PKG keys or <c>IL2CPPSTUDIO_PKG_EKPFS</c>.
    /// </summary>
    public static bool NeedsUserKey(string pkgPath)
    {
        if (!File.Exists(pkgPath) || TryParseUserEkpfs(null, out _))
            return false;

        try
        {
            using var handle = File.OpenHandle(pkgPath, FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.RandomAccess);
            var fileLen = RandomAccess.GetLength(handle);
            if (fileLen < 0x500)
                return false;

            Span<byte> hdr = stackalloc byte[0x480];
            RandomAccess.Read(handle, hdr, 0);
            if (BinaryPrimitives.ReadUInt32BigEndian(hdr) != CntMagicBe)
                return false;

            var pfsOffset = BinaryPrimitives.ReadUInt64BigEndian(hdr.Slice(0x410, 8));
            if (pfsOffset == 0 || pfsOffset + 0x50 > (ulong)fileLen)
                return false;

            Span<byte> pfsSuper = stackalloc byte[0x50];
            RandomAccess.Read(handle, pfsSuper, (long)pfsOffset);
            if (BinaryPrimitives.ReadUInt32LittleEndian(pfsSuper.Slice(8, 4)) != PfsMagic)
                return false;

            var mode = BinaryPrimitives.ReadUInt16LittleEndian(pfsSuper.Slice(0x1C, 2));
            if ((mode & 0x4) == 0)
                return false;

            return !TryDeriveFakePkgEkpfs(handle, fileLen, hdr, out _);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or CryptographicException)
        {
            return false;
        }
    }

    public static string? Extract(string pkgPath, IDiagnosticSink sink) =>
        Extract(pkgPath, sink, (string?)null);

    public static string? Extract(string pkgPath, IDiagnosticSink sink, byte[]? ekpfs) =>
        Extract(pkgPath, sink, ekpfs is { Length: 32 } ? Convert.ToHexString(ekpfs) : null);

    public static string? Extract(string pkgPath, IDiagnosticSink sink, string? passcodeOrEkpfs)
    {
        var info = new FileInfo(pkgPath);
        if (!info.Exists)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{pkgPath}' does not exist.", new() { File = pkgPath });
            return null;
        }

        byte[]? explicitEkpfs = null;
        if (!string.IsNullOrWhiteSpace(passcodeOrEkpfs) && !TryParseHex32(passcodeOrEkpfs, out explicitEkpfs) && passcodeOrEkpfs.Trim().Length != 32)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                "PS4 package key must be a 64-character hex EKPFS key or 32-character passcode.",
                new() { File = pkgPath });
            return null;
        }

        if (explicitEkpfs is null && TryParseUserEkpfs(null, out var envEkpfs))
            explicitEkpfs = envEkpfs;

        var keySuffix = explicitEkpfs is { Length: 32 }
            ? "-" + Convert.ToHexString(SHA256.HashData(explicitEkpfs)).AsSpan(0, 8).ToString()
            : string.Empty;
        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "pkg",
            $"{Path.GetFileName(pkgPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}{keySuffix}");
        if (File.Exists(Path.Combine(cache, ".complete")) && HasMetadata(cache))
            return cache;

        try
        {
            using var handle = File.OpenHandle(pkgPath, FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.RandomAccess);
            var fileLen = RandomAccess.GetLength(handle);
            if (fileLen < 0x500)
                throw new InvalidDataException("File is too small to be a PS4 .pkg container.");

            Span<byte> hdr = stackalloc byte[0x480];
            ReadExactAt(handle, 0, hdr);
            if (BinaryPrimitives.ReadUInt32BigEndian(hdr) != CntMagicBe)
                throw new InvalidDataException("Missing PS4 CNT (.pkg) header magic 0x7F434E54.");

            var pfsOffset = BinaryPrimitives.ReadUInt64BigEndian(hdr.Slice(0x410, 8));
            var pfsSize = BinaryPrimitives.ReadUInt64BigEndian(hdr.Slice(0x418, 8));
            if (pfsOffset == 0 || pfsOffset >= (ulong)fileLen)
                throw new InvalidDataException($"Invalid PFS image offset 0x{pfsOffset:X} in PS4 .pkg.");

            var availablePfsLen = Math.Min((long)pfsSize, fileLen - (long)pfsOffset);
            if (availablePfsLen < 0x400)
                throw new InvalidDataException($"PFS image at 0x{pfsOffset:X} is truncated.");

            Span<byte> pfsSuper = stackalloc byte[0x390];
            ReadExactAt(handle, (long)pfsOffset, pfsSuper);
            if (BinaryPrimitives.ReadUInt32LittleEndian(pfsSuper.Slice(8, 4)) != PfsMagic)
                throw new InvalidDataException("Missing PFS superblock magic in PS4 .pkg.");

            var mode = BinaryPrimitives.ReadUInt16LittleEndian(pfsSuper.Slice(0x1C, 2));
            var blockSize = BinaryPrimitives.ReadUInt32LittleEndian(pfsSuper.Slice(0x20, 4));
            var isEncrypted = (mode & 0x4) != 0;

            byte[]? ekpfs = explicitEkpfs;
            if (isEncrypted && ekpfs is null)
            {
                if (!TryDeriveFakePkgEkpfs(handle, fileLen, hdr, out ekpfs))
                {
                    if (TryExtractWithTool(pkgPath, cache, sink))
                        return cache;

                    sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                        $"PS4 package '{Path.GetFileName(pkgPath)}' uses an encrypted PFS image (mode=0x{mode:X}) whose EKPFS could not be recovered automatically. " +
                        "Pass --passcode <64-hex-EKPFS>, set IL2CPPSTUDIO_PKG_EKPFS, open the unpacked PS4 game folder, or configure pkgextract via PATH / IL2CPPSTUDIO_PKGEXTRACT.",
                        new() { File = pkgPath });
                    return null;
                }
            }

            PfsXtsReader? xtsReader = null;
            try
            {
                IImageReader outerReader = new SafeHandleRegionReader(handle, (long)pfsOffset, availablePfsLen);
                if (isEncrypted)
                {
                    var cryptSeed = pfsSuper.Slice(0x370, 16);
                    xtsReader = new PfsXtsReader(outerReader, blockSize, ekpfs!, cryptSeed);
                    outerReader = xtsReader;
                }

                Directory.CreateDirectory(cache);
                var count = ExtractPfsImage(outerReader, cache, allowNested: true);
                if (count > 0 && HasMetadata(cache))
                {
                    File.WriteAllText(Path.Combine(cache, ".complete"), pkgPath);
                    sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
                        $"PKG: extracted {count} IL2CPP files from '{Path.GetFileName(pkgPath)}' to '{cache}'.",
                        new() { File = pkgPath });
                    return cache;
                }
            }
            finally
            {
                xtsReader?.Dispose();
            }

            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound,
                $"No global-metadata.dat found inside PS4 package '{Path.GetFileName(pkgPath)}'.",
                new() { File = pkgPath });
            return null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidDataException or CryptographicException)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract PS4 package '{Path.GetFileName(pkgPath)}': {ex.Message}", new() { File = pkgPath }, ex);
            return null;
        }
    }

    /// <summary>
    /// Ports <c>orbis-pkg</c>'s <c>load_entry_key3()</c> + <c>load_ekpfs()</c>: decrypts <c>EntryKeys[3]</c> with
    /// <c>pkg_key3()</c>, AES-128-CBC decrypts <c>PfsImageKey</c>, then RSA-decrypts EKPFS with <c>fake_pfs_key()</c>.
    /// </summary>
    private static bool TryDeriveFakePkgEkpfs(SafeFileHandle handle, long fileLen, ReadOnlySpan<byte> hdr, out byte[] ekpfs)
    {
        ekpfs = [];
        try
        {
            var entryCount = BinaryPrimitives.ReadUInt32BigEndian(hdr.Slice(0x10, 4));
            var tableOffset = BinaryPrimitives.ReadUInt32BigEndian(hdr.Slice(0x18, 4));
            if (entryCount == 0 || entryCount > 10_000 || (long)tableOffset + (long)entryCount * 32 > fileLen)
                return false;

            var tableBytes = new byte[entryCount * 32];
            ReadExactAt(handle, tableOffset, tableBytes);

            ReadOnlySpan<byte> entryKeysRaw = default;
            ReadOnlySpan<byte> pfsImageKeyRaw = default;
            var hasEntryKeys = false;
            var hasPfsImageKey = false;

            for (var i = 0; i < (int)entryCount; i++)
            {
                var e = tableBytes.AsSpan(i * 32, 32);
                var id = BinaryPrimitives.ReadUInt32BigEndian(e[..4]);
                if (id == PkgEntryIdKeys)
                {
                    entryKeysRaw = e;
                    hasEntryKeys = true;
                }
                else if (id == PkgEntryIdPfsImageKey)
                {
                    pfsImageKeyRaw = e;
                    hasPfsImageKey = true;
                }
            }

            if (!hasEntryKeys || !hasPfsImageKey)
                return false;

            var keysOff = (long)BinaryPrimitives.ReadUInt32BigEndian(entryKeysRaw.Slice(16, 4));
            var keysSize = (long)BinaryPrimitives.ReadUInt32BigEndian(entryKeysRaw.Slice(20, 4));
            const int key3OffsetInEntryKeys = 32 + 7 * 32 + 3 * 256; // seed[32] + digests[7*32] + keys[0..2]
            if (keysSize < key3OffsetInEntryKeys + 256 || keysOff + keysSize > fileLen)
                return false;

            Span<byte> encEntryKey3 = stackalloc byte[256];
            ReadExactAt(handle, keysOff + key3OffsetInEntryKeys, encEntryKey3);

            byte[] entryKey3;
            using (var rsa3 = RSA.Create(PkgKey3Params.Value))
                entryKey3 = rsa3.Decrypt(encEntryKey3, RSAEncryptionPadding.Pkcs1);

            var imgFlags1 = BinaryPrimitives.ReadUInt32BigEndian(pfsImageKeyRaw.Slice(8, 4));
            var imgFlags2 = BinaryPrimitives.ReadUInt32BigEndian(pfsImageKeyRaw.Slice(12, 4));
            var imgOff = (long)BinaryPrimitives.ReadUInt32BigEndian(pfsImageKeyRaw.Slice(16, 4));
            var imgSize = (int)BinaryPrimitives.ReadUInt32BigEndian(pfsImageKeyRaw.Slice(20, 4));
            if (imgSize < 256 || imgSize > 4096)
                return false;

            var isEncrypted = (imgFlags1 & 0x80000000) != 0;
            var keyIndex = (imgFlags2 & 0xF000) >> 12;
            var paddedSize = isEncrypted ? (imgSize + 15) & ~15 : imgSize;
            if (imgOff + paddedSize > fileLen)
                return false;

            var imgData = new byte[paddedSize];
            ReadExactAt(handle, imgOff, imgData);

            byte[] encryptedEkpfs;
            if (isEncrypted)
            {
                if (keyIndex != 3)
                    return false;

                var hashInput = new byte[32 + entryKey3.Length];
                pfsImageKeyRaw.CopyTo(hashInput);
                entryKey3.CopyTo(hashInput.AsSpan(32));
                var secret = SHA256.HashData(hashInput);

                using var aes = Aes.Create();
                aes.Key = secret[16..32];
                encryptedEkpfs = aes.DecryptCbc(imgData, secret[0..16], PaddingMode.None)[..imgSize];
            }
            else
            {
                encryptedEkpfs = imgData[..imgSize];
            }

            using var rsaFake = RSA.Create(FakePfsKeyParams.Value);
            ekpfs = rsaFake.Decrypt(encryptedEkpfs, RSAEncryptionPadding.Pkcs1);
            return ekpfs.Length == 32;
        }
        catch (CryptographicException)
        {
            return false;
        }
        catch (InvalidDataException)
        {
            return false;
        }
    }

    private static RSAParameters BuildRsaParameters(string modulusHex, string dHex, string pHex, string qHex)
    {
        var dBytes = Convert.FromHexString(dHex);
        var pBytes = Convert.FromHexString(pHex);
        var qBytes = Convert.FromHexString(qHex);

        var d = new BigInteger(dBytes, isUnsigned: true, isBigEndian: true);
        var p = new BigInteger(pBytes, isUnsigned: true, isBigEndian: true);
        var q = new BigInteger(qBytes, isUnsigned: true, isBigEndian: true);
        var dp = d % (p - BigInteger.One);
        var dq = d % (q - BigInteger.One);
        var inverseQ = BigInteger.ModPow(q, p - 2, p);

        return new RSAParameters
        {
            Modulus = Convert.FromHexString(modulusHex),
            Exponent = RsaExponent,
            D = ToFixedBigEndian(d, 256),
            P = ToFixedBigEndian(p, 128),
            Q = ToFixedBigEndian(q, 128),
            DP = ToFixedBigEndian(dp, 128),
            DQ = ToFixedBigEndian(dq, 128),
            InverseQ = ToFixedBigEndian(inverseQ, 128),
        };
    }

    private static byte[] ToFixedBigEndian(BigInteger value, int size)
    {
        var raw = value.ToByteArray(isUnsigned: true, isBigEndian: true);
        if (raw.Length == size) return raw;
        if (raw.Length > size) return raw[^size..];
        var padded = new byte[size];
        raw.CopyTo(padded.AsSpan(size - raw.Length));
        return padded;
    }

    private static bool TryParseUserEkpfs(string? overrideValue, out byte[] ekpfs)
    {
        var raw = overrideValue ?? Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PKG_EKPFS");
        return TryParseHex32(raw, out ekpfs);
    }

    private static bool TryParseHex32(string? input, out byte[] bytes)
    {
        bytes = [];
        var raw = input?.Trim();
        if (string.IsNullOrEmpty(raw)) return false;
        raw = raw.Replace("-", string.Empty).Replace(" ", string.Empty);
        if (raw.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) raw = raw[2..];
        if (raw.Length != 64) return false;
        try
        {
            bytes = Convert.FromHexString(raw);
            return bytes.Length == 32;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static bool TryExtractWithTool(string pkgPath, string cache, IDiagnosticSink sink)
    {
        var tool = FindPkgExtractTool();
        if (tool is null) return false;

        Directory.CreateDirectory(cache);
        using var proc = Process.Start(new ProcessStartInfo
        {
            FileName = tool,
            ArgumentList = { "-o", cache, "-f", pkgPath },
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        });
        if (proc is null) return false;
        if (!proc.WaitForExit(120_000))
        {
            try { proc.Kill(entireProcessTree: true); } catch { /* best effort */ }
            return false;
        }
        if (proc.ExitCode != 0 || !HasMetadata(cache))
        {
            var err = proc.StandardError.ReadToEnd().Trim();
            if (err.Length > 0)
                sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed, $"pkgextract: {err}", new() { File = pkgPath });
            return false;
        }

        File.WriteAllText(Path.Combine(cache, ".complete"), pkgPath);
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
            $"PKG: extracted '{Path.GetFileName(pkgPath)}' via pkgextract to '{cache}'.", new() { File = pkgPath });
        return true;
    }

    private static string? FindPkgExtractTool()
    {
        if (Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PKGEXTRACT") is { Length: > 0 } envPath && File.Exists(envPath))
            return envPath;
        var names = OperatingSystem.IsWindows() ? new[] { "pkgextract.exe", "pkgextract" } : new[] { "pkgextract" };
        foreach (var n in names)
        {
            var local = Path.Combine(AppContext.BaseDirectory, n);
            if (File.Exists(local)) return local;
        }
        var pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        foreach (var dir in pathVar.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            foreach (var n in names)
            {
                var candidate = Path.Combine(dir.Trim(), n);
                if (File.Exists(candidate)) return candidate;
            }
        }
        return null;
    }

    private readonly record struct PfsInode(
        ushort Mode,
        uint Flags,
        ulong Size,
        uint BlockCount,
        int[] DirectBlocks,
        int[] IndirectBlocks);

    private sealed record DirItem(int Inode, int Type, string Name);

    internal static int ExtractPfsImage(Stream pfsStream, string cache, bool allowNested) =>
        ExtractPfsImage(new StreamImageReader(pfsStream), cache, allowNested);

    private static int ExtractPfsImage(IImageReader reader, string cache, bool allowNested)
    {
        if (reader.Length < 0x50)
            return 0;

        Span<byte> super = stackalloc byte[0x50];
        reader.ReadExactly(0, super);
        if (BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(0, 4)) == PfscMagic)
            return ExtractPfsImage(new PfscReader(reader), cache, allowNested);

        var magic = BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(8, 4));
        if (magic != PfsMagic)
            return 0;

        var mode = BinaryPrimitives.ReadUInt16LittleEndian(super.Slice(0x1C, 2));
        var isSigned = (mode & 0x1) != 0;
        var blockSize = BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(0x20, 4));
        var dinodeCount = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x30, 8));
        var dinodeBlockCount = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x40, 8));
        var superrootIno = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x48, 8));
        if (blockSize < 0x1000 || !BitOperations.IsPow2(blockSize) || dinodeBlockCount == 0 || dinodeCount == 0 || superrootIno >= dinodeCount || dinodeCount > 5_000_000)
            return 0;

        var inodeSize = isSigned ? DinodeS32Size : Dinode32Size;
        var entryStride = isSigned ? 36 : 4;
        var entryBlockOffset = isSigned ? 32 : 0;
        var ptrsPerBlock = (int)(blockSize / (uint)entryStride);

        // Per orbis-pfs/src/lib.rs:346-422: read and parse inodes block by block (inodes never cross block boundaries).
        var inodes = new List<PfsInode>((int)Math.Min(dinodeCount, 65536));
        var blockBuf = new byte[blockSize];
        for (ulong blkIdx = 0; blkIdx < dinodeBlockCount && (ulong)inodes.Count < dinodeCount; blkIdx++)
        {
            var blkOff = (long)blockSize + (long)blkIdx * blockSize;
            if (blkOff + blockSize > reader.Length)
                break;

            reader.ReadExactly(blkOff, blockBuf);
            for (var p = 0; p + inodeSize <= blockBuf.Length && (ulong)inodes.Count < dinodeCount; p += inodeSize)
            {
                var span = blockBuf.AsSpan(p, inodeSize);
                var iMode = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(0x00, 2));
                var iFlags = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(0x04, 4));
                var iSize = BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(0x08, 8));
                var iBlocks = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(0x60, 4));

                var direct = new int[12];
                for (var d = 0; d < 12; d++)
                    direct[d] = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(0x64 + d * entryStride + entryBlockOffset, 4));

                var indirect = new int[5];
                for (var k = 0; k < 5; k++)
                    indirect[k] = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(0x64 + (12 + k) * entryStride + entryBlockOffset, 4));

                inodes.Add(new PfsInode(iMode, iFlags, iSize, iBlocks, direct, indirect));
            }
        }

        if ((int)superrootIno >= inodes.Count)
            return 0;

        var fullPrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var extracted = 0;

        List<int> ResolveBlocks(int ino, out ulong fileSize)
        {
            fileSize = 0;
            if ((uint)ino >= (uint)inodes.Count) return [];
            var inode = inodes[ino];
            fileSize = inode.Size;
            if (fileSize == 0 || fileSize > 8UL * 1024 * 1024 * 1024 || inode.BlockCount == 0)
                return [];

            var blockCount = (int)inode.BlockCount;
            var result = new List<int>(Math.Min(blockCount, 65536));

            if (inode.DirectBlocks[1] == -1)
            {
                var start = inode.DirectBlocks[0];
                for (var i = 0; i < blockCount; i++)
                    result.Add(start + i);
                return result;
            }

            if (blockCount == 1)
            {
                result.Add(inode.DirectBlocks[0]);
                return result;
            }

            for (var d = 0; d < 12 && result.Count < blockCount; d++)
                result.Add(inode.DirectBlocks[d]);

            if (result.Count >= blockCount)
                return result;

            var indBuf1 = new byte[blockSize];
            var ib0Off = (long)(uint)inode.IndirectBlocks[0] * blockSize;
            if (ib0Off + blockSize <= reader.Length)
            {
                reader.ReadExactly(ib0Off, indBuf1);
                for (var i = 0; i < ptrsPerBlock && result.Count < blockCount; i++)
                    result.Add(BinaryPrimitives.ReadInt32LittleEndian(indBuf1.AsSpan(i * entryStride + entryBlockOffset, 4)));
            }

            if (result.Count >= blockCount)
                return result;

            var indBuf2 = new byte[blockSize];
            var ib1Off = (long)(uint)inode.IndirectBlocks[1] * blockSize;
            if (ib1Off + blockSize <= reader.Length)
            {
                reader.ReadExactly(ib1Off, indBuf1);
                for (var outer = 0; outer < ptrsPerBlock && result.Count < blockCount; outer++)
                {
                    var l1 = BinaryPrimitives.ReadUInt32LittleEndian(indBuf1.AsSpan(outer * entryStride + entryBlockOffset, 4));
                    var l1Off = (long)l1 * blockSize;
                    if (l1Off + blockSize > reader.Length) break;
                    reader.ReadExactly(l1Off, indBuf2);
                    for (var inner = 0; inner < ptrsPerBlock && result.Count < blockCount; inner++)
                        result.Add(BinaryPrimitives.ReadInt32LittleEndian(indBuf2.AsSpan(inner * entryStride + entryBlockOffset, 4)));
                }
            }

            return result;
        }

        IImageReader OpenInodeReader(int ino, List<int> blocks, ulong fileSize)
        {
            var contiguous = blocks.Count > 0;
            for (var i = 1; i < blocks.Count; i++)
            {
                if (blocks[i] != blocks[i - 1] + 1)
                {
                    contiguous = false;
                    break;
                }
            }

            return contiguous
                ? new RegionSubReader(reader, (long)(uint)blocks[0] * blockSize, (long)fileSize)
                : new BlockMappedReader(reader, blocks, blockSize, (long)fileSize);
        }

        byte[] ReadInodeBytes(int ino)
        {
            var blocks = ResolveBlocks(ino, out var fileSize);
            if (blocks.Count == 0 || fileSize == 0 || fileSize > int.MaxValue) return [];
            var res = new byte[(int)fileSize];
            var remaining = (long)fileSize;
            var dst = 0;
            foreach (var blk in blocks)
            {
                if (remaining <= 0) break;
                var toRead = (int)Math.Min(blockSize, remaining);
                var filePos = (long)(uint)blk * blockSize;
                if (filePos < 0 || filePos + toRead > reader.Length) break;
                reader.ReadExactly(filePos, res.AsSpan(dst, toRead));
                dst += toRead;
                remaining -= toRead;
            }
            return dst == (int)fileSize ? res : res[..dst];
        }

        void WriteInodeToFile(int ino, string target)
        {
            var blocks = ResolveBlocks(ino, out var fileSize);
            if (blocks.Count == 0 || fileSize == 0) return;
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            var buf = new byte[blockSize];
            var remaining = (long)fileSize;
            using var dst = File.Create(target);
            foreach (var blk in blocks)
            {
                if (remaining <= 0) break;
                var toRead = (int)Math.Min(blockSize, remaining);
                var filePos = (long)(uint)blk * blockSize;
                if (filePos < 0 || filePos + toRead > reader.Length) break;
                reader.ReadExactly(filePos, buf.AsSpan(0, toRead));
                dst.Write(buf, 0, toRead);
                remaining -= toRead;
            }
        }

        List<DirItem> ReadDirEntries(int dirIno)
        {
            var raw = ReadInodeBytes(dirIno);
            var items = new List<DirItem>();
            for (var p = 0; p + 16 <= raw.Length;)
            {
                var entryIno = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p, 4));
                var type = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p + 4, 4));
                var nameLen = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p + 8, 4));
                var entSize = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p + 12, 4));
                if (entSize == 0 || entSize < 16 || p + entSize > raw.Length) break;
                if (entryIno > 0 && nameLen > 0 && p + 16 + nameLen <= raw.Length)
                {
                    var name = Encoding.UTF8.GetString(raw.AsSpan(p + 16, nameLen)).TrimEnd('\0');
                    if (name is not "." and not ".." && type is 2 or 3)
                        items.Add(new DirItem(entryIno, type, name));
                }
                p += entSize;
            }
            return items;
        }

        void WalkDir(int dirIno, string relDir)
        {
            Span<byte> magicBuf = stackalloc byte[4];
            foreach (var item in ReadDirEntries(dirIno))
            {
                var relPath = relDir.Length == 0 ? item.Name : $"{relDir}/{item.Name}";
                if (item.Type == 3)
                {
                    WalkDir(item.Inode, relPath);
                }
                else if (item.Type == 2)
                {
                    if (allowNested && item.Name.Equals("pfs_image.dat", StringComparison.OrdinalIgnoreCase))
                    {
                        var blocks = ResolveBlocks(item.Inode, out var innerSize);
                        if (blocks.Count > 0 && innerSize > 0x400)
                        {
                            var innerFileReader = OpenInodeReader(item.Inode, blocks, innerSize);
                            var isCompressed = (inodes[item.Inode].Flags & 0x1) != 0;
                            if (!isCompressed && innerFileReader.Length >= 4)
                            {
                                innerFileReader.ReadExactly(0, magicBuf);
                                isCompressed = BinaryPrimitives.ReadUInt32LittleEndian(magicBuf) == PfscMagic;
                            }

                            IImageReader innerPfs = isCompressed ? new PfscReader(innerFileReader) : innerFileReader;
                            extracted += ExtractPfsImage(innerPfs, cache, allowNested: false);
                        }
                    }
                    else if (WantedNames.Contains(item.Name))
                    {
                        var target = Path.GetFullPath(Path.Combine(cache, relPath.Replace('/', Path.DirectorySeparatorChar)));
                        if (target.StartsWith(fullPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            WriteInodeToFile(item.Inode, target);
                            extracted++;
                        }
                    }
                }
            }
        }

        // Unwrap top-level `uroot` directory per orbis-pkg-util when present (while still handling flat synthetic roots).
        var rootIno = (int)superrootIno;
        var rootEntries = ReadDirEntries(rootIno);
        var uroot = rootEntries.FirstOrDefault(e => e.Type == 3 && e.Name.Equals("uroot", StringComparison.OrdinalIgnoreCase));
        if (uroot is not null)
            rootIno = uroot.Inode;

        WalkDir(rootIno, string.Empty);
        return extracted;
    }

    private static void ReadExactAt(SafeFileHandle handle, long offset, Span<byte> buffer)
    {
        var total = 0;
        while (total < buffer.Length)
        {
            var n = RandomAccess.Read(handle, buffer[total..], offset + total);
            if (n == 0)
                throw new EndOfStreamException($"Unexpected EOF reading at offset 0x{offset + total:X}.");
            total += n;
        }
    }

    private static bool HasMetadata(string dir) =>
        Directory.Exists(dir) && Directory.EnumerateFiles(dir, "global-metadata.dat", SearchOption.AllDirectories).Any();

    private interface IImageReader
    {
        long Length { get; }
        void ReadExactly(long offset, Span<byte> destination);
    }

    private sealed class SafeHandleRegionReader(SafeFileHandle handle, long baseOffset, long length) : IImageReader
    {
        public long Length => length;

        public void ReadExactly(long offset, Span<byte> destination)
        {
            if (offset < 0 || offset + destination.Length > length)
                throw new EndOfStreamException($"Out-of-bounds read at 0x{offset:X} (len=0x{destination.Length:X}).");
            ReadExactAt(handle, baseOffset + offset, destination);
        }
    }

    private sealed class StreamImageReader(Stream stream) : IImageReader
    {
        public long Length => stream.Length;

        public void ReadExactly(long offset, Span<byte> destination)
        {
            stream.Position = offset;
            stream.ReadExactly(destination);
        }
    }

    private sealed class RegionSubReader(IImageReader inner, long baseOffset, long length) : IImageReader
    {
        public long Length => Math.Min(length, Math.Max(0, inner.Length - baseOffset));

        public void ReadExactly(long offset, Span<byte> destination)
        {
            if (offset < 0 || offset + destination.Length > Length)
                throw new EndOfStreamException();
            inner.ReadExactly(baseOffset + offset, destination);
        }
    }

    private sealed class BlockMappedReader(IImageReader inner, List<int> blocks, uint blockSize, long length) : IImageReader
    {
        public long Length => length;

        public void ReadExactly(long offset, Span<byte> destination)
        {
            if (offset < 0 || offset + destination.Length > length)
                throw new EndOfStreamException();

            var pos = offset;
            var written = 0;
            while (written < destination.Length)
            {
                var bi = (int)(pos / blockSize);
                if ((uint)bi >= (uint)blocks.Count)
                    throw new EndOfStreamException();
                var inBlk = (int)(pos % blockSize);
                var toRead = Math.Min(destination.Length - written, (int)blockSize - inBlk);
                inner.ReadExactly((long)(uint)blocks[bi] * blockSize + inBlk, destination.Slice(written, toRead));
                pos += toRead;
                written += toRead;
            }
        }
    }

    /// <summary>Random-access zlib block reader for PlayStation <c>PFSC</c> compressed filesystem images.</summary>
    private sealed class PfscReader : IImageReader
    {
        private readonly IImageReader _inner;
        private readonly uint _blockSize;
        private readonly ulong _originalBlockSize;
        private readonly ulong[] _offsets;
        private readonly long _length;
        private readonly byte[] _cache;
        private int _cachedBlock = -1;

        public PfscReader(IImageReader inner)
        {
            _inner = inner;
            Span<byte> hdr = stackalloc byte[0x30];
            _inner.ReadExactly(0, hdr);
            if (BinaryPrimitives.ReadUInt32LittleEndian(hdr) != PfscMagic)
                throw new InvalidDataException("Missing PFSC header magic.");

            _blockSize = BinaryPrimitives.ReadUInt32LittleEndian(hdr.Slice(0x0C, 4));
            var blockSize2 = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x10, 8));
            var blockOffsetsStart = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x18, 8));
            var dataLength = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x28, 8));
            _originalBlockSize = blockSize2 != 0 ? blockSize2 : _blockSize;
            if (_blockSize < 0x1000 || _blockSize > 0x400000 || dataLength == 0 || dataLength > 64UL * 1024 * 1024 * 1024)
                throw new InvalidDataException($"Unsupported PFSC geometry (blockSize=0x{_blockSize:X}, dataLength=0x{dataLength:X}).");

            // Match orbis-pfs/src/pfsc.rs:119 (also ensure sentinel offset entry exists for synthetic images).
            var blockCount = (long)((dataLength + _blockSize - 1) / _blockSize);
            if (blockCount > 4_000_000)
                throw new InvalidDataException($"PFSC block count {blockCount} exceeds safety limit.");

            _offsets = new ulong[blockCount + 1];
            var rawOff = new byte[(blockCount + 1) * 8];
            _inner.ReadExactly((long)blockOffsetsStart, rawOff);
            for (var i = 0; i <= blockCount; i++)
                _offsets[i] = BinaryPrimitives.ReadUInt64LittleEndian(rawOff.AsSpan(i * 8, 8));

            _length = (long)dataLength;
            _cache = new byte[_blockSize];
        }

        public long Length => _length;

        public void ReadExactly(long offset, Span<byte> destination)
        {
            if (offset < 0 || offset + destination.Length > _length)
                throw new EndOfStreamException();

            var pos = offset;
            var written = 0;
            while (written < destination.Length)
            {
                var bi = (int)(pos / _blockSize);
                if (bi < 0 || bi >= _offsets.Length - 1)
                    throw new EndOfStreamException();
                EnsureBlock(bi);
                var inBlk = (int)(pos % _blockSize);
                var avail = Math.Min(destination.Length - written, (int)_blockSize - inBlk);
                _cache.AsSpan(inBlk, avail).CopyTo(destination.Slice(written, avail));
                pos += avail;
                written += avail;
            }
        }

        private void EnsureBlock(int blockIndex)
        {
            if (_cachedBlock == blockIndex) return;
            var start = _offsets[blockIndex];
            var end = _offsets[blockIndex + 1];
            if (end < start)
            {
                Array.Clear(_cache);
                _cachedBlock = blockIndex;
                return;
            }

            var size = end - start;
            if (size == _originalBlockSize && size == _blockSize)
            {
                _inner.ReadExactly((long)start, _cache);
            }
            else if (size < _originalBlockSize && size <= _blockSize * 2UL)
            {
                var comp = new byte[(int)size];
                _inner.ReadExactly((long)start, comp);
                using var ms = new MemoryStream(comp);
                using var zlib = new ZLibStream(ms, CompressionMode.Decompress);
                Array.Clear(_cache);
                var read = 0;
                while (read < _cache.Length)
                {
                    var n = zlib.Read(_cache, read, _cache.Length - read);
                    if (n <= 0) break;
                    read += n;
                }
            }
            else
            {
                Array.Clear(_cache);
            }
            _cachedBlock = blockIndex;
        }
    }

    /// <summary>Positional AES-128-XTS decryptor for encrypted PS4 PFS images (<c>orbis-pfs/src/image.rs</c>).</summary>
    private sealed class PfsXtsReader : IImageReader, IDisposable
    {
        private const int SectorSize = 0x1000;
        private readonly IImageReader _inner;
        private readonly uint _blockSize;
        private readonly Aes _aesTweak;
        private readonly Aes _aesData;
        private readonly byte[] _sectorCache = new byte[SectorSize];
        private long _cachedSector = -1;

        public PfsXtsReader(IImageReader inner, uint blockSize, byte[] ekpfs, ReadOnlySpan<byte> cryptSeed)
        {
            _inner = inner;
            _blockSize = blockSize;

            Span<byte> msg = stackalloc byte[20];
            BinaryPrimitives.WriteUInt32LittleEndian(msg, 1);
            cryptSeed[..16].CopyTo(msg[4..]);
            Span<byte> derived = stackalloc byte[32];
            HMACSHA256.HashData(ekpfs, msg, derived);

            _aesTweak = Aes.Create();
            _aesTweak.Mode = CipherMode.ECB;
            _aesTweak.Padding = PaddingMode.None;
            _aesTweak.Key = derived[..16].ToArray();

            _aesData = Aes.Create();
            _aesData.Mode = CipherMode.ECB;
            _aesData.Padding = PaddingMode.None;
            _aesData.Key = derived.Slice(16, 16).ToArray();
        }

        public long Length => _inner.Length;

        public void ReadExactly(long offset, Span<byte> destination)
        {
            if (offset < 0 || offset + destination.Length > Length)
                throw new EndOfStreamException();

            var pos = offset;
            var written = 0;
            while (written < destination.Length)
            {
                if (pos < _blockSize)
                {
                    var plainLen = (int)Math.Min(destination.Length - written, _blockSize - pos);
                    _inner.ReadExactly(pos, destination.Slice(written, plainLen));
                    pos += plainLen;
                    written += plainLen;
                    continue;
                }

                var sector = pos / SectorSize;
                EnsureSector(sector);
                var inSec = (int)(pos % SectorSize);
                var avail = Math.Min(destination.Length - written, SectorSize - inSec);
                _sectorCache.AsSpan(inSec, avail).CopyTo(destination.Slice(written, avail));
                pos += avail;
                written += avail;
            }
        }

        private void EnsureSector(long sectorIndex)
        {
            if (_cachedSector == sectorIndex) return;
            var offset = sectorIndex * SectorSize;
            var avail = (int)Math.Min(SectorSize, Math.Max(0, _inner.Length - offset));
            if (avail < SectorSize) Array.Clear(_sectorCache);
            if (avail > 0) _inner.ReadExactly(offset, _sectorCache.AsSpan(0, avail));

            DecryptSector(_sectorCache, sectorIndex);
            _cachedSector = sectorIndex;
        }

        private void DecryptSector(Span<byte> buf, long sectorIndex)
        {
            Span<byte> tweakInput = stackalloc byte[16];
            tweakInput.Clear();
            BinaryPrimitives.WriteUInt64LittleEndian(tweakInput, (ulong)sectorIndex);

            Span<byte> tweak = stackalloc byte[16];
            _aesTweak.EncryptEcb(tweakInput, tweak, PaddingMode.None);

            Span<byte> temp = stackalloc byte[16];
            for (var off = 0; off < SectorSize; off += 16)
            {
                var slice = buf.Slice(off, 16);
                for (var i = 0; i < 16; i++) slice[i] ^= tweak[i];
                _aesData.DecryptEcb(slice, temp, PaddingMode.None);
                for (var i = 0; i < 16; i++) slice[i] = (byte)(temp[i] ^ tweak[i]);

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

        public void Dispose()
        {
            _aesTweak.Dispose();
            _aesData.Dispose();
        }
    }
}
