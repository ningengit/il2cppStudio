using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using Il2CppStudio.Gui.ViewModels;
using TextMateSharp.Grammars;

namespace Il2CppStudio.Gui.Views;

public sealed partial class MainView : UserControl, IUiServices
{
    private MainViewModel? _attachedVm;
    private TaskCompletionSource<string?>? _promptTcs;

    public MainView()
    {
        InitializeComponent();
        try
        {
            var options = new RegistryOptions(ThemeName.DarkPlus);
            Highlight(CppEditor, options, ".cpp");
            Highlight(CSharpEditor, options, ".cs");
        }
        catch
        {
            // Keep editors usable even if TextMate grammar resources are unavailable on a target platform.
        }

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

    private static void Highlight(TextEditor editor, RegistryOptions options, string extension)
    {
        var installation = editor.InstallTextMate(options);
        installation.SetGrammar(options.GetScopeByLanguageId(options.GetLanguageByExtension(extension).Id));
    }

    private MainViewModel? Vm => DataContext as MainViewModel;

    private void Attach()
    {
        Detach();
        if (Vm is not { } vm) return;
        _attachedVm = vm;
        vm.Ui = this;
        vm.PropertyChanged += OnVmPropertyChanged;
    }

    private void Detach()
    {
        if (_attachedVm is null) return;
        _attachedVm.PropertyChanged -= OnVmPropertyChanged;
        if (ReferenceEquals(_attachedVm.Ui, this))
            _attachedVm.Ui = null;
        _attachedVm = null;
    }

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
                        "*.data", "*.data.br", "*.data.gz", "*.data.unityweb", "main", "global-metadata.dat"
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
