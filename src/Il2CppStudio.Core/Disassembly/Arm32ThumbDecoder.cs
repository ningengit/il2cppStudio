using System.Buffers.Binary;

namespace Il2CppStudio.Core.Disassembly;

/// <summary>
/// Pure-C# Thumb-1 (16-bit T16) and Thumb-2 (32-bit T32 + VFPv3 + IT block condition) instruction decoder.
/// </summary>
internal static class Arm32ThumbDecoder
{
    public static List<Arm32Insn> DecodeSequence(ReadOnlySpan<byte> code, ulong va, out int decodedBytes)
    {
        var list = new List<Arm32Insn>(Math.Max(1, code.Length / 2));
        var pos = 0;
        var itState = 0; // Thumb IT block state bits [7:4]=base cond, [3:0]=mask

        while (pos + 2 <= code.Length)
        {
            var hw1 = BinaryPrimitives.ReadUInt16LittleEndian(code.Slice(pos, 2));
            var addr = va + (ulong)pos;
            var is32 = (hw1 >> 11) >= 0x1D;

            var itCond = PopItCondition(ref itState);
            if (!is32)
            {
                if (!TryDecodeT16(hw1, addr, itCond, ref itState, out var insn))
                    break;
                list.Add(insn);
                pos += 2;
                if (insn.Mnemonic is "udf" or "bkpt")
                    break;
            }
            else
            {
                if (pos + 4 > code.Length)
                    break;
                var hw2 = BinaryPrimitives.ReadUInt16LittleEndian(code.Slice(pos + 2, 2));
                if (!TryDecodeT32(hw1, hw2, addr, itCond, out var insn))
                    break;
                list.Add(insn);
                pos += 4;
                if (insn.Mnemonic is "udf" or "bkpt")
                    break;
            }
        }

        decodedBytes = pos;
        return list;
    }

    private static string PopItCondition(ref int itState)
    {
        if ((itState & 0xF) == 0) return "";
        var baseCond = (itState >> 4) & 0xF;
        var cond = Arm32A32Decoder.CondSuffixes[baseCond];
        // Shift low 4 bits left by 1 (next instruction flips bit0 depending on bit4 vs bit0, managed during IT push).
        var nextMask = (itState & 0xF) << 1;
        if ((nextMask & 0xF) == 0)
        {
            itState = 0;
        }
        else
        {
            var nextCond0 = (nextMask >> 4) & 1;
            baseCond = (baseCond & ~1) | nextCond0;
            itState = (baseCond << 4) | (nextMask & 0xF);
        }
        return cond;
    }

    private static bool TryDecodeT16(ushort hw, ulong addr, string itCond, ref int itState, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var inIt = itCond.Length > 0;
        var sfx = inIt ? "" : "s";

        // 00xx: Shift by imm / ADD / SUB
        if (hw < 0x2000)
        {
            var op = (hw >> 11) & 3;
            var rd = hw & 7;
            var rm = (hw >> 3) & 7;
            var imm5 = (hw >> 6) & 0x1F;
            if (op < 3)
            {
                if (op == 0 && imm5 == 0)
                {
                    insn = new Arm32Insn(addr, 2, (inIt ? "mov" : "movs") + itCond, $"{r[rd]}, {r[rm]}");
                    return true;
                }
                var sh = op == 0 ? "lsl" : op == 1 ? "lsr" : "asr";
                var amt = imm5 == 0 && op is 1 or 2 ? 32 : imm5;
                insn = new Arm32Insn(addr, 2, $"{sh}{sfx}{itCond}", $"{r[rd]}, {r[rm]}, #{amt}");
                return true;
            }
            var opc = (hw >> 9) & 3;
            var rnOrImm = (hw >> 6) & 7;
            var baseOp = (opc & 1) == 0 ? "add" : "sub";
            var arg3 = (opc & 2) == 0 ? r[rnOrImm] : $"#{rnOrImm}";
            insn = new Arm32Insn(addr, 2, $"{baseOp}{sfx}{itCond}", $"{r[rd]}, {r[rm]}, {arg3}");
            return true;
        }

        // 001x: MOV / CMP / ADD / SUB imm8
        if (hw < 0x4000)
        {
            var op = (hw >> 11) & 3;
            var rd = (hw >> 8) & 7;
            var imm8 = (uint)(hw & 0xFF);
            var mnem = op switch
            {
                0 => "mov" + sfx + itCond,
                1 => "cmp" + itCond,
                2 => "add" + sfx + itCond,
                _ => "sub" + sfx + itCond,
            };
            insn = new Arm32Insn(addr, 2, mnem, $"{r[rd]}, {Arm32A32Decoder.FormatImm(imm8)}");
            return true;
        }

        // 010000: ALU reg-reg
        if (hw < 0x4400)
        {
            var op = (hw >> 6) & 0xF;
            var rd = hw & 7;
            var rm = (hw >> 3) & 7;
            var mnemBase = op switch
            {
                0 => "and", 1 => "eor", 2 => "lsl", 3 => "lsr",
                4 => "asr", 5 => "adc", 6 => "sbc", 7 => "ror",
                8 => "tst", 9 => "rsb", 10 => "cmp", 11 => "cmn",
                12 => "orr", 13 => "mul", 14 => "bic", _ => "mvn",
            };
            if (op is 8 or 10 or 11)
            {
                insn = new Arm32Insn(addr, 2, mnemBase + itCond, $"{r[rd]}, {r[rm]}");
                return true;
            }
            if (op == 9)
            {
                insn = new Arm32Insn(addr, 2, $"rsb{sfx}{itCond}", $"{r[rd]}, {r[rm]}, #0");
                return true;
            }
            if (op == 15)
            {
                insn = new Arm32Insn(addr, 2, $"mvn{sfx}{itCond}", $"{r[rd]}, {r[rm]}");
                return true;
            }
            if (op == 13)
            {
                insn = new Arm32Insn(addr, 2, $"mul{sfx}{itCond}", $"{r[rd]}, {r[rm]}, {r[rd]}");
                return true;
            }
            insn = new Arm32Insn(addr, 2, $"{mnemBase}{sfx}{itCond}", $"{r[rd]}, {r[rd]}, {r[rm]}");
            return true;
        }

        // 010001: Special data instructions & branch exchange
        if (hw < 0x4800)
        {
            var op = (hw >> 8) & 3;
            var d = ((hw >> 7) & 1) << 3;
            var rd = d | (hw & 7);
            var rm = (hw >> 3) & 0xF;
            switch (op)
            {
                case 0: insn = new Arm32Insn(addr, 2, "add" + itCond, $"{r[rd]}, {r[rd]}, {r[rm]}"); return true;
                case 1: insn = new Arm32Insn(addr, 2, "cmp" + itCond, $"{r[rd]}, {r[rm]}"); return true;
                case 2: insn = new Arm32Insn(addr, 2, "mov" + itCond, $"{r[rd]}, {r[rm]}"); return true;
                default:
                    var link = ((hw >> 7) & 1) != 0;
                    insn = new Arm32Insn(addr, 2, (link ? "blx" : "bx") + itCond, r[rm]);
                    return true;
            }
        }

        // 01001: LDR literal [pc, #imm8*4]
        if (hw < 0x5000)
        {
            var rt = (hw >> 8) & 7;
            var imm = (uint)((hw & 0xFF) << 2);
            insn = new Arm32Insn(addr, 2, "ldr" + itCond, $"{r[rt]}, {(imm == 0 ? "[pc]" : $"[pc, {Arm32A32Decoder.FormatImm(imm)}]")}");
            return true;
        }

        // 0101: Load/store register offset
        if (hw < 0x6000)
        {
            var op = (hw >> 9) & 7;
            var rt = hw & 7;
            var rn = (hw >> 3) & 7;
            var rm = (hw >> 6) & 7;
            var mnem = op switch
            {
                0 => "str", 1 => "strh", 2 => "strb", 3 => "ldrsb",
                4 => "ldr", 5 => "ldrh", 6 => "ldrb", _ => "ldrsh",
            };
            insn = new Arm32Insn(addr, 2, mnem + itCond, $"{r[rt]}, [{r[rn]}, {r[rm]}]");
            return true;
        }

        // 0110/0111/1000: Load/store word/byte/halfword imm5
        if (hw < 0x9000)
        {
            var rt = hw & 7;
            var rn = (hw >> 3) & 7;
            var imm5 = (hw >> 6) & 0x1F;
            string mnem;
            uint off;
            if (hw < 0x7000) { mnem = ((hw >> 11) & 1) != 0 ? "ldr" : "str"; off = (uint)(imm5 << 2); }
            else if (hw < 0x8000) { mnem = ((hw >> 11) & 1) != 0 ? "ldrb" : "strb"; off = (uint)imm5; }
            else { mnem = ((hw >> 11) & 1) != 0 ? "ldrh" : "strh"; off = (uint)(imm5 << 1); }

            insn = new Arm32Insn(addr, 2, mnem + itCond, $"{r[rt]}, {(off == 0 ? $"[{r[rn]}]" : $"[{r[rn]}, {Arm32A32Decoder.FormatImm(off)}]")}");
            return true;
        }

        // 1001: SP-relative LDR/STR
        if (hw < 0xA000)
        {
            var l = ((hw >> 11) & 1) != 0;
            var rt = (hw >> 8) & 7;
            var off = (uint)((hw & 0xFF) << 2);
            insn = new Arm32Insn(addr, 2, (l ? "ldr" : "str") + itCond, $"{r[rt]}, {(off == 0 ? "[sp]" : $"[sp, {Arm32A32Decoder.FormatImm(off)}]")}");
            return true;
        }

        // 1010: ADR / ADD SP imm8*4
        if (hw < 0xB000)
        {
            var isSp = ((hw >> 11) & 1) != 0;
            var rd = (hw >> 8) & 7;
            var off = (uint)((hw & 0xFF) << 2);
            insn = new Arm32Insn(addr, 2, "add" + itCond, $"{r[rd]}, {(isSp ? "sp" : "pc")}, {Arm32A32Decoder.FormatImm(off)}");
            return true;
        }

        // 1011: Miscellaneous 16-bit
        if (hw < 0xC000)
        {
            // IT block (1011_1111_cccc_mmmm, mask != 0)
            if ((hw & 0xFF00) == 0xBF00)
            {
                var mask = hw & 0xF;
                if (mask == 0)
                {
                    var h = (hw >> 4) & 0xF;
                    var hint = h switch { 0 => "nop", 1 => "yield", 2 => "wfe", 3 => "wfi", 4 => "sev", _ => null };
                    if (hint is null) return false;
                    insn = new Arm32Insn(addr, 2, hint + itCond, "");
                    return true;
                }
                var firstCond = (hw >> 4) & 0xF;
                if (firstCond == 15) return false;
                // Compute IT mnemonic suffix (t/e) and prime itState
                var count = (mask & 1) != 0 ? 4 : (mask & 2) != 0 ? 3 : (mask & 4) != 0 ? 2 : 1;
                var letters = "";
                var fc0 = firstCond & 1;
                for (var k = 1; k < count; k++)
                {
                    var bit = (mask >> (4 - k)) & 1;
                    letters += bit == fc0 ? 't' : 'e';
                }
                itState = (firstCond << 4) | mask;
                insn = new Arm32Insn(addr, 2, "it" + letters, Arm32A32Decoder.CondSuffixes[firstCond]);
                return true;
            }

            // Adjust SP
            if ((hw & 0xFF00) == 0xB000)
            {
                var isSub = ((hw >> 7) & 1) != 0;
                var off = (uint)((hw & 0x7F) << 2);
                insn = new Arm32Insn(addr, 2, (isSub ? "sub" : "add") + itCond, $"sp, sp, {Arm32A32Decoder.FormatImm(off)}");
                return true;
            }

            // CBZ / CBNZ
            if ((hw & 0xF500) == 0xB100)
            {
                var nz = ((hw >> 11) & 1) != 0;
                var rn = hw & 7;
                var i = ((hw >> 9) & 1) << 6;
                var imm5 = ((hw >> 3) & 0x1F) << 1;
                var target = addr + 4 + (ulong)(i | imm5);
                insn = new Arm32Insn(addr, 2, nz ? "cbnz" : "cbz", $"{r[rn]}, #0x{target:x}");
                return true;
            }

            // PUSH / POP
            if ((hw & 0xF600) == 0xB400)
            {
                var isPop = ((hw >> 11) & 1) != 0;
                var m = (hw >> 8) & 1;
                var mask = (ushort)((hw & 0xFF) | (m << (isPop ? 15 : 14)));
                insn = new Arm32Insn(addr, 2, (isPop ? "pop" : "push") + itCond, $"{{{Arm32A32Decoder.FormatRegList(mask)}}}");
                return true;
            }

            // SXTH / SXTB / UXTH / UXTB
            if ((hw & 0xFF00) == 0xB200)
            {
                var op = (hw >> 6) & 3;
                var rd = hw & 7;
                var rm = (hw >> 3) & 7;
                var ext = op switch { 0 => "sxth", 1 => "sxtb", 2 => "uxth", _ => "uxtb" };
                insn = new Arm32Insn(addr, 2, ext + itCond, $"{r[rd]}, {r[rm]}");
                return true;
            }

            // REV / REV16 / REVSH
            if ((hw & 0xFF00) == 0xBA00)
            {
                var op = (hw >> 6) & 3;
                if (op == 2) return false;
                var rd = hw & 7;
                var rm = (hw >> 3) & 7;
                var rv = op switch { 0 => "rev", 1 => "rev16", _ => "revsh" };
                insn = new Arm32Insn(addr, 2, rv + itCond, $"{r[rd]}, {r[rm]}");
                return true;
            }

            // BKPT
            if ((hw & 0xFF00) == 0xBE00)
            {
                insn = new Arm32Insn(addr, 2, "bkpt", $"#0x{hw & 0xFF:x}");
                return true;
            }
            return false;
        }

        // 1100: STM / LDM
        if (hw < 0xD000)
        {
            var l = ((hw >> 11) & 1) != 0;
            var rn = (hw >> 8) & 7;
            var mask = (ushort)(hw & 0xFF);
            var wb = !l || (mask & (1 << rn)) == 0 ? "!" : "";
            insn = new Arm32Insn(addr, 2, (l ? "ldm" : "stm") + itCond, $"{r[rn]}{wb}, {{{Arm32A32Decoder.FormatRegList(mask)}}}");
            return true;
        }

        // 1101: Conditional branch / UDF / SVC
        if (hw < 0xE000)
        {
            var cond = (hw >> 8) & 0xF;
            if (cond == 14)
            {
                insn = new Arm32Insn(addr, 2, "udf", $"#0x{hw & 0xFF:x}");
                return true;
            }
            if (cond == 15)
            {
                insn = new Arm32Insn(addr, 2, "svc", $"#0x{hw & 0xFF:x}");
                return true;
            }
            var imm8 = (sbyte)(hw & 0xFF);
            var target = (ulong)((long)addr + 4 + (imm8 << 1));
            insn = new Arm32Insn(addr, 2, "b" + Arm32A32Decoder.CondSuffixes[cond], $"#0x{target:x}");
            return true;
        }

        // 11100: Unconditional branch
        if ((hw >> 11) == 0x1C)
        {
            var imm11 = hw & 0x7FF;
            var sext = ((int)imm11 << 21) >> 21;
            var target = (ulong)((long)addr + 4 + (sext << 1));
            insn = new Arm32Insn(addr, 2, "b" + itCond, $"#0x{target:x}");
            return true;
        }

        return false;
    }

    private static bool TryDecodeT32(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var op1 = (hw1 >> 11) & 3; // 1=0xE800..0xEFFF, 2=0xF000..0xF7FF, 3=0xF800..0xFFFF

        // 0xEC00..0xEFFF: Coprocessor / VFPv3 / SIMD
        if ((hw1 & 0xFC00) == 0xEC00)
        {
            var equivWord = 0xE0000000u | ((uint)(hw1 & 0x0FFF) << 16) | hw2;
            return Arm32A32Decoder.TryDecodeVfp(equivWord, addr, itCond, out insn);
        }

        // 0xEA00..0xEBFF: Data processing (shifted register)
        if ((hw1 & 0xFE00) == 0xEA00)
            return DecodeT32DataProcShiftedReg(hw1, hw2, addr, itCond, out insn);

        // 0xE800..0xE9FF: Load/store multiple, dual, exclusive, table branch
        if ((hw1 & 0xFE00) == 0xE800)
        {
            var mode = (hw1 >> 7) & 3;
            var wBit = (hw1 >> 5) & 1;
            var opBit6 = (hw1 >> 6) & 1;

            // Load/store multiple when P/U mode is 01 (IA) or 10 (DB) and bit6 == 0
            if (opBit6 == 0 && mode is 1 or 2)
            {
                var w = wBit != 0;
                var l = ((hw1 >> 4) & 1) != 0;
                var rn = hw1 & 0xF;
                var mask = hw2;
                if (mask == 0) return false;
                var wb = w ? "!" : "";
                if (rn == 13 && w)
                {
                    if (!l && mode == 2) { insn = new Arm32Insn(addr, 4, "push" + itCond, $"{{{Arm32A32Decoder.FormatRegList(mask)}}}"); return true; }
                    if (l && mode == 1) { insn = new Arm32Insn(addr, 4, "pop" + itCond, $"{{{Arm32A32Decoder.FormatRegList(mask)}}}"); return true; }
                }
                var baseName = (l ? "ldm" : "stm") + (mode == 1 ? "" : "db") + itCond;
                insn = new Arm32Insn(addr, 4, baseName, $"{r[rn]}{wb}, {{{Arm32A32Decoder.FormatRegList(mask)}}}");
                return true;
            }

            // Load/store exclusive: STREX / LDREX (op2 bits: P=0, U=0, W=0 -> 0xE84x/0xE85x) or LDREXB/H/D (0xE8Cx/0xE8Dx)
            if ((hw1 & 0x01A0) == 0x0040)
            {
                var l = ((hw1 >> 4) & 1) != 0;
                var rn = hw1 & 0xF;
                var rt = (hw2 >> 12) & 0xF;
                if ((hw1 & 0x0040) != 0 && (hw1 & 0x0080) == 0)
                {
                    var imm = (uint)((hw2 & 0xFF) << 2);
                    var off = imm == 0 ? $"[{r[rn]}]" : $"[{r[rn]}, {Arm32A32Decoder.FormatImm(imm)}]";
                    insn = l
                        ? new Arm32Insn(addr, 4, "ldrex" + itCond, $"{r[rt]}, {off}")
                        : new Arm32Insn(addr, 4, "strex" + itCond, $"{r[(hw2 >> 8) & 0xF]}, {r[rt]}, {off}");
                    return true;
                }
                var op3 = (hw2 >> 4) & 0xF;
                if (op3 == 4)
                {
                    insn = new Arm32Insn(addr, 4, (l ? "ldrexb" : "strexb") + itCond, l ? $"{r[rt]}, [{r[rn]}]" : $"{r[hw2 & 0xF]}, {r[rt]}, [{r[rn]}]");
                    return true;
                }
                if (op3 == 5)
                {
                    insn = new Arm32Insn(addr, 4, (l ? "ldrexh" : "strexh") + itCond, l ? $"{r[rt]}, [{r[rn]}]" : $"{r[hw2 & 0xF]}, {r[rt]}, [{r[rn]}]");
                    return true;
                }
                if (op3 == 7)
                {
                    var rt2 = (hw2 >> 8) & 0xF;
                    insn = new Arm32Insn(addr, 4, (l ? "ldrexd" : "strexd") + itCond, l ? $"{r[rt]}, {r[rt2]}, [{r[rn]}]" : $"{r[hw2 & 0xF]}, {r[rt]}, {r[rt2]}, [{r[rn]}]");
                    return true;
                }
            }

            // TBB / TBH
            if ((hw1 & 0x0FF0) == 0x08D0 && (hw2 & 0xFFE0) == 0xF000)
            {
                var rn = hw1 & 0xF;
                var rm = hw2 & 0xF;
                var h = ((hw2 >> 4) & 1) != 0;
                insn = new Arm32Insn(addr, 4, (h ? "tbh" : "tbb") + itCond, h ? $"[{r[rn]}, {r[rm]}, lsl #1]" : $"[{r[rn]}, {r[rm]}]");
                return true;
            }

            // LDRD / STRD
            if ((hw1 & 0x0040) != 0 || ((hw1 >> 7) & 1) != 0)
            {
                var p = ((hw1 >> 8) & 1) != 0;
                var u = ((hw1 >> 7) & 1) != 0;
                var w = ((hw1 >> 5) & 1) != 0;
                var l = ((hw1 >> 4) & 1) != 0;
                var rn2 = hw1 & 0xF;
                var rt = (hw2 >> 12) & 0xF;
                var rt2 = (hw2 >> 8) & 0xF;
                var imm = (hw2 & 0xFF) << 2;
                var offText = imm == 0 ? "" : u ? $", {Arm32A32Decoder.FormatImm((uint)imm)}" : $", #-0x{imm:x}";
                var addrText = p ? (w ? $"[{r[rn2]}{offText}]!" : $"[{r[rn2]}{offText}]") : $"[{r[rn2]}], {(u ? Arm32A32Decoder.FormatImm((uint)imm) : $"#-0x{imm:x}")}";
                insn = new Arm32Insn(addr, 4, (l ? "ldrd" : "strd") + itCond, $"{r[rt]}, {r[rt2]}, {addrText}");
                return true;
            }
            return false;
        }

        // 0xF000..0xF7FF
        if (op1 == 2)
        {
            if ((hw2 & 0x8000) != 0)
                return DecodeT32BranchAndMisc(hw1, hw2, addr, itCond, out insn);

            if ((hw1 & 0x0200) == 0)
                return DecodeT32DataProcModImm(hw1, hw2, addr, itCond, out insn);

            return DecodeT32DataProcPlainImm(hw1, hw2, addr, itCond, out insn);
        }

        // 0xF800..0xFFFF
        if ((hw1 & 0xFE00) == 0xF800)
            return DecodeT32LoadStoreSingle(hw1, hw2, addr, itCond, out insn);

        if ((hw1 & 0xFF00) == 0xF900)
            return DecodeT32LoadStoreSingle(hw1, hw2, addr, itCond, out insn);

        if ((hw1 & 0xFF00) == 0xFA00)
            return DecodeT32DataProcReg(hw1, hw2, addr, itCond, out insn);

        if ((hw1 & 0xFF00) == 0xFB00)
            return DecodeT32MultiplyDivide(hw1, hw2, addr, itCond, out insn);

        return false;
    }

    private static bool DecodeT32DataProcShiftedReg(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var op = (hw1 >> 5) & 0xF;
        var s = ((hw1 >> 4) & 1) != 0;
        var rn = hw1 & 0xF;
        var rd = (hw2 >> 8) & 0xF;
        var rm = hw2 & 0xF;
        var imm5 = (((hw2 >> 12) & 7) << 2) | ((hw2 >> 6) & 3);
        var st = (hw2 >> 4) & 3;
        var sfx = s ? "s" : "";

        if (op == 2 && rn == 0xF) // MOV / shift alias
        {
            if (imm5 == 0 && st == 0)
            {
                insn = new Arm32Insn(addr, 4, $"mov{sfx}{itCond}", $"{r[rd]}, {r[rm]}");
                return true;
            }
            var shName = st switch { 0 => "lsl", 1 => "lsr", 2 => "asr", _ => imm5 == 0 ? "rrx" : "ror" };
            var amt = imm5 == 0 && st is 1 or 2 ? 32 : imm5;
            insn = new Arm32Insn(addr, 4, $"{shName}{sfx}{itCond}", st == 3 && imm5 == 0 ? $"{r[rd]}, {r[rm]}" : $"{r[rd]}, {r[rm]}, #{amt}");
            return true;
        }
        if (op == 3 && rn == 0xF) // MVN
        {
            insn = new Arm32Insn(addr, 4, $"mvn{sfx}{itCond}", $"{r[rd]}, {FormatThumbShift(rm, st, imm5)}");
            return true;
        }

        var rmExpr = FormatThumbShift(rm, st, imm5);
        if (rd == 0xF && s && op is 0 or 4 or 8 or 13)
        {
            var testMnem = op switch { 0 => "tst", 4 => "teq", 8 => "cmn", _ => "cmp" };
            insn = new Arm32Insn(addr, 4, testMnem + itCond, $"{r[rn]}, {rmExpr}");
            return true;
        }

        var opName = op switch
        {
            0 => "and", 1 => "bic", 2 => "orr", 3 => "orn",
            4 => "eor", 8 => "add", 10 => "adc", 11 => "sbc",
            13 => "sub", 14 => "rsb", _ => null,
        };
        if (opName is null) return false;
        insn = new Arm32Insn(addr, 4, $"{opName}{sfx}{itCond}", $"{r[rd]}, {r[rn]}, {rmExpr}");
        return true;
    }

    private static bool DecodeT32DataProcModImm(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var op = (hw1 >> 5) & 0xF;
        var s = ((hw1 >> 4) & 1) != 0;
        var rn = hw1 & 0xF;
        var rd = (hw2 >> 8) & 0xF;
        var imm12 = (((hw1 >> 10) & 1) << 11) | (((hw2 >> 12) & 7) << 8) | (hw2 & 0xFF);
        var imm = ThumbExpandImm(imm12);
        var immStr = Arm32A32Decoder.FormatImm(imm);
        var sfx = s ? "s" : "";

        if (op is 2 or 3 && rn == 0xF)
        {
            insn = new Arm32Insn(addr, 4, $"{(op == 2 ? "mov" : "mvn")}{sfx}{itCond}", $"{r[rd]}, {immStr}");
            return true;
        }
        if (rd == 0xF && s && op is 0 or 4 or 8 or 13)
        {
            var testMnem = op switch { 0 => "tst", 4 => "teq", 8 => "cmn", _ => "cmp" };
            insn = new Arm32Insn(addr, 4, testMnem + itCond, $"{r[rn]}, {immStr}");
            return true;
        }
        var opName = op switch
        {
            0 => "and", 1 => "bic", 2 => "orr", 3 => "orn",
            4 => "eor", 8 => "add", 10 => "adc", 11 => "sbc",
            13 => "sub", 14 => "rsb", _ => null,
        };
        if (opName is null) return false;
        insn = new Arm32Insn(addr, 4, $"{opName}{sfx}{itCond}", $"{r[rd]}, {r[rn]}, {immStr}");
        return true;
    }

    private static bool DecodeT32DataProcPlainImm(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var op = (hw1 >> 4) & 0x1F;
        var rn = hw1 & 0xF;
        var rd = (hw2 >> 8) & 0xF;
        var imm16 = (uint)(((hw1 & 0xF) << 12) | (((hw1 >> 10) & 1) << 11) | (((hw2 >> 12) & 7) << 8) | (hw2 & 0xFF));
        var imm12 = (uint)((((hw1 >> 10) & 1) << 11) | (((hw2 >> 12) & 7) << 8) | (hw2 & 0xFF));

        switch (op)
        {
            case 0x00:
                if (rn == 0xF) { insn = new Arm32Insn(addr, 4, "add" + itCond, $"{r[rd]}, pc, {Arm32A32Decoder.FormatImm(imm12)}"); return true; }
                insn = new Arm32Insn(addr, 4, "add" + itCond, $"{r[rd]}, {r[rn]}, {Arm32A32Decoder.FormatImm(imm12)}");
                return true;
            case 0x04:
                insn = new Arm32Insn(addr, 4, "movw" + itCond, $"{r[rd]}, {Arm32A32Decoder.FormatImm(imm16)}");
                return true;
            case 0x0A:
                if (rn == 0xF) { insn = new Arm32Insn(addr, 4, "sub" + itCond, $"{r[rd]}, pc, {Arm32A32Decoder.FormatImm(imm12)}"); return true; }
                insn = new Arm32Insn(addr, 4, "sub" + itCond, $"{r[rd]}, {r[rn]}, {Arm32A32Decoder.FormatImm(imm12)}");
                return true;
            case 0x0C:
                insn = new Arm32Insn(addr, 4, "movt" + itCond, $"{r[rd]}, {Arm32A32Decoder.FormatImm(imm16)}");
                return true;
            case 0x14 or 0x1C:
            {
                var lsb = (((hw2 >> 12) & 7) << 2) | ((hw2 >> 6) & 3);
                var widthm1 = hw2 & 0x1F;
                insn = new Arm32Insn(addr, 4, (op == 0x14 ? "sbfx" : "ubfx") + itCond, $"{r[rd]}, {r[rn]}, #{lsb}, #{widthm1 + 1}");
                return true;
            }
            case 0x16:
            {
                var lsb = (((hw2 >> 12) & 7) << 2) | ((hw2 >> 6) & 3);
                var msb = hw2 & 0x1F;
                var width = Math.Max(1, msb - lsb + 1);
                insn = rn == 0xF
                    ? new Arm32Insn(addr, 4, "bfc" + itCond, $"{r[rd]}, #{lsb}, #{width}")
                    : new Arm32Insn(addr, 4, "bfi" + itCond, $"{r[rd]}, {r[rn]}, #{lsb}, #{width}");
                return true;
            }
            default:
                return false;
        }
    }

    private static bool DecodeT32BranchAndMisc(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var op1 = (hw2 >> 12) & 0x7;

        // Hints & barriers: hw1 == 0xF3xx
        if ((hw1 & 0xFF00) == 0xF300)
        {
            if (hw1 == 0xF3AF && hw2 == 0x8000) { insn = new Arm32Insn(addr, 4, "nop" + itCond, ""); return true; }
            if (hw1 == 0xF3BF && (hw2 & 0xFFF0) == 0x8F40) { insn = new Arm32Insn(addr, 4, "dsb", "sy"); return true; }
            if (hw1 == 0xF3BF && (hw2 & 0xFFF0) == 0x8F50) { insn = new Arm32Insn(addr, 4, "dmb", "sy"); return true; }
            if (hw1 == 0xF3BF && (hw2 & 0xFFF0) == 0x8F60) { insn = new Arm32Insn(addr, 4, "isb", "sy"); return true; }
            if (hw1 == 0xF3BF && hw2 == 0x8F2F) { insn = new Arm32Insn(addr, 4, "clrex", ""); return true; }
            return false;
        }

        // B.W (T4)
        if ((op1 & 5) == 1)
        {
            var s = (hw1 >> 10) & 1;
            var j1 = (hw2 >> 13) & 1;
            var j2 = (hw2 >> 11) & 1;
            var i1 = (j1 ^ s ^ 1) & 1;
            var i2 = (j2 ^ s ^ 1) & 1;
            var imm = (s << 24) | (i1 << 23) | (i2 << 22) | ((hw1 & 0x3FF) << 12) | ((hw2 & 0x7FF) << 1);
            var sext = (imm << 7) >> 7;
            var target = (ulong)((long)addr + 4 + sext);
            insn = new Arm32Insn(addr, 4, "b" + itCond, $"#0x{target:x}");
            return true;
        }

        // BL (T1) and BLX (T2)
        if ((op1 & 4) != 0)
        {
            var isBl = (op1 & 1) != 0;
            var s = (hw1 >> 10) & 1;
            var j1 = (hw2 >> 13) & 1;
            var j2 = (hw2 >> 11) & 1;
            var i1 = (j1 ^ s ^ 1) & 1;
            var i2 = (j2 ^ s ^ 1) & 1;
            var low = isBl ? ((hw2 & 0x7FF) << 1) : ((hw2 & 0x7FE) << 1);
            var imm = (s << 24) | (i1 << 23) | (i2 << 22) | ((hw1 & 0x3FF) << 12) | low;
            var sext = (imm << 7) >> 7;
            var pcBase = isBl ? (long)addr + 4 : (((long)addr + 4) & ~3L);
            var target = (ulong)(pcBase + sext);
            insn = new Arm32Insn(addr, 4, (isBl ? "bl" : "blx") + itCond, $"#0x{target:x}");
            return true;
        }

        // Conditional branch T3
        var cond = (hw1 >> 6) & 0xF;
        if (cond < 14)
        {
            var s = (hw1 >> 10) & 1;
            var j1 = (hw2 >> 13) & 1;
            var j2 = (hw2 >> 11) & 1;
            var imm = (s << 20) | (j2 << 19) | (j1 << 18) | ((hw1 & 0x3F) << 12) | ((hw2 & 0x7FF) << 1);
            var sext = (imm << 11) >> 11;
            var target = (ulong)((long)addr + 4 + sext);
            insn = new Arm32Insn(addr, 4, "b" + Arm32A32Decoder.CondSuffixes[cond], $"#0x{target:x}");
            return true;
        }

        return false;
    }

    private static bool DecodeT32LoadStoreSingle(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var size = (hw1 >> 5) & 3;
        var isSigned = ((hw1 >> 8) & 1) != 0;
        var l = ((hw1 >> 4) & 1) != 0;
        var uFlag = ((hw1 >> 7) & 1) != 0;
        var rn = hw1 & 0xF;
        var rt = (hw2 >> 12) & 0xF;
        if (size == 3) return false;

        var baseMnem = (l, size, isSigned) switch
        {
            (true, 0, false) => "ldrb",
            (true, 0, true) => "ldrsb",
            (true, 1, false) => "ldrh",
            (true, 1, true) => "ldrsh",
            (true, 2, _) => "ldr",
            (false, 0, _) => "strb",
            (false, 1, _) => "strh",
            (false, 2, _) => "str",
            _ => "",
        };
        if (baseMnem.Length == 0) return false;

        // PC literal load
        if (rn == 0xF && l)
        {
            var imm12 = (uint)(hw2 & 0xFFF);
            var offText = imm12 == 0 ? "[pc]" : uFlag ? $"[pc, {Arm32A32Decoder.FormatImm(imm12)}]" : $"[pc, #-0x{imm12:x}]";
            insn = new Arm32Insn(addr, 4, baseMnem + itCond, $"{r[rt]}, {offText}");
            return true;
        }

        // U==1 => 12-bit positive immediate offset
        if (uFlag)
        {
            var imm12 = (uint)(hw2 & 0xFFF);
            var offText = imm12 == 0 ? $"[{r[rn]}]" : $"[{r[rn]}, {Arm32A32Decoder.FormatImm(imm12)}]";
            insn = new Arm32Insn(addr, 4, baseMnem + itCond, $"{r[rt]}, {offText}");
            return true;
        }

        // Bit 11 == 1 => T4/pre/post imm8
        if ((hw2 & 0x0800) != 0)
        {
            var p = ((hw2 >> 10) & 1) != 0;
            var u = ((hw2 >> 9) & 1) != 0;
            var w = ((hw2 >> 8) & 1) != 0;
            var imm8 = (uint)(hw2 & 0xFF);
            var immStr = imm8 == 0 ? "#0" : (u ? Arm32A32Decoder.FormatImm(imm8) : (imm8 <= 9 ? $"#-{imm8}" : $"#-0x{imm8:x}"));
            var addrText = !p
                ? $"[{r[rn]}], {immStr}"
                : (w ? $"[{r[rn]}, {immStr}]!" : (imm8 == 0 ? $"[{r[rn]}]" : $"[{r[rn]}, {immStr}]"));
            insn = new Arm32Insn(addr, 4, baseMnem + itCond, $"{r[rt]}, {addrText}");
            return true;
        }

        // Register shifted [rn, rm, lsl #imm2]
        if ((hw2 & 0x0FC0) == 0x0000)
        {
            var rm = hw2 & 0xF;
            var sh = (hw2 >> 4) & 3;
            var idxText = sh == 0 ? $"[{r[rn]}, {r[rm]}]" : $"[{r[rn]}, {r[rm]}, lsl #{sh}]";
            insn = new Arm32Insn(addr, 4, baseMnem + itCond, $"{r[rt]}, {idxText}");
            return true;
        }

        return false;
    }

    private static bool DecodeT32DataProcReg(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var op1 = (hw1 >> 4) & 0xF;
        var rn = hw1 & 0xF;
        var rd = (hw2 >> 8) & 0xF;
        var rm = hw2 & 0xF;
        var op2 = (hw2 >> 4) & 0xF;

        if ((op1 & 8) == 0 && op2 == 0)
        {
            var s = ((op1 & 1) != 0) ? "s" : "";
            var mnem = (op1 >> 1) switch { 0 => "lsl", 1 => "lsr", 2 => "asr", _ => "ror" };
            insn = new Arm32Insn(addr, 4, $"{mnem}{s}{itCond}", $"{r[rd]}, {r[rn]}, {r[rm]}");
            return true;
        }

        if (op1 is <= 5 && (op2 & 8) != 0)
        {
            var mnem = op1 switch
            {
                0 => rn == 0xF ? "sxth" : "sxtah",
                1 => rn == 0xF ? "uxth" : "uxtah",
                4 => rn == 0xF ? "sxtb" : "sxtab",
                5 => rn == 0xF ? "uxtb" : "uxtab",
                _ => null,
            };
            if (mnem is null) return false;
            var ops = rn == 0xF ? $"{r[rd]}, {r[rm]}" : $"{r[rd]}, {r[rn]}, {r[rm]}";
            insn = new Arm32Insn(addr, 4, mnem + itCond, ops);
            return true;
        }

        if (op1 == 0x9 && (op2 & 8) != 0)
        {
            var mnem = (op2 & 3) switch { 0 => "rev", 1 => "rev16", 2 => "rbit", _ => "revsh" };
            insn = new Arm32Insn(addr, 4, mnem + itCond, $"{r[rd]}, {r[rm]}");
            return true;
        }
        if (op1 == 0xB && op2 == 0x8)
        {
            insn = new Arm32Insn(addr, 4, "clz" + itCond, $"{r[rd]}, {r[rm]}");
            return true;
        }
        return false;
    }

    private static bool DecodeT32MultiplyDivide(ushort hw1, ushort hw2, ulong addr, string itCond, out Arm32Insn insn)
    {
        insn = default;
        var r = Arm32A32Decoder.RegNames;
        var op1 = (hw1 >> 4) & 7;
        var op2 = (hw2 >> 4) & 3;
        var rn = hw1 & 0xF;
        var ra = (hw2 >> 12) & 0xF;
        var rd = (hw2 >> 8) & 0xF;
        var rm = hw2 & 0xF;

        if (op1 == 0 && op2 == 0)
        {
            insn = ra == 0xF
                ? new Arm32Insn(addr, 4, "mul" + itCond, $"{r[rd]}, {r[rn]}, {r[rm]}")
                : new Arm32Insn(addr, 4, "mla" + itCond, $"{r[rd]}, {r[rn]}, {r[rm]}, {r[ra]}");
            return true;
        }
        if (op1 == 0 && op2 == 1)
        {
            insn = new Arm32Insn(addr, 4, "mls" + itCond, $"{r[rd]}, {r[rn]}, {r[rm]}, {r[ra]}");
            return true;
        }
        if (op1 == 1 && (hw2 & 0x00F0) == 0x00F0)
        {
            insn = new Arm32Insn(addr, 4, "sdiv" + itCond, $"{r[rd]}, {r[rn]}, {r[rm]}");
            return true;
        }
        if (op1 == 3 && (hw2 & 0x00F0) == 0x00F0)
        {
            insn = new Arm32Insn(addr, 4, "udiv" + itCond, $"{r[rd]}, {r[rn]}, {r[rm]}");
            return true;
        }
        if (op1 is 4 or 6 && op2 == 0)
        {
            var mnem = op1 == 4 ? "smull" : "umull";
            insn = new Arm32Insn(addr, 4, mnem + itCond, $"{r[ra]}, {r[rd]}, {r[rn]}, {r[rm]}");
            return true;
        }
        return false;
    }

    private static string FormatThumbShift(int rm, int shiftType, int shiftImm)
    {
        var baseReg = Arm32A32Decoder.RegNames[rm];
        if (shiftType == 0 && shiftImm == 0) return baseReg;
        if (shiftType == 3 && shiftImm == 0) return $"{baseReg}, rrx";
        var n = shiftImm == 0 && shiftType is 1 or 2 ? 32 : shiftImm;
        var sh = shiftType switch { 0 => "lsl", 1 => "lsr", 2 => "asr", _ => "ror" };
        return $"{baseReg}, {sh} #{n}";
    }

    private static uint ThumbExpandImm(int imm12)
    {
        if ((imm12 >> 10) == 0)
        {
            var byteVal = (uint)(imm12 & 0xFF);
            return ((imm12 >> 8) & 3) switch
            {
                0 => byteVal,
                1 => (byteVal << 16) | byteVal,
                2 => (byteVal << 24) | (byteVal << 8),
                _ => (byteVal << 24) | (byteVal << 16) | (byteVal << 8) | byteVal,
            };
        }
        var unrotated = 0x80u | (uint)(imm12 & 0x7F);
        var rot = (imm12 >> 7) & 0x1F;
        return (unrotated >> rot) | (unrotated << (32 - rot));
    }
}
