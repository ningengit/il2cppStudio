using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Disassembly;

namespace Il2CppStudio.Core.Lifting;

public sealed record LiftResult(int MethodIndex, bool Success, int NativeInstructions, int CilInstructions,
    int NativeFallbacks, int UnresolvedCalls, int ElidedRuntimeChecks, string? Error)
{
    /// <summary>Conversion / fallback notes collected during emission (written to the per-assembly error log).</summary>
    public IReadOnlyList<string> Notes { get; init; } = [];
    public int LoadsMaterialized { get; init; }
}

/// <summary>
/// Lifts one method: disassembles it with exact .pdata bounds, builds the block graph, wires the per-method
/// components (registers, operands, translators, calls, entry/return) and emits one CIL block per native block.
/// </summary>
public sealed class LifterDriver(NativeSymbolizer sym, StubAssemblyBuilder stubs, X64Disassembler disasm, IDiagnosticSink sink,
    Runtime.RuntimeHelperMap? helpers = null, NativeAbi abi = NativeAbi.Win64)
{
    private readonly Dictionary<string, int> _typeIndex = MemoryResolver.BuildTypeIndex(sym.Model);

    public LiftResult Lift(int methodIndex)
    {
        ArgumentLocations.Abi = abi;   // per-thread: argument registers, volatile set, struct passing rules
        var md = sym.Metadata;
        var image = stubs.ImageOfType(md.Methods[methodIndex].DeclaringType);
        if (image < 0) return new(methodIndex, false, 0, 0, 0, 0, 0, "no image");
        var stub = stubs.GetModule(image);   // builds the stub module (and its MethodDefinitions) on first use
        var method = stubs.GetMethodDef(methodIndex);
        var va = sym.Map.GetMethodVa(methodIndex);
        if (method is null || va == 0 || method.IsAbstract) return new(methodIndex, false, 0, 0, 0, 0, 0, "no native code");
        var loc = (ulong ip) => new DiagnosticLocation { Rva = ip - sym.Registration.Reader.ImageBase, MethodName = sym.MethodName(methodIndex) };
        try
        {
            var fn = disasm.Disassemble(va);
            var body = new CilMethodBody { InitializeLocals = true };
            var notes = new List<string>();
            var ctx = new EmitContext(body.Instructions, stub.Module, stub.Intrinsics) { Log = notes.Add };
            var f = stub.Module.CorLibTypeFactory;

            var regs = new RegisterTracker(body, f);
            var slots = new StructSlotTable(regs);
            var mem = new MemoryResolver(md, sym.Model, sym.Registration, sym.Names, stubs, stub.Module, _typeIndex);
            var ops = new OperandReader(regs, slots, mem, sym, f);
            var writer = new OperandWriter(regs, slots, mem);
            var arith = new InstructionTranslators(regs, ops, writer, f, new ScalarIdioms(regs, ops, writer, f, sym.Registration.Reader));
            var branch = new BranchTranslator(regs, ops, arith);
            var binder = new StructBinder(regs, slots, mem);
            var emitter = new CallEmitter(md, sym.Registration, stubs, stub.Module, mem, regs, slots, binder);
            var helperEmitter = helpers is null ? null : new Runtime.RuntimeHelperEmitter(helpers, stubs, stub.Module, sym.Registration, regs, binder);
            var calls = new CallDispatcher(sym, emitter, regs, sink, loc, helperEmitter, helpers) { InConstructor = method.IsConstructor && !method.IsStatic };
            var shape = MethodShape.Create(md, sym.Registration, stubs, mem, stub.Module, methodIndex, -1);
            var returnReg = Expr.KindOf(shape.ReturnType) == ValueKind.Void || shape.HasReturnBuffer
                ? Register.None
                : MethodShape.IsFloat(shape.ReturnType) ? Register.XMM0 : Register.RAX;
            var graph = new BlockGraph(fn.Instructions, sym.Registration, helpers is null ? null : helpers.KindOf, returnReg, ins => CallRegisters(ins, stub.Module, mem));
            var entry = new EntryState(method, shape, regs, mem);
            var returns = new ReturnEmitter(shape, regs, entry);
            var loads = new LoadMaterializer(graph, regs, branch);
            mem.OnFieldStored = (val, fieldExpr) => { regs.ReplaceAll(val, fieldExpr); branch.ReplaceExpr(val, fieldExpr); };
            var blocks = new BlockLifter(graph, regs, arith, branch, calls, returns, b => IsInitPath(graph, b), loads, helpers is null ? null : helpers.KindOf);

            for (var n = 0; n < graph.Blocks.Count; n++)
            {
                var b = graph.Blocks[n];
                if (b.IsPadding) continue;   // unreachable alignment padding
                b.Label.Instruction = body.Instructions.Add(CilOpCodes.Nop);
                regs.EnterBlock(b, isEntry: n == 0);
                if (n == 0) entry.Initialize();
                if (b.IsTrap) { body.Instructions.Add(CilOpCodes.Ldnull); body.Instructions.Add(CilOpCodes.Throw); continue; }
                blocks.Lift(ctx, b, n + 1 < graph.Blocks.Count ? graph.Blocks[n + 1] : null);
            }
            var last = body.Instructions[^1].OpCode.Code;
            if (last is not (CilCode.Ret or CilCode.Throw or CilCode.Br)) returns.EmitDefault(ctx);

            body.Instructions.OptimizeMacros();
            method.CilMethodBody = body;
            stub.LiftedMethods.Add(methodIndex);
            var result = new LiftResult(methodIndex, true, fn.Instructions.Count, body.Instructions.Count,
                blocks.Unsupported, calls.Unresolved, graph.ElidedGuards + branch.ElidedChecks + calls.Elided, null) { Notes = notes, LoadsMaterialized = loads.Materialized };
            sink.Verbose(DiagnosticCategory.Lifting, DiagnosticCodes.LiftCompleted,
                $"{result.NativeInstructions} native -> {result.CilInstructions} CIL, {result.NativeFallbacks} unsupported, " +
                $"{result.UnresolvedCalls} unresolved calls, {result.ElidedRuntimeChecks} runtime checks elided.", loc(va));
            return result;
        }
        catch (Exception ex)
        {
            StubAssemblyBuilder.SetPlaceholderBody(method);
            sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed, $"Lifting failed: {ex.Message}", loc(va), ex);
            return new(methodIndex, false, 0, 0, 0, 0, 0, ex.Message);
        }
    }

    private static Register[] AllCallArgs => ArgumentLocations.AllArgRegisters(ArgumentLocations.Abi);

    private Register[] CallRegisters(in Instruction ins, AsmResolver.DotNet.ModuleDefinition module, MemoryResolver mem)
    {
        if (ins.Op0Kind is not (OpKind.NearBranch64 or OpKind.NearBranch32)) return AllCallArgs;
        var target = ins.NearBranchTarget;
        if (sym.Map.SymbolAt(target) is { } s)
        {
            var mi = s.MethodIndex < 0 && s.MethodSpecIndex >= 0 && s.MethodSpecIndex < sym.Registration.MethodSpecs.Count
                ? sym.Registration.MethodSpecs[s.MethodSpecIndex].MethodDefinitionIndex : s.MethodIndex;
            if ((uint)mi < (uint)sym.Metadata.Methods.Length)
            {
                var sh = MethodShape.Create(sym.Metadata, sym.Registration, stubs, mem, module, mi, s.MethodSpecIndex);
                var regs = new List<Register>();
                for (var p = 0; p < sh.FirstParameterPosition; p++)
                    if (ArgumentLocations.IntRegister(p) is var r && r != Register.None) regs.Add(r);
                for (var i = 0; i < sh.Parameters.Count; i++)
                {
                    var pos = sh.FirstParameterPosition + i;
                    var isFloat = sh.Parameters[i] is not AsmResolver.DotNet.Signatures.ByReferenceTypeSignature
                        && !MethodShape.PassedByPointer(sh.Parameters[i], mem) && MethodShape.IsFloat(sh.Parameters[i]);
                    var r = isFloat ? ArgumentLocations.FloatRegister(pos) : ArgumentLocations.IntRegister(pos);
                    if (r != Register.None) regs.Add(r);
                }
                if (ArgumentLocations.IntRegister(sh.FirstParameterPosition + sh.Parameters.Count) is var mr && mr != Register.None) regs.Add(mr);
                return [.. regs];
            }
        }
        if (helpers is not null)
        {
            switch (helpers.KindOf(target))
            {
                case Runtime.RuntimeHelperKind.MetadataInit or Runtime.RuntimeHelperKind.ClassInit or Runtime.RuntimeHelperKind.ObjectNew
                    or Runtime.RuntimeHelperKind.Raise or Runtime.RuntimeHelperKind.ThrowHelper or Runtime.RuntimeHelperKind.ExceptionFactory
                    or Runtime.RuntimeHelperKind.ScopeCleanup:
                    return [ArgumentLocations.IntArg(0)];
                case Runtime.RuntimeHelperKind.WriteBarrier or Runtime.RuntimeHelperKind.ArrayNew
                    or Runtime.RuntimeHelperKind.Box or Runtime.RuntimeHelperKind.Cast:
                    return [ArgumentLocations.IntArg(0), ArgumentLocations.IntArg(1)];
                case Runtime.RuntimeHelperKind.Unbox or Runtime.RuntimeHelperKind.InterfaceLookup:
                    return [ArgumentLocations.IntArg(0), ArgumentLocations.IntArg(1), ArgumentLocations.IntArg(2)];
            }
            if (helpers.ExportNames.TryGetValue(target, out var name) && Runtime.CrtMath.ArgCount(name) is > 0 and var n)
                return n == 1 ? [Register.XMM0] : [Register.XMM0, Register.XMM1];
        }
        return AllCallArgs;
    }

    /// <summary>Short block whose only call goes to a non-metadata helper (class init / metadata init).</summary>
    private bool IsInitPath(BlockGraph graph, NativeBlock b)
    {
        var live = Enumerable.Range(b.Start, b.End - b.Start).Where(i => !graph.Elided.Contains(i)).Select(i => graph.Instructions[i]).ToList();
        var calls = live.Where(i => i.FlowControl == FlowControl.Call).ToList();
        return live.Count <= 6 && calls.Count > 0 && calls.All(c => sym.Map.SymbolAt(c.NearBranchTarget) is null);
    }
}
