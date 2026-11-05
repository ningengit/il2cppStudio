using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Lifting.Runtime;

namespace Il2CppStudio.Core.Workspace;

public sealed partial class Il2CppWorkspace
{
    private RuntimeHelperMap? _runtimeHelpers;

    /// <summary>
    /// Runtime helper classification (write barrier, object_new, casts, raise…). Built on first use because it
    /// scans every method's call sites; null when the binary could not be analysed.
    /// </summary>
    public RuntimeHelperMap? RuntimeHelpers
    {
        get
        {
            if (_runtimeHelpers is not null || Symbolizer is null || Disassembler is null || Binary is null) return _runtimeHelpers;
            var exports = Binary.GetExports().Where(e => e.Name is not null).Select(e => (e.Name!, Binary.ImageBase + e.Rva));
            _runtimeHelpers = RuntimeHelperMap.Build(Symbolizer, Disassembler, exports, Sink);

            // Out-of-line copies of managed methods (_inline functions) become aliases of the original method.
            var unknownTargets = _runtimeHelpers.Stats.Keys.Where(va => _runtimeHelpers.KindOf(va) == RuntimeHelperKind.Unknown
                                                                      && !_runtimeHelpers.ExportNames.ContainsKey(va));
            InlineCopies = InlineCopyMatcher.Match(Symbolizer, Disassembler, unknownTargets, Sink);
            AddressMap!.AddAliases(InlineCopies.Select(m => (m.Va, m.Original)));
            AddPdbAliases();
            return _runtimeHelpers;
        }
    }

    /// <summary>
    /// Enhanced mode: name the remaining non-metadata call targets from the matching PDB. "X_inline" / "X_gshared_inline"
    /// copies resolve to the method whose generated symbol is X; helper classifications are cross-checked and logged.
    /// </summary>
    private void AddPdbAliases()
    {
        if (!IsEnhanced || Backup?.PdbPath is not { } pdb || Backup.PdbMatchesBinary != true) return;
        var names = Backup.PdbPublicSymbols ??= Core.Backup.PdbPublicSymbols.Load(pdb, Binary!.Sections, Sink);
        if (names.Count == 0) return;
        var bySymbol = AddressMap!.Symbols.Values.Where(s => s.CppSymbol is not null && !s.DisplayName.EndsWith("[inline copy]"))
            .GroupBy(s => s.CppSymbol!).ToDictionary(g => g.Key, g => g.First());
        var aliases = new List<(ulong, Analysis.NativeSymbol)>();
        foreach (var va in _runtimeHelpers!.Stats.Keys)
        {
            if (!names.TryGetValue(va - Binary!.ImageBase, out var name)) continue;
            var helper = _runtimeHelpers.KindOf(va);
            if (helper != RuntimeHelperKind.Unknown)
            {
                Sink.Debug(Diagnostics.DiagnosticCategory.Pdb, Diagnostics.DiagnosticCodes.RuntimeHelperIdentified, $"PDB cross-check: sub_{va - Binary.ImageBase:X} classified {helper}, PDB name '{name}'.");
                continue;
            }
            if (AddressMap.SymbolAt(va) is not null) continue;
            var core = name.EndsWith("_inline", StringComparison.Ordinal) ? name[..^"_inline".Length] : name;
            if (bySymbol.TryGetValue(core, out var original)) aliases.Add((va, original));
            else _runtimeHelpers.ExportNames.TryAdd(va, name);   // named native function (CRT math, runtime API, …)
        }
        AddressMap.AddAliases(aliases);
        Sink.Info(Diagnostics.DiagnosticCategory.Pdb, Diagnostics.DiagnosticCodes.InlineCopyMatched, $"PDB names resolved {aliases.Count} additional out-of-line method copies.");
    }

    /// <summary>Non-metadata functions proven identical to managed method bodies (see <see cref="InlineCopyMatcher"/>).</summary>
    public IReadOnlyList<InlineCopyMatch> InlineCopies { get; private set; } = [];
}
