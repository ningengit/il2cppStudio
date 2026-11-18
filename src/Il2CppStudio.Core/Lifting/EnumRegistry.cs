using System.Collections.Concurrent;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using E = AsmResolver.PE.DotNet.Metadata.Tables.ElementType;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Enum types of the stub modules and their underlying evaluation-stack kind. CIL treats an enum value as its
/// underlying integer, so integer values flow into enum parameters/fields without conversion (ILSpy then shows the
/// named member, e.g. <c>InputAction.Interact</c>). Filled by <see cref="StubAssemblyBuilder"/>.
/// </summary>
public static class EnumRegistry
{
    private static readonly ConcurrentDictionary<string, ValueKind> Kinds = new(StringComparer.Ordinal);

    public static void Register(string fullName, ValueKind underlying) => Kinds[fullName] = underlying;

    public static bool TryGetUnderlying(TypeSignature t, out ValueKind kind)
    {
        kind = default;
        if (t is not TypeDefOrRefSignature { IsValueType: true } tdr) return false;
        if (tdr.Type is TypeDefinition { IsEnum: true } td)
        {
            var vf = td.Fields.FirstOrDefault(f => f.Name == "value__");
            kind = vf?.Signature?.FieldType.ElementType is E.I8 or E.U8 ? ValueKind.I8 : ValueKind.I4;
            return true;
        }
        return Kinds.TryGetValue(t.FullName, out kind);
    }
}
