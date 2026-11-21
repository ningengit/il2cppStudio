using System.Buffers.Binary;
using System.Security.Cryptography;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Bring-your-own-keys (BYOK) parser and HAC key derivation helper for user-supplied Nintendo Switch
/// <c>prod.keys</c> / <c>dev.keys</c> and <c>title.keys</c> files. Contains ZERO embedded proprietary keys.
/// </summary>
public sealed class SwitchKeySet
{
    private readonly Dictionary<int, byte[]> _masterKeys = new();
    private readonly Dictionary<(int Slot, int Gen), byte[]> _keyAreaKeys = new();
    private readonly Dictionary<int, byte[]> _titleKeks = new();
    private readonly Dictionary<string, byte[]> _titleKeys = new(StringComparer.OrdinalIgnoreCase);

    private byte[]? _headerKey;
    private byte[]? _headerKekSource;
    private byte[]? _headerKeySource;
    private byte[]? _aesKekGenSource;
    private byte[]? _aesKeyGenSource;
    private byte[]? _titleKekSource;
    private readonly byte[]?[] _keyAreaSources = new byte[3][];

    public bool HasHeaderKey => _headerKey is { Length: 32 };
    public ReadOnlySpan<byte> HeaderKey => _headerKey;

    public static string? ResolveDefaultPath(string? explicitPath)
    {
        if (!string.IsNullOrWhiteSpace(explicitPath) && File.Exists(explicitPath))
            return Path.GetFullPath(explicitPath);

        var envPath = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_SWITCH_KEYS");
        if (!string.IsNullOrWhiteSpace(envPath) && File.Exists(envPath))
            return Path.GetFullPath(envPath);

        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (!string.IsNullOrEmpty(home))
        {
            var prodKeys = Path.Combine(home, ".switch", "prod.keys");
            if (File.Exists(prodKeys))
                return prodKeys;
        }

        return null;
    }

    public static SwitchKeySet LoadFromFile(string keysetPath)
    {
        var set = new SwitchKeySet();
        set.ParseKeyLines(File.ReadAllLines(keysetPath), isTitleKeysFile: false);

        var dir = Path.GetDirectoryName(keysetPath);
        if (!string.IsNullOrEmpty(dir))
        {
            var siblingTitleKeys = Path.Combine(dir, "title.keys");
            if (File.Exists(siblingTitleKeys))
                set.ParseKeyLines(File.ReadAllLines(siblingTitleKeys), isTitleKeysFile: true);
        }

        set.DeriveMissingKeys();
        return set;
    }

    public void ImportTitleKeysFile(string titleKeysPath)
    {
        if (File.Exists(titleKeysPath))
            ParseKeyLines(File.ReadAllLines(titleKeysPath), isTitleKeysFile: true);
    }

    public void ImportTicket(ReadOnlySpan<byte> tik)
    {
        if (tik.Length < 4) return;
        // HAC ES tickets store the signature type little-endian (e.g. 04 00 01 00 = 0x00010004, RSA-2048/SHA-256),
        // matching nstool's es::ESignatureType parsing; accept big-endian too for robustness.
        var sigBlockSize = SignatureBlockSize(BinaryPrimitives.ReadUInt32LittleEndian(tik[..4]));
        if (sigBlockSize == 0)
            sigBlockSize = SignatureBlockSize(BinaryPrimitives.ReadUInt32BigEndian(tik[..4]));
        if (sigBlockSize == 0 || tik.Length < sigBlockSize + 0x170)
            return;

        var body = tik[sigBlockSize..];
        if (body[0x141] != 0) // 0 = Common AES-128-CBC encrypted title key; 1 = Personalized RSA-2048
            return;

        var encTitleKey = body.Slice(0x40, 16);
        var rawRev = body[0x145];
        var rightsId = body.Slice(0x160, 16);

        // Work around fake-signed scene tickets where common_key_id is left 0 but rightsId[15] encodes keygen >= 3.
        if (rawRev == 0 && tik.Length >= 12 && BinaryPrimitives.ReadUInt64LittleEndian(tik.Slice(4, 8)) == ulong.MaxValue && rightsId[15] >= 3)
            rawRev = rightsId[15];

        var keyGen = rawRev > 0 ? rawRev - 1 : 0;
        if (!_titleKeks.TryGetValue(keyGen, out var titleKek))
            return;

        var decrypted = new byte[16];
        AesEcbDecrypt(titleKek, encTitleKey, decrypted);
        // Explicit title.keys entries (loaded next to prod.keys) take precedence; the embedded ticket is the fallback.
        _titleKeys.TryAdd(Convert.ToHexStringLower(rightsId), decrypted);
    }

    /// <summary>Signature type + signature + padding (0x40-aligned) preceding the ticket body.</summary>
    private static int SignatureBlockSize(uint sigType) => sigType switch
    {
        0x00010000 or 0x00010003 => 0x240,   // RSA-4096
        0x00010001 or 0x00010004 => 0x140,   // RSA-2048
        0x00010002 or 0x00010005 => 0x80,    // ECDSA-240
        _ => 0,
    };

    public int TitleKeyCount => _titleKeys.Count;

    public bool TryGetKeyAreaKey(int slot, int keyGen, out byte[] key)
    {
        if (_keyAreaKeys.TryGetValue((slot, keyGen), out key!))
            return true;
        key = [];
        return false;
    }

    public bool TryGetTitleKey(ReadOnlySpan<byte> rightsId, out byte[] key)
    {
        if (_titleKeys.TryGetValue(Convert.ToHexStringLower(rightsId), out key!))
            return true;
        key = [];
        return false;
    }

    private void ParseKeyLines(IEnumerable<string> lines, bool isTitleKeysFile)
    {
        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line[0] is '#' or ';')
                continue;

            var commentIdx = line.IndexOfAny(['#', ';']);
            if (commentIdx >= 0)
                line = line[..commentIdx].Trim();

            var eq = line.IndexOf('=');
            if (eq <= 0) continue;

            var name = line[..eq].Trim().ToLowerInvariant();
            var hex = line[(eq + 1)..].Trim().Replace(" ", string.Empty).Replace("-", string.Empty);
            if (!TryDecodeHex(hex, out var value))
                continue;

            if (isTitleKeysFile || (name.Length == 32 && IsHexString(name)))
            {
                if (name.Length == 32 && value.Length == 16)
                    _titleKeys[name] = value;
                continue;
            }

            switch (name)
            {
                case "header_key" when value.Length == 32:
                    _headerKey = value;
                    break;
                case "header_kek_source" when value.Length == 16:
                    _headerKekSource = value;
                    break;
                case "header_key_source" when value.Length == 32:
                    _headerKeySource = value;
                    break;
                case "aes_kek_generation_source" when value.Length == 16:
                    _aesKekGenSource = value;
                    break;
                case "aes_key_generation_source" when value.Length == 16:
                    _aesKeyGenSource = value;
                    break;
                case "titlekek_source" when value.Length == 16:
                    _titleKekSource = value;
                    break;
                case "key_area_key_application_source" when value.Length == 16:
                    _keyAreaSources[0] = value;
                    break;
                case "key_area_key_ocean_source" when value.Length == 16:
                    _keyAreaSources[1] = value;
                    break;
                case "key_area_key_system_source" when value.Length == 16:
                    _keyAreaSources[2] = value;
                    break;
                default:
                    ParseIndexedKey(name, value);
                    break;
            }
        }
    }

    private void ParseIndexedKey(string name, byte[] value)
    {
        if (value.Length != 16) return;

        if (TryParseSuffixHex(name, "master_key_", out var gen))
            _masterKeys[gen] = value;
        else if (TryParseSuffixHex(name, "titlekek_", out gen))
            _titleKeks[gen] = value;
        else if (TryParseSuffixHex(name, "key_area_key_application_", out gen))
            _keyAreaKeys[(0, gen)] = value;
        else if (TryParseSuffixHex(name, "key_area_key_ocean_", out gen))
            _keyAreaKeys[(1, gen)] = value;
        else if (TryParseSuffixHex(name, "key_area_key_system_", out gen))
            _keyAreaKeys[(2, gen)] = value;
    }

    private void DeriveMissingKeys()
    {
        if (_headerKey is null
            && _masterKeys.TryGetValue(0, out var mk0)
            && _aesKekGenSource is not null
            && _aesKeyGenSource is not null
            && _headerKekSource is not null
            && _headerKeySource is { Length: 32 })
        {
            var headerKek = new byte[16];
            GenerateKey3Step(headerKek, _aesKekGenSource, _headerKekSource, _aesKeyGenSource, mk0);
            _headerKey = new byte[32];
            AesEcbDecrypt(headerKek, _headerKeySource.AsSpan(0, 16), _headerKey.AsSpan(0, 16));
            AesEcbDecrypt(headerKek, _headerKeySource.AsSpan(16, 16), _headerKey.AsSpan(16, 16));
        }

        foreach (var (gen, mk) in _masterKeys)
        {
            if (_titleKekSource is not null && !_titleKeks.ContainsKey(gen))
            {
                var tk = new byte[16];
                AesEcbDecrypt(mk, _titleKekSource, tk);
                _titleKeks[gen] = tk;
            }

            if (_aesKekGenSource is not null && _aesKeyGenSource is not null)
            {
                for (var slot = 0; slot < _keyAreaSources.Length; slot++)
                {
                    var src = _keyAreaSources[slot];
                    if (src is not null && !_keyAreaKeys.ContainsKey((slot, gen)))
                    {
                        var kak = new byte[16];
                        GenerateKey3Step(kak, _aesKekGenSource, src, _aesKeyGenSource, mk);
                        _keyAreaKeys[(slot, gen)] = kak;
                    }
                }
            }
        }
    }

    private static void GenerateKey3Step(Span<byte> dst, ReadOnlySpan<byte> kekSrc, ReadOnlySpan<byte> midSrc, ReadOnlySpan<byte> keySrc, ReadOnlySpan<byte> masterKey)
    {
        Span<byte> s1 = stackalloc byte[16];
        Span<byte> s2 = stackalloc byte[16];
        AesEcbDecrypt(masterKey, kekSrc, s1);
        AesEcbDecrypt(s1, midSrc, s2);
        AesEcbDecrypt(s2, keySrc, dst);
    }

    internal static void AesEcbDecrypt(ReadOnlySpan<byte> key, ReadOnlySpan<byte> src, Span<byte> dst)
    {
        using var aes = Aes.Create();
        aes.Key = key.ToArray();
        aes.DecryptEcb(src, dst, PaddingMode.None);
    }

    private static bool TryParseSuffixHex(string name, string prefix, out int index)
    {
        index = 0;
        if (!name.StartsWith(prefix, StringComparison.Ordinal) || name.Length != prefix.Length + 2)
            return false;
        return int.TryParse(name.AsSpan(prefix.Length), System.Globalization.NumberStyles.HexNumber, null, out index);
    }

    private static bool TryDecodeHex(string hex, out byte[] bytes)
    {
        bytes = [];
        if (hex.Length == 0 || (hex.Length & 1) != 0)
            return false;
        try
        {
            bytes = Convert.FromHexString(hex);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static bool IsHexString(string s) =>
        s.All(Uri.IsHexDigit);
}
