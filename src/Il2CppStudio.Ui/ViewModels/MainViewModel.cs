using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;
using Il2CppStudio.Gui.Services;

namespace Il2CppStudio.Gui.ViewModels;

/// <summary>
/// Main window state. All analysis goes through <see cref="Il2CppWorkspace"/> + <see cref="WorkspaceSetup"/>
/// exactly like the CLI; work runs on a background thread serialized by a semaphore (the workspace is single-threaded).
/// </summary>
public sealed partial class MainViewModel : ObservableObject, IDisposable
{
    private const int SearchDebounceMs = 200;
    private const int SelectionDebounceMs = 140;
    private const int MaxSearchResults = 500;

    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly Serilog.Core.Logger _logger;
    private readonly DiagnosticCollector _sink;
    private readonly RuntimeDiagnosticsLog _runtimeLog;
    private readonly PerformanceMonitorService _perfMonitor;
    private Il2CppWorkspace? _ws;
    private WorkspaceSearchIndex? _searchIndex;
    private CancellationTokenSource? _renderCts;
    private CancellationTokenSource? _searchCts;
    private long _selectionVersion;
    private (long Version, string? Cpp, string? Il, string? Cs) _renderedTabs;
    private int _pendingRuntimeLogRefresh;
    private volatile bool _disposed;

    public IUiServices? Ui { get; set; }

    public ObservableCollection<TreeNodeViewModel> Roots { get; } = [];
    public ObservableCollection<Diagnostic> Diagnostics { get; } = [];
    public ObservableCollection<string> ExportedResources { get; } = [];
    public ObservableCollection<RuntimeLogEntry> RuntimeLogs { get; } = [];
    public IReadOnlyList<RuntimeLogFilter> LogFilters { get; } = Enum.GetValues<RuntimeLogFilter>();
    public IReadOnlyList<int> IntervalChoices { get; } = [1, 2, 5, 10, 15, 30, 60];

    internal RuntimeDiagnosticsLog RuntimeLog => _runtimeLog;
    internal PerformanceMonitorService PerfMonitor => _perfMonitor;

    [ObservableProperty] private string _title = "Il2CppStudio";
    [ObservableProperty] private string _modeLabel = "No folder loaded";
    [ObservableProperty] private string _status = OperatingSystem.IsAndroid()
        ? "Tap File → Load File to open an APK, NSP, PKG, IPA, or game binary..."
        : "Drop a game file/folder here, or use File → Load File / Load Folder…";
    [ObservableProperty] private string _searchText = "";

    public bool IsAndroid => OperatingSystem.IsAndroid();
    public bool IsDesktop => !OperatingSystem.IsAndroid();
    [ObservableProperty] private bool _isSearching;
    [ObservableProperty] private string _searchSummary = "";
    [ObservableProperty] private IReadOnlyList<TreeNodeViewModel> _searchResults = Array.Empty<TreeNodeViewModel>();
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _substituteBcl = true;
    [ObservableProperty] private int _selectedCodeTabIndex;
    [ObservableProperty] private TreeNodeViewModel? _selectedNode;
    [ObservableProperty] private TreeNodeViewModel? _selectedTreeNode;
    [ObservableProperty] private TreeNodeViewModel? _selectedSearchNode;
    [ObservableProperty, NotifyPropertyChangedFor(nameof(ContextHasMenu), nameof(ContextHasCode), nameof(ContextIsMethod), nameof(ContextIsAssembly))]
    private TreeNodeViewModel? _contextNode;
    [ObservableProperty] private string _cppText = "";
    [ObservableProperty] private string _ilText = "";
    [ObservableProperty] private string _cSharpText = "";
    [ObservableProperty] private string _inspectionText = "";
    [ObservableProperty] private DiagnosticSeverity _minimumSeverity = DiagnosticSeverity.Information;

    // Debug & Performance Monitor panel properties
    [ObservableProperty] private bool _isDebugPanelOpen;
    [ObservableProperty] private string _cpuSummary = "CPU: 0.0% sys | 0.0% proc";
    [ObservableProperty] private string _ramSummary = "RAM: 0 / 0 MB (0%) | Proc: 0 MB";
    [ObservableProperty] private string _diskSummary = "Disk: 0.0% busy | R 0.00 / W 0.00 MB/s";
    [ObservableProperty] private string _gcSummary = "GC: Gen0 0 | Gen1 0 | Gen2 0 | Threads 0";
    [ObservableProperty] private string _runtimeSummary = "Activity: Idle | Warn 0 | Err 0 | 1stChance 0";
    [ObservableProperty] private string _perfLogFolder = "";
    [ObservableProperty] private bool _isCsvLogging;
    [ObservableProperty] private string _csvLogButtonText = "Start CSV Log";
    [ObservableProperty] private int _perfIntervalSeconds = 5;
    [ObservableProperty] private string _lastGcResultText = "";
    [ObservableProperty] private RuntimeLogFilter _selectedLogFilter = RuntimeLogFilter.All;
    [ObservableProperty] private bool _captureFirstChanceExceptions;

    private TreeNodeViewModel? ContextTarget => ContextNode ?? SelectedNode;
    public bool ContextHasMenu => ContextTarget?.HasMenu ?? false;
    public bool ContextHasCode => ContextTarget?.HasCode ?? false;
    public bool ContextIsMethod => ContextTarget?.IsMethod ?? false;
    public bool ContextIsAssembly => ContextTarget?.IsAssembly ?? false;

    public MainViewModel()
    {
        _logger = LoggingSetup.CreateLogger(new LoggingOptions { EnableConsole = false });
        _sink = new DiagnosticCollector(_logger);
        _runtimeLog = new RuntimeDiagnosticsLog();
        _runtimeLog.InstallGlobalHooks();
        if (Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PERF_LOG") == "1")
            _runtimeLog.StartFileLogging();
        _perfMonitor = new PerformanceMonitorService(_runtimeLog);
        _perfLogFolder = _perfMonitor.LogDirectory;
        _isCsvLogging = _perfMonitor.IsCsvLogging;
        _csvLogButtonText = _isCsvLogging ? "Stop CSV Log" : "Start CSV Log";
        _captureFirstChanceExceptions = _runtimeLog.CaptureFirstChanceExceptions;

        _sink.DiagnosticReported += OnDiagnosticReported;
        _runtimeLog.Changed += OnRuntimeLogChanged;
        _perfMonitor.Sampled += OnPerformanceSampled;
        _perfMonitor.Start();
    }

    private void OnDiagnosticReported(Diagnostic d)
    {
        _runtimeLog.RecordFromDiagnostic(d);
        if (d.Severity < MinimumSeverity) return;
        if (Ui is { } ui) ui.Post(() => AddDiagnostic(d));
    }

    public void RecordUnhandledUiException(Exception ex)
    {
        _runtimeLog.RecordError("Dispatcher.UIThread.UnhandledException", ex.Message, ex);
        _sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.StageFailed, ex.Message, null, ex);
    }

    private void OnRuntimeLogChanged()
    {
        if (_disposed) return;
        if (Interlocked.CompareExchange(ref _pendingRuntimeLogRefresh, 1, 0) != 0) return;
        void Apply()
        {
            Interlocked.Exchange(ref _pendingRuntimeLogRefresh, 0);
            if (!_disposed) RefreshRuntimeLogs();
        }
        if (Ui is { } ui) ui.Post(Apply);
        else Apply();
    }

    private void RefreshRuntimeLogs()
    {
        var items = _runtimeLog.Snapshot(SelectedLogFilter);
        RuntimeLogs.Clear();
        for (var i = items.Count - 1; i >= 0; i--)
            RuntimeLogs.Add(items[i]);
    }

    private void OnPerformanceSampled(PerformanceSample s)
    {
        if (_disposed) return;
        void Apply()
        {
            if (_disposed) return;
            CpuSummary = string.Format(CultureInfo.InvariantCulture,
                "CPU: {0:F1}% sys | {1:F1}% proc | Threads: {2}", s.CpuPercent, s.ProcessCpuPercent, s.Threads);
            RamSummary = string.Format(CultureInfo.InvariantCulture,
                "RAM: {0:F0}/{1:F0} MB ({2:F1}%) | Proc {3:F1} MB (Peak {4:F1}, Heap {5:F1})",
                s.RamUsedMb, s.RamTotalMb, s.RamUsedPercent, s.ThisProcessMb, s.PeakProcessMb, s.ManagedHeapMb);
            DiskSummary = string.Format(CultureInfo.InvariantCulture,
                "Disk: {0:F1}% busy | R {1:F2} / W {2:F2} MB/s | Free {3:F1} GB",
                s.DiskBusyPercent, s.DiskReadMbPerSec, s.DiskWriteMbPerSec, s.DiskFreeGb);
            GcSummary = string.Format(CultureInfo.InvariantCulture,
                "GC: Gen0 {0} | Gen1 {1} | Gen2 {2} | Priv {3:F1} MB",
                s.GcGen0, s.GcGen1, s.GcGen2, s.PrivateBytesMb);
            RuntimeSummary = string.Format(CultureInfo.InvariantCulture,
                "Activity: {0} | Warn {1} | Err {2} | 1stChance {3}",
                s.Activity, s.Warnings, s.Errors, s.FirstChanceExceptions);
        }
        if (Ui is { } ui) ui.Post(Apply);
        else Apply();
    }

    partial void OnPerfIntervalSecondsChanged(int value)
    {
        var clamped = Math.Clamp(value, 1, 60);
        if (clamped != value)
        {
            PerfIntervalSeconds = clamped;
            return;
        }
        _perfMonitor.IntervalSeconds = clamped;
    }

    partial void OnCaptureFirstChanceExceptionsChanged(bool value)
    {
        _runtimeLog.CaptureFirstChanceExceptions = value;
    }

    partial void OnSelectedLogFilterChanged(RuntimeLogFilter value) => RefreshRuntimeLogs();

    [RelayCommand]
    private void ToggleDebugPanel()
    {
        IsDebugPanelOpen = !IsDebugPanelOpen;
        if (IsDebugPanelOpen)
        {
            _perfMonitor.SampleNow(writeCsvIfEnabled: false);
            RefreshRuntimeLogs();
        }
    }

    [RelayCommand]
    private void ToggleCsvLogging()
    {
        if (_perfMonitor.IsCsvLogging)
        {
            _perfMonitor.StopCsvLogging();
            _runtimeLog.StopFileLogging();
            IsCsvLogging = false;
            CsvLogButtonText = "Start CSV Log";
            Status = "Stopped performance CSV logging.";
        }
        else
        {
            _perfMonitor.StartCsvLogging();
            _runtimeLog.StartFileLogging(_perfMonitor.LogDirectory);
            IsCsvLogging = true;
            CsvLogButtonText = "Stop CSV Log";
            _perfMonitor.SampleNow(writeCsvIfEnabled: true);
            Status = $"CSV performance log writing to {_perfMonitor.LogDirectory}.";
        }
    }

    [RelayCommand]
    private void OpenPerfLogFolder()
    {
        try
        {
            var dir = _perfMonitor.LogDirectory;
            Directory.CreateDirectory(dir);
            if (OperatingSystem.IsAndroid())
            {
                Status = $"Perf logs: {dir}";
                return;
            }
            Process.Start(new ProcessStartInfo(dir) { UseShellExecute = true });
            Status = $"Opened log folder: {dir}";
        }
        catch (Exception ex)
        {
            _runtimeLog.RecordWarning("MainViewModel.OpenPerfLogFolder", ex.Message, ex);
            Status = $"Could not open log folder: {ex.Message}";
        }
    }

    [RelayCommand]
    private void ForceGc()
    {
        var (before, after, freed) = _perfMonitor.ForceGcCompactLoh();
        LastGcResultText = string.Format(CultureInfo.InvariantCulture,
            "GC freed {0:F1} MB ({1:F1} → {2:F1} MB)", freed, before, after);
        Status = LastGcResultText;
    }

    [RelayCommand]
    private async Task ClearCache()
    {
        await RunBusy("Clearing caches…", () =>
        {
            _ws?.ClearCaches();
        }, activity: "Idle");
        _renderedTabs = (0, null, null, null);
        Status = "Cleared decompiler and stub assembly caches.";
    }

    [RelayCommand]
    private async Task CopyPerfSnapshot()
    {
        if (Ui is null) return;
        var sample = _perfMonitor.SampleNow(writeCsvIfEnabled: false);
        await Ui.SetClipboardAsync(PerformanceMonitorService.FormatSnapshotText(sample));
        Status = "Copied performance snapshot to clipboard.";
    }

    [RelayCommand]
    private void ClearRuntimeLogs()
    {
        _runtimeLog.Clear();
        RefreshRuntimeLogs();
        Status = "Cleared runtime diagnostics log.";
    }

    [RelayCommand]
    private async Task CopyRuntimeLogs()
    {
        if (Ui is null) return;
        await Ui.SetClipboardAsync(_runtimeLog.FormatText(SelectedLogFilter));
        Status = "Copied runtime logs to clipboard.";
    }

    public void ReportUiDiagnostic(DiagnosticSeverity severity, string message, Exception? ex = null)
    {
        switch (severity)
        {
            case DiagnosticSeverity.Error:
                _sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.SyntaxHighlightingStatus, message, null, ex);
                break;
            case DiagnosticSeverity.Warning:
                _sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.SyntaxHighlightingStatus, message, null, ex);
                break;
            default:
                _sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.SyntaxHighlightingStatus, message, null);
                break;
        }
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

    [RelayCommand]
    private async Task LoadFile()
    {
        if (Ui is null) return;
        var file = await Ui.PickFileAsync("Select an APK, XAPK, APKS, APKM, iOS IPA/Mach-O, PS4 PKG/PRX, Switch NSP/main, IL2CPP binary, WebGL wasm, or global-metadata.dat");
        if (file is null) return;
        await LoadPathAsync(file);
    }

    [RelayCommand]
    private async Task LoadInstalledApp()
    {
        if (Ui is null) return;
        var app = await Ui.PickInstalledAppAsync();
        if (app is null) return;
        await LoadFolderAsync(
            app.BaseApkPath,
            switchKeysetPath: null,
            ps4Passcode: null,
            additionalApkPaths: app.SplitApkPaths,
            nativeLibraryDir: app.NativeLibraryDir);
    }

    public async Task LoadPathAsync(string path)
    {
        if (Ui is null || string.IsNullOrWhiteSpace(path)) return;

        string? keysetPath = null;
        string? ps4Passcode = null;

        if (SwitchNspExtractor.IsNspPath(path) && SwitchNspExtractor.RequiresKeyset(path))
        {
            var settings = UserSettings.Load();
            keysetPath = SwitchKeySet.ResolveDefaultPath(settings.SwitchKeysetPath);
            if (keysetPath is null)
            {
                keysetPath = await Ui.PickKeysetFileAsync("Select Nintendo Switch keyset (prod.keys / dev.keys)");
                if (keysetPath is null) return;
                settings.SwitchKeysetPath = keysetPath;
                settings.Save();
            }
        }
        else if (Ps4PkgExtractor.IsPkgPath(path) && Ps4PkgExtractor.NeedsUserKey(path))
        {
            ps4Passcode = await Ui.PromptTextAsync(
                "PS4 Package Key Required",
                "This PS4 .pkg is encrypted with a non-default key. Enter a 64-character hex EKPFS key or 32-character passcode:",
                "64-hex EKPFS or 32-char passcode");
            if (string.IsNullOrWhiteSpace(ps4Passcode)) return;
        }

        await LoadFolderAsync(path, keysetPath, ps4Passcode);
    }

    public Task LoadFolderAsync(string folder) => LoadFolderAsync(folder, null, null);

    public async Task LoadFolderAsync(
        string folder,
        string? switchKeysetPath,
        string? ps4Passcode,
        IReadOnlyList<string>? additionalApkPaths = null,
        string? nativeLibraryDir = null)
    {
        CancelSearch();
        CancelRender();

        SelectedTreeNode = null;
        SelectedSearchNode = null;
        SelectedNode = null;
        ContextNode = null;
        IsSearching = false;
        SearchSummary = "";
        SearchResults = Array.Empty<TreeNodeViewModel>();
        SearchText = "";
        ReleaseTree();

        await RunBusy($"Loading {folder}…", () =>
        {
            _searchIndex = null;
            _ws?.Dispose();
            _ws = null;
            var ws = Il2CppWorkspace.Load(folder, _sink, new WorkspaceOptions
            {
                SubstituteBcl = SubstituteBcl,
                SwitchKeysetPath = switchKeysetPath,
                Ps4PkgPasscode = ps4Passcode,
                AdditionalApkPaths = additionalApkPaths,
                NativeLibraryDir = nativeLibraryDir,
            });
            WorkspaceSetup.Attach(ws);
            _searchIndex = WorkspaceSearchIndex.Build(ws.Model);
            _ws = ws;
        }, activity: "Loading");
        if (_ws is null) return;
        ModeLabel = _ws.ModeLabel;
        Title = $"Il2CppStudio — {_ws.Files.Root}";
        InspectionText = BuildInspectionText(_ws);
        PopulateRoots();
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
            progress: new Progress<string>(m => Ui.Post(() => Status = m))), activity: "Exporting");
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
            AssemblyClosure.Filter(set), new Progress<string>(m => Ui.Post(() => Status = m))), activity: "Exporting");
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
        _renderedTabs = (0, null, null, null);
        if (SelectedNode?.Type is not null)
            TriggerRender(SelectedNode, isTabSwitch: true);
    }

    // ------------------------------------------------------------------ tree & flat search

    partial void OnSearchTextChanged(string value)
    {
        var filter = value?.Trim() ?? "";
        if (filter.Length == 0 || _searchIndex is not { } index)
        {
            CancelSearch();
            ClearSearchState();
            return;
        }

        var cts = ReplaceSearchCts();
        _ = RunDebouncedSearchAsync(index, filter, cts);
    }

    private CancellationTokenSource ReplaceSearchCts()
    {
        var cts = new CancellationTokenSource();
        var old = Interlocked.Exchange(ref _searchCts, cts);
        if (old is not null)
        {
            try { old.Cancel(); } catch (ObjectDisposedException) { }
        }
        return cts;
    }

    private void CancelSearch()
    {
        var old = Interlocked.Exchange(ref _searchCts, null);
        if (old is not null)
        {
            try { old.Cancel(); } catch (ObjectDisposedException) { }
        }
    }

    private void ClearSearchState()
    {
        SelectedSearchNode = null;
        if (ContextNode?.Subtitle is not null) ContextNode = null;
        IsSearching = false;
        SearchSummary = "";
        SearchResults = Array.Empty<TreeNodeViewModel>();
    }

    private async Task RunDebouncedSearchAsync(WorkspaceSearchIndex index, string filter, CancellationTokenSource cts)
    {
        var token = cts.Token;
        try
        {
            await Task.Delay(SearchDebounceMs, token);
            var ws = _ws;
            if (ws is null || token.IsCancellationRequested || !ReferenceEquals(_searchCts, cts)) return;

            var result = await Task.Run(() => index.Search(filter, MaxSearchResults, token), token);
            if (token.IsCancellationRequested || !ReferenceEquals(_searchCts, cts)) return;

            var nodes = new TreeNodeViewModel[result.Matches.Count];
            for (var i = 0; i < result.Matches.Count; i++)
            {
                if ((i & 63) == 0) token.ThrowIfCancellationRequested();
                var m = result.Matches[i];
                if (m.Method is { } method)
                {
                    var rva = ws.GetMethodRva(method);
                    var text = rva != 0 ? $"{method.DisplaySignature}  @0x{rva:X}" : method.DisplaySignature;
                    nodes[i] = new TreeNodeViewModel(NodeKind.Method, text)
                    {
                        Assembly = m.Assembly,
                        Type = m.Type,
                        Method = method,
                        Subtitle = $"{m.Type.FullName}  [{m.Assembly.Name}]",
                    };
                }
                else
                {
                    var sub = m.Type.DeclaringType is { } decl
                        ? $"{decl.FullName}  [{m.Assembly.Name}]"
                        : string.IsNullOrEmpty(m.Type.Namespace) ? m.Assembly.Name : $"{m.Type.Namespace}  [{m.Assembly.Name}]";
                    nodes[i] = new TreeNodeViewModel(NodeKind.Type, m.Type.DisplayName)
                    {
                        Assembly = m.Assembly,
                        Type = m.Type,
                        Subtitle = sub,
                    };
                }
            }

            if (token.IsCancellationRequested || !ReferenceEquals(_searchCts, cts)) return;

            var summary = result.IsTruncated
                ? $"Showing {nodes.Length:N0} of {result.TotalMatches:N0} matches — refine search"
                : $"{nodes.Length:N0} match{(nodes.Length == 1 ? "" : "es")}";

            void Apply()
            {
                if (token.IsCancellationRequested || !ReferenceEquals(_searchCts, cts)) return;
                SelectedSearchNode = null;
                SearchSummary = summary;
                SearchResults = nodes;
                IsSearching = true;
            }

            if (Ui is { } ui) ui.Post(Apply);
            else Apply();
        }
        catch (OperationCanceledException)
        {
        }
        catch (ObjectDisposedException)
        {
        }
        catch (Exception ex)
        {
            _runtimeLog.RecordError("MainViewModel.Search", ex.Message, ex);
        }
        finally
        {
            Interlocked.CompareExchange(ref _searchCts, null, cts);
            cts.Dispose();
        }
    }

    private void ReleaseTree()
    {
        foreach (var root in Roots)
            root.ReleaseChildren();
        Roots.Clear();
    }

    private void PopulateRoots()
    {
        ReleaseTree();
        if (_ws is null) return;
        foreach (var asm in _ws.Model.Assemblies.OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase))
            Roots.Add(AssemblyNode(asm));
    }

    private TreeNodeViewModel AssemblyNode(AssemblyModel asm)
    {
        var bcl = _ws!.SubstituteBcl && Core.Decompilation.BclReferenceProvider.IsBclAssembly(asm.Name);
        return new TreeNodeViewModel(NodeKind.Assembly, $"{asm.Name} ({asm.Version}){(bcl ? "  [BCL]" : "")}", () =>
        {
            return asm.Namespaces.Select(ns => new TreeNodeViewModel(
                NodeKind.Namespace,
                ns.Name.Length == 0 ? "-" : ns.Name,
                () => ns.Types.Select(TypeNode)));
        }) { Assembly = asm };
    }

    private TreeNodeViewModel TypeNode(TypeModel t) =>
        new(NodeKind.Type, t.DisplayName, () => TypeChildren(t)) { Type = t };

    private IEnumerable<TreeNodeViewModel> TypeChildren(TypeModel t)
    {
        foreach (var n in t.NestedTypes) yield return TypeNode(n);
        foreach (var m in t.Methods)
        {
            var rva = _ws!.GetMethodRva(m);
            yield return new TreeNodeViewModel(NodeKind.Method, $"{m.DisplaySignature}{(rva != 0 ? $"  @0x{rva:X}" : "")}") { Type = t, Method = m };
        }
        foreach (var p in t.Properties) yield return new TreeNodeViewModel(NodeKind.Property, p.Name) { Type = t };
        foreach (var f in t.Fields) yield return new TreeNodeViewModel(NodeKind.Field, $"{f.Name} : {_ws!.Names.CSharpName(f.TypeIndex)}") { Type = t };
        foreach (var e in t.Events) yield return new TreeNodeViewModel(NodeKind.Event, e.Name) { Type = t };
    }

    partial void OnSelectedTreeNodeChanged(TreeNodeViewModel? value)
    {
        if (value is not null) SelectedNode = value;
    }

    partial void OnSelectedSearchNodeChanged(TreeNodeViewModel? value)
    {
        if (value is not null) SelectedNode = value;
    }

    partial void OnSelectedNodeChanged(TreeNodeViewModel? value) => TriggerRender(value, isTabSwitch: false);

    partial void OnSelectedCodeTabIndexChanged(int value)
    {
        if (SelectedNode?.Type is null) return;
        var ver = Volatile.Read(ref _selectionVersion);
        if (_renderedTabs.Version == ver)
        {
            var cached = ActiveCodeView switch
            {
                CodeView.Cpp => _renderedTabs.Cpp,
                CodeView.Il => _renderedTabs.Il,
                CodeView.CSharp => _renderedTabs.Cs,
                _ => null,
            };
            if (cached is not null) return;
        }
        TriggerRender(SelectedNode, isTabSwitch: true);
    }

    private CodeView ActiveCodeView => SelectedCodeTabIndex switch
    {
        1 => CodeView.Il,
        2 => CodeView.CSharp,
        _ => CodeView.Cpp,
    };

    private CancellationTokenSource ReplaceRenderCts()
    {
        var cts = new CancellationTokenSource();
        var prev = Interlocked.Exchange(ref _renderCts, cts);
        if (prev is not null)
        {
            try { prev.Cancel(); } catch (ObjectDisposedException) { }
        }
        return cts;
    }

    private void CancelRender()
    {
        Interlocked.Increment(ref _selectionVersion);
        var prev = Interlocked.Exchange(ref _renderCts, null);
        if (prev is not null)
        {
            try { prev.Cancel(); } catch (ObjectDisposedException) { }
        }
    }

    private void TriggerRender(TreeNodeViewModel? node, bool isTabSwitch)
    {
        if (_disposed || _ws is null || node?.Type is null)
        {
            CancelRender();
            return;
        }
        var version = isTabSwitch ? Volatile.Read(ref _selectionVersion) : Interlocked.Increment(ref _selectionVersion);
        var cts = ReplaceRenderCts();
        _ = RenderSelectionSafeAsync(version, node, cts, isTabSwitch);
    }

    /// <summary>Debounced, cancellable active-tab rendering for the selected type or method.</summary>
    private async Task RenderSelectionSafeAsync(long version, TreeNodeViewModel? node, CancellationTokenSource cts, bool isTabSwitch)
    {
        var token = cts.Token;
        var gateAcquired = false;
        try
        {
            if (_disposed || _ws is null || node?.Type is not { } type) return;
            if (!isTabSwitch)
                await Task.Delay(SelectionDebounceMs, token);

            if (token.IsCancellationRequested || Volatile.Read(ref _selectionVersion) != version || _disposed) return;

            var view = ActiveCodeView;
            var method = node.Method;
            var target = method is null ? type.FullName : $"{type.FullName}::{method.DisplaySignature}";

            await _gate.WaitAsync(token);
            gateAcquired = true;

            if (token.IsCancellationRequested || Volatile.Read(ref _selectionVersion) != version || _disposed) return;
            var ws = _ws;
            if (ws is null) return;

            IsBusy = true;
            _perfMonitor.SetActivity("Decompiling");
            Status = $"Rendering {target}…";

            var rendered = await Task.Run(() => ws.Render(type, method, view, token), token);
            if (token.IsCancellationRequested || Volatile.Read(ref _selectionVersion) != version || _disposed) return;

            void ApplyResult()
            {
                if (token.IsCancellationRequested || Volatile.Read(ref _selectionVersion) != version || _disposed) return;
                if (_renderedTabs.Version != version)
                    _renderedTabs = (version, null, null, null);
                switch (view)
                {
                    case CodeView.Cpp:
                        _renderedTabs.Cpp = rendered;
                        CppText = rendered;
                        break;
                    case CodeView.Il:
                        _renderedTabs.Il = rendered;
                        IlText = rendered;
                        break;
                    case CodeView.CSharp:
                        _renderedTabs.Cs = rendered;
                        CSharpText = rendered;
                        break;
                }
                Status = method is not null && ws.GetMethodRva(method) is var rva and > 0
                    ? $"{target}  RVA 0x{rva:X}  file offset 0x{ws.GetMethodFileOffset(method):X}"
                    : target;
            }

            if (Ui is { } ui) ui.Post(ApplyResult);
            else ApplyResult();
        }
        catch (OperationCanceledException)
        {
        }
        catch (ObjectDisposedException)
        {
        }
        catch (Exception ex)
        {
            _runtimeLog.RecordError("MainViewModel.RenderSelection", ex.Message, ex);
            _sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.StageFailed, ex.Message, null, ex);
            Status = "Error: " + ex.Message;
        }
        finally
        {
            _perfMonitor.SetActivity("Idle");
            IsBusy = false;
            if (gateAcquired) _gate.Release();
            Interlocked.CompareExchange(ref _renderCts, null, cts);
            cts.Dispose();
        }
    }

    // ------------------------------------------------------------------ context menu commands

    [RelayCommand]
    private async Task CopyRva()
    {
        if (ContextTarget is { } node) await CopyRvaAsync(node);
    }

    [RelayCommand]
    private async Task ViewInHxd()
    {
        if (ContextTarget is { } node) await ViewInHxdAsync(node);
    }

    [RelayCommand]
    private async Task ExportCSharp()
    {
        if (ContextTarget is { } node) await ExportNodeAsync(node, CodeView.CSharp);
    }

    [RelayCommand]
    private async Task ExportCpp()
    {
        if (ContextTarget is { } node) await ExportNodeAsync(node, CodeView.Cpp);
    }

    [RelayCommand]
    private async Task ExportAssembly()
    {
        if (ContextTarget is { } node) await ExportAssemblyAsync(node, withReferences: false);
    }

    [RelayCommand]
    private async Task ExportAssemblyWithReferences()
    {
        if (ContextTarget is { } node) await ExportAssemblyAsync(node, withReferences: true);
    }

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
        string text = "";
        await RunBusy("Rendering export…", () => text = _ws.Render(node.Type, node.Method, view));
        var savedPath = await Ui.SaveTextFileAsync($"Export {(view == CodeView.CSharp ? "C#" : "C++")}", WorkspaceSerializer.Sanitize(name) + "." + ext, ext, text);
        if (savedPath is not null)
            Status = $"Saved {savedPath}.";
    }

    // ------------------------------------------------------------------ helpers

    private async Task RunBusy(string status, Action work, string activity = "Busy")
    {
        await _gate.WaitAsync();
        IsBusy = true;
        _perfMonitor.SetActivity(activity);
        Status = status;
        try { await Task.Run(work); }
        catch (Exception ex)
        {
            _runtimeLog.RecordError("MainViewModel.RunBusy", ex.Message, ex);
            _sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.StageFailed, ex.Message, null, ex);
            Status = "Error: " + ex.Message;
        }
        finally
        {
            _perfMonitor.SetActivity("Idle");
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
        if (_disposed) return;
        _disposed = true;
        _sink.DiagnosticReported -= OnDiagnosticReported;
        _runtimeLog.Changed -= OnRuntimeLogChanged;
        _perfMonitor.Sampled -= OnPerformanceSampled;
        CancelSearch();
        CancelRender();
        _searchIndex = null;
        SearchResults = Array.Empty<TreeNodeViewModel>();
        ReleaseTree();
        _perfMonitor.Dispose();
        _runtimeLog.Dispose();
        _ws?.Dispose();
        _ws = null;
        _logger.Dispose();
    }
}
