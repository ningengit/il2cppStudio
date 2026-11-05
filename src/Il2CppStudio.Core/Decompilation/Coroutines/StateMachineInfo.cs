using System.Text.RegularExpressions;
using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// A compiler-generated iterator class (<c>&lt;Name&gt;d__N</c>) and the roles of its fields:
/// <c>&lt;&gt;1__state</c>, <c>&lt;&gt;2__current</c>, <c>&lt;&gt;4__this</c> (outer instance), <c>&lt;x&gt;5__N</c>
/// (hoisted user locals), <c>&lt;&gt;3__p</c> / <c>p</c> (parameter copies).
/// </summary>
public sealed partial class StateMachineInfo
{
    [GeneratedRegex(@"^<(?<method>[^>]+)>d__\d+$")]
    private static partial Regex ClassName();

    [GeneratedRegex(@"^<(?<name>[^>]+)>5__\d+$")]
    private static partial Regex HoistedLocal();

    public required TypeDeclaration Type { get; init; }
    public required string MethodName { get; init; }
    public required MethodDeclaration MoveNext { get; init; }
    public Dictionary<string, (string Name, AstType Type)> HoistedLocals { get; } = [];

    public const string StateField = "<>1__state";
    public const string CurrentField = "<>2__current";
    public const string ThisField = "<>4__this";

    public static StateMachineInfo? TryCreate(TypeDeclaration type)
    {
        var m = ClassName().Match(type.Name);
        if (!m.Success) return null;
        var moveNext = type.Members.OfType<MethodDeclaration>().FirstOrDefault(md => md.Name == "MoveNext" && !md.Body.IsNull);
        var fields = type.Members.OfType<FieldDeclaration>().SelectMany(f => f.Variables.Select(v => (Field: f, v.Name))).ToList();
        if (moveNext is null || fields.All(f => f.Name != StateField) || fields.All(f => f.Name != CurrentField)) return null;

        var info = new StateMachineInfo { Type = type, MethodName = m.Groups["method"].Value, MoveNext = moveNext };
        foreach (var (field, name) in fields)
            if (HoistedLocal().Match(name) is { Success: true } h) info.HoistedLocals[name] = (h.Groups["name"].Value, field.ReturnType);
        return info;
    }

    /// <summary>True for fields that are iterator plumbing and never appear in reconstructed code.</summary>
    public static bool IsPlumbing(string field) => field is StateField or CurrentField or ThisField || field.StartsWith("<>l__initialThreadId", StringComparison.Ordinal);
}
