using System.Text;
using Iced.Intel;

namespace Il2CppStudio.Core.Disassembly;

/// <summary>x86 (32-bit) / x86-64 text disassembly with Iced, stopping at the first <c>int3</c> padding after a return.</summary>
public sealed class IcedTextDisassembler(int bitness) : ITextDisassembler
{
    public string Name => bitness == 64 ? "x86-64 (Iced)" : "x86 (Iced)";
    public Func<ulong, string?>? SymbolResolver { get; set; }

    public string Disassemble(ReadOnlySpan<byte> code, ulong va)
    {
        var decoder = Iced.Intel.Decoder.Create(bitness, new ByteArrayCodeReader(code.ToArray()), va);
        var formatter = new IntelFormatter();
        formatter.Options.FirstOperandCharIndex = 10;
        var sb = new StringBuilder();
        var output = new StringOutput();
        var end = va + (ulong)code.Length;
        while (decoder.IP < end)
        {
            decoder.Decode(out var ins);
            if (ins.IsInvalid) { sb.AppendLine($"{ins.IP:X8}  (bad)"); continue; }
            formatter.Format(ins, output);
            var text = output.ToStringAndReset();
            if (ins.FlowControl is FlowControl.Call or FlowControl.UnconditionalBranch && ins.Op0Kind is OpKind.NearBranch32 or OpKind.NearBranch64 or OpKind.NearBranch16
                && SymbolResolver?.Invoke(ins.NearBranchTarget) is { } name)
                text += $"    ; {name}";
            sb.Append($"{ins.IP:X8}  ").AppendLine(text);
            if (ins.Mnemonic == Mnemonic.Int3 && decoder.IP < end && code[(int)(decoder.IP - va)] == 0xCC) break;
        }
        return sb.ToString();
    }
}
