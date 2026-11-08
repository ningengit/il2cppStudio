using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Symbolic machine state for one basic block: general-purpose and XMM registers map to expression trees,
/// stack slots (canonicalized to entry-RSP-relative offsets) map to scalar expressions. Values that cross a
/// block boundary are spilled to one typed local per register, chosen on first spill.
/// </summary>
public sealed class RegisterTracker(CilMethodBody body, CorLibTypeFactory corlib)
{
    private readonly Dictionary<Register, Expr> _regs = [];
    private readonly Dictionary<long, Expr> _slots = [];
    private readonly Dictionary<long, CilLocalVariable> _slotLocals = [];
    private readonly Dictionary<Register, List<CilLocalVariable>> _spillLocals = [];

    /// <summary>Bytes pushed / allocated below the entry RSP (positive = deeper).</summary>
    public long RspDelta { get; set; }

    /// <summary>Register that holds a copy of the entry RSP (`mov rax, rsp` frame idiom), and its delta when copied.</summary>
    public Register FrameCopy { get; set; } = Register.None;
    public long FrameCopyDelta { get; set; }

    public CilLocalVariable NewLocal(TypeSignature type)
    {
        var l = new CilLocalVariable(type);
        body.LocalVariables.Add(l);
        return l;
    }

    // ------------------------------------------------------------------ registers

    public Expr Get(Register r)
    {
        var n = BlockGraph.Normalize(r);
        if (_regs.TryGetValue(n, out var e)) return Narrow(r, e);
        return Narrow(r, new NativeExpr(n.ToString().ToLowerInvariant()));
    }

    public bool TryGet(Register r, out Expr e) => _regs.TryGetValue(BlockGraph.Normalize(r), out e!);

    public void Set(Register r, Expr value) => _regs[BlockGraph.Normalize(r)] = value;

    public void Clobber(Register r) => _regs.Remove(BlockGraph.Normalize(r));

    /// <summary>Snapshot of the current register → value map.</summary>
    public IReadOnlyList<KeyValuePair<Register, Expr>> Registers => [.. _regs];

    /// <summary>CIL type used to hold <paramref name="e"/> in a local.</summary>
    public TypeSignature TypeFor(Expr e) => SpillType(e);

    /// <summary>Replaces every register / stack slot currently holding <paramref name="old"/> (by reference) with <paramref name="replacement"/>.</summary>
    public void ReplaceAll(Expr old, Expr replacement)
    {
        foreach (var r in _regs.Where(e => ReferenceEquals(e.Value, old)).Select(e => e.Key).ToList()) _regs[r] = replacement;
        foreach (var k in _slots.Where(e => ReferenceEquals(e.Value, old)).Select(e => e.Key).ToList()) _slots[k] = replacement;
    }

    public void ClobberVolatiles(IEnumerable<Register> regs)
    {
        foreach (var r in regs) _regs.Remove(r);
    }

    /// <summary>Applies sub-register semantics: reading a 32-bit register of a 64-bit integer yields an int32 view.</summary>
    private Expr Narrow(Register r, Expr e)
    {
        if (!r.IsGPR() || r.IsGPR64() || e.Kind is ValueKind.I4 or ValueKind.Ref or ValueKind.R4 or ValueKind.R8) return e;
        if (e is ConstExpr c) return new ConstExpr(r.IsGPR32() ? (int)c.Value : r.IsGPR16() ? (short)c.Value : (sbyte)c.Value, ValueKind.I4, c.FromFloatBits);
        return new ConvExpr(e, corlib.Int32);
    }

    // ------------------------------------------------------------------ stack slots

    /// <summary>Canonical slot key for [rsp+disp] at the current frame depth (independent of later pushes).</summary>
    public long SlotKey(long rspDisplacement) => rspDisplacement - RspDelta;

    public Expr? GetSlot(long key) => _slots.GetValueOrDefault(key) ?? (_slotLocals.TryGetValue(key, out var l) ? new LocalExpr(l) : null);

    /// <summary>Forgets the value tracked at a stack slot (it was overwritten by a wider store).</summary>
    public void ClearSlot(long key) { _slots.Remove(key); _slotLocals.Remove(key); }

    public void SetSlot(long key, Expr value)
    {
        _slots[key] = value;
        if (value is LocalExpr le) _slotLocals[key] = le.Local;
    }

    // ------------------------------------------------------------------ block boundaries

    /// <summary>
    /// Resets state at a block start. Each live-in register takes the value its predecessors left: a stable value
    /// (address, constant, argument…) when every already-lifted predecessor agrees on it and none is still pending
    /// (back edge), otherwise the register's spill local. Previously the last block lifted in layout order decided,
    /// which was wrong at joins.
    /// </summary>
    public void EnterBlock(NativeBlock block, bool isEntry)
    {
        if (!isEntry) _regs.Clear();
        _slots.Clear();
        var preds = block.Predecessors;
        var allDone = preds.All(_exit.ContainsKey);
        foreach (var r in block.LiveIn)
        {
            if (isEntry && _regs.ContainsKey(r)) continue;
            var values = preds.Where(_exit.ContainsKey).Select(p => _exit[p].GetValueOrDefault(r)).Where(v => v is not null).ToList();
            if (values.Count > 0 && values[0] is { } first && IsRematerializable(first)
                && (allDone || !IsStorable(first)) && values.All(v => SameValue(v!, first)))
                _regs[r] = first;
            else if (values.OfType<LocalExpr>().Select(l => l.Local).Distinct().ToList() is [var singleLocal])
                _regs[r] = new LocalExpr(singleLocal);
            else if (values.FirstOrDefault(v => IsStorable(v!)) is { } storable)
                _regs[r] = new LocalExpr(SpillLocalFor(r, SpillType(storable)));
            else if (_spillLocals.TryGetValue(r, out var list) && list.Count > 0)
                _regs[r] = new LocalExpr(list[^1]);
            else if (values.FirstOrDefault() is { } any && IsRematerializable(any))
                _regs[r] = any;   // no local exists: best available (logged by the caller when values disagree)
            if (_regs.TryGetValue(r, out var bound) && bound is LocalExpr bl)
                (_entryLocals.TryGetValue(block, out var m) ? m : _entryLocals[block] = [])[r] = bl.Local;
        }
    }

    private static bool SameValue(Expr a, Expr b) => ReferenceEquals(a, b) || (a, b) switch
    {
        (LocalExpr x, LocalExpr y) => x.Local == y.Local,
        (AddressExpr x, AddressExpr y) => SameValue(x.Target, y.Target),
        (StackAddrPlaceholder x, StackAddrPlaceholder y) => x.Key == y.Key,
        (ConstExpr x, ConstExpr y) => x.Value == y.Value && x.Kind == y.Kind,
        (ArgExpr x, ArgExpr y) => x.Parameter == y.Parameter,
        (MarkerExpr x, MarkerExpr y) => x.Marker == y.Marker && x.Index == y.Index && x.Describe() == y.Describe(),
        (StringExpr x, StringExpr y) => x.Describe() == y.Describe(),
        _ => false,
    };

    /// <summary>Side-effect-free, stable values that are re-created in successors instead of being stored to a local.</summary>
    private static bool IsRematerializable(Expr e) =>
        e is AddressExpr or StackAddrPlaceholder or MarkerExpr or ConstExpr or FloatExpr or StringExpr or ArgExpr
            or StructChunkExpr { Source: LocalExpr };   // chunk of an (immutable) struct local

    /// <summary>Values that can also be kept in an ordinary typed local (addresses and runtime markers cannot).</summary>
    public static bool IsStorable(Expr e) => e is not (AddressExpr or StackAddrPlaceholder or MarkerExpr or StructChunkExpr);

    /// <summary>Register values at the end of each lifted block (after spilling).</summary>
    private readonly Dictionary<NativeBlock, Dictionary<Register, Expr>> _exit = [];

    /// <summary>Current register → expression bindings (copy), e.g. to detect which values a spill replaced by locals.</summary>
    public Dictionary<Register, Expr> Snapshot() => new(_regs);

    /// <summary>
    /// Spills live-out registers to typed locals so successor blocks observe the same values (one local per register
    /// and type). Stable values are additionally recorded as-is so agreeing successors can re-create them. Must be
    /// called before the block's terminating branch is emitted (CIL stack is empty at every branch).
    /// </summary>
    public void SpillLiveOut(EmitContext ctx, NativeBlock block)
    {
        var exit = _exit[block] = [];
        foreach (var r in block.LiveOut)
        {
            if (!_regs.TryGetValue(r, out var e)) continue;
            exit[r] = e;
            if (!IsStorable(e)) continue;
            var type = SpillType(e);
            // A successor lifted earlier (loop header via back edge) already reads a specific local for r: store into
            // that one, even if this value has another width (mov ebx, 1 … add rbx, 2), or the loop never sees updates.
            var local = block.Successors.Select(s => _entryLocals.GetValueOrDefault(s)?.GetValueOrDefault(r)).FirstOrDefault(l => l is not null)
                        ?? SpillLocalFor(r, type);
            if (e is LocalExpr le && le.Local == local) continue;
            Expr.EmitAs(ctx, e, local.VariableType);
            ctx.IL.Add(CilOpCodes.Stloc, local);
            if (!IsRematerializable(e)) exit[r] = _regs[r] = new LocalExpr(local);
        }
    }

    /// <summary>Locals each lifted block reads its live-in registers from (decided in <see cref="EnterBlock"/>).</summary>
    private readonly Dictionary<NativeBlock, Dictionary<Register, CilLocalVariable>> _entryLocals = [];

    /// <summary>
    /// Pre-creates or retrieves the spill local of a register for a specific type. 32- and 64-bit integers share one
    /// local per register: x86-64 freely mixes <c>eax</c>/<c>rax</c> writes of the same variable (mov eax, [x] …
    /// movsxd rax, [x]), and separate locals would let a join read a stale width.
    /// </summary>
    public CilLocalVariable SpillLocalFor(Register r, TypeSignature type)
    {
        if (!_spillLocals.TryGetValue(r, out var list)) _spillLocals[r] = list = [];
        if (list.FirstOrDefault(l => SignatureComparer.Default.Equals(l.VariableType, type)) is { } existing) return existing;
        if (ArgumentLocations.Abi == NativeAbi.SysV && IsInteger(type) && list.FirstOrDefault(l => IsInteger(l.VariableType)) is { } sameRegisterInteger)
            return sameRegisterInteger;   // Clang mixes eax/rax widths across joins; MSVC output keeps one local per type (distinct variables stay distinct)
        var created = NewLocal(type);
        list.Add(created);
        return created;
    }

    private static bool IsInteger(TypeSignature t) => t.ElementType is AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I4 or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I8;

    private TypeSignature SpillType(Expr e) => e.Type ?? e.Kind switch
    {
        ValueKind.I4 => corlib.Int32,
        ValueKind.R4 => corlib.Single,
        ValueKind.R8 => corlib.Double,
        ValueKind.Ref => corlib.Object,
        ValueKind.Native => corlib.IntPtr,
        _ => corlib.Int64,
    };
}
