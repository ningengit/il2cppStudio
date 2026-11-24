using System.Globalization;

namespace Il2CppStudio.Gui.Highlighting;

public enum SyntaxLanguage
{
    CSharp,
    Cpp,
    Il,
}

public enum SyntaxTokenKind
{
    Keyword,
    ControlKeyword,
    TypeName,
    MethodName,
    String,
    Number,
    Comment,
    Register,
    Directive,
    Label,
}

public readonly record struct SyntaxTokenSpan(int Start, int Length, SyntaxTokenKind Kind);

/// <summary>
/// Allocation-light, AOT/ILLink-safe single-line tokenizer producing VS Code Dark+ token spans for C#, C/C++
/// (including native x86/x64/ARM64/ARMv7/WASM disassembly), and CIL listings. Zero reflection and zero native P/Invoke.
/// </summary>
public static class DarkPlusTokenizer
{
    private static readonly HashSet<string> CSharpControlKeywords = new(StringComparer.Ordinal)
    {
        "if", "else", "switch", "case", "default", "for", "foreach", "while", "do",
        "break", "continue", "return", "goto", "try", "catch", "finally", "throw",
        "yield", "await", "when",
    };

    private static readonly HashSet<string> CSharpKeywords = new(StringComparer.Ordinal)
    {
        "abstract", "add", "alias", "and", "as", "ascending", "async", "base", "bool",
        "byte", "char", "checked", "class", "const", "decimal", "delegate", "descending",
        "double", "dynamic", "enum", "equals", "event", "explicit", "extern", "false",
        "file", "fixed", "float", "from", "get", "global", "group", "implicit", "in",
        "init", "int", "interface", "internal", "into", "is", "join", "let", "lock",
        "long", "nameof", "namespace", "new", "not", "notnull", "null", "object",
        "on", "operator", "or", "orderby", "out", "override", "params", "partial",
        "private", "protected", "public", "readonly", "record", "ref", "remove",
        "required", "sbyte", "scoped", "sealed", "select", "set", "short", "sizeof",
        "stackalloc", "static", "string", "struct", "this", "true", "typeof", "uint",
        "ulong", "unchecked", "unmanaged", "unsafe", "ushort", "using", "value", "var",
        "virtual", "void", "volatile", "where", "with",
    };

    private static readonly HashSet<string> CppControlKeywords = new(StringComparer.Ordinal)
    {
        "if", "else", "switch", "case", "default", "for", "while", "do",
        "break", "continue", "return", "goto", "try", "catch", "throw",
    };

    private static readonly HashSet<string> CppKeywords = new(StringComparer.Ordinal)
    {
        "alignas", "alignof", "asm", "auto", "bool", "char", "char8_t", "char16_t",
        "char32_t", "class", "const", "const_cast", "consteval", "constexpr",
        "constinit", "decltype", "delete", "double", "dynamic_cast", "enum",
        "explicit", "export", "extern", "false", "float", "friend", "inline",
        "int", "int8_t", "int16_t", "int32_t", "int64_t", "intptr_t", "long",
        "mutable", "namespace", "new", "noexcept", "nullptr", "operator", "private",
        "protected", "public", "register", "reinterpret_cast", "restrict", "short",
        "signed", "sizeof", "size_t", "static", "static_assert", "static_cast",
        "struct", "template", "this", "thread_local", "true", "typedef", "typeid",
        "typename", "uint8_t", "uint16_t", "uint32_t", "uint64_t", "uintptr_t",
        "union", "unsigned", "using", "virtual", "void", "volatile", "wchar_t",
        "Il2CppObject", "Il2CppString", "Il2CppArray", "MethodInfo", "Il2CppClass",
    };

    private static readonly HashSet<string> IlKeywords = new(StringComparer.Ordinal)
    {
        "public", "private", "family", "famandassem", "famorassem", "assembly",
        "nested", "static", "final", "virtual", "hidebysig", "abstract",
        "specialname", "rtspecialname", "newslot", "cil", "managed", "unmanaged",
        "aggressiveinlining", "instance", "explicit", "sequential", "auto", "ansi",
        "unicode", "beforefieldinit", "sealed", "serializable", "valuetype", "class",
        "interface", "extends", "implements", "void", "bool", "char", "int8", "uint8",
        "int16", "uint16", "int32", "uint32", "int64", "uint64", "float32", "float64",
        "string", "object", "native", "unsigned", "pinned", "modreq", "modopt",
        "init", "initonly", "literal", "notserialized", "out", "opt", "in", "default",
    };

    private static readonly HashSet<string> AsmRegisters = new(StringComparer.OrdinalIgnoreCase)
    {
        // ARM32 / Thumb
        "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7", "r8", "r9", "r10", "r11", "r12",
        "sp", "lr", "pc", "fp", "ip", "sb", "sl", "apsr_nzcv", "fpscr", "cpsr", "spsr",
        // ARM64
        "x0", "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8", "x9", "x10", "x11", "x12",
        "x13", "x14", "x15", "x16", "x17", "x18", "x19", "x20", "x21", "x22", "x23",
        "x24", "x25", "x26", "x27", "x28", "x29", "x30", "xzr", "wzr",
        "w0", "w1", "w2", "w3", "w4", "w5", "w6", "w7", "w8", "w9", "w10", "w11", "w12",
        "w13", "w14", "w15", "w16", "w17", "w18", "w19", "w20", "w21", "w22", "w23",
        "w24", "w25", "w26", "w27", "w28", "w29", "w30",
        "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s10", "s11", "s12",
        "s13", "s14", "s15", "s16", "s17", "s18", "s19", "s20", "s21", "s22", "s23",
        "s24", "s25", "s26", "s27", "s28", "s29", "s30", "s31",
        "d0", "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9", "d10", "d11", "d12",
        "d13", "d14", "d15", "d16", "d17", "d18", "d19", "d20", "d21", "d22", "d23",
        "d24", "d25", "d26", "d27", "d28", "d29", "d30", "d31",
        "q0", "q1", "q2", "q3", "q4", "q5", "q6", "q7", "q8", "q9", "q10", "q11", "q12",
        "q13", "q14", "q15",
        // x86 / x64
        "rax", "rbx", "rcx", "rdx", "rsi", "rdi", "rbp", "rsp", "r8d", "r9d", "r10d",
        "r11d", "r12d", "r13d", "r14d", "r15d", "r13", "r14", "r15",
        "eax", "ebx", "ecx", "edx", "esi", "edi", "ebp", "esp", "rip", "eip",
        "ax", "bx", "cx", "dx", "si", "di", "bp", "al", "bl", "cl", "dl",
        "xmm0", "xmm1", "xmm2", "xmm3", "xmm4", "xmm5", "xmm6", "xmm7",
        "xmm8", "xmm9", "xmm10", "xmm11", "xmm12", "xmm13", "xmm14", "xmm15",
        "st0", "st1", "st2", "st3", "st4", "st5", "st6", "st7",
    };

    public static void TokenizeLine(
        string line,
        SyntaxLanguage language,
        bool inBlockComment,
        out bool endInBlockComment,
        List<SyntaxTokenSpan> output)
    {
        output.Clear();
        endInBlockComment = inBlockComment;
        if (string.IsNullOrEmpty(line)) return;

        var i = 0;
        var n = line.Length;

        if (inBlockComment)
        {
            var close = line.IndexOf("*/", StringComparison.Ordinal);
            if (close < 0)
            {
                output.Add(new SyntaxTokenSpan(0, n, SyntaxTokenKind.Comment));
                return;
            }
            var commentEnd = close + 2;
            output.Add(new SyntaxTokenSpan(0, commentEnd, SyntaxTokenKind.Comment));
            endInBlockComment = false;
            i = commentEnd;
        }

        if (i == 0 && language == SyntaxLanguage.Cpp && TryTokenizeAsmLine(line, output))
            return;

        if (i == 0 && TryHandleLeadingPreprocessor(line, language, output))
            return;

        while (i < n)
        {
            var c = line[i];

            if (c == '/' && i + 1 < n)
            {
                if (line[i + 1] == '/')
                {
                    output.Add(new SyntaxTokenSpan(i, n - i, SyntaxTokenKind.Comment));
                    return;
                }
                if (line[i + 1] == '*')
                {
                    var close = line.IndexOf("*/", i + 2, StringComparison.Ordinal);
                    if (close < 0)
                    {
                        output.Add(new SyntaxTokenSpan(i, n - i, SyntaxTokenKind.Comment));
                        endInBlockComment = true;
                        return;
                    }
                    var len = close + 2 - i;
                    output.Add(new SyntaxTokenSpan(i, len, SyntaxTokenKind.Comment));
                    i += len;
                    continue;
                }
            }

            if (language != SyntaxLanguage.CSharp && c == ';')
            {
                // Semi-colon starts a comment in IL or native disassembly when at start or preceded by whitespace.
                var prevWs = i == 0 || char.IsWhiteSpace(line[i - 1]);
                if (language == SyntaxLanguage.Il && prevWs)
                {
                    output.Add(new SyntaxTokenSpan(i, n - i, SyntaxTokenKind.Comment));
                    return;
                }
            }

            // C# verbatim / interpolated strings
            if (language == SyntaxLanguage.CSharp && (c == '@' || c == '$') && i + 1 < n && (line[i + 1] == '"' || (c == '$' && i + 2 < n && line[i + 1] == '@' && line[i + 2] == '"')))
            {
                var start = i;
                var verbatim = line[i] == '@' || (i + 1 < n && line[i + 1] == '@');
                var qIdx = line.IndexOf('"', i);
                i = verbatim ? ScanVerbatimString(line, qIdx + 1) : ScanQuotedLiteral(line, qIdx + 1, '"');
                output.Add(new SyntaxTokenSpan(start, i - start, SyntaxTokenKind.String));
                continue;
            }

            if (c is '"' or '\'')
            {
                var start = i;
                i = ScanQuotedLiteral(line, i + 1, c);
                output.Add(new SyntaxTokenSpan(start, i - start, SyntaxTokenKind.String));
                continue;
            }

            // IL directives (.method, .maxstack, etc.)
            if (language == SyntaxLanguage.Il && c == '.' && (i == 0 || char.IsWhiteSpace(line[i - 1])) && i + 1 < n && char.IsLetter(line[i + 1]))
            {
                var start = i++;
                while (i < n && (char.IsLetterOrDigit(line[i]) || line[i] == '_')) i++;
                output.Add(new SyntaxTokenSpan(start, i - start, SyntaxTokenKind.Directive));
                continue;
            }

            // Numbers
            if (char.IsAsciiDigit(c) || (c == '#' && language == SyntaxLanguage.Cpp && i + 1 < n && (char.IsAsciiDigit(line[i + 1]) || line[i + 1] == '-')))
            {
                var start = i;
                i = ScanNumber(line, i);
                output.Add(new SyntaxTokenSpan(start, i - start, SyntaxTokenKind.Number));
                continue;
            }

            // Identifiers / keywords
            if (IsIdentStart(c))
            {
                var start = i;
                i = ScanIdent(line, i, language);
                var len = i - start;
                var token = line.AsSpan(start, len);

                if (language == SyntaxLanguage.Il && TryClassifyIlToken(line, start, len, token, out var ilKind))
                {
                    output.Add(new SyntaxTokenSpan(start, len, ilKind));
                    continue;
                }

                if (ClassifyIdentifier(line, start, len, token, language) is { } kind)
                    output.Add(new SyntaxTokenSpan(start, len, kind));
                continue;
            }

            i++;
        }
    }

    private static bool TryHandleLeadingPreprocessor(string line, SyntaxLanguage language, List<SyntaxTokenSpan> output)
    {
        if (language == SyntaxLanguage.Il) return false;
        var trim = 0;
        while (trim < line.Length && char.IsWhiteSpace(line[trim])) trim++;
        if (trim >= line.Length || line[trim] != '#') return false;

        var end = trim + 1;
        while (end < line.Length && (char.IsLetterOrDigit(line[end]) || line[end] == '_')) end++;
        output.Add(new SyntaxTokenSpan(trim, end - trim, SyntaxTokenKind.Directive));
        if (language == SyntaxLanguage.Cpp && line.AsSpan(trim, end - trim).Equals("#include", StringComparison.Ordinal))
        {
            var rest = end;
            while (rest < line.Length && char.IsWhiteSpace(line[rest])) rest++;
            if (rest < line.Length && line[rest] is '<' or '"')
                output.Add(new SyntaxTokenSpan(rest, line.Length - rest, SyntaxTokenKind.String));
        }
        return true;
    }

    private static bool TryTokenizeAsmLine(string line, List<SyntaxTokenSpan> output)
    {
        var trim = 0;
        while (trim < line.Length && char.IsWhiteSpace(line[trim])) trim++;
        if (trim >= line.Length) return false;

        // Disassembly header comment line: "; ARMv7 ..." or "; x64 ..."
        if (line[trim] == ';')
        {
            output.Add(new SyntaxTokenSpan(trim, line.Length - trim, SyntaxTokenKind.Comment));
            return true;
        }

        // Disassembly instruction line: 8..16 hex digits followed by spaces and mnemonic.
        var hexStart = trim;
        var hexEnd = hexStart;
        while (hexEnd < line.Length && char.IsAsciiHexDigit(line[hexEnd])) hexEnd++;
        var hexLen = hexEnd - hexStart;
        if (hexLen is < 8 or > 16 || hexEnd >= line.Length || line[hexEnd] != ' ')
            return false;

        output.Add(new SyntaxTokenSpan(hexStart, hexLen, SyntaxTokenKind.Number));

        var i = hexEnd;
        while (i < line.Length && char.IsWhiteSpace(line[i])) i++;
        if (i >= line.Length) return true;

        var mnemStart = i;
        while (i < line.Length && !char.IsWhiteSpace(line[i])) i++;
        output.Add(new SyntaxTokenSpan(mnemStart, i - mnemStart, SyntaxTokenKind.Keyword));

        // Tokenize operands & optional `; symbol` comment
        while (i < line.Length)
        {
            var ch = line[i];
            if (ch == ';')
            {
                output.Add(new SyntaxTokenSpan(i, line.Length - i, SyntaxTokenKind.Comment));
                return true;
            }
            if (ch == '#' || char.IsAsciiDigit(ch))
            {
                var start = i;
                i = ScanNumber(line, i);
                if (i > start)
                    output.Add(new SyntaxTokenSpan(start, i - start, SyntaxTokenKind.Number));
                else
                    i++;
                continue;
            }
            if (IsIdentStart(ch))
            {
                var start = i;
                while (i < line.Length && (char.IsLetterOrDigit(line[i]) || line[i] is '_' or '.')) i++;
                var ident = line.Substring(start, i - start);
                if (AsmRegisters.Contains(ident))
                    output.Add(new SyntaxTokenSpan(start, i - start, SyntaxTokenKind.Register));
                else if (ident.Equals("lsl", StringComparison.OrdinalIgnoreCase)
                    || ident.Equals("lsr", StringComparison.OrdinalIgnoreCase)
                    || ident.Equals("asr", StringComparison.OrdinalIgnoreCase)
                    || ident.Equals("ror", StringComparison.OrdinalIgnoreCase)
                    || ident.Equals("rrx", StringComparison.OrdinalIgnoreCase))
                    output.Add(new SyntaxTokenSpan(start, i - start, SyntaxTokenKind.Keyword));
                continue;
            }
            i++;
        }
        return true;
    }

    private static bool TryClassifyIlToken(string line, int start, int len, ReadOnlySpan<char> token, out SyntaxTokenKind kind)
    {
        if (token.StartsWith("IL_", StringComparison.Ordinal))
        {
            kind = SyntaxTokenKind.Label;
            return true;
        }
        if (IlKeywords.Contains(token.ToString()))
        {
            kind = SyntaxTokenKind.Keyword;
            return true;
        }
        if (start >= 2 && line[start - 1] == ':' && line[start - 2] == ':')
        {
            kind = SyntaxTokenKind.MethodName;
            return true;
        }
        if (IsIlOpcodePosition(line, start) && IsLikelyIlOpcode(token))
        {
            kind = SyntaxTokenKind.Keyword;
            return true;
        }
        var next = start + len;
        while (next < line.Length && line[next] == ' ') next++;
        if (next < line.Length && line[next] == '(')
        {
            kind = SyntaxTokenKind.MethodName;
            return true;
        }
        if (start > 0 && (line[start - 1] == ']' || line[start - 1] == '.') && char.IsUpper(token[0]))
        {
            kind = SyntaxTokenKind.TypeName;
            return true;
        }
        if (char.IsUpper(token[0]) && HasLowercaseOrDigit(token))
        {
            kind = SyntaxTokenKind.TypeName;
            return true;
        }
        kind = default;
        return false;
    }

    private static bool IsIlOpcodePosition(string line, int start)
    {
        // Either first non-whitespace token on the line, or immediately after `IL_xxxx: `.
        var p = 0;
        while (p < start && char.IsWhiteSpace(line[p])) p++;
        if (p == start) return true;
        if (line.AsSpan(p).StartsWith("IL_", StringComparison.Ordinal))
        {
            var colon = line.IndexOf(':', p);
            if (colon > p && colon < start)
            {
                p = colon + 1;
                while (p < start && char.IsWhiteSpace(line[p])) p++;
                return p == start;
            }
        }
        return false;
    }

    private static bool IsLikelyIlOpcode(ReadOnlySpan<char> token)
    {
        if (token.Length is < 2 or > 20) return false;
        foreach (var ch in token)
            if (!(char.IsAsciiLetterLower(ch) || char.IsAsciiDigit(ch) || ch == '.'))
                return false;
        return true;
    }

    private static SyntaxTokenKind? ClassifyIdentifier(string line, int start, int len, ReadOnlySpan<char> token, SyntaxLanguage language)
    {
        var str = token.ToString();
        if (language == SyntaxLanguage.CSharp)
        {
            if (CSharpControlKeywords.Contains(str)) return SyntaxTokenKind.ControlKeyword;
            if (CSharpKeywords.Contains(str)) return SyntaxTokenKind.Keyword;
        }
        else if (language == SyntaxLanguage.Cpp)
        {
            if (CppControlKeywords.Contains(str)) return SyntaxTokenKind.ControlKeyword;
            if (CppKeywords.Contains(str)) return SyntaxTokenKind.Keyword;
        }

        // Check next non-whitespace char for `MethodName(` or `MethodName<...>(`, skipping attribute names `[Attr]`
        var next = start + len;
        while (next < line.Length && line[next] == ' ') next++;
        if (next < line.Length && line[next] == '(')
            return SyntaxTokenKind.MethodName;

        // Avoid treating ALL_CAPS constants as TypeName; PascalCase types start uppercase and contain lowercase.
        if (char.IsAsciiLetterUpper(token[0]) && HasLowercaseOrDigit(token))
            return SyntaxTokenKind.TypeName;

        return null;
    }

    private static bool HasLowercaseOrDigit(ReadOnlySpan<char> s)
    {
        for (var i = 1; i < s.Length; i++)
            if (char.IsAsciiLetterLower(s[i]) || char.IsAsciiDigit(s[i]))
                return true;
        return false;
    }

    private static bool IsIdentStart(char c) => char.IsLetter(c) || c == '_';

    private static int ScanIdent(string line, int i, SyntaxLanguage language)
    {
        i++;
        while (i < line.Length)
        {
            var c = line[i];
            if (char.IsLetterOrDigit(c) || c == '_') { i++; continue; }
            if (language == SyntaxLanguage.Il && c == '.' && i + 1 < line.Length && char.IsLetterOrDigit(line[i + 1]))
            {
                // Dotted IL opcodes such as `ldarg.0`, `conv.i4`, `ldc.i4.s`
                i++;
                continue;
            }
            break;
        }
        return i;
    }

    private static int ScanNumber(string line, int i)
    {
        var n = line.Length;
        if (line[i] == '#')
        {
            i++;
            if (i < n && line[i] == '-') i++;
            if (i >= n || !char.IsAsciiDigit(line[i])) return i;
        }

        if (i + 1 < n && line[i] == '0' && line[i + 1] is 'x' or 'X')
        {
            i += 2;
            while (i < n && (char.IsAsciiHexDigit(line[i]) || line[i] == '_')) i++;
        }
        else if (i + 1 < n && line[i] == '0' && line[i + 1] is 'b' or 'B')
        {
            i += 2;
            while (i < n && line[i] is '0' or '1' or '_') i++;
        }
        else
        {
            while (i < n && (char.IsAsciiDigit(line[i]) || line[i] == '_')) i++;
            if (i + 1 < n && line[i] == '.' && char.IsAsciiDigit(line[i + 1]))
            {
                i++;
                while (i < n && (char.IsAsciiDigit(line[i]) || line[i] == '_')) i++;
            }
            if (i < n && line[i] is 'e' or 'E')
            {
                var exp = i + 1;
                if (exp < n && line[exp] is '+' or '-') exp++;
                if (exp < n && char.IsAsciiDigit(line[exp]))
                {
                    i = exp;
                    while (i < n && char.IsAsciiDigit(line[i])) i++;
                }
            }
        }
        while (i < n && line[i] is 'u' or 'U' or 'l' or 'L' or 'f' or 'F' or 'd' or 'D' or 'm' or 'M') i++;
        return i;
    }

    private static int ScanQuotedLiteral(string line, int i, char quote)
    {
        while (i < line.Length)
        {
            var c = line[i++];
            if (c == '\\' && i < line.Length) { i++; continue; }
            if (c == quote) break;
        }
        return i;
    }

    private static int ScanVerbatimString(string line, int i)
    {
        while (i < line.Length)
        {
            if (line[i++] == '"')
            {
                if (i < line.Length && line[i] == '"') { i++; continue; }
                break;
            }
        }
        return i;
    }
}
