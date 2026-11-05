namespace Il2CppStudio.Core.Lifting;

/// <summary>Records type-conversion and fallback decisions made during emission (routed to the error log).</summary>
public static class ConversionLog
{
    public static void Record(EmitContext ctx, string message) => ctx.Log?.Invoke(message);
}
