using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Lifting.Runtime;

namespace Il2CppStudio.Core.Workspace;

public sealed partial class Il2CppWorkspace
{
    private RuntimeHelperMap? _runtimeHelpers;
    private readonly Lock _helperGate = new();

    /// <summary>
    /// Runtime helper classification (write barrier, object_new, casts, raise...) plus inline-copy / PDB aliases
    /// (<c>sub_23E110</c> -> <c>Mathf.RoundToInt</c>). Built once under a lock and published only when complete: the
    /// GUI renders views on background threads, and a half-built map would let a concurrent lift miss the aliases
    /// (and cache that body). <see cref="WorkspaceSetup.Attach"/> builds it at load time.
    /// </summary>
    public RuntimeHelperMap? RuntimeHelpers
    {
        get
        {
            if (Volatile.Read(ref _runtimeHelpers) is { } ready) return ready;
            if (Symbolizer is null || Disassembler is null || Binary is null) return null;
            lock (_helperGate)
            {
                if (_runtimeHelpers is not null) return _runtimeHelpers;
                var exports = Binary.GetSymbols().Select(e => (e.Name, Binary.ImageBase + e.Rva));
                var map = RuntimeHelperMap.Build(Symbolizer, Disassembler, exports, Sink, Abi);
                CrtMathDiscovery.Discover(Symbolizer, Disassembler, map);

                // Out-of-line copies of managed methods (_inline functions) become aliases of the original method.
                var unknownTargets = map.Stats.Keys.Where(va => map.KindOf(va) == RuntimeHelperKind.Unknown && !map.ExportNames.ContainsKey(va));
                InlineCopies = InlineCopyMatcher.Match(Symbolizer, Disassembler, unknownTargets, Sink, map);
                AddressMap!.AddAliases(InlineCopies.Select(m => (m.Va, m.Original)));
                AddPdbAliases(map);
                Volatile.Write(ref _runtimeHelpers, map);
                return map;
            }
        }
    }

    /// <summary>
    /// Enhanced mode: name the remaining non-metadata call targets from the matching PDB. "X_inline" / "X_gshared_inline"
    /// copies resolve to the method whose generated symbol is X; helper classifications are cross-checked and logged.
    /// </summary>
    private void AddPdbAliases(RuntimeHelperMap helpers)
    {
        if (!IsEnhanced || Backup?.PdbPath is not { } pdb || Backup.PdbMatchesBinary != true) return;
        var names = Backup.PdbPublicSymbols ??= Core.Backup.PdbPublicSymbols.Load(pdb, Binary!.Sections, Sink);
        if (names.Count == 0) return;
        var bySymbol = AddressMap!.Symbols.Values.Where(s => s.CppSymbol is not null && !s.DisplayName.EndsWith("[inline copy]"))
            .GroupBy(s => s.CppSymbol!).ToDictionary(g => g.Key, g => g.First());
        var aliases = new List<(ulong, Analysis.NativeSymbol)>();
        foreach (var va in helpers.Stats.Keys)
        {
            if (!names.TryGetValue(va - Binary!.ImageBase, out var name)) continue;
            var helper = helpers.KindOf(va);
            if (helper != RuntimeHelperKind.Unknown)
            {
                Sink.Debug(Diagnostics.DiagnosticCategory.Pdb, Diagnostics.DiagnosticCodes.RuntimeHelperIdentified, $"PDB cross-check: sub_{va - Binary.ImageBase:X} classified {helper}, PDB name '{name}'.");
                continue;
            }
            if (AddressMap.SymbolAt(va) is not null) continue;
            var core = name.EndsWith("_inline", StringComparison.Ordinal) ? name[..^"_inline".Length] : name;
            if (bySymbol.TryGetValue(core, out var original)) aliases.Add((va, original));
            else helpers.ExportNames.TryAdd(va, name);   // named native function (CRT math, runtime API, ...)
        }
        AddressMap.AddAliases(aliases);
        Sink.Info(Diagnostics.DiagnosticCategory.Pdb, Diagnostics.DiagnosticCodes.InlineCopyMatched, $"PDB names resolved {aliases.Count} additional out-of-line method copies.");
    }

    /// <summary>Non-metadata functions proven identical to managed method bodies (see <see cref="InlineCopyMatcher"/>).</summary>
    public IReadOnlyList<InlineCopyMatch> InlineCopies { get; private set; } = [];
}
