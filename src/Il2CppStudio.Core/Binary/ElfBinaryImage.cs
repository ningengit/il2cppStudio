using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// ELF32/ELF64 little-endian shared object (Linux GameAssembly.so, Android libil2cpp.so). Allocated sections are used
/// when the section table exists (finer data/code split), PT_LOAD segments otherwise. The image base is 0 (RVA = vaddr).
/// The file is loaded into memory and dynamic RELATIVE / ABS relocations are applied (<see cref="ElfRelocator"/>) so
/// pointer tables such as Il2CppCodeRegistration read exactly as they do at runtime with a load base of 0.
/// </summary>
public sealed class ElfBinaryImage : IBinaryImage
{
    private readonly byte[] _data;
    private readonly List<(string Name, ulong Rva)> _symbols = [];

    public string Path { get; }
    public BinaryFormat Format => BinaryFormat.Elf;
    public CpuArchitecture Architecture { get; }
    public bool Is64Bit { get; }
    public ulong ImageBase => 0;
    public IReadOnlyList<BinarySection> Sections { get; }
    public ReadOnlyMemory<byte> Data => _data;
    internal IReadOnlyList<(ulong VAddr, ulong FileSize, long Offset, uint Flags)> Segments { get; }
    public int AppliedRelocations { get; }

    private ElfBinaryImage(string path, byte[] data, IDiagnosticSink sink)
    {
        Path = path; _data = data;
        Is64Bit = data[4] == 2;
        Architecture = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(18)) switch
        {
            0x3E => CpuArchitecture.X64, 0xB7 => CpuArchitecture.Arm64, 0x28 => CpuArchitecture.Arm, 0x03 => CpuArchitecture.X86, _ => CpuArchitecture.Unknown,
        };
        Segments = ReadSegments();
        Sections = ReadSections() is { Count: > 0 } secs ? secs
            : [.. Segments.Select((s, i) => new BinarySection($"LOAD{i}", s.VAddr, s.FileSize, s.Offset, (long)s.FileSize, (s.Flags & 1) != 0, (s.Flags & 2) != 0, true))];
        var reloc = new ElfRelocator(this, _data);
        AppliedRelocations = reloc.Apply(sink);
        _symbols.AddRange(reloc.DynamicFunctionSymbols());
    }

    public static ElfBinaryImage Load(string path, IDiagnosticSink sink)
    {
        var data = File.ReadAllBytes(path);
        if (data.Length < 0x40 || data[0] != 0x7F || data[1] != (byte)'E' || data[2] != (byte)'L' || data[3] != (byte)'F' || data[5] != 1)
            throw new InvalidDataException("Not a little-endian ELF file.");
        var img = new ElfBinaryImage(path, data, sink);
        sink.Info(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatDetected,
            $"ELF{(img.Is64Bit ? 64 : 32)} {img.Architecture}, {img.Sections.Count} sections, {img.AppliedRelocations:N0} relocations applied, {img._symbols.Count:N0} exported functions.",
            new() { File = path });
        return img;
    }

    public static bool IsElf(string path)
    {
        using var f = File.OpenRead(path);
        Span<byte> m = stackalloc byte[4];
        return f.Read(m) == 4 && m[0] == 0x7F && m[1] == (byte)'E' && m[2] == (byte)'L' && m[3] == (byte)'F';
    }

    public IReadOnlyList<(string Name, ulong Rva)> GetSymbols() => _symbols;

    public bool TryRvaToFileOffset(ulong rva, out long fileOffset) => SectionMath.TryRvaToOffset(Sections, rva, out fileOffset);
    public bool TryFileOffsetToRva(long fileOffset, out ulong rva) => SectionMath.TryOffsetToRva(Sections, fileOffset, out rva);

    internal ulong Word(long offset) => Is64Bit ? BinaryPrimitives.ReadUInt64LittleEndian(_data.AsSpan((int)offset)) : BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)offset));

    private List<(ulong, ulong, long, uint)> ReadSegments()
    {
        var phoff = (long)Word(Is64Bit ? 0x20 : 0x1C);
        int entSize = BinaryPrimitives.ReadUInt16LittleEndian(_data.AsSpan(Is64Bit ? 0x36 : 0x2A));
        int count = BinaryPrimitives.ReadUInt16LittleEndian(_data.AsSpan(Is64Bit ? 0x38 : 0x2C));
        var list = new List<(ulong, ulong, long, uint)>();
        for (var i = 0; i < count; i++)
        {
            var p = phoff + (long)i * entSize;
            var type = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)p));
            uint flags = Is64Bit ? BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)p + 4)) : BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)p + 24));
            long off = Is64Bit ? (long)Word(p + 8) : (long)Word(p + 4);
            ulong vaddr = Is64Bit ? Word(p + 16) : Word(p + 8);
            ulong filesz = Is64Bit ? Word(p + 32) : Word(p + 16);
            // PT_LOAD = 1, PT_DYNAMIC = 2
            if (type == 1) list.Add((vaddr, filesz, off, flags));
            else if (type == 2) Dynamic = (off, filesz);
        }
        return list;
    }

    /// <summary>File offset and size of the PT_DYNAMIC segment, if any.</summary>
    internal (long Offset, ulong Size)? Dynamic { get; private set; }

    private List<BinarySection> ReadSections()
    {
        var shoff = (long)Word(Is64Bit ? 0x28 : 0x20);
        int entSize = BinaryPrimitives.ReadUInt16LittleEndian(_data.AsSpan(Is64Bit ? 0x3A : 0x2E));
        int count = BinaryPrimitives.ReadUInt16LittleEndian(_data.AsSpan(Is64Bit ? 0x3C : 0x30));
        int strIdx = BinaryPrimitives.ReadUInt16LittleEndian(_data.AsSpan(Is64Bit ? 0x3E : 0x32));
        var result = new List<BinarySection>();
        if (shoff <= 0 || count == 0 || shoff + (long)count * entSize > _data.Length || strIdx >= count) return result;
        long Field(int i, int off64, int off32) => shoff + (long)i * entSize + (Is64Bit ? off64 : off32);
        var strOff = (long)Word(Field(strIdx, 0x18, 0x10));
        for (var i = 0; i < count; i++)
        {
            var type = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)Field(i, 4, 4)));
            var flags = Word(Field(i, 8, 8));
            if ((flags & 2) == 0) continue;   // SHF_ALLOC
            var addr = Word(Field(i, 0x10, 0x0C));
            var off = (long)Word(Field(i, 0x18, 0x10));
            var size = Word(Field(i, 0x20, 0x14));
            var nameOff = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)Field(i, 0, 0)));
            var nameStart = strOff + nameOff;
            var nameEnd = nameStart >= 0 && nameStart < _data.Length ? Array.IndexOf(_data, (byte)0, (int)nameStart) : -1;
            var name = nameEnd > nameStart ? Encoding.ASCII.GetString(_data, (int)nameStart, nameEnd - (int)nameStart) : $"sec{i}";
            var raw = type == 8 ? 0 : (long)size;   // SHT_NOBITS (.bss) has no file data
            result.Add(new BinarySection(name, addr, size, off, raw, (flags & 4) != 0, (flags & 1) != 0, true));
        }
        return result;
    }

    public void Dispose() { }
}
