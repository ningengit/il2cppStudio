using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Gui.ViewModels;

public enum NodeKind { Assembly, Namespace, Type, Method, Property, Field, Event, Placeholder }

/// <summary>
/// One node of the ILSpy-style assembly tree. Children are created lazily on first expansion so trees with
/// tens of thousands of members stay responsive. Context-menu commands delegate to the main view model.
/// </summary>
public sealed partial class TreeNodeViewModel : ObservableObject
{
    private readonly MainViewModel _owner;
    private readonly Func<IEnumerable<TreeNodeViewModel>>? _childFactory;
    private bool _childrenLoaded;

    public NodeKind Kind { get; }
    public string Text { get; }
    public string Icon => Kind switch
    {
        NodeKind.Assembly => "▣", NodeKind.Namespace => "{}", NodeKind.Type => "◆", NodeKind.Method => "ƒ",
        NodeKind.Property => "▸", NodeKind.Field => "▪", NodeKind.Event => "⚡", _ => "",
    };
    public AssemblyModel? Assembly { get; init; }
    public TypeModel? Type { get; init; }
    public MethodModel? Method { get; init; }
    public bool IsMethod => Kind == NodeKind.Method;
    public bool HasCode => Kind is NodeKind.Type or NodeKind.Method;
    public bool IsAssembly => Kind == NodeKind.Assembly && Assembly is not null;
    public bool HasMenu => HasCode || IsAssembly;
    public ObservableCollection<TreeNodeViewModel> Children { get; } = [];

    [ObservableProperty] private bool _isExpanded;

    public TreeNodeViewModel(MainViewModel owner, NodeKind kind, string text, Func<IEnumerable<TreeNodeViewModel>>? children = null)
    {
        _owner = owner;
        Kind = kind;
        Text = text;
        _childFactory = children;
        if (children is not null) Children.Add(new TreeNodeViewModel(owner, NodeKind.Placeholder, "…"));
    }

    partial void OnIsExpandedChanged(bool value)
    {
        if (!value || _childrenLoaded || _childFactory is null) return;
        _childrenLoaded = true;
        Children.Clear();
        foreach (var c in _childFactory()) Children.Add(c);
    }

    [RelayCommand] private Task CopyRva() => _owner.CopyRvaAsync(this);
    [RelayCommand] private Task ViewInHxd() => _owner.ViewInHxdAsync(this);
    [RelayCommand] private Task ExportCSharp() => _owner.ExportNodeAsync(this, Core.Workspace.CodeView.CSharp);
    [RelayCommand] private Task ExportCpp() => _owner.ExportNodeAsync(this, Core.Workspace.CodeView.Cpp);
    [RelayCommand] private Task ExportAssembly() => _owner.ExportAssemblyAsync(this, withReferences: false);
    [RelayCommand] private Task ExportAssemblyWithReferences() => _owner.ExportAssemblyAsync(this, withReferences: true);
}
