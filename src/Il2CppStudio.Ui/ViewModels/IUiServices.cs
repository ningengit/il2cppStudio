namespace Il2CppStudio.Gui.ViewModels;

/// <summary>Host-level UI services the view models need (implemented by the main view across desktop and mobile).</summary>
public interface IUiServices
{
    Task<string?> PickFolderAsync(string title);
    Task<string?> PickFileAsync(string title);
    Task<string?> PickKeysetFileAsync(string title);
    Task<string?> PromptTextAsync(string title, string message, string watermark, bool maskInput = false);
    Task<string?> SaveFileAsync(string title, string suggestedName, string extension);
    Task<string?> SaveTextFileAsync(string title, string suggestedName, string extension, string content);
    Task SetClipboardAsync(string text);
    Task ShowBootConfigDialogAsync(BootConfigViewModel vm);
    void Post(Action action);
}
