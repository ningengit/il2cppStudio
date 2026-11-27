using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Il2CppStudio.Gui.ViewModels;

namespace Il2CppStudio.Gui.Views;

public sealed partial class MainWindow : Window
{
    private int _dragToken;

    public MainWindow()
    {
        InitializeComponent();
        Opened += (_, _) => FitToScreen();
        AddHandler(DragDrop.DragEnterEvent, OnDragOver, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
        AddHandler(DragDrop.DragOverEvent, OnDragOver, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
        AddHandler(DragDrop.DragLeaveEvent, OnDragLeave, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
        AddHandler(DragDrop.DropEvent, OnDrop, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
        Deactivated += (_, _) => { ++_dragToken; HideDropOverlay(); };
        PointerMoved += (_, _) => { if (DropOverlay.IsVisible) { ++_dragToken; HideDropOverlay(); } };
    }

    private void HideDropOverlay() => DropOverlay.IsVisible = false;

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
        var area = screen.WorkingArea;
        var maxW = area.Width / scale * 0.95;
        var maxH = area.Height / scale * 0.92;
        if (Width > maxW) Width = maxW;
        if (Height > maxH) Height = maxH;
        Position = new Avalonia.PixelPoint(
            area.X + (int)((area.Width - Width * scale) / 2),
            area.Y + (int)((area.Height - Height * scale) / 2));
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        ++_dragToken;
        if (ExtractSupportedDropPath(e) is not null)
        {
            e.DragEffects = DragDropEffects.Copy;
            DropOverlay.IsVisible = true;
            e.Handled = true;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
            HideDropOverlay();
            e.Handled = true;
        }
    }

    private void OnDragLeave(object? sender, RoutedEventArgs e)
    {
        var token = ++_dragToken;
        Dispatcher.UIThread.Post(() =>
        {
            if (_dragToken == token)
                HideDropOverlay();
        }, DispatcherPriority.Input);
    }

    private async void OnDrop(object? sender, DragEventArgs e)
    {
        ++_dragToken;
        HideDropOverlay();
        var path = ExtractSupportedDropPath(e);
        if (path is null || DataContext is not MainViewModel vm) return;
        e.Handled = true;
        try { await vm.LoadPathAsync(path); }
        catch (Exception ex) { vm.Status = $"Failed to load dropped item: {ex.Message}"; }
    }

    private static string? ExtractSupportedDropPath(DragEventArgs e)
    {
        var items = e.DataTransfer.TryGetFiles();
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
        if (string.Equals(name, "main", StringComparison.OrdinalIgnoreCase)
            || string.Equals(name, "UnityFramework", StringComparison.OrdinalIgnoreCase))
            return true;
        if (name.Contains(".wasm", StringComparison.OrdinalIgnoreCase)
            || name.Contains(".data", StringComparison.OrdinalIgnoreCase))
            return !name.EndsWith(".map", StringComparison.OrdinalIgnoreCase)
                && !name.Contains(".framework.", StringComparison.OrdinalIgnoreCase);
        var ext = Path.GetExtension(name);
        return ext.Equals(".apk", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".xapk", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".apks", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".apkm", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".ipa", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".nsp", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".pkg", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".prx", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".elf", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".self", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".exe", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".dll", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".so", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".dylib", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".dat", StringComparison.OrdinalIgnoreCase)
            || ext.Equals(".jsgz", StringComparison.OrdinalIgnoreCase);
    }
}
