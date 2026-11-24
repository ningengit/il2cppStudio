using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;
using Avalonia;
using Avalonia.Android;
using Il2CppStudio.Gui;

namespace Il2CppStudio.Android;

[Activity(
    Label = "Il2CppStudio",
    Theme = "@style/MyTheme.NoActionBar",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    private SystemBarsInsetsListener? _insetsListener;

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        if (CacheDir?.AbsolutePath is { Length: > 0 } cacheDir)
            global::System.Environment.SetEnvironmentVariable("TMPDIR", cacheDir);
        return base.CustomizeAppBuilder(builder).WithInterFont();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Window?.DecorView is { } decorView && WindowCompat.GetInsetsController(Window, decorView) is { } controller)
        {
            controller.AppearanceLightStatusBars = false;
            controller.AppearanceLightNavigationBars = false;
        }

        if (FindViewById(global::Android.Resource.Id.Content) is { } contentView)
        {
            contentView.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#1E1E1E"));
            _insetsListener = new SystemBarsInsetsListener();
            ViewCompat.SetOnApplyWindowInsetsListener(contentView, _insetsListener);
            ViewCompat.RequestApplyInsets(contentView);
        }
    }

    protected override void OnDestroy()
    {
        _insetsListener = null;
        base.OnDestroy();
    }

    private sealed class SystemBarsInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        public WindowInsetsCompat OnApplyWindowInsets(global::Android.Views.View v, WindowInsetsCompat insets)
        {
            if (v is not null && insets is not null)
            {
                var bars = insets.GetInsets(
                    WindowInsetsCompat.Type.SystemBars()
                    | WindowInsetsCompat.Type.DisplayCutout()
                    | WindowInsetsCompat.Type.CaptionBar());
                v.SetPadding(bars.Left, bars.Top, bars.Right, bars.Bottom);
            }
            return insets!;
        }
    }
}
