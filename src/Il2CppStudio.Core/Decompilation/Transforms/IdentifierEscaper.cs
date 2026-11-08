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
        foreach (var id in tree.Descendants.OfType<Identifier>().ToList())
        {
            if (id.Parent is SimpleType && id.Name.Contains('.')) continue;   // explicit-implementation interface type (see ExplicitImplementationFixer)
            var escaped = Escape(id.Name);
            if (!ReferenceEquals(escaped, id.Name)) id.Name = escaped;
        }
    }

    public static string Escape(string name)
    {
        if (name.Length == 0 || IsStart(name[0]) && name.All(IsPart)) return name;   // keywords are @-escaped by the printer
        var chars = name.Select(c => IsPart(c) ? c : '_').ToArray();
        var result = new string(chars);
        return IsStart(result[0]) ? result : "_" + result;
    }

    private static bool IsStart(char c) => char.IsLetter(c) || c == '_';
    private static bool IsPart(char c) => char.IsLetterOrDigit(c) || c == '_';
}
