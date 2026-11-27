using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Platform;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Gui.Highlighting;
using Il2CppStudio.Gui.ViewModels;
using TextMateSharp.Grammars;

namespace Il2CppStudio.Gui.Views;

public sealed partial class MainView : UserControl, IUiServices
{
    private MainViewModel? _attachedVm;
    private TopLevel? _attachedTopLevel;
    private IInsetsManager? _insetsManager;
    private bool _initialRenderRefreshPosted;
    private TaskCompletionSource<string?>? _promptTcs;
    private TaskCompletionSource<InstalledAndroidApp?>? _appPickerTcs;
    private IReadOnlyList<InstalledAndroidApp> _scannedApps = [];
    private TextMate.Installation? _cppTextMate;
    private TextMate.Installation? _csTextMate;
    private bool _managedHighlightingFallbackActive;
    private readonly List<(DiagnosticSeverity Severity, string Message, Exception? Ex)> _pendingDiagnostics = [];

    public MainView()
    {
        InitializeComponent();
        if (OperatingSystem.IsAndroid())
        {
            WorkspaceGrid.ColumnDefinitions[0].Width = new GridLength(0.35, GridUnitType.Star);
            WorkspaceGrid.ColumnDefinitions[2].Width = new GridLength(0.65, GridUnitType.Star);
        }
        InitializeSyntaxHighlighting();

        DataContextChanged += (_, _) => Attach();
        AttachedToVisualTree += (_, _) => Attach();
        DetachedFromVisualTree += (_, _) => Detach();
        AssemblyTree.AddHandler(Control.ContextRequestedEvent, OnNodeContextRequested, RoutingStrategies.Tunnel);
        SearchList.AddHandler(Control.ContextRequestedEvent, OnNodeContextRequested, RoutingStrategies.Tunnel);
        AddHandler(InputElement.KeyDownEvent, OnViewKeyDown, RoutingStrategies.Tunnel);

        PromptOkButton.Click += (_, _) => ResolvePrompt(PromptBox.Text?.Trim());
        PromptCancelButton.Click += (_, _) => ResolvePrompt(null);
        PromptBox.KeyDown += (_, e) =>
        {
            if (e.Key == Key.Enter) { e.Handled = true; ResolvePrompt(PromptBox.Text?.Trim()); }
            else if (e.Key == Key.Escape) { e.Handled = true; ResolvePrompt(null); }
        };

        InstalledAppsFilterBox.TextChanged += (_, _) => ApplyInstalledAppsFilter();
        InstalledAppsList.SelectionChanged += (_, _) => InstalledAppsOpenButton.IsEnabled = InstalledAppsList.SelectedItem is InstalledAndroidApp;
        InstalledAppsList.DoubleTapped += (_, _) =>
        {
            if (InstalledAppsList.SelectedItem is InstalledAndroidApp selected)
                ResolveInstalledAppPicker(selected);
        };
        InstalledAppsOpenButton.Click += (_, _) => ResolveInstalledAppPicker(InstalledAppsList.SelectedItem as InstalledAndroidApp);
        InstalledAppsCancelButton.Click += (_, _) => ResolveInstalledAppPicker(null);
        InstalledAppsRefreshButton.Click += (_, _) => _ = RefreshInstalledAppsAsync();
    }

    private TopLevel? HostTopLevel => TopLevel.GetTopLevel(this);

    private void InitializeSyntaxHighlighting()
    {
        InstallManagedHighlighter(IlEditor, SyntaxLanguage.Il);
        if (OperatingSystem.IsAndroid())
        {
            ActivateManagedFallback();
            return;
        }

        // Probe TextMate + Oniguruma end-to-end before installing on editors; on targets without native libonigwrap
        // (e.g., Android arm64), DllNotFoundException only fires on first TokenizeLine rather than InstallTextMate().
        try
        {
            var options = new RegistryOptions(ThemeName.DarkPlus);
            var probeRegistry = new TextMateSharp.Registry.Registry(options);
            var csScope = options.GetScopeByLanguageId(options.GetLanguageByExtension(".cs").Id);
            var grammar = probeRegistry.LoadGrammar(csScope)
                ?? throw new InvalidOperationException($"TextMate grammar '{csScope}' returned null.");
            var probe = grammar.TokenizeLine("public class Probe { int x = 0; }");
            if (probe?.Tokens is not { Length: > 0 })
                throw new InvalidOperationException("TextMate probe produced no tokens.");

            void OnTextMateException(Exception ex) =>
                Dispatcher.UIThread.Post(() => HandleRuntimeTextMateFailure(ex));

            _cppTextMate = Highlight(CppEditor, options, ".cpp", OnTextMateException);
            _csTextMate = Highlight(CSharpEditor, options, ".cs", OnTextMateException);
        }
        catch (Exception ex)
        {
            var severity = ex is DllNotFoundException && OperatingSystem.IsAndroid()
                ? DiagnosticSeverity.Information
                : DiagnosticSeverity.Warning;
            ReportDiagnostic(severity,
                $"TextMate highlighter unavailable ({ex.GetType().Name}: {ex.Message}); activated built-in pure-C# Dark+ syntax highlighter.",
                ex);
            ActivateManagedFallback();
        }
    }

    private void HandleRuntimeTextMateFailure(Exception ex)
    {
        if (_managedHighlightingFallbackActive) return;
        ReportDiagnostic(DiagnosticSeverity.Warning,
            $"TextMate highlighter failed at runtime ({ex.GetType().Name}: {ex.Message}); switching to built-in pure-C# Dark+ syntax highlighter.",
            ex);
        ActivateManagedFallback();
    }

    private void ActivateManagedFallback()
    {
        _managedHighlightingFallbackActive = true;
        _cppTextMate?.Dispose();
        _cppTextMate = null;
        _csTextMate?.Dispose();
        _csTextMate = null;

        InstallManagedHighlighter(CppEditor, SyntaxLanguage.Cpp);
        InstallManagedHighlighter(CSharpEditor, SyntaxLanguage.CSharp);
    }

    private static void InstallManagedHighlighter(TextEditor editor, SyntaxLanguage language)
    {
        var transformers = editor.TextArea.TextView.LineTransformers;
        for (var i = transformers.Count - 1; i >= 0; i--)
        {
            if (transformers[i] is TextMateColoringTransformer or DarkPlusSyntaxHighlighter)
                transformers.RemoveAt(i);
        }
        transformers.Add(new DarkPlusSyntaxHighlighter(language));
        editor.TextArea.TextView.Redraw();
    }

    private static TextMate.Installation Highlight(
        TextEditor editor,
        RegistryOptions options,
        string extension,
        Action<Exception> exceptionHandler)
    {
        var installation = editor.InstallTextMate(options, exceptionHandler: exceptionHandler);
        installation.SetGrammar(options.GetScopeByLanguageId(options.GetLanguageByExtension(extension).Id));
        return installation;
    }

    private void ReportDiagnostic(DiagnosticSeverity severity, string message, Exception? ex = null)
    {
        if (_attachedVm is { } vm)
            vm.ReportUiDiagnostic(severity, message, ex);
        else
            _pendingDiagnostics.Add((severity, message, ex));
    }

    private MainViewModel? Vm => DataContext as MainViewModel;

    private void Attach()
    {
        Detach();
        if (HostTopLevel is { } topLevel)
        {
            _attachedTopLevel = topLevel;
            _attachedTopLevel.Opened += OnTopLevelOpened;
            if (topLevel.InsetsManager is { } insetsManager)
            {
                _insetsManager = insetsManager;
                _insetsManager.SafeAreaChanged += OnSafeAreaChanged;
                RootSafeArea.Padding = _insetsManager.SafeAreaPadding;
            }
        }
        if (Vm is not { } vm) return;
        _attachedVm = vm;
        vm.Ui = this;
        vm.PropertyChanged += OnVmPropertyChanged;
        vm.SyncUiState();
        if (!string.Equals(CppEditor.Text, vm.CppText, StringComparison.Ordinal))
            CppEditor.Text = vm.CppText;
        if (!string.Equals(IlEditor.Text, vm.IlText, StringComparison.Ordinal))
            IlEditor.Text = vm.IlText;
        if (!string.Equals(CSharpEditor.Text, vm.CSharpText, StringComparison.Ordinal))
            CSharpEditor.Text = vm.CSharpText;
        if (!string.Equals(InspectionEditor.Text, vm.InspectionText, StringComparison.Ordinal))
            InspectionEditor.Text = vm.InspectionText;
        if (_pendingDiagnostics.Count > 0)
        {
            var queued = _pendingDiagnostics.ToArray();
            _pendingDiagnostics.Clear();
            foreach (var (sev, msg, ex) in queued)
                vm.ReportUiDiagnostic(sev, msg, ex);
        }
        if (_attachedTopLevel is not null)
            ScheduleLayoutAndVisualRefresh();
    }

    private void Detach()
    {
        if (_attachedTopLevel is not null)
        {
            _attachedTopLevel.Opened -= OnTopLevelOpened;
            _attachedTopLevel = null;
        }
        if (_insetsManager is not null)
        {
            _insetsManager.SafeAreaChanged -= OnSafeAreaChanged;
            _insetsManager = null;
        }
        if (_attachedVm is null) return;
        _attachedVm.PropertyChanged -= OnVmPropertyChanged;
        if (ReferenceEquals(_attachedVm.Ui, this))
            _attachedVm.Ui = null;
        _attachedVm = null;
    }

    private void OnTopLevelOpened(object? sender, EventArgs e) => RequestRenderRefresh();

    private void ScheduleLayoutAndVisualRefresh()
    {
        if (_initialRenderRefreshPosted)
            return;
        _initialRenderRefreshPosted = true;
        Dispatcher.UIThread.Post(() =>
        {
            _initialRenderRefreshPosted = false;
            ApplyLayoutAndVisualInvalidation();
        }, DispatcherPriority.Loaded);
        Dispatcher.UIThread.Post(ApplyLayoutAndVisualInvalidation, DispatcherPriority.Render);
    }

    public void RequestRenderRefresh() =>
        Dispatcher.UIThread.Post(ApplyLayoutAndVisualInvalidation, DispatcherPriority.Render);

    private void ApplyLayoutAndVisualInvalidation()
    {
        if (_insetsManager is not null)
            RootSafeArea.Padding = _insetsManager.SafeAreaPadding;
        RootSafeArea.InvalidateMeasure();
        RootSafeArea.InvalidateArrange();
        RootSafeArea.InvalidateVisual();
        WorkspaceGrid.InvalidateMeasure();
        WorkspaceGrid.InvalidateArrange();
        WorkspaceGrid.InvalidateVisual();
        InvalidateMeasure();
        InvalidateArrange();
        InvalidateVisual();
        if (HostTopLevel is { } topLevel)
        {
            topLevel.InvalidateMeasure();
            topLevel.InvalidateArrange();
            topLevel.InvalidateVisual();
        }
    }

    private void OnSafeAreaChanged(object? sender, SafeAreaChangedArgs e) =>
        RootSafeArea.Padding = e.SafeAreaPadding;

    private void OnNodeContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        if (_attachedVm is not { } vm) return;
        var node = FindNodeDataContext(e.Source as Visual);
        vm.ContextNode = node;
        if (node is not { HasMenu: true })
            e.Handled = true;
    }

    private static TreeNodeViewModel? FindNodeDataContext(Visual? visual)
    {
        for (var cur = visual; cur is not null; cur = cur.GetVisualParent())
        {
            if (cur is TreeViewItem tvi) return tvi.DataContext as TreeNodeViewModel;
            if (cur is ListBoxItem lbi) return lbi.DataContext as TreeNodeViewModel;
        }
        return null;
    }

    private void OnViewKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.F12 && Vm is { } vm)
        {
            vm.ToggleDebugPanelCommand.Execute(null);
            e.Handled = true;
        }
    }

    /// <summary>AvaloniaEdit documents are not bindable; push text from the view model explicitly.</summary>
    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (Vm is not { } vm) return;
        switch (e.PropertyName)
        {
            case nameof(MainViewModel.CppText):
                if (!string.Equals(CppEditor.Text, vm.CppText, StringComparison.Ordinal))
                    CppEditor.Text = vm.CppText;
                break;
            case nameof(MainViewModel.IlText):
                if (!string.Equals(IlEditor.Text, vm.IlText, StringComparison.Ordinal))
                    IlEditor.Text = vm.IlText;
                break;
            case nameof(MainViewModel.CSharpText):
                if (!string.Equals(CSharpEditor.Text, vm.CSharpText, StringComparison.Ordinal))
                    CSharpEditor.Text = vm.CSharpText;
                break;
            case nameof(MainViewModel.InspectionText):
                if (!string.Equals(InspectionEditor.Text, vm.InspectionText, StringComparison.Ordinal))
                    InspectionEditor.Text = vm.InspectionText;
                break;
        }
    }

    private void OnExit(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Shutdown();
        else if (HostTopLevel is Window window)
            window.Close();
    }

    // ------------------------------------------------------------------ IUiServices

    public async Task<string?> PickFolderAsync(string title)
    {
        if (HostTopLevel?.StorageProvider is not { CanPickFolder: true } sp) return null;
        var result = await sp.OpenFolderPickerAsync(new FolderPickerOpenOptions { Title = title, AllowMultiple = false });
        return result.Count > 0 ? result[0].TryGetLocalPath() : null;
    }

    public async Task<string?> PickFileAsync(string title)
    {
        if (HostTopLevel?.StorageProvider is not { } sp) return null;
        var result = await sp.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("IL2CPP Packages & Binaries")
                {
                    Patterns =
                    [
                        "*.apk", "*.xapk", "*.apks", "*.apkm", "*.ipa", "*.pkg", "*.prx", "*.nsp", "*.dylib",
                        "GameAssembly.dll", "GameAssembly.so", "libil2cpp.so", "Il2CppUserAssemblies.prx", "UnityFramework",
                        "*.wasm", "*.wasm.br", "*.wasm.gz", "*.wasm.unityweb", "*.wasm.code", "*.wasm.code.br", "*.wasm.code.gz", "*.wasm.code.unityweb",
                        "*.data", "*.data.br", "*.data.gz", "*.data.unityweb", "*.datagz", "*.jsgz", "main", "global-metadata.dat"
                    ]
                },
                FilePickerFileTypes.All
            ],
        });
        return result.Count > 0 ? await MaterializeStorageFileAsync(result[0]) : null;
    }

    public async Task<string?> PickKeysetFileAsync(string title)
    {
        if (HostTopLevel?.StorageProvider is not { } sp) return null;
        var result = await sp.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Nintendo Switch Keysets")
                {
                    Patterns = ["prod.keys", "dev.keys", "*.keys", "*.txt"]
                },
                FilePickerFileTypes.All
            ],
        });
        return result.Count > 0 ? await MaterializeStorageFileAsync(result[0]) : null;
    }

    /// <summary>
    /// Uses the local path directly on desktop when present; otherwise copies stream-backed storage files
    /// (such as Android SAF <c>content://</c> picks) into the sandboxed cache directory.
    /// </summary>
    private static async Task<string?> MaterializeStorageFileAsync(IStorageFile file)
    {
        if (file.TryGetLocalPath() is { } local && File.Exists(local))
            return local;

        var rawName = string.IsNullOrWhiteSpace(file.Name) ? "picked.bin" : Path.GetFileName(file.Name);
        var cleanName = string.Concat(rawName.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '_' : ch));
        var dir = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "picked", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        var dest = Path.Combine(dir, cleanName);
        await using var src = await file.OpenReadAsync();
        await using var dst = File.Create(dest);
        await src.CopyToAsync(dst);
        return dest;
    }

    public async Task<string?> PromptTextAsync(string title, string message, string watermark, bool maskInput = false)
    {
        _promptTcs?.TrySetResult(null);
        var tcs = new TaskCompletionSource<string?>();
        _promptTcs = tcs;

        PromptTitle.Text = title;
        PromptMessage.Text = message;
        PromptBox.Text = "";
        PromptBox.PlaceholderText = watermark;
        PromptBox.PasswordChar = maskInput ? '●' : '\0';
        PromptOverlay.IsVisible = true;
        PromptBox.Focus();

        try
        {
            return await tcs.Task;
        }
        finally
        {
            PromptOverlay.IsVisible = false;
            if (ReferenceEquals(_promptTcs, tcs)) _promptTcs = null;
        }
    }

    private void ResolvePrompt(string? result)
    {
        var tcs = _promptTcs;
        _promptTcs = null;
        PromptOverlay.IsVisible = false;
        tcs?.TrySetResult(result);
    }

    public async Task<string?> SaveFileAsync(string title, string suggestedName, string extension)
    {
        if (HostTopLevel?.StorageProvider is not { } sp) return null;
        var file = await sp.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            SuggestedFileName = suggestedName,
            DefaultExtension = extension,
            FileTypeChoices = [new FilePickerFileType(extension.ToUpperInvariant()) { Patterns = [$"*.{extension}"] }],
        });
        return file?.TryGetLocalPath();
    }

    public async Task<string?> SaveTextFileAsync(string title, string suggestedName, string extension, string content)
    {
        if (HostTopLevel?.StorageProvider is not { } sp) return null;
        var file = await sp.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            SuggestedFileName = suggestedName,
            DefaultExtension = extension,
            FileTypeChoices = [new FilePickerFileType(extension.ToUpperInvariant()) { Patterns = [$"*.{extension}"] }],
        });
        if (file is null) return null;

        if (file.TryGetLocalPath() is { } localPath)
        {
            await File.WriteAllTextAsync(localPath, content);
            return localPath;
        }

        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        return file.Name;
    }

    public async Task SetClipboardAsync(string text)
    {
        if (HostTopLevel?.Clipboard is { } cb) await cb.SetTextAsync(text);
    }

    public async Task ShowBootConfigDialogAsync(BootConfigViewModel vm)
    {
        var tcs = new TaskCompletionSource();
        void CloseHandler() => tcs.TrySetResult();

        BootConfigDialogHost.DataContext = vm;
        BootConfigOverlay.IsVisible = true;
        vm.CloseRequested += CloseHandler;
        try
        {
            await tcs.Task;
        }
        finally
        {
            vm.CloseRequested -= CloseHandler;
            BootConfigOverlay.IsVisible = false;
            BootConfigDialogHost.DataContext = null;
        }
    }

    public void Post(Action action) => Dispatcher.UIThread.Post(action);

    public async Task<InstalledAndroidApp?> PickInstalledAppAsync()
    {
        _appPickerTcs?.TrySetResult(null);
        var tcs = new TaskCompletionSource<InstalledAndroidApp?>();
        _appPickerTcs = tcs;

        _scannedApps = [];
        InstalledAppsFilterBox.Text = "";
        InstalledAppsList.ItemsSource = null;
        InstalledAppsOpenButton.IsEnabled = false;
        InstalledAppsOverlay.IsVisible = true;
        _ = RefreshInstalledAppsAsync();

        try
        {
            return await tcs.Task;
        }
        finally
        {
            InstalledAppsOverlay.IsVisible = false;
            if (ReferenceEquals(_appPickerTcs, tcs)) _appPickerTcs = null;
        }
    }

    private async Task RefreshInstalledAppsAsync()
    {
        if (App.InstalledAppScanner is null)
        {
            InstalledAppsStatus.Text = "Installed app scanning is only available on Android.";
            return;
        }

        InstalledAppsRefreshButton.IsEnabled = false;
        InstalledAppsStatus.Text = "Scanning installed apps for Unity IL2CPP packages…";
        try
        {
            var scanned = await App.InstalledAppScanner();
            _scannedApps = scanned;
            ApplyInstalledAppsFilter();
            InstalledAppsStatus.Text = scanned.Count == 0
                ? "No Unity IL2CPP apps found (grant 'Query all packages' permission if restricted)."
                : $"Found {scanned.Count} Unity IL2CPP app(s).";
        }
        catch (Exception ex)
        {
            InstalledAppsStatus.Text = $"Scan failed: {ex.Message}";
        }
        finally
        {
            InstalledAppsRefreshButton.IsEnabled = true;
        }
    }

    private void ApplyInstalledAppsFilter()
    {
        var query = InstalledAppsFilterBox.Text?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(query)
            ? _scannedApps
            : [.. _scannedApps.Where(a =>
                a.Label.Contains(query, StringComparison.OrdinalIgnoreCase)
                || a.PackageName.Contains(query, StringComparison.OrdinalIgnoreCase))];
        InstalledAppsList.ItemsSource = filtered;
        if (filtered.Count > 0 && InstalledAppsList.SelectedItem is null)
            InstalledAppsList.SelectedIndex = 0;
        InstalledAppsOpenButton.IsEnabled = InstalledAppsList.SelectedItem is InstalledAndroidApp;
    }

    private void ResolveInstalledAppPicker(InstalledAndroidApp? app)
    {
        var tcs = _appPickerTcs;
        _appPickerTcs = null;
        InstalledAppsOverlay.IsVisible = false;
        tcs?.TrySetResult(app);
    }
}
