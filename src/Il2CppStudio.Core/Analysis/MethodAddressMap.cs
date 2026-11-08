using Il2CppStudio.Core.Backup;
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
                    if (rid < 1 || rid > module.MethodPointers.Count) continue;
                    var va = module.MethodPointers[rid - 1];
                    if (va == 0) continue;
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
}
