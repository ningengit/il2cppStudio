using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;
using E = AsmResolver.PE.DotNet.Metadata.Tables.ElementType;

namespace Il2CppStudio.Core.Lifting.AsmJs;

/// <summary>
/// Direct AST-to-CIL lifter for Unity pre-5.6 WebGL Emscripten <c>asm.js</c> builds.
/// Parses each decompiled <c>asm.js</c> function, resolves <c>STATIC_BASE</c>/metadata-v16
/// lazy init blocks and <c>FUNCTION_TABLE</c> calls, reconstructs stack-allocated structs
/// (like <c>Color</c>/<c>Vector3</c>), and emits clean verifiable CIL via <see cref="MemoryResolver"/>.
/// </summary>
public sealed class AsmJsLifter(NativeSymbolizer sym, StubAssemblyBuilder stubs, AsmJsBinaryImage asmJs, IDiagnosticSink sink)
    : IMethodLifter
{
    private readonly Dictionary<string, int> _typeIndex = MemoryResolver.BuildTypeIndex(sym.Model);

    public bool SupportsParallelLifting => false;

    public LiftResult Lift(int methodIndex)
    {
        var def = stubs.GetMethodDef(methodIndex);
        if (def is null)
            return new LiftResult(methodIndex, false, 0, 0, 0, 0, 0, "no stub method");

        var va = sym.Map.GetMethodVa(methodIndex);
        if (va == 0)
            return new LiftResult(methodIndex, false, 0, 0, 0, 0, 0, "no asm.js slot");

        if (!asmJs.TryGetFunctionSource(va, out var fnName, out var source))
            return new LiftResult(methodIndex, false, 0, 0, 0, 0, 0, $"missing asm.js body at 0x{va:X}");

        var imageIdx = stubs.ImageOfType(sym.Metadata.Methods[methodIndex].DeclaringType);
        if (imageIdx < 0)
            return new LiftResult(methodIndex, false, 0, 0, 0, 0, 0, "no declaring image");

        var stub = stubs.GetModule(imageIdx);
        var module = stub.Module;
        var body = def.CilMethodBody = new CilMethodBody { InitializeLocals = true };
        var mname = sym.MethodName(methodIndex);

        var ctx = new EmitContext(body.Instructions, module, stub.Intrinsics)
        {
            Log = msg => sink.Verbose(DiagnosticCategory.Lifting, DiagnosticCodes.LiftCompleted,
                $"{mname}: {msg}", new() { MethodName = mname, Rva = va }),
        };

        try
        {
            var ast = AsmJsParser.ParseFunction(source);
            var prevAbi = ArgumentLocations.Abi;
            ArgumentLocations.Abi = NativeAbi.Wasm32;
            try
            {
                var emitter = new MethodEmitter(this, sym, stubs, asmJs, stub, module, def, ctx, methodIndex, ast);
                emitter.Emit();
                stub.MarkLifted(methodIndex);
                body.Instructions.OptimizeMacros();
                var fallbacks = emitter.Fallbacks;
                var unresolved = emitter.UnresolvedCalls;
                var elided = emitter.ElidedChecks;
                return new LiftResult(methodIndex, true, Math.Max(1, ast.Body.Count), body.Instructions.Count, fallbacks, unresolved, elided, null);
            }
            finally
            {
                ArgumentLocations.Abi = prevAbi;
            }
        }
        catch (Exception ex)
        {
            body.Instructions.Clear();
            body.Instructions.Add(CilOpCodes.Ldstr, $"asm.js lift ({fnName}): {ex.Message}");
            body.Instructions.Add(CilOpCodes.Call, stub.Intrinsics.Stmt);
            FallbackReturn(body, def);
            sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed,
                $"{mname}: {ex.Message}", new() { MethodName = mname, Rva = va });
            return new LiftResult(methodIndex, false, 1, body.Instructions.Count, 1, 0, 0, ex.Message);
        }
    }

    private static void FallbackReturn(CilMethodBody body, MethodDefinition def)
    {
        var ret = def.Signature?.ReturnType;
        var kind = Expr.KindOf(ret);
        switch (kind)
        {
            case ValueKind.Void: break;
            case ValueKind.R4: body.Instructions.Add(CilOpCodes.Ldc_R4, 0f); break;
            case ValueKind.R8: body.Instructions.Add(CilOpCodes.Ldc_R8, 0d); break;
            case ValueKind.I8: body.Instructions.Add(CilOpCodes.Ldc_I8, 0L); break;
            case ValueKind.Ref: body.Instructions.Add(CilOpCodes.Ldnull); break;
            case ValueKind.Struct when ret is not null:
                var loc = new CilLocalVariable(ret);
                body.LocalVariables.Add(loc);
                body.Instructions.Add(CilOpCodes.Ldloca, loc);
                body.Instructions.Add(CilOpCodes.Initobj, ret.ToTypeDefOrRef());
                body.Instructions.Add(CilOpCodes.Ldloc, loc);
                break;
            default: body.Instructions.Add(CilOpCodes.Ldc_I4_0); break;
        }
        body.Instructions.Add(CilOpCodes.Ret);
    }

    private sealed class MethodEmitter
    {
        private readonly AsmJsLifter _owner;
        private readonly NativeSymbolizer _sym;
        private readonly GlobalMetadata _md;
        private readonly Il2CppRegistration _reg;
        private readonly StubAssemblyBuilder _stubs;
        private readonly AsmJsBinaryImage _asmJs;
        private readonly StubModule _stub;
        private readonly ModuleDefinition _module;
        private readonly CorLibTypeFactory _f;
        private readonly MethodDefinition _def;
        private readonly EmitContext _ctx;
        private readonly int _methodIndex;
        private readonly AsmJsFunction _ast;
        private readonly MemoryResolver _mem;
        private readonly MethodShape _shape;

        private readonly Dictionary<string, JsVal> _vars = new(StringComparer.Ordinal);
        private readonly Dictionary<string, CilLocalVariable> _boundLocals = new(StringComparer.Ordinal);
        private readonly Dictionary<int, MetadataUsage> _globalUsages = [];
        private readonly Dictionary<int, FrameSlot> _slots = [];
        private readonly List<BreakTarget> _breakStack = [];
        private readonly Dictionary<string, BreakTarget> _labeledTargets = new(StringComparer.Ordinal);

        private CilLocalVariable? _retBuf;
        private Expr? _retBufExpr;
        private int _frameOffset;
        private bool _reachable = true;

        public int Fallbacks { get; private set; }
        public int UnresolvedCalls { get; private set; }
        public int ElidedChecks { get; private set; }

        private sealed record JsVal(Expr Expr, ValueKind Kind, TypeSignature? Pointee = null, int Disp = 0)
        {
            public JsVal WithDisp(int d) => this with { Disp = Disp + d };
        }

        private sealed class FrameSlot(int offset)
        {
            public int Offset { get; } = offset;
            public int Size { get; set; }
            public TypeSignature? StructType { get; set; }
            public CilLocalVariable? Local { get; set; }
            public bool ScalarUsed { get; set; }
            public int? ConstBits { get; set; }
        }

        private sealed record BreakTarget(CilInstructionLabel BreakLabel, CilInstructionLabel? ContinueLabel, string? Label);

        private sealed class FrameAddrExpr(int offset) : Expr
        {
            public int Offset { get; } = offset;
            public override ValueKind Kind => ValueKind.I4;
            public override void Emit(EmitContext ctx) =>
                throw new InvalidOperationException($"stack frame address +0x{Offset:X} escaped without a value access");
            public override string Describe() => $"sp+0x{Offset:X}";
        }

        private sealed class FramePairExpr(Expr low, Expr high) : Expr
        {
            public Expr Low { get; } = low;
            public Expr High { get; } = high;
            public override ValueKind Kind => ValueKind.I8;
            public override void Emit(EmitContext ctx)
            {
                Expr.EmitAs(ctx, Low, ctx.Module.CorLibTypeFactory.UInt32);
                ctx.IL.Add(CilOpCodes.Conv_U8);
                Expr.EmitAs(ctx, High, ctx.Module.CorLibTypeFactory.UInt32);
                ctx.IL.Add(CilOpCodes.Conv_U8);
                ctx.IL.Add(CilOpCodes.Ldc_I4, 32);
                ctx.IL.Add(CilOpCodes.Shl);
                ctx.IL.Add(CilOpCodes.Or);
            }
        }

        public MethodEmitter(
            AsmJsLifter owner,
            NativeSymbolizer sym,
            StubAssemblyBuilder stubs,
            AsmJsBinaryImage asmJs,
            StubModule stub,
            ModuleDefinition module,
            MethodDefinition def,
            EmitContext ctx,
            int methodIndex,
            AsmJsFunction ast)
        {
            _owner = owner;
            _sym = sym;
            _md = sym.Metadata;
            _reg = sym.Registration;
            _stubs = stubs;
            _asmJs = asmJs;
            _stub = stub;
            _module = module;
            _f = module.CorLibTypeFactory;
            _def = def;
            _ctx = ctx;
            _methodIndex = methodIndex;
            _ast = ast;
            _mem = new MemoryResolver(_md, _sym.Model, _reg, _sym.Names, _stubs, _module, _owner._typeIndex, pointerSize: 4);
            _shape = MethodShape.Create(_md, _reg, _stubs, _mem, _module, methodIndex, -1);

            BindParameters();
            BindDeclaredLocals();
        }

        private void BindParameters()
        {
            var jsIdx = 0;
            var ilArgIdx = 0;

            if (_shape.HasReturnBuffer)
            {
                _retBuf = NewLocal(_shape.ReturnType);
                _retBufExpr = new LocalExpr(_retBuf);
                if (jsIdx < _ast.Parameters.Count)
                {
                    _vars[_ast.Parameters[jsIdx]] = new JsVal(new AddressExpr(_retBufExpr), ValueKind.I4, _shape.ReturnType);
                    jsIdx++;
                }
            }

            if (!_shape.IsStatic)
            {
                var thisParam = _def.Parameters.ThisParameter;
                if (jsIdx < _ast.Parameters.Count && thisParam is not null)
                {
                    Expr thisExpr = _shape.DeclaringIsValueType
                        ? new ThisStructExpr(_stubs.TypeRefFor(_module, _md.Methods[_methodIndex].DeclaringType)!.ToTypeSignature(true))
                        : new ArgExpr(thisParam, thisParam.ParameterType);
                    _vars[_ast.Parameters[jsIdx]] = new JsVal(thisExpr, thisExpr.Kind);
                    jsIdx++;
                }
            }
            else if (_md.Version.StaticMethodsTakeDummyThis)
            {
                if (jsIdx < _ast.Parameters.Count)
                {
                    _vars[_ast.Parameters[jsIdx]] = new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
                    jsIdx++;
                }
            }

            foreach (var paramType in _shape.Parameters)
            {
                if (jsIdx >= _ast.Parameters.Count || ilArgIdx >= _def.Parameters.Count) break;
                var ilParam = _def.Parameters[ilArgIdx++];
                var pName = _ast.Parameters[jsIdx++];

                if (paramType is ByReferenceTypeSignature br)
                {
                    _vars[pName] = new JsVal(new ArgExpr(ilParam, paramType), ValueKind.I4, br.BaseType);
                }
                else if (MethodShape.PassedByPointer(paramType, _mem))
                {
                    _vars[pName] = new JsVal(new IndirectStructExpr(paramType, new ArgExpr(ilParam, paramType)), ValueKind.I4, paramType);
                }
                else if (Expr.KindOf(paramType) == ValueKind.I8 && jsIdx < _ast.Parameters.Count)
                {
                    var argExpr = new ArgExpr(ilParam, paramType);
                    var loName = pName;
                    var hiName = _ast.Parameters[jsIdx++];
                    _vars[loName] = new JsVal(new ConvExpr(argExpr, _f.UInt32), ValueKind.I4);
                    _vars[hiName] = new JsVal(new Lifting.BinaryExpr(CilOpCodes.Shr_Un, argExpr, new ConstExpr(32, ValueKind.I4), ValueKind.I8), ValueKind.I4);
                }
                else
                {
                    var argExpr = new ArgExpr(ilParam, paramType);
                    _vars[pName] = new JsVal(argExpr, argExpr.Kind);
                }
            }

            // Remaining parameter is trailing const MethodInfo* (if present).
            while (jsIdx < _ast.Parameters.Count)
            {
                _vars[_ast.Parameters[jsIdx++]] = new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
            }
        }

        private void BindDeclaredLocals()
        {
            foreach (var (name, init) in _ast.Locals)
            {
                if (init is null) continue;
                var val = EvalConstOrZero(init);
                _vars[name] = val;
            }
        }

        private JsVal EvalConstOrZero(AsmJsExpr expr)
        {
            if (expr is VarExpr v && _asmJs.FloatZeroVars.Contains(v.Name))
                return new JsVal(new FloatExpr(0f, single: true), ValueKind.R4);
            if (expr is UnaryExpr { Op: "+", Operand: FloatLitExpr fl })
                return new JsVal(new FloatExpr(fl.Value, single: false), ValueKind.R8);
            if (expr is UnaryExpr { Op: "+", Operand: IntLitExpr il })
                return new JsVal(new FloatExpr(il.Value, single: false), ValueKind.R8);
            if (expr is FloatLitExpr f)
                return new JsVal(new FloatExpr(f.Value, single: false), ValueKind.R8);
            if (expr is IntLitExpr i)
                return new JsVal(new ConstExpr(i.Value, ValueKind.I4), ValueKind.I4);
            return new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
        }

        public void Emit()
        {
            EmitStmtList(_ast.Body);
            if (_reachable)
                EmitReturnValue(null);
        }

        // ------------------------------------------------------------------ statements

        private void EmitStmtList(IReadOnlyList<AsmJsStmt> stmts)
        {
            for (var i = 0; i < stmts.Count; i++)
            {
                if (!_reachable) break;
                var remaining = i + 1 < stmts.Count ? stmts.Skip(i + 1) : [];
                EmitStmt(stmts[i], remaining);
            }
        }

        private void EmitStmt(AsmJsStmt stmt, IEnumerable<AsmJsStmt> afterStmts)
        {
            if (!_reachable) return;
            switch (stmt)
            {
                case EmptyStmt:
                    return;

                case BlockStmt b:
                    EmitStmtList(b.Statements);
                    return;

                case ExprStmt e:
                    EmitExpr(e.Expr, discard: true);
                    return;

                case ReturnStmt r:
                    EmitReturnValue(r.Value);
                    _reachable = false;
                    return;

                case BreakStmt br:
                    if (FindBreakTarget(br.Label) is { } bt)
                    {
                        _ctx.IL.Add(CilOpCodes.Br, bt.BreakLabel);
                        _reachable = false;
                    }
                    return;

                case ContinueStmt cont:
                    if (FindContinueTarget(cont.Label) is { ContinueLabel: { } ct })
                    {
                        _ctx.IL.Add(CilOpCodes.Br, ct);
                        _reachable = false;
                    }
                    return;

                case LabeledStmt ls:
                    EmitLabeledStmt(ls, afterStmts);
                    return;

                case IfStmt ifs:
                    EmitIfStmt(ifs, afterStmts);
                    return;

                case WhileStmt ws:
                    EmitWhileStmt(ws, label: null);
                    return;

                case DoWhileStmt dws:
                    EmitDoWhileStmt(dws, label: null);
                    return;

                case ForStmt fs:
                    EmitForStmt(fs, label: null);
                    return;

                case SwitchStmt sw:
                    EmitSwitchStmt(sw);
                    return;
            }
        }

        private void EmitLabeledStmt(LabeledStmt ls, IEnumerable<AsmJsStmt> afterStmts)
        {
            switch (ls.Statement)
            {
                case WhileStmt ws:
                    EmitWhileStmt(ws, ls.Label);
                    return;
                case DoWhileStmt dws:
                    EmitDoWhileStmt(dws, ls.Label);
                    return;
                case ForStmt fs:
                    EmitForStmt(fs, ls.Label);
                    return;
                default:
                {
                    var endLabel = new CilInstructionLabel();
                    var target = new BreakTarget(endLabel, null, ls.Label);
                    _breakStack.Add(target);
                    _labeledTargets[ls.Label] = target;
                    try
                    {
                        EmitStmt(ls.Statement, afterStmts);
                    }
                    finally
                    {
                        _labeledTargets.Remove(ls.Label);
                        _breakStack.RemoveAt(_breakStack.Count - 1);
                    }
                    endLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                    _reachable = true;
                    return;
                }
            }
        }

        private void EmitIfStmt(IfStmt ifs, IEnumerable<AsmJsStmt> afterStmts)
        {
            if (TryHandleInitBlock(ifs))
            {
                ElidedChecks++;
                if (ifs.Else is not null)
                    EmitStmt(ifs.Else, afterStmts);
                return;
            }

            if (IsCctorGuard(ifs))
            {
                ElidedChecks++;
                return;
            }

            if (TryEvalConstInt(ifs.Condition, out var constCond))
            {
                if (constCond != 0) EmitStmt(ifs.Then, afterStmts);
                else if (ifs.Else is not null) EmitStmt(ifs.Else, afterStmts);
                return;
            }

            var readsAfter = CollectVarReads(afterStmts);
            var mutatedInBranch = new HashSet<string>(StringComparer.Ordinal);
            CollectVarWrites(ifs.Then, mutatedInBranch);
            if (ifs.Else is not null) CollectVarWrites(ifs.Else, mutatedInBranch);
            mutatedInBranch.IntersectWith(readsAfter);
            if (_asmJs.StackTopVar is { } stv) mutatedInBranch.Remove(stv);

            // Snapshot variable map before branching
            var entryVars = new Dictionary<string, JsVal>(_vars, StringComparer.Ordinal);
            var entryFrame = _frameOffset;

            var elseLabel = new CilInstructionLabel();
            var endLabel = new CilInstructionLabel();

            EmitBranchIfFalse(ifs.Condition, elseLabel);

            // Then branch
            EmitStmt(ifs.Then, afterStmts);
            var thenReachable = _reachable;
            var thenVars = new Dictionary<string, JsVal>(_vars, StringComparer.Ordinal);
            var thenFrame = _frameOffset;

            // If no Else, check if mutated variables need merge locals before jumping/falling through
            Dictionary<string, CilLocalVariable>? mergeLocals = null;
            if (ifs.Else is null)
            {
                if (thenReachable)
                {
                    foreach (var v in mutatedInBranch)
                    {
                        if (thenVars.TryGetValue(v, out var tv) && (!entryVars.TryGetValue(v, out var ev) || !SameJsVal(tv, ev))
                            && CanMaterialize(tv))
                        {
                            mergeLocals ??= new(StringComparer.Ordinal);
                            var loc = GetOrCreateMergeLocal(v, tv, entryVars.GetValueOrDefault(v));
                            mergeLocals[v] = loc;
                            Expr.EmitAs(_ctx, tv.Expr, loc.VariableType);
                            _ctx.IL.Add(CilOpCodes.Stloc, loc);
                        }
                    }
                    var afterThen = _ctx.IL.Add(CilOpCodes.Br, endLabel);
                    _ = afterThen;
                }
                elseLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);

                if (mergeLocals is not null)
                {
                    foreach (var (v, loc) in mergeLocals)
                    {
                        if (entryVars.TryGetValue(v, out var ev) && CanMaterialize(ev))
                        {
                            Expr.EmitAs(_ctx, ev.Expr, loc.VariableType);
                            _ctx.IL.Add(CilOpCodes.Stloc, loc);
                        }
                    }
                }

                endLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                RestoreAndMergeVars(entryVars, thenVars, entryVars, thenReachable, elseReachable: true, mergeLocals);
                _frameOffset = entryFrame;
                _reachable = true;
                return;
            }

            // Has Else branch: materialize mutated variables on the Then side (deferred insertion? No—emit Then merge right before Br endLabel if Then could mutate them, using entryVars hint!)
            if (thenReachable && mutatedInBranch.Count > 0)
            {
                foreach (var v in mutatedInBranch)
                {
                    if (thenVars.TryGetValue(v, out var tv) && (!entryVars.TryGetValue(v, out var ev) || !SameJsVal(tv, ev))
                        && CanMaterialize(tv))
                    {
                        mergeLocals ??= new(StringComparer.Ordinal);
                        var loc = GetOrCreateMergeLocal(v, tv, entryVars.GetValueOrDefault(v));
                        mergeLocals[v] = loc;
                        Expr.EmitAs(_ctx, tv.Expr, loc.VariableType);
                        _ctx.IL.Add(CilOpCodes.Stloc, loc);
                    }
                }
            }
            if (thenReachable)
                _ctx.IL.Add(CilOpCodes.Br, endLabel);

            elseLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            ReplaceVars(_vars, entryVars);
            _frameOffset = entryFrame;
            _reachable = true;

            EmitStmt(ifs.Else, afterStmts);
            var elseReachable = _reachable;
            var elseVars = new Dictionary<string, JsVal>(_vars, StringComparer.Ordinal);
            var elseFrame = _frameOffset;

            if (elseReachable && mutatedInBranch.Count > 0)
            {
                foreach (var v in mutatedInBranch)
                {
                    if (!elseVars.TryGetValue(v, out var elv) || !CanMaterialize(elv)) continue;
                    CilLocalVariable? loc = null;
                    if (mergeLocals is not null && mergeLocals.TryGetValue(v, out var existing))
                    {
                        loc = existing;
                    }
                    else if (thenReachable && (!thenVars.TryGetValue(v, out var tv) || !SameJsVal(tv, elv)))
                    {
                        // Only mutated in Else (Then kept entry): we still need merge local only if Then reads aren't broken;
                        // since Then wasn't emitted to store it, store in Else only if Then also wrote entry—otherwise allocate before If wasn't done.
                        // However if Then didn't mutate v, entryVars[v] may be a constant; materializing into fresh local only works if Then stored it too.
                        // Keep symbolic if only Else reachable; if both reachable, materialize only when mergeLocals already created OR fall back to Else local only if Then never assigned.
                        if (!thenVars.ContainsKey(v))
                        {
                            mergeLocals ??= new(StringComparer.Ordinal);
                            loc = GetOrCreateMergeLocal(v, elv, null);
                            mergeLocals[v] = loc;
                        }
                    }
                    if (loc is not null)
                    {
                        Expr.EmitAs(_ctx, elv.Expr, loc.VariableType);
                        _ctx.IL.Add(CilOpCodes.Stloc, loc);
                    }
                }
            }

            endLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            RestoreAndMergeVars(entryVars, thenVars, elseVars, thenReachable, elseReachable, mergeLocals);
            _frameOffset = thenReachable ? thenFrame : elseFrame;
            _reachable = thenReachable || elseReachable;
        }

        private void RestoreAndMergeVars(
            Dictionary<string, JsVal> entryVars,
            Dictionary<string, JsVal> thenVars,
            Dictionary<string, JsVal> elseVars,
            bool thenReachable,
            bool elseReachable,
            Dictionary<string, CilLocalVariable>? mergeLocals)
        {
            if (thenReachable && !elseReachable)
            {
                ReplaceVars(_vars, thenVars);
                return;
            }
            if (!thenReachable && elseReachable)
            {
                ReplaceVars(_vars, elseVars);
                return;
            }
            _vars.Clear();
            foreach (var (k, tv) in thenVars)
            {
                if (mergeLocals is not null && mergeLocals.TryGetValue(k, out var ml))
                {
                    _vars[k] = new JsVal(new LocalExpr(ml), Expr.KindOf(ml.VariableType), tv.Pointee, tv.Disp);
                    continue;
                }
                if (elseVars.TryGetValue(k, out var ev) && SameJsVal(tv, ev))
                    _vars[k] = tv;
                else if (entryVars.TryGetValue(k, out var ov))
                    _vars[k] = ov;
            }
            if (mergeLocals is not null)
            {
                foreach (var (k, ml) in mergeLocals)
                {
                    if (!_vars.ContainsKey(k))
                        _vars[k] = new JsVal(new LocalExpr(ml), Expr.KindOf(ml.VariableType));
                }
            }
        }

        private CilLocalVariable GetOrCreateMergeLocal(string name, JsVal primary, JsVal? fallback)
        {
            var type = PickStorageType(primary, fallback);
            if (_boundLocals.TryGetValue(name, out var existing) && SignatureComparer.Default.Equals(existing.VariableType, type))
                return existing;
            var loc = NewLocal(type);
            _boundLocals[name] = loc;
            return loc;
        }

        private void EmitWhileStmt(WhileStmt ws, string? label)
        {
            var mutated = new HashSet<string>(StringComparer.Ordinal);
            CollectVarWrites(ws.Body, mutated);
            if (_asmJs.StackTopVar is { } stv) mutated.Remove(stv);
            PreMaterializeLoopVars(mutated);

            var condLabel = new CilInstructionLabel();
            var breakLabel = new CilInstructionLabel();
            var target = new BreakTarget(breakLabel, condLabel, label);
            _breakStack.Add(target);
            if (label is not null) _labeledTargets[label] = target;

            try
            {
                condLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                EmitBranchIfFalse(ws.Condition, breakLabel);
                EmitStmt(ws.Body, []);
                if (_reachable)
                    _ctx.IL.Add(CilOpCodes.Br, condLabel);
            }
            finally
            {
                if (label is not null) _labeledTargets.Remove(label);
                _breakStack.RemoveAt(_breakStack.Count - 1);
            }
            breakLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            _reachable = true;
        }

        private void EmitDoWhileStmt(DoWhileStmt dws, string? label)
        {
            // Emscripten `do { ... } while (0);` is a breakable single-pass block, not a loop.
            if (TryEvalConstInt(dws.Condition, out var c) && c == 0)
            {
                var breakLbl = new CilInstructionLabel();
                var target = new BreakTarget(breakLbl, breakLbl, label);
                _breakStack.Add(target);
                if (label is not null) _labeledTargets[label] = target;
                try
                {
                    EmitStmt(dws.Body, []);
                }
                finally
                {
                    if (label is not null) _labeledTargets.Remove(label);
                    _breakStack.RemoveAt(_breakStack.Count - 1);
                }
                breakLbl.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                _reachable = true;
                return;
            }

            var mutated = new HashSet<string>(StringComparer.Ordinal);
            CollectVarWrites(dws.Body, mutated);
            if (_asmJs.StackTopVar is { } stv) mutated.Remove(stv);
            PreMaterializeLoopVars(mutated);

            var topLabel = new CilInstructionLabel();
            var condLabel = new CilInstructionLabel();
            var breakLabel = new CilInstructionLabel();
            var loopTarget = new BreakTarget(breakLabel, condLabel, label);
            _breakStack.Add(loopTarget);
            if (label is not null) _labeledTargets[label] = loopTarget;

            try
            {
                topLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                EmitStmt(dws.Body, []);
                condLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                if (_reachable)
                {
                    EmitBranchIfTrue(dws.Condition, topLabel);
                }
            }
            finally
            {
                if (label is not null) _labeledTargets.Remove(label);
                _breakStack.RemoveAt(_breakStack.Count - 1);
            }
            breakLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            _reachable = true;
        }

        private void EmitForStmt(ForStmt fs, string? label)
        {
            if (fs.Init is not null)
                EmitExpr(fs.Init, discard: true);

            var mutated = new HashSet<string>(StringComparer.Ordinal);
            CollectVarWrites(fs.Body, mutated);
            if (fs.Step is not null) CollectExprVarWrites(fs.Step, mutated);
            if (_asmJs.StackTopVar is { } stv) mutated.Remove(stv);
            PreMaterializeLoopVars(mutated);

            var condLabel = new CilInstructionLabel();
            var stepLabel = new CilInstructionLabel();
            var breakLabel = new CilInstructionLabel();
            var target = new BreakTarget(breakLabel, fs.Step is not null ? stepLabel : condLabel, label);
            _breakStack.Add(target);
            if (label is not null) _labeledTargets[label] = target;

            try
            {
                condLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                if (fs.Condition is not null)
                    EmitBranchIfFalse(fs.Condition, breakLabel);
                EmitStmt(fs.Body, []);
                stepLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                if (fs.Step is not null && _reachable)
                    EmitExpr(fs.Step, discard: true);
                if (_reachable)
                    _ctx.IL.Add(CilOpCodes.Br, condLabel);
            }
            finally
            {
                if (label is not null) _labeledTargets.Remove(label);
                _breakStack.RemoveAt(_breakStack.Count - 1);
            }
            breakLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            _reachable = true;
        }

        private void EmitSwitchStmt(SwitchStmt sw)
        {
            var disc = EmitExpr(sw.Discriminant, discard: false)?.Expr ?? new ConstExpr(0, ValueKind.I4);
            var discLocal = NewLocal(_f.Int32);
            Expr.EmitAs(_ctx, disc, _f.Int32);
            _ctx.IL.Add(CilOpCodes.Stloc, discLocal);

            var breakLabel = new CilInstructionLabel();
            var target = new BreakTarget(breakLabel, null, null);
            _breakStack.Add(target);

            var caseLabels = new CilInstructionLabel[sw.Cases.Count];
            CilInstructionLabel? defaultLabel = null;
            for (var i = 0; i < sw.Cases.Count; i++)
            {
                caseLabels[i] = new CilInstructionLabel();
                if (sw.Cases[i].Value is null) defaultLabel = caseLabels[i];
            }

            try
            {
                for (var i = 0; i < sw.Cases.Count; i++)
                {
                    if (sw.Cases[i].Value is not { } cv) continue;
                    _ctx.IL.Add(CilOpCodes.Ldloc, discLocal);
                    _ctx.IL.Add(CilOpCodes.Ldc_I4, unchecked((int)cv));
                    _ctx.IL.Add(CilOpCodes.Beq, caseLabels[i]);
                }
                _ctx.IL.Add(CilOpCodes.Br, defaultLabel ?? breakLabel);

                for (var i = 0; i < sw.Cases.Count; i++)
                {
                    caseLabels[i].Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                    _reachable = true;
                    EmitStmtList(sw.Cases[i].Body);
                }
            }
            finally
            {
                _breakStack.RemoveAt(_breakStack.Count - 1);
            }
            breakLabel.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            _reachable = true;
        }

        private void PreMaterializeLoopVars(HashSet<string> vars)
        {
            foreach (var v in vars)
            {
                if (!_vars.TryGetValue(v, out var cur) || !CanMaterialize(cur)) continue;
                if (cur.Expr is LocalExpr) continue;
                var type = PickStorageType(cur, null);
                var loc = _boundLocals.TryGetValue(v, out var ex) && SignatureComparer.Default.Equals(ex.VariableType, type)
                    ? ex : NewLocal(type);
                _boundLocals[v] = loc;
                Expr.EmitAs(_ctx, cur.Expr, loc.VariableType);
                _ctx.IL.Add(CilOpCodes.Stloc, loc);
                _vars[v] = new JsVal(new LocalExpr(loc), Expr.KindOf(loc.VariableType), cur.Pointee, cur.Disp);
            }
        }

        private bool CanMaterialize(JsVal v) =>
            v.Expr is not FrameAddrExpr && v.Expr is not AddressExpr && !v.Expr.IsMarker && v.Expr is not IndirectStructExpr;

        private TypeSignature PickStorageType(JsVal primary, JsVal? fallback)
        {
            var e = primary.Expr;
            if (e is CompareExpr || e.Type?.ElementType == E.Boolean) return _f.Boolean;
            if (e.Type is { } t && Expr.KindOf(t) is ValueKind.Ref or ValueKind.Struct or ValueKind.R4 or ValueKind.R8 or ValueKind.I8)
                return t;
            return primary.Kind switch
            {
                ValueKind.R4 => _f.Single,
                ValueKind.R8 => _f.Double,
                ValueKind.I8 => _f.Int64,
                ValueKind.Ref => fallback?.Expr.Type ?? _f.Object,
                _ => _f.Int32,
            };
        }

        // ------------------------------------------------------------------ metadata v16 init block & cctor guard elimination

        private bool TryHandleInitBlock(IfStmt ifs)
        {
            var thenStmts = ifs.Then is BlockStmt b ? b.Statements : (IReadOnlyList<AsmJsStmt>)[ifs.Then];
            if (thenStmts.Count == 0) return false;

            var pendingWrites = new List<(int ByteAddr, MetadataUsage Usage)>();
            var pendingVars = new Dictionary<string, MetadataUsage>(StringComparer.Ordinal);

            foreach (var s in thenStmts)
            {
                if (s is EmptyStmt) continue;
                if (s is not ExprStmt es) return false;
                if (!MatchInitStmtExpr(es.Expr, pendingVars, pendingWrites)) return false;
            }

            if (pendingWrites.Count == 0) return false;
            foreach (var (addr, usage) in pendingWrites)
                _globalUsages[addr] = usage;

            // If there is no Else block, apply any local variable init values directly if needed later.
            if (ifs.Else is null)
            {
                foreach (var (vName, u) in pendingVars)
                    _vars[vName] = new JsVal(ExprForUsage(u), ValueKind.I4);
            }
            return true;
        }

        private bool MatchInitStmtExpr(
            AsmJsExpr expr,
            Dictionary<string, MetadataUsage> pendingVars,
            List<(int ByteAddr, MetadataUsage Usage)> pendingWrites)
        {
            if (expr is CallExpr call && IsHelper(call, out var kind) && kind is AsmJsHelperKind.ClassInit or AsmJsHelperKind.NullCheck)
                return true;

            if (expr is not AssignExpr assign) return false;

            if (assign.Target is VarExpr v)
            {
                if (TryResolveInitValue(assign.Value, pendingVars, out var usage))
                {
                    pendingVars[v.Name] = usage;
                    return true;
                }
                return false;
            }

            if (assign.Target is IndexExpr { Target: VarExpr arr } idx
                && _asmJs.HeapViews.TryGetValue(arr.Name, out var view)
                && TryEvalConstInt(idx.Index, out var elemIdx))
            {
                var byteAddr = unchecked((int)((uint)elemIdx << ViewShift(view)));
                if (TryResolveInitValue(assign.Value, pendingVars, out var usage))
                {
                    pendingWrites.Add((byteAddr, usage));
                    return true;
                }
                // Flag store: a[1067344] = 1
                if (TryEvalConstInt(assign.Value, out _))
                    return true;
            }
            return false;
        }

        private bool TryResolveInitValue(AsmJsExpr expr, Dictionary<string, MetadataUsage> pendingVars, out MetadataUsage usage)
        {
            expr = StripIntCoercion(expr);
            if (expr is CallExpr call && IsHelper(call, out var kind) && call.Args.Count >= 1 && TryEvalConstInt(call.Args[0], out var argIdx))
            {
                if (TryUsageFromHelper(kind, unchecked((int)argIdx), out usage))
                    return true;
            }
            if (expr is VarExpr v && pendingVars.TryGetValue(v.Name, out usage))
                return true;
            usage = default;
            return false;
        }

        private bool IsCctorGuard(IfStmt ifs)
        {
            if (ifs.Else is not null) return false;
            var thenStmts = ifs.Then is BlockStmt b ? b.Statements : (IReadOnlyList<AsmJsStmt>)[ifs.Then];
            var sawHelper = false;
            var isCctorCond = IsCctorCondition(ifs.Condition);
            foreach (var s in thenStmts)
            {
                if (s is EmptyStmt) continue;
                if (s is ExprStmt { Expr: CallExpr call })
                {
                    if (IsHelper(call, out var kind) && kind is AsmJsHelperKind.ClassInit or AsmJsHelperKind.NullCheck)
                    {
                        sawHelper = true;
                        continue;
                    }
                    if (isCctorCond && call.Args.Count == 1 && call.Callee is VarExpr)
                    {
                        sawHelper = true;
                        continue;
                    }
                }
                return false;
            }
            return sawHelper;
        }

        private static bool IsCctorCondition(AsmJsExpr cond) => cond switch
        {
            CondExpr c => IsCctorCondition(c.Condition) || IsCctorCondition(c.Then),
            BinaryExpr { Op: "&", Right: IntLitExpr { Value: 256 } } => true,
            BinaryExpr { Op: "==" or "===" or "!=" or "!==" } b => IsCctorCondition(b.Left) || IsCctorCondition(b.Right),
            _ => false,
        };

        private bool IsHelper(CallExpr call, out AsmJsHelperKind kind)
        {
            if (call.Callee is VarExpr v)
            {
                kind = _asmJs.ClassifyHelper(v.Name);
                return kind != AsmJsHelperKind.None;
            }
            kind = AsmJsHelperKind.None;
            return false;
        }

        private bool TryUsageFromHelper(AsmJsHelperKind kind, int rawIndex, out MetadataUsage usage)
        {
            switch (kind)
            {
                case AsmJsHelperKind.V16MethodInfo when _reg.TryResolveV16EncodedMethodIndex(unchecked((uint)rawIndex), out var mk, out var mt):
                    usage = new MetadataUsage(0, mk, mt);
                    return true;
                case AsmJsHelperKind.V16TypeInfo:
                    usage = new MetadataUsage(0, MetadataUsageKind.TypeInfo, rawIndex);
                    return true;
                case AsmJsHelperKind.V16StringLiteral:
                    usage = new MetadataUsage(0, MetadataUsageKind.StringLiteral, rawIndex);
                    return true;
                default:
                    usage = default;
                    return false;
            }
        }

        private Expr ExprForUsage(MetadataUsage u) => u.Kind switch
        {
            MetadataUsageKind.StringLiteral => new StringExpr(_md.GetStringLiteral(u.Index), _f.String),
            MetadataUsageKind.TypeInfo => new MarkerExpr(MarkerKind.TypeInfo, u.Index, _sym.DescribeUsage(u), usage: u),
            MetadataUsageKind.Il2CppType => new MarkerExpr(MarkerKind.Il2CppType, u.Index, _sym.DescribeUsage(u), usage: u),
            MetadataUsageKind.MethodDef or MetadataUsageKind.MethodRef => new MarkerExpr(MarkerKind.MethodInfo, u.Index, _sym.DescribeUsage(u), usage: u),
            MetadataUsageKind.FieldInfo or MetadataUsageKind.FieldRva => new MarkerExpr(MarkerKind.FieldInfo, u.Index, _sym.DescribeUsage(u), usage: u),
            _ => new MarkerExpr(MarkerKind.TypeInfo, u.Index, _sym.DescribeUsage(u), usage: u),
        };

        // ------------------------------------------------------------------ expressions

        private JsVal? EmitExpr(AsmJsExpr expr, bool discard)
        {
            switch (expr)
            {
                case IntLitExpr il:
                    return discard ? null : new JsVal(new ConstExpr(il.Value, ValueKind.I4), ValueKind.I4);

                case FloatLitExpr fl:
                    return discard ? null : new JsVal(new FloatExpr(fl.Value, single: false), ValueKind.R8);

                case VarExpr v:
                    if (v.Name == _asmJs.StackTopVar)
                        return new JsVal(new FrameAddrExpr(_frameOffset), ValueKind.I4);
                    if (_asmJs.FloatZeroVars.Contains(v.Name))
                        return discard ? null : new JsVal(new FloatExpr(0f, single: true), ValueKind.R4);
                    if (_vars.TryGetValue(v.Name, out var found))
                        return found;
                    return discard ? null : new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);

                case AssignExpr a:
                    return EmitAssign(a, discard);

                case IndexExpr ix:
                    return EmitHeapLoad(ix);

                case CallExpr call:
                    return EmitCall(call, discard);

                case UnaryExpr u:
                    return EmitUnary(u, discard);

                case BinaryExpr b:
                    return EmitBinary(b, discard);

                case CondExpr c:
                    return EmitCondExpr(c, discard);

                default:
                    Fallbacks++;
                    return new JsVal(new NativeExpr(expr.ToString() ?? "asmjs"), ValueKind.I4);
            }
        }

        private JsVal? EmitAssign(AssignExpr assign, bool discard)
        {
            if (assign.Target is VarExpr v)
            {
                // Skip parameter type coercion boilerplate like `a = a | 0;`
                if (IsSameVarCoercion(v.Name, assign.Value) && _vars.ContainsKey(v.Name))
                    return discard ? null : _vars[v.Name];

                var rhs = EmitExpr(assign.Value, discard: false) ?? new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
                if (v.Name == _asmJs.StackTopVar)
                {
                    if (rhs.Expr is FrameAddrExpr fa)
                        _frameOffset = fa.Offset;
                    return discard ? null : rhs;
                }

                // Store symbolically if constant/marker/frame/address OR pure parameter expression unchanged;
                // only store into CLR local when runtime expression has side effects or non-trivial computation (except pure FieldExpr/Const/Marker).
                if (rhs.Disp != 0 || rhs.Expr is FrameAddrExpr or AddressExpr or MarkerExpr or StringExpr or ConstExpr or FloatExpr or ArgExpr or IndirectStructExpr or ThisStructExpr)
                {
                    _vars[v.Name] = rhs;
                    return discard ? null : rhs;
                }

                // Runtime expression (call result, arithmetic, loaded field): store in typed CLR local so it isn't re-evaluated twice!
                if (rhs.Expr is LocalExpr existingLoc)
                {
                    _vars[v.Name] = rhs;
                    return discard ? null : rhs;
                }

                var targetType = PickStorageType(rhs, _vars.GetValueOrDefault(v.Name));
                var loc = _boundLocals.TryGetValue(v.Name, out var curLoc) && SignatureComparer.Default.Equals(curLoc.VariableType, targetType)
                    ? curLoc : NewLocal(targetType);
                _boundLocals[v.Name] = loc;
                Expr.EmitAs(_ctx, rhs.Expr, loc.VariableType);
                _ctx.IL.Add(CilOpCodes.Stloc, loc);
                var stored = new JsVal(new LocalExpr(loc), Expr.KindOf(loc.VariableType), rhs.Pointee, rhs.Disp);
                _vars[v.Name] = stored;
                return discard ? null : stored;
            }

            if (assign.Target is IndexExpr ix)
            {
                var rhs = EmitExpr(assign.Value, discard: false) ?? new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
                EmitHeapStore(ix, rhs);
                return discard ? null : rhs;
            }

            Fallbacks++;
            return null;
        }

        private bool IsSameVarCoercion(string name, AsmJsExpr rhs) => rhs switch
        {
            BinaryExpr { Op: "|", Left: VarExpr lv, Right: IntLitExpr { Value: 0 } } => lv.Name == name,
            UnaryExpr { Op: "+", Operand: VarExpr lv } => lv.Name == name,
            CallExpr { Callee: VarExpr fv, Args: [VarExpr av] } when fv.Name == _asmJs.FroundVar => av.Name == name,
            _ => false,
        };

        private JsVal? EmitUnary(UnaryExpr u, bool discard)
        {
            var inner = EmitExpr(u.Operand, discard: false);
            if (inner is null) return null;
            if (discard) return null;

            switch (u.Op)
            {
                case "+":
                    if (inner.Expr is FloatExpr f) return new JsVal(new FloatExpr(f.Value, single: false), ValueKind.R8);
                    if (inner.Expr is ConstExpr c) return new JsVal(new FloatExpr(c.Value, single: false), ValueKind.R8);
                    if (inner.Expr.Kind == ValueKind.R8) return inner;
                    return new JsVal(new ConvExpr(inner.Expr, _f.Double), ValueKind.R8);

                case "-":
                    if (inner.Expr is ConstExpr ci) return new JsVal(new ConstExpr(-ci.Value, ValueKind.I4), ValueKind.I4);
                    if (inner.Expr is FloatExpr fi) return new JsVal(new FloatExpr(-fi.Value, fi.Kind == ValueKind.R4), fi.Kind);
                    return new JsVal(new Lifting.UnaryExpr(CilOpCodes.Neg, inner.Expr), inner.Kind);

                case "~":
                    if (inner.Expr is ConstExpr ct) return new JsVal(new ConstExpr(~ct.Value, ValueKind.I4), ValueKind.I4);
                    return new JsVal(new Lifting.UnaryExpr(CilOpCodes.Not, inner.Expr), ValueKind.I4);

                case "!":
                    if (inner.Expr is CompareExpr ce)
                        return new JsVal(new CompareExpr(NegateCondition(ce.Condition), ce.Left, ce.Right), ValueKind.I4);
                    return new JsVal(new CompareExpr(Condition.Eq, inner.Expr, new ConstExpr(0, ValueKind.I4)), ValueKind.I4);
            }
            return inner;
        }

        private JsVal? EmitBinary(BinaryExpr b, bool discard)
        {
            if (b.Op == ",")
            {
                EmitExpr(b.Left, discard: true);
                return EmitExpr(b.Right, discard);
            }

            var left = EmitExpr(b.Left, discard: false) ?? new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
            var right = EmitExpr(b.Right, discard: false) ?? new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
            if (discard) return null;

            // `expr | 0` in asm.js is an i32 type tag — preserve FrameAddr / Address / Marker / Ref / Bool / Float!
            if (b.Op == "|" && right.Expr is ConstExpr { Value: 0 })
                return left;
            if (b.Op == ">>>" && right.Expr is ConstExpr { Value: 0 })
                return left;

            // Pointer / FrameAddr + constant displacement
            if (b.Op == "+")
            {
                if (left.Expr is FrameAddrExpr fa && right.Expr is ConstExpr rc)
                    return new JsVal(new FrameAddrExpr(fa.Offset + unchecked((int)rc.Value)), ValueKind.I4);
                if (right.Expr is FrameAddrExpr fb && left.Expr is ConstExpr lc)
                    return new JsVal(new FrameAddrExpr(fb.Offset + unchecked((int)lc.Value)), ValueKind.I4);

                if (right.Expr is ConstExpr rcPtr && (left.Expr.Kind is ValueKind.Ref or ValueKind.Struct || left.Expr is AddressExpr or IndirectStructExpr or ThisStructExpr or NewObjExpr || left.Pointee is not null))
                    return left.WithDisp(unchecked((int)rcPtr.Value));
                if (left.Expr is ConstExpr lcPtr && (right.Expr.Kind is ValueKind.Ref or ValueKind.Struct || right.Expr is AddressExpr or IndirectStructExpr or ThisStructExpr or NewObjExpr || right.Pointee is not null))
                    return right.WithDisp(unchecked((int)lcPtr.Value));
            }
            if (b.Op == "-" && right.Expr is ConstExpr subC)
            {
                if (left.Expr is FrameAddrExpr fa)
                    return new JsVal(new FrameAddrExpr(fa.Offset - unchecked((int)subC.Value)), ValueKind.I4);
            }

            // Constant integer folding
            if (left.Expr is ConstExpr lci && right.Expr is ConstExpr rci)
            {
                var l32 = unchecked((int)lci.Value);
                var r32 = unchecked((int)rci.Value);
                long? folded = b.Op switch
                {
                    "+" => l32 + r32,
                    "-" => l32 - r32,
                    "*" => l32 * r32,
                    "&" => l32 & r32,
                    "|" => l32 | r32,
                    "^" => l32 ^ r32,
                    "<<" => l32 << (r32 & 31),
                    ">>" => l32 >> (r32 & 31),
                    ">>>" => (uint)l32 >> (r32 & 31),
                    _ => null,
                };
                if (folded is { } fv)
                    return new JsVal(new ConstExpr(fv, ValueKind.I4), ValueKind.I4);
            }

            // Comparisons
            Condition? cond = b.Op switch
            {
                "==" or "===" => Condition.Eq,
                "!=" or "!==" => Condition.Ne,
                "<" => IsUnsignedExpr(b.Left, b.Right) ? Condition.LtUn : Condition.Lt,
                "<=" => IsUnsignedExpr(b.Left, b.Right) ? Condition.LeUn : Condition.Le,
                ">" => IsUnsignedExpr(b.Left, b.Right) ? Condition.GtUn : Condition.Gt,
                ">=" => IsUnsignedExpr(b.Left, b.Right) ? Condition.GeUn : Condition.Ge,
                _ => null,
            };
            if (cond is { } cOp)
                return new JsVal(new CompareExpr(cOp, left.Expr, right.Expr), ValueKind.I4);

            var outKind = left.Kind == ValueKind.R8 || right.Kind == ValueKind.R8 ? ValueKind.R8
                : left.Kind == ValueKind.R4 || right.Kind == ValueKind.R4 ? ValueKind.R4
                : ValueKind.I4;

            var cilOp = b.Op switch
            {
                "+" => CilOpCodes.Add,
                "-" => CilOpCodes.Sub,
                "*" => CilOpCodes.Mul,
                "/" => IsUnsignedExpr(b.Left, b.Right) ? CilOpCodes.Div_Un : CilOpCodes.Div,
                "%" => IsUnsignedExpr(b.Left, b.Right) ? CilOpCodes.Rem_Un : CilOpCodes.Rem,
                "&" => CilOpCodes.And,
                "|" => CilOpCodes.Or,
                "^" => CilOpCodes.Xor,
                "<<" => CilOpCodes.Shl,
                ">>" => CilOpCodes.Shr,
                ">>>" => CilOpCodes.Shr_Un,
                _ => throw new NotSupportedException($"asm.js binary op '{b.Op}'"),
            };

            return new JsVal(new Lifting.BinaryExpr(cilOp, left.Expr, right.Expr, outKind), outKind);
        }

        private JsVal? EmitCondExpr(CondExpr c, bool discard)
        {
            if (TryEvalConstInt(c.Condition, out var constCond))
                return EmitExpr(constCond != 0 ? c.Then : c.Else, discard);

            var elseLbl = new CilInstructionLabel();
            var endLbl = new CilInstructionLabel();
            EmitBranchIfFalse(c.Condition, elseLbl);
            var tVal = EmitExpr(c.Then, discard);
            if (discard)
            {
                _ctx.IL.Add(CilOpCodes.Br, endLbl);
                elseLbl.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                EmitExpr(c.Else, discard: true);
                endLbl.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
                return null;
            }
            var tType = tVal is not null ? PickStorageType(tVal, null) : _f.Int32;
            var resLoc = NewLocal(tType);
            if (tVal is not null)
            {
                Expr.EmitAs(_ctx, tVal.Expr, resLoc.VariableType);
                _ctx.IL.Add(CilOpCodes.Stloc, resLoc);
            }
            _ctx.IL.Add(CilOpCodes.Br, endLbl);
            elseLbl.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            var eVal = EmitExpr(c.Else, discard: false);
            if (eVal is not null)
            {
                Expr.EmitAs(_ctx, eVal.Expr, resLoc.VariableType);
                _ctx.IL.Add(CilOpCodes.Stloc, resLoc);
            }
            endLbl.Instruction = _ctx.IL.Add(CilOpCodes.Nop);
            return new JsVal(new LocalExpr(resLoc), Expr.KindOf(resLoc.VariableType));
        }

        private void EmitBranchIfFalse(AsmJsExpr cond, CilInstructionLabel falseTarget)
        {
            if (TryEvalConstInt(cond, out var cVal))
            {
                if (cVal == 0) _ctx.IL.Add(CilOpCodes.Br, falseTarget);
                return;
            }
            if (cond is UnaryExpr { Op: "!", Operand: var inner })
            {
                EmitBranchIfTrue(inner, falseTarget);
                return;
            }
            var val = EmitExpr(cond, discard: false);
            if (val is null)
            {
                _ctx.IL.Add(CilOpCodes.Br, falseTarget);
                return;
            }
            if (val.Expr is CompareExpr cmp)
            {
                if (cmp.Condition == Condition.Ne && cmp.Right is ConstExpr { Value: 0 })
                {
                    Expr.EmitAs(_ctx, cmp.Left, cmp.Left.Kind == ValueKind.Ref ? _f.Object : _f.Int32);
                    _ctx.IL.Add(CilOpCodes.Brfalse, falseTarget);
                    return;
                }
                if (cmp.Condition == Condition.Eq && cmp.Right is ConstExpr { Value: 0 })
                {
                    Expr.EmitAs(_ctx, cmp.Left, cmp.Left.Kind == ValueKind.Ref ? _f.Object : _f.Int32);
                    _ctx.IL.Add(CilOpCodes.Brtrue, falseTarget);
                    return;
                }
            }
            val.Expr.Emit(_ctx);
            _ctx.IL.Add(CilOpCodes.Brfalse, falseTarget);
        }

        private void EmitBranchIfTrue(AsmJsExpr cond, CilInstructionLabel trueTarget)
        {
            if (TryEvalConstInt(cond, out var cVal))
            {
                if (cVal != 0) _ctx.IL.Add(CilOpCodes.Br, trueTarget);
                return;
            }
            if (cond is UnaryExpr { Op: "!", Operand: var inner })
            {
                EmitBranchIfFalse(inner, trueTarget);
                return;
            }
            var val = EmitExpr(cond, discard: false);
            if (val is null) return;
            val.Expr.Emit(_ctx);
            _ctx.IL.Add(CilOpCodes.Brtrue, trueTarget);
        }

        // ------------------------------------------------------------------ heap load/store

        private (Expr Base, int Disp, int Size, HeapViewKind View) DecodeHeapAccess(IndexExpr ix)
        {
            if (ix.Target is not VarExpr arr || !_asmJs.HeapViews.TryGetValue(arr.Name, out var view))
                throw new InvalidOperationException($"Unsupported heap target '{ix.Target}'");

            var shift = ViewShift(view);
            var size = 1 << shift;

            var addrExpr = ix.Index;
            // Unwrap (addr >> shift) or (addr >>> shift) when shift matches view element size
            if (shift > 0 && addrExpr is BinaryExpr { Op: ">>" or ">>>", Left: var innerAddr, Right: IntLitExpr rs } && rs.Value == shift)
            {
                var baseVal = EmitExpr(innerAddr, discard: false) ?? new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
                return (baseVal.Expr, baseVal.Disp, size, view);
            }

            if (TryEvalConstInt(addrExpr, out var constIdx))
            {
                var byteAddr = unchecked((int)((uint)constIdx << shift));
                return (new ConstExpr(byteAddr, ValueKind.I4), 0, size, view);
            }

            var dynamicIdx = EmitExpr(addrExpr, discard: false) ?? new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);
            if (dynamicIdx.Expr is ConstExpr dc)
            {
                var byteAddr = unchecked((int)((uint)dc.Value << shift));
                return (new ConstExpr(byteAddr, ValueKind.I4), 0, size, view);
            }
            if (shift == 0)
                return (dynamicIdx.Expr, dynamicIdx.Disp, size, view);

            // Scaled heap base address: base + (index << shift)
            return (new Lifting.BinaryExpr(CilOpCodes.Shl, dynamicIdx.Expr, new ConstExpr(shift, ValueKind.I4), ValueKind.I4), 0, size, view);
        }

        private JsVal EmitHeapLoad(IndexExpr ix)
        {
            var (baseExpr, disp, size, view) = DecodeHeapAccess(ix);
            var viewKind = KindOfView(view);

            if (baseExpr is ConstExpr constAddr && disp == 0)
            {
                var addr32 = unchecked((int)constAddr.Value);
                if (_globalUsages.TryGetValue(addr32, out var gu))
                    return new JsVal(ExprForUsage(gu), ValueKind.I4);
                if (_sym.TryGetUsage((ulong)(uint)addr32, out var ru))
                    return new JsVal(ExprForUsage(ru), ValueKind.I4);
            }

            if (baseExpr is FrameAddrExpr fa)
            {
                var loaded = FrameLoad(fa.Offset + disp, size, view);
                return new JsVal(loaded, loaded.Kind);
            }

            Expr? resolved = null;
            if (baseExpr is IndirectStructExpr || baseExpr is AddressExpr || baseExpr.IsMarker
                || (baseExpr.Type is { } t && (Expr.KindOf(t) is ValueKind.Ref or ValueKind.Struct)))
            {
                resolved = _mem.Load(baseExpr.Type is not null && Expr.KindOf(baseExpr.Type) == ValueKind.Struct && baseExpr is not IndirectStructExpr && baseExpr is not AddressExpr
                    ? new AddressExpr(baseExpr) : baseExpr, null, 0, disp, size);
            }
            else
            {
                resolved = _mem.Load(baseExpr, null, 0, disp, size);
            }

            if (resolved is not null)
                return new JsVal(resolved, resolved.Kind);

            // Fallback: typed scalar read if pointer/address couldn't resolve to a CLR field
            var native = new NativeExpr($"mem[{baseExpr.Describe()}+0x{disp:X}:{size}]", viewKind);
            Fallbacks++;
            return new JsVal(native, viewKind);
        }

        private void EmitHeapStore(IndexExpr ix, JsVal value)
        {
            var (baseExpr, disp, size, view) = DecodeHeapAccess(ix);

            if (baseExpr is FrameAddrExpr fa)
            {
                FrameStore(fa.Offset + disp, size, view, value);
                return;
            }

            var targetBase = baseExpr.Type is { } t && Expr.KindOf(t) == ValueKind.Struct
                && baseExpr is not IndirectStructExpr && baseExpr is not AddressExpr
                ? new AddressExpr(baseExpr)
                : baseExpr;

            if (_mem.Store(_ctx, targetBase, null, 0, disp, size, value.Expr))
                return;

            if (baseExpr is AddressExpr addr && disp == 0)
            {
                addr.Emit(_ctx);
                value.Expr.Emit(_ctx);
                IndirectExpr.EmitStore(_ctx, addr.Target.Type ?? _f.Int32);
                return;
            }

            Fallbacks++;
            _ctx.IL.Add(CilOpCodes.Ldstr, $"*({baseExpr.Describe()}+0x{disp:X}) = {value.Expr.Describe()}");
            _ctx.IL.Add(CilOpCodes.Call, _stub.Intrinsics.Stmt);
        }

        // ------------------------------------------------------------------ frame slots & struct reconstruction

        private FrameSlot? StructSlotCovering(int offset) =>
            _slots.Values.FirstOrDefault(s => s.StructType is not null && offset >= s.Offset && offset < s.Offset + s.Size);

        private Expr FrameLoad(int offset, int size, HeapViewKind view)
        {
            if (StructSlotCovering(offset) is { } covering)
            {
                var rel = offset - covering.Offset;
                if (_mem.Load(new AddressExpr(new LocalExpr(covering.Local!)), null, 0, rel, size) is { } loaded)
                    return loaded;
            }
            if (_slots.TryGetValue(offset, out var ex) && ex.StructType is null && ex.Size == size && ex.Local is not null)
            {
                if (ex.ConstBits is { } cb && view == HeapViewKind.Float32)
                    return new FloatExpr(BitConverter.Int32BitsToSingle(cb), single: true);
                return new LocalExpr(ex.Local);
            }
            var loc = NewLocal(TypeOfView(view));
            _slots[offset] = new FrameSlot(offset) { Size = size, Local = loc, ScalarUsed = true };
            return new LocalExpr(loc);
        }

        private void FrameStore(int offset, int size, HeapViewKind view, JsVal value)
        {
            if (StructSlotCovering(offset) is { } covering)
            {
                if (_mem.Store(_ctx, new AddressExpr(new LocalExpr(covering.Local!)), null, 0, offset - covering.Offset, size, value.Expr))
                    return;
            }

            var wantType = value.Expr.Type is { } et && Expr.KindOf(et) is ValueKind.R4 or ValueKind.R8 or ValueKind.Ref
                ? et
                : value.Expr.Kind == ValueKind.R4 ? _f.Single
                : value.Expr.Kind == ValueKind.R8 ? _f.Double
                : TypeOfView(view);

            if (!_slots.TryGetValue(offset, out var slot) || slot.StructType is not null || slot.Size != size
                || !SignatureComparer.Default.Equals(slot.Local!.VariableType, wantType))
            {
                slot = new FrameSlot(offset) { Size = size, Local = NewLocal(wantType), ScalarUsed = true };
                _slots[offset] = slot;
            }
            slot.ConstBits = value.Expr is ConstExpr c ? unchecked((int)c.Value) : null;
            Expr.EmitAs(_ctx, value.Expr, slot.Local!.VariableType);
            _ctx.IL.Add(CilOpCodes.Stloc, slot.Local);
        }

        private Expr StructLValue(int offset, TypeSignature type, bool preserveExisting)
        {
            if (StructSlotCovering(offset) is { } covering)
            {
                var rel = offset - covering.Offset;
                if (rel == 0 && SignatureComparer.Default.Equals(covering.StructType, type))
                    return new LocalExpr(covering.Local!);
                if (rel > 0 && _mem.StructField(new LocalExpr(covering.Local!), _mem.TypeDefOf(covering.StructType!), rel) is FieldExpr fe)
                    return fe;
            }

            var size = _mem.ValueSize(type);
            var overlapping = _slots.Where(kv => kv.Value.Offset < offset + size && offset < kv.Value.Offset + kv.Value.Size).ToList();
            var preserved = preserveExisting
                ? overlapping.Select(kv => kv.Value).Where(s => s.StructType is null && s.ScalarUsed && s.Local is not null).OrderBy(s => s.Offset).ToList()
                : [];
            foreach (var kv in overlapping) _slots.Remove(kv.Key);

            var state = new FrameSlot(offset) { Size = size, StructType = type, Local = NewLocal(type) };
            _slots[offset] = state;

            if (preserved.Count > 0)
            {
                var destTd = _mem.TypeDefOf(type);
                var localExpr = new LocalExpr(state.Local);
                foreach (var s in preserved)
                {
                    var rel = s.Offset - offset;
                    if (rel < 0 || rel >= size) continue;
                    var destField = destTd >= 0 ? _mem.StructField(localExpr, destTd, rel) : null;
                    Expr srcExpr = s.ConstBits is { } cb && (destField?.Type is { } dt ? Expr.KindOf(dt) is ValueKind.R4 or ValueKind.R8 or ValueKind.I4 : Expr.KindOf(s.Local!.VariableType) is ValueKind.I4 or ValueKind.I8)
                        ? new ConstExpr(cb, ValueKind.I4, true)
                        : new LocalExpr(s.Local!);
                    _mem.Store(_ctx, new AddressExpr(localExpr), null, 0, rel, s.Size, srcExpr);
                }
            }
            return new LocalExpr(state.Local);
        }

        // ------------------------------------------------------------------ calls

        private JsVal? EmitCall(CallExpr call, bool discard)
        {
            if (call.Callee is VarExpr fnVar)
            {
                // 1. Math.fround (ea)
                if (fnVar.Name == _asmJs.FroundVar && call.Args.Count >= 1)
                {
                    var arg = EmitExpr(call.Args[0], discard: false) ?? new JsVal(new FloatExpr(0f, true), ValueKind.R4);
                    if (discard) return null;
                    if (arg.Expr is FloatExpr fl)
                        return new JsVal(new FloatExpr((float)fl.Value, single: true), ValueKind.R4);
                    if (arg.Expr is ConstExpr cl)
                        return new JsVal(new FloatExpr(cl.Value, single: true), ValueKind.R4);
                    if (arg.Expr.Kind == ValueKind.R4)
                        return arg;
                    if (arg.Expr is ConvExpr cv && cv.Operand.Kind == ValueKind.R4)
                        return new JsVal(cv.Operand, ValueKind.R4);
                    return new JsVal(new ConvExpr(arg.Expr, _f.Single), ValueKind.R4);
                }

                // 2. Math.* aliases
                if (_asmJs.MathAliases.TryGetValue(fnVar.Name, out var mathName))
                {
                    var mathArgs = call.Args.Select(a => EmitExpr(a, discard: false)?.Expr ?? new FloatExpr(0d, false)).ToArray();
                    if (discard) return null;
                    if (mathName == "imul" && mathArgs.Length >= 2)
                        return new JsVal(new Lifting.BinaryExpr(CilOpCodes.Mul, mathArgs[0], mathArgs[1], ValueKind.I4), ValueKind.I4);
                    var pascal = mathName switch
                    {
                        "floor" => "Floor",
                        "ceil" => "Ceiling",
                        "abs" => "Abs",
                        "sqrt" => "Sqrt",
                        "pow" => "Pow",
                        "cos" => "Cos",
                        "sin" => "Sin",
                        "tan" => "Tan",
                        "acos" => "Acos",
                        "asin" => "Asin",
                        "atan" => "Atan",
                        "atan2" => "Atan2",
                        "exp" => "Exp",
                        "log" => "Log",
                        "min" => "Min",
                        "max" => "Max",
                        _ => null,
                    };
                    if (pascal is not null)
                        return new JsVal(new MathCallExpr(pascal, _f.Double, mathArgs), ValueKind.R8);
                }

                // 3. v16 Metadata / ClassInit / NullCheck helpers
                var helper = _asmJs.ClassifyHelper(fnVar.Name);
                if (helper is AsmJsHelperKind.ClassInit or AsmJsHelperKind.NullCheck)
                    return null;
                if (helper is AsmJsHelperKind.V16MethodInfo or AsmJsHelperKind.V16TypeInfo or AsmJsHelperKind.V16StringLiteral)
                {
                    if (call.Args.Count >= 1 && TryEvalConstInt(call.Args[0], out var idx)
                        && TryUsageFromHelper(helper, unchecked((int)idx), out var usage))
                    {
                        return new JsVal(ExprForUsage(usage), ValueKind.I4);
                    }
                }

                // 4. Direct managed / native function in module
                if (_asmJs.TryGetFuncVa(fnVar.Name, out var targetVa))
                    return EmitDirectOrIndirectTargetCall(targetVa, fnVar.Name, call.Args, discard);
            }
            else if (call.Callee is IndexExpr { Target: VarExpr tableVar } tableIdx)
            {
                var unwrapped = UnwrapMask(tableIdx.Index);
                if (TryEvalConstInt(unwrapped, out var slot)
                    && _asmJs.TryResolveIndirectTable(tableVar.Name, unchecked((int)slot), out var targetVa))
                {
                    return EmitDirectOrIndirectTargetCall(targetVa, $"{tableVar.Name}[{slot}]", call.Args, discard);
                }
            }

            // Evaluate args so side effects aren't dropped, then emit fallback CallNative
            var rawArgs = new List<Expr>(call.Args.Count);
            foreach (var a in call.Args)
                rawArgs.Add(EmitExpr(a, discard: false)?.Expr ?? new ConstExpr(0, ValueKind.I4));
            UnresolvedCalls++;
            Fallbacks++;
            return EmitCallNativeFallback(call.Callee.ToString() ?? "fn", rawArgs, discard);
        }

        private JsVal? EmitDirectOrIndirectTargetCall(ulong targetVa, string calleeLabel, IReadOnlyList<AsmJsExpr> rawAstArgs, bool discard)
        {
            var evalArgs = new JsVal[rawAstArgs.Count];
            for (var i = 0; i < rawAstArgs.Count; i++)
                evalArgs[i] = EmitExpr(rawAstArgs[i], discard: false) ?? new JsVal(new ConstExpr(0, ValueKind.I4), ValueKind.I4);

            var candidates = _sym.Map.CandidatesAt(targetVa);
            if (candidates.Count == 0)
            {
                // Non-metadata helper passed a TypeInfo/MethodInfo/Il2CppType marker (ClassInit / NullCheck / MetadataInit)
                if (evalArgs.Length == 1 && evalArgs[0].Expr is MarkerExpr { Marker: MarkerKind.TypeInfo or MarkerKind.Il2CppType or MarkerKind.MethodInfo })
                {
                    ElidedChecks++;
                    return null;
                }
                UnresolvedCalls++;
                Fallbacks++;
                return EmitCallNativeFallback(calleeLabel, evalArgs.Select(e => e.Expr).ToList(), discard);
            }

            // Determine receiver info for ICF disambiguation
            var c0 = candidates[0];
            var shape0 = MethodShape.Create(_md, _reg, _stubs, _mem, _module, c0.MethodIndex, c0.MethodSpecIndex, forCallSite: true);
            Expr? recvCandidate = null;
            int recvTd = -1;
            if (!shape0.IsStatic)
            {
                var thisArgPos = shape0.HasReturnBuffer ? 1 : 0;
                if (thisArgPos < evalArgs.Length)
                {
                    recvCandidate = evalArgs[thisArgPos].Expr;
                    recvTd = recvCandidate switch
                    {
                        NewObjExpr n => _mem.TypeDefOf(n.Type),
                        AddressExpr a => _mem.TypeDefOf(a.Target.Type),
                        _ => _mem.TypeDefOf(recvCandidate.Type),
                    };
                }
            }

            var chosen = IcfResolver.Choose(
                candidates,
                recvTd,
                _md,
                _reg,
                _sym.Names,
                log: _ctx.InfoLog,
                receiverIsNewObject: recvCandidate is NewObjExpr,
                receiverType: recvCandidate?.Type,
                stubs: _stubs,
                module: _module);

            var chosenMethodIndex = chosen.MethodIndex;
            var specIndex = chosen.MethodSpecIndex;

            // Check trailing const MethodInfo* for exact generic instantiation (GetComponent<Renderer>(), etc.)
            if (evalArgs.Length > 0 && evalArgs[^1].Expr is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { Kind: MetadataUsageKind.MethodRef } mu }
                && (uint)mu.Index < (uint)_reg.MethodSpecs.Count
                && _reg.MethodSpecs[mu.Index].MethodDefinitionIndex == chosenMethodIndex)
            {
                specIndex = mu.Index;
            }

            var shape = MethodShape.Create(_md, _reg, _stubs, _mem, _module, chosenMethodIndex, specIndex, forCallSite: true);
            var target = _stubs.MethodRefFor(_module, chosenMethodIndex, specIndex);
            if (target is null)
            {
                UnresolvedCalls++;
                Fallbacks++;
                return EmitCallNativeFallback(calleeLabel, evalArgs.Select(e => e.Expr).ToList(), discard);
            }

            var jsArgIdx = 0;
            JsVal? retPtrVal = null;
            if (shape.HasReturnBuffer)
            {
                if (jsArgIdx < evalArgs.Length) retPtrVal = evalArgs[jsArgIdx++];
            }

            JsVal? thisVal = null;
            if (!shape.IsStatic)
            {
                if (jsArgIdx < evalArgs.Length) thisVal = evalArgs[jsArgIdx++];
                if (thisVal is not null)
                    CallEmitter.RebindReceiverGeneric(ref target, ref shape, thisVal.Expr, _md, _reg, _stubs, _mem, _module, chosenMethodIndex, specIndex);
            }
            else if (_md.Version.StaticMethodsTakeDummyThis)
            {
                if (jsArgIdx < evalArgs.Length) jsArgIdx++;
            }

            // Emit `this`
            if (!shape.IsStatic && thisVal is not null)
            {
                var baseTarget = target is MethodSpecification ms ? ms.Method : target;
                var thisTargetType = baseTarget is MemberReference { Parent: TypeSpecification { Signature: { } ts } }
                    ? ts
                    : _stubs.TypeRefFor(_module, _md.Methods[chosenMethodIndex].DeclaringType)?.ToTypeSignature(shape.DeclaringIsValueType)
                    ?? _f.Object;
                if (shape.DeclaringIsValueType)
                {
                    if (thisVal.Expr is FrameAddrExpr fa)
                    {
                        var lv = StructLValue(fa.Offset, thisTargetType, preserveExisting: true);
                        _ctx.IL.Add(CilOpCodes.Ldloca, ((LocalExpr)lv).Local);
                    }
                    else if (thisVal.Expr is LocalExpr le && Expr.KindOf(le.Type) == ValueKind.Struct)
                    {
                        _ctx.IL.Add(CilOpCodes.Ldloca, le.Local);
                    }
                    else if (thisVal.Expr is AddressExpr ae)
                    {
                        ae.Emit(_ctx);
                    }
                    else
                    {
                        thisVal.Expr.Emit(_ctx);
                    }
                }
                else
                {
                    Expr.EmitAs(_ctx, thisVal.Expr, thisTargetType);
                }
            }

            // Emit managed arguments
            foreach (var pType in shape.Parameters)
            {
                if (jsArgIdx >= evalArgs.Length)
                {
                    EmitDefaultValue(pType);
                    continue;
                }

                if (pType is ByReferenceTypeSignature br)
                {
                    var arg = evalArgs[jsArgIdx++];
                    EmitByRefArg(arg, br.BaseType);
                }
                else if (MethodShape.PassedByPointer(pType, _mem))
                {
                    var arg = evalArgs[jsArgIdx++];
                    EmitStructArg(arg, pType);
                }
                else if (Expr.KindOf(pType) == ValueKind.I8 && jsArgIdx + 1 < evalArgs.Length)
                {
                    var lo = evalArgs[jsArgIdx++];
                    var hi = evalArgs[jsArgIdx++];
                    Expr.EmitAs(_ctx, new FramePairExpr(lo.Expr, hi.Expr), pType);
                }
                else
                {
                    var arg = evalArgs[jsArgIdx++];
                    Expr.EmitAs(_ctx, arg.Expr, pType);
                }
            }

            _ctx.IL.Add(CilOpCodes.Call, target);

            // Handle return value / return buffer
            var retKind = Expr.KindOf(shape.ReturnType);
            if (retKind == ValueKind.Void)
                return null;

            if (shape.HasReturnBuffer && retPtrVal is not null)
            {
                if (retPtrVal.Expr is FrameAddrExpr rfa)
                {
                    var lv = (LocalExpr)StructLValue(rfa.Offset, shape.ReturnType, preserveExisting: false);
                    _ctx.IL.Add(CilOpCodes.Stloc, lv.Local);
                    return discard ? null : new JsVal(lv, ValueKind.Struct);
                }
                if (retPtrVal.Expr is AddressExpr { Target: LocalExpr rl })
                {
                    _ctx.IL.Add(CilOpCodes.Stloc, rl.Local);
                    return discard ? null : new JsVal(rl, ValueKind.Struct);
                }
                var tmpBuf = NewLocal(shape.ReturnType);
                _ctx.IL.Add(CilOpCodes.Stloc, tmpBuf);
                return discard ? null : new JsVal(new LocalExpr(tmpBuf), ValueKind.Struct);
            }

            if (discard)
            {
                _ctx.IL.Add(CilOpCodes.Pop);
                return null;
            }

            var resLocal = NewLocal(shape.ReturnType);
            _ctx.IL.Add(CilOpCodes.Stloc, resLocal);
            return new JsVal(new LocalExpr(resLocal), Expr.KindOf(shape.ReturnType));
        }

        private void EmitByRefArg(JsVal arg, TypeSignature elemType)
        {
            if (arg.Expr is FrameAddrExpr fa)
            {
                if (Expr.KindOf(elemType) == ValueKind.Struct)
                {
                    var lv = (LocalExpr)StructLValue(fa.Offset, elemType, preserveExisting: true);
                    _ctx.IL.Add(CilOpCodes.Ldloca, lv.Local);
                    return;
                }
                if (_slots.TryGetValue(fa.Offset, out var slot) && slot.Local is not null)
                {
                    _ctx.IL.Add(CilOpCodes.Ldloca, slot.Local);
                    return;
                }
                var tmp = NewLocal(elemType);
                _slots[fa.Offset] = new FrameSlot(fa.Offset) { Size = _mem.ValueSize(elemType), Local = tmp, ScalarUsed = true };
                _ctx.IL.Add(CilOpCodes.Ldloca, tmp);
                return;
            }
            if (arg.Expr is LocalExpr le)
            {
                _ctx.IL.Add(CilOpCodes.Ldloca, le.Local);
                return;
            }
            arg.Expr.Emit(_ctx);
        }

        private void EmitStructArg(JsVal arg, TypeSignature structType)
        {
            if (arg.Expr is FrameAddrExpr fa)
            {
                var lv = StructLValue(fa.Offset, structType, preserveExisting: true);
                Expr.EmitAs(_ctx, lv, structType);
                return;
            }
            if (arg.Expr is AddressExpr ae && ae.Target.Type is not null && SignatureComparer.Default.Equals(ae.Target.Type, structType))
            {
                ae.Target.Emit(_ctx);
                return;
            }
            if (arg.Expr is IndirectStructExpr ise)
            {
                ise.Emit(_ctx);
                return;
            }
            Expr.EmitAs(_ctx, arg.Expr, structType);
        }

        private JsVal? EmitCallNativeFallback(string callee, List<Expr> args, bool discard)
        {
            _ctx.IL.Add(CilOpCodes.Ldstr, callee);
            for (var i = 0; i < 4; i++)
            {
                if (i < args.Count) Expr.EmitAs(_ctx, args[i], _f.Int64);
                else _ctx.IL.Add(CilOpCodes.Ldc_I8, 0L);
            }
            _ctx.IL.Add(CilOpCodes.Call, _stub.Intrinsics.CallNative);
            if (discard)
            {
                _ctx.IL.Add(CilOpCodes.Pop);
                return null;
            }
            var loc = NewLocal(_f.Int64);
            _ctx.IL.Add(CilOpCodes.Stloc, loc);
            return new JsVal(new LocalExpr(loc), ValueKind.I8);
        }

        private void EmitReturnValue(AsmJsExpr? retExpr)
        {
            var retType = _shape.ReturnType;
            if (_shape.HasReturnBuffer && _retBuf is not null)
            {
                _ctx.IL.Add(CilOpCodes.Ldloc, _retBuf);
                _ctx.IL.Add(CilOpCodes.Ret);
                return;
            }
            if (Expr.KindOf(retType) == ValueKind.Void)
            {
                if (retExpr is not null) EmitExpr(retExpr, discard: true);
                _ctx.IL.Add(CilOpCodes.Ret);
                return;
            }
            if (retExpr is null)
            {
                EmitDefaultValue(retType);
                _ctx.IL.Add(CilOpCodes.Ret);
                return;
            }
            var v = EmitExpr(retExpr, discard: false);
            if (v is null) EmitDefaultValue(retType);
            else Expr.EmitAs(_ctx, v.Expr, retType);
            _ctx.IL.Add(CilOpCodes.Ret);
        }

        private void EmitDefaultValue(TypeSignature ret)
        {
            switch (Expr.KindOf(ret))
            {
                case ValueKind.R4: _ctx.IL.Add(CilOpCodes.Ldc_R4, 0f); break;
                case ValueKind.R8: _ctx.IL.Add(CilOpCodes.Ldc_R8, 0d); break;
                case ValueKind.I8: _ctx.IL.Add(CilOpCodes.Ldc_I8, 0L); break;
                case ValueKind.Ref: _ctx.IL.Add(CilOpCodes.Ldnull); break;
                case ValueKind.Struct:
                    var loc = NewLocal(ret);
                    _ctx.IL.Add(CilOpCodes.Ldloca, loc);
                    _ctx.IL.Add(CilOpCodes.Initobj, ret.ToTypeDefOrRef());
                    _ctx.IL.Add(CilOpCodes.Ldloc, loc);
                    break;
                default: _ctx.IL.Add(CilOpCodes.Ldc_I4_0); break;
            }
        }

        // ------------------------------------------------------------------ small utilities

        private CilLocalVariable NewLocal(TypeSignature type)
        {
            var loc = new CilLocalVariable(type);
            _def.CilMethodBody!.LocalVariables.Add(loc);
            return loc;
        }

        private static int ViewShift(HeapViewKind view) => view switch
        {
            HeapViewKind.Int8 or HeapViewKind.Uint8 => 0,
            HeapViewKind.Int16 or HeapViewKind.Uint16 => 1,
            HeapViewKind.Int32 or HeapViewKind.Uint32 or HeapViewKind.Float32 => 2,
            HeapViewKind.Float64 => 3,
            _ => 2,
        };

        private static ValueKind KindOfView(HeapViewKind view) => view switch
        {
            HeapViewKind.Float32 => ValueKind.R4,
            HeapViewKind.Float64 => ValueKind.R8,
            _ => ValueKind.I4,
        };

        private TypeSignature TypeOfView(HeapViewKind view) => view switch
        {
            HeapViewKind.Float32 => _f.Single,
            HeapViewKind.Float64 => _f.Double,
            _ => _f.Int32,
        };

        private BreakTarget? FindBreakTarget(string? label)
        {
            if (label is not null)
                return _labeledTargets.GetValueOrDefault(label);
            return _breakStack.Count > 0 ? _breakStack[^1] : null;
        }

        private BreakTarget? FindContinueTarget(string? label)
        {
            if (label is not null)
                return _labeledTargets.TryGetValue(label, out var t) && t.ContinueLabel is not null ? t : null;
            for (var i = _breakStack.Count - 1; i >= 0; i--)
                if (_breakStack[i].ContinueLabel is not null) return _breakStack[i];
            return null;
        }

        private static AsmJsExpr StripIntCoercion(AsmJsExpr e) =>
            e is BinaryExpr { Op: "|", Left: var inner, Right: IntLitExpr { Value: 0 } } ? inner : e;

        private static AsmJsExpr UnwrapMask(AsmJsExpr e) =>
            e is BinaryExpr { Op: "&", Left: var inner, Right: IntLitExpr } ? inner : e;

        private static bool IsUnsignedExpr(AsmJsExpr left, AsmJsExpr right) =>
            left is BinaryExpr { Op: ">>>" } || right is BinaryExpr { Op: ">>>" };

        private static Condition NegateCondition(Condition c) => c switch
        {
            Condition.Eq => Condition.Ne,
            Condition.Ne => Condition.Eq,
            Condition.Lt => Condition.Ge,
            Condition.Ge => Condition.Lt,
            Condition.Gt => Condition.Le,
            Condition.Le => Condition.Gt,
            Condition.LtUn => Condition.GeUn,
            Condition.GeUn => Condition.LtUn,
            Condition.GtUn => Condition.LeUn,
            Condition.LeUn => Condition.GtUn,
            _ => Condition.Eq,
        };

        private static bool TryEvalConstInt(AsmJsExpr e, out long value)
        {
            switch (e)
            {
                case IntLitExpr il:
                    value = il.Value;
                    return true;
                case UnaryExpr { Op: "-", Operand: var op } when TryEvalConstInt(op, out var v):
                    value = -v;
                    return true;
                case UnaryExpr { Op: "+", Operand: var op } when TryEvalConstInt(op, out var v):
                    value = v;
                    return true;
                case UnaryExpr { Op: "~", Operand: var op } when TryEvalConstInt(op, out var v):
                    value = ~unchecked((int)v);
                    return true;
                case BinaryExpr { Op: "|", Left: var l, Right: IntLitExpr { Value: 0 } } when TryEvalConstInt(l, out var v):
                    value = unchecked((int)v);
                    return true;
                case BinaryExpr { Op: "+", Left: var l, Right: var r } when TryEvalConstInt(l, out var lv) && TryEvalConstInt(r, out var rv):
                    value = unchecked((int)lv + (int)rv);
                    return true;
                case BinaryExpr { Op: "-", Left: var l, Right: var r } when TryEvalConstInt(l, out var lv) && TryEvalConstInt(r, out var rv):
                    value = unchecked((int)lv - (int)rv);
                    return true;
                case BinaryExpr { Op: "<<", Left: var l, Right: var r } when TryEvalConstInt(l, out var lv) && TryEvalConstInt(r, out var rv):
                    value = unchecked((int)lv << ((int)rv & 31));
                    return true;
                case BinaryExpr { Op: ">>", Left: var l, Right: var r } when TryEvalConstInt(l, out var lv) && TryEvalConstInt(r, out var rv):
                    value = unchecked((int)lv >> ((int)rv & 31));
                    return true;
                case BinaryExpr { Op: "&", Left: var l, Right: var r } when TryEvalConstInt(l, out var lv) && TryEvalConstInt(r, out var rv):
                    value = unchecked((int)lv & (int)rv);
                    return true;
                default:
                    value = 0;
                    return false;
            }
        }

        private static bool SameJsVal(JsVal a, JsVal b) =>
            a.Disp == b.Disp && a.Kind == b.Kind &&
            (ReferenceEquals(a.Expr, b.Expr)
             || (a.Expr is FrameAddrExpr fa && b.Expr is FrameAddrExpr fb && fa.Offset == fb.Offset)
             || (a.Expr is ConstExpr ca && b.Expr is ConstExpr cb && ca.Value == cb.Value)
             || (a.Expr is LocalExpr la && b.Expr is LocalExpr lb && la.Local == lb.Local)
             || (a.Expr is ArgExpr aa && b.Expr is ArgExpr ab && aa.Parameter == ab.Parameter));

        private static void ReplaceVars(Dictionary<string, JsVal> dst, Dictionary<string, JsVal> src)
        {
            dst.Clear();
            foreach (var (k, v) in src) dst[k] = v;
        }

        private static void CollectVarWrites(AsmJsStmt stmt, HashSet<string> writes)
        {
            switch (stmt)
            {
                case BlockStmt b: foreach (var s in b.Statements) CollectVarWrites(s, writes); break;
                case ExprStmt e: CollectExprVarWrites(e.Expr, writes); break;
                case IfStmt ifs:
                    CollectExprVarWrites(ifs.Condition, writes);
                    CollectVarWrites(ifs.Then, writes);
                    if (ifs.Else is not null) CollectVarWrites(ifs.Else, writes);
                    break;
                case WhileStmt ws: CollectExprVarWrites(ws.Condition, writes); CollectVarWrites(ws.Body, writes); break;
                case DoWhileStmt dws: CollectVarWrites(dws.Body, writes); CollectExprVarWrites(dws.Condition, writes); break;
                case ForStmt fs:
                    if (fs.Init is not null) CollectExprVarWrites(fs.Init, writes);
                    if (fs.Condition is not null) CollectExprVarWrites(fs.Condition, writes);
                    if (fs.Step is not null) CollectExprVarWrites(fs.Step, writes);
                    CollectVarWrites(fs.Body, writes);
                    break;
                case SwitchStmt sw:
                    CollectExprVarWrites(sw.Discriminant, writes);
                    foreach (var c in sw.Cases) foreach (var s in c.Body) CollectVarWrites(s, writes);
                    break;
                case LabeledStmt ls: CollectVarWrites(ls.Statement, writes); break;
                case ReturnStmt { Value: { } rv }: CollectExprVarWrites(rv, writes); break;
            }
        }

        private static void CollectExprVarWrites(AsmJsExpr expr, HashSet<string> writes)
        {
            switch (expr)
            {
                case AssignExpr a:
                    if (a.Target is VarExpr v) writes.Add(v.Name);
                    else CollectExprVarWrites(a.Target, writes);
                    CollectExprVarWrites(a.Value, writes);
                    break;
                case UnaryExpr u: CollectExprVarWrites(u.Operand, writes); break;
                case BinaryExpr b: CollectExprVarWrites(b.Left, writes); CollectExprVarWrites(b.Right, writes); break;
                case CondExpr c: CollectExprVarWrites(c.Condition, writes); CollectExprVarWrites(c.Then, writes); CollectExprVarWrites(c.Else, writes); break;
                case IndexExpr ix: CollectExprVarWrites(ix.Target, writes); CollectExprVarWrites(ix.Index, writes); break;
                case CallExpr call:
                    CollectExprVarWrites(call.Callee, writes);
                    foreach (var arg in call.Args) CollectExprVarWrites(arg, writes);
                    break;
            }
        }

        private static HashSet<string> CollectVarReads(IEnumerable<AsmJsStmt> stmts)
        {
            var reads = new HashSet<string>(StringComparer.Ordinal);
            foreach (var s in stmts) CollectStmtReads(s, reads);
            return reads;
        }

        private static void CollectStmtReads(AsmJsStmt stmt, HashSet<string> reads)
        {
            switch (stmt)
            {
                case BlockStmt b: foreach (var s in b.Statements) CollectStmtReads(s, reads); break;
                case ExprStmt e: CollectExprReads(e.Expr, reads); break;
                case IfStmt ifs:
                    CollectExprReads(ifs.Condition, reads);
                    CollectStmtReads(ifs.Then, reads);
                    if (ifs.Else is not null) CollectStmtReads(ifs.Else, reads);
                    break;
                case WhileStmt ws: CollectExprReads(ws.Condition, reads); CollectStmtReads(ws.Body, reads); break;
                case DoWhileStmt dws: CollectStmtReads(dws.Body, reads); CollectExprReads(dws.Condition, reads); break;
                case ForStmt fs:
                    if (fs.Init is not null) CollectExprReads(fs.Init, reads);
                    if (fs.Condition is not null) CollectExprReads(fs.Condition, reads);
                    if (fs.Step is not null) CollectExprReads(fs.Step, reads);
                    CollectStmtReads(fs.Body, reads);
                    break;
                case SwitchStmt sw:
                    CollectExprReads(sw.Discriminant, reads);
                    foreach (var c in sw.Cases) foreach (var s in c.Body) CollectStmtReads(s, reads);
                    break;
                case LabeledStmt ls: CollectStmtReads(ls.Statement, reads); break;
                case ReturnStmt { Value: { } rv }: CollectExprReads(rv, reads); break;
            }
        }

        private static void CollectExprReads(AsmJsExpr expr, HashSet<string> reads)
        {
            switch (expr)
            {
                case VarExpr v: reads.Add(v.Name); break;
                case AssignExpr a:
                    if (a.Target is IndexExpr assignIx) { CollectExprReads(assignIx.Target, reads); CollectExprReads(assignIx.Index, reads); }
                    CollectExprReads(a.Value, reads);
                    break;
                case UnaryExpr u: CollectExprReads(u.Operand, reads); break;
                case BinaryExpr b: CollectExprReads(b.Left, reads); CollectExprReads(b.Right, reads); break;
                case CondExpr c: CollectExprReads(c.Condition, reads); CollectExprReads(c.Then, reads); CollectExprReads(c.Else, reads); break;
                case IndexExpr ix: CollectExprReads(ix.Target, reads); CollectExprReads(ix.Index, reads); break;
                case CallExpr call:
                    CollectExprReads(call.Callee, reads);
                    foreach (var arg in call.Args) CollectExprReads(arg, reads);
                    break;
            }
        }
    }
}
