using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Gives memory loads load-time semantics. Expressions are built lazily (a register holds "this.field", evaluated
/// where it is used), which is wrong if memory changes in between. Before any instruction that can write memory —
/// a store or a call — every live register value that reads memory is evaluated into a local, so it keeps the value
/// it had at the point of the native load. Dead values are left alone (no spurious locals in the output).
/// </summary>
public sealed class LoadMaterializer(BlockGraph graph, RegisterTracker regs)
{
    public int Materialized { get; private set; }

    public void BeforeMemoryWrite(EmitContext ctx, NativeBlock block, int index)
    {
        foreach (var (r, e) in regs.Registers)
        {
            if (e is StructChunkExpr || !ReadsMemory(e) || !graph.IsLiveAfter(block, index, r)) continue;   // chunks are copied field-wise later
            var local = regs.NewLocal(regs.TypeFor(e));
            Expr.EmitAs(ctx, e, local.VariableType);
            ctx.IL.Add(CilOpCodes.Stloc, local);
            var materialized = new LocalExpr(local);
            regs.ReplaceAll(e, materialized);   // same tree held by other registers / slots
            Materialized++;
        }
    }

    /// <summary>True when evaluating the expression reads mutable memory (fields, statics, array elements).</summary>
    public static bool ReadsMemory(Expr e) => e switch
    {
        // Fields of struct-typed locals / arguments are not memory that a store or call can change (each struct value
        // written to the stack gets its own fresh local); fields of objects, statics and array elements are.
        FieldExpr { Instance: LocalExpr { Kind: ValueKind.Struct } or ArgExpr { Kind: ValueKind.Struct } } => false,
        FieldExpr { Instance: { Kind: ValueKind.Struct } inner } => ReadsMemory(inner),
        FieldExpr or StaticFieldExpr or ArrayElementExpr or ThisStructExpr => true,
        StructChunkExpr c => ReadsMemory(c.Source),
        BinaryExpr b => ReadsMemory(b.Left) || ReadsMemory(b.Right),
        UnaryExpr u => ReadsMemory(u.Operand),
        ConvExpr c => ReadsMemory(c.Operand),
        CompareExpr c => ReadsMemory(c.Left) || ReadsMemory(c.Right),
        ArrayLengthExpr a => ReadsMemory(a.Array),     // length itself is immutable
        StringLengthExpr s => ReadsMemory(s.String),
        _ => false,
    };

    /// <summary>True for instructions that may write managed memory.</summary>
    public static bool WritesMemory(in Instruction ins) =>
        ins.FlowControl == FlowControl.Call
        || ins.OpCount > 0 && ins.Op0Kind == OpKind.Memory && ins.MemoryBase != Register.RSP
           && ins.Mnemonic is not (Mnemonic.Cmp or Mnemonic.Test or Mnemonic.Lea or Mnemonic.Push or Mnemonic.Prefetchw);
}
