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

/// <summary>
/// Control-flow graph of one native function: leaders, blocks, IL2CPP runtime plumbing detection and
/// backward register liveness (used to decide which registers must be spilled to locals at block ends).
/// </summary>
public sealed class BlockGraph
{
    public IReadOnlyList<Instruction> Instructions { get; }
    public List<NativeBlock> Blocks { get; } = [];
    public HashSet<int> Elided { get; } = [];
    public int ElidedGuards { get; private set; }
    private readonly Dictionary<ulong, int> _indexOfVa = [];
    private readonly Dictionary<ulong, NativeBlock> _blockAt = [];
    private readonly Func<ulong, Runtime.RuntimeHelperKind> _helperKind;
    private readonly Register _returnReg;
    private readonly Func<Instruction, Register[]> _callReads;

    public BlockGraph(IReadOnlyList<Instruction> instructions, Il2CppRegistration reg, Func<ulong, Runtime.RuntimeHelperKind>? helperKind = null,
        Register returnReg = Register.RAX, Func<Instruction, Register[]>? callReads = null)
    {
        Instructions = instructions;
        _helperKind = helperKind ?? (_ => Runtime.RuntimeHelperKind.Unknown);
        _returnReg = returnReg;
        _callReads = callReads ?? (_ => CallArgs);
        for (var i = 0; i < instructions.Count; i++) _indexOfVa[instructions[i].IP] = i;
        FindMetadataInitGuards(reg);
        FindInterfaceOffsetLoops();
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
    private void FindMetadataInitGuards(Il2CppRegistration reg)
    {
        for (var i = 0; i + 1 < Instructions.Count; i++)
        {
            var c = Instructions[i];
            if (c.Mnemonic != Mnemonic.Cmp || !c.IsIPRelativeMemoryOperand || c.MemorySize != MemorySize.UInt8
                || c.Op1Kind != OpKind.Immediate8 || c.Immediate8 != 0)
                continue;
            var flag = c.IPRelativeMemoryAddress;
            for (var j = i + 1; j < Math.Min(Instructions.Count, i + 64); j++)
            {
                var s = Instructions[j];
                if (s.Mnemonic != Mnemonic.Mov || !s.IsIPRelativeMemoryOperand || s.IPRelativeMemoryAddress != flag || s.Op0Kind != OpKind.Memory)
                    continue;
                for (var k = i; k <= j; k++)
                {
                    var x = Instructions[k];
                    var isUsageLea = x.Mnemonic == Mnemonic.Lea && x.IsIPRelativeMemoryOperand && reg.MetadataUsages.ContainsKey(x.IPRelativeMemoryAddress);
                    if (k == i || k == j || isUsageLea || x.FlowControl is FlowControl.ConditionalBranch or FlowControl.Call)
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

    private void BuildBlocks()
    {
        // Branches to trap blocks (call ThrowHelper; int3) are NullCheck / bounds checks: they do not split blocks,
        // so symbolic state flows straight through the guarded code.
        bool IsTrapTarget(ulong va) => _indexOfVa.TryGetValue(va, out var t) && t + 1 < Instructions.Count
            && (Instructions[t].FlowControl == FlowControl.Call && Instructions[t + 1].Mnemonic == Mnemonic.Int3 || IsRuntimeExceptionCall(Instructions[t]));

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

    /// <summary>Trap = the block's only real work is a call followed by int3 (never returns).</summary>
    private void MarkTraps()
    {
        foreach (var b in Blocks)
        {
            var live = Enumerable.Range(b.Start, b.End - b.Start).Where(i => !Elided.Contains(i)).ToList();
            if (live.Count is 1 or 2 && Instructions[live[0]].FlowControl == FlowControl.Call
                && (live.Count == 1 ? b.End == Instructions.Count || Instructions[b.End].Mnemonic == Mnemonic.Int3 : Instructions[live[1]].Mnemonic == Mnemonic.Int3))
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
        if (ins.FlowControl == FlowControl.Return && _returnReg != Register.None) reads.Add(_returnReg);
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
                var zeroIdiom = ins.Mnemonic is Mnemonic.Xor or Mnemonic.Xorps or Mnemonic.Pxor or Mnemonic.Xorpd
                    && ins.OpCount == 2 && ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Register && ins.Op0Register == ins.Op1Register;
                foreach (var u in info.GetUsedRegisters())
                {
                    var r = Normalize(u.Register);
                    if (!IsTracked(r)) continue;
                    var reads = u.Access is OpAccess.Read or OpAccess.ReadWrite or OpAccess.CondRead or OpAccess.ReadCondWrite;
                    if (reads && !zeroIdiom && !b.Def.Contains(r)) b.Use.Add(r);
                }
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
                if (ins.FlowControl == FlowControl.Return && _returnReg != Register.None && !b.Def.Contains(_returnReg))
                    b.Use.Add(_returnReg);
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
