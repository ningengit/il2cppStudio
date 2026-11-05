using System.Text;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Decoder = Iced.Intel.Decoder;

namespace Il2CppStudio.Core.Disassembly;

/// <summary>Iced <see cref="CodeReader"/> over a <see cref="ReadOnlyMemory{T}"/> slice of the mapped file (no copy).</summary>
public sealed class MemoryCodeReader(ReadOnlyMemory<byte> code) : CodeReader
{
    private int _pos;
    public override int ReadByte() => _pos < code.Length ? code.Span[_pos++] : -1;
}

public sealed record DisassembledFunction(ulong StartVa, ulong EndVa, IReadOnlyList<Instruction> Instructions, bool Truncated);

/// <summary>x86-64 disassembly of method bodies with IL2CPP-aware symbolization (Intel syntax).</summary>
public sealed class X64Disassembler(NativeSymbolizer symbols)
{
    public NativeSymbolizer Symbols => symbols;

    public DisassembledFunction Disassemble(ulong va, int maxBytes = 0x10000)
    {
        var (start, end) = symbols.Map.GetFunctionBounds(va, maxBytes);
        var reader = symbols.Registration.Reader;
        var length = (int)Math.Min(end - start, (ulong)maxBytes);
        var mem = reader.SliceMemory(start, length);
        var decoder = Decoder.Create(64, new MemoryCodeReader(mem), start, DecoderOptions.None);
        var list = new List<Instruction>(Math.Max(16, length / 4));
        var stop = start + (ulong)mem.Length;
        var truncated = end - start > (ulong)maxBytes;
        while (decoder.IP < stop)
        {
            decoder.Decode(out var instr);
            if (instr.IsInvalid) { truncated = true; break; }
            list.Add(instr);
        }
        return new DisassembledFunction(start, stop, list, truncated);
    }

    public string Format(DisassembledFunction fn, bool withBytes = true)
    {
        var formatter = new IntelFormatter(new Resolver(symbols));
        formatter.Options.FirstOperandCharIndex = 10;
        formatter.Options.SpaceAfterOperandSeparator = true;
        var output = new StringOutput();
        var sb = new StringBuilder();
        var reader = symbols.Registration.Reader;
        var imageBase = reader.ImageBase;
        foreach (var instr in fn.Instructions)
        {
            formatter.Format(instr, output);
            sb.Append($"{instr.IP - imageBase:X8}  ");
            if (withBytes)
            {
                var bytes = reader.Slice(instr.IP, instr.Length);
                sb.Append(Convert.ToHexString(bytes).PadRight(24));
            }
            sb.Append(output.ToStringAndReset());
            var comment = Comment(instr);
            if (comment is not null) sb.Append("    ; ").Append(comment);
            sb.AppendLine();
        }
        if (fn.Truncated) sb.AppendLine("; ... (truncated)");
        return sb.ToString();
    }

    private string? Comment(in Instruction instr)
    {
        if (instr.IsIPRelativeMemoryOperand && symbols.Describe(instr.IPRelativeMemoryAddress) is { } d) return d;
        return null;
    }

    private sealed class Resolver(NativeSymbolizer symbols) : ISymbolResolver
    {
        public bool TryGetSymbol(in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol)
        {
            if (symbols.Map.SymbolAt(address) is { } s)
            {
                symbol = new SymbolResult(address, s.CppSymbol ?? s.DisplayName);
                return true;
            }
            symbol = default;
            return false;
        }
    }
}
