using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Gui.ViewModels;

/// <summary>
/// Main window state. All analysis goes through <see cref="Il2CppWorkspace"/> + <see cref="WorkspaceSetup"/>
/// exactly like the CLI; work runs on a background thread serialized by a semaphore (the workspace is single-threaded).
/// </summary>
public sealed partial class MainViewModel : ObservableObject, IDisposable
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly Serilog.Core.Logger _logger;
    private readonly DiagnosticCollector _sink;
    private Il2CppWorkspace? _ws;
    private CancellationTokenSource? _renderCts;

    public IUiServices? Ui { get; set; }

    public ObservableCollection<TreeNodeViewModel> Roots { get; } = [];
    public ObservableCollection<Diagnostic> Diagnostics { get; } = [];
    public ObservableCollection<string> ExportedResources { get; } = [];

    [ObservableProperty] private string _title = "Il2CppStudio";
    [ObservableProperty] private string _modeLabel = "No folder loaded";
    [ObservableProperty] private string _status = "File → Load Folder… to open an IL2CPP build.";
    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _substituteBcl = true;
    [ObservableProperty] private TreeNodeViewModel? _selectedNode;
    [ObservableProperty] private string _cppText = "";
    [ObservableProperty] private string _ilText = "";
    [ObservableProperty] private string _cSharpText = "";
    [ObservableProperty] private string _inspectionText = "";
    [ObservableProperty] private DiagnosticSeverity _minimumSeverity = DiagnosticSeverity.Information;

    public MainViewModel()
    {
        _logger = LoggingSetup.CreateLogger(new LoggingOptions { EnableConsole = false });
        _sink = new DiagnosticCollector(_logger);
        _sink.DiagnosticReported += d =>
        {
            if (d.Severity < MinimumSeverity) return;
            if (Ui is { } ui) ui.Post(() => AddDiagnostic(d));
        };
    }

    private void AddDiagnostic(Diagnostic d)
    {
        Diagnostics.Add(d);
        if (Diagnostics.Count > 20_000) Diagnostics.RemoveAt(0);
    }

    // ------------------------------------------------------------------ File menu

    [RelayCommand]
    private async Task LoadFolder()
    {
        if (Ui is null) return;
        var folder = await Ui.PickFolderAsync("Select an IL2CPP game folder");
        if (folder is not null) await LoadFolderAsync(folder);
    }

    public async Task LoadFolderAsync(string folder)
    {
        await RunBusy($"Loading {folder}…", () =>
        {
            _ws?.Dispose();
            _ws = null;
            var ws = Il2CppWorkspace.Load(folder, _sink, new WorkspaceOptions { SubstituteBcl = SubstituteBcl });
            WorkspaceSetup.Attach(ws);
            _ws = ws;
        });
        if (_ws is null) return;
        ModeLabel = _ws.ModeLabel;
        Title = $"Il2CppStudio — {_ws.Files.Root}";
        InspectionText = BuildInspectionText(_ws);
        RebuildTree();
        Status = $"{_ws.ModeLabel}  {_ws.Model.Statistics.Types:N0} types, {_ws.Model.Statistics.Methods:N0} methods.";
    }

    [RelayCommand]
    private async Task ExportAll()
    {
        if (_ws is null || Ui is null) return;
        var dir = await Ui.PickFolderAsync("Export all to…");
        if (dir is null) return;
        ExportSummary? summary = null;
        await RunBusy("Exporting…", () => summary = ExportService.ExportAll(_ws, dir, includeIl: true,
            progress: new Progress<string>(m => Ui.Post(() => Status = m))));
        if (summary is not null)
        {
            Status = $"Exported {summary.Assemblies.Count} assemblies to {dir} in {summary.Elapsed.TotalSeconds:F1}s.";
            ShowArtifacts(summary.Artifacts);
        }
    }

    /// <summary>Sidebar right-click on an assembly: export just that project (optionally with its reference closure).</summary>
    public async Task ExportAssemblyAsync(TreeNodeViewModel node, bool withReferences)
    {
        if (_ws is null || Ui is null || node.Assembly is not { } asm) return;
        var dir = await Ui.PickFolderAsync($"Export {asm.Name}{(withReferences ? " + references" : "")} to…");
        if (dir is null) return;
        var set = withReferences ? AssemblyClosure.Of(_ws.Model, asm) : [asm];
        ExportSummary? summary = null;
        await RunBusy($"Exporting {asm.Name}…", () => summary = ExportService.ExportAll(_ws, dir, includeIl: true,
            AssemblyClosure.Filter(set), new Progress<string>(m => Ui.Post(() => Status = m))));
        if (summary is not null)
        {
            Status = $"Exported {summary.Assemblies.Count} assemblies ({string.Join(", ", set.Select(a => a.Name).Take(6))}{(set.Count > 6 ? ", …" : "")}) to {dir} in {summary.Elapsed.TotalSeconds:F1}s.";
            ShowArtifacts(summary.Artifacts);
        }
    }

    // ------------------------------------------------------------------ Tools menu

    [RelayCommand]
    private async Task ConfigureBootFlags()
    {
        if (_ws is null || Ui is null) return;
        await Ui.ShowBootConfigDialogAsync(new BootConfigViewModel(_ws.Files.Root, _sink));
    }

    [RelayCommand]
    private async Task ExportResources()
    {
        if (_ws is null || Ui is null) return;
        var dir = await Ui.PickFolderAsync("Export resources to…");
        if (dir is null) return;
        SerializationSummary? s = null;
        await RunBusy("Exporting resources…", () => s = _ws.ExportArtifacts(dir));
        if (s is not null) { ShowArtifacts(s); Status = $"{s.Resources} resources, {s.StringLiterals:N0} string literals exported to {dir}."; }
    }

    private void ShowArtifacts(SerializationSummary s)
    {
        ExportedResources.Clear();
        foreach (var f in s.Files) ExportedResources.Add(f);
    }

    partial void OnSubstituteBclChanged(bool value)
    {
        if (_ws is null) return;
        _ws.SubstituteBcl = value;
        Status = value ? "Substitute BCL (Clean/Fast)" : "Pure Native View";
        _ = RenderSelectionAsync();
    }

    // ------------------------------------------------------------------ tree

    partial void OnSearchTextChanged(string value) => RebuildTree();

    private void RebuildTree()
    {
        Roots.Clear();
        if (_ws is null) return;
        var filter = SearchText.Trim();
        foreach (var asm in _ws.Model.Assemblies.OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase))
        {
            if (filter.Length == 0)
            {
                Roots.Add(AssemblyNode(asm, null));
                continue;
            }
            var matches = asm.Namespaces.SelectMany(n => n.Types).SelectMany(Flatten)
                .Where(t => t.FullName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                            || t.Methods.Any(m => m.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            if (matches.Count == 0) continue;
            var node = AssemblyNode(asm, matches);
            node.IsExpanded = true;
            Roots.Add(node);
        }
    }

    private static IEnumerable<TypeModel> Flatten(TypeModel t) => new[] { t }.Concat(t.NestedTypes.SelectMany(Flatten));

    private TreeNodeViewModel AssemblyNode(AssemblyModel asm, List<TypeModel>? filtered)
    {
        var bcl = _ws!.SubstituteBcl && Core.Decompilation.BclReferenceProvider.IsBclAssembly(asm.Name);
        return new TreeNodeViewModel(this, NodeKind.Assembly, $"{asm.Name} ({asm.Version}){(bcl ? "  [BCL]" : "")}", () =>
        {
            var groups = filtered is null
                ? asm.Namespaces.Select(n => (n.Name, Types: (IEnumerable<TypeModel>)n.Types))
                : filtered.GroupBy(t => t.Namespace).OrderBy(g => g.Key).Select(g => (g.Key, Types: (IEnumerable<TypeModel>)g));
            return groups.Select(g => new TreeNodeViewModel(this, NodeKind.Namespace, g.Item1.Length == 0 ? "-" : g.Item1,
                () => g.Types.Select(TypeNode)) { IsExpanded = filtered is not null });
        }) { Assembly = asm };
    }

    private TreeNodeViewModel TypeNode(TypeModel t) =>
        new(this, NodeKind.Type, t.DeclaringType is null ? t.DisplayName : t.DisplayName, () => TypeChildren(t)) { Type = t };

    private IEnumerable<TreeNodeViewModel> TypeChildren(TypeModel t)
    {
        foreach (var n in t.NestedTypes) yield return TypeNode(n);
        foreach (var m in t.Methods)
        {
            var rva = _ws!.GetMethodRva(m);
            yield return new TreeNodeViewModel(this, NodeKind.Method, $"{m.DisplaySignature}{(rva != 0 ? $"  @0x{rva:X}" : "")}") { Type = t, Method = m };
        }
        foreach (var p in t.Properties) yield return new TreeNodeViewModel(this, NodeKind.Property, p.Name) { Type = t };
        foreach (var f in t.Fields) yield return new TreeNodeViewModel(this, NodeKind.Field, $"{f.Name} : {_ws!.Names.CSharpName(f.TypeIndex)}") { Type = t };
        foreach (var e in t.Events) yield return new TreeNodeViewModel(this, NodeKind.Event, e.Name) { Type = t };
    }

    partial void OnSelectedNodeChanged(TreeNodeViewModel? value) => _ = RenderSelectionAsync();

    /// <summary>Renders the three synchronized views (C/C++, IL, C#) for the selected type or method.</summary>
    private async Task RenderSelectionAsync()
    {
        var node = SelectedNode;
        if (_ws is null || node?.Type is not { } type) return;
        _renderCts?.Cancel();
        var cts = _renderCts = new CancellationTokenSource();
        var method = node.Method;
        var target = method is null ? type.FullName : $"{type.FullName}::{method.DisplaySignature}";
        CppText = IlText = CSharpText = $"// Rendering {target}…";
        string cpp = "", il = "", cs = "";
        await RunBusy($"Rendering {target}…", () =>
        {
            if (cts.IsCancellationRequested) return;
            cpp = _ws.Render(type, method, CodeView.Cpp);
            if (cts.IsCancellationRequested) return;
            il = _ws.Render(type, method, CodeView.Il);
            if (cts.IsCancellationRequested) return;
            cs = _ws.Render(type, method, CodeView.CSharp);
        });
        if (cts.IsCancellationRequested) return;
        CppText = cpp; IlText = il; CSharpText = cs;
        Status = method is not null && _ws.GetMethodRva(method) is var rva and > 0 ? $"{target}  RVA 0x{rva:X}  file offset 0x{_ws.GetMethodFileOffset(method):X}" : target;
    }

    // ------------------------------------------------------------------ context menu

    public async Task CopyRvaAsync(TreeNodeViewModel node)
    {
        if (_ws is null || node.Method is null || Ui is null) return;
        var rva = _ws.GetMethodRva(node.Method);
        if (rva == 0) { Status = "Method has no native code."; return; }
        await Ui.SetClipboardAsync($"0x{rva:X}");
        Status = $"Copied RVA 0x{rva:X}.";
    }

    public async Task ViewInHxdAsync(TreeNodeViewModel node)
    {
        if (_ws is null || node.Method is null || Ui is null) return;
        var target = HexEditorLauncher.Resolve(_ws, node.Method);
        if (target is null) { Status = "Method has no native code."; return; }
        await Ui.SetClipboardAsync(target.FileOffset.ToString("X"));
        Status = HexEditorLauncher.Open(target, _sink) ? target.Instructions + " (offset copied to clipboard)" : "HxD could not be started; see Diagnostics.";
    }

    public async Task ExportNodeAsync(TreeNodeViewModel node, CodeView view)
    {
        if (_ws is null || node.Type is null || Ui is null) return;
        var name = node.Method is null ? node.Type.Name : $"{node.Type.Name}.{node.Method.Name}";
        var ext = view == CodeView.CSharp ? "cs" : "cpp";
        var path = await Ui.SaveFileAsync($"Export {(view == CodeView.CSharp ? "C#" : "C++")}", WorkspaceSerializer.Sanitize(name) + "." + ext, ext);
        if (path is null) return;
        string text = "";
        await RunBusy("Exporting…", () => text = _ws.Render(node.Type, node.Method, view));
        await File.WriteAllTextAsync(path, text);
        Status = $"Saved {path}.";
    }

    // ------------------------------------------------------------------ helpers

    private async Task RunBusy(string status, Action work)
    {
        await _gate.WaitAsync();
        IsBusy = true;
        Status = status;
        try { await Task.Run(work); }
        catch (Exception ex)
        {
            _sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.StageFailed, ex.Message, null, ex);
            Status = "Error: " + ex.Message;
        }
        finally
        {
            IsBusy = false;
            _gate.Release();
        }
    }

    private static string BuildInspectionText(Il2CppWorkspace ws)
    {
        var sb = new StringBuilder();
        var s = ws.Model.Statistics;
        sb.AppendLine(ws.ModeLabel);
        sb.AppendLine($"Game root     : {ws.Files.Root}");
        sb.AppendLine($"Unity         : {ws.Environment.UnityVersion ?? "unknown"} ({ws.Environment.UnityVersionSource})  backend {ws.Environment.ScriptingBackend}");
        sb.AppendLine($"API compat    : {ws.Environment.ApiCompatibility?.Value}   stripping: {ws.Environment.StrippingLevel?.Value}");
        sb.AppendLine($"Metadata      : v{s.Version} ({s.UnityRange}), {s.FileSize:N0} bytes — {ws.Metadata.Path}");
        sb.AppendLine($"Counts        : {s.Images} images, {s.Types:N0} types, {s.Methods:N0} methods, {s.Fields:N0} fields, {s.Properties:N0} properties, {s.Events:N0} events");
        sb.AppendLine($"                {s.GenericContainers:N0} generic containers, {s.GenericParameters:N0} generic params, {s.StringLiterals:N0} string literals, {s.VTableSlots:N0} vtable slots");
        if (ws.Binary is { } pe)
        {
            var linker = pe is Core.Binary.PeBinaryImage pei ? $"  linker {pei.Headers.LinkerVersion}" : "";
            sb.AppendLine($"Binary        : {pe.Path}  {pe.Format} {pe.Architecture}  image base 0x{pe.ImageBase:X}{linker}");
            foreach (var sec in pe.Sections) sb.AppendLine($"   {sec.Name,-8} RVA 0x{sec.Rva:X8}  size 0x{sec.VirtualSize:X}  file 0x{sec.FileOffset:X}");
        }
        if (ws.Registration is { } reg)
        {
            sb.AppendLine($"CodeRegistration     RVA 0x{reg.CodeRegistrationVa - reg.Reader.ImageBase:X}  ({reg.CodeGenModules.Count} modules)");
            sb.AppendLine($"MetadataRegistration RVA 0x{reg.MetadataRegistrationVa - reg.Reader.ImageBase:X}  ({reg.TypeCount:N0} types, {reg.MetadataUsages.Count:N0} runtime metadata globals)");
        }
        if (ws.AddressMap is { } map) sb.AppendLine($"Methods with code    {map.ResolvedMethodCount:N0} definitions, {map.GenericInstanceCount:N0} generic instances");
        if (ws.Backup is { } b) sb.AppendLine($"Build backup         {b.Folder}  ({b.CppFileCount} C/C++ files, PDB match: {b.PdbMatchesBinary?.ToString() ?? "n/a"})");
        sb.AppendLine();
        sb.AppendLine($"{"Section",-40} {"Offset",10} {"Size",12} {"Elem",5} {"Count",9}  OK");
        foreach (var r in ws.Metadata.SectionReports.OrderBy(r => r.Offset))
            sb.AppendLine($"{r.Name,-40} 0x{r.Offset,8:X} {r.Size,12:N0} {r.ElementSize,5} {r.ElementCount,9:N0}  {(r.InBounds && r.Aligned ? "yes" : "NO")}");
        return sb.ToString();
    }

    public void Dispose()
    {
        _ws?.Dispose();
        _logger.Dispose();
    }
}
