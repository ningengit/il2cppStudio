using AsmResolver.DotNet.Signatures;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Win64 argument locations: positions 0-3 use RCX/RDX/R8/R9, or XMM0-3 for float/double;
/// position n ≥ 4 is at [rsp + 0x20 + 8·(n−4)] at the call site.
/// </summary>
public static class ArgumentLocations
{
    private static readonly Register[] IntRegs = [Register.RCX, Register.RDX, Register.R8, Register.R9];
    private static readonly Register[] FloatRegs = [Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3];

    public static Register IntRegister(int position) => position < 4 ? IntRegs[position] : Register.None;

    public static Register FloatRegister(int position) => position < 4 ? FloatRegs[position] : Register.None;

    /// <summary>Reads the value of argument <paramref name="position"/> at a call site.</summary>
    public static Expr Read(RegisterTracker regs, int position, bool isFloat)
    {
        if (position < 4) return regs.Get(isFloat ? FloatRegs[position] : IntRegs[position]);
        var key = regs.SlotKey(0x20 + 8L * (position - 4));
        return regs.GetSlot(key) ?? new NativeExpr($"stack_arg{position}");
    }

    /// <summary>Location of incoming parameter <paramref name="position"/> at function entry (before any push).</summary>
    public static long IncomingStackKey(int position) => 8 + 8L * position;   // return address, then home/stack area

    public static bool IsFloat(TypeSignature t) => MethodShape.IsFloat(t);
}
