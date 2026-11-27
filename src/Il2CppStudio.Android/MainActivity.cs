using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;
using Avalonia.Android;

namespace Il2CppStudio.Android;

[Activity(
    Label = "Il2CppStudio",
    Theme = "@style/MyTheme.NoActionBar",
    MainLauncher = true,
    ResizeableActivity = true,
    ScreenOrientation = ScreenOrientation.FullUser,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode | ConfigChanges.Density | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Navigation)]
public class MainActivity : AvaloniaMainActivity
{
    private SystemBarsInsetsListener? _insetsListener;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Window?.Attributes is { } attrs)
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(30))
                attrs.LayoutInDisplayCutoutMode = global::Android.Views.LayoutInDisplayCutoutMode.Always;
            else if (OperatingSystem.IsAndroidVersionAtLeast(28))
                attrs.LayoutInDisplayCutoutMode = global::Android.Views.LayoutInDisplayCutoutMode.ShortEdges;
            Window.Attributes = attrs;
        }

        if (Window?.DecorView is { } decorView)
        {
            decorView.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#1E1E1E"));
            if (WindowCompat.GetInsetsController(Window, decorView) is { } controller)
            {
                controller.AppearanceLightStatusBars = false;
                controller.AppearanceLightNavigationBars = false;
            }
        }

        if (FindViewById(global::Android.Resource.Id.Content) is { } contentView)
        {
            contentView.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#1E1E1E"));
            if (contentView is global::Android.Views.ViewGroup vg)
            {
                for (var i = 0; i < vg.ChildCount; i++)
                {
                    if (vg.GetChildAt(i) is AvaloniaView av)
                    {
                        if (av.LayoutParameters is { } lp)
                        {
                            lp.Width = global::Android.Views.ViewGroup.LayoutParams.MatchParent;
                            lp.Height = global::Android.Views.ViewGroup.LayoutParams.MatchParent;
                            av.LayoutParameters = lp;
                        }
                        else
                        {
                            av.LayoutParameters = new global::Android.Widget.FrameLayout.LayoutParams(
                                global::Android.Views.ViewGroup.LayoutParams.MatchParent,
                                global::Android.Views.ViewGroup.LayoutParams.MatchParent);
                        }
                    }
                }
            }
            if (!OperatingSystem.IsAndroidVersionAtLeast(35))
            {
                _insetsListener = new SystemBarsInsetsListener();
                ViewCompat.SetOnApplyWindowInsetsListener(contentView, _insetsListener);
                ViewCompat.RequestApplyInsets(contentView);
            }
        }
    }

    protected override void OnDestroy()
    {
        if (_insetsListener is not null)
        {
            if (FindViewById(global::Android.Resource.Id.Content) is { } contentView)
                ViewCompat.SetOnApplyWindowInsetsListener(contentView, null);
            _insetsListener.Dispose();
            _insetsListener = null;
        }
        base.OnDestroy();
    }

    private sealed class SystemBarsInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        public WindowInsetsCompat? OnApplyWindowInsets(global::Android.Views.View? v, WindowInsetsCompat? insets)
        {
            if (v is not null && insets is not null)
            {
                var bars = insets.GetInsets(
                    WindowInsetsCompat.Type.SystemBars()
                    | WindowInsetsCompat.Type.DisplayCutout()
                    | WindowInsetsCompat.Type.CaptionBar());
                if (bars is not null)
                    v.SetPadding(bars.Left, bars.Top, bars.Right, bars.Bottom);
            }
            return insets;
        }
    }
}
