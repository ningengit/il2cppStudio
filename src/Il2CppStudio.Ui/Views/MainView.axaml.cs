using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Platform;
using Avalonia.Input;
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
    private static readonly object s_tmHookLock = new();
    private static WeakReference<MainView>? s_activeView;
    private static bool s_tmHookRegistered;

    private MainViewModel? _attachedVm;
    private IInsetsManager? _insetsManager;
    private TaskCompletionSource<string?>? _promptTcs;
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

        PromptOkButton.Click += (_, _) => ResolvePrompt(PromptBox.Text?.Trim());
        PromptCancelButton.Click += (_, _) => ResolvePrompt(null);
        PromptBox.KeyDown += (_, e) =>
        {
            if (e.Key == Key.Enter) { e.Handled = true; ResolvePrompt(PromptBox.Text?.Trim()); }
            else if (e.Key == Key.Escape) { e.Handled = true; ResolvePrompt(null); }
        };
    }

    private TopLevel? HostTopLevel => TopLevel.GetTopLevel(this);

    private void InitializeSyntaxHighlighting()
    {
        InstallManagedHighlighter(IlEditor, SyntaxLanguage.Il);

        lock (s_tmHookLock)
        {
            s_activeView = new WeakReference<MainView>(this);
            if (!s_tmHookRegistered)
            {
                TextMate.RegisterExceptionHandler(OnStaticTextMateException);
                s_tmHookRegistered = true;
            }
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

            _cppTextMate = Highlight(CppEditor, options, ".cpp");
            _csTextMate = Highlight(CSharpEditor, options, ".cs");
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

    private static void OnStaticTextMateException(Exception ex)
    {
        if (s_activeView is null || !s_activeView.TryGetTarget(out var view)) return;
        Dispatcher.UIThread.Post(() => view.HandleRuntimeTextMateFailure(ex));
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

    private static TextMate.Installation Highlight(TextEditor editor, RegistryOptions options, string extension)
    {
        var installation = editor.InstallTextMate(options);
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
        lock (s_tmHookLock)
        {
            s_activeView = new WeakReference<MainView>(this);
        }
        if (HostTopLevel?.InsetsManager is { } insetsManager)
        {
            _insetsManager = insetsManager;
            _insetsManager.SafeAreaChanged += OnSafeAreaChanged;
            RootSafeArea.Padding = _insetsManager.SafeAreaPadding;
        }
        if (Vm is not { } vm) return;
        _attachedVm = vm;
        vm.Ui = this;
        vm.PropertyChanged += OnVmPropertyChanged;
        if (_pendingDiagnostics.Count > 0)
        {
            var queued = _pendingDiagnostics.ToArray();
            _pendingDiagnostics.Clear();
            foreach (var (sev, msg, ex) in queued)
                vm.ReportUiDiagnostic(sev, msg, ex);
        }
    }

    private void Detach()
    {
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

    /// <summary>AvaloniaEdit documents are not bindable; push text from the view model explicitly.</summary>
    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (Vm is not { } vm) return;
        switch (e.PropertyName)
        {
            case nameof(MainViewModel.CppText): CppEditor.Text = vm.CppText; break;
            case nameof(MainViewModel.IlText): IlEditor.Text = vm.IlText; break;
            case nameof(MainViewModel.CSharpText): CSharpEditor.Text = vm.CSharpText; break;
            case nameof(MainViewModel.InspectionText): InspectionEditor.Text = vm.InspectionText; break;
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
                        "*.apk", "*.xapk", "*.ipa", "*.pkg", "*.prx", "*.nsp", "*.dylib",
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
        PromptBox.Watermark = watermark;
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
}
