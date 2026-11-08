using System.Text;
using WasmDisassembler;

namespace Il2CppStudio.Core.Disassembly;

/// <summary>
/// WebAssembly bytecode text for one function body, with the vendored Cpp2IL WasmDisassembler. The body starts with
/// its local declarations (vec of (count, valtype)), which are skipped before decoding instructions. Block nesting is
/// shown by indentation; <c>call N</c> is annotated with a symbol when the resolver knows function N.
/// </summary>
public sealed class WasmTextDisassembler : ITextDisassembler
{
    public string Name => "WebAssembly (WasmDisassembler)";

    /// <summary>Resolves a function index (not a table slot) to a name.</summary>
    public Func<ulong, string?>? SymbolResolver { get; set; }

    public string Disassemble(ReadOnlySpan<byte> code, ulong functionIndex)
    {
        var body = code.ToArray();
        var p = 0;
        var groups = ReadU32(body, ref p);
        var locals = new StringBuilder();
        for (var i = 0; i < groups; i++) { var n = ReadU32(body, ref p); var t = body[p++]; locals.Append($"{n}x{ValType(t)} "); }
        var sb = new StringBuilder();
        sb.AppendLine($"; function {functionIndex}, locals: {(locals.Length == 0 ? "none" : locals.ToString().Trim())}");
        List<WasmInstruction> insns;
        try { insns = WasmDisassembler.Disassembler.Disassemble(body[p..], (uint)p); }
        catch (Exception ex) { return sb.AppendLine($"; decoding failed: {ex.Message}").ToString(); }
        var depth = 1;
        foreach (var ins in insns)
        {
            if (ins.Mnemonic is WasmMnemonic.End or WasmMnemonic.Else) depth = Math.Max(1, depth - 1);
            var ops = ins.Operands is { Length: > 0 } ? " " + string.Join(", ", ins.Operands) : "";
            var line = $"{ins.Ip:X6}  {new string(' ', depth * 2)}{ins.Mnemonic.ToString().ToLowerInvariant()}{ops}";
            if (ins.Mnemonic == WasmMnemonic.Call && ins.Operands is [var target, ..] && SymbolResolver?.Invoke(Convert.ToUInt64(target)) is { } name)
                line += $"    ; {name}";
            sb.AppendLine(line);
            if (ins.Mnemonic is WasmMnemonic.Block or WasmMnemonic.Loop or WasmMnemonic.If or WasmMnemonic.Else) depth++;
        }
        return sb.ToString();
    }

    private static string ValType(byte t) => t switch { 0x7F => "i32", 0x7E => "i64", 0x7D => "f32", 0x7C => "f64", 0x7B => "v128", _ => $"0x{t:X2}" };

    private static uint ReadU32(byte[] d, ref int p)
    {
        uint r = 0; var s = 0; byte b;
        do { b = d[p++]; r |= (uint)(b & 0x7F) << s; s += 7; } while ((b & 0x80) != 0);
        return r;
    }
}
