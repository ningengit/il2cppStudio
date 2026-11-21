using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Makes every identifier legal C#: compiler-generated names (<c>&lt;BootTimer&gt;d__14</c>, <c>&lt;&gt;1__state</c>,
/// <c>&lt;time&gt;5__2</c>, <c>&lt;&gt;c</c>) become <c>_BootTimer_d__14</c>, <c>__1__state</c>, … deterministically, so
/// declarations and uses (also across files of one export) still match. Keywords get an <c>@</c> prefix.
/// Runs last: the coroutine reconstruction recognizes the original names.
/// </summary>
public static class IdentifierEscaper
{
    public static void Apply(SyntaxTree tree)
    {
        AstWalk.Visit<Identifier>(tree, static id =>
        {
            if (id.Parent is SimpleType && id.Name.Contains('.')) return;   // explicit-implementation interface type (see ExplicitImplementationFixer)
            var escaped = Escape(id.Name);
            if (!ReferenceEquals(escaped, id.Name)) id.Name = escaped;
        });
    }

    public static string Escape(string name)
    {
        if (name.Length == 0) return name;
        if (IsStart(name[0]))
        {
            var allValid = true;
            for (var i = 1; i < name.Length; i++)
            {
                if (!IsPart(name[i])) { allValid = false; break; }
            }
            if (allValid) return name;   // keywords are @-escaped by the printer
        }
        var prepend = char.IsDigit(name[0]);
        return string.Create(prepend ? name.Length + 1 : name.Length, name, static (dst, src) =>
        {
            var off = 0;
            if (char.IsDigit(src[0])) dst[off++] = '_';
            for (var i = 0; i < src.Length; i++)
                dst[off + i] = IsPart(src[i]) ? src[i] : '_';
        });
    }

    private static bool IsStart(char c) => char.IsLetter(c) || c == '_';
    private static bool IsPart(char c) => char.IsLetterOrDigit(c) || c == '_';
}
