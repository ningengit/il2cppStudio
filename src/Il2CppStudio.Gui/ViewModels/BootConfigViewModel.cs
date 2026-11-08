using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Il2CppStudio.Core.Config;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Gui.ViewModels;

public sealed partial class BootConfigEntry : ObservableObject
{
    public required string Key { get; init; }
    public required string Recommended { get; init; }
    public string? Current { get; init; }
    [ObservableProperty] private bool _enabled = true;
    [ObservableProperty] private string _value = "";
}

/// <summary>Tools → Configure Diagnostic Boot Flags. Same operations as `il2cppstudio enable-debug`.</summary>
public sealed partial class BootConfigViewModel : ObservableObject
{
    private readonly string _path;
    private readonly IDiagnosticSink _sink;

    public string FilePath { get; }
    public ObservableCollection<BootConfigEntry> Entries { get; } = [];
    [ObservableProperty] private string _status = "";

    public event Action? CloseRequested;

    public BootConfigViewModel(string gamePath, IDiagnosticSink sink)
    {
        _path = gamePath;
        _sink = sink;
        var cfg = BootConfigManager.Open(gamePath, sink);
        FilePath = cfg.Path;
        foreach (var (k, v) in BootConfigManager.DiagnosticFlags)
            Entries.Add(new BootConfigEntry { Key = k, Recommended = v, Current = cfg.Get(k), Value = v });
    }

    [RelayCommand]
    private void Apply()
    {
        var cfg = BootConfigManager.Open(_path, _sink);
        var changes = Entries.Where(e => e.Enabled).Select(e => cfg.Set(e.Key, e.Value.Trim())).OfType<BootConfigChange>().ToList();
        if (changes.Count > 0) cfg.Save();
        Status = changes.Count == 0 ? "Already configured; no changes." : $"Saved {changes.Count} change(s): " + string.Join(", ", changes.Select(c => $"{c.Key}={c.NewValue}"));
    }

    [RelayCommand]
    private void Restore() =>
        Status = BootConfigManager.RestoreBackup(_path, _sink) ? "boot.config restored from backup." : "No backup to restore.";

    [RelayCommand]
    private void Close() => CloseRequested?.Invoke();
}
