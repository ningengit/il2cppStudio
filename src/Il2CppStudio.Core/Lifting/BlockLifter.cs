using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Per-instruction dispatch inside one basic block; returns true when the block was terminated.</summary>
public sealed class BlockLifter(
    BlockGraph graph, RegisterTracker regs, InstructionTranslators arith, BranchTranslator branch,
    CallDispatcher calls, ReturnEmitter returns, Func<NativeBlock, bool> isInitPath,
    LoadMaterializer? loads = null, Func<ulong, Runtime.RuntimeHelperKind>? helperKind = null)
{
    public int Unsupported { get; private set; }

    /// <summary>Helper calls that cannot modify managed fields (no need to pin earlier loads).</summary>
    private bool IsPureHelperCall(in Instruction ins) =>
        ins.FlowControl == FlowControl.Call && helperKind is not null && ins.Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32
        && helperKind(ins.NearBranchTarget) is Runtime.RuntimeHelperKind.WriteBarrier or Runtime.RuntimeHelperKind.MetadataInit
            or Runtime.RuntimeHelperKind.ObjectNew or Runtime.RuntimeHelperKind.ArrayNew or Runtime.RuntimeHelperKind.Box or Runtime.RuntimeHelperKind.Cast;

    /// <summary>True when the call's result is null-tested right away (`test rax,rax`) — isinst rather than castclass.</summary>
    private bool NextTestsRax(NativeBlock block, int i)
    {
        for (var k = i + 1; k < Math.Min(graph.Instructions.Count, i + 4); k++)
        {
            var x = graph.Instructions[k];
            if (x.Mnemonic == Mnemonic.Test && x.Op0Kind == OpKind.Register && x.Op0Register.GetFullRegister() == Register.RAX) return true;
            if (x.Op0Kind == OpKind.Register && x.Op0Register.GetFullRegister() == Register.RAX) return false;   // rax overwritten
        }
        return false;
    }

    public bool Lift(EmitContext ctx, NativeBlock block, NativeBlock? next)
    {
        for (var i = block.Start; i < block.End; i++)
        {
            if (graph.Elided.Contains(i)) continue;
            var ins = graph.Instructions[i];
            if (branch.Observe(ins)) continue;
            if (loads is not null && LoadMaterializer.WritesMemory(ins) && !IsPureHelperCall(ins)) loads.BeforeMemoryWrite(ctx, block, i);

            switch (ins.FlowControl)
            {
                case FlowControl.Call or FlowControl.IndirectCall:
                    if (calls.Handle(ctx, ins, (r, _) => regs.Get(r), NextTestsRax(block, i))) return true;   // no-return helper emitted a throw
                    continue;
                case FlowControl.Return:
                    returns.Emit(ctx);
                    return true;
                case FlowControl.ConditionalBranch:
                    branch.BeforeSpill();
                    regs.SpillLiveOut(ctx, block);
                    branch.AfterSpill();
                    if (graph.BlockAt(ins.NearBranchTarget) is { } t) branch.EmitConditional(ctx, ins, t, next, isInitPath);
                    continue;
                case FlowControl.UnconditionalBranch:
                    if (graph.BlockAt(ins.NearBranchTarget) is null && !graph.TryIndexOf(ins.NearBranchTarget, out _))
                    {
                        // Tail call (jmp to another function): call it, then return its result.
                        if (calls.Handle(ctx, ins, (r, _) => regs.Get(r))) return true;
                        returns.Emit(ctx);
                        return true;
                    }
                    regs.SpillLiveOut(ctx, block);
                    if (graph.BlockAt(ins.NearBranchTarget) is { } u && !u.IsTrap) ctx.IL.Add(AsmResolver.PE.DotNet.Cil.CilOpCodes.Br, u.Label);
                    return true;
                case FlowControl.Interrupt:
                    continue;   // int3 padding / after noreturn calls
            }

            var conditional = ins.ConditionCode != ConditionCode.None;
            if (conditional && ins.Mnemonic.ToString().StartsWith("Set", StringComparison.Ordinal)) { branch.EmitSet(ins); continue; }
            if (conditional && ins.Mnemonic.ToString().StartsWith("Cmov", StringComparison.Ordinal)) { branch.EmitCmov(ctx, ins); continue; }
            if (arith.Translate(ctx, ins))
            {
                if (ins.RflagsModified != 0) branch.NoteResult(ins.OpCount > 0 && ins.Op0Kind == OpKind.Register ? ins.Op0Register : Register.None);
                continue;
            }
            if (ins.Mnemonic is Mnemonic.Nop or Mnemonic.Int3 or Mnemonic.Prefetchw or Mnemonic.Prefetcht0) continue;
            Unsupported++;
            ConversionLog.Record(ctx, $"unsupported instruction at 0x{ins.IP:X}: {ins}");
            if (ins.Op0Kind == OpKind.Register && ins.OpCount > 0) regs.Set(ins.Op0Register, new NativeExpr(ins.ToString()));
        }
        regs.SpillLiveOut(ctx, block);
        return false;
    }
}
