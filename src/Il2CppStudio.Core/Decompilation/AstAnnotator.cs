using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.TypeSystem;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Adds dnSpy-style annotations to a decompiled syntax tree, using the ORIGINAL IL2CPP metadata tokens:
/// <code>
/// // Token: 0x02000004 RID: 4
/// // Token: 0x0600000B RID: 11 RVA: 0x0023FA00 File Offset: 0x0023E000
/// // Token: 0x04000002 RID: 2 Offset: 0x20
/// </code>
/// and an explanation inside every <c>throw null;</c> fallback body.
/// </summary>
public sealed class AstAnnotator(AnnotationContext ctx, EntityMapper map)
{
    public void Annotate(SyntaxTree tree)
    {
        foreach (var node in tree.Descendants.OfType<EntityDeclaration>().ToList())
        {
            var symbol = node.GetSymbol();
            var line = symbol switch
            {
                ITypeDefinition t when map.Type(t) is { } tm => Token(tm.Token),
                IMethod m when map.Method(m) is { } mm => MethodLine(mm, node),
                IField f when f.DeclaringTypeDefinition is { } dt && map.Type(dt) is { } ftm && map.Field(f) is { } fm => FieldLine(ftm, fm),
                _ => null,
            };
            if (line is not null && ctx.ShowTokens) node.Parent?.InsertChildBefore(node, new Comment(" " + line), Roles.Comment);
        }
    }

    private static string Token(uint token) => $"Token: 0x{token:X8} RID: {token & 0x00FFFFFF}";

    private string MethodLine(Metadata.MethodModel m, EntityDeclaration node)
    {
        var (rva, offset) = ctx.MethodLocation(m);
        if (ctx.FallbackReason(m) is { } reason && node.GetChildByRole(Roles.Body) is { IsNull: false } body
            && body.Statements.FirstOrDefault() is ThrowStatement first)
        {
            body.InsertChildBefore(first, new Comment($" Il2CppStudio Decompilation Fallback: throw null; emitted because {reason}" +
                (rva != 0 ? $" (RVA 0x{rva:X8}, file offset 0x{offset:X8})." : ".")), Roles.Comment);
        }
        return Token(m.Token) + (rva != 0 ? $" RVA: 0x{rva:X8} File Offset: 0x{offset:X8}" : "");
    }

    private string FieldLine(Metadata.TypeModel type, Metadata.FieldModel f)
    {
        var ordinal = type.Fields.IndexOf(f);
        var off = ordinal >= 0 ? ctx.FieldOffset(type, ordinal) : -1;
        return Token(f.Token) + (off >= 0 ? $" Offset: 0x{off:X}" : "");
    }
}
