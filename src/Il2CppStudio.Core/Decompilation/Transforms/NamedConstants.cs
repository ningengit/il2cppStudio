using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Replaces literal constructions of Unity math types that equal a built-in static property, e.g.
/// <c>new Color(1f, 0f, 0f, 1f)</c> → <c>Color.red</c>, <c>new Vector3(0f, 0f, 0f)</c> → <c>Vector3.zero</c>.
/// Values are bit-identical to Unity's definitions (Color.yellow uses Unity's exact 0.92156863/0.015686275).
/// </summary>
public static class NamedConstants
{
    private static readonly Dictionary<string, (float[] Value, string Name)[]> Table = new()
    {
        ["Color"] =
        [
            ([1, 0, 0, 1], "red"), ([0, 1, 0, 1], "green"), ([0, 0, 1, 1], "blue"), ([1, 1, 1, 1], "white"),
            ([0, 0, 0, 1], "black"), ([0, 1, 1, 1], "cyan"), ([1, 0, 1, 1], "magenta"), ([0.5f, 0.5f, 0.5f, 1], "gray"),
            ([0, 0, 0, 0], "clear"), ([1, 0.92156863f, 0.015686275f, 1], "yellow"),
        ],
        ["Vector3"] =
        [
            ([0, 0, 0], "zero"), ([1, 1, 1], "one"), ([0, 1, 0], "up"), ([0, -1, 0], "down"),
            ([-1, 0, 0], "left"), ([1, 0, 0], "right"), ([0, 0, 1], "forward"), ([0, 0, -1], "back"),
        ],
        ["Vector2"] = [([0, 0], "zero"), ([1, 1], "one"), ([0, 1], "up"), ([0, -1], "down"), ([-1, 0], "left"), ([1, 0], "right")],
        ["Quaternion"] = [([0, 0, 0, 1], "identity")],
    };

    public static void Apply(SyntaxTree tree)
    {
        foreach (var oce in tree.Descendants.OfType<ObjectCreateExpression>().ToList())
        {
            if (oce.Type is not SimpleType { Identifier: var typeName } || !Table.TryGetValue(typeName, out var entries)) continue;
            var args = oce.Arguments.Select(Literal).ToList();
            if (args.Any(a => a is null)) continue;
            foreach (var (value, name) in entries)
            {
                if (value.Length != args.Count || !value.Zip(args, (v, a) => BitConverter.SingleToInt32Bits(v) == BitConverter.SingleToInt32Bits(a!.Value)).All(x => x)) continue;
                oce.ReplaceWith(new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType(typeName)), name));
                break;
            }
        }
    }

    private static float? Literal(Expression e) => e switch
    {
        PrimitiveExpression { Value: float f } => f,
        PrimitiveExpression { Value: int i } => i,
        UnaryOperatorExpression { Operator: UnaryOperatorType.Minus, Expression: PrimitiveExpression { Value: float f } } => -f,
        UnaryOperatorExpression { Operator: UnaryOperatorType.Minus, Expression: PrimitiveExpression { Value: int i } } => -i,
        _ => null,
    };
}
