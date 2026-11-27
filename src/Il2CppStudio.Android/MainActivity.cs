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

        Window?.SetBackgroundDrawable(
            new global::Android.Graphics.Drawables.ColorDrawable(global::Android.Graphics.Color.ParseColor("#1E1E1E")));
        ApplyWindowChrome();

        if (FindViewById(global::Android.Resource.Id.Content) is { } contentView)
        {
            // Keep the host FrameLayout transparent so AvaloniaView's SurfaceView hole-punch renders visibly on cold start.
            contentView.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            EnsureAvaloniaViewMatchParent(contentView);

            if (!OperatingSystem.IsAndroidVersionAtLeast(35))
            {
                _insetsListener = new SystemBarsInsetsListener();
                ViewCompat.SetOnApplyWindowInsetsListener(contentView, _insetsListener);
                ViewCompat.RequestApplyInsets(contentView);
            }
        }

        ScheduleColdLaunchVisualSync();
    }

    protected override void OnResume()
    {
        base.OnResume();
        ApplyWindowChrome();
        ScheduleColdLaunchVisualSync();
    }

    public override void OnWindowFocusChanged(bool hasFocus)
    {
        base.OnWindowFocusChanged(hasFocus);
        if (hasFocus)
        {
            ApplyWindowChrome();
            ScheduleColdLaunchVisualSync();
        }
    }

    private void ApplyWindowChrome()
    {
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
            decorView.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            if (WindowCompat.GetInsetsController(Window, decorView) is { } controller)
            {
                controller.AppearanceLightStatusBars = false;
                controller.AppearanceLightNavigationBars = false;
            }
        }
    }

    private static void EnsureAvaloniaViewMatchParent(global::Android.Views.View contentView)
    {
        if (contentView is not global::Android.Views.ViewGroup vg) return;
        for (var i = 0; i < vg.ChildCount; i++)
        {
            if (vg.GetChildAt(i) is not AvaloniaView av) continue;
            SetMatchParentKeepingLayoutParamsType(av, useFrameLayoutFallback: true);
            for (var j = 0; j < av.ChildCount; j++)
            {
                if (av.GetChildAt(j) is global::Android.Views.SurfaceView sv)
                    SetMatchParentKeepingLayoutParamsType(sv, useFrameLayoutFallback: true);
            }
        }
    }

    private static void SetMatchParentKeepingLayoutParamsType(global::Android.Views.View view, bool useFrameLayoutFallback)
    {
        if (view.LayoutParameters is { } lp)
        {
            if (lp.Width != global::Android.Views.ViewGroup.LayoutParams.MatchParent
                || lp.Height != global::Android.Views.ViewGroup.LayoutParams.MatchParent)
            {
                lp.Width = global::Android.Views.ViewGroup.LayoutParams.MatchParent;
                lp.Height = global::Android.Views.ViewGroup.LayoutParams.MatchParent;
                view.LayoutParameters = lp;
            }
        }
        else if (useFrameLayoutFallback)
        {
            view.LayoutParameters = new global::Android.Widget.FrameLayout.LayoutParams(
                global::Android.Views.ViewGroup.LayoutParams.MatchParent,
                global::Android.Views.ViewGroup.LayoutParams.MatchParent);
        }
    }

    private void ScheduleColdLaunchVisualSync()
    {
        if (FindViewById(global::Android.Resource.Id.Content) is not { } contentView)
            return;

        contentView.Post(() =>
        {
            if (IsFinishing || IsDestroyed)
                return;

            if (Window?.DecorView is { } decorView)
                ViewCompat.RequestApplyInsets(decorView);
            ViewCompat.RequestApplyInsets(contentView);

            if (contentView is global::Android.Views.ViewGroup vg)
            {
                for (var i = 0; i < vg.ChildCount; i++)
                {
                    if (vg.GetChildAt(i) is not AvaloniaView av) continue;
                    SetMatchParentKeepingLayoutParamsType(av, useFrameLayoutFallback: true);
                    av.RequestLayout();
                    av.Invalidate();
                    for (var j = 0; j < av.ChildCount; j++)
                    {
                        if (av.GetChildAt(j) is global::Android.Views.SurfaceView sv)
                        {
                            SetMatchParentKeepingLayoutParamsType(sv, useFrameLayoutFallback: true);
                            sv.RequestLayout();
                            sv.Invalidate();
                        }
                    }
                    if (av.Content is Il2CppStudio.Gui.Views.MainView mainView)
                        mainView.RequestRenderRefresh();
                }
            }
        });
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
