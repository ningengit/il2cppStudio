using System.Collections.Concurrent;
using AsmResolver.DotNet.Signatures;

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
        return t is TypeDefOrRefSignature { IsValueType: true } && Kinds.TryGetValue(t.FullName, out kind);
    }
}
