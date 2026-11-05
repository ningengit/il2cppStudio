using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Writes an instruction's destination operand: register state, scalar stack slot, field of a struct-typed
/// slot, object/static/array memory (via <see cref="MemoryResolver"/>), or a visible native fallback statement.
/// </summary>
public sealed class OperandWriter(RegisterTracker regs, StructSlotTable slots, MemoryResolver mem)
{
    public int Fallbacks { get; private set; }

    public void Write(EmitContext ctx, in Instruction ins, Expr value)
    {
        if (ins.Op0Kind == OpKind.Register) { regs.Set(ins.Op0Register, value); return; }
        var disp = unchecked((long)ins.MemoryDisplacement64);
        var size = Math.Max(1, ins.MemorySize.GetSize());
        long? key = ins.MemoryBase == Register.RSP && ins.MemoryIndex == Register.None ? regs.SlotKey(disp)
            : !ins.IsIPRelativeMemoryOperand && ins.MemoryIndex == Register.None && regs.Get(ins.MemoryBase) is StackAddrPlaceholder p ? p.Key + disp
            : null;
        if (key is { } k)
        {
            // A store into a stack slot starts building a new value: keep it pending (materialized into a fresh
            // struct local when a call consumes the slot) and leave the previous struct local untouched.
            slots.Forget(k);
            regs.SetSlot(k, value);
            return;
        }
        if (!ins.IsIPRelativeMemoryOperand)
        {
            var index = ins.MemoryIndex == Register.None ? null : regs.Get(ins.MemoryIndex);
            if (mem.Store(ctx, regs.Get(ins.MemoryBase), index, ins.MemoryIndexScale, disp, size, value)) return;
        }
        Fallbacks++;
        ctx.IL.Add(CilOpCodes.Ldstr, $"{ins} ; value = {value.Describe()}");
        ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.Stmt);
    }
}
