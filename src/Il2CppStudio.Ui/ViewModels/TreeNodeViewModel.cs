using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Gui.ViewModels;

public enum NodeKind { Assembly, Namespace, Type, Method, Property, Field, Event, Placeholder }

/// <summary>
/// One node of the ILSpy-style assembly tree or virtualized search list. Leaf nodes share a single static
/// empty child array and context-menu commands are routed through <see cref="MainViewModel"/> so thousands of
/// nodes allocate minimal memory and retain no closure references.
/// </summary>
public sealed partial class TreeNodeViewModel : ObservableObject
{
    private static readonly IReadOnlyList<TreeNodeViewModel> EmptyChildren = [];
    private static readonly TreeNodeViewModel PlaceholderNode = new(NodeKind.Placeholder, "…");

    private Func<IEnumerable<TreeNodeViewModel>>? _childFactory;
    private ObservableCollection<TreeNodeViewModel>? _children;
    private bool _childrenLoaded;

    public NodeKind Kind { get; }
    public string Text { get; }
    public string? Subtitle { get; init; }
    public bool HasSubtitle => !string.IsNullOrEmpty(Subtitle);

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
    public IReadOnlyList<TreeNodeViewModel> Children => _children ?? EmptyChildren;

    [ObservableProperty] private bool _isExpanded;

    public TreeNodeViewModel(NodeKind kind, string text, Func<IEnumerable<TreeNodeViewModel>>? children = null)
    {
        Kind = kind;
        Text = text;
        _childFactory = children;
        if (children is not null)
            _children = [PlaceholderNode];
    }

    partial void OnIsExpandedChanged(bool value)
    {
        if (!value || _childrenLoaded || _childFactory is null || _children is null) return;
        _childrenLoaded = true;
        var factory = _childFactory;
        _childFactory = null;
        _children.Clear();
        foreach (var c in factory()) _children.Add(c);
    }

    public void ReleaseChildren()
    {
        _childFactory = null;
        if (_children is null) return;
        foreach (var c in _children)
        {
            if (!ReferenceEquals(c, PlaceholderNode))
                c.ReleaseChildren();
        }
        _children.Clear();
    }
}
