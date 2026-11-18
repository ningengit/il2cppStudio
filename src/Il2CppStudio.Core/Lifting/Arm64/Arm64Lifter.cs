using System.Buffers.Binary;
using AsmArm64;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Arm64;

internal sealed record Arm64Insn(ulong Address, Arm64Instruction Ins);

internal sealed class Arm64Block(int start, ulong va)
{
    public int Start { get; } = start;
    public int End { get; set; }
    public ulong Va { get; } = va;
    public CilInstructionLabel Label { get; } = new();
    public List<Arm64Block> Successors { get; } = [];
    public List<Arm64Block> Predecessors { get; } = [];
    public HashSet<int> Use { get; } = [];
    public HashSet<int> Def { get; } = [];
    public HashSet<int> LiveIn { get; } = [];
    public HashSet<int> LiveOut { get; } = [];
    public bool IsTrap { get; set; }
}

/// <summary>
/// AAPCS64 (ARM64) native method lifter using the managed AsmArm64 decoder, lifting directly into
/// shared Unified IR expressions and CIL method bodies for Android arm64-v8a (and Switch NSO).
/// </summary>
public sealed class Arm64Lifter(NativeSymbolizer sym, StubAssemblyBuilder stubs, IDiagnosticSink sink,
    Runtime.RuntimeHelperMap? helpers = null) : IMethodLifter
{
    private readonly Dictionary<string, int> _typeIndex = MemoryResolver.BuildTypeIndex(sym.Model);

    public LiftResult Lift(int methodIndex)
    {
        ArgumentLocations.Abi = NativeAbi.SysV;
        var md = sym.Metadata;
        var image = stubs.ImageOfType(md.Methods[methodIndex].DeclaringType);
        if (image < 0) return new(methodIndex, false, 0, 0, 0, 0, 0, "no image");
        var stub = stubs.GetModule(image);
        var method = stubs.GetMethodDef(methodIndex);
        var va = sym.Map.GetMethodVa(methodIndex);
        if (method is null || va == 0 || method.IsAbstract) return new(methodIndex, false, 0, 0, 0, 0, 0, "no native code");

        var loc = (ulong ip) => new DiagnosticLocation { Rva = ip - sym.Registration.Reader.ImageBase, MethodName = sym.MethodName(methodIndex) };
        try
        {
            var (start, end) = sym.Map.GetFunctionBounds(va, 0x10000);
            var reader = sym.Registration.Reader;
            var code = reader.Slice(start, (int)Math.Min(end - start, 0x10000UL));
            var insns = DecodeFunction(code, start);
            if (insns.Count == 0) return new(methodIndex, false, 0, 0, 0, 0, 0, "empty function");

            var blocks = BuildBlocks(insns, start, sym);
            ComputeLiveness(insns, blocks, start, end);

            var body = new CilMethodBody { InitializeLocals = true };
            var notes = new List<string>();
            var ctx = new EmitContext(body.Instructions, stub.Module, stub.Intrinsics) { Log = notes.Add };
            var mem = new MemoryResolver(md, sym.Model, sym.Registration, sym.Names, stubs, stub.Module, _typeIndex, pointerSize: 8);
            var shape = MethodShape.Create(md, sym.Registration, stubs, mem, stub.Module, methodIndex, -1);
            var tr = new Arm64BlockTranslator(sym, stubs, stub.Module, method, shape, mem, body, helpers, loc, sink, notes, start, end);

            var order = TopologicalOrder(blocks);
            for (var bi = 0; bi < order.Count; bi++)
            {
                var b = order[bi];
                b.Label.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                tr.EnterBlock(b, isEntry: b == blocks[0]);
                if (b.IsTrap)
                {
                    ctx.IL.Add(CilOpCodes.Ldnull);
                    ctx.IL.Add(CilOpCodes.Throw);
                    continue;
                }
                var nextInEmit = bi + 1 < order.Count ? order[bi + 1] : null;
                tr.TranslateBlock(ctx, insns, b, nextInEmit);
            }

            if (body.Instructions.Count == 0 || body.Instructions[^1].OpCode.Code is not (CilCode.Ret or CilCode.Throw or CilCode.Br))
                tr.EmitDefaultReturn(ctx);

            body.Instructions.OptimizeMacros();
            method.CilMethodBody = body;
            stub.LiftedMethods.Add(methodIndex);

            return new LiftResult(methodIndex, true, insns.Count, body.Instructions.Count, tr.Unsupported, tr.Unresolved, tr.Elided, null)
            {
                Notes = notes,
            };
        }
        catch (Exception ex)
        {
            StubAssemblyBuilder.SetPlaceholderBody(method);
            sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed, $"ARM64 lifting failed: {ex.Message}", loc(va), ex);
            return new(methodIndex, false, 0, 0, 0, 0, 0, ex.Message);
        }
    }

    private static List<Arm64Insn> DecodeFunction(ReadOnlySpan<byte> code, ulong startVa)
    {
        var list = new List<Arm64Insn>(code.Length / 4);
        var aligned = code.Length & ~3;
        for (var off = 0; off < aligned; off += 4)
        {
            var raw = BinaryPrimitives.ReadUInt32LittleEndian(code.Slice(off, 4));
            if (raw == 0) break;
            var ins = Arm64Instruction.Decode(raw);
            if (ins.Id == Arm64InstructionId.Invalid) break;
            list.Add(new Arm64Insn(startVa + (ulong)off, ins));
        }
        return list;
    }

    private static List<Arm64Block> BuildBlocks(List<Arm64Insn> insns, ulong funcStart, NativeSymbolizer sym)
    {
        var indexAtVa = new Dictionary<ulong, int>(insns.Count);
        for (var i = 0; i < insns.Count; i++) indexAtVa[insns[i].Address] = i;
        var funcEnd = insns[^1].Address + 4;

        bool IsTrapAt(ulong targetVa)
        {
            if (!indexAtVa.TryGetValue(targetVa, out var idx)) return false;
            var limit = Math.Min(insns.Count, idx + 4);
            var hasCall = false;
            for (var k = idx; k < limit; k++)
            {
                var m = insns[k].Ins.Mnemonic;
                if (m is Arm64Mnemonic.RET or Arm64Mnemonic.BR) return false;
                if (m == Arm64Mnemonic.BL)
                {
                    if (sym.Map.SymbolAt(BranchTarget(insns[k].Address, insns[k].Ins, 0)) is not null) return false;
                    hasCall = true;
                    continue;
                }
                if (m is Arm64Mnemonic.BRK or Arm64Mnemonic.UDF) return true;
                if (hasCall && m == Arm64Mnemonic.B) return false;
            }
            return hasCall && idx >= insns.Count - 4;
        }

        var leaders = new SortedSet<int> { 0 };
        for (var i = 0; i < insns.Count; i++)
        {
            var ins = insns[i].Ins;
            var pc = insns[i].Address;
            switch (ins.Mnemonic)
            {
                case Arm64Mnemonic.B when ins.OperandCount == 2: // b.cond
                {
                    var target = BranchTarget(pc, ins, 1);
                    if (!IsTrapAt(target) && indexAtVa.TryGetValue(target, out var ti))
                    {
                        leaders.Add(ti);
                        if (i + 1 < insns.Count) leaders.Add(i + 1);
                    }
                    break;
                }
                case Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ:
                {
                    var target = BranchTarget(pc, ins, 1);
                    if (!IsTrapAt(target) && indexAtVa.TryGetValue(target, out var ti))
                    {
                        leaders.Add(ti);
                        if (i + 1 < insns.Count) leaders.Add(i + 1);
                    }
                    break;
                }
                case Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ:
                {
                    var target = BranchTarget(pc, ins, 2);
                    if (!IsTrapAt(target) && indexAtVa.TryGetValue(target, out var ti))
                    {
                        leaders.Add(ti);
                        if (i + 1 < insns.Count) leaders.Add(i + 1);
                    }
                    break;
                }
                case Arm64Mnemonic.B when ins.OperandCount == 1:
                {
                    var target = BranchTarget(pc, ins, 0);
                    if (indexAtVa.TryGetValue(target, out var ti)) leaders.Add(ti);
                    if (i + 1 < insns.Count) leaders.Add(i + 1);
                    break;
                }
                case Arm64Mnemonic.RET or Arm64Mnemonic.BR or Arm64Mnemonic.BRK or Arm64Mnemonic.UDF:
                    if (i + 1 < insns.Count) leaders.Add(i + 1);
                    break;
            }
        }

        var leaderList = leaders.ToList();
        var blocks = new List<Arm64Block>(leaderList.Count);
        var blockAtVa = new Dictionary<ulong, Arm64Block>(leaderList.Count);
        for (var bi = 0; bi < leaderList.Count; bi++)
        {
            var start = leaderList[bi];
            var end = bi + 1 < leaderList.Count ? leaderList[bi + 1] : insns.Count;
            var b = new Arm64Block(start, insns[start].Address) { End = end };
            blocks.Add(b);
            blockAtVa[b.Va] = b;
        }

        for (var bi = 0; bi < blocks.Count; bi++)
        {
            var b = blocks[bi];
            if (b.End <= b.Start) continue;
            // Trap detection: isolated cold block containing only trap BL / BRK / UDF
            var lastIdx = b.End - 1;
            if (lastIdx - b.Start <= 2 && IsTrapAt(b.Va) && b != blocks[0])
            {
                b.IsTrap = true;
                continue;
            }
            var last = insns[lastIdx];
            var m = last.Ins.Mnemonic;
            if (m is Arm64Mnemonic.RET or Arm64Mnemonic.BR or Arm64Mnemonic.BRK or Arm64Mnemonic.UDF) continue;
            if (m == Arm64Mnemonic.B && last.Ins.OperandCount == 1)
            {
                var target = BranchTarget(last.Address, last.Ins, 0);
                if (target >= funcStart && target < funcEnd && blockAtVa.TryGetValue(target, out var tb))
                    b.Successors.Add(tb);
                continue;
            }
            ulong condTarget = 0;
            var isCond = false;
            if (m == Arm64Mnemonic.B && last.Ins.OperandCount == 2) { condTarget = BranchTarget(last.Address, last.Ins, 1); isCond = true; }
            else if (m is Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ) { condTarget = BranchTarget(last.Address, last.Ins, 1); isCond = true; }
            else if (m is Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ) { condTarget = BranchTarget(last.Address, last.Ins, 2); isCond = true; }

            if (isCond && blockAtVa.TryGetValue(condTarget, out var cb) && !cb.IsTrap && !IsTrapAt(condTarget))
                b.Successors.Add(cb);
            if (bi + 1 < blocks.Count && !blocks[bi + 1].IsTrap)
                b.Successors.Add(blocks[bi + 1]);
        }

        foreach (var b in blocks)
            foreach (var s in b.Successors.Distinct())
                s.Predecessors.Add(b);
        return blocks;
    }

    private static List<Arm64Block> TopologicalOrder(List<Arm64Block> blocks)
    {
        var reachable = new HashSet<Arm64Block>();
        var queue = new Queue<Arm64Block>();
        reachable.Add(blocks[0]);
        queue.Enqueue(blocks[0]);
        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            foreach (var s in cur.Successors)
                if (!s.IsTrap && reachable.Add(s)) queue.Enqueue(s);
        }
        return [.. blocks.Where(reachable.Contains)];
    }

    private static void ComputeLiveness(List<Arm64Insn> insns, List<Arm64Block> blocks, ulong funcStart, ulong funcEnd)
    {
        foreach (var b in blocks)
        {
            for (var i = b.Start; i < b.End; i++)
            {
                var ins = insns[i].Ins;
                // Mark register reads/writes conservatively across basic blocks.
                var defs = new List<int>(2);
                var reads = new List<int>(4);
                ClassifyOperands(ins, defs, reads);
                foreach (var r in reads) if (!b.Def.Contains(r)) b.Use.Add(r);
                foreach (var d in defs) b.Def.Add(d);
                if (ins.Mnemonic is Arm64Mnemonic.BL or Arm64Mnemonic.BLR)
                {
                    for (var r = 0; r <= 7; r++) if (!b.Def.Contains(r)) b.Use.Add(r);
                    for (var r = 32; r <= 39; r++) if (!b.Def.Contains(r)) b.Use.Add(r);
                    if (!b.Def.Contains(8)) b.Use.Add(8);
                    for (var r = 0; r <= 18; r++) b.Def.Add(r);
                    for (var r = 32; r <= 39; r++) b.Def.Add(r);
                }
                else if (ins.Mnemonic == Arm64Mnemonic.RET)
                {
                    if (!b.Def.Contains(0)) b.Use.Add(0);
                    for (var r = 32; r <= 35; r++) if (!b.Def.Contains(r)) b.Use.Add(r);
                }
            }
        }
        bool changed;
        do
        {
            changed = false;
            for (var i = blocks.Count - 1; i >= 0; i--)
            {
                var b = blocks[i];
                foreach (var s in b.Successors)
                    foreach (var r in s.LiveIn)
                        changed |= b.LiveOut.Add(r);
                foreach (var r in b.Use) changed |= b.LiveIn.Add(r);
                foreach (var r in b.LiveOut)
                    if (!b.Def.Contains(r)) changed |= b.LiveIn.Add(r);
            }
        } while (changed);
        _ = funcStart;
        _ = funcEnd;
    }

    private static void ClassifyOperands(in Arm64Instruction ins, List<int> defs, List<int> reads)
    {
        if (ins.OperandCount == 0) return;
        var m = ins.Mnemonic;
        var isStore = m is Arm64Mnemonic.STR or Arm64Mnemonic.STRB or Arm64Mnemonic.STRH or Arm64Mnemonic.STUR or Arm64Mnemonic.STP;
        var isCompare = m is Arm64Mnemonic.CMP or Arm64Mnemonic.CMN or Arm64Mnemonic.TST or Arm64Mnemonic.FCMP or Arm64Mnemonic.FCMPE;
        var isBranch = m is Arm64Mnemonic.B or Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ or Arm64Mnemonic.BR or Arm64Mnemonic.BLR;
        var firstIsRead = isStore || isCompare || isBranch;

        for (var oi = 0; oi < ins.OperandCount; oi++)
        {
            var op = ins.GetOperand(oi);
            if (op.Kind == Arm64OperandKind.Register)
            {
                var k = RegKey((Arm64RegisterOperand)op);
                if (k < 0 || k == 99) continue;
                if (oi == 0 && !firstIsRead) defs.Add(k);
                else if (m == Arm64Mnemonic.LDP && oi == 1) defs.Add(k);
                else reads.Add(k);
            }
            else if (op.Kind == Arm64OperandKind.Memory)
            {
                var acc = ((Arm64MemoryOperand)op).Accessor;
                var bk = RawRegKey(acc.BaseRegister);
                if (bk >= 0 && bk != 99) reads.Add(bk);
                if (acc.HasIndexRegister)
                {
                    var ik = RawRegKey(acc.IndexRegister);
                    if (ik >= 0 && ik != 99) reads.Add(ik);
                }
            }
        }
    }

    internal static ulong BranchTarget(ulong pc, in Arm64Instruction ins, int opIndex)
    {
        if (opIndex >= ins.OperandCount) return 0;
        var op = ins.GetOperand(opIndex);
        if (op.Kind != Arm64OperandKind.Label) return 0;
        var lbl = (Arm64LabelOperand)op;
        return unchecked((ulong)((long)pc + lbl.Offset));
    }

    internal static int RegKey(in Arm64RegisterOperand rop) => rop.IsZeroRegister ? -1 : RawRegKey(rop.Value);

    internal static int RawRegKey(Arm64RegisterAny r) => r.Kind switch
    {
        Arm64RegisterKind.X or Arm64RegisterKind.W => r.Index == 31 ? -1 : r.Index,
        Arm64RegisterKind.SP or Arm64RegisterKind.WSP => 99,
        Arm64RegisterKind.Invalid or Arm64RegisterKind.C => -1,
        _ => 32 + (r.Index & 31),
    };
}
