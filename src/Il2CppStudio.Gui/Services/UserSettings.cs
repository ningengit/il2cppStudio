using System.Text.Json;

namespace Il2CppStudio.Gui.Services;

/// <summary>Persists user-provided local tool settings (e.g. remembered Switch keyset path) without storing keys.</summary>
public sealed class UserSettings
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public string? SwitchKeysetPath { get; set; }

    public static string SettingsFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Il2CppStudio",
        "settings.json");

    public static UserSettings Load()
    {
        try
        {
            var path = SettingsFilePath;
            if (!File.Exists(path)) return new UserSettings();
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<UserSettings>(json, JsonOptions) ?? new UserSettings();
        }
        catch
        {
            return new UserSettings();
        }
    }

    public void Save()
    {
        try
        {
            var path = SettingsFilePath;
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, JsonSerializer.Serialize(this, JsonOptions));
        }
        catch
        {
            // Best-effort preference persistence.
        }
    }
}
