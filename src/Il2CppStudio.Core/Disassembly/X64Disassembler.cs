using System.Buffers.Binary;
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

public sealed record DisassembledFunction(ulong StartVa, ulong EndVa, IReadOnlyList<Instruction> Instructions, bool Truncated, string? DecodeWarning = null, ulong FailedVa = 0, string? FailedBytesHex = null);

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
        var bitness = reader.PointerSize == 8 ? 64 : 32;
        return DisassembleBytes(mem, start, reader.ImageBase, bitness, end - start > (ulong)maxBytes, maxBytes);
    }

    internal static DisassembledFunction DisassembleBytes(ReadOnlyMemory<byte> mem, ulong start, ulong imageBase, int bitness, bool preTruncated = false, int maxBytes = 0x10000)
    {
        var decoder = Decoder.Create(bitness, new MemoryCodeReader(mem), start, DecoderOptions.None);
        var list = new List<Instruction>(Math.Max(16, mem.Length / 4));
        var stop = start + (ulong)mem.Length;
        var truncated = preTruncated;
        string? decodeWarning = truncated ? $"truncated at 0x{maxBytes:X} byte limit" : null;
        ulong failedVa = 0;
        string? failedBytesHex = null;
        var maxBranchTarget = start;
        var maxBtPrefix = new List<ulong>(Math.Max(16, mem.Length / 4));
        while (decoder.IP < stop)
        {
            var insVa = decoder.IP;
            decoder.Decode(out var instr);
            if (instr.IsInvalid)
            {
                var off = (int)(insVa - start);
                var tail = mem.Span[off..];
                if (list.Count > 0 && IsTerminator(list[^1])
                    && (tail[0] is 0x00 or 0xCC || AllPadding(tail) || IsInlineJumpTableTail(tail, insVa, imageBase, start, stop)))
                    break;

                if (TryTrimPostTerminatorJumpTable(list, maxBtPrefix, mem.Span, start, stop, imageBase))
                    break;

                truncated = true;
                failedVa = insVa;
                failedBytesHex = Convert.ToHexString(tail[..Math.Min(16, tail.Length)]);
                decodeWarning = $"decoding stopped at RVA 0x{insVa - imageBase:X} after {list.Count} instruction(s) (invalid opcode or inline data)";
                break;
            }
            if (instr.FlowControl is FlowControl.ConditionalBranch or FlowControl.UnconditionalBranch
                && instr.OpCount >= 1 && instr.Op0Kind is OpKind.NearBranch16 or OpKind.NearBranch32 or OpKind.NearBranch64)
            {
                var bt = instr.NearBranchTarget;
                if (bt >= start && bt < stop && bt > maxBranchTarget)
                    maxBranchTarget = bt;
            }
            list.Add(instr);
            maxBtPrefix.Add(maxBranchTarget);
            if (IsTerminator(instr) && decoder.IP >= maxBranchTarget)
            {
                var off = (int)(decoder.IP - start);
                var tail = mem.Span[off..];
                if (tail.Length > 0 && (AllPadding(tail) || IsInlineJumpTableTail(tail, decoder.IP, imageBase, start, stop)))
                    break;
            }
        }
        return new DisassembledFunction(start, stop, list, truncated, decodeWarning, failedVa, failedBytesHex);
    }

    private static bool TryTrimPostTerminatorJumpTable(
        List<Instruction> list, List<ulong> maxBtPrefix, ReadOnlySpan<byte> mem, ulong start, ulong stop, ulong imageBase)
    {
        for (var k = list.Count - 1; k >= 0; k--)
        {
            var prev = list[k];
            if (!IsTerminator(prev) || prev.NextIP < maxBtPrefix[k]) continue;
            var off = (int)(prev.NextIP - start);
            if ((uint)off >= (uint)mem.Length) continue;
            var tail = mem[off..];
            if (AllPadding(tail) || IsInlineJumpTableTail(tail, prev.NextIP, imageBase, start, stop, minEntries: 2))
            {
                list.RemoveRange(k + 1, list.Count - (k + 1));
                return true;
            }
        }
        // Fallback when invalid opcode was reached after a call (noreturn) or nop alignment immediately preceding >=4 RVAs.
        for (var k = list.Count - 1; k >= 0; k--)
        {
            var prev = list[k];
            if (prev.Mnemonic != Mnemonic.Nop && prev.FlowControl != FlowControl.Call) continue;
            if (prev.NextIP < maxBtPrefix[k]) continue;
            var off = (int)(prev.NextIP - start);
            if ((uint)off >= (uint)mem.Length) continue;
            var tail = mem[off..];
            if (IsInlineJumpTableTail(tail, prev.NextIP, imageBase, start, stop, minEntries: 4))
            {
                list.RemoveRange(k + 1, list.Count - (k + 1));
                return true;
            }
        }
        return false;
    }

    private static bool IsInlineJumpTableTail(
        ReadOnlySpan<byte> tail, ulong tailVa, ulong imageBase, ulong start, ulong stop, int minEntries = 2)
    {
        if (tail.Length < minEntries * 4) return false;
        var padEnd = SkipX64Pad(tail);
        var alignPad = (4 - (int)(tailVa & 3)) & 3;
        for (var cand = alignPad; cand <= padEnd; cand += 4)
        {
            if (cand >= tail.Length) break;
            if (cand > 0 && SkipX64Pad(tail[..cand]) != cand) continue;
            if (IsValidRvaTableAt(tail, cand, tailVa, imageBase, start, stop, minEntries))
                return true;
        }
        return false;
    }

    private static bool IsValidRvaTableAt(
        ReadOnlySpan<byte> tail, int startPos, ulong tailVa, ulong imageBase, ulong start, ulong stop, int minEntries)
    {
        var pos = startPos;
        var tables = 0;
        while (pos < tail.Length)
        {
            var validEntries = 0;
            while (pos + 4 <= tail.Length)
            {
                var rva = BinaryPrimitives.ReadUInt32LittleEndian(tail.Slice(pos, 4));
                var target = imageBase + rva;
                if (target < start || target >= stop)
                    break;
                validEntries++;
                pos += 4;
            }
            if (validEntries < (tables == 0 ? minEntries : 2)) return false;
            tables++;

            // Optional MSVC 1-byte switch index lookup table immediately trailing the 4-byte RVA table.
            if (validEntries <= 255)
            {
                while (pos < tail.Length && tail[pos] < validEntries)
                    pos++;
            }

            if (AllPadding(tail[pos..]))
                return true;

            // Multiple switches in one function tail: skip alignment padding to the next 4-byte aligned RVA table.
            var nextAlign = pos + ((4 - (int)((tailVa + (ulong)pos) & 3)) & 3);
            var padLen = SkipX64Pad(tail[pos..]);
            if (pos + padLen < nextAlign || nextAlign >= tail.Length) return false;
            pos = nextAlign;
        }
        return tables > 0;
    }

    private static bool IsTerminator(in Instruction ins) =>
        ins.FlowControl is FlowControl.Return or FlowControl.UnconditionalBranch or FlowControl.IndirectBranch or FlowControl.Exception
        || ins.Code is Code.Int3 or Code.Ud2;

    private static bool AllPadding(ReadOnlySpan<byte> bytes) => SkipX64Pad(bytes) == bytes.Length;

    private static int SkipX64Pad(ReadOnlySpan<byte> bytes)
    {
        var pos = 0;
        while (pos < bytes.Length)
        {
            var b = bytes[pos];
            if (b is 0x00 or 0xCC or 0x90) { pos++; continue; }
            var rem = bytes.Length - pos;
            if (b == 0x66)
            {
                if (rem >= 2 && bytes[pos + 1] == 0x90) { pos += 2; continue; }
                if (rem >= 3 && bytes[pos + 1] == 0x66 && bytes[pos + 2] == 0x90) { pos += 3; continue; }
                if (rem >= 4 && bytes[pos + 1] == 0x66 && bytes[pos + 2] == 0x66 && bytes[pos + 3] == 0x90) { pos += 4; continue; }
                if (rem >= 6 && bytes[pos + 1] == 0x0F && bytes[pos + 2] == 0x1F && bytes[pos + 3] == 0x44 && bytes[pos + 4] == 0x00 && bytes[pos + 5] == 0x00) { pos += 6; continue; }
                if (rem >= 9 && bytes[pos + 1] == 0x0F && bytes[pos + 2] == 0x1F && bytes[pos + 3] == 0x84 && bytes[pos + 4] == 0x00 && bytes[pos + 5] == 0x00 && bytes[pos + 6] == 0x00 && bytes[pos + 7] == 0x00 && bytes[pos + 8] == 0x00) { pos += 9; continue; }
                break;
            }
            if (b == 0x0F && rem >= 3 && bytes[pos + 1] == 0x1F)
            {
                if (bytes[pos + 2] == 0x00) { pos += 3; continue; }
                if (rem >= 4 && bytes[pos + 2] == 0x40 && bytes[pos + 3] == 0x00) { pos += 4; continue; }
                if (rem >= 5 && bytes[pos + 2] == 0x44 && bytes[pos + 3] == 0x00 && bytes[pos + 4] == 0x00) { pos += 5; continue; }
                if (rem >= 7 && bytes[pos + 2] == 0x80 && bytes[pos + 3] == 0x00 && bytes[pos + 4] == 0x00 && bytes[pos + 5] == 0x00 && bytes[pos + 6] == 0x00) { pos += 7; continue; }
                if (rem >= 8 && bytes[pos + 2] == 0x84 && bytes[pos + 3] == 0x00 && bytes[pos + 4] == 0x00 && bytes[pos + 5] == 0x00 && bytes[pos + 6] == 0x00 && bytes[pos + 7] == 0x00) { pos += 8; continue; }
            }
            break;
        }
        return pos;
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
        if (fn.DecodeWarning is not null) sb.Append("; ").AppendLine(fn.DecodeWarning);
        else if (fn.Truncated) sb.AppendLine("; ... (truncated)");
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
