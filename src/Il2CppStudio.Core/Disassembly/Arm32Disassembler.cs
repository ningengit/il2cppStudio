using System.Globalization;
using System.Text;

namespace Il2CppStudio.Core.Disassembly;

public readonly record struct Arm32Insn(ulong Address, int Size, string Mnemonic, string Operands);

/// <summary>
/// 100% pure-C#, MIT-licensed ARMv7-A (A32) and Thumb/Thumb-2 (T16/T32 + VFPv3) disassembler.
/// Replaces native <c>capstone.dll</c> across all desktop and mobile targets with zero native dependencies.
/// </summary>
public sealed class Arm32Disassembler : ITextDisassembler
{
    private string _lastMode = "Thumb-2";

    public string Name => $"ARMv7 (Pure C#, {_lastMode})";
    public Func<ulong, string?>? SymbolResolver { get; set; }

    /// <summary>
    /// Disassembles [va, va + code.Length). If bit 0 of <paramref name="va"/> was cleared before reaching
    /// the disassembler, both Thumb-2 and A32 decoders run and the highest-confidence decode wins.
    /// </summary>
    public string Disassemble(ReadOnlySpan<byte> code, ulong va)
    {
        var insns = DisassembleInstructions(code, va, out var isThumb, out var decodedBytes);
        _lastMode = isThumb ? "Thumb-2" : "A32";

        var sb = new StringBuilder(insns.Count * 40);
        foreach (var ins in insns)
        {
            var line = string.IsNullOrEmpty(ins.Operands)
                ? $"{ins.Address:X8}  {ins.Mnemonic}"
                : $"{ins.Address:X8}  {ins.Mnemonic,-10}{ins.Operands}";
            if (TryResolveBranchComment(ins) is { } name)
                line += $"    ; {name}";
            sb.AppendLine(line);
        }
        if (insns.Count == 0 || decodedBytes + 2 < code.Length - 4)
            sb.AppendLine($"; decoding stopped after {insns.Count} instruction(s) (data or unsupported encoding follows)");
        return sb.ToString();
    }

    public IReadOnlyList<Arm32Insn> DisassembleInstructions(ReadOnlySpan<byte> code, ulong va, out bool isThumb) =>
        DisassembleInstructions(code, va, out isThumb, out _);

    public IReadOnlyList<Arm32Insn> DisassembleInstructions(ReadOnlySpan<byte> code, ulong va, out bool isThumb, out int decodedBytes)
    {
        if (code.IsEmpty)
        {
            isThumb = false;
            decodedBytes = 0;
            _lastMode = "A32";
            return [];
        }

        if ((va & 1) != 0)
        {
            var thumbInsns = Arm32ThumbDecoder.DecodeSequence(code, va & ~1UL, out decodedBytes);
            isThumb = true;
            _lastMode = "Thumb-2";
            return thumbInsns;
        }

        var armInsns = Arm32A32Decoder.DecodeSequence(code, va, out var armBytes);
        var thumbCand = Arm32ThumbDecoder.DecodeSequence(code, va, out var thumbBytes);

        var armScore = ScoreA32(code, armInsns, armBytes);
        var thumbScore = ScoreThumb(code, thumbCand, thumbBytes);

        isThumb = thumbScore > armScore;
        decodedBytes = isThumb ? thumbBytes : armBytes;
        _lastMode = isThumb ? "Thumb-2" : "A32";
        return isThumb ? thumbCand : armInsns;
    }

    private static int ScoreA32(ReadOnlySpan<byte> code, List<Arm32Insn> insns, int decodedBytes)
    {
        if (insns.Count == 0 || code.Length < 4) return int.MinValue / 2;

        // Strong A32 signal: unconditional (cond == 0xE) word density and standard AAPCS32 prologues.
        var score = Math.Min(decodedBytes, code.Length) * 2;
        var word0 = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(code);
        var cond0 = word0 >> 28;
        if (cond0 == 0xE || (word0 & 0xFE000000u) == 0xFA000000u)
            score += 40;
        else
            score -= 60;

        var m0 = insns[0].Mnemonic;
        if (m0 is "push" or "vpush" or "mov" or "movw" or "sub" or "add" or "ldr" or "b" or "bx")
            score += 24;

        var alCount = 0;
        var suspiciousCount = 0;
        var checkWords = Math.Min(insns.Count, 32);
        for (var i = 0; i < checkWords; i++)
        {
            var off = i * 4;
            if (off + 4 > code.Length) break;
            var w = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(code.Slice(off, 4));
            if ((w >> 28) == 0xE || (w & 0xFE000000u) == 0xFA000000u)
                alCount++;
            if (w == 0x00000000 || insns[i].Mnemonic.StartsWith("svc", StringComparison.Ordinal))
                suspiciousCount++;
        }

        if (checkWords > 0)
            score += (alCount * 64) / checkWords;
        score -= suspiciousCount * 16;

        // Bonus if we reached a clean function return / tail branch.
        for (var i = 0; i < insns.Count; i++)
        {
            if (IsReturnInsn(insns[i])) { score += 18; break; }
        }
        return score;
    }

    private static int ScoreThumb(ReadOnlySpan<byte> code, List<Arm32Insn> insns, int decodedBytes)
    {
        if (insns.Count == 0 || code.Length < 2) return int.MinValue / 2;

        var score = Math.Min(decodedBytes, code.Length) * 2;
        var hw0 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(code);
        var m0 = insns[0].Mnemonic;

        // Classic Thumb-1 push {..., lr} (0xB5xx / 0xB4xx) or Thumb-2 push.w (0xE92D) or cbz/movs prologue.
        if ((hw0 & 0xFE00) == 0xB400 || m0 is "push" or "movs" or "cbz" or "cbnz")
            score += 36;
        if (m0.StartsWith("svc", StringComparison.Ordinal) || m0 == "bkpt")
            score -= 40;

        // Penalize spurious unconditional short `b` in the first 3 instructions when no conditional precedes it,
        // which commonly appears when an A32 0xE... top halfword is misread as 16-bit Thumb `b #imm`.
        var weirdBranches = 0;
        var suspiciousOps = 0;
        var check = Math.Min(insns.Count, 24);
        for (var i = 0; i < check; i++)
        {
            var m = insns[i].Mnemonic;
            if (m == "b" && i < 2 && insns.Count > 4) weirdBranches++;
            if (m.StartsWith("svc", StringComparison.Ordinal) || m == "bkpt" || m == "udf") suspiciousOps++;
        }
        score -= weirdBranches * 28;
        score -= suspiciousOps * 20;

        for (var i = 0; i < insns.Count; i++)
        {
            if (IsReturnInsn(insns[i])) { score += 16; break; }
        }
        return score;
    }

    private static bool IsReturnInsn(in Arm32Insn ins) =>
        (ins.Mnemonic == "bx" && ins.Operands.Equals("lr", StringComparison.Ordinal))
        || (ins.Mnemonic.StartsWith("pop", StringComparison.Ordinal) && ins.Operands.Contains("pc", StringComparison.Ordinal));

    private string? TryResolveBranchComment(in Arm32Insn ins)
    {
        if (SymbolResolver is null || !ins.Operands.StartsWith('#')) return null;
        if (!(ins.Mnemonic.StartsWith("bl", StringComparison.Ordinal) || ins.Mnemonic == "b")) return null;
        var span = ins.Operands.AsSpan(1).Trim();
        if (span.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            && ulong.TryParse(span[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var target))
            return SymbolResolver(target);
        return null;
    }
}
