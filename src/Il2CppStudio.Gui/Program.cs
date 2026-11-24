using Avalonia;

namespace Il2CppStudio.Gui;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.DesktopMainWindowFactory = vm => new Views.MainWindow { DataContext = vm };
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .WithInterFont()
        .LogToTrace();
}
