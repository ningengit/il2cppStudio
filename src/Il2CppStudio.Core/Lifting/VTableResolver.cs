using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Maps a vtable slot of a class (from <c>call [klass + 0x138 + 16·slot]</c>) to the implementing method using
/// the metadata vtableMethods table. Entries are encoded (v27+) as <c>(usage &lt;&lt; 29) | (index &lt;&lt; 1) | 1</c>,
/// with usage 3 = method definition and 6 = generic method spec.
/// </summary>
public static class VTableResolver
{
    public readonly record struct SlotTarget(int MethodIndex, int SpecIndex);

    public static SlotTarget? Resolve(GlobalMetadata md, int typeDef, int slot)
    {
        if ((uint)typeDef >= (uint)md.TypeDefinitions.Length) return null;
        var def = md.TypeDefinitions[typeDef];
        if ((uint)slot >= def.VtableCount) return null;
        var encoded = md.VTableMethods[def.VtableStart + slot];
        if (encoded == 0) return null;
        var usage = (encoded >> 29) & 7;
        var index = md.Version.Major >= 27 ? (int)((encoded & 0x1FFFFFFE) >> 1) : (int)(encoded & 0x1FFFFFFF);
        return usage switch
        {
            3 when (uint)index < (uint)md.Methods.Length => new SlotTarget(index, -1),
            6 => new SlotTarget(-1, index),
            _ => null,
        };
    }
}
