using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Metadata.Tables;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Writes a compile-only copy of a stub assembly with every type and member public (a "publicized" reference, as used
/// by the Unity modding community). IL2CPP inlines small methods, so lifted code legitimately reads private fields of
/// other classes (<c>Button.m_OnClick</c>); against a publicized reference that code binds. Only the reference copy
/// in <c>_references</c> is changed - the decompiled declarations keep their real accessibility.
/// </summary>
public static class ReferencePublicizer
{
    public static byte[] Publicize(byte[] image)
    {
        var module = ModuleDefinition.FromBytes(image);
        foreach (var type in module.GetAllTypes())
        {
            if (type.IsModuleType) continue;
            type.Attributes = (type.Attributes & ~TypeAttributes.VisibilityMask) |
                              (type.DeclaringType is null ? TypeAttributes.Public : TypeAttributes.NestedPublic);
            foreach (var f in type.Fields)
                f.Attributes = (f.Attributes & ~(FieldAttributes.FieldAccessMask | FieldAttributes.InitOnly)) | FieldAttributes.Public;
            foreach (var m in type.Methods)
            {
                // protected / protected internal stay as they are: overrides in the compiled project must match them.
                var access = m.Attributes & MethodAttributes.MemberAccessMask;
                if (access is MethodAttributes.Family or MethodAttributes.FamilyOrAssembly && m.IsVirtual) continue;
                m.Attributes = (m.Attributes & ~MethodAttributes.MemberAccessMask) | MethodAttributes.Public;
            }
        }
        using var ms = new MemoryStream();
        module.Write(ms);
        return ms.ToArray();
    }
}
