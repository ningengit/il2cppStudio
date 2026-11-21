using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using Il2CppStudio.Gui.ViewModels;
using TextMateSharp.Grammars;

namespace Il2CppStudio.Gui.Views;

public sealed partial class MainWindow : Window, IUiServices
{
    public MainWindow()
    {
        InitializeComponent();
        var options = new RegistryOptions(ThemeName.DarkPlus);
        Highlight(CppEditor, options, ".cpp");
        Highlight(CSharpEditor, options, ".cs");
        DataContextChanged += (_, _) => Attach();
        Opened += (_, _) => FitToScreen();
        AddHandler(DragDrop.DragOverEvent, OnDragOver);
        AddHandler(DragDrop.DragLeaveEvent, OnDragLeave);
        AddHandler(DragDrop.DropEvent, OnDrop);
    }

    /// <summary>
    /// Keeps the whole window — including the OS title bar with minimize/maximize/close — on screen. With display
    /// scaling (125–175 %) a fixed size can exceed the monitor's work area; a centered window then has its title bar
    /// above the top edge, so it looks undecorated and cannot be dragged.
    /// </summary>
    private void FitToScreen()
    {
        var screen = Screens.ScreenFromWindow(this) ?? Screens.Primary;
        if (screen is null) return;
        var scale = screen.Scaling;
        var area = screen.WorkingArea;   // physical pixels, excludes the taskbar
        var maxW = area.Width / scale * 0.95;
        var maxH = area.Height / scale * 0.92;
        if (Width > maxW) Width = maxW;
        if (Height > maxH) Height = maxH;
        Position = new Avalonia.PixelPoint(
            area.X + (int)((area.Width - Width * scale) / 2),
            area.Y + (int)((area.Height - Height * scale) / 2));
    }

    private static void Highlight(TextEditor editor, RegistryOptions options, string extension)
    {
        var installation = editor.InstallTextMate(options);
        installation.SetGrammar(options.GetScopeByLanguageId(options.GetLanguageByExtension(extension).Id));
    }

    private MainViewModel? Vm => DataContext as MainViewModel;

    private void Attach()
    {
        if (Vm is not { } vm) return;
        vm.Ui = this;
        vm.PropertyChanged += OnVmPropertyChanged;
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

    private void OnExit(object? sender, RoutedEventArgs e) => Close();

    // ------------------------------------------------------------------ IUiServices

    public async Task<string?> PickFolderAsync(string title)
    {
        var result = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { Title = title, AllowMultiple = false });
        return result.Count > 0 ? result[0].TryGetLocalPath() : null;
    }

    public async Task<string?> PickFileAsync(string title)
    {
        var result = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("IL2CPP Packages & Binaries")
                {
                    Patterns = ["*.apk", "*.xapk", "*.pkg", "*.prx", "*.nsp", "GameAssembly.dll", "GameAssembly.so", "libil2cpp.so", "Il2CppUserAssemblies.prx", "*.wasm", "*.wasm.br", "*.wasm.gz", "*.wasm.unityweb", "*.wasm.code", "*.wasm.code.br", "*.wasm.code.gz", "*.wasm.code.unityweb", "*.data", "*.data.br", "*.data.gz", "*.data.unityweb", "main", "global-metadata.dat"]
                },
                FilePickerFileTypes.All
            ],
        });
        return result.Count > 0 ? result[0].TryGetLocalPath() : null;
    }

    public async Task<string?> PickKeysetFileAsync(string title)
    {
        var result = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
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
        return result.Count > 0 ? result[0].TryGetLocalPath() : null;
    }

    public async Task<string?> PromptTextAsync(string title, string message, string watermark, bool maskInput = false)
    {
        var tcs = new TaskCompletionSource<string?>();
        var box = new TextBox
        {
            Watermark = watermark,
            Width = 440,
            PasswordChar = maskInput ? '●' : '\0',
        };
        var ok = new Button { Content = "OK", IsDefault = true, MinWidth = 80 };
        var cancel = new Button { Content = "Cancel", IsCancel = true, MinWidth = 80 };
        var buttons = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            Spacing = 8,
            Children = { cancel, ok },
        };
        var panel = new StackPanel
        {
            Margin = new Avalonia.Thickness(16),
            Spacing = 12,
            Children =
            {
                new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap, MaxWidth = 440 },
                box,
                buttons,
            },
        };
        var dialog = new Window
        {
            Title = title,
            CanResize = false,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = panel,
        };
        ok.Click += (_, _) => { tcs.TrySetResult(box.Text?.Trim()); dialog.Close(); };
        cancel.Click += (_, _) => { tcs.TrySetResult(null); dialog.Close(); };
        dialog.Closed += (_, _) => tcs.TrySetResult(null);
        await dialog.ShowDialog(this);
        return await tcs.Task;
    }

    public async Task<string?> SaveFileAsync(string title, string suggestedName, string extension)
    {
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            SuggestedFileName = suggestedName,
            DefaultExtension = extension,
            FileTypeChoices = [new FilePickerFileType(extension.ToUpperInvariant()) { Patterns = [$"*.{extension}"] }],
        });
        return file?.TryGetLocalPath();
    }

    public async Task SetClipboardAsync(string text)
    {
        if (Clipboard is { } cb) await cb.SetTextAsync(text);
    }

    public async Task ShowBootConfigDialogAsync(BootConfigViewModel vm)
    {
        var dialog = new BootConfigWindow { DataContext = vm };
        vm.CloseRequested += dialog.Close;
        await dialog.ShowDialog(this);
    }

    public void Post(Action action) => Dispatcher.UIThread.Post(action);

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (ExtractSupportedDropPath(e) is not null)
        {
            e.DragEffects = DragDropEffects.Copy;
            DropOverlay.IsVisible = true;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
            DropOverlay.IsVisible = false;
        }
        e.Handled = true;
    }

    private void OnDragLeave(object? sender, RoutedEventArgs e)
    {
        DropOverlay.IsVisible = false;
    }

    private async void OnDrop(object? sender, DragEventArgs e)
    {
        DropOverlay.IsVisible = false;
        var path = ExtractSupportedDropPath(e);
        if (path is null || DataContext is not MainViewModel vm) return;
        e.Handled = true;
        try { await vm.LoadPathAsync(path); }
        catch (Exception ex) { vm.Status = $"Failed to load dropped item: {ex.Message}"; }
    }

    private static string? ExtractSupportedDropPath(DragEventArgs e)
    {
        var items = e.Data.GetFiles();
        if (items is null) return null;
        foreach (var item in items)
        {
            var path = item.TryGetLocalPath();
            if (string.IsNullOrWhiteSpace(path)) continue;
            if (Directory.Exists(path) || IsSupportedDropFile(path))
                return path;
        }
        return null;
    }

    private static bool IsSupportedDropFile(string path)
    {
        var name = Path.GetFileName(path);
        if (string.Equals(name, "main", StringComparison.OrdinalIgnoreCase)) return true;
        if (name.Contains(".wasm", StringComparison.OrdinalIgnoreCase)
            || name.Contains(".data", StringComparison.OrdinalIgnoreCase))
            return !name.EndsWith(".map", StringComparison.OrdinalIgnoreCase)
                && !name.Contains(".framework.", StringComparison.OrdinalIgnoreCase);
        var ext = Path.GetExtension(name);
        return ext.Equals(".apk", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".xapk", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".nsp", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".pkg", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".prx", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".elf", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".self", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".dll", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".so", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".dylib", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".dat", StringComparison.OrdinalIgnoreCase);
    }
}
