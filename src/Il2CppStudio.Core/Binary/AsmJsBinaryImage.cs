using System.Collections.Concurrent;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Core.Binary;

public enum HeapViewKind { Int8, Int16, Int32, Uint8, Uint16, Uint32, Float32, Float64 }

public enum AsmJsHelperKind { None, V16MethodInfo, V16TypeInfo, V16StringLiteral, ClassInit, NullCheck }

/// <summary>
/// Legacy pre-5.6 Unity WebGL Emscripten <c>asm.js</c> binary image backed by <c>*.jsgz</c>/<c>*.asm.js</c> and its
/// sibling memory initializer (<c>*.js.memgz</c>/<c>*.mem</c>). The memory initializer supplies <see cref="IBinaryImage.Data"/>
/// mapped at <c>STATIC_BASE</c> (typically <c>8</c>) so the standard 32-bit IL2CPP registration scanner locates
/// <c>Il2CppMetadataRegistration</c> and <c>Il2CppCodeRegistration</c> unchanged, while <c>FUNCTION_TABLE</c> arrays
/// inside the <c>"use asm"</c> module resolve table slots to concrete <c>asm.js</c> function bodies.
/// </summary>
public sealed partial class AsmJsBinaryImage : IBinaryImage
{
    public const ulong FuncVaBase = 0x8000_0000UL;

    private readonly byte[] _memory;
    private readonly List<BinarySection> _sections = [];
    private readonly string _jsText;
    private readonly Dictionary<string, (int Id, int Offset, int Length)> _functions = new(StringComparer.Ordinal);
    private readonly Dictionary<uint, string> _funcById = [];
    private readonly Dictionary<string, (string[] Entries, uint Mask)> _tablesBySig = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string[]> _tablesByVar = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, int> _paramCountCache = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, AsmJsHelperKind> _helperCache = new(StringComparer.Ordinal);
    private readonly List<(string Name, ulong Rva)> _exports = [];

    public string Path { get; }
    public string MemoryPath { get; }
    public BinaryFormat Format => BinaryFormat.AsmJs;
    public CpuArchitecture Architecture => CpuArchitecture.AsmJs;
    public bool Is64Bit => false;
    public ulong ImageBase => 0;
    public IReadOnlyList<BinarySection> Sections => _sections;
    public ReadOnlyMemory<byte> Data => _memory;
    public int StaticBase { get; }
    public int MaxTableSlot { get; private set; }
    public int FunctionCount => _functions.Count;
    public int TableCount => _tablesBySig.Count;

    public IReadOnlyDictionary<string, HeapViewKind> HeapViews { get; }
    public IReadOnlyDictionary<string, string> MathAliases { get; }
    public IReadOnlySet<string> FloatZeroVars { get; }
    public string? StackTopVar { get; }
    public string? FroundVar { get; }

    private AsmJsBinaryImage(
        string jsPath,
        string memPath,
        string jsText,
        byte[] memory,
        int staticBase,
        Dictionary<string, HeapViewKind> heapViews,
        Dictionary<string, string> mathAliases,
        HashSet<string> floatZeroVars,
        string? stackTopVar,
        string? froundVar)
    {
        Path = jsPath;
        MemoryPath = memPath;
        _jsText = jsText;
        _memory = memory;
        StaticBase = staticBase;
        HeapViews = heapViews;
        MathAliases = mathAliases;
        FloatZeroVars = floatZeroVars;
        StackTopVar = stackTopVar;
        FroundVar = froundVar;
        _sections.Add(new BinarySection("memory", (ulong)staticBase, (ulong)memory.Length, 0, memory.Length, false, true, true));

        IndexModule();
    }

    public bool TryRvaToFileOffset(ulong rva, out long fileOffset) => SectionMath.TryRvaToOffset(_sections, rva, out fileOffset);
    public bool TryFileOffsetToRva(long fileOffset, out ulong rva) => SectionMath.TryOffsetToRva(_sections, fileOffset, out rva);
    public IReadOnlyList<(string Name, ulong Rva)> GetSymbols() => _exports;
    public void Dispose() { }

    /// <summary>True when <paramref name="jsOrMemPath"/> pairs a real Emscripten <c>"use asm"</c> script with a memory initializer.</summary>
    public static bool CanLoad(string jsOrMemPath)
    {
        if (!TryLocatePair(jsOrMemPath, out var jsPath, out _)) return false;
        return ContainsUseAsmMarker(jsPath);
    }

    /// <summary>Loads an <c>asm.js</c> + memory-initializer pair, or returns null when the file is not an Emscripten <c>asm.js</c> build.</summary>
    public static AsmJsBinaryImage? TryLoad(string jsOrMemPath, IDiagnosticSink sink)
    {
        if (!TryLocatePair(jsOrMemPath, out var jsPath, out var memPath))
            return null;

        byte[] memBytes;
        string jsText;
        try
        {
            memBytes = ReadMaybeCompressed(memPath);
            jsText = Encoding.UTF8.GetString(ReadMaybeCompressed(jsPath));
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.FileAccessFailed,
                $"Could not read legacy WebGL asm.js files: {ex.Message}", new() { File = jsPath }, ex);
            return null;
        }

        var useAsmIdx = FindUseAsm(jsText, out _);
        if (useAsmIdx < 0) return null;

        var staticBase = ParseStaticBase(jsText, useAsmIdx);
        ParsePreamble(jsText, useAsmIdx, out var heapViews, out var mathAliases, out var floatZeroVars, out var stackTopVar, out var froundVar);

        var img = new AsmJsBinaryImage(jsPath, memPath, jsText, memBytes, staticBase, heapViews, mathAliases, floatZeroVars, stackTopVar, froundVar);
        sink.Info(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatDetected,
            $"WebGL asm.js module: {img.FunctionCount:N0} functions, {img.TableCount} dynCall tables, memory 0x{staticBase:X}+0x{memBytes.Length:X}.",
            new() { File = jsPath });
        return img;
    }

    public bool TryGetFuncVa(string name, out ulong funcVa)
    {
        if (_functions.TryGetValue(name, out var f))
        {
            funcVa = FuncVaBase + (uint)f.Id;
            return true;
        }
        funcVa = 0;
        return false;
    }

    public bool TryGetFunctionSource(ulong funcVa, out string name, out string source)
    {
        if (funcVa > FuncVaBase && _funcById.TryGetValue((uint)(funcVa - FuncVaBase), out var n) && _functions.TryGetValue(n, out var info))
        {
            name = n;
            source = _jsText.Substring(info.Offset, info.Length);
            return true;
        }
        name = "";
        source = "";
        return false;
    }

    public bool TryGetFunctionSourceByName(string name, out string source)
    {
        if (_functions.TryGetValue(name, out var info))
        {
            source = _jsText.Substring(info.Offset, info.Length);
            return true;
        }
        source = "";
        return false;
    }

    public bool TryResolveTableSlot(string sig, ulong rawSlot, out ulong funcVa)
    {
        funcVa = 0;
        if (!_tablesBySig.TryGetValue(sig, out var tbl) || tbl.Entries.Length == 0) return false;
        var slot = (int)(rawSlot & tbl.Mask);
        if ((uint)slot >= (uint)tbl.Entries.Length) return false;
        return TryGetFuncVa(tbl.Entries[slot], out funcVa);
    }

    public bool TryResolveIndirectTable(string tableVar, int slot, out ulong funcVa)
    {
        funcVa = 0;
        if (!_tablesByVar.TryGetValue(tableVar, out var entries) || (uint)slot >= (uint)entries.Length) return false;
        return TryGetFuncVa(entries[slot], out funcVa);
    }

    public bool TryGetFunctionParamCount(ulong funcVa, out int count)
    {
        count = 0;
        if (funcVa <= FuncVaBase || !_funcById.TryGetValue((uint)(funcVa - FuncVaBase), out var name))
            return false;
        if (_paramCountCache.TryGetValue(name, out count)) return true;
        if (!_functions.TryGetValue(name, out var info)) return false;

        var open = _jsText.IndexOf('(', info.Offset);
        var close = open >= 0 ? _jsText.IndexOf(')', open + 1) : -1;
        if (open < 0 || close < 0 || close - open > 512) return false;
        var paramList = _jsText.AsSpan(open + 1, close - open - 1).Trim();
        count = paramList.IsEmpty ? 0 : paramList.Count(',') + 1;
        _paramCountCache[name] = count;
        return true;
    }

    public AsmJsHelperKind ClassifyHelper(string fnName)
    {
        if (_helperCache.TryGetValue(fnName, out var cached)) return cached;
        var kind = ClassifyHelperCore(fnName);
        _helperCache[fnName] = kind;
        return kind;
    }

    private AsmJsHelperKind ClassifyHelperCore(string fnName)
    {
        if (!TryGetFunctionSourceByName(fnName, out var src)) return AsmJsHelperKind.None;
        if (src.Contains("2147483647", StringComparison.Ordinal) && src.Contains("<0", StringComparison.Ordinal))
            return AsmJsHelperKind.V16MethodInfo;
        if (src.Contains("==-1", StringComparison.Ordinal) && src.Contains("<<3", StringComparison.Ordinal) && src.Contains("+8>>2", StringComparison.Ordinal))
            return AsmJsHelperKind.V16StringLiteral;
        if (src.Contains("==-1", StringComparison.Ordinal) && src.Contains("+28>>2", StringComparison.Ordinal) && !src.Contains("<<3", StringComparison.Ordinal))
            return AsmJsHelperKind.V16TypeInfo;
        return AsmJsHelperKind.None;
    }

    // ------------------------------------------------------------------ indexing

    private void IndexModule()
    {
        var useAsmIdx = FindUseAsm(_jsText, out _);
        if (useAsmIdx < 0) return;

        var funcIdx = FindFirstFunctionDecl(_jsText, useAsmIdx);
        if (funcIdx < 0) return;

        var endAsmIdx = _jsText.IndexOf("EMSCRIPTEN_END_ASM", funcIdx, StringComparison.Ordinal);
        if (endAsmIdx < 0) endAsmIdx = _jsText.Length;

        var pos = funcIdx;
        var nextId = 1u;
        var lastFuncEnd = funcIdx;

        while (pos < endAsmIdx)
        {
            while (pos < endAsmIdx && (_jsText[pos] is ' ' or '\t' or '\r' or '\n' or ';')) pos++;
            if (pos >= endAsmIdx) break;

            if (!MatchesKeyword(_jsText, pos, "function "))
            {
                // Reached table / return section after function declarations.
                if (MatchesKeyword(_jsText, pos, "var ") || MatchesKeyword(_jsText, pos, "return"))
                    break;
                pos++;
                continue;
            }

            var nameStart = pos + "function ".Length;
            while (nameStart < endAsmIdx && _jsText[nameStart] == ' ') nameStart++;
            var openParen = _jsText.IndexOf('(', nameStart);
            if (openParen < 0 || openParen >= endAsmIdx) break;
            var name = _jsText.Substring(nameStart, openParen - nameStart).Trim();
            var openBrace = _jsText.IndexOf('{', openParen);
            if (openBrace < 0 || openBrace >= endAsmIdx) break;

            var depth = 0;
            var i = openBrace;
            for (; i < endAsmIdx; i++)
            {
                var ch = _jsText[i];
                if (ch == '{') depth++;
                else if (ch == '}')
                {
                    if (--depth == 0) { i++; break; }
                }
            }
            if (depth != 0) break;

            if (!string.IsNullOrEmpty(name))
            {
                var id = nextId++;
                _functions[name] = ((int)id, pos, i - pos);
                _funcById[id] = name;
            }
            lastFuncEnd = i;
            pos = i;
        }

        // Parse function tables and dynCall exports in the module tail.
        var tail = _jsText.Substring(lastFuncEnd, endAsmIdx - lastFuncEnd);
        foreach (Match m in TableVarRegex().Matches(tail))
        {
            var tableVar = m.Groups[1].Value;
            var entries = m.Groups[2].Value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            _tablesByVar[tableVar] = entries;
            if (entries.Length - 1 > MaxTableSlot) MaxTableSlot = entries.Length - 1;
        }

        var retIdx = tail.LastIndexOf("return{", StringComparison.Ordinal);
        if (retIdx < 0) retIdx = tail.LastIndexOf("return {", StringComparison.Ordinal);
        if (retIdx >= 0)
        {
            var closeBrace = tail.IndexOf('}', retIdx);
            if (closeBrace > retIdx)
            {
                var retBody = tail.Substring(retIdx, closeBrace - retIdx + 1);
                foreach (Match em in ExportPairRegex().Matches(retBody))
                {
                    var exportName = em.Groups[1].Value;
                    var targetFn = em.Groups[2].Value;
                    if (TryGetFuncVa(targetFn, out var fva))
                        _exports.Add((exportName, fva));

                    if (exportName.StartsWith("dynCall_", StringComparison.Ordinal) && TryGetFunctionSourceByName(targetFn, out var dynSrc))
                    {
                        var dm = DynCallLookupRegex().Match(dynSrc);
                        if (dm.Success
                            && _tablesByVar.TryGetValue(dm.Groups[1].Value, out var entries)
                            && uint.TryParse(dm.Groups[2].Value, out var mask))
                        {
                            var sig = exportName["dynCall_".Length..];
                            _tablesBySig[sig] = (entries, mask);
                        }
                    }
                }
            }
        }
    }

    private static int FindUseAsm(string text, out int quoteEnd)
    {
        quoteEnd = -1;
        var idx = text.IndexOf("\"use asm\"", StringComparison.Ordinal);
        if (idx < 0) idx = text.IndexOf("'use asm'", StringComparison.Ordinal);
        if (idx >= 0) quoteEnd = idx + 9;
        return idx;
    }

    private static int FindFirstFunctionDecl(string text, int from)
    {
        var idx = text.IndexOf("function ", from, StringComparison.Ordinal);
        while (idx >= 0)
        {
            if (idx == 0 || text[idx - 1] is ';' or '\n' or '\r' or '}')
                return idx;
            idx = text.IndexOf("function ", idx + 9, StringComparison.Ordinal);
        }
        return -1;
    }

    private static bool MatchesKeyword(string s, int pos, string kw) =>
        pos + kw.Length <= s.Length && string.CompareOrdinal(s, pos, kw, 0, kw.Length) == 0;

    private static int ParseStaticBase(string jsText, int beforeIdx)
    {
        var scanLen = Math.Min(beforeIdx, 500_000);
        var preamble = jsText.Substring(0, scanLen);
        var result = 8;
        foreach (Match m in StaticBaseRegex().Matches(preamble))
        {
            if (int.TryParse(m.Groups[1].Value, out var v) && v > 0)
                result = v;
        }
        return result;
    }

    private static void ParsePreamble(
        string jsText,
        int useAsmIdx,
        out Dictionary<string, HeapViewKind> heapViews,
        out Dictionary<string, string> mathAliases,
        out HashSet<string> floatZeroVars,
        out string? stackTopVar,
        out string? froundVar)
    {
        var firstFunc = FindFirstFunctionDecl(jsText, useAsmIdx);
        var headerEnd = firstFunc > useAsmIdx ? firstFunc : Math.Min(jsText.Length, useAsmIdx + 16_384);
        var header = jsText.Substring(useAsmIdx, headerEnd - useAsmIdx);

        heapViews = new Dictionary<string, HeapViewKind>(StringComparer.Ordinal);
        foreach (Match m in HeapViewRegex().Matches(header))
        {
            var varName = m.Groups[1].Value;
            var kind = m.Groups[2].Value switch
            {
                "Int8Array" => HeapViewKind.Int8,
                "Int16Array" => HeapViewKind.Int16,
                "Int32Array" => HeapViewKind.Int32,
                "Uint8Array" => HeapViewKind.Uint8,
                "Uint16Array" => HeapViewKind.Uint16,
                "Uint32Array" => HeapViewKind.Uint32,
                "Float32Array" => HeapViewKind.Float32,
                "Float64Array" => HeapViewKind.Float64,
                _ => HeapViewKind.Int32,
            };
            heapViews[varName] = kind;
        }

        stackTopVar = StackTopRegex().Match(header) is { Success: true } sm ? sm.Groups[1].Value : null;
        froundVar = FroundRegex().Match(header) is { Success: true } fm ? fm.Groups[1].Value : null;

        mathAliases = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (Match mm in MathAliasRegex().Matches(header))
            mathAliases[mm.Groups[1].Value] = mm.Groups[2].Value;

        floatZeroVars = new HashSet<string>(StringComparer.Ordinal);
        foreach (Match zm in FloatZeroVarRegex().Matches(header))
        {
            var fn = zm.Groups[2].Value;
            if (string.IsNullOrEmpty(fn) || fn == froundVar)
                floatZeroVars.Add(zm.Groups[1].Value);
        }
    }

    // ------------------------------------------------------------------ file discovery & decompression

    private static bool TryLocatePair(string jsOrMemPath, out string jsPath, out string memPath)
    {
        jsPath = "";
        memPath = "";
        if (!File.Exists(jsOrMemPath)) return false;

        var dir = System.IO.Path.GetDirectoryName(jsOrMemPath);
        if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return false;

        static bool IsHelperScript(string file)
        {
            var n = System.IO.Path.GetFileName(file);
            return n.StartsWith("UnityLoader", StringComparison.OrdinalIgnoreCase)
                || n.StartsWith("UnityProgress", StringComparison.OrdinalIgnoreCase)
                || n.Contains(".framework", StringComparison.OrdinalIgnoreCase);
        }

        var files = Directory.EnumerateFiles(dir).ToList();
        var isMem = jsOrMemPath.EndsWith(".memgz", StringComparison.OrdinalIgnoreCase)
            || jsOrMemPath.EndsWith(".js.mem", StringComparison.OrdinalIgnoreCase)
            || jsOrMemPath.EndsWith(".mem", StringComparison.OrdinalIgnoreCase)
            || jsOrMemPath.EndsWith(".asm.memory.unityweb", StringComparison.OrdinalIgnoreCase);

        if (isMem)
        {
            memPath = jsOrMemPath;
            jsPath = files.FirstOrDefault(f => !IsHelperScript(f) && (
                f.EndsWith(".jsgz", StringComparison.OrdinalIgnoreCase)
                || f.EndsWith(".asm.code.unityweb", StringComparison.OrdinalIgnoreCase)
                || f.EndsWith(".asm.js", StringComparison.OrdinalIgnoreCase)
                || f.EndsWith(".js", StringComparison.OrdinalIgnoreCase))) ?? "";
        }
        else
        {
            jsPath = jsOrMemPath;
            memPath = files.FirstOrDefault(f =>
                f.EndsWith(".js.memgz", StringComparison.OrdinalIgnoreCase)
                || f.EndsWith(".memgz", StringComparison.OrdinalIgnoreCase)
                || f.EndsWith(".asm.memory.unityweb", StringComparison.OrdinalIgnoreCase)
                || f.EndsWith(".js.mem", StringComparison.OrdinalIgnoreCase)
                || f.EndsWith(".mem", StringComparison.OrdinalIgnoreCase)) ?? "";
        }

        return File.Exists(jsPath) && File.Exists(memPath);
    }

    private static bool ContainsUseAsmMarker(string jsPath)
    {
        try
        {
            var raw = File.ReadAllBytes(jsPath);
            using var ms = new MemoryStream(raw);
            Stream stream = raw.Length >= 2 && raw[0] == 0x1F && raw[1] == 0x8B
                ? new GZipStream(ms, CompressionMode.Decompress)
                : jsPath.EndsWith(".unityweb", StringComparison.OrdinalIgnoreCase) || jsPath.EndsWith(".br", StringComparison.OrdinalIgnoreCase)
                    ? new BrotliStream(ms, CompressionMode.Decompress)
                    : ms;
            using (stream)
            {
                // Emscripten emits module preamble + runtime (~200-600 KiB) before `"use asm"`.
                var buf = new byte[1024 * 1024];
                var total = 0;
                while (total < buf.Length)
                {
                    var n = stream.Read(buf, total, buf.Length - total);
                    if (n <= 0) break;
                    total += n;
                }
                var text = Encoding.UTF8.GetString(buf, 0, total);
                return text.Contains("\"use asm\"", StringComparison.Ordinal) || text.Contains("'use asm'", StringComparison.Ordinal);
            }
        }
        catch
        {
            return false;
        }
    }

    private static byte[] ReadMaybeCompressed(string path)
    {
        var raw = File.ReadAllBytes(path);
        if (raw.Length >= 2 && raw[0] == 0x1F && raw[1] == 0x8B)
        {
            using var ms = new MemoryStream(raw);
            using var gz = new GZipStream(ms, CompressionMode.Decompress);
            using var outMs = new MemoryStream();
            gz.CopyTo(outMs);
            return outMs.ToArray();
        }
        if (path.EndsWith(".br", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".unityweb", StringComparison.OrdinalIgnoreCase))
        {
            using var ms = new MemoryStream(raw);
            using var br = new BrotliStream(ms, CompressionMode.Decompress);
            using var outMs = new MemoryStream();
            br.CopyTo(outMs);
            return outMs.ToArray();
        }
        return raw;
    }

    [GeneratedRegex(@"STATIC_BASE\s*=\s*(\d+)", RegexOptions.CultureInvariant)]
    private static partial Regex StaticBaseRegex();

    [GeneratedRegex(@"var\s+([A-Za-z_$][\w$]*)\s*=\s*new\s+global\.(Int8Array|Int16Array|Int32Array|Uint8Array|Uint16Array|Uint32Array|Float32Array|Float64Array)\s*\(\s*buffer\s*\)", RegexOptions.CultureInvariant)]
    private static partial Regex HeapViewRegex();

    [GeneratedRegex(@"var\s+([A-Za-z_$][\w$]*)\s*=\s*env\.STACKTOP\b", RegexOptions.CultureInvariant)]
    private static partial Regex StackTopRegex();

    [GeneratedRegex(@"var\s+([A-Za-z_$][\w$]*)\s*=\s*global\.Math\.fround\b", RegexOptions.CultureInvariant)]
    private static partial Regex FroundRegex();

    [GeneratedRegex(@"var\s+([A-Za-z_$][\w$]*)\s*=\s*global\.Math\.([A-Za-z0-9_]+)\b", RegexOptions.CultureInvariant)]
    private static partial Regex MathAliasRegex();

    [GeneratedRegex(@"(?:var|,)\s*([A-Za-z_$][\w$]*)\s*=\s*(?:0\.0|([A-Za-z_$][\w$]*)\(\s*0(?:\.0)?\s*\))", RegexOptions.CultureInvariant)]
    private static partial Regex FloatZeroVarRegex();

    [GeneratedRegex(@"var\s+([A-Za-z_$][\w$]*)\s*=\s*\[([^\]]+)\]", RegexOptions.CultureInvariant)]
    private static partial Regex TableVarRegex();

    [GeneratedRegex(@"([A-Za-z_$][\w$]*)\s*:\s*([A-Za-z_$][\w$]*)", RegexOptions.CultureInvariant)]
    private static partial Regex ExportPairRegex();

    [GeneratedRegex(@"([A-Za-z_$][\w$]*)\s*\[\s*[A-Za-z_$][\w$]*\s*&\s*(\d+)\s*\]", RegexOptions.CultureInvariant)]
    private static partial Regex DynCallLookupRegex();
}
