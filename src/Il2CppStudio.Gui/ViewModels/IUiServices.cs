namespace Il2CppStudio.Gui.ViewModels;

/// <summary>Window-level services the view models need (implemented by the main window).</summary>
public interface IUiServices
{
    Task<string?> PickFolderAsync(string title);
    Task<string?> SaveFileAsync(string title, string suggestedName, string extension);
    Task SetClipboardAsync(string text);
    Task ShowBootConfigDialogAsync(BootConfigViewModel vm);
    void Post(Action action);
}
