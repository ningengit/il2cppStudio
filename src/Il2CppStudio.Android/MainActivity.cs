using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Il2CppStudio.Gui;

namespace Il2CppStudio.Android;

[Activity(
    Label = "Il2CppStudio",
    Theme = "@style/MyTheme.NoActionBar",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        if (CacheDir?.AbsolutePath is { Length: > 0 } cacheDir)
            Environment.SetEnvironmentVariable("TMPDIR", cacheDir);
        return base.CustomizeAppBuilder(builder).WithInterFont();
    }
}
