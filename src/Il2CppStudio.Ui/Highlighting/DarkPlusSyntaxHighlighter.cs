using Avalonia.Media;
using Avalonia.Media.Immutable;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;

namespace Il2CppStudio.Gui.Highlighting;

/// <summary>
/// Pure-C# VS Code Dark+ <see cref="DocumentColorizingTransformer"/> for AvaloniaEdit.
/// Requires no native Oniguruma library and no reflection/JSON deserialization, working identically on Desktop and Android.
/// </summary>
public sealed class DarkPlusSyntaxHighlighter(SyntaxLanguage language) : DocumentColorizingTransformer
{
    private static readonly IBrush KeywordBrush = new ImmutableSolidColorBrush(Color.FromRgb(0x56, 0x9C, 0xD6));
    private static readonly IBrush ControlBrush = new ImmutableSolidColorBrush(Color.FromRgb(0xC5, 0x86, 0xC0));
    private static readonly IBrush TypeBrush = new ImmutableSolidColorBrush(Color.FromRgb(0x4E, 0xC9, 0xB0));
    private static readonly IBrush MethodBrush = new ImmutableSolidColorBrush(Color.FromRgb(0xDC, 0xDC, 0xAA));
    private static readonly IBrush StringBrush = new ImmutableSolidColorBrush(Color.FromRgb(0xCE, 0x91, 0x78));
    private static readonly IBrush NumberBrush = new ImmutableSolidColorBrush(Color.FromRgb(0xB5, 0xCE, 0xA8));
    private static readonly IBrush CommentBrush = new ImmutableSolidColorBrush(Color.FromRgb(0x6A, 0x99, 0x55));
    private static readonly IBrush RegisterBrush = new ImmutableSolidColorBrush(Color.FromRgb(0x9C, 0xDC, 0xFE));

    private readonly List<SyntaxTokenSpan> _tokens = [];
    private TextDocument? _cachedDoc;
    private ITextSourceVersion? _cachedVersion;
    private bool[] _lineStartsInBlockComment = [];

    protected override void ColorizeLine(DocumentLine line)
    {
        if (line.Length == 0 || CurrentContext?.Document is not { } doc) return;

        EnsureBlockCommentStates(doc);
        var lineIndex = line.LineNumber - 1;
        var inBlock = (uint)lineIndex < (uint)_lineStartsInBlockComment.Length && _lineStartsInBlockComment[lineIndex];
        var text = doc.GetText(line);

        DarkPlusTokenizer.TokenizeLine(text, language, inBlock, out _, _tokens);
        var lineStart = line.Offset;
        for (var i = 0; i < _tokens.Count; i++)
        {
            var tok = _tokens[i];
            if (tok.Length <= 0) continue;
            var brush = BrushFor(tok.Kind);
            ChangeLinePart(lineStart + tok.Start, lineStart + tok.Start + tok.Length,
                element => element.TextRunProperties.SetForegroundBrush(brush));
        }
    }

    private void EnsureBlockCommentStates(TextDocument doc)
    {
        var ver = doc.Version;
        if (ReferenceEquals(_cachedDoc, doc) && ReferenceEquals(_cachedVersion, ver)) return;

        var count = doc.LineCount;
        var states = _lineStartsInBlockComment.Length >= count ? _lineStartsInBlockComment : new bool[count];
        var inBlock = false;
        var scanTokens = new List<SyntaxTokenSpan>();
        var idx = 0;
        for (var cur = doc.GetLineByNumber(1); cur is not null && idx < count; cur = cur.NextLine, idx++)
        {
            states[idx] = inBlock;
            if (!inBlock && cur.Length < 2) continue;
            var lineText = doc.GetText(cur);
            if (inBlock || lineText.Contains("/*", StringComparison.Ordinal))
                DarkPlusTokenizer.TokenizeLine(lineText, language, inBlock, out inBlock, scanTokens);
        }

        _lineStartsInBlockComment = states;
        _cachedDoc = doc;
        _cachedVersion = ver;
    }

    private static IBrush BrushFor(SyntaxTokenKind kind) => kind switch
    {
        SyntaxTokenKind.Keyword => KeywordBrush,
        SyntaxTokenKind.ControlKeyword or SyntaxTokenKind.Directive => ControlBrush,
        SyntaxTokenKind.TypeName => TypeBrush,
        SyntaxTokenKind.MethodName => MethodBrush,
        SyntaxTokenKind.String => StringBrush,
        SyntaxTokenKind.Number => NumberBrush,
        SyntaxTokenKind.Comment => CommentBrush,
        SyntaxTokenKind.Register or SyntaxTokenKind.Label => RegisterBrush,
        _ => KeywordBrush,
    };
}
