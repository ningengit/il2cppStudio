using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Flag-producing compares and condition-consuming instructions. Conditional branches to trap blocks
/// (NullCheck / bounds failures) are dropped, IL2CPP runtime checks are folded to the path that skips the
/// initialization helper, and everything else becomes <c>cond; brtrue label</c>.
/// </summary>
public sealed class BranchTranslator(RegisterTracker regs, OperandReader ops, InstructionTranslators arith)
{
    private FlagState _flags;
    public int ElidedChecks { get; private set; }

    /// <summary>Updates flag state; returns true if the instruction was a pure compare (no other effect).</summary>
    public bool Observe(in Instruction ins)
    {
        switch (ins.Mnemonic)
        {
            case Mnemonic.Cmp: _flags = new(FlagSource.Compare, ops.Read(ins, 0), ops.Read(ins, 1)); return true;
            case Mnemonic.Test: _flags = new(FlagSource.Test, ops.Read(ins, 0), ops.Read(ins, 1)); return true;
            case Mnemonic.Comiss or Mnemonic.Ucomiss or Mnemonic.Comisd or Mnemonic.Ucomisd:
                _flags = new(FlagSource.FloatCompare, ops.Read(ins, 0), ops.Read(ins, 1)); return true;
            default: return false;
        }
    }

    /// <summary>Called after an arithmetic instruction that modifies flags.</summary>
    public void NoteResult(Register destination = Register.None)
    {
        _flags = new(FlagSource.Result, arith.LastResult, null);
        _resultRegister = destination;
    }

    private Register _resultRegister;
    private Expr? _preSpill;

    /// <summary>
    /// Call before spilling live-out registers at a conditional branch. If the flag-setting result still sits in its
    /// destination register and the spill stores it to a local, the condition must read that local afterwards —
    /// re-evaluating <c>num - 1</c> after <c>num = num - 1</c> was stored would subtract twice.
    /// </summary>
    public void BeforeSpill() =>
        _preSpill = _flags.Source == FlagSource.Result && _resultRegister != Register.None ? regs.Get(_resultRegister) : null;

    public void AfterSpill()
    {
        if (_preSpill is null || !ReferenceEquals(_preSpill, _flags.Left)) return;
        if (regs.Get(_resultRegister) is LocalExpr spilled && !ReferenceEquals(spilled, _preSpill))
            _flags = _flags with { Left = spilled };
        _preSpill = null;
    }

    /// <summary>Emits a conditional branch. <paramref name="isInitPath"/> identifies runtime-initialization blocks.</summary>
    public void EmitConditional(EmitContext ctx, in Instruction ins, NativeBlock target, NativeBlock? fallthrough, Func<NativeBlock, bool> isInitPath)
    {
        if (target.IsTrap) { ElidedChecks++; return; }
        var cond = ConditionMapper.Map(_flags, ins.ConditionCode);
        if (cond is null) return;
        if (ConditionMapper.IsRuntimeCheck(cond))
        {
            ElidedChecks++;
            if (fallthrough is not null && isInitPath(fallthrough) && !isInitPath(target)) ctx.IL.Add(CilOpCodes.Br, target.Label);
            return;
        }
        cond.Emit(ctx);
        ctx.IL.Add(CilOpCodes.Brtrue, target.Label);
    }

    /// <summary>setcc reg → reg = (condition).</summary>
    public void EmitSet(in Instruction ins)
    {
        var cond = ConditionMapper.Map(_flags, ins.ConditionCode);
        regs.Set(ins.Op0Register, cond is null ? new NativeExpr(ins.ToString(), ValueKind.I4) : cond);
    }

    /// <summary>cmovcc dst, src → dst = cond ? src : dst, materialized into a local with a small diamond.</summary>
    public void EmitCmov(EmitContext ctx, in Instruction ins)
    {
        var cond = ConditionMapper.Map(_flags, ins.ConditionCode);
        var src = ops.Read(ins, 1);
        var old = regs.Get(ins.Op0Register);
        if (cond is null) { regs.Set(ins.Op0Register, new NativeExpr(ins.ToString())); return; }

        var f = ctx.Module.CorLibTypeFactory;
        var type = old.Type ?? src.Type ?? (ins.Op0Register.IsGPR64() ? f.Int64 : f.Int32);
        var temp = regs.NewLocal(type);
        var elseLabel = new CilInstructionLabel();
        var endLabel = new CilInstructionLabel();
        cond.Emit(ctx);
        ctx.IL.Add(CilOpCodes.Brfalse, elseLabel);
        Expr.EmitAs(ctx, src, type);
        ctx.IL.Add(CilOpCodes.Stloc, temp);
        ctx.IL.Add(CilOpCodes.Br, endLabel);
        elseLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        Expr.EmitAs(ctx, old, type);
        ctx.IL.Add(CilOpCodes.Stloc, temp);
        endLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        regs.Set(ins.Op0Register, new LocalExpr(temp));
    }
}
