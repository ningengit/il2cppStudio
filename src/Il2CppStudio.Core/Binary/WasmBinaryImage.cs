using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// WebAssembly module (Unity WebGL <c>*.wasm</c>, optionally <c>.br</c>/<c>.gz</c> compressed). IL2CPP's registration
/// structures live in linear memory, initialized by active data segments; each segment becomes a 32-bit "section" at
/// its memory offset so the format-neutral registration scanner works unchanged. Method pointers in those structures
/// are indices into the function table (element section) and are resolved to code bodies by <see cref="FunctionBody"/>.
/// </summary>
public sealed class WasmBinaryImage : IBinaryImage
{
    private readonly byte[] _data;
    private readonly List<BinarySection> _sections = [];
    private readonly Dictionary<uint, uint> _table = [];                 // table slot -> function index
    private readonly List<(int Offset, int Length)> _bodies = [];       // defined functions, in order
    private readonly List<(string Name, ulong Rva)> _exports = [];

    public string Path { get; }
    public BinaryFormat Format => BinaryFormat.Wasm;
    public CpuArchitecture Architecture => CpuArchitecture.Wasm32;
    public bool Is64Bit => false;
    public ulong ImageBase => 0;
    public IReadOnlyList<BinarySection> Sections => _sections;
    public ReadOnlyMemory<byte> Data => _combined ?? _data;
    public int ImportedFunctions { get; private set; }
    public int DefinedFunctions => _bodies.Count;

    private WasmBinaryImage(string path, byte[] data) { Path = path; _data = data; Parse(); FinishLayout(); }

    public static WasmBinaryImage Load(string path, IDiagnosticSink sink)
    {
        var data = WasmFiles.ReadDecompressed(path);
        if (data.Length < 8 || BitConverter.ToUInt32(data) != 0x6D736100) throw new InvalidDataException("Not a WebAssembly module (missing \\0asm magic).");
        var img = new WasmBinaryImage(path, data);
        sink.Info(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatDetected,
            $"WebAssembly module: {img.ImportedFunctions:N0} imported + {img.DefinedFunctions:N0} defined functions, {img._table.Count:N0} table slots, {img.DataSegments:N0} data segments → linear memory 0x{img._memoryBase:X}+0x{(img._sections.FirstOrDefault()?.RawSize ?? 0):X}.",
            new() { File = path });
        return img;
    }

    public IReadOnlyList<(string Name, ulong Rva)> GetSymbols() => _exports;

    /// <summary>Code bytes of the function behind a function-table slot (an IL2CPP method pointer), or empty.</summary>
    public ReadOnlySpan<byte> FunctionBody(ulong tableSlot, out uint functionIndex)
    {
        functionIndex = 0;
        if (!_table.TryGetValue((uint)tableSlot, out functionIndex)) return [];
        var defined = (int)functionIndex - ImportedFunctions;
        if ((uint)defined >= (uint)_bodies.Count) return [];
        var (off, len) = _bodies[defined];
        return _data.AsSpan(off, len);
    }

    public bool TryRvaToFileOffset(ulong rva, out long fileOffset) => SectionMath.TryRvaToOffset(_sections, rva, out fileOffset);
    public bool TryFileOffsetToRva(long fileOffset, out ulong rva) => SectionMath.TryOffsetToRva(_sections, fileOffset, out rva);
    public void Dispose() { }

    private void Parse()
    {
        var p = 8;
        while (p < _data.Length)
        {
            var id = _data[p++];
            var size = (int)Leb.U32(_data, ref p);
            var end = p + size;
            switch (id)
            {
                case 2: ParseImports(p); break;
                case 7: ParseExports(p); break;
                case 9: ParseElements(p); break;
                case 10: ParseCode(p); break;
                case 11: ParseData(p); break;
            }
            p = end;
        }
    }

    private void ParseImports(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            SkipName(ref p); SkipName(ref p);
            var kind = _data[p++];
            switch (kind)
            {
                case 0: Leb.U32(_data, ref p); ImportedFunctions++; break;            // function: type index
                case 1: p++; Limits(ref p); break;                                     // table: elemtype + limits
                case 2: Limits(ref p); break;                                          // memory
                case 3: p += 2; break;                                                 // global: valtype + mut
            }
        }
    }

    private void ParseExports(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            var len = (int)Leb.U32(_data, ref p);
            var name = Encoding.UTF8.GetString(_data, p, len); p += len;
            var kind = _data[p++];
            var index = Leb.U32(_data, ref p);
            if (kind == 0) _exports.Add((name, index));   // function index (not a table slot)
        }
    }

    private void ParseElements(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            var flags = Leb.U32(_data, ref p);
            if (flags is not (0 or 2)) return;                                         // only active funcref segments (what Emscripten emits)
            if (flags == 2) Leb.U32(_data, ref p);                                     // table index
            var offset = ConstExpr(ref p);
            if (flags == 2) p++;                                                       // elemkind
            var count = Leb.U32(_data, ref p);
            for (uint k = 0; k < count; k++) _table[(uint)offset + k] = Leb.U32(_data, ref p);
        }
    }

    private void ParseCode(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            var size = (int)Leb.U32(_data, ref p);
            _bodies.Add((p, size));
            p += size;
        }
    }

    private void ParseData(int p)
    {
        // Active segments are copied into one linear-memory image appended after the file bytes (Data = file ++ memory),
        // exposed as a single section: structures that straddle segment boundaries read contiguously.
        var segments = new List<(long Offset, int FileOffset, int Size)>();
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            var flags = Leb.U32(_data, ref p);
            if (flags == 2) Leb.U32(_data, ref p);                                     // memory index
            var offset = flags == 1 ? -1 : ConstExpr(ref p);                            // 1 = passive (no address)
            var size = (int)Leb.U32(_data, ref p);
            if (offset >= 0) segments.Add((offset, p, size));
            p += size;
        }
        if (segments.Count == 0) return;
        var lo = segments.Min(s => s.Offset);
        var hi = segments.Max(s => s.Offset + s.Size);
        _memory = new byte[hi - lo];
        foreach (var s in segments) Buffer.BlockCopy(_data, s.FileOffset, _memory, (int)(s.Offset - lo), s.Size);
        _memoryBase = lo;
        DataSegments = segments.Count;
    }

    private byte[]? _memory;
    private long _memoryBase;
    public int DataSegments { get; private set; }

    private void FinishLayout()
    {
        if (_memory is null) return;
        var combined = new byte[_data.Length + _memory.Length];
        Buffer.BlockCopy(_data, 0, combined, 0, _data.Length);
        Buffer.BlockCopy(_memory, 0, combined, _data.Length, _memory.Length);
        _combined = combined;
        _sections.Add(new BinarySection("memory", (ulong)_memoryBase, (ulong)_memory.Length, _data.Length, _memory.Length, false, true, true));
        _memory = null;
    }

    private byte[]? _combined;

    /// <summary><c>i32.const N; end</c> (also accepts <c>i64.const</c>); other initializers yield 0.</summary>
    private long ConstExpr(ref int p)
    {
        var op = _data[p++];
        long v = op is 0x41 or 0x42 ? Leb.S64(_data, ref p) : 0;
        while (_data[p++] != 0x0B) { }
        return v;
    }

    private void SkipName(ref int p) { var len = (int)Leb.U32(_data, ref p); p += len; }
    private void Limits(ref int p) { var f = _data[p++]; Leb.U32(_data, ref p); if ((f & 1) != 0) Leb.U32(_data, ref p); }
}
