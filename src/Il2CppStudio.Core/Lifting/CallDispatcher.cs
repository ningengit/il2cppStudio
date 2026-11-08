using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Classifies each native <c>call</c>: metadata method (direct or generic instance), runtime plumbing to elide
/// (class/metadata initialization helpers taking an Il2CppClass*/MethodInfo* marker), vtable dispatch, or an
/// unresolved native function (emitted as <c>NativeIntrinsics.CallNative</c> and reported with its RVA).
/// </summary>
public sealed class CallDispatcher(NativeSymbolizer sym, CallEmitter emitter, RegisterTracker regs, IDiagnosticSink sink, Func<ulong, DiagnosticLocation> loc,
    Runtime.RuntimeHelperEmitter? helpers = null, Runtime.RuntimeHelperMap? helperMap = null)
{
    public int Unresolved { get; private set; }
    public int Elided { get; private set; }

    /// <summary>True while lifting an instance constructor (calls on `this` then prefer base constructors under ICF).</summary>
    public bool InConstructor { get; init; }

    /// <summary>Emits the call. Returns true when the call never returns (throw), terminating the block.</summary>
    public bool Handle(EmitContext ctx, in Instruction ins, Func<Register, long, Expr?> memoryBase, bool resultNullTested = false)
    {
        if (ins.Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32 or OpKind.NearBranch16)
        {
            var target = ins.NearBranchTarget;
            if (sym.Map.SymbolAt(target) is not null)
            {
                var s = IcfResolver.Choose(sym.Map.CandidatesAt(target), emitter.ReceiverTypeDef(), sym.Metadata, sym.Registration, sym.Names, ctx.Log,
                    receiverIsNewObject: regs.Get(ArgumentLocations.IntArg(0)) is NewObjExpr { IsMaterialized: false }
                                         || InConstructor && regs.Get(ArgumentLocations.IntArg(0)) is ArgExpr { Parameter.Index: -1 });
                if (emitter.Emit(ctx, s.MethodIndex, s.MethodSpecIndex, null)) return false;
            }
            return HandleUnknown(ctx, target, ins.IP, resultNullTested);
        }

        if (ins.Op0Kind == OpKind.Memory && !ins.IsIPRelativeMemoryOperand && ins.MemoryIndex == Register.None)
        {
            var baseExpr = memoryBase(ins.MemoryBase, (long)ins.MemoryDisplacement64);
            if (baseExpr is MarkerExpr { Marker: MarkerKind.Klass } klass && klass.Base is { } obj)
            {
                var disp = (long)ins.MemoryDisplacement64;
                if (disp >= MemoryResolver.VTableOffset && (disp - MemoryResolver.VTableOffset) % 16 == 0)
                {
                    var slot = (int)((disp - MemoryResolver.VTableOffset) / 16);
                    var td = klass.Index;
                    if (VTableResolver.Resolve(sym.Metadata, td, slot) is { } t && emitter.Emit(ctx, t.MethodIndex, t.SpecIndex, obj)) return false;
                    Report(DiagnosticCodes.UnresolvedVTableCall, $"vtable slot {slot} of {sym.Names.TypeDefName(td, true)} could not be resolved.", ins.IP);
                }
            }
            else if (baseExpr is MarkerExpr { Marker: MarkerKind.VTableEntry, Base: { } ifaceObj, Usage: { } u } ifaceEntry && (long)ins.MemoryDisplacement64 == 0)
            {
                var td = ifaceEntry.Index;
                var slot = u.Index;
                if ((uint)td < (uint)sym.Metadata.TypeDefinitions.Length)
                {
                    var def = sym.Metadata.TypeDefinitions[td];
                    if (slot < def.MethodCount && emitter.Emit(ctx, def.MethodStart + (int)slot, -1, ifaceObj)) return false;
                }
            }
            else Report(DiagnosticCodes.UnresolvedVTableCall, $"Indirect call through [{ins.MemoryBase}+0x{ins.MemoryDisplacement64:X}] (interface/delegate dispatch).", ins.IP);
        }
        else if (ins.Op0Kind == OpKind.Memory && ins.IsIPRelativeMemoryOperand)
        {
            Report(DiagnosticCodes.UnresolvedCall, $"Call through import/function pointer at RVA 0x{ins.IPRelativeMemoryAddress - sym.Registration.Reader.ImageBase:X}.", ins.IP);
        }
        else Report(DiagnosticCodes.UnresolvedVTableCall, $"Indirect call through register {ins.Op0Register}.", ins.IP);

        EmitNative(ctx, $"indirect@{ins.IP - sym.Registration.Reader.ImageBase:X}");
        return false;
    }

    private bool HandleUnknown(EmitContext ctx, ulong target, ulong ip, bool resultNullTested)
    {
        switch (helpers?.Emit(ctx, target, resultNullTested))
        {
            case Runtime.HelperOutcome.Handled: Elided++; return false;
            case Runtime.HelperOutcome.Terminated: return true;
        }
        // Legacy heuristic (no helper map available): a call taking a runtime-metadata marker is treated as
        // metadata/class initialization. With a helper map this is NOT applied: it would also swallow object_new.
        if (helpers is null && regs.TryGet(ArgumentLocations.IntArg(0), out var rcx) && rcx is MarkerExpr { Marker: MarkerKind.TypeInfo or MarkerKind.StaticFields or MarkerKind.MethodInfo or MarkerKind.Il2CppType })
        {
            Elided++;
            regs.ClobberVolatiles(CallEmitter.Volatile);
            return false;
        }
        var name = helperMap?.ExportNames.GetValueOrDefault(target) ?? $"sub_{target - sym.Registration.Reader.ImageBase:X}";
        if (Runtime.CrtMath.Build(name, regs, ctx.Module.CorLibTypeFactory) is { } math)
        {
            var result = regs.NewLocal(math.Type!);
            math.Emit(ctx);
            ctx.IL.Add(CilOpCodes.Stloc, result);
            regs.ClobberVolatiles(CallEmitter.Volatile);
            regs.Set(Register.XMM0, new LocalExpr(result));
            return false;
        }
        Report(DiagnosticCodes.UnresolvedCall, $"Call to {name} (no metadata method; runtime helper or inlined-function thunk).", ip);
        EmitNative(ctx, name);
        return false;
    }

    /// <summary>long CallNative(name, rcx, rdx, r8, r9) → RAX.</summary>
    private void EmitNative(EmitContext ctx, string name)
    {
        ctx.IL.Add(CilOpCodes.Ldstr, name);
        foreach (var r in new[] { ArgumentLocations.IntArg(0), ArgumentLocations.IntArg(1), ArgumentLocations.IntArg(2), ArgumentLocations.IntArg(3) })
        {
            if (regs.TryGet(r, out var e) && !e.IsMarker) Expr.EmitAs(ctx, e, ctx.Module.CorLibTypeFactory.Int64);
            else ctx.IL.Add(CilOpCodes.Ldc_I8, 0L);
        }
        ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.CallNative);
        regs.ClobberVolatiles(CallEmitter.Volatile);
        var result = regs.NewLocal(ctx.Module.CorLibTypeFactory.Int64);
        ctx.IL.Add(CilOpCodes.Stloc, result);
        regs.Set(Register.RAX, new LocalExpr(result));
    }

    private void Report(string code, string message, ulong ip)
    {
        Unresolved++;
        sink.Report(DiagnosticSeverity.Debug, DiagnosticCategory.Lifting, code, message, loc(ip));
    }
}
