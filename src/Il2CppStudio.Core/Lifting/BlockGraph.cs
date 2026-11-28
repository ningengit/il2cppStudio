using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>A basic block of native instructions.</summary>
public sealed class NativeBlock
{
    public int Start { get; init; }
    public int End { get; set; }
    public ulong Va { get; init; }
    public CilInstructionLabel Label { get; } = new();
    public List<NativeBlock> Successors { get; } = [];
    public List<NativeBlock> Predecessors { get; } = [];
    public HashSet<Register> Use { get; } = [];
    public HashSet<Register> Def { get; } = [];
    public HashSet<Register> LiveIn { get; } = [];
    public HashSet<Register> LiveOut { get; } = [];

    /// <summary>Block is `call ThrowHelper; int3` (NullCheck / bounds check failure path) — implicit in CIL.</summary>
    public bool IsTrap { get; set; }

    /// <summary>Block consists only of int3 alignment padding.</summary>
    public bool IsPadding { get; set; }
}

public sealed record X64JumpTable(Register IndexRegister, ulong[] Targets, ulong DefaultTargetVa);

/// <summary>
/// Control-flow graph of one native function: leaders, blocks, IL2CPP runtime plumbing detection and
/// backward register liveness (used to decide which registers must be spilled to locals at block ends).
/// </summary>
public sealed class BlockGraph
{
    public IReadOnlyList<Instruction> Instructions { get; }
    public List<NativeBlock> Blocks { get; } = [];
    public HashSet<int> Elided { get; } = [];
    public Dictionary<int, X64JumpTable> JumpTables { get; } = [];
    public int ElidedGuards { get; private set; }
    private readonly Dictionary<ulong, int> _indexOfVa = [];
    private readonly Dictionary<ulong, NativeBlock> _blockAt = [];
    private readonly Func<ulong, Runtime.RuntimeHelperKind> _helperKind;
    private readonly Register[] _returnRegs;
    private readonly Func<Instruction, Register[]> _callReads;

    public BlockGraph(IReadOnlyList<Instruction> instructions, Il2CppRegistration reg, Func<ulong, Runtime.RuntimeHelperKind>? helperKind = null,
        Register returnReg = Register.RAX, Func<Instruction, Register[]>? callReads = null, Register[]? returnRegs = null)
    {
        Instructions = instructions;
        _helperKind = helperKind ?? (_ => Runtime.RuntimeHelperKind.Unknown);
        _returnRegs = returnRegs ?? (returnReg == Register.None ? [] : [returnReg]);
        _callReads = callReads ?? (_ => CallArgs);
        for (var i = 0; i < instructions.Count; i++) _indexOfVa[instructions[i].IP] = i;
        FindMetadataInitGuards(reg);
        FindInterfaceOffsetLoops();
        FindJumpTables(reg);
        BuildBlocks();
        MarkTraps();
        foreach (var b in Blocks)
            foreach (var s in b.Successors.Distinct())
                s.Predecessors.Add(b);
        ComputeLiveness();
    }

    private Runtime.RuntimeHelperKind CallKind(in Instruction ins) =>
        ins.FlowControl == FlowControl.Call && ins.Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32 ? _helperKind(ins.NearBranchTarget) : Runtime.RuntimeHelperKind.Unknown;

    private bool IsNoReturnCall(in Instruction ins) => CallKind(ins) is Runtime.RuntimeHelperKind.Raise or Runtime.RuntimeHelperKind.ThrowHelper;

    /// <summary>Calls that only build/raise a runtime exception (no user-visible work).</summary>
    private bool IsRuntimeExceptionCall(in Instruction ins) => CallKind(ins) is Runtime.RuntimeHelperKind.Raise or Runtime.RuntimeHelperKind.ThrowHelper
        or Runtime.RuntimeHelperKind.ExceptionFactory or Runtime.RuntimeHelperKind.MetadataInit;

    public NativeBlock? BlockAt(ulong va) => _blockAt.GetValueOrDefault(va);

    public bool TryIndexOf(ulong va, out int index) => _indexOfVa.TryGetValue(va, out index);

    /// <summary>
    /// <c>if (!s_Il2CppMethodInitialized) { il2cpp_codegen_initialize_runtime_metadata(&amp;X_var); s_Il2CppMethodInitialized = true; }</c>
    /// compiles to <c>cmp byte [rip+flag],0 / jcc / lea rcx,[usage] / call init / mov byte [rip+flag],1</c>.
    /// The compare, branch, init calls and flag store are runtime plumbing with no CIL counterpart.
    /// </summary>
    private static ulong DirectGlobalAddr(in Instruction ins)
    {
        if (ins.IsIPRelativeMemoryOperand) return ins.IPRelativeMemoryAddress;
        if (ArgumentLocations.PointerSize == 4 && ins.MemoryBase == Register.None && ins.MemoryIndex == Register.None && ins.MemoryDisplacement32 != 0)
            return ins.MemoryDisplacement32;
        return 0;
    }

    private void FindMetadataInitGuards(Il2CppRegistration reg)
    {
        for (var i = 0; i + 1 < Instructions.Count; i++)
        {
            var c = Instructions[i];
            if (c.Mnemonic != Mnemonic.Cmp || c.MemorySize != MemorySize.UInt8
                || c.Op1Kind != OpKind.Immediate8 || c.Immediate8 != 0)
                continue;
            var flag = DirectGlobalAddr(c);
            if (flag == 0) continue;
            for (var j = i + 1; j < Math.Min(Instructions.Count, i + 64); j++)
            {
                var s = Instructions[j];
                if (s.Mnemonic != Mnemonic.Mov || DirectGlobalAddr(s) != flag || s.Op0Kind != OpKind.Memory)
                    continue;
                for (var k = i; k <= j; k++)
                {
                    var x = Instructions[k];
                    var ga = DirectGlobalAddr(x);
                    var isUsageLea = ga != 0 && reg.MetadataUsages.ContainsKey(ga)
                        && (x.Mnemonic == Mnemonic.Lea || ArgumentLocations.PointerSize == 4 && x.Mnemonic is Mnemonic.Push or Mnemonic.Mov);
                    if (k == i || k == j || isUsageLea || x.FlowControl is FlowControl.ConditionalBranch or FlowControl.Call
                        || ArgumentLocations.PointerSize == 4 && x.Mnemonic == Mnemonic.Add && x.Op0Kind == OpKind.Register && x.Op0Register == Register.ESP)
                        Elided.Add(k);
                }
                ElidedGuards++;
                break;
            }
        }
    }

    /// <summary>
    /// Elides inlined <c>InterfaceFuncInvoker</c> interface-offset table loops (<c>movzx reg, word ptr [klass+12Eh] … jb</c>)
    /// that precede <c>GetInterfaceInvokeDataFromVTableSlowPath</c>, plus their out-of-line fast-path index stubs.
    /// </summary>
    private void FindInterfaceOffsetLoops()
    {
        for (var i = 0; i < Instructions.Count; i++)
        {
            var ins = Instructions[i];
            if (ins.Mnemonic != Mnemonic.Movzx || ins.Op1Kind != OpKind.Memory || (long)ins.MemoryDisplacement64 != 0x12E) continue;
            for (var j = i + 1; j < Math.Min(Instructions.Count, i + 20); j++)
            {
                if (Instructions[j].Mnemonic != Mnemonic.Jb) continue;
                for (var k = i; k <= j; k++)
                {
                    if (Instructions[k].Mnemonic == Mnemonic.Je && _indexOfVa.TryGetValue(Instructions[k].NearBranchTarget, out var fast))
                        for (var f = fast; f < Math.Min(Instructions.Count, fast + 10); f++)
                        {
                            Elided.Add(f);
                            if (Instructions[f].FlowControl == FlowControl.UnconditionalBranch) break;
                        }
                    Elided.Add(k);
                }
                ElidedGuards++;
                break;
            }
        }
    }

    private void FindJumpTables(Il2CppRegistration reg)
    {
        for (var i = 0; i < Instructions.Count; i++)
        {
            if (Elided.Contains(i)) continue;
            var ins = Instructions[i];
            if (ins.FlowControl != FlowControl.IndirectBranch || ins.Op0Kind != OpKind.Register) continue;
            var targetReg = Normalize(ins.Op0Register);
            if (!targetReg.IsGPR64()) continue;

            var addIdx = -1;
            var baseReg = Register.None;
            for (var j = i - 1; j >= 0 && i - j <= 4; j--)
            {
                if (Elided.Contains(j)) continue;
                var x = Instructions[j];
                if (x.FlowControl != FlowControl.Next) break;
                if (x.Mnemonic == Mnemonic.Add && x.OpCount == 2 && x.Op0Kind == OpKind.Register && x.Op1Kind == OpKind.Register
                    && Normalize(x.Op0Register) == targetReg)
                {
                    addIdx = j;
                    baseReg = Normalize(x.Op1Register);
                    break;
                }
            }
            if (addIdx < 0 || !baseReg.IsGPR64()) continue;

            var loadIdx = -1;
            var indexReg = Register.None;
            var tableDisp = 0L;
            var signExtendEntries = false;
            for (var j = addIdx - 1; j >= 0 && addIdx - j <= 4; j--)
            {
                if (Elided.Contains(j)) continue;
                var x = Instructions[j];
                if (x.FlowControl != FlowControl.Next) break;
                if (x.Mnemonic is Mnemonic.Mov or Mnemonic.Movsxd && x.Op0Kind == OpKind.Register
                    && Normalize(x.Op0Register) == targetReg && x.Op1Kind == OpKind.Memory
                    && x.MemoryIndexScale == 4 && x.MemoryIndex != Register.None
                    && Normalize(x.MemoryBase) == baseReg)
                {
                    loadIdx = j;
                    indexReg = Normalize(x.MemoryIndex);
                    tableDisp = unchecked((long)x.MemoryDisplacement64);
                    signExtendEntries = x.Mnemonic == Mnemonic.Movsxd;
                    break;
                }
            }
            if (loadIdx < 0 || !indexReg.IsGPR64()) continue;

            var leaIdx = -1;
            var baseVa = 0UL;
            for (var j = loadIdx - 1; j >= 0 && loadIdx - j <= 6; j--)
            {
                if (Elided.Contains(j)) continue;
                var x = Instructions[j];
                if (x.FlowControl != FlowControl.Next) break;
                if (x.Mnemonic == Mnemonic.Lea && x.Op0Kind == OpKind.Register
                    && Normalize(x.Op0Register) == baseReg && x.IsIPRelativeMemoryOperand)
                {
                    leaIdx = j;
                    baseVa = x.IPRelativeMemoryAddress;
                    break;
                }
            }
            if (leaIdx < 0 || baseVa == 0) continue;

            var defaultVa = 0UL;
            var count = 0;
            for (var j = leaIdx - 1; j >= 0 && leaIdx - j <= 12; j--)
            {
                if (Elided.Contains(j)) continue;
                var x = Instructions[j];
                if (x.FlowControl == FlowControl.Call || x.FlowControl == FlowControl.Return) break;
                if (x.Mnemonic is Mnemonic.Ja or Mnemonic.Jae && j > 0 && Instructions[j - 1].Mnemonic == Mnemonic.Cmp
                    && Instructions[j - 1].Op1Kind is OpKind.Immediate8 or OpKind.Immediate8to32 or OpKind.Immediate32 or OpKind.Immediate8to64)
                {
                    var bound = unchecked((long)Instructions[j - 1].GetImmediate(1));
                    var c = x.Mnemonic == Mnemonic.Ja ? bound + 1 : bound;
                    if (c is >= 2 and <= 512)
                    {
                        count = (int)c;
                        defaultVa = x.NearBranchTarget;
                    }
                    break;
                }
            }
            if (count == 0) continue;

            var tableVa = unchecked((ulong)((long)baseVa + tableDisp));
            var targets = new ulong[count];
            var ok = true;
            for (var k = 0; k < count; k++)
            {
                var entryAddr = tableVa + (ulong)(k * 4);
                if (!reg.Reader.IsMapped(entryAddr)) { ok = false; break; }
                var targetVa = signExtendEntries
                    ? unchecked((ulong)((long)baseVa + reg.Reader.ReadI32(entryAddr)))
                    : baseVa + reg.Reader.ReadU32(entryAddr);
                if (!_indexOfVa.ContainsKey(targetVa)) { ok = false; break; }
                targets[k] = targetVa;
            }
            if (!ok) continue;

            JumpTables[i] = new X64JumpTable(indexReg, targets, defaultVa);
            Elided.Add(loadIdx);
            Elided.Add(addIdx);
            var baseUsedBetween = false;
            for (var k = leaIdx + 1; k < loadIdx; k++)
            {
                var mid = Instructions[k];
                if (mid.Op0Kind == OpKind.Register && Normalize(mid.Op0Register) == baseReg
                    || mid.Op1Kind == OpKind.Register && Normalize(mid.Op1Register) == baseReg
                    || mid.Op0Kind == OpKind.Memory && (Normalize(mid.MemoryBase) == baseReg || Normalize(mid.MemoryIndex) == baseReg)
                    || mid.Op1Kind == OpKind.Memory && (Normalize(mid.MemoryBase) == baseReg || Normalize(mid.MemoryIndex) == baseReg))
                {
                    baseUsedBetween = true;
                    break;
                }
            }
            if (!baseUsedBetween) Elided.Add(leaIdx);
        }
    }

    private void BuildBlocks()
    {
        // Branches to trap blocks (call ThrowHelper; int3, optionally with Win32 `push`) are NullCheck / bounds
        // checks: they do not split blocks, so symbolic state flows straight through the guarded code.
        bool IsTrapTarget(ulong va)
        {
            if (!_indexOfVa.TryGetValue(va, out var t)) return false;
            for (var skip = 0; skip <= 2 && t + 1 < Instructions.Count; skip++, t++)
            {
                if (Instructions[t].FlowControl == FlowControl.Call
                    && (Instructions[t + 1].Mnemonic == Mnemonic.Int3 || IsRuntimeExceptionCall(Instructions[t])))
                    return true;
                if (Instructions[t].Mnemonic is not (Mnemonic.Push or Mnemonic.Mov or Mnemonic.Xor)) break;
            }
            return false;
        }

        var leaders = new SortedSet<int> { 0 };
        for (var i = 0; i < Instructions.Count; i++)
        {
            if (Elided.Contains(i)) continue;
            var ins = Instructions[i];
            if (ins.FlowControl == FlowControl.ConditionalBranch && IsTrapTarget(ins.NearBranchTarget)) continue;
            if (IsNoReturnCall(ins) && i + 1 < Instructions.Count) { leaders.Add(i + 1); continue; }
            if (ins.FlowControl is FlowControl.ConditionalBranch or FlowControl.UnconditionalBranch)
            {
                if (_indexOfVa.TryGetValue(ins.NearBranchTarget, out var t)) leaders.Add(t);
                if (i + 1 < Instructions.Count) leaders.Add(i + 1);
            }
            else if (ins.FlowControl is FlowControl.Return or FlowControl.IndirectBranch && i + 1 < Instructions.Count)
                leaders.Add(i + 1);
            else if (ins.FlowControl == FlowControl.Interrupt && i + 1 < Instructions.Count && Instructions[i + 1].Mnemonic != Mnemonic.Int3)
                leaders.Add(i + 1);
        }

        var list = leaders.ToList();
        foreach (var jt in JumpTables.Values)
        {
            foreach (var tva in jt.Targets)
                if (_indexOfVa.TryGetValue(tva, out var idx)) leaders.Add(idx);
            if (jt.DefaultTargetVa != 0 && _indexOfVa.TryGetValue(jt.DefaultTargetVa, out var defIdx))
                leaders.Add(defIdx);
        }
        list = leaders.ToList();
        for (var n = 0; n < list.Count; n++)
        {
            var start = list[n];
            var end = n + 1 < list.Count ? list[n + 1] : Instructions.Count;
            var b = new NativeBlock { Start = start, End = end, Va = Instructions[start].IP };
            Blocks.Add(b);
            _blockAt[b.Va] = b;
        }

        foreach (var b in Blocks)
        {
            var last = LastLive(b);
            if (last < 0) { AddFallthrough(b); continue; }
            var ins = Instructions[last];
            if (IsNoReturnCall(ins)) continue;   // never falls through
            switch (ins.FlowControl)
            {
                case FlowControl.ConditionalBranch:
                    if (_blockAt.TryGetValue(ins.NearBranchTarget, out var t)) b.Successors.Add(t);
                    AddFallthrough(b);
                    break;
                case FlowControl.UnconditionalBranch:
                    if (_blockAt.TryGetValue(ins.NearBranchTarget, out var u)) b.Successors.Add(u);
                    break;
                case FlowControl.IndirectBranch when JumpTables.TryGetValue(last, out var jt):
                    foreach (var tva in jt.Targets.Distinct())
                        if (_blockAt.TryGetValue(tva, out var tb)) b.Successors.Add(tb);
                    if (jt.DefaultTargetVa != 0 && _blockAt.TryGetValue(jt.DefaultTargetVa, out var db) && !b.Successors.Contains(db))
                        b.Successors.Add(db);
                    break;
                case FlowControl.Return or FlowControl.Interrupt or FlowControl.IndirectBranch:
                    break;
                default:
                    AddFallthrough(b);
                    break;
            }
        }
    }

    private void AddFallthrough(NativeBlock b)
    {
        var idx = Blocks.IndexOf(b);
        if (idx + 1 < Blocks.Count) b.Successors.Add(Blocks[idx + 1]);
    }

    /// <summary>Index of the last non-elided instruction of a block, or -1.</summary>
    public int LastLive(NativeBlock b)
    {
        for (var i = b.End - 1; i >= b.Start; i--) if (!Elided.Contains(i)) return i;
        return -1;
    }

    /// <summary>Trap = the block's only real work is a call (with optional arg push) followed by int3 padding.</summary>
    private void MarkTraps()
    {
        foreach (var b in Blocks)
        {
            var live = Enumerable.Range(b.Start, b.End - b.Start).Where(i => !Elided.Contains(i)).ToList();
            if (IsPushCallInt3Trap(live, b.End))
            {
                b.IsTrap = true;
                b.Successors.Clear();
            }
            else if (live.Count >= 1 && live.All(i => Instructions[i].Mnemonic == Mnemonic.Int3))
            {
                b.IsTrap = true;
                b.IsPadding = true;
                b.Successors.Clear();
            }
            else if (live.Any(i => IsNoReturnCall(Instructions[i]))
                     && live.Where(i => Instructions[i].FlowControl == FlowControl.Call).All(i => IsRuntimeExceptionCall(Instructions[i])))
            {
                // Runtime-raised exception (failed cast, bounds, null): only exception factories/raise calls — implicit in CIL.
                b.IsTrap = true;
                b.Successors.Clear();
            }
        }
    }

    private bool IsPushCallInt3Trap(List<int> live, int blockEnd)
    {
        if (live.Count == 0) return false;
        var pos = 0;
        while (pos < live.Count && pos < 2 && Instructions[live[pos]].Mnemonic is Mnemonic.Push or Mnemonic.Mov or Mnemonic.Xor) pos++;
        if (pos >= live.Count || Instructions[live[pos]].FlowControl != FlowControl.Call) return false;
        if (pos + 1 == live.Count)
            return blockEnd == Instructions.Count || Instructions[blockEnd].Mnemonic == Mnemonic.Int3;
        return live.Skip(pos + 1).All(i => Instructions[i].Mnemonic == Mnemonic.Int3);
    }

    // ------------------------------------------------------------------ liveness

    private static Register[] CallArgs => ArgumentLocations.AllArgRegisters(ArgumentLocations.Abi);
    private static Register[] CallClobbers => CallEmitter.Volatile;
    private Register[][]? _reads, _writes;

    private void RecordInstructionRegisters(int i, in Instruction ins, InstructionInfo info, bool zeroIdiom)
    {
        _reads ??= new Register[Instructions.Count][];
        _writes ??= new Register[Instructions.Count][];
        var reads = new List<Register>();
        var writes = new List<Register>();
        foreach (var u in info.GetUsedRegisters())
        {
            var r = Normalize(u.Register);
            if (!IsTracked(r)) continue;
            if (!zeroIdiom && u.Access is OpAccess.Read or OpAccess.ReadWrite or OpAccess.CondRead or OpAccess.ReadCondWrite) reads.Add(r);
            if (u.Access is OpAccess.Write or OpAccess.ReadWrite) writes.Add(r);
        }
        if (ins.FlowControl == FlowControl.Call) { reads.AddRange(_callReads(ins)); writes.AddRange(CallClobbers); }
        if (ins.FlowControl == FlowControl.Return) reads.AddRange(_returnRegs);
        if (ins.FlowControl == FlowControl.IndirectBranch && JumpTables.TryGetValue(i, out var jt))
        {
            var jmpTargetReg = Normalize(ins.Op0Register);
            reads.RemoveAll(r => r == jmpTargetReg);
            reads.Add(jt.IndexRegister);
        }
        _reads[i] = [.. reads];
        _writes[i] = [.. writes];
    }

    /// <summary>
    /// Whether register <paramref name="r"/> still holds a needed value after instruction <paramref name="index"/>
    /// (read before being overwritten later in the block, or live out of it). The instruction's own writes count.
    /// </summary>
    public bool IsLiveAfter(NativeBlock b, int index, Register r)
    {
        if (_writes?[index] is { } w && w.Contains(r)) return false;
        for (var k = index + 1; k < b.End; k++)
        {
            if (Elided.Contains(k) || _reads?[k] is not { } rd) continue;
            if (rd.Contains(r)) return true;
            if (_writes![k].Contains(r)) return false;
        }
        return b.LiveOut.Contains(r);
    }

    public static Register Normalize(Register r)
    {
        if (r == Register.None) return r;
        if (r.IsXMM() || r.IsYMM() || r.IsZMM()) return Register.XMM0 + (r.GetNumber() & 15);
        if (r.IsGPR()) return r.GetFullRegister();
        return Register.None;
    }

    public static bool IsTracked(Register r) =>
        r != Register.None && r != Register.RSP && r != Register.RIP && (r.IsGPR64() || r.IsXMM());

    private void ComputeLiveness()
    {
        var factory = new InstructionInfoFactory();
        foreach (var b in Blocks)
        {
            for (var i = b.Start; i < b.End; i++)
            {
                if (Elided.Contains(i)) continue;
                var ins = Instructions[i];
                var info = factory.GetInfo(ins);
                var zeroIdiom = ins.Mnemonic is Mnemonic.Xor or Mnemonic.Xorps or Mnemonic.Pxor or Mnemonic.Xorpd or Mnemonic.Sbb
                    && ins.OpCount == 2 && ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Register && ins.Op0Register == ins.Op1Register;
                X64JumpTable? jt = null;
                var isJumpTable = ins.FlowControl == FlowControl.IndirectBranch && JumpTables.TryGetValue(i, out jt);
                foreach (var u in info.GetUsedRegisters())
                {
                    var r = isJumpTable ? Register.None : Normalize(u.Register);
                    if (!IsTracked(r)) continue;
                    var reads = u.Access is OpAccess.Read or OpAccess.ReadWrite or OpAccess.CondRead or OpAccess.ReadCondWrite;
                    if (reads && !zeroIdiom && !b.Def.Contains(r)) b.Use.Add(r);
                }
                if (isJumpTable && !b.Def.Contains(jt!.IndexRegister))
                    b.Use.Add(jt.IndexRegister);
                foreach (var u in info.GetUsedRegisters())
                {
                    var r = Normalize(u.Register);
                    if (IsTracked(r) && u.Access is OpAccess.Write or OpAccess.ReadWrite) b.Def.Add(r);
                }
                if (ins.FlowControl == FlowControl.Call)
                {
                    foreach (var r in _callReads(ins))
                        if (!b.Def.Contains(r)) b.Use.Add(r);
                    foreach (var r in CallClobbers)
                        b.Def.Add(r);
                }
                if (ins.FlowControl == FlowControl.Return)
                    foreach (var rr in _returnRegs)
                        if (!b.Def.Contains(rr)) b.Use.Add(rr);
                RecordInstructionRegisters(i, ins, info, zeroIdiom);
            }
        }

        bool changed;
        do
        {
            changed = false;
            for (var n = Blocks.Count - 1; n >= 0; n--)
            {
                var b = Blocks[n];
                foreach (var s in b.Successors)
                    foreach (var r in s.LiveIn)
                        changed |= b.LiveOut.Add(r);
                foreach (var r in b.Use) changed |= b.LiveIn.Add(r);
                foreach (var r in b.LiveOut)
                    if (!b.Def.Contains(r)) changed |= b.LiveIn.Add(r);
            }
        } while (changed);
    }
}
