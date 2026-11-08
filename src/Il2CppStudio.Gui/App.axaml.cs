using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Il2CppStudio.Gui.ViewModels;
using Il2CppStudio.Gui.Views;

namespace Il2CppStudio.Gui;

public sealed class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = new MainViewModel();
            desktop.MainWindow = new MainWindow { DataContext = vm };
            desktop.ShutdownRequested += (_, _) => vm.Dispose();
            // Optional: open a folder passed on the command line (same semantics as `il2cppstudio load`).
            if (desktop.Args is { Length: > 0 } args && Directory.Exists(args[0])) _ = vm.LoadFolderAsync(args[0]);
        }
        base.OnFrameworkInitializationCompleted();
    }
}
