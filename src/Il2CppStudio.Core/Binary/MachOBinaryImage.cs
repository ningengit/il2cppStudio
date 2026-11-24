using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// Mach-O 32/64-bit and Universal (Fat) binary image for iOS (<c>UnityFramework</c>, app executables) and
/// macOS (<c>GameAssembly.dylib</c>, app bundles). Resolves <c>LC_SEGMENT[_64]</c> sections, applies
/// <c>LC_DYLD_CHAINED_FIXUPS</c> pointer rebases in memory, and exposes <c>LC_FUNCTION_STARTS</c> and exports.
/// </summary>
public sealed class MachOBinaryImage : IBinaryImage
{
    private const uint Magic32 = 0xFEEDFACE;
    private const uint Magic64 = 0xFEEDFACF;
    private const uint FatMagicLe = 0xBEBAFECA;   // 0xCAFEBABE big-endian on disk
    private const uint FatMagic64Le = 0xBFBAFECA; // 0xCAFEBABF big-endian on disk

    private const uint CpuTypeX86 = 0x00000007;
    private const uint CpuTypeArm = 0x0000000C;
    private const uint CpuTypeX86_64 = 0x01000007;
    private const uint CpuTypeArm64 = 0x0100000C;

    private const uint LcSegment = 0x1;
    private const uint LcSymtab = 0x2;
    private const uint LcSegment64 = 0x19;
    private const uint LcEncryptionInfo = 0x21;
    private const uint LcDyldInfo = 0x22;
    private const uint LcDyldInfoOnly = 0x80000022;
    private const uint LcFunctionStarts = 0x26;
    private const uint LcEncryptionInfo64 = 0x2C;
    private const uint LcDyldExportsTrie = 0x80000033;
    private const uint LcDyldChainedFixups = 0x80000034;

    private readonly byte[] _data;
    private readonly List<BinarySection> _sections = [];
    private readonly List<(string Name, ulong Rva)> _symbols = [];
    private readonly (uint Begin, uint End)[] _functions = [];

    public string Path { get; }
    public BinaryFormat Format => BinaryFormat.MachO;
    public CpuArchitecture Architecture { get; }
    public bool Is64Bit { get; }
    public ulong ImageBase { get; private set; }
    public IReadOnlyList<BinarySection> Sections => _sections;
    public ReadOnlyMemory<byte> Data => _data;
    public int AppliedFixups { get; private set; }
    public bool IsEncrypted { get; private set; }

    private MachOBinaryImage(string path, byte[] raw, IDiagnosticSink sink)
    {
        Path = path;
        _data = SelectSliceIfUniversal(raw, out var selectedCpu);
        if (_data.Length < 32)
            throw new InvalidDataException("Mach-O binary is too small.");

        var magic = BinaryPrimitives.ReadUInt32LittleEndian(_data);
        Is64Bit = magic switch
        {
            Magic64 => true,
            Magic32 => false,
            _ => throw new InvalidDataException($"Unsupported Mach-O magic 0x{magic:X8}."),
        };

        var cpuType = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(4));
        Architecture = cpuType switch
        {
            CpuTypeArm64 => CpuArchitecture.Arm64,
            CpuTypeX86_64 => CpuArchitecture.X64,
            CpuTypeArm => CpuArchitecture.Arm,
            CpuTypeX86 => CpuArchitecture.X86,
            _ => CpuArchitecture.Unknown,
        };
        _ = selectedCpu;

        var ncmds = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(16));
        var sizeofcmds = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(20));
        var headerSize = Is64Bit ? 32 : 28;
        var cmdsEnd = Math.Min(_data.Length, headerSize + (int)Math.Min(sizeofcmds, (uint)(_data.Length - headerSize)));

        var segments = new List<(string Name, ulong VmAddr, ulong VmSize, long FileOff, long FileSize, uint InitProt, int CmdOffset, uint NumSects)>();
        uint chainedOffset = 0, chainedSize = 0;
        uint funcStartsOff = 0, funcStartsSize = 0;
        uint exportOff = 0, exportSize = 0;
        uint symOff = 0, symCount = 0, strOff = 0, strSize = 0;

        var p = headerSize;
        for (uint i = 0; i < ncmds && p + 8 <= cmdsEnd; i++)
        {
            var cmd = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p));
            var cmdSize = (int)BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 4));
            if (cmdSize < 8 || p + cmdSize > cmdsEnd) break;

            switch (cmd)
            {
                case LcSegment64 when cmdSize >= 72:
                {
                    var segName = ReadFixedCString(_data.AsSpan(p + 8, 16));
                    var vmAddr = BinaryPrimitives.ReadUInt64LittleEndian(_data.AsSpan(p + 24));
                    var vmSize = BinaryPrimitives.ReadUInt64LittleEndian(_data.AsSpan(p + 32));
                    var fileOff = (long)BinaryPrimitives.ReadUInt64LittleEndian(_data.AsSpan(p + 40));
                    var fileSize = (long)BinaryPrimitives.ReadUInt64LittleEndian(_data.AsSpan(p + 48));
                    var initProt = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 60));
                    var nsects = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 64));
                    if (segName == "__TEXT") ImageBase = vmAddr;
                    segments.Add((segName, vmAddr, vmSize, fileOff, fileSize, initProt, p, nsects));
                    break;
                }
                case LcSegment when cmdSize >= 56:
                {
                    var segName = ReadFixedCString(_data.AsSpan(p + 8, 16));
                    var vmAddr = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 24));
                    var vmSize = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 28));
                    var fileOff = (long)BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 32));
                    var fileSize = (long)BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 36));
                    var initProt = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 44));
                    var nsects = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 48));
                    if (segName == "__TEXT") ImageBase = vmAddr;
                    segments.Add((segName, vmAddr, vmSize, fileOff, fileSize, initProt, p, nsects));
                    break;
                }
                case LcEncryptionInfo64 or LcEncryptionInfo when cmdSize >= 20:
                {
                    var cryptId = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 16));
                    if (cryptId != 0) IsEncrypted = true;
                    break;
                }
                case LcDyldChainedFixups when cmdSize >= 16:
                    chainedOffset = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 8));
                    chainedSize = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 12));
                    break;
                case LcFunctionStarts when cmdSize >= 16:
                    funcStartsOff = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 8));
                    funcStartsSize = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 12));
                    break;
                case LcDyldExportsTrie when cmdSize >= 16:
                    exportOff = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 8));
                    exportSize = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 12));
                    break;
                case LcDyldInfo or LcDyldInfoOnly when cmdSize >= 48 && exportOff == 0:
                    exportOff = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 40));
                    exportSize = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 44));
                    break;
                case LcSymtab when cmdSize >= 24:
                    symOff = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 8));
                    symCount = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 12));
                    strOff = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 16));
                    strSize = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan(p + 20));
                    break;
            }
            p += cmdSize;
        }

        BuildSections(segments);

        if (IsEncrypted)
        {
            sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatUnsupported,
                $"Encrypted Mach-O binary '{System.IO.Path.GetFileName(path)}' (FairPlay cryptid != 0); native code and pointers may require a decrypted IPA dump.",
                new() { File = path });
        }

        if (chainedOffset != 0 && chainedSize >= 28)
            AppliedFixups = ApplyChainedFixups(chainedOffset, chainedSize);

        _functions = ParseFunctionStarts(funcStartsOff, funcStartsSize);
        ParseExportsAndSymbols(exportOff, exportSize, symOff, symCount, strOff, strSize);
    }

    public static MachOBinaryImage Load(string path, IDiagnosticSink sink) =>
        Load(path, File.ReadAllBytes(path), sink);

    public static MachOBinaryImage Load(string path, byte[] data, IDiagnosticSink sink)
    {
        if (!IsMachO(data))
            throw new InvalidDataException($"'{path}' is not a valid Mach-O binary.");
        var img = new MachOBinaryImage(path, data, sink);
        sink.Info(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatDetected,
            $"Mach-O{(img.Is64Bit ? 64 : 32)} {img.Architecture}, {img._sections.Count} sections, " +
            $"{img.AppliedFixups:N0} chained fixups, {img._functions.Length:N0} functions, {img._symbols.Count:N0} symbols.",
            new() { File = path });
        return img;
    }

    public static bool IsMachO(string path)
    {
        try
        {
            using var fs = File.OpenRead(path);
            var len = (int)Math.Min(fs.Length, 512);
            if (len < 8) return false;
            var buf = new byte[len];
            var read = fs.Read(buf, 0, len);
            return IsMachO(buf.AsSpan(0, read));
        }
        catch
        {
            return false;
        }
    }

    public static bool IsMachO(ReadOnlySpan<byte> header)
    {
        if (header.Length < 8) return false;
        var magic = BinaryPrimitives.ReadUInt32LittleEndian(header);
        if (magic is Magic64 or Magic32) return true;
        if (magic is not (FatMagicLe or FatMagic64Le)) return false;

        var count = BinaryPrimitives.ReadUInt32BigEndian(header[4..8]);
        if (count is 0 or > 16) return false;
        var entrySize = magic == FatMagic64Le ? 32 : 20;
        if (header.Length < 8 + entrySize) return false;
        var firstCpu = BinaryPrimitives.ReadUInt32BigEndian(header[8..12]);
        return firstCpu is CpuTypeArm64 or CpuTypeX86_64 or CpuTypeArm or CpuTypeX86 or 0x0200000C;
    }

    public IReadOnlyList<(string Name, ulong Rva)> GetSymbols() => _symbols;
    public IReadOnlyList<(uint Begin, uint End)> GetFunctionTable() => _functions;
    public bool TryRvaToFileOffset(ulong rva, out long fileOffset) => SectionMath.TryRvaToOffset(_sections, rva, out fileOffset);
    public bool TryFileOffsetToRva(long fileOffset, out ulong rva) => SectionMath.TryOffsetToRva(_sections, fileOffset, out rva);
    public void Dispose() { }

    private static byte[] SelectSliceIfUniversal(byte[] raw, out uint selectedCpu)
    {
        selectedCpu = raw.Length >= 8 ? BinaryPrimitives.ReadUInt32LittleEndian(raw.AsSpan(4)) : 0;
        if (raw.Length < 8) return raw;
        var magic = BinaryPrimitives.ReadUInt32LittleEndian(raw);
        if (magic is not (FatMagicLe or FatMagic64Le)) return raw;

        var isFat64 = magic == FatMagic64Le;
        var entrySize = isFat64 ? 32 : 20;
        var count = BinaryPrimitives.ReadUInt32BigEndian(raw.AsSpan(4));
        if (count is 0 or > 32 || 8 + count * entrySize > raw.Length)
            throw new InvalidDataException("Invalid universal Mach-O header.");

        var entries = new List<(uint Cpu, long Offset, long Size)>();
        for (var i = 0; i < count; i++)
        {
            var e = raw.AsSpan(8 + i * entrySize, entrySize);
            var cpu = BinaryPrimitives.ReadUInt32BigEndian(e);
            var off = isFat64 ? (long)BinaryPrimitives.ReadUInt64BigEndian(e.Slice(8, 8)) : BinaryPrimitives.ReadUInt32BigEndian(e.Slice(8, 4));
            var sz = isFat64 ? (long)BinaryPrimitives.ReadUInt64BigEndian(e.Slice(16, 8)) : BinaryPrimitives.ReadUInt32BigEndian(e.Slice(12, 4));
            if (off >= 0 && sz >= 32 && off + sz <= raw.Length)
                entries.Add((cpu, off, sz));
        }
        if (entries.Count == 0)
            throw new InvalidDataException("Universal Mach-O contains no valid architecture slice.");

        static int Preference(uint cpu) => cpu switch
        {
            CpuTypeX86_64 => 0,
            CpuTypeArm64 => 1,
            CpuTypeArm => 2,
            CpuTypeX86 => 3,
            _ => 4,
        };

        var chosen = entries.OrderBy(e => Preference(e.Cpu)).First();
        selectedCpu = chosen.Cpu;
        var slice = new byte[chosen.Size];
        Buffer.BlockCopy(raw, (int)chosen.Offset, slice, 0, (int)chosen.Size);
        return slice;
    }

    private void BuildSections(List<(string Name, ulong VmAddr, ulong VmSize, long FileOff, long FileSize, uint InitProt, int CmdOffset, uint NumSects)> segments)
    {
        var secStride = Is64Bit ? 80 : 68;
        var segCmdSize = Is64Bit ? 72 : 56;

        foreach (var seg in segments)
        {
            if (seg.Name == "__PAGEZERO" || (seg.VmSize == 0 && seg.FileSize == 0)) continue;
            var isSegWritable = (seg.InitProt & 2) != 0;
            var isSegReadable = (seg.InitProt & 1) != 0 || isSegWritable;

            if (seg.NumSects == 0)
            {
                if (seg.Name != "__LINKEDIT" && seg.FileSize > 0 && seg.FileOff >= 0 && seg.FileOff < _data.Length)
                {
                    var rva = seg.VmAddr >= ImageBase ? seg.VmAddr - ImageBase : seg.VmAddr;
                    var rawLen = Math.Min(seg.FileSize, _data.Length - seg.FileOff);
                    _sections.Add(new BinarySection(seg.Name, rva, seg.VmSize, seg.FileOff, rawLen, (seg.InitProt & 4) != 0, isSegWritable, isSegReadable));
                }
                continue;
            }

            var secPos = seg.CmdOffset + segCmdSize;
            for (uint j = 0; j < seg.NumSects && secPos + secStride <= _data.Length; j++, secPos += secStride)
            {
                var secSpan = _data.AsSpan(secPos, secStride);
                var sectName = ReadFixedCString(secSpan[..16]);
                var addr = Is64Bit ? BinaryPrimitives.ReadUInt64LittleEndian(secSpan.Slice(32, 8)) : BinaryPrimitives.ReadUInt32LittleEndian(secSpan.Slice(32, 4));
                var size = Is64Bit ? BinaryPrimitives.ReadUInt64LittleEndian(secSpan.Slice(40, 8)) : BinaryPrimitives.ReadUInt32LittleEndian(secSpan.Slice(36, 4));
                var offset = (long)(Is64Bit ? BinaryPrimitives.ReadUInt32LittleEndian(secSpan.Slice(48, 4)) : BinaryPrimitives.ReadUInt32LittleEndian(secSpan.Slice(40, 4)));
                var flags = Is64Bit ? BinaryPrimitives.ReadUInt32LittleEndian(secSpan.Slice(64, 4)) : BinaryPrimitives.ReadUInt32LittleEndian(secSpan.Slice(56, 4));

                var secType = flags & 0xFF;
                var zeroFill = secType is 0x1 or 0xC or 0x12; // S_ZEROFILL, S_THREAD_LOCAL_ZEROFILL, S_GB_ZEROFILL
                var rawSize = zeroFill || offset < 0 || offset >= _data.Length
                    ? 0L
                    : (long)Math.Min(size, (ulong)(_data.Length - offset));
                var isExec = (flags & 0x80000400u) != 0; // S_ATTR_PURE_INSTRUCTIONS | S_ATTR_SOME_INSTRUCTIONS
                var rva = addr >= ImageBase ? addr - ImageBase : addr;
                _sections.Add(new BinarySection($"{seg.Name},{sectName}", rva, size, offset, rawSize, isExec, isSegWritable, isSegReadable));
            }
        }
    }

    private int ApplyChainedFixups(uint fixupsOffset, uint fixupsSize)
    {
        if (fixupsOffset + 28 > _data.Length || fixupsSize < 28) return 0;
        var hdr = _data.AsSpan((int)fixupsOffset, (int)Math.Min(fixupsSize, (uint)(_data.Length - fixupsOffset)));
        var fixupsVersion = BinaryPrimitives.ReadUInt32LittleEndian(hdr);
        var startsOffset = BinaryPrimitives.ReadUInt32LittleEndian(hdr[4..]);
        if (fixupsVersion != 0) return 0;

        var startsBase = (long)fixupsOffset + startsOffset;
        if (startsBase + 4 > _data.Length) return 0;
        var segCount = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)startsBase));
        if (segCount > 64 || startsBase + 4 + segCount * 4 > _data.Length) return 0;

        var patched = 0;
        for (uint segIdx = 0; segIdx < segCount; segIdx++)
        {
            var segRel = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)startsBase + 4 + (int)segIdx * 4));
            if (segRel == 0) continue;
            var segInfoOff = startsBase + segRel;
            if (segInfoOff + 22 > _data.Length) continue;

            var sInfo = _data.AsSpan((int)segInfoOff);
            var pageSize = BinaryPrimitives.ReadUInt16LittleEndian(sInfo[4..]);
            var pointerFormat = BinaryPrimitives.ReadUInt16LittleEndian(sInfo[6..]);
            var segmentOffset = BinaryPrimitives.ReadUInt64LittleEndian(sInfo[8..]);
            var pageCount = BinaryPrimitives.ReadUInt16LittleEndian(sInfo[20..]);
            if (pageSize == 0 || segInfoOff + 22 + pageCount * 2 > _data.Length) continue;

            var pageStartsOff = segInfoOff + 22;
            for (ushort pageIdx = 0; pageIdx < pageCount; pageIdx++)
            {
                var startVal = BinaryPrimitives.ReadUInt16LittleEndian(_data.AsSpan((int)pageStartsOff + pageIdx * 2));
                if (startVal == 0xFFFF) continue; // DYLD_CHAINED_PTR_START_NONE

                if ((startVal & 0x8000) != 0) // DYLD_CHAINED_PTR_START_MULTI
                {
                    var overflowIdx = startVal & 0x7FFF;
                    while (true)
                    {
                        var ovOff = pageStartsOff + overflowIdx * 2;
                        if (ovOff + 2 > _data.Length) break;
                        var entry = BinaryPrimitives.ReadUInt16LittleEndian(_data.AsSpan((int)ovOff));
                        var chainStart = (long)segmentOffset + (long)pageIdx * pageSize + (entry & 0x7FFF);
                        patched += WalkFixupChain(chainStart, pointerFormat);
                        if ((entry & 0x8000) != 0) break;
                        overflowIdx++;
                    }
                }
                else
                {
                    var chainStart = (long)segmentOffset + (long)pageIdx * pageSize + startVal;
                    patched += WalkFixupChain(chainStart, pointerFormat);
                }
            }
        }
        return patched;
    }

    private int WalkFixupChain(long offset, ushort pointerFormat)
    {
        var count = 0;
        var guard = 0;
        while (offset >= 0 && offset + 8 <= _data.Length && guard++ < 1_000_000)
        {
            if (pointerFormat == 4) // DYLD_CHAINED_PTR_32
            {
                var raw32 = BinaryPrimitives.ReadUInt32LittleEndian(_data.AsSpan((int)offset));
                var bind32 = (raw32 >> 31) != 0;
                var next32 = (raw32 >> 26) & 0x1F;
                if (!bind32)
                {
                    var target32 = raw32 & 0x03FFFFFFu;
                    BinaryPrimitives.WriteUInt32LittleEndian(_data.AsSpan((int)offset), (uint)ImageBase + target32);
                    count++;
                }
                if (next32 == 0) break;
                offset += next32 * 4L;
                continue;
            }

            var raw64 = BinaryPrimitives.ReadUInt64LittleEndian(_data.AsSpan((int)offset));
            ulong next;
            long stride;
            bool isBind;
            ulong rebased = 0;
            bool write = false;

            switch (pointerFormat)
            {
                case 2: // DYLD_CHAINED_PTR_64
                case 6: // DYLD_CHAINED_PTR_64_OFFSET
                {
                    stride = 4;
                    isBind = ((raw64 >> 63) & 1) != 0;
                    next = (raw64 >> 51) & 0xFFF;
                    if (!isBind)
                    {
                        var target = raw64 & 0xFFFFFFFFFUL;
                        var high8 = (raw64 >> 36) & 0xFFUL;
                        rebased = (high8 << 56) | target;
                        if (pointerFormat == 6) rebased += ImageBase;
                        write = true;
                    }
                    break;
                }
                case 1:  // DYLD_CHAINED_PTR_ARM64E
                case 7:  // DYLD_CHAINED_PTR_ARM64E_USERLAND
                case 12: // DYLD_CHAINED_PTR_ARM64E_USERLAND24
                {
                    stride = 8;
                    var auth = ((raw64 >> 63) & 1) != 0;
                    isBind = ((raw64 >> 62) & 1) != 0;
                    next = (raw64 >> 51) & 0x7FF;
                    if (!isBind)
                    {
                        if (auth)
                        {
                            rebased = ImageBase + (raw64 & 0xFFFFFFFFUL);
                        }
                        else
                        {
                            var target = raw64 & 0x7FFFFFFFFFFUL;
                            var high8 = (raw64 >> 43) & 0xFFUL;
                            rebased = (high8 << 56) | target;
                            if (pointerFormat is 7 or 12) rebased += ImageBase;
                        }
                        write = true;
                    }
                    break;
                }
                default:
                    return count;
            }

            if (write)
            {
                BinaryPrimitives.WriteUInt64LittleEndian(_data.AsSpan((int)offset), rebased);
                count++;
            }
            if (next == 0) break;
            offset += (long)next * stride;
        }
        return count;
    }

    private (uint Begin, uint End)[] ParseFunctionStarts(uint offset, uint size)
    {
        if (offset == 0 || size == 0 || offset + size > _data.Length) return [];
        var endLimit = (int)(offset + size);
        var p = (int)offset;
        var starts = new List<uint>();
        ulong cur = 0;
        while (p < endLimit)
        {
            var delta = ReadUleb128(_data, ref p, endLimit);
            if (delta == 0) break;
            cur += delta;
            if (cur <= uint.MaxValue) starts.Add((uint)cur);
        }
        if (starts.Count == 0) return [];

        uint textEnd = 0;
        foreach (var s in _sections)
            if (s.IsExecutable && s.Rva + Math.Max(s.VirtualSize, (ulong)s.RawSize) > textEnd)
                textEnd = (uint)(s.Rva + Math.Max(s.VirtualSize, (ulong)s.RawSize));

        var table = new (uint Begin, uint End)[starts.Count];
        for (var i = 0; i < starts.Count; i++)
        {
            var begin = starts[i];
            var end = i + 1 < starts.Count ? starts[i + 1] : Math.Max(begin, textEnd);
            table[i] = (begin, end);
        }
        return table;
    }

    private void ParseExportsAndSymbols(uint exportOff, uint exportSize, uint symOff, uint symCount, uint strOff, uint strSize)
    {
        var seen = new HashSet<(string Name, ulong Rva)>();

        if (exportOff != 0 && exportSize != 0 && (long)exportOff + exportSize <= _data.Length)
        {
            ParseExportTrie(exportOff, exportSize, (name, rva) =>
            {
                var trimmed = TrimUnderscore(name);
                if (trimmed.Length > 0 && seen.Add((trimmed, rva)))
                    _symbols.Add((trimmed, rva));
            });
        }

        if (symOff == 0 || symCount == 0 || strOff == 0 || strSize == 0) return;
        var nlistSize = Is64Bit ? 16 : 12;
        if ((long)symOff + (long)symCount * nlistSize > _data.Length || (long)strOff + strSize > _data.Length) return;

        for (uint i = 0; i < symCount; i++)
        {
            var sp = _data.AsSpan((int)symOff + (int)i * nlistSize, nlistSize);
            var nType = sp[4];
            var nSect = sp[5];
            if ((nType & 0xE0) != 0) continue; // STAB debug entry
            if ((nType & 0x0E) != 0x0E || nSect == 0) continue; // not N_SECT defined symbol

            var nStrx = BinaryPrimitives.ReadUInt32LittleEndian(sp);
            if (nStrx >= strSize) continue;
            var va = Is64Bit ? BinaryPrimitives.ReadUInt64LittleEndian(sp.Slice(8, 8)) : BinaryPrimitives.ReadUInt32LittleEndian(sp.Slice(8, 4));
            if (va < ImageBase) continue;
            var rva = va - ImageBase;
            if (_sections.FirstOrDefault(s => s.ContainsRva(rva)) is not { IsExecutable: true }) continue;

            var name = ReadNullTerminated(_data.AsSpan((int)(strOff + nStrx), (int)(strSize - nStrx)));
            var trimmed = TrimUnderscore(name);
            if (trimmed.Length > 0 && seen.Add((trimmed, rva)))
                _symbols.Add((trimmed, rva));
        }
    }

    private void ParseExportTrie(uint baseOff, uint size, Action<string, ulong> onExport)
    {
        var end = (int)(baseOff + size);
        var queue = new Queue<(string Prefix, int RelOffset)>();
        var visited = new HashSet<int>();
        queue.Enqueue(("", 0));
        visited.Add(0);

        while (queue.Count > 0 && visited.Count < 500_000)
        {
            var (prefix, rel) = queue.Dequeue();
            var p = (int)baseOff + rel;
            if (p < (int)baseOff || p >= end) continue;

            var termSize = (int)ReadUleb128(_data, ref p, end);
            var childrenPos = p + termSize;
            if (childrenPos > end) continue;

            if (termSize > 0)
            {
                var flags = ReadUleb128(_data, ref p, childrenPos);
                if ((flags & 0x08) == 0) // skip ReExport
                {
                    var addr = ReadUleb128(_data, ref p, childrenPos);
                    onExport(prefix, addr);
                }
            }

            p = childrenPos;
            if (p >= end) continue;
            var childCount = _data[p++];
            for (var c = 0; c < childCount && p < end; c++)
            {
                var labelEnd = _data.AsSpan(p, end - p).IndexOf((byte)0);
                if (labelEnd < 0) break;
                var seg = Encoding.UTF8.GetString(_data, p, labelEnd);
                p += labelEnd + 1;
                if (p >= end) break;
                var childOff = (int)ReadUleb128(_data, ref p, end);
                if (visited.Add(childOff))
                    queue.Enqueue((prefix + seg, childOff));
            }
        }
    }

    private static ulong ReadUleb128(byte[] data, ref int p, int end)
    {
        ulong result = 0;
        var shift = 0;
        while (p < end && shift < 64)
        {
            var b = data[p++];
            result |= (ulong)(b & 0x7F) << shift;
            if ((b & 0x80) == 0) break;
            shift += 7;
        }
        return result;
    }

    private static string TrimUnderscore(string s) => s.Length > 1 && s[0] == '_' ? s[1..] : s;

    private static string ReadFixedCString(ReadOnlySpan<byte> span)
    {
        var nul = span.IndexOf((byte)0);
        return Encoding.ASCII.GetString(nul >= 0 ? span[..nul] : span);
    }

    private static string ReadNullTerminated(ReadOnlySpan<byte> span)
    {
        var nul = span.IndexOf((byte)0);
        return Encoding.UTF8.GetString(nul >= 0 ? span[..nul] : span);
    }
}
