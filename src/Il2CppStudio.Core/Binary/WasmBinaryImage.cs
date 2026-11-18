using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// WebAssembly module (Unity WebGL <c>*.wasm</c>, optionally <c>.br</c>/<c>.gz</c> compressed). IL2CPP's registration
/// structures live in linear memory, initialized by active data segments; each segment becomes a 32-bit "section" at
/// its memory offset so the format-neutral registration scanner works unchanged. Method pointers in those structures
/// are indices into the function table (element section) and are resolved to code bodies by <see cref="FunctionBody"/>.
/// </summary>
/// <summary>WebAssembly function type: value type bytes (0x7F i32, 0x7E i64, 0x7D f32, 0x7C f64).</summary>
public readonly record struct WasmFuncType(byte[] Parameters, byte[] Results)
{
    public int ParameterCount => Parameters.Length;
    public int ResultCount => Results.Length;
    public byte Parameter(int index) => Parameters[index];
    public byte Result(int index) => Results[index];
}

/// <summary>A module global: value type, mutability and constant initializer bits.</summary>
public readonly record struct WasmGlobal(byte ValueType, bool Mutable, long Init);

public sealed class WasmBinaryImage : IBinaryImage
{
    private readonly byte[] _data;
    private readonly List<BinarySection> _sections = [];
    private readonly Dictionary<uint, uint> _table = [];                 // table slot -> function index
    private readonly List<(int Offset, int Length)> _bodies = [];       // defined functions, in order
    private readonly List<(string Name, ulong Rva)> _exports = [];
    private readonly List<WasmFuncType> _types = [];                    // type section
    private readonly List<uint> _importFunctionTypes = [];              // function imports, in index order
    private readonly List<(string Module, string Name)> _importFunctionNames = [];   // function imports, in index order
    private readonly List<uint> _definedFunctionTypes = [];             // code section entries, in index order
    private readonly List<WasmGlobal> _globals = [];                    // imported globals first, then defined

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

    /// <summary>Function index behind a function-table slot (an IL2CPP method pointer).</summary>
    public bool TryGetFunctionIndex(ulong tableSlot, out uint functionIndex) =>
        _table.TryGetValue((uint)tableSlot, out functionIndex);

    /// <summary>Code bytes of the function behind a function-table slot (an IL2CPP method pointer), or empty.</summary>
    public ReadOnlySpan<byte> FunctionBody(ulong tableSlot, out uint functionIndex)
    {
        functionIndex = 0;
        if (!_table.TryGetValue((uint)tableSlot, out functionIndex)) return [];
        return FunctionBody(functionIndex);
    }

    /// <summary>Body (local declarations + code) of a defined function, or empty for imports/unknown indices.</summary>
    public ReadOnlySpan<byte> FunctionBody(uint functionIndex)
    {
        var defined = (int)functionIndex - ImportedFunctions;
        if ((uint)defined >= (uint)_bodies.Count) return [];
        var (off, len) = _bodies[defined];
        return _data.AsSpan(off, len);
    }

    /// <summary>Signature of import (index &lt; <see cref="ImportedFunctions"/>) or defined function.</summary>
    public bool TryGetFunctionType(uint functionIndex, out WasmFuncType type)    {
        type = default;
        var typeIndex = functionIndex < ImportedFunctions
            ? (uint)functionIndex < (uint)_importFunctionTypes.Count ? _importFunctionTypes[(int)functionIndex] : uint.MaxValue
            : (int)(functionIndex - ImportedFunctions) is var d && (uint)d < (uint)_definedFunctionTypes.Count ? _definedFunctionTypes[d] : uint.MaxValue;
        if (typeIndex == uint.MaxValue || typeIndex >= _types.Count) return false;
        type = _types[(int)typeIndex];
        return true;
    }

    /// <summary>Signature and body of the defined function behind a table slot.</summary>
    public bool TryGetFunction(ulong tableSlot, out uint functionIndex, out WasmFuncType type)
    {
        type = default;
        if (!_table.TryGetValue((uint)tableSlot, out functionIndex)) return false;
        if (!TryGetFunctionType(functionIndex, out type)) return false;
        var defined = (int)functionIndex - ImportedFunctions;
        return (uint)defined < (uint)_bodies.Count;
    }

    public bool TryGetGlobal(uint index, out WasmGlobal global)
    {
        if ((uint)index >= (uint)_globals.Count) { global = default; return false; }
        global = _globals[(int)index];
        return true;
    }

    /// <summary>Module and field name of an imported function (e.g. <c>env</c>/<c>emscripten_memcpy</c>).</summary>
    public bool TryGetImportName(uint functionIndex, out string module, out string name)
    {
        if (functionIndex >= (uint)_importFunctionNames.Count) { module = name = ""; return false; }
        (module, name) = _importFunctionNames[(int)functionIndex];
        return true;
    }

    /// <summary>Function type by type-section index (used for block signatures that name a type).</summary>
    public bool TryGetType(uint typeIndex, out WasmFuncType type)
    {
        if (typeIndex >= (uint)_types.Count) { type = default; return false; }
        type = _types[(int)typeIndex];
        return true;
    }

    public int FunctionCount => ImportedFunctions + _bodies.Count;
    public int TypeCount => _types.Count;

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
                case 1: ParseTypes(p); break;
                case 2: ParseImports(p); break;
                case 3: ParseFunctions(p); break;
                case 6: ParseGlobals(p); break;
                case 7: ParseExports(p); break;
                case 9: ParseElements(p); break;
                case 10: ParseCode(p); break;
                case 11: ParseData(p); break;
            }
            p = end;
        }
    }

    private void ParseTypes(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            var form = _data[p++];
            if (form != 0x60) { _types.Add(default); continue; }   // only func types are legal in the type section
            var pc = (int)Leb.U32(_data, ref p);
            var ps = _data.AsSpan(p, pc).ToArray(); p += pc;
            var rc = (int)Leb.U32(_data, ref p);
            var rs = _data.AsSpan(p, rc).ToArray(); p += rc;
            _types.Add(new WasmFuncType(ps, rs));
        }
    }

    private void ParseFunctions(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++) _definedFunctionTypes.Add(Leb.U32(_data, ref p));
    }

    private void ParseGlobals(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            var type = _data[p++];
            var mutable = _data[p++] != 0;
            var value = GlobalInit(ref p);
            _globals.Add(new WasmGlobal(type, mutable, value));
        }
    }

    /// <summary><c>i32.const</c>/<c>i64.const</c>/<c>f32.const</c>/<c>f64.const</c> initializer; other initializers yield 0.</summary>
    private long GlobalInit(ref int p)
    {
        var op = _data[p++];
        long v;
        switch (op)
        {
            case 0x41 or 0x42: v = Leb.S64(_data, ref p); break;
            case 0x43: v = BitConverter.ToInt32(_data, p); p += 4; break;
            case 0x44: v = BitConverter.ToInt64(_data, p); p += 8; break;
            default: v = 0; break;
        }
        while (_data[p++] != 0x0B) { }
        return v;
    }

    private void ParseImports(int p)
    {
        var n = Leb.U32(_data, ref p);
        for (var i = 0; i < n; i++)
        {
            var module = ReadName(ref p);
            var field = ReadName(ref p);
            var kind = _data[p++];
            switch (kind)
            {
                case 0:
                    _importFunctionTypes.Add(Leb.U32(_data, ref p));
                    _importFunctionNames.Add((module, field));
                    ImportedFunctions++;
                    break;
                case 1: p++; Limits(ref p); break;                                     // table: elemtype + limits
                case 2: Limits(ref p); break;                                          // memory
                case 3: p += 2; break;                                                 // global: valtype + mut
            }
        }
    }

    private string ReadName(ref int p)
    {
        var len = (int)Leb.U32(_data, ref p);
        var s = Encoding.UTF8.GetString(_data, p, len);
        p += len;
        return s;
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

    /// <summary>
    /// Skips a code body's local declarations: a vec of (count, valtype) groups. Returns the number of declared
    /// locals (parameters are not included) and the offset of the first instruction.
    /// </summary>
    public static bool TryReadLocals(ReadOnlySpan<byte> body, out List<byte> locals, out int instructionOffset)
    {
        locals = [];
        instructionOffset = 0;
        try
        {
            var p = 0;
            var groups = Leb.U32(body, ref p);
            for (var g = 0u; g < groups; g++)
            {
                var count = Leb.U32(body, ref p);
                if (count > 1_000_000) return false;
                var type = body[p++];
                for (var i = 0u; i < count; i++) locals.Add(type);
            }
            instructionOffset = p;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
