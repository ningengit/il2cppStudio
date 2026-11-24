using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Il2CppStudio.Gui.ViewModels;
using Il2CppStudio.Gui.Views;

namespace Il2CppStudio.Gui;

public sealed class App : Application
{
    /// <summary>Optional desktop window factory (set by <c>Il2CppStudio.Gui</c> so desktop keeps drag-and-drop &amp; window sizing).</summary>
    public static Func<MainViewModel, Window>? DesktopMainWindowFactory { get; set; }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = new MainViewModel();
            desktop.MainWindow = DesktopMainWindowFactory?.Invoke(vm)
                ?? new Window { Title = "Il2CppStudio", Content = new MainView { DataContext = vm }, DataContext = vm };
            desktop.ShutdownRequested += (_, _) => vm.Dispose();
            // Optional: open a file/folder passed on the command line (same semantics as `il2cppstudio load`).
            if (desktop.Args is { Length: > 0 } args && (Directory.Exists(args[0]) || File.Exists(args[0])))
                _ = vm.LoadFolderAsync(args[0]);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var vm = new MainViewModel();
            singleView.MainView = new MainView { DataContext = vm };
        }
        base.OnFrameworkInitializationCompleted();
    }
}
