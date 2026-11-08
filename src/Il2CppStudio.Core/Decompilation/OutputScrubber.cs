using System.Text;
using System.Text.RegularExpressions;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Removes decompiler diagnostics from C# output (stack type warnings such as <c>//IL_007e: Expected I, but got I8</c>,
/// <c>/*Error near IL_…*/</c>) and reports each one to the error log with the member it belongs to.
/// </summary>
public static partial class OutputScrubber
{
    [GeneratedRegex(@"^\s*//\s*IL_[0-9A-Fa-f]{4}:.*$")]
    private static partial Regex IlWarningLine();

    [GeneratedRegex(@"/\*\s*(?:Error|Warning)[^*]*\*/")]
    private static partial Regex InlineError();

    [GeneratedRegex(@"^\s*// Token: 0x[0-9A-F]{8} RID: \d+(?: RVA: (?<rva>0x[0-9A-F]+))?")]
    private static partial Regex TokenLine();

    public static string Scrub(string code, string assembly, DecompilationErrorLog? log)
    {
        var sb = new StringBuilder(code.Length);
        string member = "<type>";
        string? rva = null;
        var awaitingDeclaration = false;
        foreach (var raw in code.Split('\n'))
        {
            var line = raw.TrimEnd('\r');
            if (TokenLine().Match(line) is { Success: true } tm)
            {
                rva = tm.Groups["rva"].Success ? tm.Groups["rva"].Value : null;
                awaitingDeclaration = true;
            }
            else if (awaitingDeclaration && line.Trim().Length > 0 && !line.TrimStart().StartsWith("//") && !line.TrimStart().StartsWith('['))
            {
                member = line.Trim();
                awaitingDeclaration = false;
            }

            if (IlWarningLine().IsMatch(line))
            {
                log?.Add(assembly, "DecompilerWarning", member, rva, line.Trim().TrimStart('/').Trim(),
                    "Removed from C# output; the decompiler inserted an implicit conversion to continue.");
                continue;
            }
            var cleaned = InlineError().Replace(line, m =>
            {
                log?.Add(assembly, "DecompilerError", member, rva, m.Value, "Comment removed from C# output.");
                return "";
            });
            sb.Append(cleaned).Append('\n');
        }
        return sb.ToString();
    }
}
