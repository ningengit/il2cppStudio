using System.Text.Json;
using System.Text.Json.Serialization;

namespace Il2CppStudio.Gui.Services;

/// <summary>Persists user-provided local tool settings (e.g. remembered Switch keyset path) without storing keys.</summary>
public sealed class UserSettings
{
    public string? SwitchKeysetPath { get; set; }

    public static string SettingsFilePath
    {
        get
        {
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrWhiteSpace(baseDir))
                baseDir = Path.GetTempPath();
            return Path.Combine(baseDir, "Il2CppStudio", "settings.json");
        }
    }

    public static UserSettings Load()
    {
        try
        {
            var path = SettingsFilePath;
            if (!File.Exists(path)) return new UserSettings();
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize(json, UserSettingsJsonContext.Default.UserSettings) ?? new UserSettings();
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
            File.WriteAllText(path, JsonSerializer.Serialize(this, UserSettingsJsonContext.Default.UserSettings));
        }
        catch
        {
            // Best-effort preference persistence.
        }
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(UserSettings))]
internal sealed partial class UserSettingsJsonContext : JsonSerializerContext;
