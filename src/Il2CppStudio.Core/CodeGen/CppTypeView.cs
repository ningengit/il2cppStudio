using System.Text;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.CodeGen;

/// <summary>
/// The C/C++ tab for a whole type: the generated header (<c>Type.h</c>: struct layout, fields, prototypes) followed by
/// the implementation (<c>Type.cpp</c>: each method's original il2cppOutput definition in Enhanced mode, or
/// annotated pseudo-C++ over the disassembly in Native mode), including nested types such as iterator classes.
/// </summary>
public static class CppTypeView
{
    public static string Render(CppHeaderGenerator cpp, TypeModel type)
    {
        var sb = new StringBuilder();
        var file = CppHeaderGenerator.MangleFile(type);
        sb.AppendLine($"// ===================================================== {file}.h");
        sb.AppendLine(cpp.GenerateTypeHeader(type));
        sb.AppendLine($"// ===================================================== {file}.cpp");
        sb.AppendLine($"#include \"{file}.h\"");
        sb.AppendLine();
        AppendMethods(cpp, type, sb);
        return sb.ToString();
    }

    private static void AppendMethods(CppHeaderGenerator cpp, TypeModel type, StringBuilder sb)
    {
        foreach (var m in type.Methods)
        {
            sb.AppendLine(cpp.GenerateMethod(type, m).TrimEnd());
            sb.AppendLine();
        }
        foreach (var nested in type.NestedTypes) AppendMethods(cpp, nested, sb);
    }
}
