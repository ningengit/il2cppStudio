using System.ComponentModel;
using Avalonia.Controls;
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
}
