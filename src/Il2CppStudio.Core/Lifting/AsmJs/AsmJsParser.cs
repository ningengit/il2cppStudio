using System.Globalization;

namespace Il2CppStudio.Core.Lifting.AsmJs;

public sealed record AsmJsFunction(
    string Name,
    IReadOnlyList<string> Parameters,
    IReadOnlyList<(string Name, AsmJsExpr? Init)> Locals,
    IReadOnlyList<AsmJsStmt> Body);

public abstract record AsmJsStmt;
public sealed record EmptyStmt : AsmJsStmt { public static readonly EmptyStmt Instance = new(); }
public sealed record BlockStmt(IReadOnlyList<AsmJsStmt> Statements) : AsmJsStmt;
public sealed record ExprStmt(AsmJsExpr Expr) : AsmJsStmt;
public sealed record IfStmt(AsmJsExpr Condition, AsmJsStmt Then, AsmJsStmt? Else) : AsmJsStmt;
public sealed record WhileStmt(AsmJsExpr Condition, AsmJsStmt Body) : AsmJsStmt;
public sealed record DoWhileStmt(AsmJsStmt Body, AsmJsExpr Condition) : AsmJsStmt;
public sealed record ForStmt(AsmJsExpr? Init, AsmJsExpr? Condition, AsmJsExpr? Step, AsmJsStmt Body) : AsmJsStmt;
public sealed record SwitchCase(long? Value, IReadOnlyList<AsmJsStmt> Body);
public sealed record SwitchStmt(AsmJsExpr Discriminant, IReadOnlyList<SwitchCase> Cases) : AsmJsStmt;
public sealed record LabeledStmt(string Label, AsmJsStmt Statement) : AsmJsStmt;
public sealed record BreakStmt(string? Label) : AsmJsStmt;
public sealed record ContinueStmt(string? Label) : AsmJsStmt;
public sealed record ReturnStmt(AsmJsExpr? Value) : AsmJsStmt;

public abstract record AsmJsExpr;
public sealed record VarExpr(string Name) : AsmJsExpr;
public sealed record IntLitExpr(long Value) : AsmJsExpr;
public sealed record FloatLitExpr(double Value) : AsmJsExpr;
public sealed record UnaryExpr(string Op, AsmJsExpr Operand) : AsmJsExpr;
public sealed record BinaryExpr(string Op, AsmJsExpr Left, AsmJsExpr Right) : AsmJsExpr;
public sealed record CondExpr(AsmJsExpr Condition, AsmJsExpr Then, AsmJsExpr Else) : AsmJsExpr;
public sealed record AssignExpr(AsmJsExpr Target, AsmJsExpr Value) : AsmJsExpr;
public sealed record IndexExpr(AsmJsExpr Target, AsmJsExpr Index) : AsmJsExpr;
public sealed record CallExpr(AsmJsExpr Callee, IReadOnlyList<AsmJsExpr> Args) : AsmJsExpr;
public sealed record MemberExpr(AsmJsExpr Target, string Member) : AsmJsExpr;

/// <summary>
/// Hand-written recursive-descent parser for Emscripten <c>asm.js</c> function bodies.
/// Handles minified semicolons omitted before <c>}</c>, labeled loops/blocks, switches,
/// ternary expressions, heap view indexing, and 32-bit bitwise/float coercions.
/// </summary>
public static class AsmJsParser
{
    private enum TokKind
    {
        Eof, Ident, Int, Float,
        LParen, RParen, LBrace, RBrace, LBracket, RBracket,
        Comma, Semi, Colon, Question, Dot,
        Eq, EqEq, EqEqEq, Ne, Neq,
        Lt, Le, Gt, Ge,
        Plus, Minus, Star, Slash, Percent,
        Amp, Pipe, Caret, Tilde, Bang,
        Shl, Shr, UShr,
    }

    private readonly record struct Token(TokKind Kind, string Text, long IntVal, double FloatVal);

    public static AsmJsFunction ParseFunction(string source)
    {
        var tokens = Tokenize(source);
        var p = new Parser(tokens);
        return p.ParseFunction();
    }

    private static List<Token> Tokenize(string s)
    {
        var list = new List<Token>(Math.Max(32, s.Length / 3));
        var i = 0;
        var n = s.Length;
        while (i < n)
        {
            var ch = s[i];
            if (char.IsWhiteSpace(ch)) { i++; continue; }
            if (ch == '/' && i + 1 < n)
            {
                if (s[i + 1] == '/')
                {
                    i += 2;
                    while (i < n && s[i] != '\n' && s[i] != '\r') i++;
                    continue;
                }
                if (s[i + 1] == '*')
                {
                    i += 2;
                    while (i + 1 < n && !(s[i] == '*' && s[i + 1] == '/')) i++;
                    i = Math.Min(n, i + 2);
                    continue;
                }
            }

            if (IsIdentStart(ch))
            {
                var start = i++;
                while (i < n && IsIdentPart(s[i])) i++;
                list.Add(new Token(TokKind.Ident, s[start..i], 0, 0));
                continue;
            }

            if (char.IsAsciiDigit(ch) || (ch == '.' && i + 1 < n && char.IsAsciiDigit(s[i + 1])))
            {
                list.Add(ReadNumber(s, ref i));
                continue;
            }

            switch (ch)
            {
                case '(': list.Add(new Token(TokKind.LParen, "(", 0, 0)); i++; break;
                case ')': list.Add(new Token(TokKind.RParen, ")", 0, 0)); i++; break;
                case '{': list.Add(new Token(TokKind.LBrace, "{", 0, 0)); i++; break;
                case '}': list.Add(new Token(TokKind.RBrace, "}", 0, 0)); i++; break;
                case '[': list.Add(new Token(TokKind.LBracket, "[", 0, 0)); i++; break;
                case ']': list.Add(new Token(TokKind.RBracket, "]", 0, 0)); i++; break;
                case ',': list.Add(new Token(TokKind.Comma, ",", 0, 0)); i++; break;
                case ';': list.Add(new Token(TokKind.Semi, ";", 0, 0)); i++; break;
                case ':': list.Add(new Token(TokKind.Colon, ":", 0, 0)); i++; break;
                case '?': list.Add(new Token(TokKind.Question, "?", 0, 0)); i++; break;
                case '.': list.Add(new Token(TokKind.Dot, ".", 0, 0)); i++; break;
                case '+': list.Add(new Token(TokKind.Plus, "+", 0, 0)); i++; break;
                case '-': list.Add(new Token(TokKind.Minus, "-", 0, 0)); i++; break;
                case '*': list.Add(new Token(TokKind.Star, "*", 0, 0)); i++; break;
                case '/': list.Add(new Token(TokKind.Slash, "/", 0, 0)); i++; break;
                case '%': list.Add(new Token(TokKind.Percent, "%", 0, 0)); i++; break;
                case '&': list.Add(new Token(TokKind.Amp, "&", 0, 0)); i++; break;
                case '|': list.Add(new Token(TokKind.Pipe, "|", 0, 0)); i++; break;
                case '^': list.Add(new Token(TokKind.Caret, "^", 0, 0)); i++; break;
                case '~': list.Add(new Token(TokKind.Tilde, "~", 0, 0)); i++; break;
                case '=':
                    if (i + 2 < n && s[i + 1] == '=' && s[i + 2] == '=') { list.Add(new Token(TokKind.EqEqEq, "===", 0, 0)); i += 3; }
                    else if (i + 1 < n && s[i + 1] == '=') { list.Add(new Token(TokKind.EqEq, "==", 0, 0)); i += 2; }
                    else { list.Add(new Token(TokKind.Eq, "=", 0, 0)); i++; }
                    break;
                case '!':
                    if (i + 2 < n && s[i + 1] == '=' && s[i + 2] == '=') { list.Add(new Token(TokKind.Neq, "!==", 0, 0)); i += 3; }
                    else if (i + 1 < n && s[i + 1] == '=') { list.Add(new Token(TokKind.Ne, "!=", 0, 0)); i += 2; }
                    else { list.Add(new Token(TokKind.Bang, "!", 0, 0)); i++; }
                    break;
                case '<':
                    if (i + 1 < n && s[i + 1] == '<') { list.Add(new Token(TokKind.Shl, "<<", 0, 0)); i += 2; }
                    else if (i + 1 < n && s[i + 1] == '=') { list.Add(new Token(TokKind.Le, "<=", 0, 0)); i += 2; }
                    else { list.Add(new Token(TokKind.Lt, "<", 0, 0)); i++; }
                    break;
                case '>':
                    if (i + 2 < n && s[i + 1] == '>' && s[i + 2] == '>') { list.Add(new Token(TokKind.UShr, ">>>", 0, 0)); i += 3; }
                    else if (i + 1 < n && s[i + 1] == '>') { list.Add(new Token(TokKind.Shr, ">>", 0, 0)); i += 2; }
                    else if (i + 1 < n && s[i + 1] == '=') { list.Add(new Token(TokKind.Ge, ">=", 0, 0)); i += 2; }
                    else { list.Add(new Token(TokKind.Gt, ">", 0, 0)); i++; }
                    break;
                default:
                    throw new FormatException($"Unexpected character '{ch}' at offset {i} in asm.js function");
            }
        }
        list.Add(new Token(TokKind.Eof, "", 0, 0));
        return list;
    }

    private static Token ReadNumber(string s, ref int i)
    {
        var start = i;
        var n = s.Length;
        if (s[i] == '0' && i + 1 < n && (s[i + 1] is 'x' or 'X'))
        {
            i += 2;
            while (i < n && IsHexDigit(s[i])) i++;
            var hex = s[(start + 2)..i];
            var u = ulong.Parse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return new Token(TokKind.Int, s[start..i], unchecked((long)u), 0);
        }

        var isFloat = false;
        while (i < n && char.IsAsciiDigit(s[i])) i++;
        if (i < n && s[i] == '.')
        {
            isFloat = true;
            i++;
            while (i < n && char.IsAsciiDigit(s[i])) i++;
        }
        if (i < n && (s[i] is 'e' or 'E'))
        {
            isFloat = true;
            i++;
            if (i < n && (s[i] is '+' or '-')) i++;
            while (i < n && char.IsAsciiDigit(s[i])) i++;
        }
        var raw = s[start..i];
        if (!isFloat && long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var iv))
            return new Token(TokKind.Int, raw, iv, 0);
        var dv = double.Parse(raw, NumberStyles.Float, CultureInfo.InvariantCulture);
        return new Token(TokKind.Float, raw, 0, dv);
    }

    private static bool IsIdentStart(char c) => char.IsAsciiLetter(c) || c == '_' || c == '$';
    private static bool IsIdentPart(char c) => char.IsAsciiLetterOrDigit(c) || c == '_' || c == '$';
    private static bool IsHexDigit(char c) => char.IsAsciiDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

    private sealed class Parser(List<Token> tokens)
    {
        private int _pos;

        private Token Peek(int ahead = 0) => _pos + ahead < tokens.Count ? tokens[_pos + ahead] : tokens[^1];
        private Token Next() { var t = Peek(); if (t.Kind != TokKind.Eof) _pos++; return t; }
        private bool Match(TokKind k) { if (Peek().Kind != k) return false; _pos++; return true; }
        private bool MatchKw(string kw) { if (Peek().Kind != TokKind.Ident || Peek().Text != kw) return false; _pos++; return true; }
        private bool IsKw(string kw) => Peek().Kind == TokKind.Ident && Peek().Text == kw;

        private void Expect(TokKind k)
        {
            if (Peek().Kind != k)
                throw new FormatException($"Expected {k} but found '{Peek().Text}' ({Peek().Kind}) at token {_pos}");
            _pos++;
        }

        private void ExpectKw(string kw)
        {
            if (!IsKw(kw))
                throw new FormatException($"Expected '{kw}' but found '{Peek().Text}' at token {_pos}");
            _pos++;
        }

        private void ConsumeSemi()
        {
            if (Match(TokKind.Semi)) return;
            if (Peek().Kind is TokKind.RBrace or TokKind.Eof) return;
        }

        public AsmJsFunction ParseFunction()
        {
            ExpectKw("function");
            var name = Next();
            if (name.Kind != TokKind.Ident) throw new FormatException($"Expected function name, got '{name.Text}'");
            Expect(TokKind.LParen);
            var parameters = new List<string>();
            if (!Match(TokKind.RParen))
            {
                do
                {
                    var p = Next();
                    if (p.Kind != TokKind.Ident) throw new FormatException($"Expected parameter name, got '{p.Text}'");
                    parameters.Add(p.Text);
                } while (Match(TokKind.Comma));
                Expect(TokKind.RParen);
            }
            Expect(TokKind.LBrace);
            var locals = new List<(string, AsmJsExpr?)>();
            var body = new List<AsmJsStmt>();
            while (Peek().Kind is not TokKind.RBrace and not TokKind.Eof)
            {
                if (IsKw("var"))
                {
                    ParseVarDecl(locals);
                    continue;
                }
                body.Add(ParseStmt());
            }
            Expect(TokKind.RBrace);
            return new AsmJsFunction(name.Text, parameters, locals, body);
        }

        private void ParseVarDecl(List<(string, AsmJsExpr?)> locals)
        {
            ExpectKw("var");
            do
            {
                var id = Next();
                if (id.Kind != TokKind.Ident) throw new FormatException($"Expected local name, got '{id.Text}'");
                AsmJsExpr? init = Match(TokKind.Eq) ? ParseAssign() : null;
                locals.Add((id.Text, init));
            } while (Match(TokKind.Comma));
            ConsumeSemi();
        }

        private AsmJsStmt ParseStmt()
        {
            if (Match(TokKind.Semi)) return EmptyStmt.Instance;
            if (Match(TokKind.LBrace))
            {
                var stmts = new List<AsmJsStmt>();
                while (Peek().Kind is not TokKind.RBrace and not TokKind.Eof)
                {
                    if (IsKw("var"))
                    {
                        var midLocals = new List<(string, AsmJsExpr?)>();
                        ParseVarDecl(midLocals);
                        foreach (var (ln, li) in midLocals)
                            if (li is not null) stmts.Add(new ExprStmt(new AssignExpr(new VarExpr(ln), li)));
                    }
                    else
                    {
                        stmts.Add(ParseStmt());
                    }
                }
                Expect(TokKind.RBrace);
                return new BlockStmt(stmts);
            }
            if (MatchKw("if"))
            {
                Expect(TokKind.LParen);
                var cond = ParseExpr();
                Expect(TokKind.RParen);
                var thenS = ParseStmt();
                var elseS = MatchKw("else") ? ParseStmt() : null;
                return new IfStmt(cond, thenS, elseS);
            }
            if (MatchKw("while"))
            {
                Expect(TokKind.LParen);
                var cond = ParseExpr();
                Expect(TokKind.RParen);
                var body = ParseStmt();
                return new WhileStmt(cond, body);
            }
            if (MatchKw("do"))
            {
                var body = ParseStmt();
                ExpectKw("while");
                Expect(TokKind.LParen);
                var cond = ParseExpr();
                Expect(TokKind.RParen);
                ConsumeSemi();
                return new DoWhileStmt(body, cond);
            }
            if (MatchKw("for"))
            {
                Expect(TokKind.LParen);
                AsmJsExpr? init = Peek().Kind == TokKind.Semi ? null : ParseExpr();
                Expect(TokKind.Semi);
                AsmJsExpr? cond = Peek().Kind == TokKind.Semi ? null : ParseExpr();
                Expect(TokKind.Semi);
                AsmJsExpr? step = Peek().Kind == TokKind.RParen ? null : ParseExpr();
                Expect(TokKind.RParen);
                var body = ParseStmt();
                return new ForStmt(init, cond, step, body);
            }
            if (MatchKw("switch"))
            {
                Expect(TokKind.LParen);
                var disc = ParseExpr();
                Expect(TokKind.RParen);
                Expect(TokKind.LBrace);
                var cases = new List<SwitchCase>();
                while (Peek().Kind is not TokKind.RBrace and not TokKind.Eof)
                {
                    long? caseVal = null;
                    if (MatchKw("case"))
                    {
                        var neg = Match(TokKind.Minus);
                        var num = Next();
                        if (num.Kind != TokKind.Int) throw new FormatException($"Expected integer case label, got '{num.Text}'");
                        caseVal = neg ? -num.IntVal : num.IntVal;
                        Expect(TokKind.Colon);
                    }
                    else if (MatchKw("default"))
                    {
                        Expect(TokKind.Colon);
                    }
                    else
                    {
                        throw new FormatException($"Expected case/default in switch, got '{Peek().Text}'");
                    }
                    var body = new List<AsmJsStmt>();
                    while (Peek().Kind is not TokKind.RBrace and not TokKind.Eof && !IsKw("case") && !IsKw("default"))
                        body.Add(ParseStmt());
                    cases.Add(new SwitchCase(caseVal, body));
                }
                Expect(TokKind.RBrace);
                return new SwitchStmt(disc, cases);
            }
            if (MatchKw("break"))
            {
                string? label = Peek().Kind == TokKind.Ident && !IsEndOfStmtLike() ? Next().Text : null;
                ConsumeSemi();
                return new BreakStmt(label);
            }
            if (MatchKw("continue"))
            {
                string? label = Peek().Kind == TokKind.Ident && !IsEndOfStmtLike() ? Next().Text : null;
                ConsumeSemi();
                return new ContinueStmt(label);
            }
            if (MatchKw("return"))
            {
                AsmJsExpr? val = Peek().Kind is TokKind.Semi or TokKind.RBrace or TokKind.Eof || IsKw("else") ? null : ParseExpr();
                ConsumeSemi();
                return new ReturnStmt(val);
            }
            if (Peek().Kind == TokKind.Ident && Peek(1).Kind == TokKind.Colon)
            {
                var label = Next().Text;
                Expect(TokKind.Colon);
                var inner = ParseStmt();
                return new LabeledStmt(label, inner);
            }
            var expr = ParseExpr();
            ConsumeSemi();
            return new ExprStmt(expr);
        }

        private bool IsEndOfStmtLike() => Peek().Kind is TokKind.Semi or TokKind.RBrace or TokKind.Eof || IsKw("else");

        private AsmJsExpr ParseExpr()
        {
            var e = ParseAssign();
            while (Match(TokKind.Comma))
                e = new BinaryExpr(",", e, ParseAssign());
            return e;
        }

        private AsmJsExpr ParseAssign()
        {
            var e = ParseCond();
            if (Match(TokKind.Eq))
                return new AssignExpr(e, ParseAssign());
            return e;
        }

        private AsmJsExpr ParseCond()
        {
            var c = ParseBitOr();
            if (Match(TokKind.Question))
            {
                var t = ParseAssign();
                Expect(TokKind.Colon);
                var f = ParseAssign();
                return new CondExpr(c, t, f);
            }
            return c;
        }

        private AsmJsExpr ParseBitOr()
        {
            var e = ParseBitXor();
            while (Match(TokKind.Pipe)) e = new BinaryExpr("|", e, ParseBitXor());
            return e;
        }

        private AsmJsExpr ParseBitXor()
        {
            var e = ParseBitAnd();
            while (Match(TokKind.Caret)) e = new BinaryExpr("^", e, ParseBitAnd());
            return e;
        }

        private AsmJsExpr ParseBitAnd()
        {
            var e = ParseEquality();
            while (Match(TokKind.Amp)) e = new BinaryExpr("&", e, ParseEquality());
            return e;
        }

        private AsmJsExpr ParseEquality()
        {
            var e = ParseRelational();
            while (Peek().Kind is TokKind.EqEq or TokKind.EqEqEq or TokKind.Ne or TokKind.Neq)
            {
                var op = Next().Text;
                e = new BinaryExpr(op, e, ParseRelational());
            }
            return e;
        }

        private AsmJsExpr ParseRelational()
        {
            var e = ParseShift();
            while (Peek().Kind is TokKind.Lt or TokKind.Le or TokKind.Gt or TokKind.Ge)
            {
                var op = Next().Text;
                e = new BinaryExpr(op, e, ParseShift());
            }
            return e;
        }

        private AsmJsExpr ParseShift()
        {
            var e = ParseAdditive();
            while (Peek().Kind is TokKind.Shl or TokKind.Shr or TokKind.UShr)
            {
                var op = Next().Text;
                e = new BinaryExpr(op, e, ParseAdditive());
            }
            return e;
        }

        private AsmJsExpr ParseAdditive()
        {
            var e = ParseMultiplicative();
            while (Peek().Kind is TokKind.Plus or TokKind.Minus)
            {
                var op = Next().Text;
                e = new BinaryExpr(op, e, ParseMultiplicative());
            }
            return e;
        }

        private AsmJsExpr ParseMultiplicative()
        {
            var e = ParseUnary();
            while (Peek().Kind is TokKind.Star or TokKind.Slash or TokKind.Percent)
            {
                var op = Next().Text;
                e = new BinaryExpr(op, e, ParseUnary());
            }
            return e;
        }

        private AsmJsExpr ParseUnary()
        {
            if (Peek().Kind is TokKind.Plus or TokKind.Minus or TokKind.Tilde or TokKind.Bang)
            {
                var op = Next().Text;
                return new UnaryExpr(op, ParseUnary());
            }
            return ParsePostfix();
        }

        private AsmJsExpr ParsePostfix()
        {
            var e = ParsePrimary();
            while (true)
            {
                if (Match(TokKind.LBracket))
                {
                    var idx = ParseExpr();
                    Expect(TokKind.RBracket);
                    e = new IndexExpr(e, idx);
                }
                else if (Match(TokKind.LParen))
                {
                    var args = new List<AsmJsExpr>();
                    if (!Match(TokKind.RParen))
                    {
                        do args.Add(ParseAssign()); while (Match(TokKind.Comma));
                        Expect(TokKind.RParen);
                    }
                    e = new CallExpr(e, args);
                }
                else if (Match(TokKind.Dot))
                {
                    var member = Next();
                    if (member.Kind != TokKind.Ident) throw new FormatException($"Expected member name, got '{member.Text}'");
                    e = new MemberExpr(e, member.Text);
                }
                else
                {
                    break;
                }
            }
            return e;
        }

        private AsmJsExpr ParsePrimary()
        {
            if (Match(TokKind.LParen))
            {
                var inner = ParseExpr();
                Expect(TokKind.RParen);
                return inner;
            }
            var t = Next();
            return t.Kind switch
            {
                TokKind.Int => new IntLitExpr(t.IntVal),
                TokKind.Float => new FloatLitExpr(t.FloatVal),
                TokKind.Ident => t.Text switch
                {
                    "NaN" => new FloatLitExpr(double.NaN),
                    "Infinity" => new FloatLitExpr(double.PositiveInfinity),
                    _ => new VarExpr(t.Text),
                },
                _ => throw new FormatException($"Unexpected token '{t.Text}' ({t.Kind}) at token {_pos - 1}"),
            };
        }
    }
}
