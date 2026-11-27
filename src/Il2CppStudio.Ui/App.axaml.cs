using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Il2CppStudio.Gui.ViewModels;
using Il2CppStudio.Gui.Views;

namespace Il2CppStudio.Gui;

public sealed class App : Application
{
    private static WeakReference<MainViewModel>? s_activeVm;
    private static bool s_uiExceptionHooked;

    /// <summary>Optional desktop window factory (set by <c>Il2CppStudio.Gui</c> so desktop keeps drag-and-drop &amp; window sizing).</summary>
    public static Func<MainViewModel, Window>? DesktopMainWindowFactory { get; set; }

    /// <summary>Optional installed-app scanner (set by <c>Il2CppStudio.Android</c> to enumerate Unity IL2CPP packages).</summary>
    public static Func<Task<IReadOnlyList<InstalledAndroidApp>>>? InstalledAppScanner { get; set; }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = new MainViewModel();
            HookUiUnhandledExceptions(vm);
            desktop.MainWindow = DesktopMainWindowFactory?.Invoke(vm)
                ?? new Window { Title = "Il2CppStudio", Content = new MainView { DataContext = vm }, DataContext = vm };
            desktop.ShutdownRequested += (_, _) => vm.Dispose();
            // Optional: open a file/folder passed on the command line (same semantics as `il2cppstudio load`).
            if (desktop.Args is { Length: > 0 } args && (Directory.Exists(args[0]) || File.Exists(args[0])))
                _ = vm.LoadFolderAsync(args[0]);
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime activityLifetime)
        {
            var vm = new MainViewModel();
            HookUiUnhandledExceptions(vm);
            activityLifetime.MainViewFactory = () => new MainView { DataContext = vm };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var vm = new MainViewModel();
            HookUiUnhandledExceptions(vm);
            singleView.MainView = new MainView { DataContext = vm };
        }
        base.OnFrameworkInitializationCompleted();
    }

    private static void HookUiUnhandledExceptions(MainViewModel vm)
    {
        s_activeVm = new WeakReference<MainViewModel>(vm);
        if (s_uiExceptionHooked) return;
        s_uiExceptionHooked = true;
        Dispatcher.UIThread.UnhandledException += (_, e) =>
        {
            if (s_activeVm is not null && s_activeVm.TryGetTarget(out var current))
                current.RecordUnhandledUiException(e.Exception);
        };
    }
}
