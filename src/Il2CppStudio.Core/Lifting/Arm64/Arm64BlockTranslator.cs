using System.Buffers.Binary;
using System.Collections.Concurrent;
using AsmArm64;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Arm64;

internal enum Arm64HelperKind { Unknown, NoReturn, WriteBarrier, ClassInit, ObjectNew, ArrayCheck }

internal sealed class Arm64BlockTranslator
{
    private readonly NativeSymbolizer _sym;
    private readonly StubAssemblyBuilder _stubs;
    private readonly ModuleDefinition _module;
    private readonly MethodDefinition _method;
    private readonly MethodShape _shape;
    private readonly MemoryResolver _mem;
    private readonly CilMethodBody _body;
    private readonly CorLibTypeFactory _f;
    private readonly Runtime.RuntimeHelperMap? _helpers;
    private readonly Func<ulong, DiagnosticLocation> _loc;
    private readonly IDiagnosticSink _sink;
    private readonly List<string> _notes;
    private readonly ulong _funcStart, _funcEnd;
    private readonly ConcurrentDictionary<ulong, Arm64HelperKind> _helperCache;
    private readonly IReadOnlyDictionary<ulong, string>? _symbolNames;
    private readonly Dictionary<int, Expr> _regs = [];
    private readonly Dictionary<long, Expr> _slots = [];
    private readonly Dictionary<long, (CilLocalVariable Local, TypeSignature Type, int Size)> _stackStructs = [];
    private readonly Dictionary<(int Reg, string TypeName), CilLocalVariable> _spillLocals = [];
    private readonly Dictionary<Arm64Block, Dictionary<int, Expr>> _exit = [];
    private readonly Dictionary<Arm64Block, Dictionary<int, CilLocalVariable>> _entryLocals = [];
    private readonly Dictionary<(Arm64Block Block, int Reg), CilLocalVariable> _joinLocals = [];
    private readonly Dictionary<int, Expr> _lastRemat = [];
    private readonly HashSet<Expr> _gotValues = new(ReferenceEqualityComparer.Instance);
    private CilLocalVariable? _retBuf;
    private long _spDelta;

    private enum FlagKind { None, Compare, FloatCompare, Test }
    private FlagKind _flagKind;
    private Expr? _flagLeft, _flagRight;
    private ulong[]? _curBlockLiveAfter;
    private int _curBlockStart, _curInsnIdx;
    private (Expr StoredVal, Expr FieldExpr, Expr? InverseExpr)? _pendingFieldRewrite;

    public int Unsupported { get; private set; }
    public int Unresolved { get; private set; }
    public int Elided { get; private set; }

    public Arm64BlockTranslator(
        NativeSymbolizer sym, StubAssemblyBuilder stubs, ModuleDefinition module, MethodDefinition method,
        MethodShape shape, MemoryResolver mem, CilMethodBody body, Runtime.RuntimeHelperMap? helpers,
        Func<ulong, DiagnosticLocation> loc, IDiagnosticSink sink, List<string> notes, ulong funcStart, ulong funcEnd,
        ConcurrentDictionary<ulong, Arm64HelperKind>? helperCache = null,
        IReadOnlyDictionary<ulong, string>? symbolNames = null)
    {
        _sym = sym; _stubs = stubs; _module = module; _method = method; _shape = shape;
        _mem = mem; _body = body; _f = module.CorLibTypeFactory; _helpers = helpers;
        _loc = loc; _sink = sink; _notes = notes; _funcStart = funcStart; _funcEnd = funcEnd;
        _helperCache = helperCache ?? new();
        _symbolNames = symbolNames;
        _mem.BeforeFieldStored = (ctx, val, fieldExpr) =>
        {
            _pendingFieldRewrite = null;
            var isCompound = val is (BinaryExpr or UnaryExpr or MathCallExpr) && val.Kind == fieldExpr.Kind;
            Expr? exclude = isCompound ? val : null;

            var hasAliasedRead = (_flagLeft is not null && ContainsReadOf(_flagLeft, fieldExpr, exclude))
                || (_flagRight is not null && ContainsReadOf(_flagRight, fieldExpr, exclude))
                || _regs.Any(kv => IsRegLiveAfterCurInsn(kv.Key) && ContainsReadOf(kv.Value, fieldExpr, exclude))
                || _slots.Any(kv => ContainsReadOf(kv.Value, fieldExpr, exclude));
            if (!hasAliasedRead)
            {
                if (isCompound)
                    _pendingFieldRewrite = (val, fieldExpr, null);
                return;
            }

            // Integer `field +/- const`: rewrite remaining reads of `field` to `field -/+ const` after the store (avoids temp locals).
            if (isCompound
                && val is BinaryExpr bin
                && (bin.Op.Code == CilCode.Add || bin.Op.Code == CilCode.Sub)
                && SameFieldRef(bin.Left, fieldExpr)
                && bin.Right is ConstExpr { Value: var delta }
                && fieldExpr.Kind is ValueKind.I4 or ValueKind.I8)
            {
                var invOp = bin.Op.Code == CilCode.Add ? CilOpCodes.Sub : CilOpCodes.Add;
                var inv = new BinaryExpr(invOp, fieldExpr, new ConstExpr(delta, fieldExpr.Kind), fieldExpr.Kind);
                _pendingFieldRewrite = (val, fieldExpr, inv);
                return;
            }

            // General case: snapshot old field value into a local before overwriting the field.
            var snap = NewLocal(TypeFor(fieldExpr));
            fieldExpr.Emit(ctx);
            ctx.IL.Add(CilOpCodes.Stloc, snap);
            var snapExpr = new LocalExpr(snap);
            foreach (var k in _regs.Keys.ToList())
            {
                if (IsRegLiveAfterCurInsn(k))
                    _regs[k] = RewriteReadOf(_regs[k], fieldExpr, exclude, snapExpr);
            }
            foreach (var k in _slots.Keys.ToList())
                _slots[k] = RewriteReadOf(_slots[k], fieldExpr, exclude, snapExpr);
            if (_flagLeft is not null) _flagLeft = RewriteReadOf(_flagLeft, fieldExpr, exclude, snapExpr);
            if (_flagRight is not null) _flagRight = RewriteReadOf(_flagRight, fieldExpr, exclude, snapExpr);
            if (isCompound)
                _pendingFieldRewrite = (val, fieldExpr, null);
        };
        _mem.OnFieldStored = (val, fieldExpr) =>
        {
            var pending = _pendingFieldRewrite;
            _pendingFieldRewrite = null;
            var isCompound = val is (BinaryExpr or UnaryExpr or MathCallExpr) && val.Kind == fieldExpr.Kind;
            var invExpr = pending is { } p && ReferenceEquals(p.FieldExpr, fieldExpr) ? p.InverseExpr : null;
            if (!isCompound && invExpr is null) return;

            Expr RewritePost(Expr root) => RewritePostFieldStore(root, isCompound ? val : null, fieldExpr, invExpr);
            foreach (var k in _regs.Keys.ToList())
                _regs[k] = RewritePost(_regs[k]);
            foreach (var k in _slots.Keys.ToList())
                _slots[k] = RewritePost(_slots[k]);
            if (_flagLeft is not null) _flagLeft = RewritePost(_flagLeft);
            if (_flagRight is not null) _flagRight = RewritePost(_flagRight);
        };
    }

    private CilLocalVariable NewLocal(TypeSignature type)
    {
        var l = new CilLocalVariable(type);
        _body.LocalVariables.Add(l);
        return l;
    }

    public void EnterBlock(Arm64Block block, bool isEntry)
    {
        _regs.Clear();
        _flagKind = FlagKind.None;
        _flagLeft = _flagRight = null;
        if (isEntry)
        {
            InitializeEntry();
            return;
        }
        var preds = block.Predecessors;
        var allDone = preds.All(_exit.ContainsKey);
        foreach (var r in block.LiveIn)
        {
            var values = preds.Where(_exit.ContainsKey).Select(p => _exit[p].GetValueOrDefault(r)).Where(v => v is not null).ToList();
            if (values.Count > 0 && values[0] is { } first && IsRematerializable(first)
                && (allDone || !RegisterTracker.IsStorable(first)) && values.All(v => SameValue(v!, first)))
                _regs[r] = first;
            else if (values.All(v => v is LocalExpr) && values.Cast<LocalExpr>().Select(l => l.Local).Distinct().ToList() is [var singleLocal])
                _regs[r] = new LocalExpr(singleLocal);
            else if (_joinLocals.TryGetValue((block, r), out var joinLocal))
                _regs[r] = new LocalExpr(joinLocal);
            else if (values.FirstOrDefault(v => RegisterTracker.IsStorable(v!)) is { } storable)
                _regs[r] = new LocalExpr(SpillLocalFor(r, TypeFor(storable)));
            else if (_spillLocals.Where(kv => kv.Key.Reg == r).Select(kv => kv.Value).LastOrDefault() is { } existing)
                _regs[r] = new LocalExpr(existing);
            else if (values.FirstOrDefault() is { } any && IsRematerializable(any))
                _regs[r] = any;
            else if (r is >= 19 and <= 28 && _lastRemat.TryGetValue(r, out var remat))
                _regs[r] = remat;
            if (_regs.TryGetValue(r, out var bound) && bound is LocalExpr bl)
                (_entryLocals.TryGetValue(block, out var m) ? m : _entryLocals[block] = [])[r] = bl.Local;
        }
    }

    private void InitializeEntry()
    {
        var xIdx = 0;
        var vIdx = 0;
        if (!IsHfa(_shape.ReturnType, out _, out _) && _shape.HasReturnBuffer)
        {
            _retBuf = NewLocal(_shape.ReturnType);
            _regs[8] = new AddressExpr(new LocalExpr(_retBuf));
        }
        if (!_shape.IsStatic)
        {
            var thisType = _method.DeclaringType!.ToTypeSignature(_shape.DeclaringIsValueType);
            _regs[xIdx++] = _shape.DeclaringIsValueType
                ? new AddressExpr(new ThisStructExpr(thisType))
                : new ArgExpr(_method.Parameters.ThisParameter!, thisType);
        }
        for (var i = 0; i < _shape.Parameters.Count && i < _method.Parameters.Count; i++)
        {
            var pt = _shape.Parameters[i];
            var arg = new ArgExpr(_method.Parameters[i], pt);
            if (pt is ByReferenceTypeSignature)
            {
                if (xIdx < 8) _regs[xIdx++] = arg;
            }
            else if (IsHfa(pt, out var count, out _) && vIdx + count <= 8)
            {
                var td = _mem.TypeDefOf(pt);
                for (var c = 0; c < count; c++)
                    _regs[32 + vIdx++] = (td >= 0 ? _mem.StructField(arg, td, c * 4) : null) ?? new StructChunkExpr(arg, c * 4, 4);
            }
            else if (MethodShape.IsFloat(pt))
            {
                if (vIdx < 8) _regs[32 + vIdx++] = arg;
            }
            else if (Expr.KindOf(pt) == ValueKind.Struct)
            {
                if (xIdx < 8) _regs[xIdx++] = _mem.ValueSize(pt) > 16 ? new AddressExpr(arg) : arg;
            }
            else
            {
                if (xIdx < 8) _regs[xIdx++] = arg;
            }
        }
        if (xIdx < 8) _regs[xIdx] = new MarkerExpr(MarkerKind.MethodInfo, _shape.MethodIndex, "method");
    }

    public void TranslateBlock(EmitContext ctx, List<Arm64Insn> insns, Arm64Block b, Arm64Block? nextInEmit, HashSet<Arm64Block> emitted)
    {
        _curBlockLiveAfter = ComputeLiveAfterMap(insns, b);
        _curBlockStart = b.Start;
        for (var i = b.Start; i < b.End; i++)
        {
            _curInsnIdx = i;
            var item = insns[i];
            var pc = item.Address;
            var ins = item.Ins;
            var isLast = i == b.End - 1;
            var m = ins.Mnemonic;

            if (isLast && b.Successors.Count == 0 && (m == Arm64Mnemonic.B && ins.OperandCount == 1))
            {
                var target = Arm64Lifter.BranchTarget(pc, ins, 0);
                if (target < _funcStart || target >= _funcEnd)
                {
                    EmitCallTarget(ctx, target, pc);
                    EmitReturn(ctx);
                    return;
                }
                // Internal unconditional jump to a pruned trap block
                ctx.IL.Add(CilOpCodes.Ldnull);
                ctx.IL.Add(CilOpCodes.Throw);
                return;
            }

            if (m == Arm64Mnemonic.RET)
            {
                EmitReturn(ctx);
                return;
            }

            if (isLast && m == Arm64Mnemonic.BR && b.JumpTable is { } jt)
            {
                SpillLiveOut(ctx, b, branchReadReg: jt.IndexReg);
                Expr.EmitAs(ctx, GetRegRaw(jt.IndexReg), _f.Int32);
                Arm64Block? defBlock = jt.DefaultTargetVa != 0
                    ? b.Successors.FirstOrDefault(s => s.Va == jt.DefaultTargetVa && emitted.Contains(s))
                    : null;
                var labels = new CilInstructionLabel[jt.Targets.Length];
                CilInstructionLabel? fallbackLabel = null;
                for (var ti = 0; ti < jt.Targets.Length; ti++)
                {
                    var tb = b.Successors.FirstOrDefault(s => s.Va == jt.Targets[ti] && emitted.Contains(s)) ?? defBlock;
                    if (tb is null)
                    {
                        fallbackLabel ??= new CilInstructionLabel();
                        labels[ti] = fallbackLabel;
                    }
                    else
                    {
                        labels[ti] = tb.Label;
                    }
                }
                ctx.IL.Add(CilOpCodes.Switch, labels);
                if (defBlock is not null)
                {
                    if (fallbackLabel is not null)
                        fallbackLabel.Instruction = ctx.IL.Add(CilOpCodes.Br, defBlock.Label);
                    else if (defBlock != nextInEmit)
                        ctx.IL.Add(CilOpCodes.Br, defBlock.Label);
                }
                else
                {
                    if (fallbackLabel is not null)
                        fallbackLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                    EmitDefaultReturn(ctx);
                }
                return;
            }

            if (isLast && b.Successors.Count == 0 && m == Arm64Mnemonic.BR)
            {
                var targetExpr = GetReg((Arm64RegisterOperand)ins.GetOperand(0));
                if (targetExpr is MarkerExpr { Marker: MarkerKind.VTableEntry, Base: { } obj } mkt)
                {
                    var td = _mem.TypeDefOf(obj.Type);
                    if (td >= 0 && VTableResolver.Resolve(_sym.Metadata, td, mkt.Index) is { } vt && EmitManagedCall(ctx, vt.MethodIndex, vt.SpecIndex, obj))
                    {
                        EmitReturn(ctx);
                        return;
                    }
                }
            }

            if (m == Arm64Mnemonic.B && ins.OperandCount == 1)
            {
                SpillLiveOut(ctx, b);
                if (b.Successors.Count == 1 && emitted.Contains(b.Successors[0]) && b.Successors[0] != nextInEmit)
                    ctx.IL.Add(CilOpCodes.Br, b.Successors[0].Label);
                else if (b.Successors.Count == 0)
                {
                    ctx.IL.Add(CilOpCodes.Ldnull);
                    ctx.IL.Add(CilOpCodes.Throw);
                }
                return;
            }

            if (isLast && TryEmitBranch(ctx, b, ins, pc, nextInEmit, emitted))
                return;

            // Elided trap branch inside a multi-instruction sequence (not ending the block): ignore
            if (IsBranch(ins))
            {
                Elided++;
                continue;
            }

            TranslateInstruction(ctx, pc, ins);
            if (ctx.IL.Count > 0 && ctx.IL[^1].OpCode.Code == CilCode.Throw)
                return;
        }
        SpillLiveOut(ctx, b);
        if (b.Successors.Count == 1 && emitted.Contains(b.Successors[0]) && b.Successors[0] != nextInEmit)
            ctx.IL.Add(CilOpCodes.Br, b.Successors[0].Label);
    }

    private bool TryEmitBranch(EmitContext ctx, Arm64Block b, in Arm64Instruction ins, ulong pc, Arm64Block? nextInEmit, HashSet<Arm64Block> emitted)
    {
        var m = ins.Mnemonic;
        if (!IsBranch(ins)) return false;

        Arm64Block? fallthrough = b.Successors.Count >= 1 && b.Successors[^1] != null ? b.Successors.FirstOrDefault(s => s.Va == pc + 4) ?? (b.Successors.Count > 1 ? b.Successors[1] : null) : null;
        if (fallthrough is not null && !emitted.Contains(fallthrough)) fallthrough = null;
        ulong targetVa = m == Arm64Mnemonic.TBZ || m == Arm64Mnemonic.TBNZ
            ? Arm64Lifter.BranchTarget(pc, ins, 2)
            : Arm64Lifter.BranchTarget(pc, ins, 1);
        Arm64Block? target = b.Successors.FirstOrDefault(s => s.Va == targetVa);
        if (target is not null && !emitted.Contains(target)) target = null;

        // Guard branches testing s_Il2CppMethodInitialized jump forward over init helpers when already initialized.
        if (m is Arm64Mnemonic.TBNZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.CBZ)
        {
            var testReg = GetReg((Arm64RegisterOperand)ins.GetOperand(0));
            if (testReg is MarkerExpr { Marker: MarkerKind.InitFlag })
            {
                Elided++;
                SpillLiveOut(ctx, b);
                if (m != Arm64Mnemonic.CBZ && target is not null && target != nextInEmit && (target.Va > pc + 4 || m is Arm64Mnemonic.TBNZ or Arm64Mnemonic.CBNZ))
                    ctx.IL.Add(CilOpCodes.Br, target.Label);
                else if (m == Arm64Mnemonic.CBZ && fallthrough is not null && fallthrough != nextInEmit)
                    ctx.IL.Add(CilOpCodes.Br, fallthrough.Label);
                else if (m != Arm64Mnemonic.CBZ && target is null && fallthrough is not null && fallthrough != nextInEmit)
                    ctx.IL.Add(CilOpCodes.Br, fallthrough.Label);
                return true;
            }
        }

        if (target is null)
        {
            // Conditional jump to a trap block was pruned from Successors.
            Elided++;
            SpillLiveOut(ctx, b);
            if (fallthrough is not null && fallthrough != nextInEmit) ctx.IL.Add(CilOpCodes.Br, fallthrough.Label);
            return true;
        }

        var branchReg = m is Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ
            ? Arm64Lifter.RegKey((Arm64RegisterOperand)ins.GetOperand(0))
            : -1;
        SpillLiveOut(ctx, b, branchReadReg: branchReg);
        Expr? cond = m switch
        {
            Arm64Mnemonic.CBZ => SimplifyCompare(Condition.Eq, GetReg((Arm64RegisterOperand)ins.GetOperand(0)), ZeroLike(GetReg((Arm64RegisterOperand)ins.GetOperand(0)))),
            Arm64Mnemonic.CBNZ => SimplifyCompare(Condition.Ne, GetReg((Arm64RegisterOperand)ins.GetOperand(0)), ZeroLike(GetReg((Arm64RegisterOperand)ins.GetOperand(0)))),
            Arm64Mnemonic.TBZ => BitTestCompare(GetReg((Arm64RegisterOperand)ins.GetOperand(0)), (int)ImmOp(ins, 1), zero: true),
            Arm64Mnemonic.TBNZ => BitTestCompare(GetReg((Arm64RegisterOperand)ins.GetOperand(0)), (int)ImmOp(ins, 1), zero: false),
            Arm64Mnemonic.B when ins.OperandCount == 2 => BuildFlagCompare(((Arm64EnumOperand)ins.GetOperand(0)).AsConditional),
            _ => null,
        };

        if (cond is null)
        {
            if (target != nextInEmit) ctx.IL.Add(CilOpCodes.Br, target.Label);
            return true;
        }
        cond.Emit(ctx);
        ctx.IL.Add(CilOpCodes.Brtrue, target.Label);
        if (fallthrough is not null && fallthrough != nextInEmit)
            ctx.IL.Add(CilOpCodes.Br, fallthrough.Label);
        return true;
    }

    private void TranslateInstruction(EmitContext ctx, ulong pc, in Arm64Instruction ins)
    {
        var m = ins.Mnemonic;
        if (m is Arm64Mnemonic.NOP) return;

        switch (m)
        {
            case Arm64Mnemonic.ADRP:
            {
                var rd = RegOp(ins, 0);
                var lbl = (Arm64LabelOperand)ins.GetOperand(1);
                var page = unchecked((long)(((pc & ~0xFFFUL) + (ulong)lbl.Offset)));
                SetReg(rd, new ConstExpr(page, ValueKind.I8));
                return;
            }
            case Arm64Mnemonic.ADR:
            {
                var rd = RegOp(ins, 0);
                var lbl = (Arm64LabelOperand)ins.GetOperand(1);
                SetReg(rd, new ConstExpr(unchecked((long)pc + lbl.Offset), ValueKind.I8));
                return;
            }
            case Arm64Mnemonic.MOV or Arm64Mnemonic.MOVZ or Arm64Mnemonic.MOVN:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                if (dst.Value.Kind == Arm64RegisterKind.SP) return;
                if (m == Arm64Mnemonic.MOV && ins.GetOperand(1).Kind == Arm64OperandKind.Register)
                {
                    var src = (Arm64RegisterOperand)ins.GetOperand(1);
                    if (src.Value.Kind == Arm64RegisterKind.SP) { SetReg(RegOp(ins, 0), new StackAddrPlaceholder(-_spDelta)); return; }
                    SetReg(RegOp(ins, 0), NarrowIfW(dst, GetReg(src)));
                    return;
                }
                var imm = ImmOp(ins, 1);
                if (m == Arm64Mnemonic.MOVN) imm = ~imm;
                if (ins.OperandCount >= 3 && ins.GetOperand(2).Kind == Arm64OperandKind.Shift)
                    imm <<= ((Arm64ShiftOperand)ins.GetOperand(2)).Amount;
                SetReg(RegOp(ins, 0), new ConstExpr(imm, dst.Value.Kind == Arm64RegisterKind.W ? ValueKind.I4 : ValueKind.I8));
                return;
            }
            case Arm64Mnemonic.MOVK:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var imm = ImmOp(ins, 1) & 0xFFFF;
                var shift = ins.OperandCount >= 3 && ins.GetOperand(2).Kind == Arm64OperandKind.Shift
                    ? ((Arm64ShiftOperand)ins.GetOperand(2)).Amount : 0;
                var cur = GetReg(dst);
                if (cur is ConstExpr c)
                {
                    var mask = ~(0xFFFFL << shift);
                    SetReg(Arm64Lifter.RegKey(dst), new ConstExpr((c.Value & mask) | (imm << shift), c.Kind));
                }
                else
                    SetReg(Arm64Lifter.RegKey(dst), new BinaryExpr(CilOpCodes.Or, cur, new ConstExpr(imm << shift, cur.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4), cur.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4));
                return;
            }
            case Arm64Mnemonic.FMOV:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var srcOp = ins.GetOperand(1);
                var isDouble = dst.Value.VKind == Arm64RegisterVKind.D;
                if (srcOp.Kind == Arm64OperandKind.Immediate)
                {
                    var immOp = (Arm64ImmediateOperand)srcOp;
                    SetReg(Arm64Lifter.RegKey(dst), new FloatExpr(immOp.IsFloat ? immOp.ValueAsFloat : immOp.Value, !isDouble));
                    return;
                }
                if (srcOp.Kind == Arm64OperandKind.Register)
                {
                    var sv = GetReg((Arm64RegisterOperand)srcOp);
                    if (dst.Value.Kind is Arm64RegisterKind.X or Arm64RegisterKind.W)
                    {
                        if (sv is FloatExpr fe)
                            SetReg(Arm64Lifter.RegKey(dst), dst.Value.Kind == Arm64RegisterKind.X
                                ? new ConstExpr(BitConverter.DoubleToInt64Bits(fe.Value), ValueKind.I8)
                                : new ConstExpr(BitConverter.SingleToInt32Bits((float)fe.Value), ValueKind.I4));
                        else
                            SetReg(Arm64Lifter.RegKey(dst), sv);
                    }
                    else
                    {
                        if (sv is ConstExpr ce)
                            SetReg(Arm64Lifter.RegKey(dst), isDouble
                                ? new FloatExpr(BitConverter.Int64BitsToDouble(ce.Value), false)
                                : new FloatExpr(BitConverter.Int32BitsToSingle(unchecked((int)ce.Value)), true));
                        else
                            SetReg(Arm64Lifter.RegKey(dst), sv);
                    }
                }
                return;
            }
            case Arm64Mnemonic.SXTW or Arm64Mnemonic.SXTH or Arm64Mnemonic.UXTH or Arm64Mnemonic.SXTB or Arm64Mnemonic.UXTB:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var src = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                if (m == Arm64Mnemonic.UXTB)
                {
                    SetReg(Arm64Lifter.RegKey(dst), new BinaryExpr(CilOpCodes.And, src, new ConstExpr(0xFF, ValueKind.I4), dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4));
                    return;
                }
                if (m == Arm64Mnemonic.UXTH)
                {
                    SetReg(Arm64Lifter.RegKey(dst), new BinaryExpr(CilOpCodes.And, src, new ConstExpr(0xFFFF, ValueKind.I4), dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4));
                    return;
                }
                var target = dst.Value.Kind == Arm64RegisterKind.X ? _f.Int64 : _f.Int32;
                SetReg(Arm64Lifter.RegKey(dst), new ConvExpr(src, target));
                return;
            }
            case Arm64Mnemonic.MVN or Arm64Mnemonic.NEG or Arm64Mnemonic.NEGS:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var k = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                var src = ReadShifted(ins, 1);
                if (src.Kind != k) src = new ConvExpr(src, k == ValueKind.I8 ? _f.Int64 : _f.Int32);
                var expr = m == Arm64Mnemonic.MVN
                    ? (Expr)new UnaryExpr(CilOpCodes.Not, src)
                    : new UnaryExpr(CilOpCodes.Neg, src);
                SetReg(Arm64Lifter.RegKey(dst), expr);
                if (m == Arm64Mnemonic.NEGS) SetCompareResult(expr);
                return;
            }
            case Arm64Mnemonic.UBFX or Arm64Mnemonic.SBFX or Arm64Mnemonic.UBFIZ or Arm64Mnemonic.SBFIZ or Arm64Mnemonic.BFI or Arm64Mnemonic.BFXIL:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var k = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                var bits = k == ValueKind.I8 ? 64 : 32;
                var src = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                var lsb = (int)ImmOp(ins, 2);
                var width = (int)ImmOp(ins, 3);
                long mask = width >= bits ? -1L : unchecked((long)((1UL << width) - 1));
                Expr result;
                if (m == Arm64Mnemonic.UBFX)
                {
                    var shifted = lsb > 0 ? new BinaryExpr(CilOpCodes.Shr_Un, src, new ConstExpr(lsb, ValueKind.I4), k) : src;
                    result = width < bits ? new BinaryExpr(CilOpCodes.And, shifted, new ConstExpr(mask, k), k) : shifted;
                }
                else if (m == Arm64Mnemonic.SBFX)
                {
                    var leftShift = Math.Max(0, bits - lsb - width);
                    var shl = leftShift > 0 ? new BinaryExpr(CilOpCodes.Shl, src, new ConstExpr(leftShift, ValueKind.I4), k) : src;
                    var rightShift = Math.Max(0, bits - width);
                    result = rightShift > 0 ? new BinaryExpr(CilOpCodes.Shr, shl, new ConstExpr(rightShift, ValueKind.I4), k) : shl;
                }
                else if (m is Arm64Mnemonic.UBFIZ or Arm64Mnemonic.SBFIZ)
                {
                    var masked = width < bits ? new BinaryExpr(CilOpCodes.And, src, new ConstExpr(mask, k), k) : src;
                    result = lsb > 0 ? new BinaryExpr(CilOpCodes.Shl, masked, new ConstExpr(lsb, ValueKind.I4), k) : masked;
                }
                else if (m == Arm64Mnemonic.BFI)
                {
                    var dstVal = GetReg(dst);
                    var clearMask = ~(mask << lsb);
                    var cleared = new BinaryExpr(CilOpCodes.And, dstVal, new ConstExpr(clearMask, k), k);
                    var piece = new BinaryExpr(CilOpCodes.And, src, new ConstExpr(mask, k), k);
                    if (lsb > 0) piece = new BinaryExpr(CilOpCodes.Shl, piece, new ConstExpr(lsb, ValueKind.I4), k);
                    result = new BinaryExpr(CilOpCodes.Or, cleared, piece, k);
                }
                else // BFXIL
                {
                    var dstVal = GetReg(dst);
                    var cleared = new BinaryExpr(CilOpCodes.And, dstVal, new ConstExpr(~mask, k), k);
                    var shifted = lsb > 0 ? new BinaryExpr(CilOpCodes.Shr_Un, src, new ConstExpr(lsb, ValueKind.I4), k) : src;
                    var piece = new BinaryExpr(CilOpCodes.And, shifted, new ConstExpr(mask, k), k);
                    result = new BinaryExpr(CilOpCodes.Or, cleared, piece, k);
                }
                SetReg(Arm64Lifter.RegKey(dst), result);
                return;
            }
            case Arm64Mnemonic.SMULL or Arm64Mnemonic.UMULL or Arm64Mnemonic.SMADDL or Arm64Mnemonic.UMADDL:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var isSigned = m is Arm64Mnemonic.SMULL or Arm64Mnemonic.SMADDL;
                var convType = isSigned ? _f.Int64 : _f.UInt64;
                var prod = new BinaryExpr(CilOpCodes.Mul,
                    new ConvExpr(GetReg((Arm64RegisterOperand)ins.GetOperand(1)), convType),
                    new ConvExpr(GetReg((Arm64RegisterOperand)ins.GetOperand(2)), convType), ValueKind.I8);
                if (m is Arm64Mnemonic.SMADDL or Arm64Mnemonic.UMADDL)
                {
                    var ra = GetReg((Arm64RegisterOperand)ins.GetOperand(3));
                    SetReg(Arm64Lifter.RegKey(dst), new BinaryExpr(CilOpCodes.Add, ra, prod, ValueKind.I8));
                }
                else
                    SetReg(Arm64Lifter.RegKey(dst), prod);
                return;
            }
            case Arm64Mnemonic.ADD or Arm64Mnemonic.ADDS:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var lhs = (Arm64RegisterOperand)ins.GetOperand(1);
                if (dst.Value.Kind == Arm64RegisterKind.SP && lhs.Value.Kind == Arm64RegisterKind.SP)
                {
                    _spDelta -= ImmOp(ins, 2);
                    return;
                }
                var leftVal = lhs.Value.Kind == Arm64RegisterKind.SP ? new StackAddrPlaceholder(-_spDelta) : GetReg(lhs);
                if (ins.GetOperand(2).Kind == Arm64OperandKind.Immediate)
                {
                    var disp = ImmOp(ins, 2);
                    if (ins.OperandCount >= 4 && ins.GetOperand(3).Kind == Arm64OperandKind.Shift)
                        disp <<= ((Arm64ShiftOperand)ins.GetOperand(3)).Amount;
                    if (leftVal is ConstExpr baseC && ResolveUsageAt(unchecked((ulong)(baseC.Value + disp))) is { } uExpr)
                    { SetReg(Arm64Lifter.RegKey(dst), uExpr); return; }
                    if (leftVal is StackAddrPlaceholder sp) { SetReg(Arm64Lifter.RegKey(dst), new StackAddrPlaceholder(sp.Key + disp)); return; }
                    if (_mem.ResolveFieldAddress(leftVal, disp) is { } fa) { SetReg(Arm64Lifter.RegKey(dst), fa); return; }
                    var k = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                    var res = new BinaryExpr(CilOpCodes.Add, leftVal, new ConstExpr(disp, k), k);
                    SetReg(Arm64Lifter.RegKey(dst), res);
                    if (m == Arm64Mnemonic.ADDS) SetCompareResult(res);
                    return;
                }
                var rhs = ReadShifted(ins, 2);
                var kind = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                var sum = new BinaryExpr(CilOpCodes.Add, leftVal, rhs, kind);
                SetReg(Arm64Lifter.RegKey(dst), sum);
                if (m == Arm64Mnemonic.ADDS) SetCompareResult(sum);
                return;
            }
            case Arm64Mnemonic.SUB or Arm64Mnemonic.SUBS:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var lhs = (Arm64RegisterOperand)ins.GetOperand(1);
                if (dst.Value.Kind == Arm64RegisterKind.SP && lhs.Value.Kind == Arm64RegisterKind.SP)
                {
                    _spDelta += ImmOp(ins, 2);
                    return;
                }
                if (dst.IsZeroRegister && m == Arm64Mnemonic.SUBS)
                {
                    _flagKind = FlagKind.Compare;
                    _flagLeft = GetReg(lhs);
                    _flagRight = ReadShifted(ins, 2);
                    return;
                }
                var leftVal = GetReg(lhs);
                var rightVal = ReadShifted(ins, 2);
                var kind = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                var diff = new BinaryExpr(CilOpCodes.Sub, leftVal, rightVal, kind);
                SetReg(Arm64Lifter.RegKey(dst), diff);
                if (m == Arm64Mnemonic.SUBS) SetCompareResult(diff);
                return;
            }
            case Arm64Mnemonic.MUL or Arm64Mnemonic.SDIV or Arm64Mnemonic.UDIV
                or Arm64Mnemonic.AND or Arm64Mnemonic.ANDS or Arm64Mnemonic.BIC or Arm64Mnemonic.BICS
                or Arm64Mnemonic.ORR or Arm64Mnemonic.ORN or Arm64Mnemonic.EOR or Arm64Mnemonic.EON
                or Arm64Mnemonic.LSL or Arm64Mnemonic.LSR or Arm64Mnemonic.ASR or Arm64Mnemonic.ROR:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                if (dst.IsZeroRegister && m == Arm64Mnemonic.ANDS)
                {
                    _flagKind = FlagKind.Test;
                    _flagLeft = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                    _flagRight = ReadShifted(ins, 2);
                    return;
                }
                var l = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                var r = ReadShifted(ins, 2);
                var kind = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                if (m is Arm64Mnemonic.BIC or Arm64Mnemonic.BICS or Arm64Mnemonic.ORN or Arm64Mnemonic.EON)
                {
                    if (r.Kind != kind) r = new ConvExpr(r, kind == ValueKind.I8 ? _f.Int64 : _f.Int32);
                    var combOp = m switch
                    {
                        Arm64Mnemonic.ORN => CilOpCodes.Or,
                        Arm64Mnemonic.EON => CilOpCodes.Xor,
                        _ => CilOpCodes.And,
                    };
                    var bitRes = new BinaryExpr(combOp, l, new UnaryExpr(CilOpCodes.Not, r), kind);
                    SetReg(Arm64Lifter.RegKey(dst), bitRes);
                    if (m == Arm64Mnemonic.BICS) SetCompareResult(bitRes);
                    return;
                }
                if (m == Arm64Mnemonic.ROR)
                {
                    var bits = kind == ValueKind.I8 ? 64 : 32;
                    var invSh = new BinaryExpr(CilOpCodes.Sub, new ConstExpr(bits, ValueKind.I4), r, ValueKind.I4);
                    var rorRes = new BinaryExpr(CilOpCodes.Or,
                        new BinaryExpr(CilOpCodes.Shr_Un, l, r, kind),
                        new BinaryExpr(CilOpCodes.Shl, l, invSh, kind), kind);
                    SetReg(Arm64Lifter.RegKey(dst), rorRes);
                    return;
                }
                var cil = m switch
                {
                    Arm64Mnemonic.MUL => CilOpCodes.Mul,
                    Arm64Mnemonic.SDIV => CilOpCodes.Div,
                    Arm64Mnemonic.UDIV => CilOpCodes.Div_Un,
                    Arm64Mnemonic.AND or Arm64Mnemonic.ANDS => CilOpCodes.And,
                    Arm64Mnemonic.ORR => CilOpCodes.Or,
                    Arm64Mnemonic.EOR => CilOpCodes.Xor,
                    Arm64Mnemonic.LSL => CilOpCodes.Shl,
                    Arm64Mnemonic.ASR => CilOpCodes.Shr,
                    _ => CilOpCodes.Shr_Un,
                };
                var res = new BinaryExpr(cil, l, r, kind);
                SetReg(Arm64Lifter.RegKey(dst), res);
                if (m == Arm64Mnemonic.ANDS) SetCompareResult(res);
                return;
            }
            case Arm64Mnemonic.MADD or Arm64Mnemonic.MSUB:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var rn = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                var rm = GetReg((Arm64RegisterOperand)ins.GetOperand(2));
                var ra = GetReg((Arm64RegisterOperand)ins.GetOperand(3));
                var k = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                var prod = new BinaryExpr(CilOpCodes.Mul, rn, rm, k);
                if (ins.GetOperand(3) is var op3 && op3.Kind == Arm64OperandKind.Register && ((Arm64RegisterOperand)op3).IsZeroRegister)
                    SetReg(Arm64Lifter.RegKey(dst), m == Arm64Mnemonic.MSUB ? new UnaryExpr(CilOpCodes.Neg, prod) : prod);
                else
                    SetReg(Arm64Lifter.RegKey(dst), m == Arm64Mnemonic.MADD
                        ? new BinaryExpr(CilOpCodes.Add, ra, prod, k)
                        : new BinaryExpr(CilOpCodes.Sub, ra, prod, k));
                return;
            }
            case Arm64Mnemonic.FADD or Arm64Mnemonic.FSUB or Arm64Mnemonic.FMUL or Arm64Mnemonic.FDIV:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var kind = dst.Value.VKind == Arm64RegisterVKind.D ? ValueKind.R8 : ValueKind.R4;
                var l = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                var r = GetReg((Arm64RegisterOperand)ins.GetOperand(2));
                var cil = m switch
                {
                    Arm64Mnemonic.FADD => CilOpCodes.Add,
                    Arm64Mnemonic.FSUB => CilOpCodes.Sub,
                    Arm64Mnemonic.FMUL => CilOpCodes.Mul,
                    _ => CilOpCodes.Div,
                };
                SetReg(Arm64Lifter.RegKey(dst), new BinaryExpr(cil, l, r, kind));
                return;
            }
            case Arm64Mnemonic.FMADD or Arm64Mnemonic.FMSUB:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var kind = dst.Value.VKind == Arm64RegisterVKind.D ? ValueKind.R8 : ValueKind.R4;
                var prod = new BinaryExpr(CilOpCodes.Mul, GetReg((Arm64RegisterOperand)ins.GetOperand(1)), GetReg((Arm64RegisterOperand)ins.GetOperand(2)), kind);
                var ra = GetReg((Arm64RegisterOperand)ins.GetOperand(3));
                SetReg(Arm64Lifter.RegKey(dst), m == Arm64Mnemonic.FMADD
                    ? new BinaryExpr(CilOpCodes.Add, prod, ra, kind)
                    : new BinaryExpr(CilOpCodes.Sub, ra, prod, kind));
                return;
            }
            case Arm64Mnemonic.FNEG:
                SetReg(RegOp(ins, 0), new UnaryExpr(CilOpCodes.Neg, GetReg((Arm64RegisterOperand)ins.GetOperand(1))));
                return;
            case Arm64Mnemonic.FABS:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var t = dst.Value.VKind == Arm64RegisterVKind.D ? _f.Double : _f.Single;
                SetReg(Arm64Lifter.RegKey(dst), new MathCallExpr("Abs", t, GetReg((Arm64RegisterOperand)ins.GetOperand(1))));
                return;
            }
            case Arm64Mnemonic.FABD:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var isD = dst.Value.VKind == Arm64RegisterVKind.D;
                var kind = isD ? ValueKind.R8 : ValueKind.R4;
                var diff = new BinaryExpr(CilOpCodes.Sub, GetReg((Arm64RegisterOperand)ins.GetOperand(1)), GetReg((Arm64RegisterOperand)ins.GetOperand(2)), kind);
                SetReg(Arm64Lifter.RegKey(dst), new MathCallExpr("Abs", isD ? _f.Double : _f.Single, diff));
                return;
            }
            case Arm64Mnemonic.FMIN or Arm64Mnemonic.FMAX:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var isD = dst.Value.VKind == Arm64RegisterVKind.D;
                SetReg(Arm64Lifter.RegKey(dst), new MathCallExpr(m == Arm64Mnemonic.FMIN ? "Min" : "Max", isD ? _f.Double : _f.Single,
                    GetReg((Arm64RegisterOperand)ins.GetOperand(1)), GetReg((Arm64RegisterOperand)ins.GetOperand(2))));
                return;
            }
            case Arm64Mnemonic.FRINTM or Arm64Mnemonic.FRINTP or Arm64Mnemonic.FRINTN or Arm64Mnemonic.FRINTZ:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var isD = dst.Value.VKind == Arm64RegisterVKind.D;
                var arg = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                var fnName = m switch { Arm64Mnemonic.FRINTM => "Floor", Arm64Mnemonic.FRINTP => "Ceiling", Arm64Mnemonic.FRINTN => "Round", _ => "Truncate" };
                Expr call = new MathCallExpr(fnName, _f.Double, isD ? arg : new ConvExpr(arg, _f.Double));
                SetReg(Arm64Lifter.RegKey(dst), isD ? call : new ConvExpr(call, _f.Single));
                return;
            }
            case Arm64Mnemonic.FSQRT:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var isD = dst.Value.VKind == Arm64RegisterVKind.D;
                var arg = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                Expr sqrt = new MathCallExpr("Sqrt", _f.Double, isD ? arg : new ConvExpr(arg, _f.Double));
                SetReg(Arm64Lifter.RegKey(dst), isD ? sqrt : new ConvExpr(sqrt, _f.Single));
                return;
            }
            case Arm64Mnemonic.SCVTF or Arm64Mnemonic.UCVTF or Arm64Mnemonic.FCVTZS or Arm64Mnemonic.FCVTZU
                or Arm64Mnemonic.FCVTPS or Arm64Mnemonic.FCVTPU or Arm64Mnemonic.FCVTMS or Arm64Mnemonic.FCVTMU
                or Arm64Mnemonic.FCVTNS or Arm64Mnemonic.FCVTNU or Arm64Mnemonic.FCVTAS or Arm64Mnemonic.FCVTAU
                or Arm64Mnemonic.FCVT:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var srcOp = (Arm64RegisterOperand)ins.GetOperand(1);
                var srcExpr = GetReg(srcOp);
                if (m is Arm64Mnemonic.FCVTPS or Arm64Mnemonic.FCVTPU or Arm64Mnemonic.FCVTMS or Arm64Mnemonic.FCVTMU
                    or Arm64Mnemonic.FCVTNS or Arm64Mnemonic.FCVTNU or Arm64Mnemonic.FCVTAS or Arm64Mnemonic.FCVTAU)
                {
                    var fn = m is Arm64Mnemonic.FCVTPS or Arm64Mnemonic.FCVTPU ? "Ceiling"
                        : m is Arm64Mnemonic.FCVTMS or Arm64Mnemonic.FCVTMU ? "Floor"
                        : "Round";
                    var srcDouble = srcOp.Value.VKind == Arm64RegisterVKind.D ? srcExpr : new ConvExpr(srcExpr, _f.Double);
                    var rounded = new MathCallExpr(fn, _f.Double, srcDouble);
                    SetReg(Arm64Lifter.RegKey(dst), new ConvExpr(rounded, dst.Value.Kind == Arm64RegisterKind.X ? _f.Int64 : _f.Int32));
                    return;
                }
                var target = m is Arm64Mnemonic.FCVTZS or Arm64Mnemonic.FCVTZU
                    ? (dst.Value.Kind == Arm64RegisterKind.X ? _f.Int64 : _f.Int32)
                    : (dst.Value.VKind == Arm64RegisterVKind.D ? _f.Double : _f.Single);
                SetReg(Arm64Lifter.RegKey(dst), new ConvExpr(srcExpr, target));
                return;
            }
            case Arm64Mnemonic.CMP or Arm64Mnemonic.CCMP:
                _flagKind = FlagKind.Compare;
                _flagLeft = GetReg((Arm64RegisterOperand)ins.GetOperand(0));
                _flagRight = m == Arm64Mnemonic.CCMP ? ReadOpOrImm(ins, 1) : ReadShifted(ins, 1);
                return;
            case Arm64Mnemonic.CMN:
                _flagKind = FlagKind.Compare;
                _flagLeft = GetReg((Arm64RegisterOperand)ins.GetOperand(0));
                var cr = ReadShifted(ins, 1);
                _flagRight = cr is ConstExpr cc ? new ConstExpr(-cc.Value, cc.Kind) : new UnaryExpr(CilOpCodes.Neg, cr);
                return;
            case Arm64Mnemonic.TST:
                _flagKind = FlagKind.Test;
                _flagLeft = GetReg((Arm64RegisterOperand)ins.GetOperand(0));
                _flagRight = ReadShifted(ins, 1);
                return;
            case Arm64Mnemonic.FCMP or Arm64Mnemonic.FCMPE or Arm64Mnemonic.FCCMP:
                _flagKind = FlagKind.FloatCompare;
                _flagLeft = GetReg((Arm64RegisterOperand)ins.GetOperand(0));
                _flagRight = ReadOpOrImm(ins, 1);
                return;
            case Arm64Mnemonic.CSET:
            {
                var dst = RegOp(ins, 0);
                var cond = ((Arm64EnumOperand)ins.GetOperand(1)).AsConditional;
                SetReg(dst, (Expr?)BuildFlagCompare(cond) ?? new ConstExpr(0, ValueKind.I4));
                return;
            }
            case Arm64Mnemonic.CINC or Arm64Mnemonic.CINV or Arm64Mnemonic.CNEG:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var cond = ((Arm64EnumOperand)ins.GetOperand(2)).AsConditional;
                var srcVal = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                var k = dst.Value.Kind == Arm64RegisterKind.X ? ValueKind.I8 : ValueKind.I4;
                Expr vTrue = m switch
                {
                    Arm64Mnemonic.CINC => srcVal is ConstExpr ci ? new ConstExpr(ci.Value + 1, ci.Kind) : new BinaryExpr(CilOpCodes.Add, srcVal, new ConstExpr(1, k), k),
                    Arm64Mnemonic.CINV => srcVal is ConstExpr civ ? new ConstExpr(~civ.Value, civ.Kind) : new UnaryExpr(CilOpCodes.Not, srcVal),
                    _ => srcVal is ConstExpr cn ? new ConstExpr(-cn.Value, cn.Kind) : new UnaryExpr(CilOpCodes.Neg, srcVal),
                };
                EmitCondSelect(ctx, dst, BuildFlagCompare(cond), vTrue, srcVal);
                return;
            }
            case Arm64Mnemonic.CSEL or Arm64Mnemonic.CSINC or Arm64Mnemonic.CSINV or Arm64Mnemonic.CSNEG or Arm64Mnemonic.FCSEL:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var cond = ((Arm64EnumOperand)ins.GetOperand(3)).AsConditional;
                var vTrue = GetReg((Arm64RegisterOperand)ins.GetOperand(1));
                var vFalse = GetReg((Arm64RegisterOperand)ins.GetOperand(2));
                if (m == Arm64Mnemonic.CSINC) vFalse = vFalse is ConstExpr fc ? new ConstExpr(fc.Value + 1, fc.Kind) : new BinaryExpr(CilOpCodes.Add, vFalse, new ConstExpr(1, vFalse.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4), vFalse.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4);
                else if (m == Arm64Mnemonic.CSINV) vFalse = vFalse is ConstExpr fc2 ? new ConstExpr(~fc2.Value, fc2.Kind) : new UnaryExpr(CilOpCodes.Not, vFalse);
                else if (m == Arm64Mnemonic.CSNEG) vFalse = vFalse is ConstExpr fc3 ? new ConstExpr(-fc3.Value, fc3.Kind) : new UnaryExpr(CilOpCodes.Neg, vFalse);
                EmitCondSelect(ctx, dst, BuildFlagCompare(cond), vTrue, vFalse);
                return;
            }
            case Arm64Mnemonic.LDR or Arm64Mnemonic.LDRB or Arm64Mnemonic.LDRH or Arm64Mnemonic.LDRSB or Arm64Mnemonic.LDRSH or Arm64Mnemonic.LDRSW
                or Arm64Mnemonic.LDUR or Arm64Mnemonic.LDURB or Arm64Mnemonic.LDURH or Arm64Mnemonic.LDURSB or Arm64Mnemonic.LDURSH or Arm64Mnemonic.LDURSW:
            {
                var dst = (Arm64RegisterOperand)ins.GetOperand(0);
                var size = LoadStoreSize(m, dst);
                if (ins.GetOperand(1).Kind == Arm64OperandKind.Label)
                {
                    var litVa = Arm64Lifter.BranchTarget(pc, ins, 1);
                    SetReg(Arm64Lifter.RegKey(dst), ReadGlobal(litVa, size, dst.Value.Kind >= Arm64RegisterKind.VScalar));
                    return;
                }
                var memOp = (Arm64MemoryOperand)ins.GetOperand(1);
                var postImm = ins.OperandCount >= 3 && ins.GetOperand(2).Kind == Arm64OperandKind.Immediate ? ImmOp(ins, 2) : 0;
                var val = LoadMem(ctx, memOp.Accessor, size, dst.Value.Kind >= Arm64RegisterKind.VScalar, dst);
                var dstKey = Arm64Lifter.RegKey(dst);
                if (!IsSpCalleeSave(memOp.Accessor, dst)) SetReg(dstKey, val);
                ApplyWriteback(memOp.Accessor, postImm, dstKey);
                return;
            }
            case Arm64Mnemonic.LDP:
            {
                var r1 = (Arm64RegisterOperand)ins.GetOperand(0);
                var r2 = (Arm64RegisterOperand)ins.GetOperand(1);
                var memOp = (Arm64MemoryOperand)ins.GetOperand(2);
                var postImm = ins.OperandCount >= 4 && ins.GetOperand(3).Kind == Arm64OperandKind.Immediate ? ImmOp(ins, 3) : 0;
                var size = LoadStoreSize(Arm64Mnemonic.LDR, r1);
                var k1 = Arm64Lifter.RegKey(r1);
                var k2 = Arm64Lifter.RegKey(r2);
                if (!IsSpCalleeSave(memOp.Accessor, r1))
                    SetReg(k1, LoadMem(ctx, memOp.Accessor, size, r1.Value.Kind >= Arm64RegisterKind.VScalar, r1));
                if (!IsSpCalleeSave(memOp.Accessor, r2))
                    SetReg(k2, LoadMem(ctx, memOp.Accessor, size, r2.Value.Kind >= Arm64RegisterKind.VScalar, r2, extraDisp: size));
                ApplyWriteback(memOp.Accessor, postImm, k1, k2);
                return;
            }
            case Arm64Mnemonic.STR or Arm64Mnemonic.STRB or Arm64Mnemonic.STRH or Arm64Mnemonic.STUR or Arm64Mnemonic.STURB or Arm64Mnemonic.STURH:
            {
                var src = (Arm64RegisterOperand)ins.GetOperand(0);
                var memOp = (Arm64MemoryOperand)ins.GetOperand(1);
                var postImm = ins.OperandCount >= 3 && ins.GetOperand(2).Kind == Arm64OperandKind.Immediate ? ImmOp(ins, 2) : 0;
                if (memOp.Accessor.BaseRegister.Kind == Arm64RegisterKind.SP && memOp.Accessor.IsPreIncrement)
                    _spDelta -= memOp.Accessor.Immediate;
                if (!IsSpCalleeSave(memOp.Accessor, src))
                    StoreMem(ctx, ins, memOp.Accessor, LoadStoreSize(m, src), GetReg(src));
                if (memOp.Accessor.BaseRegister.Kind == Arm64RegisterKind.SP)
                {
                    if (postImm != 0) _spDelta -= postImm;
                }
                else
                    ApplyWriteback(memOp.Accessor, postImm);
                return;
            }
            case Arm64Mnemonic.STP:
            {
                var r1 = (Arm64RegisterOperand)ins.GetOperand(0);
                var r2 = (Arm64RegisterOperand)ins.GetOperand(1);
                var memOp = (Arm64MemoryOperand)ins.GetOperand(2);
                var postImm = ins.OperandCount >= 4 && ins.GetOperand(3).Kind == Arm64OperandKind.Immediate ? ImmOp(ins, 3) : 0;
                if (memOp.Accessor.BaseRegister.Kind == Arm64RegisterKind.SP && memOp.Accessor.IsPreIncrement)
                    _spDelta -= memOp.Accessor.Immediate;
                var size = LoadStoreSize(Arm64Mnemonic.STR, r1);
                if (!IsSpCalleeSave(memOp.Accessor, r1)) StoreMem(ctx, ins, memOp.Accessor, size, GetReg(r1));
                if (!IsSpCalleeSave(memOp.Accessor, r2)) StoreMem(ctx, ins, memOp.Accessor, size, GetReg(r2), extraDisp: size);
                if (memOp.Accessor.BaseRegister.Kind == Arm64RegisterKind.SP)
                {
                    if (postImm != 0) _spDelta -= postImm;
                }
                else
                    ApplyWriteback(memOp.Accessor, postImm);
                return;
            }
            case Arm64Mnemonic.BL:
            {
                var target = Arm64Lifter.BranchTarget(pc, ins, 0);
                EmitCallTarget(ctx, target, pc);
                return;
            }
            case Arm64Mnemonic.BLR:
            {
                var targetExpr = GetReg((Arm64RegisterOperand)ins.GetOperand(0));
                if (targetExpr is MarkerExpr { Marker: MarkerKind.VTableEntry, Base: { } obj } mkt)
                {
                    var td = _mem.TypeDefOf(obj.Type);
                    if (td >= 0 && VTableResolver.Resolve(_sym.Metadata, td, mkt.Index) is { } vt && EmitManagedCall(ctx, vt.MethodIndex, vt.SpecIndex, obj))
                        return;
                }
                Unresolved++;
                ctx.IL.Add(CilOpCodes.Ldstr, $"arm64_indirect@{pc - _sym.Registration.Reader.ImageBase:X}");
                EmitFourCallNativeArgs(ctx);
                ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.CallNative);
                var tmp = NewLocal(_f.Int64);
                ctx.IL.Add(CilOpCodes.Stloc, tmp);
                ClobberVolatiles();
                _regs[0] = new LocalExpr(tmp);
                return;
            }
            default:
                // Non-critical barrier / hint instructions ignored cleanly; others noted.
                if (m is not (Arm64Mnemonic.DMB or Arm64Mnemonic.DSB or Arm64Mnemonic.ISB or Arm64Mnemonic.PRFM or Arm64Mnemonic.HINT or Arm64Mnemonic.BTI or Arm64Mnemonic.CLREX))
                {
                    if (ins.OperandCount > 0 && ins.GetOperand(0).Kind == Arm64OperandKind.Register)
                    {
                        var dk = Arm64Lifter.RegKey((Arm64RegisterOperand)ins.GetOperand(0));
                        if (dk >= 0 && _regs.TryGetValue(dk, out var prev) && prev is AddressExpr { Target: ThisStructExpr })
                            _regs[dk] = new NativeExpr($"unhandled:{m}", ValueKind.I4);
                    }
                    Unsupported++;
                    _notes.Add($"unsupported instruction at 0x{pc - _sym.Registration.Reader.ImageBase:X}: {ins}");
                }
                return;
        }
    }

    private void EmitCondSelect(EmitContext ctx, in Arm64RegisterOperand dst, Expr? condExpr, Expr vTrue, Expr vFalse)
    {
        if (condExpr is null) { SetReg(Arm64Lifter.RegKey(dst), vTrue); return; }
        var fallbackType = dst.Value.Kind >= Arm64RegisterKind.VScalar
            ? (dst.Value.VKind == Arm64RegisterVKind.D ? _f.Double : _f.Single)
            : (dst.Value.Kind == Arm64RegisterKind.X ? _f.Int64 : _f.Int32);
        var type = vTrue.Type ?? vFalse.Type ?? fallbackType;
        if (vTrue is ConstExpr && vFalse is ConstExpr)
        {
            SetReg(Arm64Lifter.RegKey(dst), new CondSelectExpr(condExpr, vTrue, vFalse, type));
            return;
        }
        var local = NewLocal(type);
        var elseLabel = new CilInstructionLabel();
        var endLabel = new CilInstructionLabel();
        condExpr.Emit(ctx);
        ctx.IL.Add(CilOpCodes.Brfalse, elseLabel);
        Expr.EmitAs(ctx, vTrue, type);
        ctx.IL.Add(CilOpCodes.Stloc, local);
        ctx.IL.Add(CilOpCodes.Br, endLabel);
        elseLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        Expr.EmitAs(ctx, vFalse, type);
        ctx.IL.Add(CilOpCodes.Stloc, local);
        endLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        var localExpr = new LocalExpr(local);
        if (_gotValues.Contains(vTrue) || _gotValues.Contains(vFalse))
            _gotValues.Add(localExpr);
        SetReg(Arm64Lifter.RegKey(dst), localExpr);
    }

    private void ApplyWriteback(in Arm64MemoryAccessorAny acc, long postImm, int writtenReg1 = -1, int writtenReg2 = -1)
    {
        if (acc.BaseRegister.Kind == Arm64RegisterKind.SP)
        {
            if (acc.IsPreIncrement) _spDelta -= acc.Immediate;
            else if (postImm != 0) _spDelta -= postImm;
            return;
        }
        var delta = acc.IsPreIncrement ? acc.Immediate : postImm;
        if (delta == 0 || acc.HasIndexRegister) return;
        var bk = Arm64Lifter.RawRegKey(acc.BaseRegister);
        if (bk < 0 || bk == 99 || bk == writtenReg1 || bk == writtenReg2) return;
        var baseExpr = GetRegRaw(bk);
        if (baseExpr is ConstExpr cb)
        {
            var addr = unchecked(cb.Value + delta);
            SetReg(bk, ResolveUsageAt(unchecked((ulong)addr)) ?? new ConstExpr(addr, ValueKind.I8));
            return;
        }
        if (baseExpr is StackAddrPlaceholder sp) { SetReg(bk, new StackAddrPlaceholder(sp.Key + delta)); return; }
        if (_mem.ResolveFieldAddress(baseExpr, delta) is { } fa) { SetReg(bk, fa); return; }
        SetReg(bk, new BinaryExpr(CilOpCodes.Add, baseExpr, new ConstExpr(delta, ValueKind.I8), ValueKind.I8));
    }

    private Expr LoadMem(EmitContext ctx, in Arm64MemoryAccessorAny acc, int size, bool isFloat, in Arm64RegisterOperand dst, int extraDisp = 0)
    {
        var bk = Arm64Lifter.RawRegKey(acc.BaseRegister);
        var disp = (long)acc.Immediate + extraDisp;
        if (bk == 99 && !acc.HasIndexRegister)
            return LoadStackSlot(disp - _spDelta, size, isFloat);
        var baseExpr = bk < 0 ? new ConstExpr(0, ValueKind.I8) : GetRegRaw(bk);
        if (baseExpr is MarkerExpr { Marker: MarkerKind.TypeInfo } && !acc.HasIndexRegister
            && ((size == 1 && disp is >= 200 and <= 360) || (size == 4 && disp is >= 200 and <= 260)))
            return new MarkerExpr(MarkerKind.InitFlag, -1, "cctor_init");
        if (baseExpr is ConstExpr cb && !acc.HasIndexRegister)
            return ReadGlobal(unchecked((ulong)(cb.Value + disp)), size, isFloat);
        if (baseExpr is StackAddrPlaceholder sp && !acc.HasIndexRegister)
            return LoadStackSlot(sp.Key + disp, size, isFloat);
        if (_gotValues.Contains(baseExpr) && !acc.HasIndexRegister && disp == 0)
            return baseExpr;
        if (disp == _mem.StaticFieldsOffset && !acc.HasIndexRegister && size == 8 && baseExpr is NativeExpr
            && _lastRemat.Values.OfType<MarkerExpr>().LastOrDefault(m => m.Marker == MarkerKind.TypeInfo) is { } ti)
            return new MarkerExpr(MarkerKind.StaticFields, ti.Index, $"{_sym.Names.TypeDefName(ti.Index, true)}.static_fields");

        Expr? indexExpr = acc.HasIndexRegister ? GetRegRaw(Arm64Lifter.RawRegKey(acc.IndexRegister)) : null;
        int scale = acc.HasExtend && acc.Extend.Amount > 0 ? 1 << acc.Extend.Amount : 1;
        if (indexExpr is CondSelectExpr { TrueVal: ConstExpr ct, FalseVal: ConstExpr cf } cs
            && baseExpr.Type is not null && baseExpr.Type is not SzArrayTypeSignature && _mem.TypeDefOf(baseExpr.Type) >= 0)
        {
            var eTrue = _mem.Load(baseExpr, null, 1, disp + ct.Value * scale, size);
            var eFalse = _mem.Load(baseExpr, null, 1, disp + cf.Value * scale, size);
            if (eTrue is not null && eFalse is not null)
            {
                EmitCondSelect(ctx, dst, cs.Cond, eTrue, eFalse);
                return GetRegRaw(Arm64Lifter.RegKey(dst));
            }
        }
        return _mem.Load(baseExpr, indexExpr, scale, disp, size)
               ?? new NativeExpr($"*({baseExpr.Describe()}+0x{disp:X})", isFloat ? (size == 8 ? ValueKind.R8 : ValueKind.R4) : (size == 4 ? ValueKind.I4 : ValueKind.I8));
    }

    private void StoreMem(EmitContext ctx, in Arm64Instruction ins, in Arm64MemoryAccessorAny acc, int size, Expr value, int extraDisp = 0)
    {
        var bk = Arm64Lifter.RawRegKey(acc.BaseRegister);
        var disp = (long)acc.Immediate + extraDisp;
        if (bk == 99 && !acc.HasIndexRegister)
        {
            StoreStackSlot(ctx, disp - _spDelta, size, value);
            return;
        }
        var baseExpr = bk < 0 ? new ConstExpr(0, ValueKind.I8) : GetRegRaw(bk);
        if (baseExpr is ConstExpr) return;   // guard byte in .bss (s_Il2CppMethodInitialized)
        if (baseExpr is StackAddrPlaceholder sp && !acc.HasIndexRegister) { StoreStackSlot(ctx, sp.Key + disp, size, value); return; }
        Expr? indexExpr = acc.HasIndexRegister ? GetRegRaw(Arm64Lifter.RawRegKey(acc.IndexRegister)) : null;
        int scale = acc.HasExtend && acc.Extend.Amount > 0 ? 1 << acc.Extend.Amount : 1;
        if (_mem.Store(ctx, baseExpr, indexExpr, scale, disp, size, value)) return;
        Unsupported++;
        ctx.IL.Add(CilOpCodes.Ldstr, $"{ins} ; value = {value.Describe()}");
        ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.Stmt);
    }

    private Expr LoadStackSlot(long key, int size, bool isFloat)
    {
        if (TryFindStackStruct(key, out var slotStart, out var entry))
        {
            var fieldOff = (int)(key - slotStart);
            var localExpr = new LocalExpr(entry.Local);
            if (fieldOff == 0 && (Expr.KindOf(entry.Type) != ValueKind.Struct || size >= entry.Size))
                return localExpr;
            if (_mem.TypeDefOf(entry.Type) is >= 0 and var td && _mem.StructField(localExpr, td, fieldOff) is { } fe
                && (fe.Type is null || _mem.ValueSize(fe.Type) >= size))
                return fe;
            return new StructChunkExpr(localExpr, fieldOff, size);
        }
        if (_slots.TryGetValue(key, out var slotVal)) return slotVal;
        return new NativeExpr($"stack[0x{key:X}]", isFloat ? (size == 8 ? ValueKind.R8 : ValueKind.R4) : (size == 4 ? ValueKind.I4 : ValueKind.I8));
    }

    private void StoreStackSlot(EmitContext ctx, long key, int size, Expr value)
    {
        if (TryFindStackStruct(key, out var slotStart, out var entry))
        {
            var fieldOff = (int)(key - slotStart);
            if (fieldOff == 0 && (Expr.KindOf(entry.Type) != ValueKind.Struct || size >= entry.Size))
            {
                Expr.EmitAs(ctx, value, entry.Type);
                ctx.IL.Add(CilOpCodes.Stloc, entry.Local);
                return;
            }
            if (_mem.Store(ctx, new AddressExpr(new LocalExpr(entry.Local)), null, 1, fieldOff, size, value))
                return;
        }
        _slots[key] = value;
    }

    private CilLocalVariable BindStackStruct(EmitContext ctx, long key, TypeSignature type)
    {
        var size = Math.Max(1, _mem.ValueSize(type));
        if (_stackStructs.TryGetValue(key, out var existing) && SignatureComparer.Default.Equals(existing.Type, type))
        {
            PopulateBoundLocalFromSlots(ctx, key, type, existing.Local);
            ClearSlotRange(key, size);
            return existing.Local;
        }
        foreach (var k in _stackStructs.Where(kv => kv.Key < key + size && key < kv.Key + kv.Value.Size).Select(kv => kv.Key).ToList())
            _stackStructs.Remove(k);

        if (_slots.TryGetValue(key, out var singleSlot)
            && singleSlot is LocalExpr le && SignatureComparer.Default.Equals(le.Local.VariableType, type))
        {
            _stackStructs[key] = (le.Local, type, size);
            ClearSlotRange(key, size);
            return le.Local;
        }

        var loc = NewLocal(type);
        PopulateBoundLocalFromSlots(ctx, key, type, loc);
        ClearSlotRange(key, size);
        _stackStructs[key] = (loc, type, size);
        return loc;
    }

    private void PopulateBoundLocalFromSlots(EmitContext ctx, long key, TypeSignature type, CilLocalVariable loc)
    {
        if (_slots.TryGetValue(key, out var slotVal) && Expr.KindOf(type) != ValueKind.Struct)
        {
            Expr.EmitAs(ctx, slotVal, type);
            ctx.IL.Add(CilOpCodes.Stloc, loc);
            return;
        }
        if (Expr.KindOf(type) == ValueKind.Struct && _mem.TypeDefOf(type) is >= 0 and var td)
        {
            var localExpr = new LocalExpr(loc);
            if (_slots.TryGetValue(key, out var wholeVal) && wholeVal.Type is not null && SignatureComparer.Default.Equals(wholeVal.Type, type))
            {
                Expr.EmitAs(ctx, wholeVal, type);
                ctx.IL.Add(CilOpCodes.Stloc, loc);
                return;
            }
            var addrExpr = new AddressExpr(localExpr);
            foreach (var f in _mem.FieldsOf(td))
            {
                if (f.IsStatic) continue;
                if (_slots.TryGetValue(key + f.Offset, out var fv))
                {
                    var fsz = Math.Max(1, _mem.ValueSize(_mem.FieldType(f, type)));
                    _mem.Store(ctx, addrExpr, null, 1, f.Offset, fsz, fv);
                }
            }
        }
        else if (_slots.TryGetValue(key, out var fallbackVal))
        {
            Expr.EmitAs(ctx, fallbackVal, type);
            ctx.IL.Add(CilOpCodes.Stloc, loc);
        }
    }

    private void ClearSlotRange(long key, int size)
    {
        foreach (var sk in _slots.Keys.Where(k => k >= key && k < key + size).ToList())
            _slots.Remove(sk);
    }

    private bool TryFindStackStruct(long key, out long slotStart, out (CilLocalVariable Local, TypeSignature Type, int Size) entry)
    {
        foreach (var kv in _stackStructs)
        {
            if (key >= kv.Key && key < kv.Key + kv.Value.Size)
            {
                slotStart = kv.Key;
                entry = kv.Value;
                return true;
            }
        }
        slotStart = 0;
        entry = default;
        return false;
    }

    private Expr ReadGlobal(ulong va, int size, bool isFloat)
    {
        if (ResolveUsageAt(va) is { } u) return u;
        var reader = _sym.Registration.Reader;
        if (size == 8 && reader.IsWritableData(va))
        {
            var ptr = reader.ReadPointer(va);
            if (ResolveUsageAt(ptr) is { } gu)
            {
                _gotValues.Add(gu);
                return gu;
            }
        }
        if (!reader.IsWritableData(va) && reader.IsMapped(va))
        {
            if (isFloat && size == 4) return new FloatExpr(BitConverter.Int32BitsToSingle((int)reader.ReadU32(va)), true);
            if (isFloat && size == 8) return new FloatExpr(BitConverter.Int64BitsToDouble((long)reader.ReadU64(va)), false);
            if (size == 4) return new ConstExpr(reader.ReadI32(va), ValueKind.I4);
            if (size == 8) return new ConstExpr((long)reader.ReadU64(va), ValueKind.I8);
            if (size == 16) return new Const128Expr(reader.ReadU64(va), reader.ReadU64(va + 8));
        }
        if (size == 1 && reader.IsWritableData(va))
            return new MarkerExpr(MarkerKind.InitFlag, -1, $"arm64_init_{va:X}");
        return new NativeExpr($"global_{va - reader.ImageBase:X}", isFloat ? (size == 8 ? ValueKind.R8 : ValueKind.R4) : (size == 4 ? ValueKind.I4 : ValueKind.I8));
    }

    private Expr? ResolveUsageAt(ulong va) => _sym.TryGetUsage(va, out var u) ? UsageValue(u) : null;

    private Expr UsageValue(MetadataUsage u) => u.Kind switch
    {
        MetadataUsageKind.StringLiteral => new StringExpr(_sym.Metadata.GetStringLiteral(u.Index), _f.String),
        MetadataUsageKind.TypeInfo or MetadataUsageKind.Il2CppType => new MarkerExpr(
            u.Kind == MetadataUsageKind.TypeInfo ? MarkerKind.TypeInfo : MarkerKind.Il2CppType,
            _sym.Registration.TryGetType(u.Index, out var t) ? _sym.Names.ResolveTypeDefIndex(t) : -1, _sym.DescribeUsage(u), usage: u),
        MetadataUsageKind.FieldInfo or MetadataUsageKind.FieldRva => new MarkerExpr(MarkerKind.FieldInfo, u.Index, _sym.DescribeUsage(u), usage: u),
        _ => new MarkerExpr(MarkerKind.MethodInfo, u.Index, _sym.DescribeUsage(u), usage: u),
    };

    private void EmitCallTarget(EmitContext ctx, ulong targetVa, ulong pc)
    {
        if (_sym.Map.SymbolAt(targetVa) is not null)
        {
            var candidates = _sym.Map.CandidatesAt(targetVa);
            for (var r = 7; r >= 0; r--)
            {
                if (GetRegRaw(r) is not MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { } mu }) continue;
                if (mu.Kind == MetadataUsageKind.MethodRef && (uint)mu.Index < (uint)_sym.Registration.MethodSpecs.Count)
                {
                    var spec = _sym.Registration.MethodSpecs[mu.Index];
                    var sShape = MethodShape.Create(_sym.Metadata, _sym.Registration, _stubs, _mem, _module, spec.MethodDefinitionIndex, mu.Index, forCallSite: true);
                    var expR = (!sShape.IsStatic ? 1 : 0) + sShape.Parameters.Count(p => !IsHfa(p, out _, out _) && !MethodShape.IsFloat(p));
                    if (r == expR && EmitManagedCall(ctx, spec.MethodDefinitionIndex, mu.Index, null)) return;
                }
                else if (mu.Kind == MetadataUsageKind.MethodDef && (uint)mu.Index < (uint)_sym.Metadata.Methods.Length && candidates.Count > 1)
                {
                    var sShape = MethodShape.Create(_sym.Metadata, _sym.Registration, _stubs, _mem, _module, mu.Index, -1, forCallSite: true);
                    var expR = (!sShape.IsStatic ? 1 : 0) + sShape.Parameters.Count(p => !IsHfa(p, out _, out _) && !MethodShape.IsFloat(p));
                    if (r == expR && EmitManagedCall(ctx, mu.Index, -1, null)) return;
                }
            }
            var x0 = GetRegRaw(0);
            var recvTd = x0 switch
            {
                NewObjExpr n => _mem.TypeDefOf(n.Type),
                AddressExpr a => _mem.TypeDefOf(a.Target.Type),
                _ => _mem.TypeDefOf(x0.Type),
            };
            var recvType = x0 switch
            {
                NewObjExpr n => n.Type,
                AddressExpr a => a.Target.Type,
                _ => x0.Type,
            };
            var s = IcfResolver.Choose(candidates, recvTd, _sym.Metadata, _sym.Registration, _sym.Names, ctx.Log,
                receiverIsNewObject: x0 is NewObjExpr { IsMaterialized: false }
                                     || (_method.Name == ".ctor" && x0 is ArgExpr { Parameter.Index: -1 } or AddressExpr { Target: ThisStructExpr }),
                receiverType: recvType, stubs: _stubs, module: _module);
            if (EmitManagedCall(ctx, s.MethodIndex, s.MethodSpecIndex, null)) return;
        }

        // Runtime helpers via anchor map or heuristic argument shape
        if (_helpers is not null)
        {
            var kind = _helpers.KindOf(targetVa);
            if (kind is Runtime.RuntimeHelperKind.WriteBarrier or Runtime.RuntimeHelperKind.MetadataInit or Runtime.RuntimeHelperKind.ClassInit or Runtime.RuntimeHelperKind.ScopeCleanup)
            {
                Elided++;
                var a0 = GetRegRaw(0);
                ClobberVolatiles();
                if (kind == Runtime.RuntimeHelperKind.MetadataInit && a0 is MarkerExpr or StringExpr) _regs[0] = a0;
                return;
            }
            if (kind == Runtime.RuntimeHelperKind.ObjectNew && GetRegRaw(0) is MarkerExpr { Marker: MarkerKind.TypeInfo, Usage: { Kind: MetadataUsageKind.TypeInfo } ou } ouExpr && _stubs.ToSig(_module, ou.Index) is { } ot)
            {
                Elided++;
                ClobberVolatiles();
                var td = ouExpr.Index >= 0 ? _stubs.GetTypeDef(ouExpr.Index) : (_mem.TypeDefOf(ot) is >= 0 and var tdi ? _stubs.GetTypeDef(tdi) : null);
                _regs[0] = new NewObjExpr(ot, td, new RegisterTracker(_body, _f));
                return;
            }
            if (_helpers.ExportNames.TryGetValue(targetVa, out var exp) && BuildArm64Math(exp) is { } mathExpr)
            {
                var res = NewLocal(mathExpr.Type!);
                mathExpr.Emit(ctx);
                ctx.IL.Add(CilOpCodes.Stloc, res);
                ClobberVolatiles();
                _regs[32] = new LocalExpr(res);
                return;
            }
        }
        if (GetRegRaw(0) is AddressExpr { Target: FieldExpr or StaticFieldExpr or ArrayElementExpr } && ClassifyNativeHelper(targetVa) == Arm64HelperKind.WriteBarrier)
        {
            Elided++;
            ClobberVolatiles();
            return;
        }
        var detectedKind = ClassifyNativeHelper(targetVa);
        if (detectedKind == Arm64HelperKind.NoReturn)
        {
            Elided++;
            var ex = GetRegRaw(0);
            if (ex.Kind == ValueKind.Ref && IsExceptionType(ex.Type))
                ex.Emit(ctx);
            else
                ctx.IL.Add(CilOpCodes.Ldnull);
            ClobberVolatiles();
            ctx.IL.Add(CilOpCodes.Throw);
            return;
        }
        if (detectedKind is Arm64HelperKind.WriteBarrier or Arm64HelperKind.ClassInit or Arm64HelperKind.ArrayCheck)
        {
            Elided++;
            var a0k = GetRegRaw(0);
            ClobberVolatiles();
            if (detectedKind == Arm64HelperKind.ClassInit && a0k is MarkerExpr or StringExpr)
                _regs[0] = a0k;
            return;
        }
        if (GetRegRaw(1) is MarkerExpr { Marker: MarkerKind.ElementClass })
        {
            Elided++;
            var v0 = GetRegRaw(0);
            ClobberVolatiles();
            _regs[0] = v0;
            return;
        }
        var symName = _helpers?.ExportNames.GetValueOrDefault(targetVa) ?? _symbolNames?.GetValueOrDefault(targetVa);
        if (symName is not null)
        {
            if (symName is "modf" or "modff")
            {
                Elided++;
                var isSingle = symName == "modff";
                var floatSig = isSingle ? _f.Single : _f.Double;
                var valArg = GetRegRaw(32);
                var valD = isSingle ? new ConvExpr(valArg, _f.Double) : valArg;
                Expr truncD = new MathCallExpr("Truncate", _f.Double, valD);
                Expr truncTyped = isSingle ? new ConvExpr(truncD, _f.Single) : truncD;
                var intPartPtr = GetRegRaw(0);
                var intLocal = NewLocal(floatSig);
                Expr.EmitAs(ctx, truncTyped, floatSig);
                ctx.IL.Add(CilOpCodes.Stloc, intLocal);
                var intLocalExpr = new LocalExpr(intLocal);
                if (intPartPtr is StackAddrPlaceholder modfSp)
                    StoreStackSlot(ctx, modfSp.Key, isSingle ? 4 : 8, intLocalExpr);
                else if (intPartPtr is AddressExpr modfAe)
                    _mem.Store(ctx, modfAe, null, 1, 0, isSingle ? 4 : 8, intLocalExpr);
                var frac = new BinaryExpr(CilOpCodes.Sub, valArg, intLocalExpr, isSingle ? ValueKind.R4 : ValueKind.R8);
                var fracLocal = NewLocal(floatSig);
                Expr.EmitAs(ctx, frac, floatSig);
                ctx.IL.Add(CilOpCodes.Stloc, fracLocal);
                ClobberVolatiles();
                _regs[32] = new LocalExpr(fracLocal);
                return;
            }
            if (BuildArm64Math(symName) is { } symMath)
            {
                Elided++;
                var res = NewLocal(symMath.Type!);
                symMath.Emit(ctx);
                ctx.IL.Add(CilOpCodes.Stloc, res);
                ClobberVolatiles();
                _regs[32] = new LocalExpr(res);
                return;
            }
        }
        if (GetRegRaw(0) is MarkerExpr { Marker: MarkerKind.TypeInfo, Usage: { Kind: MetadataUsageKind.TypeInfo } u } uExpr
            && _stubs.ToSig(_module, u.Index) is { } sig)
        {
            if (sig is SzArrayTypeSignature arrSig && _regs.TryGetValue(1, out var lenExpr) && !lenExpr.IsMarker)
            {
                Elided++;
                Expr.EmitAs(ctx, lenExpr, _f.Int32);
                ctx.IL.Add(CilOpCodes.Newarr, arrSig.BaseType.ToTypeDefOrRef());
                var arrLocal = NewLocal(arrSig);
                ctx.IL.Add(CilOpCodes.Stloc, arrLocal);
                ClobberVolatiles();
                _regs[0] = new LocalExpr(arrLocal);
                return;
            }
            if (sig.IsValueType && _regs.TryGetValue(1, out var boxArg) && boxArg is AddressExpr or StackAddrPlaceholder)
            {
                Elided++;
                var valExpr = boxArg is AddressExpr ae
                    ? ae.Target
                    : LoadStackSlot(((StackAddrPlaceholder)boxArg).Key, Math.Max(1, _mem.ValueSize(sig)), MethodShape.IsFloat(sig));
                Expr.EmitAs(ctx, valExpr, sig);
                ctx.IL.Add(CilOpCodes.Box, sig.ToTypeDefOrRef());
                var boxLocal = NewLocal(_f.Object);
                ctx.IL.Add(CilOpCodes.Stloc, boxLocal);
                ClobberVolatiles();
                _regs[0] = new LocalExpr(boxLocal);
                return;
            }
            if (!sig.IsValueType && sig is not SzArrayTypeSignature and not ArrayTypeSignature)
            {
                Elided++;
                ClobberVolatiles();
                var td = uExpr.Index >= 0 ? _stubs.GetTypeDef(uExpr.Index) : (_mem.TypeDefOf(sig) is >= 0 and var tdi ? _stubs.GetTypeDef(tdi) : null);
                _regs[0] = new NewObjExpr(sig, td, new RegisterTracker(_body, _f));
                return;
            }
            if (sig.IsValueType && !_regs.ContainsKey(1))
            {
                Elided++;
                ClobberVolatiles();
                return;
            }
        }
        if (GetRegRaw(1) is MarkerExpr { Marker: MarkerKind.TypeInfo, Usage: { Kind: MetadataUsageKind.TypeInfo } cu }
            && _stubs.ToSig(_module, cu.Index) is { IsValueType: false } castSig)
        {
            Elided++;
            Expr.EmitAs(ctx, GetRegRaw(0), _f.Object);
            ctx.IL.Add(CilOpCodes.Isinst, castSig.ToTypeDefOrRef());
            var castLocal = NewLocal(castSig);
            ctx.IL.Add(CilOpCodes.Stloc, castLocal);
            ClobberVolatiles();
            _regs[0] = new LocalExpr(castLocal);
            return;
        }
        if (GetRegRaw(0) is MarkerExpr { Marker: MarkerKind.MethodInfo or MarkerKind.Il2CppType or MarkerKind.StaticFields })
        {
            Elided++;
            ClobberVolatiles();
            return;
        }

        Unresolved++;
        var name = _helpers?.ExportNames.GetValueOrDefault(targetVa)
            ?? _symbolNames?.GetValueOrDefault(targetVa)
            ?? $"sub_{targetVa - _sym.Registration.Reader.ImageBase:X}";
        ctx.IL.Add(CilOpCodes.Ldstr, name);
        EmitFourCallNativeArgs(ctx);
        ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.CallNative);
        var tmp = NewLocal(_f.Int64);
        ctx.IL.Add(CilOpCodes.Stloc, tmp);
        ClobberVolatiles();
        _regs[0] = new LocalExpr(tmp);
        _ = pc;
    }

    private Arm64HelperKind ClassifyNativeHelper(ulong va) =>
        ClassifyNativeHelper(_sym.Registration.Reader, _helperCache, va, _symbolNames);

    internal static Arm64HelperKind ClassifyNativeHelper(
        Binary.VirtualReader reader,
        ConcurrentDictionary<ulong, Arm64HelperKind> cache,
        ulong va,
        IReadOnlyDictionary<ulong, string>? symbolNames = null)
    {
        if (cache.TryGetValue(va, out var cached)) return cached;
        var kind = InspectHelper(reader, va, depth: 0, symbolNames);
        cache[va] = kind;
        return kind;
    }

    private static bool IsKnownNoReturnSymbol(string name) =>
        name is "__cxa_throw" or "__cxa_rethrow" or "il2cpp_raise_exception" or "abort" or "_Unwind_Resume"
            or "__stack_chk_fail"
        || name.Contains("il2cpp_codegen_raise_", StringComparison.Ordinal)
        || name.Contains("raise_exception", StringComparison.Ordinal);

    private static bool IsFunctionPrologueAfterCall(in Arm64Instruction ins) =>
        ins.Mnemonic is Arm64Mnemonic.BTI or Arm64Mnemonic.PACIASP
        || (ins.Mnemonic is Arm64Mnemonic.STR or Arm64Mnemonic.STP
            && ins.OperandCount >= 2
            && ins.GetOperand(ins.OperandCount - 1) is { Kind: Arm64OperandKind.Memory } mop
            && ((Arm64MemoryOperand)mop).Accessor is { BaseRegister.Kind: Arm64RegisterKind.SP, IsPreIncrement: true })
        || (ins.Mnemonic == Arm64Mnemonic.SUB && ins.OperandCount >= 3
            && ins.GetOperand(0) is { Kind: Arm64OperandKind.Register } op0 && ((Arm64RegisterOperand)op0).Value.Kind == Arm64RegisterKind.SP
            && ins.GetOperand(1) is { Kind: Arm64OperandKind.Register } op1 && ((Arm64RegisterOperand)op1).Value.Kind == Arm64RegisterKind.SP);

    private static Arm64HelperKind InspectHelper(
        Binary.VirtualReader reader, ulong va, int depth, IReadOnlyDictionary<ulong, string>? symbolNames)
    {
        if (symbolNames is not null && symbolNames.TryGetValue(va, out var directName) && IsKnownNoReturnSymbol(directName))
            return Arm64HelperKind.NoReturn;
        var impl = FollowBThunks(reader, va);
        if (symbolNames is not null && symbolNames.TryGetValue(impl, out var implName))
        {
            if (IsKnownNoReturnSymbol(implName)) return Arm64HelperKind.NoReturn;
            if (implName.Contains("wbarrier", StringComparison.OrdinalIgnoreCase)) return Arm64HelperKind.WriteBarrier;
            if (implName.Contains("runtime_class_init", StringComparison.OrdinalIgnoreCase)) return Arm64HelperKind.ClassInit;
            if (implName == "il2cpp_object_new") return Arm64HelperKind.ObjectNew;
        }
        var scanVa = impl;
        var head = reader.Slice(scanVa, 160);
        if (head.Length >= 8)
        {
            var sawBl = false;
            var sawRet = false;
            var sawBr = false;
            var sawCond = false;
            var sawExternalB = false;
            var scanBytes = 0;
            List<ulong>? blTargets = depth < 3 ? [] : null;
            for (var off = 0; off + 4 <= head.Length; off += 4)
            {
                var raw = BinaryPrimitives.ReadUInt32LittleEndian(head.Slice(off, 4));
                if (raw == 0) break;
                var ins = Arm64Instruction.Decode(raw);
                if (ins.Id == Arm64InstructionId.Invalid) break;
                var m = ins.Mnemonic;
                if (sawBl && IsFunctionPrologueAfterCall(ins))
                    break;
                scanBytes = off + 4;
                if (m is Arm64Mnemonic.RET or Arm64Mnemonic.BR)
                {
                    if (m == Arm64Mnemonic.RET) sawRet = true;
                    else sawBr = true;
                    break;
                }
                if (m == Arm64Mnemonic.B && ins.OperandCount == 1)
                {
                    var bTarget = Arm64Lifter.BranchTarget(scanVa + (ulong)off, ins, 0);
                    if (bTarget < scanVa || bTarget >= scanVa + 160)
                    {
                        if (sawBl && !sawCond && blTargets is not null)
                            blTargets.Add(bTarget);
                        else
                            sawExternalB = true;
                        break;
                    }
                }
                if (m is Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ
                    || (m == Arm64Mnemonic.B && ins.OperandCount == 2))
                {
                    sawCond = true;
                }
                else if (m == Arm64Mnemonic.BL)
                {
                    sawBl = true;
                    if (blTargets is not null)
                    {
                        var target = Arm64Lifter.BranchTarget(scanVa + (ulong)off, ins, 0);
                        if (target != scanVa && target != va) blTargets.Add(target);
                    }
                }
                else if (sawBl && m is Arm64Mnemonic.UDF or Arm64Mnemonic.BRK)
                {
                    return Arm64HelperKind.NoReturn;
                }
            }
            if (sawBl && !sawRet && !sawBr && !sawExternalB)
            {
                if (!sawCond && scanBytes <= 16) return Arm64HelperKind.NoReturn;
                if (blTargets is not null)
                {
                    foreach (var target in blTargets)
                    {
                        if (InspectHelper(reader, target, depth + 1, symbolNames) == Arm64HelperKind.NoReturn)
                            return Arm64HelperKind.NoReturn;
                    }
                }
            }
        }
        if (depth > 0) return Arm64HelperKind.Unknown;

        var body = reader.Slice(impl, 112);
        if (body.Length < 8) return Arm64HelperKind.Unknown;

        var hasAtomicLoad = false;
        var hasAtomicStore = false;
        var hasCall = false;
        var hasBitMask = false;
        var hasMetadataTagDecode = false;
        var ubfxFromArgCount = 0;
        var lsrFromArgCount = 0;
        var checksCctorFlag = false;
        var storesKlassPtr = false;
        var loadsZeroOffset = false;
        var loadsElemClassToX1 = false;
        var cbzCount = 0;
        var hasRetInBody = false;

        for (var off = 0; off + 4 <= body.Length; off += 4)
        {
            var raw = BinaryPrimitives.ReadUInt32LittleEndian(body.Slice(off, 4));
            if (raw == 0) break;
            var ins = Arm64Instruction.Decode(raw);
            if (ins.Id == Arm64InstructionId.Invalid) break;
            switch (ins.Mnemonic)
            {
                case Arm64Mnemonic.LDXR or Arm64Mnemonic.LDAXR: hasAtomicLoad = true; break;
                case Arm64Mnemonic.STXR or Arm64Mnemonic.STLXR: hasAtomicStore = true; break;
                case Arm64Mnemonic.UBFX:
                    hasBitMask = true;
                    if (ins.OperandCount >= 2 && ins.GetOperand(1).Kind == Arm64OperandKind.Register
                        && Arm64Lifter.RegKey((Arm64RegisterOperand)ins.GetOperand(1)) == 0)
                    {
                        ubfxFromArgCount++;
                        if (ins.OperandCount >= 4
                            && ins.GetOperand(2).Kind == Arm64OperandKind.Immediate
                            && ins.GetOperand(3).Kind == Arm64OperandKind.Immediate)
                        {
                            var lsb = ((Arm64ImmediateOperand)ins.GetOperand(2)).Value;
                            var width = ((Arm64ImmediateOperand)ins.GetOperand(3)).Value;
                            if ((lsb == 29 && width == 3) || (lsb == 1 && width == 28))
                                hasMetadataTagDecode = true;
                        }
                    }
                    break;
                case Arm64Mnemonic.LSR:
                    hasBitMask = true;
                    if (ins.OperandCount >= 2 && ins.GetOperand(1).Kind == Arm64OperandKind.Register
                        && Arm64Lifter.RegKey((Arm64RegisterOperand)ins.GetOperand(1)) == 0)
                        lsrFromArgCount++;
                    break;
                case Arm64Mnemonic.ORR: hasBitMask = true; break;
                case Arm64Mnemonic.BL or Arm64Mnemonic.BLR: hasCall = true; break;
                case Arm64Mnemonic.CBZ: cbzCount++; break;
                case Arm64Mnemonic.RET: hasRetInBody = true; break;
                case Arm64Mnemonic.LDR when ins.OperandCount >= 2 && ins.GetOperand(0).Kind == Arm64OperandKind.Register
                    && ins.GetOperand(1).Kind == Arm64OperandKind.Memory:
                    var ldrAcc = ((Arm64MemoryOperand)ins.GetOperand(1)).Accessor;
                    if (ldrAcc.BaseRegister.Kind != Arm64RegisterKind.SP)
                    {
                        if (ldrAcc.Immediate == 0) loadsZeroOffset = true;
                        else if (ldrAcc.Immediate == 64 && Arm64Lifter.RegKey((Arm64RegisterOperand)ins.GetOperand(0)) == 1)
                            loadsElemClassToX1 = true;
                    }
                    break;
                case Arm64Mnemonic.LDRB when ins.OperandCount >= 2 && ins.GetOperand(1).Kind == Arm64OperandKind.Memory
                    && ((Arm64MemoryOperand)ins.GetOperand(1)).Accessor.Immediate == 303:
                    checksCctorFlag = true; break;
                case Arm64Mnemonic.STP when ins.OperandCount >= 3 && ins.GetOperand(2).Kind == Arm64OperandKind.Memory
                    && ((Arm64MemoryOperand)ins.GetOperand(2)).Accessor is { BaseRegister.Index: 0, Immediate: 0 }
                    && ins.GetOperand(1).Kind == Arm64OperandKind.Register && ((Arm64RegisterOperand)ins.GetOperand(1)).IsZeroRegister:
                    storesKlassPtr = true; break;
                case Arm64Mnemonic.STR when ins.OperandCount >= 2 && ins.GetOperand(1).Kind == Arm64OperandKind.Memory
                    && ((Arm64MemoryOperand)ins.GetOperand(1)).Accessor is { BaseRegister.Index: 0, Immediate: 0 }:
                    storesKlassPtr = true; break;
            }
        }

        if (hasMetadataTagDecode)
            return Arm64HelperKind.ClassInit;
        if (hasAtomicLoad && hasAtomicStore && hasBitMask && !hasCall)
            return Arm64HelperKind.WriteBarrier;
        // MSVC Windows ARM64 & Unity 6000 Android ARM64 card-table mark (il2cpp_gc_wbarrier_generic_internal): ubfx/lsr slices on x0.
        if ((ubfxFromArgCount >= 2 || (ubfxFromArgCount >= 1 && lsrFromArgCount >= 1)) && hasBitMask && !checksCctorFlag && !storesKlassPtr)
            return Arm64HelperKind.WriteBarrier;
        if (loadsZeroOffset && loadsElemClassToX1 && cbzCount >= 2 && hasCall && hasRetInBody && !checksCctorFlag && !storesKlassPtr)
            return Arm64HelperKind.ArrayCheck;
        if (checksCctorFlag && hasAtomicLoad)
            return Arm64HelperKind.ClassInit;
        if (storesKlassPtr && hasCall && !checksCctorFlag)
            return Arm64HelperKind.ObjectNew;
        return Arm64HelperKind.Unknown;
    }

    private static ulong FollowBThunks(Binary.VirtualReader reader, ulong va)
    {
        for (var hop = 0; hop < 4; hop++)
        {
            var s = reader.Slice(va, 20);
            if (s.Length < 4) break;
            var followed = false;
            for (var off = 0; off + 4 <= Math.Min(16, s.Length); off += 4)
            {
                var ins = Arm64Instruction.Decode(BinaryPrimitives.ReadUInt32LittleEndian(s.Slice(off, 4)));
                if (ins.Mnemonic is Arm64Mnemonic.MOV or Arm64Mnemonic.BTI or Arm64Mnemonic.HINT or Arm64Mnemonic.NOP) continue;
                if (ins.Mnemonic == Arm64Mnemonic.B && ins.OperandCount == 1 && ins.GetOperand(0).Kind == Arm64OperandKind.Label)
                {
                    va = Arm64Lifter.BranchTarget(va + (ulong)off, ins, 0);
                    followed = true;
                }
                break;
            }
            if (!followed) break;
        }
        return va;
    }

    private Expr? BuildArm64Math(string name)
    {
        if (!Runtime.CrtMath.IsKnown(name)) return null;
        var args = Runtime.CrtMath.ArgCount(name);
        var single = name.EndsWith('f');
        var list = new Expr[args];
        for (var i = 0; i < args; i++) list[i] = single ? new ConvExpr(GetRegRaw(32 + i), _f.Double) : GetRegRaw(32 + i);
        var mname = name.TrimEnd('f') switch
        {
            "sqrt" => "Sqrt", "sin" => "Sin", "cos" => "Cos", "tan" => "Tan",
            "asin" => "Asin", "acos" => "Acos", "atan" => "Atan", "atan2" => "Atan2",
            "pow" => "Pow", "exp" => "Exp", "log" => "Log", "log10" => "Log10",
            "floor" => "Floor", "ceil" => "Ceiling", "fabs" => "Abs", "fmod" => "IEEERemainder",
            var o => char.ToUpperInvariant(o[0]) + o[1..],
        };
        Expr call = new MathCallExpr(mname, _f.Double, list);
        return single ? new ConvExpr(call, _f.Single) : call;
    }

    private bool EmitManagedCall(EmitContext ctx, int methodIndex, int specIndex, Expr? explicitThis)
    {
        if (methodIndex < 0 && specIndex >= 0 && specIndex < _sym.Registration.MethodSpecs.Count)
            methodIndex = _sym.Registration.MethodSpecs[specIndex].MethodDefinitionIndex;
        if ((uint)methodIndex >= (uint)_sym.Metadata.Methods.Length) return false;

        var baseShape = MethodShape.Create(_sym.Metadata, _sym.Registration, _stubs, _mem, _module, methodIndex, specIndex, forCallSite: true);
        // Check exact instantiation MethodInfo* in X[intArgCount]
        var miX = (!baseShape.IsStatic ? 1 : 0) + baseShape.Parameters.Count(p => !IsHfa(p, out _, out _) && !MethodShape.IsFloat(p));
        if (miX < 8 && GetRegRaw(miX) is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { Kind: MetadataUsageKind.MethodRef } mu }
            && (uint)mu.Index < (uint)_sym.Registration.MethodSpecs.Count && _sym.Registration.MethodSpecs[mu.Index].MethodDefinitionIndex == methodIndex)
            specIndex = mu.Index;

        var target = _stubs.MethodRefFor(_module, methodIndex, specIndex);
        if (target is null) return false;
        var shape = MethodShape.Create(_sym.Metadata, _sym.Registration, _stubs, _mem, _module, methodIndex, specIndex, forCallSite: true);

        var xIdx = 0;
        var vIdx = 0;
        Expr? thisVal = null;
        if (!shape.IsStatic)
        {
            var regThis = xIdx < 8 ? GetRegRaw(xIdx++) : new NativeExpr("this");
            thisVal = explicitThis ?? regThis;
        }
        CallEmitter.RebindReceiverGeneric(ref target, ref shape, thisVal, _sym.Metadata, _sym.Registration, _stubs, _mem, _module, methodIndex, specIndex);
        if (thisVal is NewObjExpr { IsMaterialized: false } inlined && _sym.Metadata.GetString(_sym.Metadata.Methods[methodIndex].NameIndex) == ".ctor"
            && DelegateCtorFor(inlined, target) is null
            && inlined.Definition is not null && _stubs.GetMethodDef(methodIndex)?.DeclaringType != inlined.Definition)
        {
            var inlinedLocal = inlined.Materialize(ctx);
            ClobberVolatiles();
            foreach (var k in _regs.Where(e => ReferenceEquals(e.Value, inlined)).Select(e => e.Key).ToList()) _regs[k] = inlinedLocal;
            return true;
        }
        if (thisVal is NewObjExpr { IsMaterialized: false } pending && _sym.Metadata.GetString(_sym.Metadata.Methods[methodIndex].NameIndex) == ".ctor")
        {
            for (var i = 0; i < shape.Parameters.Count; i++) EmitCallParam(ctx, shape.Parameters[i], ref xIdx, ref vIdx);
            ctx.IL.Add(CilOpCodes.Newobj, DelegateCtorFor(pending, target) ?? target);
            ClobberVolatiles();
            var obj = NewLocal(pending.Type);
            ctx.IL.Add(CilOpCodes.Stloc, obj);
            var localExpr = new LocalExpr(obj);
            foreach (var k in _regs.Where(e => ReferenceEquals(e.Value, pending)).Select(e => e.Key).ToList()) _regs[k] = localExpr;
            return true;
        }

        if (thisVal is not null)
        {
            var baseTarget = target is MethodSpecification ms ? ms.Method : target;
            var declSig = baseTarget is MemberReference { Parent: TypeSpecification { Signature: { } ts } }
                ? ts
                : _stubs.TypeRefFor(_module, _sym.Metadata.Methods[methodIndex].DeclaringType)?.ToTypeSignature(shape.DeclaringIsValueType);
            if (shape.DeclaringIsValueType)
            {
                var rawTarget = thisVal is AddressExpr ae ? ae.Target : thisVal;
                var rawType = rawTarget.Type is ByReferenceTypeSignature br ? br.BaseType : rawTarget.Type;
                if (thisVal is StackAddrPlaceholder thisSp && declSig is not null)
                {
                    var spLoc = BindStackStruct(ctx, thisSp.Key, declSig);
                    ctx.IL.Add(CilOpCodes.Ldloca, spLoc);
                }
                else if (declSig is not null && rawType is not null && !SignatureComparer.Default.Equals(rawType, declSig))
                {
                    if (_mem.TypeDefOf(rawType) is >= 0 and var srcTd && _mem.StructField(rawTarget, srcTd, 0) is { } f0
                        && f0.Type is not null && SignatureComparer.Default.Equals(f0.Type, declSig))
                    {
                        new AddressExpr(f0).Emit(ctx);
                    }
                    else if (thisVal is not MarkerExpr)
                    {
                        Expr.EmitAs(ctx, rawTarget, declSig);
                        var tmp = NewLocal(declSig);
                        ctx.IL.Add(CilOpCodes.Stloc, tmp);
                        ctx.IL.Add(CilOpCodes.Ldloca, tmp);
                    }
                    else thisVal.Emit(ctx);
                }
                else if (thisVal is AddressExpr || thisVal.Type is ByReferenceTypeSignature) thisVal.Emit(ctx);
                else if (thisVal is LocalExpr or ArgExpr or FieldExpr or StaticFieldExpr or ArrayElementExpr) new AddressExpr(thisVal).Emit(ctx);
                else if (declSig is not null && thisVal is not MarkerExpr)
                {
                    Expr.EmitAs(ctx, thisVal, declSig);
                    var tmp = NewLocal(declSig);
                    ctx.IL.Add(CilOpCodes.Stloc, tmp);
                    ctx.IL.Add(CilOpCodes.Ldloca, tmp);
                }
                else new AddressExpr(thisVal).Emit(ctx);
            }
            else if (thisVal is ConstExpr { Value: 0 }) ctx.IL.Add(CilOpCodes.Ldnull);
            else if (declSig is not null)
            {
                Expr.EmitAs(ctx, thisVal, declSig);
                if (thisVal.Kind == ValueKind.Ref && Expr.KindOf(declSig) == ValueKind.Ref && thisVal.Type is not null)
                {
                    var declTd = _mem.TypeDefOf(declSig);
                    var actualTd = _mem.TypeDefOf(thisVal.Type);
                    if (declTd >= 0 && actualTd >= 0 && declTd != actualTd && !_mem.IsStrictDerivedFrom(actualTd, declTd)
                        && (_mem.IsStrictDerivedFrom(declTd, actualTd)
                            || ((uint)declTd < (uint)_sym.Metadata.TypeDefinitions.Length && !_sym.Metadata.TypeDefinitions[declTd].IsInterface)))
                        ctx.IL.Add(CilOpCodes.Castclass, declSig.ToTypeDefOrRef());
                }
            }
            else thisVal.Emit(ctx);
        }
        var x8BeforeCall = _regs.GetValueOrDefault(8);
        for (var i = 0; i < shape.Parameters.Count; i++) EmitCallParam(ctx, shape.Parameters[i], ref xIdx, ref vIdx);

        var curTd = (uint)_shape.MethodIndex < (uint)_sym.Metadata.Methods.Length
            ? _sym.Metadata.Methods[_shape.MethodIndex].DeclaringType : -1;
        var declTdCall = _sym.Metadata.Methods[methodIndex].DeclaringType;
        var isBaseCall = explicitThis is null
            && thisVal is ArgExpr { Parameter.Index: -1 }
            && !shape.IsStatic && !shape.DeclaringIsValueType
            && curTd >= 0 && declTdCall != curTd
            && (_sym.Metadata.Methods[methodIndex].Flags & 0x0040) != 0
            && _mem.IsStrictDerivedFrom(curTd, declTdCall);
        ctx.IL.Add(!shape.IsStatic && !shape.DeclaringIsValueType && !isBaseCall ? CilOpCodes.Callvirt : CilOpCodes.Call, target);
        ClobberVolatiles();

        var rk = Expr.KindOf(shape.ReturnType);
        if (rk == ValueKind.Void) return true;
        var res = NewLocal(shape.ReturnType);
        ctx.IL.Add(CilOpCodes.Stloc, res);
        var resExpr = new LocalExpr(res);
        if (IsHfa(shape.ReturnType, out var rCount, out _))
        {
            var td = _mem.TypeDefOf(shape.ReturnType);
            for (var c = 0; c < rCount; c++)
                _regs[32 + c] = (td >= 0 ? _mem.StructField(resExpr, td, c * 4) : null) ?? new StructChunkExpr(resExpr, c * 4, 4);
            return true;
        }
        if (rk == ValueKind.Struct && !IsHfa(shape.ReturnType, out _, out _))
        {
            var vsize = _mem.ValueSize(shape.ReturnType);
            if (vsize > 16)
            {
                if (x8BeforeCall is StackAddrPlaceholder retSp)
                {
                    ClearSlotRange(retSp.Key, vsize);
                    _stackStructs[retSp.Key] = (res, shape.ReturnType, vsize);
                }
                _regs[0] = new AddressExpr(resExpr);
                return true;
            }
            if (vsize is > 8 and <= 16 && _mem.TypeDefOf(shape.ReturnType) is >= 0 and var sTd)
            {
                _regs[0] = _mem.StructField(resExpr, sTd, 0) ?? resExpr;
                if (_mem.StructField(resExpr, sTd, 8) is { } f8) _regs[1] = f8;
                return true;
            }
        }
        _regs[MethodShape.IsFloat(shape.ReturnType) ? 32 : 0] = resExpr;
        return true;
    }

    private void EmitCallParam(EmitContext ctx, TypeSignature pt, ref int xIdx, ref int vIdx)
    {
        if (pt is ByReferenceTypeSignature br)
        {
            var e = xIdx < 8 ? GetRegRaw(xIdx++) : new NativeExpr("byref");
            if (e is StackAddrPlaceholder sp)
            {
                var loc = BindStackStruct(ctx, sp.Key, br.BaseType);
                ctx.IL.Add(CilOpCodes.Ldloca, loc);
                return;
            }
            if (e is LocalExpr or ArgExpr or FieldExpr or StaticFieldExpr or ArrayElementExpr)
                new AddressExpr(e).Emit(ctx);
            else
                e.Emit(ctx);
            return;
        }
        if (IsHfa(pt, out var count, out var isD) && vIdx + count <= 8)
        {
            var lanes = new Expr[count];
            for (var c = 0; c < count; c++) lanes[c] = GetRegRaw(32 + vIdx++);
            EmitStructFromLanes(ctx, pt, lanes, isD ? 8 : 4);
            return;
        }
        if (MethodShape.IsFloat(pt))
        {
            var e = vIdx < 8 ? GetRegRaw(32 + vIdx++) : new FloatExpr(0, true);
            Expr.EmitAs(ctx, e, pt);
            return;
        }
        var x = xIdx < 8 ? GetRegRaw(xIdx++) : new ConstExpr(0, ValueKind.I8);
        if (Expr.KindOf(pt) == ValueKind.Native && x is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { } u } && FunctionPointer(u) is { } fn)
        {
            ctx.IL.Add(CilOpCodes.Ldftn, fn);
            return;
        }
        if (pt.FullName == "System.RuntimeTypeHandle" && x is MarkerExpr { Marker: MarkerKind.Il2CppType, Usage: { Kind: MetadataUsageKind.Il2CppType } tu }
            && _stubs.ToSig(_module, tu.Index) is { } ts)
        {
            ctx.IL.Add(CilOpCodes.Ldtoken, ts.ToTypeDefOrRef());
            return;
        }
        if (Expr.KindOf(pt) == ValueKind.Struct)
        {
            if (x is StackAddrPlaceholder valSp)
            {
                if (TryFindStackStruct(valSp.Key, out var ss, out var entry) && ss == valSp.Key && SignatureComparer.Default.Equals(entry.Type, pt))
                {
                    ctx.IL.Add(CilOpCodes.Ldloc, entry.Local);
                    return;
                }
                if (_slots.TryGetValue(valSp.Key, out var s0))
                {
                    if (s0 is StructChunkExpr { Offset: 0 } sc && sc.Source.Type is not null && SignatureComparer.Default.Equals(sc.Source.Type, pt))
                    {
                        sc.Source.Emit(ctx);
                        return;
                    }
                    if (s0.Type is not null && SignatureComparer.Default.Equals(s0.Type, pt))
                    {
                        s0.Emit(ctx);
                        return;
                    }
                }
                var tmpLoc = BindStackStruct(ctx, valSp.Key, pt);
                ctx.IL.Add(CilOpCodes.Ldloc, tmpLoc);
                return;
            }
            if (x is AddressExpr ae)
            {
                Expr.EmitAs(ctx, ae.Target, pt);
                return;
            }
        }
        Expr.EmitAs(ctx, x, pt);
        if (x.Kind == ValueKind.Ref && Expr.KindOf(pt) == ValueKind.Ref
            && x.Type is not null && _mem.IsStrictDerivedFrom(_mem.TypeDefOf(pt), _mem.TypeDefOf(x.Type)))
            ctx.IL.Add(CilOpCodes.Castclass, pt.ToTypeDefOrRef());
    }

    private void EmitStructFromLanes(EmitContext ctx, TypeSignature type, Expr[] lanes, int elemSize)
    {
        // Fast path: all lanes come from consecutive fields of the same struct local/arg
        Expr? wholeSource = null;
        var allMatch = true;
        for (var i = 0; i < lanes.Length; i++)
        {
            var src = lanes[i] switch
            {
                FieldExpr fe when i == 0 => fe.Instance,
                FieldExpr fe => fe.Instance,
                StructChunkExpr sc when sc.Offset == i * elemSize => sc.Source,
                _ => null,
            };
            if (src is null || src.Type is null || !SignatureComparer.Default.Equals(src.Type, type)) { allMatch = false; break; }
            if (wholeSource is null) wholeSource = src;
            else if (!SameValue(wholeSource, src)) { allMatch = false; break; }
        }
        if (allMatch && wholeSource is not null) { wholeSource.Emit(ctx); return; }

        var local = NewLocal(type);
        var target = new LocalExpr(local);
        var td = _mem.TypeDefOf(type);
        if (td >= 0)
        {
            for (var i = 0; i < lanes.Length; i++)
            {
                if (_mem.StructField(target, td, i * elemSize) is not FieldExpr fe) continue;
                new AddressExpr(fe.Instance).Emit(ctx);
                Expr.EmitAs(ctx, lanes[i], fe.Type!);
                ctx.IL.Add(CilOpCodes.Stfld, fe.Field);
            }
        }
        ctx.IL.Add(CilOpCodes.Ldloc, local);
    }

    private void EmitReturn(EmitContext ctx)
    {
        var ret = _shape.ReturnType;
        if (ret is ByReferenceTypeSignature brRet)
        {
            var rv = GetRegRaw(0);
            if (rv is AddressExpr || rv.Type is ByReferenceTypeSignature) rv.Emit(ctx);
            else if (rv is LocalExpr or ArgExpr or FieldExpr or StaticFieldExpr or ArrayElementExpr) new AddressExpr(rv).Emit(ctx);
            else new AddressExpr(new LocalExpr(NewLocal(brRet.BaseType))).Emit(ctx);
            ctx.IL.Add(CilOpCodes.Ret);
            return;
        }
        var k = Expr.KindOf(ret);
        if (k == ValueKind.Void) { ctx.IL.Add(CilOpCodes.Ret); return; }
        if (IsHfa(ret, out var count, out var isD))
        {
            var lanes = Enumerable.Range(0, count).Select(i => GetRegRaw(32 + i)).ToArray();
            EmitStructFromLanes(ctx, ret, lanes, isD ? 8 : 4);
            ctx.IL.Add(CilOpCodes.Ret);
            return;
        }
        if (_retBuf is not null && !IsHfa(ret, out _, out _) && _shape.HasReturnBuffer)
        {
            ctx.IL.Add(CilOpCodes.Ldloc, _retBuf);
            ctx.IL.Add(CilOpCodes.Ret);
            return;
        }
        var retVal = GetRegRaw(MethodShape.IsFloat(ret) ? 32 : 0);
        if (retVal is AddressExpr { Target: ThisStructExpr } && k is not ValueKind.Struct and not ValueKind.Native)
            retVal = new NativeExpr("this_addr", k);
        Expr.EmitAs(ctx, retVal, ret);
        ctx.IL.Add(CilOpCodes.Ret);
    }

    public void EmitDefaultReturn(EmitContext ctx)
    {
        var k = Expr.KindOf(_shape.ReturnType);
        switch (k)
        {
            case ValueKind.Void: break;
            case ValueKind.I4: ctx.IL.Add(CilOpCodes.Ldc_I4_0); break;
            case ValueKind.I8: ctx.IL.Add(CilOpCodes.Ldc_I8, 0L); break;
            case ValueKind.R4: ctx.IL.Add(CilOpCodes.Ldc_R4, 0f); break;
            case ValueKind.R8: ctx.IL.Add(CilOpCodes.Ldc_R8, 0d); break;
            case ValueKind.Ref: ctx.IL.Add(CilOpCodes.Ldnull); break;
            default: ctx.IL.Add(CilOpCodes.Ldloc, NewLocal(_shape.ReturnType)); break;
        }
        ctx.IL.Add(CilOpCodes.Ret);
    }

    private void EmitFourCallNativeArgs(EmitContext ctx)
    {
        for (var i = 0; i < 4; i++)
        {
            if (_regs.TryGetValue(i, out var e) && !e.IsMarker) Expr.EmitAs(ctx, e, _f.Int64);
            else ctx.IL.Add(CilOpCodes.Ldc_I8, 0L);
        }
    }

    private void ClobberVolatiles()
    {
        for (var r = 0; r <= 18; r++) _regs.Remove(r);
        for (var r = 32; r <= 39; r++) _regs.Remove(r);
        for (var r = 48; r <= 63; r++) _regs.Remove(r);
    }

    private bool IsExceptionType(TypeSignature? type)
    {
        if (type is null) return false;
        var current = _mem.TypeDefOf(type);
        var seen = new HashSet<int>();
        while ((uint)current < (uint)_sym.Metadata.TypeDefinitions.Length && seen.Add(current))
        {
            if (_stubs.GetTypeDef(current) is { FullName: "System.Exception" }) return true;
            var def = _sym.Metadata.TypeDefinitions[current];
            if (def.ParentIndex < 0 || !_sym.Registration.TryGetType(def.ParentIndex, out var parent)) break;
            current = _sym.Names.ResolveTypeDefIndex(parent);
        }
        return false;
    }

    private IMethodDescriptor? DelegateCtorFor(NewObjExpr obj, IMethodDescriptor resolved)
    {
        if (resolved.Signature is not MethodSignature { ParameterTypes.Count: 2 } sig
            || sig.ParameterTypes[0].ElementType != AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Object
            || sig.ParameterTypes[1].ElementType != AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I) return null;
        if (resolved.DeclaringType is { } dt && dt.FullName == obj.Type.FullName) return null;
        return new MemberReference(obj.Type.ToTypeDefOrRef(), ".ctor", sig);
    }

    private IMethodDescriptor? FunctionPointer(MetadataUsage u) => u.Kind switch
    {
        MetadataUsageKind.MethodDef => _stubs.MethodRefFor(_module, u.Index),
        MetadataUsageKind.MethodRef when (uint)u.Index < (uint)_sym.Registration.MethodSpecs.Count =>
            _stubs.MethodRefFor(_module, _sym.Registration.MethodSpecs[u.Index].MethodDefinitionIndex, u.Index),
        _ => null,
    };

    private bool IsHfa(TypeSignature type, out int count, out bool isDouble)
    {
        count = 0; isDouble = false;
        if (type is ByReferenceTypeSignature || Expr.KindOf(type) != ValueKind.Struct) return false;
        var td = _mem.TypeDefOf(type);
        if (td < 0) return false;
        var fields = _mem.FieldsOf(td).Where(f => !f.IsStatic).OrderBy(f => f.Offset).ToList();
        if (fields.Count is 0 or > 4) return false;
        var firstKind = Expr.KindOf(_stubs.ToSig(_module, fields[0].TypeIndex));
        if (firstKind is not (ValueKind.R4 or ValueKind.R8)) return false;
        foreach (var f in fields)
            if (Expr.KindOf(_stubs.ToSig(_module, f.TypeIndex)) != firstKind) return false;
        count = fields.Count;
        isDouble = firstKind == ValueKind.R8;
        return true;
    }

    private CompareExpr? BuildFlagCompare(Arm64ConditionalKind c)
    {
        if (_flagKind == FlagKind.None || _flagLeft is null) return null;
        var right = _flagRight ?? ZeroLike(_flagLeft);
        if (_flagKind == FlagKind.Test)
        {
            Expr tested = _flagLeft;
            if (!(_flagRight is ConstExpr { Value: -1 } || SameValue(_flagLeft, right)))
                tested = new BinaryExpr(CilOpCodes.And, _flagLeft, right, _flagLeft.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4);
            return c switch
            {
                Arm64ConditionalKind.EQ => new CompareExpr(Condition.Eq, tested, ZeroLike(tested)),
                Arm64ConditionalKind.NE => new CompareExpr(Condition.Ne, tested, ZeroLike(tested)),
                _ => null,
            };
        }
        if (_flagKind == FlagKind.FloatCompare)
        {
            return c switch
            {
                Arm64ConditionalKind.EQ => new CompareExpr(Condition.Eq, _flagLeft, right),
                Arm64ConditionalKind.NE => new CompareExpr(Condition.Ne, _flagLeft, right),
                Arm64ConditionalKind.GT or Arm64ConditionalKind.HI => new CompareExpr(Condition.Gt, _flagLeft, right),
                Arm64ConditionalKind.GE or Arm64ConditionalKind.HS or Arm64ConditionalKind.PL => new CompareExpr(Condition.Ge, _flagLeft, right),
                Arm64ConditionalKind.LT or Arm64ConditionalKind.LO or Arm64ConditionalKind.MI => new CompareExpr(Condition.Lt, _flagLeft, right),
                Arm64ConditionalKind.LE or Arm64ConditionalKind.LS => new CompareExpr(Condition.Le, _flagLeft, right),
                _ => null,
            };
        }
        return c switch
        {
            Arm64ConditionalKind.EQ => SimplifyCompare(Condition.Eq, _flagLeft, right),
            Arm64ConditionalKind.NE => SimplifyCompare(Condition.Ne, _flagLeft, right),
            Arm64ConditionalKind.LT or Arm64ConditionalKind.MI => new CompareExpr(Condition.Lt, _flagLeft, right),
            Arm64ConditionalKind.LE => new CompareExpr(Condition.Le, _flagLeft, right),
            Arm64ConditionalKind.GT => new CompareExpr(Condition.Gt, _flagLeft, right),
            Arm64ConditionalKind.GE or Arm64ConditionalKind.PL => new CompareExpr(Condition.Ge, _flagLeft, right),
            Arm64ConditionalKind.LO => new CompareExpr(Condition.LtUn, _flagLeft, right),
            Arm64ConditionalKind.LS => new CompareExpr(Condition.LeUn, _flagLeft, right),
            Arm64ConditionalKind.HI => new CompareExpr(Condition.GtUn, _flagLeft, right),
            Arm64ConditionalKind.HS => new CompareExpr(Condition.GeUn, _flagLeft, right),
            _ => null,
        };
    }

    private void SetCompareResult(Expr res) { _flagKind = FlagKind.Compare; _flagLeft = res; _flagRight = ZeroLike(res); }

    private static CompareExpr SimplifyCompare(Condition cond, Expr left, Expr right)
    {
        if (cond is Condition.Eq or Condition.Ne && right is ConstExpr { Value: 0 })
        {
            if (left is BinaryExpr { Op.Code: CilCode.Sub, Left: var x, Right: ConstExpr c })
                return new CompareExpr(cond, x, c);
            if (left is BinaryExpr { Op.Code: CilCode.Add, Left: var x2, Right: ConstExpr c2 })
                return new CompareExpr(cond, x2, new ConstExpr(-c2.Value, c2.Kind));
        }
        return new CompareExpr(cond, left, right);
    }

    private static bool IsBool01(Expr e) =>
        e.Type?.ElementType == AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Boolean
        || e is CompareExpr
        || (e is BinaryExpr { Op.Code: CilCode.And } band && (band.Right is ConstExpr { Value: 1 } || band.Left is ConstExpr { Value: 1 }))
        || (e is BinaryExpr { Op.Code: CilCode.And or CilCode.Or or CilCode.Xor } bb && IsBool01(bb.Left) && IsBool01(bb.Right))
        || (e is ConvExpr c && IsBool01(c.Operand));

    private static Expr FoldBoolBitNot(Expr e, out bool isExactBool)
    {
        if (IsBool01(e)) { isExactBool = true; return e; }
        if (e is UnaryExpr { Op.Code: CilCode.Not, Operand: var inner })
        {
            var innerFolded = FoldBoolBitNot(inner, out var ib);
            if (ib)
            {
                isExactBool = true;
                return new CompareExpr(Condition.Eq, innerFolded, new ConstExpr(0, ValueKind.I4));
            }
        }
        if (e is BinaryExpr { Op.Code: CilCode.And or CilCode.Or or CilCode.Xor } b)
        {
            var fl = FoldBoolBitNot(b.Left, out var lb);
            var fr = FoldBoolBitNot(b.Right, out var rb);
            if (lb && rb)
            {
                isExactBool = true;
                return new BinaryExpr(b.Op, fl, fr, ValueKind.I4);
            }
        }
        isExactBool = false;
        return e;
    }

    private static Expr BitTestCompare(Expr val, int bit, bool zero)
    {
        if (bit == 0)
        {
            var folded = FoldBoolBitNot(val, out var isExactBool);
            if (isExactBool)
                return new CompareExpr(zero ? Condition.Eq : Condition.Ne, folded, new ConstExpr(0, ValueKind.I4));
        }
        var k = val.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4;
        var masked = new BinaryExpr(CilOpCodes.And, val, new ConstExpr(1L << bit, k), k);
        return new CompareExpr(zero ? Condition.Eq : Condition.Ne, masked, new ConstExpr(0, k));
    }

    private void SpillLiveOut(EmitContext ctx, Arm64Block block, int branchReadReg = -1)
    {
        var exit = _exit[block] = [];
        var liveOutList = block.LiveOut.ToList();
        for (var idx = 0; idx < liveOutList.Count; idx++)
        {
            var r = liveOutList[idx];
            if (!_regs.TryGetValue(r, out var e)) continue;
            exit[r] = e;
            if (e is MarkerExpr or ArgExpr) _lastRemat[r] = e; else _lastRemat.Remove(r);
            if (!RegisterTracker.IsStorable(e))
            {
                if (e is AddressExpr { Target: FieldExpr { Type: { } fType and not ByReferenceTypeSignature } }
                    && block.Successors.Any(s => s.Predecessors.Count > 1 && s.LiveIn.Contains(r)))
                {
                    var byRefType = fType.MakeByReferenceType();
                    var byRefLocal = block.Successors.Select(s => _entryLocals.GetValueOrDefault(s)?.GetValueOrDefault(r)).FirstOrDefault(l => l is not null && SameSpillFamily(l.VariableType, byRefType, e))
                                     ?? block.Successors.Where(s => s.Predecessors.Count > 1)
                                         .Select(s => _joinLocals.GetValueOrDefault((s, r)))
                                         .FirstOrDefault(l => l is not null && SameSpillFamily(l.VariableType, byRefType, e))
                                     ?? SpillLocalFor(r, byRefType);
                    foreach (var s in block.Successors)
                        if (s.Predecessors.Count > 1 && s.LiveIn.Contains(r))
                            _joinLocals.TryAdd((s, r), byRefLocal);
                    e.Emit(ctx);
                    ctx.IL.Add(CilOpCodes.Stloc, byRefLocal);
                    exit[r] = _regs[r] = new LocalExpr(byRefLocal);
                }
                continue;
            }
            var eType = TypeFor(e);
            var local = block.Successors.Select(s => _entryLocals.GetValueOrDefault(s)?.GetValueOrDefault(r)).FirstOrDefault(l => l is not null)
                        ?? block.Successors.Where(s => s.Predecessors.Count > 1)
                            .Select(s => _joinLocals.GetValueOrDefault((s, r)))
                            .FirstOrDefault(l => l is not null && SameSpillFamily(l.VariableType, eType, e))
                        ?? SpillLocalFor(r, eType);
            foreach (var s in block.Successors)
                if (s.Predecessors.Count > 1 && s.LiveIn.Contains(r))
                    _joinLocals.TryAdd((s, r), local);
            if (e is LocalExpr le && le.Local == local) continue;

            // Preserve `local` if any flag operand, branch register, or later live-out still reads it before we overwrite `local`.
            var clobbersLiveLocal = (_flagLeft is not null && ContainsLocal(_flagLeft, local))
                || (_flagRight is not null && ContainsLocal(_flagRight, local))
                || (branchReadReg >= 0 && branchReadReg != r && _regs.TryGetValue(branchReadReg, out var brExpr) && ContainsLocal(brExpr, local))
                || Enumerable.Range(idx + 1, liveOutList.Count - idx - 1)
                    .Any(j => _regs.TryGetValue(liveOutList[j], out var later) && ContainsLocal(later, local))
                || _slots.Values.Any(sv => ContainsLocal(sv, local));
            if (clobbersLiveLocal)
            {
                var snap = NewLocal(local.VariableType);
                ctx.IL.Add(CilOpCodes.Ldloc, local);
                ctx.IL.Add(CilOpCodes.Stloc, snap);
                var snapExpr = new LocalExpr(snap);
                if (_flagLeft is not null) _flagLeft = RewriteLocal(_flagLeft, local, snapExpr);
                if (_flagRight is not null) _flagRight = RewriteLocal(_flagRight, local, snapExpr);
                if (branchReadReg >= 0 && branchReadReg != r && _regs.TryGetValue(branchReadReg, out var brVal))
                    _regs[branchReadReg] = RewriteLocal(brVal, local, snapExpr);
                for (var j = idx + 1; j < liveOutList.Count; j++)
                {
                    var lr = liveOutList[j];
                    if (_regs.TryGetValue(lr, out var laterVal))
                        _regs[lr] = RewriteLocal(laterVal, local, snapExpr);
                }
                foreach (var sk in _slots.Keys.ToList())
                    _slots[sk] = RewriteLocal(_slots[sk], local, snapExpr);
            }

            Expr.EmitAs(ctx, e, local.VariableType);
            ctx.IL.Add(CilOpCodes.Stloc, local);
            if (!IsRematerializable(e))
            {
                var spilled = new LocalExpr(local);
                if (_gotValues.Contains(e)) _gotValues.Add(spilled);
                exit[r] = _regs[r] = spilled;
                if (_flagLeft is not null) _flagLeft = RewriteExact(_flagLeft, e, spilled);
                if (_flagRight is not null) _flagRight = RewriteExact(_flagRight, e, spilled);
            }
        }
    }

    private ulong[] ComputeLiveAfterMap(List<Arm64Insn> insns, Arm64Block b)
    {
        var count = b.End - b.Start;
        var map = new ulong[count];
        ulong live = 0;
        foreach (var r in b.LiveOut)
            if ((uint)r < 64) live |= 1UL << r;
        var defs = new List<int>(2);
        var reads = new List<int>(4);
        for (var i = b.End - 1; i >= b.Start; i--)
        {
            map[i - b.Start] = live;
            var ins = insns[i].Ins;
            defs.Clear();
            reads.Clear();
            Arm64Lifter.ClassifyOperands(ins, defs, reads);
            if (ins.Mnemonic is Arm64Mnemonic.BL or Arm64Mnemonic.BLR)
            {
                for (var r = 0; r <= 18; r++) live &= ~(1UL << r);
                for (var r = 32; r <= 39; r++) live &= ~(1UL << r);
                for (var r = 48; r <= 63; r++) live &= ~(1UL << r);
                for (var r = 0; r <= 8; r++) live |= 1UL << r;
                for (var r = 32; r <= 39; r++) live |= 1UL << r;
            }
            else
            {
                foreach (var d in defs) if ((uint)d < 64) live &= ~(1UL << d);
                foreach (var r in reads) if ((uint)r < 64) live |= 1UL << r;
            }
        }
        return map;
    }

    private bool IsRegLiveAfterCurInsn(int r)
    {
        if (_curBlockLiveAfter is null || (uint)r >= 64) return false;
        var idx = _curInsnIdx - _curBlockStart;
        if ((uint)idx >= (uint)_curBlockLiveAfter.Length) return true;
        return (_curBlockLiveAfter[idx] & (1UL << r)) != 0;
    }

    private static bool SameFieldRef(Expr a, Expr b) => (a, b) switch
    {
        (FieldExpr fa, FieldExpr fb) => fa.Field.Name == fb.Field.Name
            && SignatureComparer.Default.Equals(fa.Field.DeclaringType, fb.Field.DeclaringType)
            && SameValue(fa.Instance, fb.Instance),
        (StaticFieldExpr sa, StaticFieldExpr sb) => sa.Field.Name == sb.Field.Name
            && SignatureComparer.Default.Equals(sa.Field.DeclaringType, sb.Field.DeclaringType),
        _ => false,
    };

    private static bool ContainsReadOf(Expr root, Expr fieldExpr, Expr? excludeExact)
    {
        if (excludeExact is not null && ReferenceEquals(root, excludeExact)) return false;
        if (root is AddressExpr) return false;
        if (SameFieldRef(root, fieldExpr)) return true;
        return root switch
        {
            FieldExpr fe => ContainsReadOf(fe.Instance, fieldExpr, excludeExact),
            BinaryExpr be => ContainsReadOf(be.Left, fieldExpr, excludeExact) || ContainsReadOf(be.Right, fieldExpr, excludeExact),
            UnaryExpr ue => ContainsReadOf(ue.Operand, fieldExpr, excludeExact),
            ConvExpr ce => ContainsReadOf(ce.Operand, fieldExpr, excludeExact),
            CompareExpr cmp => ContainsReadOf(cmp.Left, fieldExpr, excludeExact) || ContainsReadOf(cmp.Right, fieldExpr, excludeExact),
            CondSelectExpr cs => ContainsReadOf(cs.Cond, fieldExpr, excludeExact) || ContainsReadOf(cs.TrueVal, fieldExpr, excludeExact) || ContainsReadOf(cs.FalseVal, fieldExpr, excludeExact),
            MathCallExpr mc => mc.Args.Any(a => ContainsReadOf(a, fieldExpr, excludeExact)),
            ArrayElementExpr ae => ContainsReadOf(ae.Array, fieldExpr, excludeExact) || ContainsReadOf(ae.Index, fieldExpr, excludeExact),
            ArrayLengthExpr al => ContainsReadOf(al.Array, fieldExpr, excludeExact),
            StringLengthExpr sl => ContainsReadOf(sl.String, fieldExpr, excludeExact),
            IndirectExpr ie => ContainsReadOf(ie.Pointer, fieldExpr, excludeExact),
            IndirectStructExpr ise => ContainsReadOf(ise.Address, fieldExpr, excludeExact),
            StructChunkExpr sc => ContainsReadOf(sc.Source, fieldExpr, excludeExact),
            _ => false,
        };
    }

    private static Expr RewriteReadOf(Expr root, Expr fieldExpr, Expr? excludeExact, Expr replacement)
    {
        if (excludeExact is not null && ReferenceEquals(root, excludeExact)) return root;
        if (root is AddressExpr) return root;
        if (SameFieldRef(root, fieldExpr)) return replacement;
        return RewriteChildren(root, child => RewriteReadOf(child, fieldExpr, excludeExact, replacement));
    }

    private static Expr RewritePostFieldStore(Expr root, Expr? storedVal, Expr fieldExpr, Expr? inverseExpr)
    {
        if (storedVal is not null && ReferenceEquals(root, storedVal))
            return fieldExpr;
        if (inverseExpr is not null && root is not AddressExpr && SameFieldRef(root, fieldExpr))
            return inverseExpr;
        if (root is AddressExpr) return root;
        return RewriteChildren(root, child => RewritePostFieldStore(child, storedVal, fieldExpr, inverseExpr));
    }

    private static bool ContainsLocal(Expr root, CilLocalVariable local) => root switch
    {
        LocalExpr le => le.Local == local,
        AddressExpr ae => ContainsLocal(ae.Target, local),
        FieldExpr fe => ContainsLocal(fe.Instance, local),
        BinaryExpr be => ContainsLocal(be.Left, local) || ContainsLocal(be.Right, local),
        UnaryExpr ue => ContainsLocal(ue.Operand, local),
        ConvExpr ce => ContainsLocal(ce.Operand, local),
        CompareExpr cmp => ContainsLocal(cmp.Left, local) || ContainsLocal(cmp.Right, local),
        CondSelectExpr cs => ContainsLocal(cs.Cond, local) || ContainsLocal(cs.TrueVal, local) || ContainsLocal(cs.FalseVal, local),
        MathCallExpr mc => mc.Args.Any(a => ContainsLocal(a, local)),
        ArrayElementExpr ae => ContainsLocal(ae.Array, local) || ContainsLocal(ae.Index, local),
        ArrayLengthExpr al => ContainsLocal(al.Array, local),
        StringLengthExpr sl => ContainsLocal(sl.String, local),
        IndirectExpr ie => ContainsLocal(ie.Pointer, local),
        IndirectStructExpr ise => ContainsLocal(ise.Address, local),
        StructChunkExpr sc => ContainsLocal(sc.Source, local),
        _ => false,
    };

    private static Expr RewriteLocal(Expr root, CilLocalVariable local, Expr replacement)
    {
        if (root is LocalExpr le && le.Local == local) return replacement;
        if (root is AddressExpr) return root;
        return RewriteChildren(root, child => RewriteLocal(child, local, replacement));
    }

    private static Expr RewriteExact(Expr root, Expr oldExpr, Expr newExpr)
    {
        if (ReferenceEquals(root, oldExpr)) return newExpr;
        if (root is AddressExpr) return root;
        return RewriteChildren(root, child => RewriteExact(child, oldExpr, newExpr));
    }

    private static Expr RewriteChildren(Expr root, Func<Expr, Expr> fn)
    {
        switch (root)
        {
            case FieldExpr fe:
            {
                var ni = fn(fe.Instance);
                return ReferenceEquals(ni, fe.Instance) ? fe : new FieldExpr(ni, fe.Field, fe.Type);
            }
            case BinaryExpr be:
            {
                var nl = fn(be.Left);
                var nr = fn(be.Right);
                return ReferenceEquals(nl, be.Left) && ReferenceEquals(nr, be.Right) ? be : new BinaryExpr(be.Op, nl, nr, be.Kind);
            }
            case UnaryExpr ue:
            {
                var no = fn(ue.Operand);
                return ReferenceEquals(no, ue.Operand) ? ue : new UnaryExpr(ue.Op, no);
            }
            case ConvExpr ce:
            {
                var no = fn(ce.Operand);
                return ReferenceEquals(no, ce.Operand) ? ce : new ConvExpr(no, ce.Type);
            }
            case CompareExpr cmp:
            {
                var nl = fn(cmp.Left);
                var nr = fn(cmp.Right);
                return ReferenceEquals(nl, cmp.Left) && ReferenceEquals(nr, cmp.Right) ? cmp : new CompareExpr(cmp.Condition, nl, nr);
            }
            case CondSelectExpr cs:
            {
                var nc = fn(cs.Cond);
                var nt = fn(cs.TrueVal);
                var nf = fn(cs.FalseVal);
                return ReferenceEquals(nc, cs.Cond) && ReferenceEquals(nt, cs.TrueVal) && ReferenceEquals(nf, cs.FalseVal)
                    ? cs
                    : new CondSelectExpr(nc, nt, nf, cs.Type);
            }
            case MathCallExpr mc:
            {
                var changed = false;
                var newArgs = new Expr[mc.Args.Count];
                for (var i = 0; i < mc.Args.Count; i++)
                {
                    newArgs[i] = fn(mc.Args[i]);
                    if (!ReferenceEquals(newArgs[i], mc.Args[i])) changed = true;
                }
                return !changed ? mc : new MathCallExpr(mc.Name, mc.Type!, newArgs);
            }
            case ArrayElementExpr ae:
            {
                var na = fn(ae.Array);
                var ni = fn(ae.Index);
                return ReferenceEquals(na, ae.Array) && ReferenceEquals(ni, ae.Index) ? ae : new ArrayElementExpr(na, ni, ae.Type);
            }
            case ArrayLengthExpr al:
            {
                var na = fn(al.Array);
                return ReferenceEquals(na, al.Array) ? al : new ArrayLengthExpr(na);
            }
            case StringLengthExpr sl:
            {
                var ns = fn(sl.String);
                return ReferenceEquals(ns, sl.String) ? sl : new StringLengthExpr(ns);
            }
            case IndirectExpr ie:
            {
                var np = fn(ie.Pointer);
                return ReferenceEquals(np, ie.Pointer) ? ie : new IndirectExpr(np, ie.Type);
            }
            case IndirectStructExpr ise:
            {
                var na = fn(ise.Address);
                return ReferenceEquals(na, ise.Address) ? ise : new IndirectStructExpr(ise.Type, na);
            }
            case StructChunkExpr sc:
            {
                var ns = fn(sc.Source);
                return ReferenceEquals(ns, sc.Source) ? sc : new StructChunkExpr(ns, sc.Offset, sc.Size);
            }
            default:
                return root;
        }
    }

    private static bool SameSpillFamily(TypeSignature existing, TypeSignature candidate, Expr e)
    {
        if (existing is ByReferenceTypeSignature && candidate is ByReferenceTypeSignature)
            return SignatureComparer.Default.Equals(existing, candidate);
        var ek = Expr.KindOf(existing);
        var ck = Expr.KindOf(candidate);
        if (ek == ValueKind.Ref && e is ConstExpr { Value: 0 }) return true;
        if (ek is ValueKind.I4 or ValueKind.I8 && ck is ValueKind.I4 or ValueKind.I8) return true;
        if (ek is ValueKind.R4 or ValueKind.R8 && ck is ValueKind.R4 or ValueKind.R8) return true;
        return ek == ValueKind.Ref && ck == ValueKind.Ref && (existing.ElementType == AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Object || SignatureComparer.Default.Equals(existing, candidate));
    }

    private CilLocalVariable SpillLocalFor(int r, TypeSignature t)
    {
        var key = (r, t.FullName);
        if (_spillLocals.TryGetValue(key, out var existing)) return existing;
        var l = NewLocal(t);
        _spillLocals[key] = l;
        return l;
    }

    private TypeSignature TypeFor(Expr e) => e.Type ?? e.Kind switch
    {
        ValueKind.I4 => _f.Int32,
        ValueKind.R4 => _f.Single,
        ValueKind.R8 => _f.Double,
        ValueKind.Ref => _f.Object,
        ValueKind.Native => _f.IntPtr,
        _ => _f.Int64,
    };

    private static bool IsRematerializable(Expr e) =>
        e is AddressExpr or StackAddrPlaceholder or MarkerExpr or ConstExpr or FloatExpr or StringExpr or ArgExpr or StructChunkExpr { Source: LocalExpr };

    private static bool SameValue(Expr a, Expr b) => ReferenceEquals(a, b) || (a, b) switch
    {
        (LocalExpr x, LocalExpr y) => x.Local == y.Local,
        (AddressExpr x, AddressExpr y) => SameValue(x.Target, y.Target),
        (ArgExpr x, ArgExpr y) => x.Parameter == y.Parameter,
        (ConstExpr x, ConstExpr y) => x.Value == y.Value && x.Kind == y.Kind,
        (FloatExpr x, FloatExpr y) => x.Value.Equals(y.Value) && x.Kind == y.Kind,
        (StringExpr x, StringExpr y) => x.Value == y.Value,
        (MarkerExpr x, MarkerExpr y) => x.Marker == y.Marker && x.Index == y.Index,
        _ => false,
    };

    private Expr GetReg(in Arm64RegisterOperand rop)
    {
        if (rop.IsZeroRegister) return new ConstExpr(0, rop.Value.Kind == Arm64RegisterKind.W ? ValueKind.I4 : ValueKind.I8);
        return NarrowIfW(rop, GetRegRaw(Arm64Lifter.RegKey(rop)));
    }

    private Expr GetRegRaw(int key)
    {
        if (key < 0) return new ConstExpr(0, ValueKind.I8);
        if (_regs.TryGetValue(key, out var e)) return e;
        return new NativeExpr(key >= 32 ? $"v{key - 32}" : $"x{key}");
    }

    private Expr NarrowIfW(in Arm64RegisterOperand rop, Expr e)
    {
        if (rop.Value.Kind != Arm64RegisterKind.W || e.Kind != ValueKind.I8 || e is StructChunkExpr) return e;
        if (e is ConstExpr c) return new ConstExpr(unchecked((int)c.Value), ValueKind.I4, c.FromFloatBits);
        return new ConvExpr(e, _f.Int32);
    }

    private void SetReg(int key, Expr value)
    {
        if (key < 0 || key == 99) return;
        _regs[key] = value;
        if (value is MarkerExpr or ArgExpr) _lastRemat[key] = value; else _lastRemat.Remove(key);
    }

    private Expr ReadOpOrImm(in Arm64Instruction ins, int idx)
    {
        var op = ins.GetOperand(idx);
        if (op.Kind == Arm64OperandKind.Register) return GetReg((Arm64RegisterOperand)op);
        if (op.Kind == Arm64OperandKind.Immediate)
        {
            var imm = (Arm64ImmediateOperand)op;
            return imm.IsFloat ? new FloatExpr(imm.ValueAsFloat, true) : new ConstExpr(imm.Value, ValueKind.I4);
        }
        return new ConstExpr(0, ValueKind.I4);
    }

    private Expr ReadShifted(in Arm64Instruction ins, int idx)
    {
        var baseVal = ReadOpOrImm(ins, idx);
        if (ins.OperandCount > idx + 1)
        {
            var extra = ins.GetOperand(idx + 1);
            if (extra.Kind == Arm64OperandKind.Shift)
            {
                var sh = (Arm64ShiftOperand)extra;
                if (sh.Amount > 0)
                {
                    if (sh.ShiftKind == Arm64ShiftKind.LSR
                        && baseVal is UnaryExpr { Op.Code: CilCode.Not, Operand: var notInner }
                        && ((sh.Amount == 31 && notInner.Kind == ValueKind.I4) || (sh.Amount == 63 && notInner.Kind == ValueKind.I8)))
                        return new CompareExpr(Condition.Ge, notInner, ZeroLike(notInner));
                    if (baseVal is ConstExpr c)
                        return new ConstExpr(sh.ShiftKind == Arm64ShiftKind.LSL ? c.Value << sh.Amount
                            : sh.ShiftKind == Arm64ShiftKind.ASR ? c.Value >> sh.Amount
                            : unchecked((long)((ulong)c.Value >>> sh.Amount)), c.Kind);
                    var k = baseVal.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4;
                    var cilShift = sh.ShiftKind == Arm64ShiftKind.LSL ? CilOpCodes.Shl
                        : sh.ShiftKind == Arm64ShiftKind.ASR ? CilOpCodes.Shr
                        : CilOpCodes.Shr_Un;
                    return new BinaryExpr(cilShift, baseVal, new ConstExpr(sh.Amount, ValueKind.I4), k);
                }
            }
            else if (extra.Kind == Arm64OperandKind.Extend)
            {
                var ext = (Arm64ExtendOperand)extra;
                if (ext.Amount > 0)
                {
                    if (baseVal is ConstExpr c)
                        return new ConstExpr(c.Value << ext.Amount, ValueKind.I8);
                    var extended = baseVal.Kind == ValueKind.I8 ? baseVal : new ConvExpr(baseVal, _f.Int64);
                    return new BinaryExpr(CilOpCodes.Shl, extended, new ConstExpr(ext.Amount, ValueKind.I4), ValueKind.I8);
                }
            }
        }
        return baseVal;
    }

    private static int RegOp(in Arm64Instruction ins, int idx) => Arm64Lifter.RegKey((Arm64RegisterOperand)ins.GetOperand(idx));
    private static long ImmOp(in Arm64Instruction ins, int idx) => ((Arm64ImmediateOperand)ins.GetOperand(idx)).Value;
    private static Expr ZeroLike(Expr e) => e.Kind is ValueKind.R4 or ValueKind.R8 ? new FloatExpr(0, e.Kind == ValueKind.R4) : new ConstExpr(0, e.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4);
    private static bool IsBranch(in Arm64Instruction ins) => ins.Mnemonic is Arm64Mnemonic.B or Arm64Mnemonic.CBZ or Arm64Mnemonic.CBNZ or Arm64Mnemonic.TBZ or Arm64Mnemonic.TBNZ or Arm64Mnemonic.BR;

    private static int LoadStoreSize(Arm64Mnemonic m, in Arm64RegisterOperand rop) => m switch
    {
        Arm64Mnemonic.LDRB or Arm64Mnemonic.LDRSB or Arm64Mnemonic.STRB or Arm64Mnemonic.LDURB or Arm64Mnemonic.LDURSB or Arm64Mnemonic.STURB => 1,
        Arm64Mnemonic.LDRH or Arm64Mnemonic.LDRSH or Arm64Mnemonic.STRH or Arm64Mnemonic.LDURH or Arm64Mnemonic.LDURSH or Arm64Mnemonic.STURH => 2,
        Arm64Mnemonic.LDRSW or Arm64Mnemonic.LDURSW => 4,
        _ when rop.Value.Kind == Arm64RegisterKind.W => 4,
        _ when rop.Value.Kind >= Arm64RegisterKind.VScalar => rop.Value.VKind switch
        {
            Arm64RegisterVKind.B => 1,
            Arm64RegisterVKind.H => 2,
            Arm64RegisterVKind.S => 4,
            Arm64RegisterVKind.Q => 16,
            _ => 8,
        },
        _ => 8,
    };

    private static bool IsSpCalleeSave(in Arm64MemoryAccessorAny acc, in Arm64RegisterOperand reg)
    {
        if (acc.BaseRegister.Kind != Arm64RegisterKind.SP || acc.HasIndexRegister) return false;
        if (reg.Value.Kind is Arm64RegisterKind.X or Arm64RegisterKind.W) return reg.Value.Index is >= 19 and <= 30;
        if (reg.Value.Kind >= Arm64RegisterKind.VScalar) return reg.Value.Index is >= 8 and <= 15;
        return false;
    }
}
