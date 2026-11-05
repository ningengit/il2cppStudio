using System.Buffers.Binary;

namespace Il2CppStudio.Core.Binary;

/// <summary>One @comp.id entry of the MSVC "Rich" header: which tool built how many objects.</summary>
public sealed record RichEntry(ushort ProductId, ushort BuildNumber, uint Count)
{
    public string ProductName => RichHeader.DescribeProduct(ProductId);
    public string ToolsetGuess => RichHeader.DescribeBuild(BuildNumber);
    public bool IsLtcg => ProductId is 0x0108 or 0x0109 or 0x010A;
    public bool IsPgo => ProductId is >= 0x010B and <= 0x010E;
}

public sealed record RichHeader(uint XorKey, bool ChecksumValid, uint ComputedChecksum, long FileOffset, IReadOnlyList<RichEntry> Entries)
{
    /// <summary>
    /// Parses the undocumented MSVC linker "Rich" header between the DOS stub and the PE header.
    /// Layout: "DanS" ^ key, 3x padding(key), {compid ^ key, count ^ key}*, "Rich", key.
    /// Returns null if the binary was not linked by MSVC link.exe (e.g. lld-link without /richheader).
    /// </summary>
    public static RichHeader? TryParse(ReadOnlySpan<byte> file)
    {
        if (file.Length < 0x40) return null;
        var peOffset = BinaryPrimitives.ReadInt32LittleEndian(file[0x3C..]);
        if (peOffset <= 0x80 || peOffset > file.Length) return null;

        var stub = file[..peOffset];
        // Find "Rich" marker scanning backwards from the PE header.
        int richPos = -1;
        for (var i = (peOffset & ~3) - 8; i >= 0x80; i -= 4)
        {
            if (BinaryPrimitives.ReadUInt32LittleEndian(stub[i..]) == 0x68636952) { richPos = i; break; } // "Rich"
        }
        if (richPos < 0) return null;

        var key = BinaryPrimitives.ReadUInt32LittleEndian(stub[(richPos + 4)..]);
        int dansPos = -1;
        for (var i = richPos - 4; i >= 0x40; i -= 4)
        {
            if ((BinaryPrimitives.ReadUInt32LittleEndian(stub[i..]) ^ key) == 0x536E6144) { dansPos = i; break; } // "DanS"
        }
        if (dansPos < 0) return null;

        var entries = new List<RichEntry>();
        for (var i = dansPos + 16; i + 8 <= richPos; i += 8)
        {
            var compId = BinaryPrimitives.ReadUInt32LittleEndian(stub[i..]) ^ key;
            var count = BinaryPrimitives.ReadUInt32LittleEndian(stub[(i + 4)..]) ^ key;
            entries.Add(new RichEntry((ushort)(compId >> 16), (ushort)(compId & 0xFFFF), count));
        }

        // Checksum per the well-known algorithm: offset + rol(DOS header bytes excl. e_lfanew) + rol(compid, count).
        uint checksum = (uint)dansPos;
        for (var i = 0; i < dansPos; i++)
        {
            if (i is >= 0x3C and < 0x40) continue;
            checksum += uint.RotateLeft(file[i], i & 31);
        }
        foreach (var e in entries)
        {
            var compId = ((uint)e.ProductId << 16) | e.BuildNumber;
            checksum += uint.RotateLeft(compId, (int)(e.Count & 31));
        }

        return new RichHeader(key, checksum == key, checksum, dansPos, entries);
    }

    /// <summary>Best-effort product names for VS2015+ (VC 14.x) tool IDs. Raw IDs are always reported too.</summary>
    public static string DescribeProduct(ushort id) => id switch
    {
        0x0000 => "Unmarked objects",
        0x0001 => "Import (DLL imports)",
        0x00FD => "AliasObj (VS2015+)",
        0x00FE => "Cvtpgd (VS2015+)",
        0x00FF => "Cvtres (resources, VS2015+)",
        0x0100 => "Export (VS2015+)",
        0x0101 => "Implib (VS2015+)",
        0x0102 => "Linker (VS2015+)",
        0x0103 => "MASM (VS2015+)",
        0x0104 => "C compiler (VS2015+)",
        0x0105 => "C++ compiler (VS2015+)",
        0x0106 => "C compiler /clr (VS2015+)",
        0x0107 => "C++ compiler /clr (VS2015+)",
        0x0108 => "C compiler LTCG /GL (VS2015+)",
        0x0109 => "C++ compiler LTCG /GL (VS2015+)",
        0x010A => "MSIL LTCG (VS2015+)",
        0x010B => "C compiler PGO instrument (VS2015+)",
        0x010C => "C++ compiler PGO instrument (VS2015+)",
        0x010D => "C compiler PGO optimize (VS2015+)",
        0x010E => "C++ compiler PGO optimize (VS2015+)",
        _ when id < 0x00FD => $"Pre-VS2015 tool (0x{id:X4})",
        _ => $"Unknown tool (0x{id:X4})",
    };

    /// <summary>Approximate mapping of MSVC build numbers to Visual Studio releases.</summary>
    public static string DescribeBuild(ushort build) => build switch
    {
        0 => "n/a",
        < 23026 => "pre-VS2015",
        < 25017 => "VS2015 (MSVC 14.0)",
        < 27508 => "VS2017 (MSVC 14.1x)",
        < 30133 => "VS2019 (MSVC 14.2x)",
        < 36000 => "VS2022 (MSVC 14.3x/14.4x)",
        _ => "VS2026+ (MSVC 14.5x+)",
    };
}
