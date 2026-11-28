using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Maps a vtable slot of a class (from <c>call [klass + 0x138 + 16·slot]</c>) to the implementing method using
/// the metadata vtableMethods table. Entries are encoded (v27+) as <c>(usage &lt;&lt; 29) | (index &lt;&lt; 1) | 1</c>,
/// with usage 3 = method definition and 6 = generic method spec.
/// </summary>
public static class VTableResolver
{
    public readonly record struct SlotTarget(int MethodIndex, int SpecIndex);

    public static SlotTarget? Resolve(GlobalMetadata md, int typeDef, int slot, Il2CppRegistration? reg = null, TypeNameService? names = null)
    {
        if ((uint)typeDef >= (uint)md.TypeDefinitions.Length) return null;
        var def = md.TypeDefinitions[typeDef];
        if ((uint)slot < def.VtableCount)
        {
            var encoded = md.VTableMethods[def.VtableStart + slot];
            if (encoded != 0)
            {
                var usage = (encoded >> 29) & 7;
                var index = md.Version.Major >= 27 ? (int)((encoded & 0x1FFFFFFE) >> 1) : (int)(encoded & 0x1FFFFFFF);
                var methodDefUsage = md.Version.Major >= 108 ? 2u : 3u;
                var methodSpecUsage = md.Version.Major >= 108 ? 5u : 6u;
                if (usage == methodDefUsage && (uint)index < (uint)md.Methods.Length) return new SlotTarget(index, -1);
                if (usage == methodSpecUsage) return new SlotTarget(-1, index);
            }
        }
        if (reg is null || names is null) return null;
        var cur = typeDef;
        for (var hop = 0; hop < 16 && (uint)cur < (uint)md.TypeDefinitions.Length; hop++)
        {
            var cdef = md.TypeDefinitions[cur];
            for (var m = cdef.MethodStart; m < cdef.MethodStart + cdef.MethodCount; m++)
            {
                var mdRec = md.Methods[m];
                if (mdRec.Slot == slot && (mdRec.Flags & 0x0040) != 0)
                    return new SlotTarget(m, -1);
            }
            if (cdef.ParentIndex < 0 || !reg.TryGetType(cdef.ParentIndex, out var parent)) break;
            cur = names.ResolveTypeDefIndex(parent);
        }
        return null;
    }
}
