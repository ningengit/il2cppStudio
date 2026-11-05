using System.Reflection.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Maps decompiler entities of a stub module back to the IL2CPP metadata model. Stub types/members are emitted in
/// metadata order, so a member's position within its declaring type identifies it exactly (overloads included).
/// </summary>
public sealed class EntityMapper(MetadataModel model)
{
    private readonly Dictionary<string, TypeModel> _types = model.Types.GroupBy(t => t.FullName).ToDictionary(g => g.Key, g => g.First());

    public TypeModel? Type(ITypeDefinition? t) => t is null ? null : _types.GetValueOrDefault(t.ReflectionName);

    public MethodModel? Method(IMethod m) => Member(m, m.DeclaringTypeDefinition, (tm, i) => i < tm.Methods.Count ? tm.Methods[i] : null,
        (reader, th) => reader.GetTypeDefinition(th).GetMethods().Select(h => (EntityHandle)h));

    public FieldModel? Field(IField f) => Member(f, f.DeclaringTypeDefinition, (tm, i) => i < tm.Fields.Count ? tm.Fields[i] : null,
        (reader, th) => reader.GetTypeDefinition(th).GetFields().Select(h => (EntityHandle)h));

    private T? Member<T>(IEntity entity, ITypeDefinition? declaring, Func<TypeModel, int, T?> pick,
        Func<MetadataReader, TypeDefinitionHandle, IEnumerable<EntityHandle>> members) where T : class
    {
        if (Type(declaring) is not { } tm || entity.ParentModule?.MetadataFile is not { } file || declaring!.MetadataToken.IsNil) return null;
        var reader = file.Metadata;
        var index = 0;
        foreach (var h in members(reader, (TypeDefinitionHandle)declaring.MetadataToken))
        {
            if (h == entity.MetadataToken) return pick(tm, index);
            index++;
        }
        return null;
    }
}
