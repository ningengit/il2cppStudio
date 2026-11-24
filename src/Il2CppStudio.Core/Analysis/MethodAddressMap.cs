using Il2CppStudio.Core.Backup;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Analysis;

/// <summary>A call target known to the analysis: a method definition, optionally a generic instantiation of it.</summary>
public sealed record NativeSymbol(ulong Va, int MethodIndex, int MethodSpecIndex, string DisplayName, string? CppSymbol);

/// <summary>
/// Maps metadata methods to native addresses (CodeGenModule.methodPointers, indexed by token RID-1) and
/// generic instantiations (genericMethodTable → genericMethodPointers), and native addresses back to symbols.
/// Function extents come from x64 .pdata RUNTIME_FUNCTION entries; when a function has no unwind entry
/// (leaf functions), the next known function start bounds it.
/// </summary>
public sealed class MethodAddressMap
{
    private readonly ulong[] _methodVa;              // by method index
    private readonly Dictionary<ulong, NativeSymbol> _byVa = [];
    private readonly Dictionary<ulong, List<NativeSymbol>> _folded = [];
    private readonly (uint Begin, uint End)[] _functions;
    private ulong[] _sortedStarts;
    private readonly ulong _imageBase;

    public int ResolvedMethodCount { get; }
    public int GenericInstanceCount { get; }
    public IReadOnlyDictionary<ulong, NativeSymbol> Symbols => _byVa;

    public MethodAddressMap(GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, TypeNameService names,
        IReadOnlyList<(uint Begin, uint End)> functions, BuildBackupInfo? backup, IDiagnosticSink sink)
    {
        _imageBase = reg.Reader.ImageBase;
        _functions = [.. functions];
        _methodVa = new ulong[md.Methods.Length];
        var wasmSplit = reg.Reader.Image is WasmBinaryImage { IsSplitFunctionTable: true } w ? w : null;

        var moduleByName = reg.CodeGenModules.ToDictionary(m => m.Name, StringComparer.OrdinalIgnoreCase);
        var backupByName = backup?.CodeGenModules.ToDictionary(m => m.AssemblyName, StringComparer.OrdinalIgnoreCase);

        var resolved = 0;
        foreach (var img in md.Images)
        {
            var imageName = md.GetString(img.NameIndex);
            if (!moduleByName.TryGetValue(imageName, out var module)) continue;
            CodeGenModule? backupModule = null;
            backupByName?.TryGetValue(imageName, out backupModule);

            for (var t = img.TypeStart; t < img.TypeStart + img.TypeCount; t++)
            {
                var def = md.TypeDefinitions[t];
                for (var m = def.MethodStart; m < def.MethodStart + def.MethodCount; m++)
                {
                    var token = md.Methods[m].Token;
                    var rid = (int)(token & 0x00FFFFFF);
                    var va = rid >= 1 && rid <= module.MethodPointers.Count ? module.MethodPointers[rid - 1] : 0UL;
                    if (va == 0 && md.Version.HasCodegenIndices && (uint)md.Methods[m].MethodIndex < (uint)reg.GlobalMethodPointers.Count)
                        va = reg.GlobalMethodPointers[md.Methods[m].MethodIndex];
                    if (va == 0) continue;
                    if (wasmSplit is not null)
                    {
                        va = ResolveWasmMethodSlot(md, model, reg, wasmSplit, m, null, va);
                        if (va == 0) continue;
                    }
                    _methodVa[m] = va;
                    resolved++;
                    var cpp = backupModule?.SymbolForToken(token);
                    if (cpp is "NULL" or "0") cpp = null;
                    // Identical code folding (COMDAT folding) maps many methods to one address; keep the first.
                    var symbol = new NativeSymbol(va, m, -1, $"{model.Types[t].FullName}::{md.GetString(md.Methods[m].NameIndex)}", cpp);
                    if (!_byVa.TryAdd(va, symbol)) (_folded.TryGetValue(va, out var group) ? group : _folded[va] = [_byVa[va]]).Add(symbol);
                }
            }
        }
        ResolvedMethodCount = resolved;

        var generic = 0;
        foreach (var g in reg.GenericMethodTable)
        {
            if ((uint)g.MethodIndex >= (uint)reg.GenericMethodPointers.Count || (uint)g.GenericMethodIndex >= (uint)reg.MethodSpecs.Count) continue;
            var va = reg.GenericMethodPointers[g.MethodIndex];
            if (va == 0) continue;
            var spec = reg.MethodSpecs[g.GenericMethodIndex];
            if ((uint)spec.MethodDefinitionIndex >= (uint)md.Methods.Length) continue;
            if (wasmSplit is not null)
            {
                va = ResolveWasmMethodSlot(md, model, reg, wasmSplit, spec.MethodDefinitionIndex, spec, va);
                if (va == 0) continue;
            }
            generic++;
            if (_byVa.ContainsKey(va)) continue;
            _byVa[va] = new NativeSymbol(va, spec.MethodDefinitionIndex, g.GenericMethodIndex, SpecDisplayName(md, model, reg, names, spec), null);
        }
        GenericInstanceCount = generic;

        _sortedStarts = [.. _byVa.Keys.Order()];
        sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationFound,
            $"Method map: {resolved:N0} method definitions with code, {generic:N0} generic instances, {_functions.Length:N0} .pdata functions.");
    }

    public static string SpecDisplayName(GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, TypeNameService names, Il2CppMethodSpecRecord spec)
    {
        var def = md.Methods[spec.MethodDefinitionIndex];
        var owner = (uint)def.DeclaringType < (uint)model.Types.Count ? model.Types[def.DeclaringType].FullName : "?";
        string Inst(int idx) => (uint)idx < (uint)reg.GenericInstVas.Count
            ? "<" + string.Join(", ", reg.ReadGenericInst(reg.GenericInstVas[idx]).TypeArgVas.Select(v => names.CSharpName(reg.GetTypeAt(v)))) + ">"
            : "";
        return $"{TypeNameService.StripArity(owner)}{(spec.ClassInstIndex >= 0 ? Inst(spec.ClassInstIndex) : "")}::{md.GetString(def.NameIndex)}{(spec.MethodInstIndex >= 0 ? Inst(spec.MethodInstIndex) : "")}";
    }

    public int AliasCount { get; private set; }

    /// <summary>Registers proven copies of metadata methods (e.g. out-of-line _inline functions) as call targets.</summary>
    public void AddAliases(IEnumerable<(ulong Va, NativeSymbol Original)> aliases)
    {
        foreach (var (va, original) in aliases)
            if (_byVa.TryAdd(va, original with { Va = va, DisplayName = original.DisplayName + " [inline copy]", CppSymbol = original.CppSymbol is null ? null : original.CppSymbol + "_inline" }))
                AliasCount++;
        _sortedStarts = [.. _byVa.Keys.Order()];
    }

    public ulong GetMethodVa(int methodIndex) => (uint)methodIndex < (uint)_methodVa.Length ? _methodVa[methodIndex] : 0;

    public ulong GetMethodRva(int methodIndex) => GetMethodVa(methodIndex) is var va and > 0 ? va - _imageBase : 0;

    public NativeSymbol? SymbolAt(ulong va) => _byVa.GetValueOrDefault(va);

    /// <summary>
    /// All metadata methods sharing this address. MSVC identical-code folding (/OPT:ICF) merges methods with identical
    /// machine code (empty constructors, trivial getters), so one address can stand for many methods.
    /// </summary>
    public IReadOnlyList<NativeSymbol> CandidatesAt(ulong va) =>
        _folded.TryGetValue(va, out var g) ? g : _byVa.TryGetValue(va, out var s) ? [s] : [];

    public int FoldedAddressCount => _folded.Count;

    /// <summary>Returns the [start, end) VA range of the function starting at <paramref name="va"/>.</summary>
    public (ulong Start, ulong End) GetFunctionBounds(ulong va, int maxSize = 0x10000)
    {
        var rva = (uint)(va - _imageBase);
        var lo = 0; var hi = _functions.Length - 1;
        while (lo <= hi)
        {
            var mid = (lo + hi) >>> 1;
            if (_functions[mid].Begin == rva) return (va, _imageBase + _functions[mid].End);
            if (_functions[mid].Begin < rva) lo = mid + 1; else hi = mid - 1;
        }
        // Leaf function (no unwind data): bounded by the next known function start.
        var nextFunc = lo < _functions.Length ? _imageBase + _functions[lo].Begin : ulong.MaxValue;
        var idx = Array.BinarySearch(_sortedStarts, va);
        var nextSym = idx >= 0 && idx + 1 < _sortedStarts.Length ? _sortedStarts[idx + 1] : ulong.MaxValue;
        var end = Math.Min(Math.Min(nextFunc, nextSym), va + (ulong)maxSize);
        return (va, end);
    }

    private static ulong ResolveWasmMethodSlot(
        GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, WasmBinaryImage wasm,
        int methodIndex, Il2CppMethodSpecRecord? spec, ulong rawSlot)
    {
        var method = md.Methods[methodIndex];
        IReadOnlyList<ulong>? classArgs = null;
        IReadOnlyList<ulong>? methodArgs = null;
        if (spec is { ClassInstIndex: >= 0 } s && (uint)s.ClassInstIndex < (uint)reg.GenericInstVas.Count)
            classArgs = reg.ReadGenericInst(reg.GenericInstVas[s.ClassInstIndex]).TypeArgVas;
        if (spec is { MethodInstIndex: >= 0 } ms && (uint)ms.MethodInstIndex < (uint)reg.GenericInstVas.Count)
            methodArgs = reg.ReadGenericInst(reg.GenericInstVas[ms.MethodInstIndex]).TypeArgVas;

        var isStatic = (method.Flags & 0x0010) != 0;
        var retInfo = reg.TryGetType(method.ReturnType, out var rt)
            ? ClassifyWasmType(md, model, reg, rt, classArgs, methodArgs, isReturn: true, depth: 0)
            : (Primary: 'i', Alt: (char?)null, HasRetBuf: false);

        var retVariants = new List<(char Ret, bool RetBuf)>(2);
        if (retInfo.HasRetBuf)
        {
            if (retInfo.Alt is { } altRet) retVariants.Add((altRet, false));
            retVariants.Add(('v', true));
        }
        else
        {
            retVariants.Add((retInfo.Primary, false));
        }

        var paramChoices = new List<(char Primary, char? Alt)>(method.ParameterCount);
        var altIndices = new List<int>(4);
        for (var p = 0; p < method.ParameterCount; p++)
        {
            var pi = method.ParameterStart + p;
            if ((uint)pi >= (uint)md.Parameters.Length || !reg.TryGetType(md.Parameters[pi].TypeIndex, out var pt))
            {
                paramChoices.Add(('i', null));
                continue;
            }
            var c = ClassifyWasmType(md, model, reg, pt, classArgs, methodArgs, isReturn: false, depth: 0);
            if (c.Alt is not null && altIndices.Count < 4) altIndices.Add(paramChoices.Count);
            paramChoices.Add((c.Primary, c.Alt));
        }

        var comboCount = 1 << altIndices.Count;
        foreach (var (retChar, useRetBuf) in retVariants)
        {
            for (var mask = 0; mask < comboCount; mask++)
            {
                var sig = BuildWasmSig(retChar, useRetBuf, !isStatic, paramChoices, altIndices, mask);
                if (wasm.TryResolveSplitTableSlot(sig, rawSlot, out var slot) && MatchesWasmFuncType(wasm, slot, sig))
                    return slot;
            }
        }
        return 0;
    }

    private static (char Primary, char? Alt, bool HasRetBuf) ClassifyWasmType(
        GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, Il2CppTypeRecord t,
        IReadOnlyList<ulong>? classArgs, IReadOnlyList<ulong>? methodArgs, bool isReturn, int depth)
    {
        if (depth > 8) return ('i', null, false);
        if (t.ByRef) return ('i', null, false);

        if (t.Type is Il2CppTypeEnum.Var or Il2CppTypeEnum.MVar)
        {
            var gpi = t.GenericParameterIndex;
            var outer = t.Type == Il2CppTypeEnum.Var ? classArgs : methodArgs;
            if (outer is not null && (uint)gpi < (uint)md.GenericParameters.Length)
            {
                var num = md.GenericParameters[gpi].Num;
                if (num < outer.Count)
                    return ClassifyWasmType(md, model, reg, reg.GetTypeAt(outer[num]), classArgs, methodArgs, isReturn, depth + 1);
            }
            return ('i', null, false);
        }

        switch (t.Type)
        {
            case Il2CppTypeEnum.Void: return ('v', null, false);
            case Il2CppTypeEnum.I8 or Il2CppTypeEnum.U8: return ('j', null, false);
            case Il2CppTypeEnum.R4: return ('f', null, false);
            case Il2CppTypeEnum.R8: return ('d', null, false);
            case Il2CppTypeEnum.Enum: return ('i', null, false);
            case Il2CppTypeEnum.ValueType:
                return ClassifyValueType(md, model, reg, t.KlassIndex, instArgs: null, classArgs, methodArgs, isReturn, depth + 1);
            case Il2CppTypeEnum.GenericInst:
            {
                var gc = reg.ReadGenericClass(t.Data);
                var defType = reg.GetTypeAt(gc.TypeVa);
                if (defType.Type == Il2CppTypeEnum.ValueType)
                {
                    var instArgs = gc.ClassInstVa != 0 ? reg.ReadGenericInst(gc.ClassInstVa).TypeArgVas : null;
                    return ClassifyValueType(md, model, reg, defType.KlassIndex, instArgs, classArgs, methodArgs, isReturn, depth + 1);
                }
                return ('i', null, false);
            }
            default:
                return ('i', null, false);
        }
    }

    private static (char Primary, char? Alt, bool HasRetBuf) ClassifyValueType(
        GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, int typeDefIndex,
        IReadOnlyList<ulong>? instArgs, IReadOnlyList<ulong>? outerClassArgs, IReadOnlyList<ulong>? outerMethodArgs,
        bool isReturn, int depth)
    {
        if ((uint)typeDefIndex >= (uint)md.TypeDefinitions.Length) return ('i', null, false);
        var td = md.TypeDefinitions[typeDefIndex];
        if (td.IsEnum)
        {
            for (var i = 0; i < td.FieldCount; i++)
            {
                var fi = td.FieldStart + i;
                if ((uint)fi >= (uint)md.Fields.Length) break;
                if (reg.TryGetType(md.Fields[fi].TypeIndex, out var ft) && (ft.Attrs & 0x0010) == 0)
                    return (ft.Type is Il2CppTypeEnum.I8 or Il2CppTypeEnum.U8 ? 'j' : 'i', null, false);
            }
            return ('i', null, false);
        }

        var instanceFieldType = (Il2CppTypeRecord?)null;
        var instanceCount = 0;
        for (var i = 0; i < td.FieldCount; i++)
        {
            var fi = td.FieldStart + i;
            if ((uint)fi >= (uint)md.Fields.Length) break;
            if (!reg.TryGetType(md.Fields[fi].TypeIndex, out var ft)) continue;
            if ((ft.Attrs & 0x0010) != 0) continue; // static field
            instanceCount++;
            if (instanceCount > 1) break;
            instanceFieldType = ft;
        }

        if (instanceCount == 0)
            return isReturn ? ('v', 'i', true) : ('i', null, false);

        if (instanceCount == 1 && instanceFieldType is { } fType)
        {
            var effectiveClassArgs = SubstituteTypeArgs(reg, instArgs, outerClassArgs, outerMethodArgs, md) ?? outerClassArgs;
            var scalar = ClassifyWasmType(md, model, reg, fType, effectiveClassArgs, outerMethodArgs, isReturn: false, depth + 1);
            if (!scalar.HasRetBuf && scalar.Alt is null)
                return isReturn ? ('v', scalar.Primary, true) : (scalar.Primary, scalar.Primary == 'i' ? null : 'i', false);
            return isReturn ? ('v', null, true) : ('i', null, false);
        }

        return isReturn ? ('v', null, true) : ('i', null, false);
    }

    private static IReadOnlyList<ulong>? SubstituteTypeArgs(
        Il2CppRegistration reg, IReadOnlyList<ulong>? instArgs,
        IReadOnlyList<ulong>? outerClassArgs, IReadOnlyList<ulong>? outerMethodArgs, GlobalMetadata md)
    {
        if (instArgs is null || instArgs.Count == 0) return instArgs;
        List<ulong>? resolved = null;
        for (var i = 0; i < instArgs.Count; i++)
        {
            var va = instArgs[i];
            var t = reg.GetTypeAt(va);
            if (t.Type is Il2CppTypeEnum.Var or Il2CppTypeEnum.MVar)
            {
                var outer = t.Type == Il2CppTypeEnum.Var ? outerClassArgs : outerMethodArgs;
                var gpi = t.GenericParameterIndex;
                if (outer is not null && (uint)gpi < (uint)md.GenericParameters.Length)
                {
                    var num = md.GenericParameters[gpi].Num;
                    if (num < outer.Count)
                    {
                        resolved ??= [.. instArgs];
                        resolved[i] = outer[num];
                    }
                }
            }
        }
        return resolved ?? instArgs;
    }

    private static string BuildWasmSig(
        char retChar, bool useRetBuf, bool hasThis,
        List<(char Primary, char? Alt)> paramChoices, List<int> altIndices, int altMask)
    {
        var len = 1 + (useRetBuf ? 1 : 0) + (hasThis ? 1 : 0) + paramChoices.Count + 1;
        return string.Create(len, (retChar, useRetBuf, hasThis, paramChoices, altIndices, altMask), static (span, s) =>
        {
            var idx = 0;
            span[idx++] = s.retChar;
            if (s.useRetBuf) span[idx++] = 'i';
            if (s.hasThis) span[idx++] = 'i';
            for (var i = 0; i < s.paramChoices.Count; i++)
                span[idx++] = s.paramChoices[i].Primary;
            for (var a = 0; a < s.altIndices.Count; a++)
            {
                if ((s.altMask & (1 << a)) == 0) continue;
                var pIdx = s.altIndices[a];
                if (s.paramChoices[pIdx].Alt is { } alt)
                    span[1 + (s.useRetBuf ? 1 : 0) + (s.hasThis ? 1 : 0) + pIdx] = alt;
            }
            span[idx] = 'i'; // MethodInfo*
        });
    }

    private static bool MatchesWasmFuncType(WasmBinaryImage wasm, uint slot, string sig)
    {
        if (!wasm.TryGetFunction(slot, out _, out var ft)) return false;
        if (sig[0] == 'v')
        {
            if (ft.ResultCount != 0) return false;
        }
        else if (ft.ResultCount != 1 || ft.Result(0) != WasmTypeByte(sig[0]))
        {
            return false;
        }
        if (ft.ParameterCount != sig.Length - 1) return false;
        for (var i = 0; i < ft.ParameterCount; i++)
            if (ft.Parameter(i) != WasmTypeByte(sig[i + 1])) return false;
        return true;
    }

    private static byte WasmTypeByte(char c) => c switch
    {
        'i' => 0x7F,
        'j' => 0x7E,
        'f' => 0x7D,
        'd' => 0x7C,
        _ => 0,
    };
}
