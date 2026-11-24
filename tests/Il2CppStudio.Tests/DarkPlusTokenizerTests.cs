using Il2CppStudio.Gui.Highlighting;

namespace Il2CppStudio.Tests;

public sealed class DarkPlusTokenizerTests
{
    private static List<(string Text, SyntaxTokenKind Kind)> Tokenize(string line, SyntaxLanguage lang, bool inBlock = false)
    {
        var list = new List<SyntaxTokenSpan>();
        DarkPlusTokenizer.TokenizeLine(line, lang, inBlock, out _, list);
        return [.. list.Select(t => (line.Substring(t.Start, t.Length), t.Kind))];
    }

    [Fact]
    public void TokenizeCSharp_HighlightsKeywordsTypesMethodsStringsNumbersAndComments()
    {
        var tokens = Tokenize("public static void Run(PlayerController pc) { if (pc != null) return \"ok\"; } // note 42", SyntaxLanguage.CSharp);
        Assert.Contains(("public", SyntaxTokenKind.Keyword), tokens);
        Assert.Contains(("static", SyntaxTokenKind.Keyword), tokens);
        Assert.Contains(("void", SyntaxTokenKind.Keyword), tokens);
        Assert.Contains(("Run", SyntaxTokenKind.MethodName), tokens);
        Assert.Contains(("PlayerController", SyntaxTokenKind.TypeName), tokens);
        Assert.Contains(("if", SyntaxTokenKind.ControlKeyword), tokens);
        Assert.Contains(("null", SyntaxTokenKind.Keyword), tokens);
        Assert.Contains(("return", SyntaxTokenKind.ControlKeyword), tokens);
        Assert.Contains(("\"ok\"", SyntaxTokenKind.String), tokens);
        Assert.Contains(("// note 42", SyntaxTokenKind.Comment), tokens);
    }

    [Fact]
    public void TokenizeCSharp_PropagatesBlockCommentsAcrossLines()
    {
        var spans = new List<SyntaxTokenSpan>();
        DarkPlusTokenizer.TokenizeLine("int a = 1; /* start", SyntaxLanguage.CSharp, false, out var inBlock1, spans);
        Assert.True(inBlock1);
        Assert.Contains(spans, s => s.Kind == SyntaxTokenKind.Comment);

        DarkPlusTokenizer.TokenizeLine("middle comment line", SyntaxLanguage.CSharp, inBlock1, out var inBlock2, spans);
        Assert.True(inBlock2);
        var single = Assert.Single(spans);
        Assert.Equal(SyntaxTokenKind.Comment, single.Kind);

        DarkPlusTokenizer.TokenizeLine("end */ return 0x10;", SyntaxLanguage.CSharp, inBlock2, out var inBlock3, spans);
        Assert.False(inBlock3);
        Assert.Contains(spans, s => s.Kind == SyntaxTokenKind.ControlKeyword);
        Assert.Contains(spans, s => s.Kind == SyntaxTokenKind.Number);
    }

    [Fact]
    public void TokenizeCpp_HighlightsBothCppDeclsAndNativeDisassembly()
    {
        var cppTokens = Tokenize("struct Player { int32_t hp; }; // 0x10", SyntaxLanguage.Cpp);
        Assert.Contains(("struct", SyntaxTokenKind.Keyword), cppTokens);
        Assert.Contains(("Player", SyntaxTokenKind.TypeName), cppTokens);
        Assert.Contains(("int32_t", SyntaxTokenKind.Keyword), cppTokens);
        Assert.Contains(("// 0x10", SyntaxTokenKind.Comment), cppTokens);

        var asmTokens = Tokenize("00844A6C  vsub.f32  s0, s16, s0    ; Transform::get_position", SyntaxLanguage.Cpp);
        Assert.Contains(("00844A6C", SyntaxTokenKind.Number), asmTokens);
        Assert.Contains(("vsub.f32", SyntaxTokenKind.Keyword), asmTokens);
        Assert.Contains(("s0", SyntaxTokenKind.Register), asmTokens);
        Assert.Contains(("s16", SyntaxTokenKind.Register), asmTokens);
        Assert.Contains(("; Transform::get_position", SyntaxTokenKind.Comment), asmTokens);
    }

    [Fact]
    public void TokenizeIl_HighlightsDirectivesLabelsOpcodesTypesAndMethods()
    {
        var dirTokens = Tokenize(".method public hidebysig instance void Run() cil managed", SyntaxLanguage.Il);
        Assert.Contains((".method", SyntaxTokenKind.Directive), dirTokens);
        Assert.Contains(("public", SyntaxTokenKind.Keyword), dirTokens);
        Assert.Contains(("void", SyntaxTokenKind.Keyword), dirTokens);
        Assert.Contains(("Run", SyntaxTokenKind.MethodName), dirTokens);

        var opTokens = Tokenize("IL_0004: callvirt instance string [mscorlib]System.Object::ToString() /* tag */", SyntaxLanguage.Il);
        Assert.Contains(("IL_0004", SyntaxTokenKind.Label), opTokens);
        Assert.Contains(("callvirt", SyntaxTokenKind.Keyword), opTokens);
        Assert.Contains(("System.Object", SyntaxTokenKind.TypeName), opTokens);
        Assert.Contains(("ToString", SyntaxTokenKind.MethodName), opTokens);
        Assert.Contains(("/* tag */", SyntaxTokenKind.Comment), opTokens);
    }
}
