using System.Buffers.Binary;
using System.Globalization;
using System.Text;

namespace Il2CppStudio.Core.Disassembly;

/// <summary>
/// ARMv7-A (A32) + VFPv3 + common Advanced SIMD (NEON) instruction decoder.
/// Also exposes shared VFP/SIMD helpers used by <see cref="Arm32ThumbDecoder"/>.
/// </summary>
internal static class Arm32A32Decoder
{
    internal static readonly string[] RegNames =
    [
        "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7",
        "r8", "r9", "r10", "fp", "r12", "sp", "lr", "pc",
    ];

    internal static readonly string[] CondSuffixes =
    [
        "eq", "ne", "hs", "lo", "mi", "pl", "vs", "vc",
        "hi", "ls", "ge", "lt", "gt", "le", "", "",
    ];

    private static readonly string[] AluOps =
    [
        "and", "eor", "sub", "rsb", "add", "adc", "sbc", "rsc",
        "tst", "teq", "cmp", "cmn", "orr", "mov", "bic", "mvn",
    ];

    private static readonly string[] ShiftNames = ["lsl", "lsr", "asr", "ror"];

    public static List<Arm32Insn> DecodeSequence(ReadOnlySpan<byte> code, ulong va, out int decodedBytes)
    {
        var count = code.Length / 4;
        var list = new List<Arm32Insn>(count);
        var pos = 0;
        while (pos + 4 <= code.Length)
        {
            var word = BinaryPrimitives.ReadUInt32LittleEndian(code.Slice(pos, 4));
            var addr = va + (ulong)pos;
            if (!TryDecodeWord(word, addr, out var insn))
                break;
            list.Add(insn);
            pos += 4;
            if (insn.Mnemonic is "udf" or "bkpt")
                break;
        }
        decodedBytes = pos;
        return list;
    }

    public static bool TryDecodeWord(uint word, ulong addr, out Arm32Insn insn)
    {
        insn = default;
        var condBits = (int)(word >> 28);
        if (condBits == 0xF)
            return TryDecodeUnconditional(word, addr, out insn);

        var cond = CondSuffixes[condBits];
        var op1 = (int)((word >> 25) & 0x7);

        var ok = op1 switch
        {
            0b000 => TryDecodeOp0(word, cond, out insn),
            0b001 => TryDecodeOp1(word, cond, out insn),
            0b010 => DecodeLoadStoreImm(word, cond, out insn),
            0b011 => TryDecodeOp3(word, cond, out insn),
            0b100 => DecodeBlockDataTransfer(word, cond, out insn),
            0b101 => DecodeBranch(word, addr, cond, out insn),
            0b110 or 0b111 => TryDecodeCoprocOrSvc(word, addr, cond, out insn),
            _ => false,
        };
        if (ok && insn.Address != addr)
            insn = insn with { Address = addr };
        return ok;
    }

    private static bool TryDecodeUnconditional(uint word, ulong addr, out Arm32Insn insn)
    {
        insn = default;
        // BLX (immediate): 1111_101H_imm24
        if ((word & 0xFE000000u) == 0xFA000000u)
        {
            var h = (word >> 23) & 2;
            var imm24 = (int)(word & 0x00FFFFFFu);
            var sext = (imm24 << 8) >> 8;
            var target = (ulong)((long)addr + 8 + ((long)sext << 2) + h);
            insn = new Arm32Insn(addr, 4, "blx", $"#0x{target:x}");
            return true;
        }

        // Barriers & memory hints
        if ((word & 0xFFFFFFF0u) == 0xF57FF040u) { insn = new Arm32Insn(addr, 4, "dsb", "sy"); return true; }
        if ((word & 0xFFFFFFF0u) == 0xF57FF050u) { insn = new Arm32Insn(addr, 4, "dmb", "sy"); return true; }
        if ((word & 0xFFFFFFF0u) == 0xF57FF060u) { insn = new Arm32Insn(addr, 4, "isb", "sy"); return true; }
        if (word == 0xF57FF01Fu) { insn = new Arm32Insn(addr, 4, "clrex", ""); return true; }
        if ((word & 0xFD70F000u) == 0xF550F000u)
        {
            var rn = (int)((word >> 16) & 0xF);
            insn = new Arm32Insn(addr, 4, "pld", $"[{RegNames[rn]}]");
            return true;
        }

        // NEON Advanced SIMD data processing: 1111_001x_xxxx_xxxx_xxxx_xxxx_xxx0_xxxx
        if ((word & 0xFE000000u) == 0xF2000000u)
            return TryDecodeNeonDataProc(word, addr, out insn);

        // NEON element load/store: 1111_0100_xxxx_...
        if ((word & 0xFF100000u) is 0xF4000000u or 0xF4100000u)
            return TryDecodeNeonElementLoadStore(word, addr, out insn);

        return false;
    }

    private static bool TryDecodeOp0(uint word, string cond, out Arm32Insn insn)
    {
        insn = default;
        var bit4 = ((word >> 4) & 1) != 0;
        var bit7 = ((word >> 7) & 1) != 0;
        var op = (int)((word >> 21) & 0xF);
        var s = ((word >> 20) & 1) != 0;

        // Miscellaneous instructions (op in 8..11 with S=0)
        if (!s && op is >= 8 and <= 11)
        {
            var op2 = (int)((word >> 4) & 0xF);
            if ((word & 0x0FFFFFF0u) == 0x012FFF10u)
            {
                insn = new Arm32Insn(0, 4, "bx" + cond, RegNames[word & 0xF]);
                return true;
            }
            if ((word & 0x0FFFFFF0u) == 0x012FFF30u)
            {
                insn = new Arm32Insn(0, 4, "blx" + cond, RegNames[word & 0xF]);
                return true;
            }
            if ((word & 0x0FFF0FF0u) == 0x016F0F10u)
            {
                var rd = (int)((word >> 12) & 0xF);
                var rm = (int)(word & 0xF);
                insn = new Arm32Insn(0, 4, "clz" + cond, $"{RegNames[rd]}, {RegNames[rm]}");
                return true;
            }
            if ((word & 0x0FF000F0u) == 0x01200070u)
            {
                var imm16 = (((word >> 8) & 0xFFF) << 4) | (word & 0xF);
                insn = new Arm32Insn(0, 4, "bkpt", $"#0x{imm16:x}");
                return true;
            }
            // Saturating arithmetic: QADD/QSUB/QDADD/QDSUB
            if ((word & 0x0F900FF0u) == 0x01000050u)
            {
                var rn = (int)((word >> 16) & 0xF);
                var rd = (int)((word >> 12) & 0xF);
                var rm = (int)(word & 0xF);
                var qmnem = ((word >> 21) & 3) switch
                {
                    0 => "qadd",
                    1 => "qsub",
                    2 => "qdadd",
                    _ => "qdsub",
                };
                insn = new Arm32Insn(0, 4, qmnem + cond, $"{RegNames[rd]}, {RegNames[rm]}, {RegNames[rn]}");
                return true;
            }
            // Halfword multiplies (SMULxx, SMLAxx, SMULWx, SMLAWx, SMLALxx)
            if (bit7 && !bit4 && (op2 & 0x9) == 0x8)
            {
                var rn = (int)((word >> 16) & 0xF);
                var ra = (int)((word >> 12) & 0xF);
                var rm = (int)((word >> 8) & 0xF);
                var rd = (int)(word & 0xF);
                var x = ((word >> 5) & 1) != 0 ? 't' : 'b';
                var y = ((word >> 6) & 1) != 0 ? 't' : 'b';
                var mOp = (int)((word >> 21) & 3);
                switch (mOp)
                {
                    case 0:
                        insn = new Arm32Insn(0, 4, $"smla{x}{y}{cond}", $"{RegNames[rn]}, {RegNames[rd]}, {RegNames[rm]}, {RegNames[ra]}");
                        return true;
                    case 1:
                        var wMnem = ((word >> 5) & 1) != 0 ? $"smulw{y}{cond}" : $"smlaw{y}{cond}";
                        insn = ((word >> 5) & 1) != 0
                            ? new Arm32Insn(0, 4, wMnem, $"{RegNames[rn]}, {RegNames[rd]}, {RegNames[rm]}")
                            : new Arm32Insn(0, 4, wMnem, $"{RegNames[rn]}, {RegNames[rd]}, {RegNames[rm]}, {RegNames[ra]}");
                        return true;
                    case 2:
                        insn = new Arm32Insn(0, 4, $"smlal{x}{y}{cond}", $"{RegNames[ra]}, {RegNames[rn]}, {RegNames[rd]}, {RegNames[rm]}");
                        return true;
                    case 3:
                        insn = new Arm32Insn(0, 4, $"smul{x}{y}{cond}", $"{RegNames[rn]}, {RegNames[rd]}, {RegNames[rm]}");
                        return true;
                }
            }
            // MRS / MSR register
            if ((word & 0x0FBF0FFFu) == 0x010F0000u)
            {
                var rd = (int)((word >> 12) & 0xF);
                var sysReg = ((word >> 22) & 1) != 0 ? "spsr" : "cpsr";
                insn = new Arm32Insn(0, 4, "mrs" + cond, $"{RegNames[rd]}, {sysReg}");
                return true;
            }
            if ((word & 0x0FB0FFF0u) == 0x0120F000u)
            {
                var rm = (int)(word & 0xF);
                insn = new Arm32Insn(0, 4, "msr" + cond, $"apsr_nzcvq, {RegNames[rm]}");
                return true;
            }
            return false;
        }

        // Multiply & Extra Load/Store (bit7 == 1 && bit4 == 1)
        if (bit7 && bit4)
        {
            if ((word & 0x0F0000F0u) == 0x00000090u)
                return DecodeMultiply(word, cond, out insn);

            if ((word & 0x0F0000F0u) is 0x01000090u)
                return TryDecodeExclusiveOrSwap(word, cond, out insn);

            return DecodeExtraLoadStore(word, cond, out insn);
        }

        // Standard ALU (register or register-shifted register)
        if (bit4 && bit7) return false;
        return DecodeAluReg(word, cond, bit4, out insn);
    }

    private static bool DecodeMultiply(uint word, string cond, out Arm32Insn insn)
    {
        insn = default;
        var op = (int)((word >> 21) & 0x7);
        var s = ((word >> 20) & 1) != 0 ? "s" : "";
        var rd = (int)((word >> 16) & 0xF);
        var ra = (int)((word >> 12) & 0xF);
        var rm = (int)((word >> 8) & 0xF);
        var rn = (int)(word & 0xF);

        switch (op)
        {
            case 0: insn = new Arm32Insn(0, 4, $"mul{s}{cond}", $"{RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}"); return true;
            case 1: insn = new Arm32Insn(0, 4, $"mla{s}{cond}", $"{RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}, {RegNames[ra]}"); return true;
            case 2: if (s.Length == 0) { insn = new Arm32Insn(0, 4, $"umaal{cond}", $"{RegNames[ra]}, {RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}"); return true; } return false;
            case 3: if (s.Length == 0) { insn = new Arm32Insn(0, 4, $"mls{cond}", $"{RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}, {RegNames[ra]}"); return true; } return false;
            case 4: insn = new Arm32Insn(0, 4, $"umull{s}{cond}", $"{RegNames[ra]}, {RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}"); return true;
            case 5: insn = new Arm32Insn(0, 4, $"umlal{s}{cond}", $"{RegNames[ra]}, {RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}"); return true;
            case 6: insn = new Arm32Insn(0, 4, $"smull{s}{cond}", $"{RegNames[ra]}, {RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}"); return true;
            case 7: insn = new Arm32Insn(0, 4, $"smlal{s}{cond}", $"{RegNames[ra]}, {RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}"); return true;
            default: return false;
        }
    }

    private static bool TryDecodeExclusiveOrSwap(uint word, string cond, out Arm32Insn insn)
    {
        insn = default;
        var op = (int)((word >> 20) & 0xF);
        var rn = (int)((word >> 16) & 0xF);
        var rt = (int)((word >> 12) & 0xF);
        var rm = (int)(word & 0xF);
        switch (op)
        {
            case 0x8: insn = new Arm32Insn(0, 4, "strex" + cond, $"{RegNames[rt]}, {RegNames[rm]}, [{RegNames[rn]}]"); return true;
            case 0x9: insn = new Arm32Insn(0, 4, "ldrex" + cond, $"{RegNames[rt]}, [{RegNames[rn]}]"); return true;
            case 0xA: insn = new Arm32Insn(0, 4, "strexd" + cond, $"{RegNames[rt]}, {RegNames[rm]}, {RegNames[(rm + 1) & 0xF]}, [{RegNames[rn]}]"); return true;
            case 0xB: insn = new Arm32Insn(0, 4, "ldrexd" + cond, $"{RegNames[rt]}, {RegNames[(rt + 1) & 0xF]}, [{RegNames[rn]}]"); return true;
            case 0xC: insn = new Arm32Insn(0, 4, "strexb" + cond, $"{RegNames[rt]}, {RegNames[rm]}, [{RegNames[rn]}]"); return true;
            case 0xD: insn = new Arm32Insn(0, 4, "ldrexb" + cond, $"{RegNames[rt]}, [{RegNames[rn]}]"); return true;
            case 0xE: insn = new Arm32Insn(0, 4, "strexh" + cond, $"{RegNames[rt]}, {RegNames[rm]}, [{RegNames[rn]}]"); return true;
            case 0xF: insn = new Arm32Insn(0, 4, "ldrexh" + cond, $"{RegNames[rt]}, [{RegNames[rn]}]"); return true;
            default: return false;
        }
    }

    private static bool DecodeExtraLoadStore(uint word, string cond, out Arm32Insn insn)
    {
        insn = default;
        var p = ((word >> 24) & 1) != 0;
        var u = ((word >> 23) & 1) != 0;
        var i = ((word >> 22) & 1) != 0;
        var w = ((word >> 21) & 1) != 0;
        var l = ((word >> 20) & 1) != 0;
        var rn = (int)((word >> 16) & 0xF);
        var rt = (int)((word >> 12) & 0xF);
        var rm = (int)(word & 0xF);
        var op2 = (int)((word >> 5) & 3);
        var imm8 = (int)(((word >> 4) & 0xF0) | (word & 0xF));

        string mnem;
        var isDual = false;
        if (!l && op2 == 1) mnem = "strh";
        else if (l && op2 == 1) mnem = "ldrh";
        else if (l && op2 == 2) mnem = "ldrsb";
        else if (l && op2 == 3) mnem = "ldrsh";
        else if (!l && op2 == 2) { mnem = "ldrd"; isDual = true; }
        else if (!l && op2 == 3) { mnem = "strd"; isDual = true; }
        else return false;

        if (isDual && (rt & 1) != 0) return false;
        var addrText = FormatExtraAddr(rn, rm, imm8, i, p, u, w);
        var opsText = isDual
            ? $"{RegNames[rt]}, {RegNames[(rt + 1) & 0xF]}, {addrText}"
            : $"{RegNames[rt]}, {addrText}";
        insn = new Arm32Insn(0, 4, mnem + cond, opsText);
        return true;
    }

    private static bool DecodeAluReg(uint word, string cond, bool regShift, out Arm32Insn insn)
    {
        var op = (int)((word >> 21) & 0xF);
        var s = ((word >> 20) & 1) != 0;
        var rn = (int)((word >> 16) & 0xF);
        var rd = (int)((word >> 12) & 0xF);
        var rm = (int)(word & 0xF);
        var st = (int)((word >> 5) & 3);
        var shiftImm = (int)((word >> 7) & 0x1F);
        var rs = (int)((word >> 8) & 0xF);

        var isTest = op is 8 or 9 or 10 or 11;
        var sfx = !isTest && s ? "s" : "";

        // MOV with non-zero shift is emitted as lsl/lsr/asr/ror/rrx unified alias for Arm32Lifter compatibility.
        if (op == 13 && (regShift || shiftImm != 0 || st == 3))
        {
            if (!regShift && st == 3 && shiftImm == 0)
            {
                insn = new Arm32Insn(0, 4, $"rrx{sfx}{cond}", $"{RegNames[rd]}, {RegNames[rm]}");
                return true;
            }
            var decodedImm = DecodeShiftAmount(st, shiftImm);
            var shOp = ShiftNames[st];
            var opStr = regShift
                ? $"{RegNames[rd]}, {RegNames[rm]}, {RegNames[rs]}"
                : $"{RegNames[rd]}, {RegNames[rm]}, #{decodedImm}";
            insn = new Arm32Insn(0, 4, $"{shOp}{sfx}{cond}", opStr);
            return true;
        }

        var shiftText = FormatRegOperand(rm, st, shiftImm, rs, regShift);
        var mnem = $"{AluOps[op]}{sfx}{cond}";
        string operands;
        if (isTest)
            operands = $"{RegNames[rn]}, {shiftText}";
        else if (op is 13 or 15) // mov / mvn
            operands = $"{RegNames[rd]}, {shiftText}";
        else
            operands = $"{RegNames[rd]}, {RegNames[rn]}, {shiftText}";

        insn = new Arm32Insn(0, 4, mnem, operands);
        return true;
    }

    private static bool TryDecodeOp1(uint word, string cond, out Arm32Insn insn)
    {
        insn = default;
        var op = (int)((word >> 21) & 0xF);
        var s = ((word >> 20) & 1) != 0;
        var rn = (int)((word >> 16) & 0xF);
        var rd = (int)((word >> 12) & 0xF);

        if (!s && op is >= 8 and <= 11)
        {
            if (op == 8) // MOVW
            {
                var imm16 = (((word >> 16) & 0xF) << 12) | (word & 0xFFF);
                insn = new Arm32Insn(0, 4, "movw" + cond, $"{RegNames[rd]}, {FormatImm(imm16)}");
                return true;
            }
            if (op == 10) // MOVT
            {
                var imm16 = (((word >> 16) & 0xF) << 12) | (word & 0xFFF);
                insn = new Arm32Insn(0, 4, "movt" + cond, $"{RegNames[rd]}, {FormatImm(imm16)}");
                return true;
            }
            if (op == 9 && rn == 0 && rd == 0xF)
            {
                var hint = word & 0xFF;
                var hintName = hint switch
                {
                    0 => "nop",
                    1 => "yield",
                    2 => "wfe",
                    3 => "wfi",
                    4 => "sev",
                    _ => null,
                };
                if (hintName is null) return false;
                insn = new Arm32Insn(0, 4, hintName + cond, "");
                return true;
            }
            return false;
        }

        var imm8 = word & 0xFF;
        var rot = (int)(((word >> 8) & 0xF) * 2);
        var imm = rot == 0 ? imm8 : (imm8 >> rot) | (imm8 << (32 - rot));
        var isTest = op is 8 or 9 or 10 or 11;
        var sfx = !isTest && s ? "s" : "";
        var mnem = $"{AluOps[op]}{sfx}{cond}";

        var immStr = FormatImm(imm);
        var operands = isTest
            ? $"{RegNames[rn]}, {immStr}"
            : op is 13 or 15
                ? $"{RegNames[rd]}, {immStr}"
                : $"{RegNames[rd]}, {RegNames[rn]}, {immStr}";

        insn = new Arm32Insn(0, 4, mnem, operands);
        return true;
    }

    private static bool DecodeLoadStoreImm(uint word, string cond, out Arm32Insn insn)
    {
        var p = ((word >> 24) & 1) != 0;
        var u = ((word >> 23) & 1) != 0;
        var b = ((word >> 22) & 1) != 0;
        var w = ((word >> 21) & 1) != 0;
        var l = ((word >> 20) & 1) != 0;
        var rn = (int)((word >> 16) & 0xF);
        var rt = (int)((word >> 12) & 0xF);
        var imm12 = (int)(word & 0xFFF);

        // Single-register push/pop UAL aliases
        if (rn == 13 && !b && imm12 == 4)
        {
            if (p && !u && w && !l)
            {
                insn = new Arm32Insn(0, 4, "push" + cond, $"{{{RegNames[rt]}}}");
                return true;
            }
            if (!p && u && !w && l)
            {
                insn = new Arm32Insn(0, 4, "pop" + cond, $"{{{RegNames[rt]}}}");
                return true;
            }
        }

        var mnem = (l ? "ldr" : "str") + (b ? "b" : "") + cond;
        var addrText = FormatExtraAddr(rn, 0, imm12, isImm: true, p, u, w);
        insn = new Arm32Insn(0, 4, mnem, $"{RegNames[rt]}, {addrText}");
        return true;
    }

    private static bool TryDecodeOp3(uint word, string cond, out Arm32Insn insn)
    {
        insn = default;
        if (((word >> 4) & 1) != 0)
            return TryDecodeMedia(word, cond, out insn);

        var p = ((word >> 24) & 1) != 0;
        var u = ((word >> 23) & 1) != 0;
        var b = ((word >> 22) & 1) != 0;
        var w = ((word >> 21) & 1) != 0;
        var l = ((word >> 20) & 1) != 0;
        var rn = (int)((word >> 16) & 0xF);
        var rt = (int)((word >> 12) & 0xF);
        var rm = (int)(word & 0xF);
        var st = (int)((word >> 5) & 3);
        var shiftImm = (int)((word >> 7) & 0x1F);

        var mnem = (l ? "ldr" : "str") + (b ? "b" : "") + cond;
        var rmExpr = FormatRegOperand(rm, st, shiftImm, 0, regShift: false);
        if (!u) rmExpr = "-" + rmExpr;
        var addrText = p
            ? (w ? $"[{RegNames[rn]}, {rmExpr}]!" : $"[{RegNames[rn]}, {rmExpr}]")
            : $"[{RegNames[rn]}], {rmExpr}";

        insn = new Arm32Insn(0, 4, mnem, $"{RegNames[rt]}, {addrText}");
        return true;
    }

    private static bool TryDecodeMedia(uint word, string cond, out Arm32Insn insn)
    {
        insn = default;
        if ((word & 0x0FF000F0u) == 0x07F000F0u)
        {
            insn = new Arm32Insn(0, 4, "udf", $"#0x{(((word >> 8) & 0xFFF) << 4) | (word & 0xF):x}");
            return true;
        }

        var op1 = (int)((word >> 20) & 0x1F);
        var rn = (int)((word >> 16) & 0xF);
        var rd = (int)((word >> 12) & 0xF);
        var rm = (int)(word & 0xF);
        var op2 = (int)((word >> 5) & 0x7);

        // Extend (SXTB/SXTH/UXTB/UXTH + SXTAB/SXTAH/UXTAB/UXTAH)
        if (op1 is 0x0A or 0x0B or 0x0E or 0x0F && (op2 & 3) == 3)
        {
            var baseName = op1 switch
            {
                0x0A => rn == 0xF ? "sxtb" : "sxtab",
                0x0B => rn == 0xF ? "sxth" : "sxtah",
                0x0E => rn == 0xF ? "uxtb" : "uxtab",
                _ => rn == 0xF ? "uxth" : "uxtah",
            };
            var rotBits = (int)((word >> 10) & 3) * 8;
            var rotStr = rotBits == 0 ? "" : $", ror #{rotBits}";
            var ops = rn == 0xF
                ? $"{RegNames[rd]}, {RegNames[rm]}{rotStr}"
                : $"{RegNames[rd]}, {RegNames[rn]}, {RegNames[rm]}{rotStr}";
            insn = new Arm32Insn(0, 4, baseName + cond, ops);
            return true;
        }

        // Bitfield extract: UBFX / SBFX
        if ((op1 & 0x1B) is 0x1A or 0x1E && (op2 & 3) == 2)
        {
            var mnem = (op1 & 4) != 0 ? "ubfx" : "sbfx";
            var lsb = (int)((word >> 7) & 0x1F);
            var widthm1 = (int)((word >> 16) & 0x1F);
            insn = new Arm32Insn(0, 4, mnem + cond, $"{RegNames[rd]}, {RegNames[rm]}, #{lsb}, #{widthm1 + 1}");
            return true;
        }

        // BFI / BFC
        if ((op1 & 0x1E) == 0x1C && (op2 & 3) == 0)
        {
            var lsb = (int)((word >> 7) & 0x1F);
            var msb = (int)((word >> 16) & 0x1F);
            var width = Math.Max(1, msb - lsb + 1);
            if (rm == 0xF)
                insn = new Arm32Insn(0, 4, "bfc" + cond, $"{RegNames[rd]}, #{lsb}, #{width}");
            else
                insn = new Arm32Insn(0, 4, "bfi" + cond, $"{RegNames[rd]}, {RegNames[rm]}, #{lsb}, #{width}");
            return true;
        }

        // SDIV / UDIV
        if ((word & 0x0FD0F0F0u) == 0x0710F010u)
        {
            var mnem = ((word >> 21) & 1) != 0 ? "udiv" : "sdiv";
            var ra = (int)((word >> 8) & 0xF);
            insn = new Arm32Insn(0, 4, mnem + cond, $"{RegNames[rn]}, {RegNames[rm]}, {RegNames[ra]}");
            return true;
        }

        // REV / REV16 / RBIT / REVSH
        if ((word & 0x0FFB0FF0u) is 0x06BF0F30u or 0x06BF0FB0u or 0x06FF0F30u or 0x06FF0FB0u)
        {
            var mnem = (op1 & 4) == 0
                ? (op2 == 1 ? "rev" : "rev16")
                : (op2 == 1 ? "rbit" : "revsh");
            insn = new Arm32Insn(0, 4, mnem + cond, $"{RegNames[rd]}, {RegNames[rm]}");
            return true;
        }

        return false;
    }

    private static bool DecodeBlockDataTransfer(uint word, string cond, out Arm32Insn insn)
    {
        var p = ((word >> 24) & 1) != 0;
        var u = ((word >> 23) & 1) != 0;
        var s = ((word >> 22) & 1) != 0;
        var w = ((word >> 21) & 1) != 0;
        var l = ((word >> 20) & 1) != 0;
        var rn = (int)((word >> 16) & 0xF);
        var mask = (ushort)(word & 0xFFFF);
        if (mask == 0) { insn = default; return false; }
        var regs = FormatRegList(mask);
        var caret = s ? "^" : "";

        if (rn == 13 && w && !s && !l && p && !u)
        {
            insn = new Arm32Insn(0, 4, "push" + cond, $"{{{regs}}}");
            return true;
        }
        if (rn == 13 && w && !s && l && !p && u)
        {
            insn = new Arm32Insn(0, 4, "pop" + cond, $"{{{regs}}}");
            return true;
        }

        var mode = (p, u) switch
        {
            (false, true) => "ia",
            (true, true) => "ib",
            (false, false) => "da",
            (true, false) => "db",
        };
        var baseName = l ? "ldm" : "stm";
        if (mode == "ia") mode = "";
        var wb = w ? "!" : "";
        insn = new Arm32Insn(0, 4, $"{baseName}{mode}{cond}", $"{RegNames[rn]}{wb}, {{{regs}}}{caret}");
        return true;
    }

    private static bool DecodeBranch(uint word, ulong addr, string cond, out Arm32Insn insn)
    {
        var link = ((word >> 24) & 1) != 0;
        var imm24 = (int)(word & 0x00FFFFFFu);
        var sext = (imm24 << 8) >> 8;
        var target = (ulong)((long)addr + 8 + ((long)sext << 2));
        var mnem = (link ? "bl" : "b") + cond;
        insn = new Arm32Insn(addr, 4, mnem, $"#0x{target:x}");
        return true;
    }

    private static bool TryDecodeCoprocOrSvc(uint word, ulong addr, string cond, out Arm32Insn insn)
    {
        if ((word & 0x0F000000u) == 0x0F000000u)
        {
            insn = new Arm32Insn(addr, 4, "svc" + cond, $"#0x{word & 0x00FFFFFFu:x}");
            return true;
        }

        var coproc = (int)((word >> 8) & 0xF);
        if (coproc is 10 or 11)
            return TryDecodeVfp(word, addr, cond, out insn);

        insn = default;
        return false;
    }

    internal static bool TryDecodeVfp(uint word, ulong addr, string cond, out Arm32Insn insn)
    {
        insn = default;
        var top = (int)((word >> 24) & 0xF);
        var sz = ((word >> 8) & 1) != 0; // true = .f64 (coproc 11), false = .f32 (coproc 10)
        var typeSfx = sz ? ".f64" : ".f32";

        // VFP 64-bit transfer between ARM core registers and single/double extension registers
        if ((word & 0x0FE00E00u) == 0x0C400A00u)
        {
            var l = ((word >> 20) & 1) != 0;
            var rt2 = (int)((word >> 16) & 0xF);
            var rt = (int)((word >> 12) & 0xF);
            var mBit = (int)((word >> 5) & 1);
            var vm = (int)(word & 0xF);
            var ops = sz
                ? (l ? $"{RegNames[rt]}, {RegNames[rt2]}, d{(mBit << 4) | vm}" : $"d{(mBit << 4) | vm}, {RegNames[rt]}, {RegNames[rt2]}")
                : (l ? $"{RegNames[rt]}, {RegNames[rt2]}, s{(vm << 1) | mBit}, s{((vm << 1) | mBit) + 1}" : $"s{(vm << 1) | mBit}, s{((vm << 1) | mBit) + 1}, {RegNames[rt]}, {RegNames[rt2]}");
            insn = new Arm32Insn(addr, 4, "vmov" + cond, ops);
            return true;
        }

        // VFP load/store and block load/store (0xC / 0xD)
        if (top is 0xC or 0xD)
        {
            var p = ((word >> 24) & 1) != 0;
            var u = ((word >> 23) & 1) != 0;
            var dBit = (int)((word >> 22) & 1);
            var w = ((word >> 21) & 1) != 0;
            var l = ((word >> 20) & 1) != 0;
            var rn = (int)((word >> 16) & 0xF);
            var vd = (int)((word >> 12) & 0xF);
            var imm8 = (int)(word & 0xFF);
            var regName = VfpRegName(vd, dBit, sz);

            // VLDR / VSTR (P=1, W=0)
            if (p && !w)
            {
                var off = imm8 << 2;
                var mnem = (l ? "vldr" : "vstr") + cond;
                var addrText = off == 0
                    ? $"[{RegNames[rn]}]"
                    : u ? $"[{RegNames[rn]}, {FormatImm((uint)off)}]" : $"[{RegNames[rn]}, #-0x{off:x}]";
                insn = new Arm32Insn(addr, 4, mnem, $"{regName}, {addrText}");
                return true;
            }

            // VPUSH / VPOP / VLDM / VSTM
            var regCount = sz ? (imm8 / 2) : imm8;
            if (regCount <= 0 || regCount > 32) return false;
            var startIdx = sz ? ((dBit << 4) | vd) : ((vd << 1) | dBit);
            var prefix = sz ? 'd' : 's';
            var listText = FormatSequentialVfpList(prefix, startIdx, regCount);

            if (rn == 13 && w && !l && p && !u)
            {
                insn = new Arm32Insn(addr, 4, "vpush" + cond, $"{{{listText}}}");
                return true;
            }
            if (rn == 13 && w && l && !p && u)
            {
                insn = new Arm32Insn(addr, 4, "vpop" + cond, $"{{{listText}}}");
                return true;
            }

            if (p == u) return false; // illegal P==U in vldm/vstm unless FLDMX
            var vMode = !p && u ? "ia" : "db";
            var vMnem = (l ? "vldm" : "vstm") + vMode + cond;
            var wb = w ? "!" : "";
            insn = new Arm32Insn(addr, 4, vMnem, $"{RegNames[rn]}{wb}, {{{listText}}}");
            return true;
        }

        // VFP 32-bit transfer (top == 0xE && bit4 == 1)
        if (((word >> 4) & 1) != 0)
        {
            var l = ((word >> 20) & 1) != 0;
            var opc1 = (int)((word >> 21) & 0x7);
            var vn = (int)((word >> 16) & 0xF);
            var rt = (int)((word >> 12) & 0xF);
            var nBit = (int)((word >> 7) & 1);
            var sReg = $"s{(vn << 1) | nBit}";

            // VMRS / VMSR
            if (opc1 == 0b111 && !sz)
            {
                if (l && vn == 1)
                {
                    var dst = rt == 15 ? "apsr_nzcv" : RegNames[rt];
                    insn = new Arm32Insn(addr, 4, "vmrs" + cond, $"{dst}, fpscr");
                    return true;
                }
                if (!l && vn == 1)
                {
                    insn = new Arm32Insn(addr, 4, "vmsr" + cond, $"fpscr, {RegNames[rt]}");
                    return true;
                }
                return false;
            }

            if (opc1 == 0 && !sz)
            {
                var ops = l ? $"{RegNames[rt]}, {sReg}" : $"{sReg}, {RegNames[rt]}";
                insn = new Arm32Insn(addr, 4, "vmov" + cond, ops);
                return true;
            }
            if (sz && opc1 is 0 or 1 or 2 or 3)
            {
                // VDUP / VMOV scalar
                var idx = (opc1 & 1);
                var dReg = $"d{(nBit << 4) | vn}";
                var ops = l ? $"{RegNames[rt]}, {dReg}[{idx}]" : $"{dReg}[{idx}], {RegNames[rt]}";
                insn = new Arm32Insn(addr, 4, "vmov" + cond, ops);
                return true;
            }
            return false;
        }

        // VFP data processing (top == 0xE && bit4 == 0)
        {
            var opc1A = ((word >> 23) & 1) != 0;
            var dBit = (int)((word >> 22) & 1);
            var opc1B = (int)((word >> 20) & 3);
            var opc2 = (int)((word >> 16) & 0xF);
            var vd = (int)((word >> 12) & 0xF);
            var nBit = (int)((word >> 7) & 1);
            var opc3 = ((word >> 6) & 1) != 0;
            var mBit = (int)((word >> 5) & 1);
            var vm = (int)(word & 0xF);

            var dReg = VfpRegName(vd, dBit, sz);
            var nReg = VfpRegName(opc2, nBit, sz);
            var mReg = VfpRegName(vm, mBit, sz);

            if (!opc1A)
            {
                var mnem = (opc1B, opc3) switch
                {
                    (0, false) => "vmla",
                    (0, true) => "vmls",
                    (1, false) => "vnmls",
                    (1, true) => "vnmla",
                    (2, false) => "vmul",
                    (2, true) => "vnmul",
                    (3, false) => "vadd",
                    _ => "vsub",
                };
                insn = new Arm32Insn(addr, 4, $"{mnem}{cond}{typeSfx}", $"{dReg}, {nReg}, {mReg}");
                return true;
            }

            if (opc1B == 0 && !opc3)
            {
                insn = new Arm32Insn(addr, 4, $"vdiv{cond}{typeSfx}", $"{dReg}, {nReg}, {mReg}");
                return true;
            }

            if (opc1B is 1 or 2)
            {
                var mnem = opc1B == 2 ? (opc3 ? "vfms" : "vfma") : (opc3 ? "vfnms" : "vfnma");
                insn = new Arm32Insn(addr, 4, $"{mnem}{cond}{typeSfx}", $"{dReg}, {nReg}, {mReg}");
                return true;
            }

            if (opc1B == 3 && !opc3)
            {
                // VMOV (immediate)
                var imm8 = (uint)((opc2 << 4) | vm);
                var valStr = FormatVfpImmediate(imm8, sz);
                insn = new Arm32Insn(addr, 4, $"vmov{cond}{typeSfx}", $"{dReg}, #{valStr}");
                return true;
            }

            // Two-register VFP operations (opc1A=1, opc1B=3, opc3=1)
            switch (opc2)
            {
                case 0:
                    insn = new Arm32Insn(addr, 4, $"{(nBit == 0 ? "vmov" : "vabs")}{cond}{typeSfx}", $"{dReg}, {mReg}");
                    return true;
                case 1:
                    insn = new Arm32Insn(addr, 4, $"{(nBit == 0 ? "vneg" : "vsqrt")}{cond}{typeSfx}", $"{dReg}, {mReg}");
                    return true;
                case 4:
                    insn = new Arm32Insn(addr, 4, $"{(nBit == 0 ? "vcmp" : "vcmpe")}{cond}{typeSfx}", $"{dReg}, {mReg}");
                    return true;
                case 5:
                    insn = new Arm32Insn(addr, 4, $"{(nBit == 0 ? "vcmp" : "vcmpe")}{cond}{typeSfx}", $"{dReg}, #0");
                    return true;
                case 7 when nBit != 0:
                {
                    var dstName = VfpRegName(vd, dBit, !sz);
                    var cvtSuffix = sz ? ".f32.f64" : ".f64.f32";
                    insn = new Arm32Insn(addr, 4, $"vcvt{cond}{cvtSuffix}", $"{dstName}, {mReg}");
                    return true;
                }
                case 8:
                {
                    var mSingle = $"s{(vm << 1) | mBit}";
                    var cvtSuffix = nBit != 0 ? $"{typeSfx}.s32" : $"{typeSfx}.u32";
                    insn = new Arm32Insn(addr, 4, $"vcvt{cond}{cvtSuffix}", $"{dReg}, {mSingle}");
                    return true;
                }
                case 12 or 13:
                {
                    var dstSingle = $"s{(vd << 1) | dBit}";
                    var signed = opc2 == 13;
                    var roundRoot = nBit != 0 ? "vcvt" : "vcvtr";
                    var cvtSuffix = (signed ? ".s32" : ".u32") + typeSfx;
                    insn = new Arm32Insn(addr, 4, $"{roundRoot}{cond}{cvtSuffix}", $"{dstSingle}, {mReg}");
                    return true;
                }
                default:
                    return false;
            }
        }
    }

    private static bool TryDecodeNeonDataProc(uint word, ulong addr, out Arm32Insn insn)
    {
        insn = default;
        var u = ((word >> 24) & 1) != 0;
        var bit4 = ((word >> 4) & 1) != 0;
        var q = ((word >> 6) & 1) != 0;
        var dBit = (int)((word >> 22) & 1);
        var vd = (int)((word >> 12) & 0xF);
        var nBit = (int)((word >> 7) & 1);
        var vn = (int)((word >> 16) & 0xF);
        var mBit = (int)((word >> 5) & 1);
        var vm = (int)(word & 0xF);
        var opCode = (int)((word >> 8) & 0xF);

        string Reg(int r, int b) => q ? $"q{((b << 4) | r) >> 1}" : $"d{(b << 4) | r}";
        var dReg = Reg(vd, dBit);
        var nReg = Reg(vn, nBit);
        var mReg = Reg(vm, mBit);

        // Modified immediate VMOV/VORR/VMVN/VBIC: bits[27:25]=0b001, bit[23]=1, bit[4]=0
        if (((word >> 23) & 1) != 0 && !bit4 && ((word >> 19) & 0x7) == 0 && (opCode is 0 or 2 or 4 or 6 or 8 or 10 or 12 or 14))
        {
            var cmode = opCode;
            var imm8 = (((word >> 24) & 1) << 7) | (((word >> 16) & 7) << 4) | (word & 0xF);
            var typeSfx = cmode is 12 or 13 ? ".i16" : cmode == 14 ? ".i8" : ".i32";
            insn = new Arm32Insn(addr, 4, "vmov" + typeSfx, $"{dReg}, #0x{imm8:x}");
            return true;
        }

        // Common NEON 3-same instructions
        if (!bit4 || opCode is 1 or 8 or 9 or 12 or 13)
        {
            var sizeBits = (int)((word >> 20) & 3);
            var dt = sizeBits switch { 0 => "8", 1 => "16", 2 => "32", _ => "64" };
            var mnem = (opCode, bit4, u) switch
            {
                (1, false, false) when sizeBits == 2 => "vorr",
                (1, false, false) => "vand",
                (1, false, true) when sizeBits == 3 => "veor",
                (1, true, _) => $"vrhadd.u{dt}",
                (8, false, _) => $"vadd.i{dt}",
                (8, true, _) => $"vsub.i{dt}",
                (9, true, _) => $"vmul.i{dt}",
                (13, false, _) when (sizeBits & 1) == 0 => "vadd.f32",
                (13, false, true) when (sizeBits & 1) != 0 => "vsub.f32",
                (13, true, true) => "vmul.f32",
                _ => null,
            };
            if (mnem is not null)
            {
                insn = new Arm32Insn(addr, 4, mnem, $"{dReg}, {nReg}, {mReg}");
                return true;
            }
        }

        return false;
    }

    private static bool TryDecodeNeonElementLoadStore(uint word, ulong addr, out Arm32Insn insn)
    {
        insn = default;
        var l = ((word >> 21) & 1) != 0;
        var dBit = (int)((word >> 22) & 1);
        var vd = (int)((word >> 12) & 0xF);
        var rn = (int)((word >> 16) & 0xF);
        var size = (int)((word >> 6) & 3);
        var type = (int)((word >> 8) & 0xF);
        var bits = 8 << Math.Min(size, 3);
        var dStart = (dBit << 4) | vd;
        var mnem = (l ? "vld1" : "vst1") + $".{bits}";
        var regList = type switch
        {
            0x7 => $"d{dStart}",
            0xA => $"d{dStart}, d{dStart + 1}",
            0x6 => $"d{dStart}, d{dStart + 1}, d{dStart + 2}",
            0x2 => $"d{dStart}, d{dStart + 1}, d{dStart + 2}, d{dStart + 3}",
            _ => $"d{dStart}",
        };
        insn = new Arm32Insn(addr, 4, mnem, $"{{{regList}}}, [{RegNames[rn]}]");
        return true;
    }

    internal static string VfpRegName(int reg4, int topBit, bool isDouble) =>
        isDouble ? $"d{(topBit << 4) | reg4}" : $"s{(reg4 << 1) | topBit}";

    internal static string FormatVfpImmediate(uint imm8, bool isDouble)
    {
        var a = (imm8 >> 7) & 1;
        var b = (imm8 >> 6) & 1;
        var rep = imm8 & 0x3F;
        if (isDouble)
        {
            var bits = ((ulong)a << 63)
                | ((ulong)(b ^ 1) << 62)
                | ((ulong)(b != 0 ? 0xFF : 0) << 54)
                | ((ulong)rep << 48);
            var val = BitConverter.Int64BitsToDouble(unchecked((long)bits));
            return val.ToString("e6", CultureInfo.InvariantCulture);
        }
        else
        {
            var bits = (a << 31)
                | ((b ^ 1) << 30)
                | ((b != 0 ? 0x1Fu : 0u) << 25)
                | (rep << 19);
            var val = BitConverter.Int32BitsToSingle(unchecked((int)bits));
            return val.ToString("e6", CultureInfo.InvariantCulture);
        }
    }

    private static string FormatSequentialVfpList(char prefix, int start, int count)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(prefix).Append(start + i);
        }
        return sb.ToString();
    }

    internal static string FormatRegList(ushort mask)
    {
        var sb = new StringBuilder();
        var first = true;
        for (var i = 0; i < 16; i++)
        {
            if ((mask & (1 << i)) == 0) continue;
            if (!first) sb.Append(", ");
            first = false;
            sb.Append(RegNames[i]);
        }
        return sb.ToString();
    }

    internal static string FormatImm(uint value) =>
        value <= 9 ? $"#{value}" : $"#0x{value:x}";

    private static int DecodeShiftAmount(int shiftType, int shiftImm) =>
        shiftImm == 0 && shiftType is 1 or 2 ? 32 : shiftImm;

    private static string FormatRegOperand(int rm, int shiftType, int shiftImm, int rs, bool regShift)
    {
        if (!regShift && shiftType == 0 && shiftImm == 0)
            return RegNames[rm];
        if (!regShift && shiftType == 3 && shiftImm == 0)
            return $"{RegNames[rm]}, rrx";
        if (regShift)
            return $"{RegNames[rm]}, {ShiftNames[shiftType]} {RegNames[rs]}";
        var n = DecodeShiftAmount(shiftType, shiftImm);
        return $"{RegNames[rm]}, {ShiftNames[shiftType]} #{n}";
    }

    private static string FormatExtraAddr(int rn, int rm, int imm, bool isImm, bool p, bool u, bool w)
    {
        var baseName = RegNames[rn];
        if (!isImm)
        {
            var signedRm = u ? RegNames[rm] : $"-{RegNames[rm]}";
            if (!p) return $"[{baseName}], {signedRm}";
            return w ? $"[{baseName}, {signedRm}]!" : $"[{baseName}, {signedRm}]";
        }
        if (p && !w && imm == 0)
            return $"[{baseName}]";
        var immText = imm == 0 ? "#0" : (u ? FormatImm((uint)imm) : $"-#{FormatSignedImm((uint)imm)}");
        if (!u && imm != 0)
            immText = imm <= 9 ? $"#-{imm}" : $"#-0x{imm:x}";
        if (!p) return $"[{baseName}], {immText}";
        return w ? $"[{baseName}, {immText}]!" : $"[{baseName}, {immText}]";
    }

    private static string FormatSignedImm(uint v) => v <= 9 ? v.ToString(CultureInfo.InvariantCulture) : $"0x{v:x}";
}
