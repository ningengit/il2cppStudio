using System.Text;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>
/// Position-independent fingerprint of a function's instruction stream. Two functions with equal fingerprints
/// compute the same thing: same instruction forms, registers, immediates and displacements; RIP-relative data and
/// external call targets compared by absolute address (shared constants/helpers); internal branches compared by
/// offset from the function start. Byte-sized RIP operands are wildcards: every IL2CPP method (and every
/// out-of-line <c>_inline</c> copy) owns a private <c>s_Il2CppMethodInitialized</c> guard flag.
/// </summary>
public static class FunctionFingerprint
{
    public static string Compute(IReadOnlyList<Instruction> ins, ulong start, ulong end)
    {
        var sb = new StringBuilder(ins.Count * 24);
        foreach (var i in ins)
        {
            if (i.Mnemonic == Mnemonic.Int3) break;   // trailing padding / no-return tails are not part of the body
            sb.Append((int)i.Code).Append(':');
            for (var op = 0; op < i.OpCount; op++)
            {
                switch (i.GetOpKind(op))
                {
                    case OpKind.Register: sb.Append('r').Append((int)i.GetOpRegister(op)); break;
                    case OpKind.NearBranch16 or OpKind.NearBranch32 or OpKind.NearBranch64:
                        var t = i.NearBranchTarget;
                        sb.Append(t >= start && t < end ? $"L{t - start:X}" : $"A{t:X}");
                        break;
                    case OpKind.Memory when i.IsIPRelativeMemoryOperand:
                        sb.Append(i.MemorySize == MemorySize.UInt8 ? "Mflag" : $"M{i.IPRelativeMemoryAddress:X}");
                        break;
                    case OpKind.Memory:
                        sb.Append($"m{(int)i.MemoryBase},{(int)i.MemoryIndex},{i.MemoryIndexScale},{i.MemoryDisplacement64:X},{(int)i.MemorySize}");
                        break;
                    case OpKind.Immediate8 or OpKind.Immediate16 or OpKind.Immediate32 or OpKind.Immediate64 or OpKind.Immediate8_2nd
                        or OpKind.Immediate8to16 or OpKind.Immediate8to32 or OpKind.Immediate8to64 or OpKind.Immediate32to64:
                        sb.Append('i').Append(i.GetImmediate(op).ToString("X"));
                        break;
                    default:
                        sb.Append('k').Append((int)i.GetOpKind(op));   // string/segment/far operands: form only
                        break;
                }
                sb.Append(',');
            }
            sb.Append(';');
        }
        return sb.ToString();
    }
}
