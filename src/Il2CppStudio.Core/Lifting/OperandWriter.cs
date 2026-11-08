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
        var size = Math.Max(1, ins.MemorySize.GetSize());
        if (value is PackedFloatsExpr packed && size == 8)
        {
            // movsd/movq [mem], xmm after unpcklps: two adjacent float fields (Vector2, Vector3.x/y).
            WriteAt(ctx, ins, 0, 4, packed.Lane0);
            WriteAt(ctx, ins, 4, 4, packed.Lane1);
            return;
        }
        if (value is VectorConstExpr vc && size == 16)
        {
            // movaps [mem], xmm holding a Color/Vector4/Quaternion literal: four float fields.
            for (var lane = 0; lane < 4; lane++) WriteAt(ctx, ins, lane * 4, 4, vc.Lane(lane));
            return;
        }
        WriteAt(ctx, ins, 0, size, value is PackedFloatsExpr p && size == 4 ? p.Lane0 : value);
    }

    private void WriteAt(EmitContext ctx, in Instruction ins, long extra, int size, Expr value)
    {
        var disp = unchecked((long)ins.MemoryDisplacement64) + extra;
        long? key = ins.MemoryBase == Register.RSP && ins.MemoryIndex == Register.None ? regs.SlotKey(disp)
            : !ins.IsIPRelativeMemoryOperand && ins.MemoryIndex == Register.None && regs.Get(ins.MemoryBase) is StackAddrPlaceholder p ? p.Key + disp
            : null;
        if (key is { } k)
        {
            // A store into a stack slot starts building a new value: keep it pending (materialized into a fresh
            // struct local when a call consumes the slot) and leave the previous struct local untouched.
            slots.Forget(k);
            // A wide store (movsd/movq 8 bytes, movaps 16) overwrites every narrower value inside it: drop them, or a
            // later struct bind replays a stale field (EnvMapAnimator: scale.y = euler.y after movsd [rsp+30h]).
            for (var inner = k + 1; inner < k + size; inner++) { regs.ClearSlot(inner); slots.Forget(inner); }
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
