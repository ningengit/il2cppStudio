using System.Buffers.Binary;
using System.Collections.Concurrent;
using AsmArm64;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Arm64;

internal sealed record Arm64Insn(ulong Address, Arm64Instruction Ins);

internal sealed record Arm64JumpTable(int IndexReg, ulong[] Targets, ulong DefaultTargetVa);

internal sealed record Arm64InterfaceCall(int IfaceReg, int Slot, int CallReg, ulong SkipLdpVa);

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
    public HashSet<int> NonZeroDispRegs { get; } = [];
    public HashSet<int> WrittenOrEscapedRegs { get; } = [];
    public bool IsTrap { get; set; }
    public Arm64JumpTable? JumpTable { get; set; }
    public Arm64Block? InterfaceRedirect { get; set; }
    public Arm64InterfaceCall? InterfaceCall { get; set; }
}

/// <summary>
/// AAPCS64 (ARM64) native method lifter using the managed AsmArm64 decoder, lifting directly into
/// shared Unified IR expressions and CIL method bodies for Android arm64-v8a (and Switch NSO).
/// </summary>
public sealed class Arm64Lifter(NativeSymbolizer sym, StubAssemblyBuilder stubs, IDiagnosticSink sink,
    Runtime.RuntimeHelperMap? helpers = null, IReadOnlyDictionary<ulong, string>? symbolNames = null) : IMethodLifter
{
    private readonly Dictionary<string, int> _typeIndex = MemoryResolver.BuildTypeIndex(sym.Model);
    private readonly ConcurrentDictionary<ulong, Arm64HelperKind> _helperCache = new();

    public bool SupportsParallelLifting => true;

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
            var insns = DecodeFunction(reader, code, start, out var failedVa, out var failedBytesHex);
            if (failedVa != 0)
            {
                sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.DisassemblyFailed,
                    $"ARM64 decode failed at RVA 0x{failedVa - reader.ImageBase:X} (offset +0x{failedVa - start:X}): raw bytes [{failedBytesHex}]",
                    loc(failedVa));
            }
            if (insns.Count == 0) return new(methodIndex, false, 0, 0, 0, 0, 0, "empty function");

            var blocks = BuildBlocks(insns, start, sym, _helperCache, symbolNames);
            ComputeLiveness(insns, blocks, start, end);

            var body = new CilMethodBody { InitializeLocals = true };
            var notes = new List<string>();
            var ctx = new EmitContext(body.Instructions, stub.Module, stub.Intrinsics) { Log = notes.Add };
            var mem = new MemoryResolver(md, sym.Model, sym.Registration, sym.Names, stubs, stub.Module, _typeIndex, pointerSize: 8);
            var shape = MethodShape.Create(md, sym.Registration, stubs, mem, stub.Module, methodIndex, -1);
            var tr = new Arm64BlockTranslator(sym, stubs, stub.Module, method, shape, mem, body, helpers, loc, sink, notes, start, end, _helperCache, symbolNames);

            var order = TopologicalOrder(blocks);
            var emitted = order.ToHashSet();
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
                tr.TranslateBlock(ctx, insns, b, nextInEmit, emitted);
            }

            if (body.Instructions.Count == 0 || body.Instructions[^1].OpCode.Code is not (CilCode.Ret or CilCode.Throw or CilCode.Br))
                tr.EmitDefaultReturn(ctx);

            body.Instructions.OptimizeMacros();
            method.CilMethodBody = body;
            stub.MarkLifted(methodIndex);

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

    private static List<Arm64Insn> DecodeFunction(Binary.VirtualReader reader, ReadOnlySpan<byte> code, ulong startVa, out ulong failedVa, out string? failedBytesHex)
    {
        failedVa = 0;
        failedBytesHex = null;
        var list = new List<Arm64Insn>(code.Length / 4);
        var aligned = code.Length & ~3;
        var endVa = startVa + (ulong)aligned;
        var maxBranchTarget = startVa;
        var hasIndirectBranch = false;

        for (var off = 0; off < aligned; off += 4)
        {
            var raw = BinaryPrimitives.ReadUInt32LittleEndian(code.Slice(off, 4));
            if (raw == 0) break;
            var ins = Arm64Instruction.Decode(raw);
            if (ins.Id == Arm64InstructionId.Invalid)
            {
                if (list.Count == 0 || !IsArm64Terminator(list[^1].Ins))
                {
                    failedVa = startVa + (ulong)off;
                    failedBytesHex = Convert.ToHexString(code.Slice(off, 4));
                }
                break;
            }
            var va = startVa + (ulong)off;
            if (ins.Mnemonic == Arm64Mnemonic.B && ins.OperandCount == 1 && ins.GetOperand(0).Kind == Arm64OperandKind.Label)
            {
                var target = BranchTarget(va, ins, 0);
                if ((target < startVa || target >= endVa) && TryInlineCortexA53Veneer(reader, va, target, out var inlined))
                    ins = inlined;
            }
            list.Add(new Arm64Insn(va, ins));

            if (ins.Mnemonic == Arm64Mnemonic.BR)
            {
                hasIndirectBranch = true;
            }
            else if (ins.Mnemonic is Arm64Mnemonic.B or Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ)
            {
                for (var oi = 0; oi < ins.OperandCount; oi++)
                {
                    if (ins.GetOperand(oi).Kind == Arm64OperandKind.Label)
                    {
                        var target = BranchTarget(va, ins, oi);
                        if (target >= startVa && target < endVa && target > maxBranchTarget)
                            maxBranchTarget = target;
                    }
                }
            }

            if (!hasIndirectBranch && va + 4 > maxBranchTarget && IsUnconditionalArm64Terminator(ins))
                break;
        }
        return list;
    }

    private static bool TryInlineCortexA53Veneer(Binary.VirtualReader reader, ulong callSiteVa, ulong veneerVa, out Arm64Instruction inlined)
    {
        inlined = default;
        if (!reader.IsMapped(veneerVa) || !reader.IsMapped(veneerVa + 7)) return false;
        var bytes = reader.Slice(veneerVa, 8);
        if (bytes.Length < 8) return false;
        var v0 = Arm64Instruction.Decode(BinaryPrimitives.ReadUInt32LittleEndian(bytes[..4]));
        var v1 = Arm64Instruction.Decode(BinaryPrimitives.ReadUInt32LittleEndian(bytes.Slice(4, 4)));
        if (v0.Id == Arm64InstructionId.Invalid || IsArm64Terminator(v0)) return false;
        if (v0.Mnemonic is Arm64Mnemonic.ADRP or Arm64Mnemonic.ADR) return false;
        if (v0.OperandCount >= 2 && v0.GetOperand(1).Kind == Arm64OperandKind.Label) return false;
        if (v1.Mnemonic != Arm64Mnemonic.B || v1.OperandCount != 1 || BranchTarget(veneerVa + 4, v1, 0) != callSiteVa + 4)
            return false;
        inlined = v0;
        return true;
    }

    private static bool IsUnconditionalArm64Terminator(Arm64Instruction ins) =>
        (ins.Mnemonic != Arm64Mnemonic.B || ins.OperandCount == 1) && IsArm64Terminator(ins);

    private static bool IsArm64Terminator(Arm64Instruction ins) =>
        ins.Mnemonic is Arm64Mnemonic.RET or Arm64Mnemonic.B or Arm64Mnemonic.BR or Arm64Mnemonic.BRK or Arm64Mnemonic.UDF
        || ins.Mnemonic.ToString() is "RETAA" or "RETAB" or "BRAA" or "BRAB" or "BRAAZ" or "BRABZ" or "HLT";

    private static List<Arm64Block> BuildBlocks(List<Arm64Insn> insns, ulong funcStart, NativeSymbolizer sym,
        ConcurrentDictionary<ulong, Arm64HelperKind> helperCache, IReadOnlyDictionary<ulong, string>? symbolNames)
    {
        var indexAtVa = new Dictionary<ulong, int>(insns.Count);
        for (var i = 0; i < insns.Count; i++) indexAtVa[insns[i].Address] = i;
        var funcEnd = insns[^1].Address + 4;
        var effectiveEnd = insns.Count;
        while (effectiveEnd > 0 && insns[effectiveEnd - 1].Ins.Mnemonic == Arm64Mnemonic.NOP)
            effectiveEnd--;
        var reader = sym.Registration.Reader;

        bool IsTrapAt(ulong targetVa)
        {
            if (!indexAtVa.TryGetValue(targetVa, out var idx)) return false;
            var limit = Math.Min(effectiveEnd, idx + 10);
            var hasCall = false;
            var k = idx;
            for (; k < limit; k++)
            {
                var ins = insns[k].Ins;
                var m = ins.Mnemonic;
                if (m is Arm64Mnemonic.RET or Arm64Mnemonic.BR) return false;
                if (m is Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ
                    || (m == Arm64Mnemonic.B && ins.OperandCount == 2))
                    return false;
                if (m is Arm64Mnemonic.BRK or Arm64Mnemonic.UDF) return true;
                if (m == Arm64Mnemonic.BL)
                {
                    var target = BranchTarget(insns[k].Address, ins, 0);
                    if (sym.Map.SymbolAt(target) is not null) return false;
                    if (Arm64BlockTranslator.ClassifyNativeHelper(reader, helperCache, target, symbolNames) == Arm64HelperKind.NoReturn)
                        return true;
                    hasCall = true;
                    continue;
                }
                if (m == Arm64Mnemonic.B)
                {
                    var target = BranchTarget(insns[k].Address, ins, 0);
                    if (sym.Map.SymbolAt(target) is not null) return false;
                    if ((target < funcStart || target >= funcEnd)
                        && Arm64BlockTranslator.ClassifyNativeHelper(reader, helperCache, target, symbolNames) == Arm64HelperKind.NoReturn)
                        return true;
                    return false;
                }
            }
            return hasCall && k >= effectiveEnd;
        }

        bool IsMathErrnoBranch(int i)
        {
            if (i <= 0) return false;
            var ins = insns[i].Ins;
            if (ins.Mnemonic != Arm64Mnemonic.B || ins.OperandCount != 2) return false;
            if (((Arm64EnumOperand)ins.GetOperand(0)).AsConditional != Arm64ConditionalKind.VS) return false;
            var prev = insns[i - 1].Ins;
            return prev.Mnemonic is Arm64Mnemonic.FCMP or Arm64Mnemonic.FCMPE
                && prev.OperandCount >= 2
                && prev.GetOperand(0).Kind == Arm64OperandKind.Register
                && prev.GetOperand(1).Kind == Arm64OperandKind.Register
                && RawRegKey(((Arm64RegisterOperand)prev.GetOperand(0)).Value) == RawRegKey(((Arm64RegisterOperand)prev.GetOperand(1)).Value);
        }

        bool IsColdInitFallthrough(int branchIdx, ulong targetVa)
        {
            if (!indexAtVa.TryGetValue(targetVa, out var targetIdx) || targetIdx <= branchIdx + 1 || targetIdx - branchIdx > 10)
                return false;
            var bi = insns[branchIdx].Ins;
            if (bi.Mnemonic is Arm64Mnemonic.TBNZ && bi.OperandCount >= 3 && ((Arm64ImmediateOperand)bi.GetOperand(1)).Value == 0)
            {
                var hasInitCall = false;
                for (var k = branchIdx + 1; k < targetIdx; k++)
                {
                    var km = insns[k].Ins.Mnemonic;
                    if (km is Arm64Mnemonic.RET or Arm64Mnemonic.BR or Arm64Mnemonic.B) return false;
                    if (km == Arm64Mnemonic.BL)
                    {
                        if (sym.Map.SymbolAt(BranchTarget(insns[k].Address, insns[k].Ins, 0)) is not null) return false;
                        hasInitCall = true;
                    }
                }
                return hasInitCall && insns[targetIdx - 1].Ins.Mnemonic == Arm64Mnemonic.STRB;
            }
            if (bi.Mnemonic == Arm64Mnemonic.TBZ && bi.OperandCount >= 3 && ((Arm64ImmediateOperand)bi.GetOperand(1)).Value == 1)
            {
                if (insns[targetIdx - 1].Ins.Mnemonic == Arm64Mnemonic.BL
                    && sym.Map.SymbolAt(BranchTarget(insns[targetIdx - 1].Address, insns[targetIdx - 1].Ins, 0)) is null
                    && Enumerable.Range(branchIdx + 1, targetIdx - branchIdx - 2).All(k =>
                        insns[k].Ins.Mnemonic is Arm64Mnemonic.LDR or Arm64Mnemonic.CBNZ))
                    return true;
                if (targetIdx == branchIdx + 3
                    && insns[branchIdx + 1].Ins.Mnemonic == Arm64Mnemonic.LDR
                    && insns[branchIdx + 2].Ins.Mnemonic == Arm64Mnemonic.CBZ)
                {
                    var coldVa = BranchTarget(insns[branchIdx + 2].Address, insns[branchIdx + 2].Ins, 1);
                    if (indexAtVa.TryGetValue(coldVa, out var coldIdx)
                        && insns[coldIdx].Ins.Mnemonic == Arm64Mnemonic.BL
                        && sym.Map.SymbolAt(BranchTarget(insns[coldIdx].Address, insns[coldIdx].Ins, 0)) is null)
                        return true;
                }
            }
            if (bi.Mnemonic == Arm64Mnemonic.CBNZ)
            {
                if (insns[targetIdx - 1].Ins.Mnemonic != Arm64Mnemonic.BL) return false;
                if (sym.Map.SymbolAt(BranchTarget(insns[targetIdx - 1].Address, insns[targetIdx - 1].Ins, 0)) is not null) return false;
                for (var k = branchIdx + 1; k < targetIdx - 1; k++)
                {
                    var km = insns[k].Ins.Mnemonic;
                    if (km is not (Arm64Mnemonic.LDR or Arm64Mnemonic.CBNZ)) return false;
                    if (km == Arm64Mnemonic.CBNZ && BranchTarget(insns[k].Address, insns[k].Ins, 1) != targetVa) return false;
                }
                return true;
            }
            return false;
        }

        var leaders = new SortedSet<int> { 0 };
        var jumpTablesByBrIdx = new Dictionary<int, Arm64JumpTable>();
        for (var i = 0; i < insns.Count; i++)
        {
            var ins = insns[i].Ins;
            var pc = insns[i].Address;
            switch (ins.Mnemonic)
            {
                case Arm64Mnemonic.B when ins.OperandCount == 2: // b.cond
                {
                    if (IsMathErrnoBranch(i)) break;
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
                case Arm64Mnemonic.BR:
                {
                    if (TryDecodeJumpTable(insns, i, indexAtVa, reader) is { } jt)
                    {
                        jumpTablesByBrIdx[i] = jt;
                        foreach (var tVa in jt.Targets)
                            if (indexAtVa.TryGetValue(tVa, out var ti)) leaders.Add(ti);
                        if (jt.DefaultTargetVa != 0 && !IsTrapAt(jt.DefaultTargetVa) && indexAtVa.TryGetValue(jt.DefaultTargetVa, out var di))
                            leaders.Add(di);
                    }
                    if (i + 1 < insns.Count) leaders.Add(i + 1);
                    break;
                }
                case Arm64Mnemonic.RET or Arm64Mnemonic.BRK or Arm64Mnemonic.UDF:
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
            if (end > start && jumpTablesByBrIdx.TryGetValue(end - 1, out var jt))
                b.JumpTable = jt;
            blocks.Add(b);
            blockAtVa[b.Va] = b;
        }

        // Mark cold trap blocks first so no unconditional or fallthrough edge ever targets a skipped trap block.
        foreach (var b in blocks)
        {
            if (b.End <= b.Start || b == blocks[0]) continue;
            var lastIdx = b.End - 1;
            if (lastIdx - b.Start <= 8 && IsTrapAt(b.Va))
                b.IsTrap = true;
        }

        for (var bi = 0; bi < blocks.Count; bi++)
        {
            var b = blocks[bi];
            if (b.End <= b.Start || b.IsTrap) continue;
            var lastIdx = b.End - 1;
            var last = insns[lastIdx];
            var m = last.Ins.Mnemonic;
            if (m == Arm64Mnemonic.BR && b.JumpTable is { } jt)
            {
                foreach (var tVa in jt.Targets.Distinct())
                {
                    if (blockAtVa.TryGetValue(tVa, out var tb) && !tb.IsTrap && !IsTrapAt(tVa))
                        b.Successors.Add(tb);
                }
                if (jt.DefaultTargetVa != 0 && blockAtVa.TryGetValue(jt.DefaultTargetVa, out var db) && !db.IsTrap && !IsTrapAt(jt.DefaultTargetVa) && !b.Successors.Contains(db))
                    b.Successors.Add(db);
                continue;
            }
            if (m is Arm64Mnemonic.RET or Arm64Mnemonic.BR or Arm64Mnemonic.BRK or Arm64Mnemonic.UDF) continue;
            if (m == Arm64Mnemonic.B && last.Ins.OperandCount == 1)
            {
                var target = BranchTarget(last.Address, last.Ins, 0);
                if (target >= funcStart && target < funcEnd && blockAtVa.TryGetValue(target, out var tb) && !tb.IsTrap && !IsTrapAt(target))
                    b.Successors.Add(tb);
                continue;
            }
            ulong condTarget = 0;
            var isCond = false;
            if (m == Arm64Mnemonic.B && last.Ins.OperandCount == 2 && !IsMathErrnoBranch(lastIdx))
            { condTarget = BranchTarget(last.Address, last.Ins, 1); isCond = true; }
            else if (m is Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ) { condTarget = BranchTarget(last.Address, last.Ins, 1); isCond = true; }
            else if (m is Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ) { condTarget = BranchTarget(last.Address, last.Ins, 2); isCond = true; }

            var addedTarget = isCond && blockAtVa.TryGetValue(condTarget, out var cb) && !cb.IsTrap && !IsTrapAt(condTarget);
            if (addedTarget)
                b.Successors.Add(blockAtVa[condTarget]);
            if (bi + 1 < blocks.Count && !blocks[bi + 1].IsTrap && !(addedTarget && IsColdInitFallthrough(lastIdx, condTarget)))
                b.Successors.Add(blocks[bi + 1]);
        }

        CollapseInterfaceDiamonds(insns, blocks);

        var reachable = new HashSet<Arm64Block>();
        var q = new Queue<Arm64Block>();
        reachable.Add(blocks[0]);
        q.Enqueue(blocks[0]);
        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            foreach (var s in cur.Successors)
                if (!s.IsTrap && reachable.Add(s)) q.Enqueue(s);
        }

        foreach (var b in blocks)
        {
            var definedHere = new HashSet<int>();
            for (var i = b.Start; i < b.End; i++)
            {
                var ins = insns[i].Ins;
                var m = ins.Mnemonic;
                for (var oi = 0; oi < ins.OperandCount; oi++)
                {
                    if (ins.GetOperand(oi).Kind != Arm64OperandKind.Memory) continue;
                    var acc = ((Arm64MemoryOperand)ins.GetOperand(oi)).Accessor;
                    var bk = RawRegKey(acc.BaseRegister);
                    if (acc.Immediate != 0 && acc.BaseRegister.Kind != Arm64RegisterKind.SP && bk >= 0)
                        b.NonZeroDispRegs.Add(bk);
                    if (bk >= 0 && bk != 99 && !definedHere.Contains(bk))
                    {
                        var isSimpleLoad = m is Arm64Mnemonic.LDR or Arm64Mnemonic.LDP or Arm64Mnemonic.LDUR
                            && !acc.IsPreIncrement
                            && (ins.OperandCount <= oi + 1 || ins.GetOperand(oi + 1).Kind != Arm64OperandKind.Immediate);
                        if (!isSimpleLoad)
                            b.WrittenOrEscapedRegs.Add(bk);
                    }
                    if (acc.HasIndexRegister)
                    {
                        var ik = RawRegKey(acc.IndexRegister);
                        if (ik >= 0 && ik != 99 && !definedHere.Contains(ik)) b.WrittenOrEscapedRegs.Add(ik);
                    }
                }
                var defs = new List<int>(2);
                var reads = new List<int>(4);
                ClassifyOperands(ins, defs, reads);
                foreach (var r in reads)
                {
                    if (!definedHere.Contains(r) && !(m is Arm64Mnemonic.LDR or Arm64Mnemonic.LDP or Arm64Mnemonic.LDUR))
                        b.WrittenOrEscapedRegs.Add(r);
                }
                if (m is Arm64Mnemonic.BL or Arm64Mnemonic.BLR)
                {
                    for (var r = 0; r <= 7; r++)
                        if (!definedHere.Contains(r)) b.WrittenOrEscapedRegs.Add(r);
                    for (var r = 0; r <= 18; r++) definedHere.Add(r);
                }
                else if (m == Arm64Mnemonic.RET && !definedHere.Contains(0))
                    b.WrittenOrEscapedRegs.Add(0);
                foreach (var d in defs) definedHere.Add(d);
            }
            if (reachable.Contains(b))
                foreach (var s in b.Successors.Distinct())
                    s.Predecessors.Add(b);
        }
        return blocks;
    }

    private static void CollapseInterfaceDiamonds(List<Arm64Insn> insns, List<Arm64Block> blocks)
    {
        foreach (var hdr in blocks)
        {
            if (hdr.IsTrap || hdr.End - hdr.Start < 2 || hdr.Successors.Count != 2) continue;
            var last = insns[hdr.End - 1].Ins;
            if (last.Mnemonic is not (Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ)) continue;

            // Header loads interface_offsets_count (ldrh w_, [rClass, #260..340]) before cbz -> slowBlock.
            var foundCountLoad = false;
            for (var i = hdr.Start; i < hdr.End - 1; i++)
            {
                var ins = insns[i].Ins;
                if (ins.Mnemonic == Arm64Mnemonic.LDRH && ins.OperandCount >= 2 && ins.GetOperand(1).Kind == Arm64OperandKind.Memory
                    && ((Arm64MemoryOperand)ins.GetOperand(1)).Accessor is { BaseRegister.Kind: not Arm64RegisterKind.SP, Immediate: >= 260 and <= 340 })
                {
                    foundCountLoad = true;
                    break;
                }
            }
            if (!foundCountLoad) continue;

            var slow = hdr.Successors[0];
            var loopInit = hdr.Successors[1];
            if (slow.IsTrap || loopInit.IsTrap || slow.Successors.Count != 1 || loopInit.Successors.Count != 1) continue;
            var callBlock = slow.Successors[0];
            var loopHead = loopInit.Successors[0];
            if (loopHead.Successors.Count != 2) continue;
            var fast = loopHead.Successors[0];
            var loopTail = loopHead.Successors[1];
            if (fast.Successors.Count != 1 || fast.Successors[0] != callBlock) continue;
            if (loopTail.Successors.Count != 2 || loopTail.Successors[0] != loopHead || loopTail.Successors[1] != slow)
                continue;

            var ifaceReg = -1;
            for (var i = loopHead.Start; i < loopHead.End; i++)
            {
                var li = insns[i].Ins;
                if (li.Mnemonic == Arm64Mnemonic.CMP && li.OperandCount >= 2 && li.GetOperand(1).Kind == Arm64OperandKind.Register)
                    ifaceReg = RegKey((Arm64RegisterOperand)li.GetOperand(1));
            }
            var hasStride16 = false;
            for (var i = loopTail.Start; i < loopTail.End; i++)
            {
                var ti = insns[i].Ins;
                if (ti.Mnemonic == Arm64Mnemonic.ADD && ti.OperandCount >= 3 && ti.GetOperand(2).Kind == Arm64OperandKind.Immediate
                    && ((Arm64ImmediateOperand)ti.GetOperand(2)).Value == 16)
                    hasStride16 = true;
            }
            if (ifaceReg < 0 || !hasStride16 || callBlock.End - callBlock.Start < 2) continue;

            var callFirst = insns[callBlock.Start].Ins;
            if (callFirst.Mnemonic != Arm64Mnemonic.LDP || callFirst.OperandCount < 3
                || callFirst.GetOperand(2).Kind != Arm64OperandKind.Memory
                || RawRegKey(((Arm64MemoryOperand)callFirst.GetOperand(2)).Accessor.BaseRegister) != 0)
                continue;

            var callReg = -1;
            for (var i = callBlock.Start + 1; i < callBlock.End; i++)
            {
                var ci = insns[i].Ins;
                if (ci.Mnemonic == Arm64Mnemonic.BLR && ci.OperandCount >= 1 && ci.GetOperand(0).Kind == Arm64OperandKind.Register)
                {
                    callReg = RegKey((Arm64RegisterOperand)ci.GetOperand(0));
                    break;
                }
            }
            if (callReg < 0) continue;

            var slot = 0;
            for (var i = fast.Start; i < fast.End; i++)
            {
                var fi = insns[i].Ins;
                if (fi.Mnemonic == Arm64Mnemonic.ADD && fi.OperandCount >= 3 && fi.GetOperand(2).Kind == Arm64OperandKind.Immediate)
                {
                    var imm = ((Arm64ImmediateOperand)fi.GetOperand(2)).Value;
                    if (imm is > 0 and < 100) slot = (int)imm;
                }
            }
            if (slot == 0)
            {
                for (var i = slow.Start; i < slow.End; i++)
                {
                    var si = insns[i].Ins;
                    if (si.Mnemonic is Arm64Mnemonic.MOV or Arm64Mnemonic.MOVZ && si.OperandCount >= 2
                        && si.GetOperand(0).Kind == Arm64OperandKind.Register && RegKey((Arm64RegisterOperand)si.GetOperand(0)) == 2
                        && si.GetOperand(1).Kind == Arm64OperandKind.Immediate)
                        slot = (int)((Arm64ImmediateOperand)si.GetOperand(1)).Value;
                }
            }

            hdr.Successors.Clear();
            hdr.Successors.Add(callBlock);
            hdr.InterfaceRedirect = callBlock;
            callBlock.InterfaceCall = new Arm64InterfaceCall(ifaceReg, slot, callReg, insns[callBlock.Start].Address);
        }
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
                if (b.InterfaceCall is { } skipIc && insns[i].Address == skipIc.SkipLdpVa)
                    continue;
                var ins = insns[i].Ins;
                // Mark register reads/writes conservatively across basic blocks.
                var defs = new List<int>(2);
                var reads = new List<int>(4);
                ClassifyOperands(ins, defs, reads);
                foreach (var r in reads) if (!b.Def.Contains(r)) b.Use.Add(r);
                foreach (var d in defs) b.Def.Add(d);
                var isTailCall = (ins.Mnemonic == Arm64Mnemonic.B && ins.OperandCount == 1
                        && BranchTarget(insns[i].Address, ins, 0) is var bt && (bt < funcStart || bt >= funcEnd))
                    || (ins.Mnemonic == Arm64Mnemonic.BR && b.Successors.Count == 0);
                if (ins.Mnemonic is Arm64Mnemonic.BL or Arm64Mnemonic.BLR || isTailCall)
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
            if (b.JumpTable is { } jt && !b.Def.Contains(jt.IndexReg))
                b.Use.Add(jt.IndexReg);
            if (b.InterfaceCall is { } ic)
            {
                if (!b.Def.Contains(ic.IfaceReg)) b.Use.Add(ic.IfaceReg);
                if (!b.Def.Contains(0)) b.Use.Add(0);
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

    private static Arm64JumpTable? TryDecodeJumpTable(
        List<Arm64Insn> insns, int brIdx, Dictionary<ulong, int> indexAtVa, Binary.VirtualReader reader)
    {
        if (insns[brIdx].Ins.OperandCount < 1 || insns[brIdx].Ins.GetOperand(0).Kind != Arm64OperandKind.Register)
            return null;
        var targetReg = RegKey((Arm64RegisterOperand)insns[brIdx].Ins.GetOperand(0));
        if (targetReg < 0) return null;

        var earliest = brIdx;
        var floor = Math.Max(0, brIdx - 20);

        // 1. Optional `mov xTarget, xTmp` + `add xTarget, xBase, xOff(, shift/extend)`.
        var baseReg = -1;
        var offReg = -1;
        var addShift = 0;
        var addIdx = -1;
        for (var k = brIdx - 1; k >= floor; k--)
        {
            var ins = insns[k].Ins;
            var m = ins.Mnemonic;
            if (k < brIdx - 1 && (IsArm64Terminator(ins) || m is Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ or Arm64Mnemonic.BL or Arm64Mnemonic.BLR))
                break;
            if (m == Arm64Mnemonic.MOV && ins.OperandCount == 2 && ins.GetOperand(0).Kind == Arm64OperandKind.Register
                && ins.GetOperand(1).Kind == Arm64OperandKind.Register && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == targetReg)
            {
                targetReg = RegKey((Arm64RegisterOperand)ins.GetOperand(1));
                earliest = Math.Min(earliest, k);
                continue;
            }
            if (m == Arm64Mnemonic.ADD && ins.OperandCount >= 3 && ins.GetOperand(0).Kind == Arm64OperandKind.Register
                && ins.GetOperand(1).Kind == Arm64OperandKind.Register && ins.GetOperand(2).Kind == Arm64OperandKind.Register
                && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == targetReg)
            {
                baseReg = RegKey((Arm64RegisterOperand)ins.GetOperand(1));
                offReg = RegKey((Arm64RegisterOperand)ins.GetOperand(2));
                if (ins.OperandCount >= 4)
                {
                    var op3 = ins.GetOperand(3);
                    if (op3.Kind == Arm64OperandKind.Shift) addShift = ((Arm64ShiftOperand)op3).Amount;
                    else if (op3.Kind == Arm64OperandKind.Extend) addShift = ((Arm64ExtendOperand)op3).Amount;
                }
                addIdx = k;
                earliest = Math.Min(earliest, k);
                break;
            }
        }
        if (addIdx < 0 || baseReg < 0 || offReg < 0) return null;

        // 2. Locate base address definition (`adr xBase, label` or `adrp + add`), offset load (`ldr* wOff, [xTable, xIdx]`),
        //    and table address (`adrp + add` or `adr`).
        ulong baseVa = 0;
        long baseAddImm = 0;
        var baseNeedsAdrp = false;
        var tableReg = -1;
        var idxReg = -1;
        var entrySize = 0;
        var isSigned = false;
        ulong tableVa = 0;
        long tableAddImm = 0;
        var tableNeedsAdrp = false;

        for (var k = addIdx - 1; k >= floor; k--)
        {
            var ins = insns[k].Ins;
            var pc = insns[k].Address;
            var m = ins.Mnemonic;
            if (IsArm64Terminator(ins) || m is Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ or Arm64Mnemonic.BL or Arm64Mnemonic.BLR)
                break;

            if (baseVa == 0 && !baseNeedsAdrp && m == Arm64Mnemonic.ADR && ins.OperandCount >= 2
                && ins.GetOperand(0).Kind == Arm64OperandKind.Register && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == baseReg)
            {
                baseVa = BranchTarget(pc, ins, 1);
                earliest = Math.Min(earliest, k);
                continue;
            }
            if (baseVa == 0 && m == Arm64Mnemonic.ADD && ins.OperandCount >= 3
                && ins.GetOperand(0).Kind == Arm64OperandKind.Register && ins.GetOperand(1).Kind == Arm64OperandKind.Register
                && ins.GetOperand(2).Kind == Arm64OperandKind.Immediate
                && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == baseReg)
            {
                baseAddImm = ((Arm64ImmediateOperand)ins.GetOperand(2)).Value;
                if (ins.OperandCount >= 4 && ins.GetOperand(3).Kind == Arm64OperandKind.Shift)
                    baseAddImm <<= ((Arm64ShiftOperand)ins.GetOperand(3)).Amount;
                baseReg = RegKey((Arm64RegisterOperand)ins.GetOperand(1));
                baseNeedsAdrp = true;
                earliest = Math.Min(earliest, k);
                continue;
            }
            if (baseVa == 0 && m == Arm64Mnemonic.ADRP && ins.OperandCount >= 2
                && ins.GetOperand(0).Kind == Arm64OperandKind.Register && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == baseReg
                && ins.GetOperand(1).Kind == Arm64OperandKind.Label)
            {
                var lbl = (Arm64LabelOperand)ins.GetOperand(1);
                var page = (pc & ~0xFFFUL) + unchecked((ulong)lbl.Offset);
                baseVa = unchecked((ulong)((long)page + baseAddImm));
                baseNeedsAdrp = false;
                earliest = Math.Min(earliest, k);
                continue;
            }

            if (entrySize == 0 && m is Arm64Mnemonic.LDRB or Arm64Mnemonic.LDRSB or Arm64Mnemonic.LDRH or Arm64Mnemonic.LDRSH or Arm64Mnemonic.LDRSW or Arm64Mnemonic.LDR
                && ins.OperandCount >= 2 && ins.GetOperand(0).Kind == Arm64OperandKind.Register && ins.GetOperand(1).Kind == Arm64OperandKind.Memory
                && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == offReg)
            {
                var dstOp = (Arm64RegisterOperand)ins.GetOperand(0);
                var acc = ((Arm64MemoryOperand)ins.GetOperand(1)).Accessor;
                if (acc.HasIndexRegister && acc.Immediate == 0)
                {
                    tableReg = RawRegKey(acc.BaseRegister);
                    idxReg = RawRegKey(acc.IndexRegister);
                    (entrySize, isSigned) = m switch
                    {
                        Arm64Mnemonic.LDRB => (1, false),
                        Arm64Mnemonic.LDRSB => (1, true),
                        Arm64Mnemonic.LDRH => (2, false),
                        Arm64Mnemonic.LDRSH => (2, true),
                        Arm64Mnemonic.LDRSW => (4, true),
                        Arm64Mnemonic.LDR when dstOp.Value.Kind == Arm64RegisterKind.W => (4, false),
                        _ => (0, false),
                    };
                    if (entrySize == 0) return null;
                    earliest = Math.Min(earliest, k);
                    continue;
                }
            }

            if (tableReg >= 0 && tableVa == 0)
            {
                if (!tableNeedsAdrp && m == Arm64Mnemonic.ADR && ins.OperandCount >= 2
                    && ins.GetOperand(0).Kind == Arm64OperandKind.Register && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == tableReg)
                {
                    tableVa = BranchTarget(pc, ins, 1);
                    earliest = Math.Min(earliest, k);
                    continue;
                }
                if (!tableNeedsAdrp && m == Arm64Mnemonic.ADD && ins.OperandCount >= 3
                    && ins.GetOperand(0).Kind == Arm64OperandKind.Register && ins.GetOperand(1).Kind == Arm64OperandKind.Register
                    && ins.GetOperand(2).Kind == Arm64OperandKind.Immediate
                    && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == tableReg)
                {
                    tableAddImm = ((Arm64ImmediateOperand)ins.GetOperand(2)).Value;
                    if (ins.OperandCount >= 4 && ins.GetOperand(3).Kind == Arm64OperandKind.Shift)
                        tableAddImm <<= ((Arm64ShiftOperand)ins.GetOperand(3)).Amount;
                    tableReg = RegKey((Arm64RegisterOperand)ins.GetOperand(1));
                    tableNeedsAdrp = true;
                    earliest = Math.Min(earliest, k);
                    continue;
                }
                if (m == Arm64Mnemonic.ADRP && ins.OperandCount >= 2
                    && ins.GetOperand(0).Kind == Arm64OperandKind.Register && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == tableReg
                    && ins.GetOperand(1).Kind == Arm64OperandKind.Label)
                {
                    var lbl = (Arm64LabelOperand)ins.GetOperand(1);
                    var page = (pc & ~0xFFFUL) + unchecked((ulong)lbl.Offset);
                    tableVa = unchecked((ulong)((long)page + tableAddImm));
                    tableNeedsAdrp = false;
                    earliest = Math.Min(earliest, k);
                    continue;
                }
            }
        }

        if (baseVa == 0 || tableVa == 0 || idxReg < 0 || entrySize == 0)
            return null;

        // 3. Locate the preceding `cmp wIdx, #maxVal` + `b.hi`/`b.hs defaultVa` bounds guard.
        ulong defaultVa = 0;
        var isHs = false;
        var guardBranchIdx = -1;
        var guardFloor = Math.Max(0, earliest - 12);
        for (var k = earliest - 1; k >= guardFloor; k--)
        {
            var ins = insns[k].Ins;
            var m = ins.Mnemonic;
            if (m == Arm64Mnemonic.B && ins.OperandCount == 2)
            {
                var cc = ((Arm64EnumOperand)ins.GetOperand(0)).AsConditional;
                if (cc is Arm64ConditionalKind.HI or Arm64ConditionalKind.HS)
                {
                    defaultVa = BranchTarget(insns[k].Address, ins, 1);
                    isHs = cc == Arm64ConditionalKind.HS;
                    guardBranchIdx = k;
                    break;
                }
            }
            if (IsArm64Terminator(ins) || m is Arm64Mnemonic.BL or Arm64Mnemonic.BLR or Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ)
                break;
        }
        if (guardBranchIdx < 0) return null;

        var maxVal = -1L;
        var cmpFloor = Math.Max(0, guardBranchIdx - 8);
        for (var k = guardBranchIdx - 1; k >= cmpFloor; k--)
        {
            var ins = insns[k].Ins;
            var m = ins.Mnemonic;
            if (m == Arm64Mnemonic.CMP && ins.OperandCount >= 2
                && ins.GetOperand(0).Kind == Arm64OperandKind.Register && ins.GetOperand(1).Kind == Arm64OperandKind.Immediate
                && RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == idxReg)
            {
                maxVal = ((Arm64ImmediateOperand)ins.GetOperand(1)).Value;
                break;
            }
            if (m == Arm64Mnemonic.SUBS && ins.OperandCount >= 3
                && ins.GetOperand(0).Kind == Arm64OperandKind.Register && ((Arm64RegisterOperand)ins.GetOperand(0)).IsZeroRegister
                && ins.GetOperand(1).Kind == Arm64OperandKind.Register && ins.GetOperand(2).Kind == Arm64OperandKind.Immediate
                && RegKey((Arm64RegisterOperand)ins.GetOperand(1)) == idxReg)
            {
                maxVal = ((Arm64ImmediateOperand)ins.GetOperand(2)).Value;
                break;
            }
            if (IsArm64Terminator(ins) || m is Arm64Mnemonic.BL or Arm64Mnemonic.BLR or Arm64Mnemonic.CMP or Arm64Mnemonic.SUBS or Arm64Mnemonic.FCMP or Arm64Mnemonic.TST)
                break;
        }
        if (maxVal < 1 || maxVal > 511) return null;

        var count = isHs ? (int)maxVal : (int)maxVal + 1;
        if (count < 2 || count > 512) return null;
        var byteLen = count * entrySize;
        if (!reader.IsMapped(tableVa) || !reader.IsMapped(tableVa + (ulong)byteLen - 1))
            return null;
        var raw = reader.Slice(tableVa, byteLen);
        if (raw.Length < byteLen) return null;

        var targets = new ulong[count];
        for (var c = 0; c < count; c++)
        {
            var off = c * entrySize;
            long entry = entrySize switch
            {
                1 => isSigned ? unchecked((sbyte)raw[off]) : raw[off],
                2 => isSigned ? BinaryPrimitives.ReadInt16LittleEndian(raw.Slice(off, 2)) : BinaryPrimitives.ReadUInt16LittleEndian(raw.Slice(off, 2)),
                _ => isSigned ? BinaryPrimitives.ReadInt32LittleEndian(raw.Slice(off, 4)) : BinaryPrimitives.ReadUInt32LittleEndian(raw.Slice(off, 4)),
            };
            var tVa = unchecked((ulong)((long)baseVa + (entry << addShift)));
            if (!indexAtVa.ContainsKey(tVa)) return null;
            targets[c] = tVa;
        }
        return new Arm64JumpTable(idxReg, targets, defaultVa);
    }

    internal static void ClassifyOperands(in Arm64Instruction ins, List<int> defs, List<int> reads)
    {
        if (ins.OperandCount == 0) return;
        var m = ins.Mnemonic;
        var isStore = m is Arm64Mnemonic.STR or Arm64Mnemonic.STRB or Arm64Mnemonic.STRH or Arm64Mnemonic.STUR or Arm64Mnemonic.STURB or Arm64Mnemonic.STURH or Arm64Mnemonic.STP;
        var isCompare = m is Arm64Mnemonic.CMP or Arm64Mnemonic.CMN or Arm64Mnemonic.TST or Arm64Mnemonic.FCMP or Arm64Mnemonic.FCMPE or Arm64Mnemonic.CCMP or Arm64Mnemonic.FCCMP;
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
                if (bk >= 0 && bk != 99)
                {
                    reads.Add(bk);
                    if (acc.IsPreIncrement || (ins.OperandCount > oi + 1 && ins.GetOperand(oi + 1).Kind == Arm64OperandKind.Immediate))
                        defs.Add(bk);
                }
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
