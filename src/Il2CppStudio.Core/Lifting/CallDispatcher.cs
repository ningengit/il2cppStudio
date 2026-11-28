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
                var candidates = sym.Map.CandidatesAt(target);
                NativeSymbol? exact = null;
                var exactSpec = -1;
                if (candidates.Count > 1)
                {
                    for (var pos = 0; pos <= 3 && exact is null; pos++)
                    {
                        if (ArgumentLocations.TryReadHelperArg(regs, pos, out var arg)
                            && arg is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { Kind: Registration.MetadataUsageKind.MethodRef } u }
                            && (uint)u.Index < (uint)sym.Registration.MethodSpecs.Count)
                        {
                            var specMethodIdx = sym.Registration.MethodSpecs[u.Index].MethodDefinitionIndex;
                            var match = candidates.FirstOrDefault(c => c.MethodIndex == specMethodIdx);
                            if (match is not null)
                            {
                                var sh = MethodShape.Create(sym.Metadata, sym.Registration, emitter.Stubs, emitter.Mem, emitter.Module, match.MethodIndex, u.Index);
                                if (pos == sh.FirstParameterPosition + sh.Parameters.Count)
                                {
                                    exact = match;
                                    exactSpec = u.Index;
                                }
                            }
                        }
                    }
                }
                if (exact is not null && emitter.Emit(ctx, exact.MethodIndex, exactSpec, null)) return false;
                var recv = ArgumentLocations.Abi == NativeAbi.Win32 ? emitter.Win32ThisCandidate() : regs.Get(ArgumentLocations.IntArg(0));
                var s = IcfResolver.Choose(candidates, emitter.ReceiverTypeDef(), sym.Metadata, sym.Registration, sym.Names, ctx.InfoLog,
                    receiverIsNewObject: recv is NewObjExpr { IsMaterialized: false }
                                         || InConstructor && recv is ArgExpr { Parameter.Index: -1 },
                    receiverType: emitter.ReceiverType(), stubs: emitter.Stubs, module: emitter.Module);
                if (emitter.Emit(ctx, s.MethodIndex, s.MethodSpecIndex, null)) return false;
            }
            return HandleUnknown(ctx, target, ins.IP, resultNullTested);
        }

        if (ins.Op0Kind == OpKind.Memory && !ins.IsIPRelativeMemoryOperand && ins.MemoryIndex == Register.None)
        {
            var baseExpr = memoryBase(ins.MemoryBase, ArgumentLocations.PointerSize == 4 && ins.MemoryDisplacement64 > int.MaxValue
                ? unchecked((int)ins.MemoryDisplacement32)
                : (long)ins.MemoryDisplacement64);
            if (baseExpr is MarkerExpr { Marker: MarkerKind.Klass } klass && klass.Base is { } obj)
            {
                var disp = ArgumentLocations.PointerSize == 4 && ins.MemoryDisplacement64 > int.MaxValue
                    ? unchecked((int)ins.MemoryDisplacement32)
                    : (long)ins.MemoryDisplacement64;
                var vtOff = sym.Registration.Ptr == 8
                    ? (sym.Metadata.Version.Major >= 108 ? MemoryResolver.VTableOffset64V108 : MemoryResolver.VTableOffset64)
                    : 0xB8;
                var vtSize = sym.Registration.Ptr * 2;
                if (disp >= vtOff && (disp - vtOff) % vtSize == 0)
                {
                    var slot = (int)((disp - vtOff) / vtSize);
                    var td = klass.Index;
                    if (VTableResolver.Resolve(sym.Metadata, td, slot, sym.Registration, sym.Names) is { } t && emitter.Emit(ctx, t.MethodIndex, t.SpecIndex, obj)) return false;
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
        // Known CRT math imports/thunks must run before runtime-metadata marker heuristics: eliding an inline
        // class-init guard before a math call can leave TypeInfo in RCX when entering modf/floor/ceil.
        var name = helperMap?.ExportNames.GetValueOrDefault(target);
        if (name is not null)
        {
            if (Runtime.CrtMath.TryEmitModF(name, ctx, regs, emitter.Mem, emitter.Slots, ctx.Module.CorLibTypeFactory))
                return false;
            if (Runtime.CrtMath.Build(name, regs, ctx.Module.CorLibTypeFactory) is { } math)
            {
                var result = regs.NewLocal(math.Type!);
                math.Emit(ctx);
                ctx.IL.Add(CilOpCodes.Stloc, result);
                regs.ClobberVolatiles(CallEmitter.Volatile);
                regs.Set(Register.XMM0, new LocalExpr(result));
                return false;
            }
        }
        // Legacy heuristic (no helper map available): a call taking a runtime-metadata marker is treated as
        // metadata/class initialization. With a helper map this is NOT applied: it would also swallow object_new.
        if (helpers is null && ArgumentLocations.TryReadHelperArg(regs, 0, out var rcx) && rcx is MarkerExpr { Marker: MarkerKind.TypeInfo or MarkerKind.StaticFields or MarkerKind.MethodInfo or MarkerKind.Il2CppType })
        {
            Elided++;
            regs.ClobberVolatiles(CallEmitter.Volatile);
            return false;
        }
        if (ArgumentLocations.TryReadHelperArg(regs, 0, out var a0) && a0 is MarkerExpr { Marker: MarkerKind.TypeInfo })
        {
            Elided++;
            regs.ClobberVolatiles(CallEmitter.Volatile);
            regs.Set(Register.RAX, a0);
            return false;
        }
        if (ArgumentLocations.TryReadHelperArg(regs, 1, out var a1) && a1 is MarkerExpr { Marker: MarkerKind.ElementClass })
        {
            Elided++;
            var a0Keep = regs.Get(ArgumentLocations.IntArg(0));
            regs.ClobberVolatiles(CallEmitter.Volatile);
            regs.Set(Register.RAX, a0Keep);
            return false;
        }
        name ??= $"sub_{target - sym.Registration.Reader.ImageBase:X}";
        Report(DiagnosticCodes.UnresolvedCall, $"Call to {name} (no metadata method; runtime helper or inlined-function thunk).", ip);
        EmitNative(ctx, name);
        return false;
    }

    /// <summary>long CallNative(name, a0, a1, a2, a3) → RAX.</summary>
    private void EmitNative(EmitContext ctx, string name)
    {
        ctx.IL.Add(CilOpCodes.Ldstr, name);
        for (var i = 0; i < 4; i++)
        {
            if (ArgumentLocations.TryReadHelperArg(regs, i, out var e) && !e.IsMarker) Expr.EmitAs(ctx, e, ctx.Module.CorLibTypeFactory.Int64);
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
