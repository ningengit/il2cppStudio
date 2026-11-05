using System.Text.Json;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;
using Serilog.Events;

namespace Il2CppStudio.Cli;

/// <summary>Shared plumbing for CLI verbs: logging, workspace loading, member lookup, output.</summary>
internal static class CliHost
{
    public static int Run(bool verbose, Func<DiagnosticCollector, int> body)
    {
        var logDir = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_LOG_DIR") ?? LoggingOptions.DefaultLogDirectory;
        using var logger = LoggingSetup.CreateLogger(new LoggingOptions
        {
            ConsoleLevel = verbose ? LogEventLevel.Verbose : LogEventLevel.Information,
            LogDirectory = logDir,
        });
        var sink = new DiagnosticCollector(logger);
        try
        {
            var code = body(sink);
            Console.Error.WriteLine($"Diagnostics: {sink.CountOf(DiagnosticSeverity.Warning)} warning(s), " +
                $"{sink.CountOf(DiagnosticSeverity.Error) + sink.CountOf(DiagnosticSeverity.Fatal)} error(s). Log: {logDir}");
            return code;
        }
        catch (Exception ex)
        {
            sink.Fatal(DiagnosticCategory.Workspace, DiagnosticCodes.StageFailed, ex.Message, null, ex);
            return 1;
        }
    }

    public static Il2CppWorkspace Load(string path, DiagnosticCollector sink, bool substituteBcl, string? bclDir = null)
    {
        var ws = Il2CppWorkspace.Load(path, sink, new WorkspaceOptions { SubstituteBcl = substituteBcl, BclReferenceDirectory = bclDir });
        Console.Error.WriteLine(ws.ModeLabel);
        return ws;
    }

    /// <summary>Resolves --type / --method; prints candidates and returns false when ambiguous or missing.</summary>
    public static bool TryResolve(Il2CppWorkspace ws, string typeName, string? methodName, out TypeModel type, out List<MethodModel> methods)
    {
        type = null!;
        methods = [];
        var types = ws.FindTypes(typeName);
        if (types.Count == 0)
        {
            var suggestions = ws.Model.Types.Where(t => t.FullName.Contains(typeName, StringComparison.OrdinalIgnoreCase)).Take(10).ToList();
            Console.Error.WriteLine($"Type '{typeName}' not found.{(suggestions.Count > 0 ? " Did you mean: " + string.Join(", ", suggestions.Select(s => s.FullName)) : "")}");
            return false;
        }
        if (types.Count > 1)
        {
            Console.Error.WriteLine($"'{typeName}' is ambiguous; use the full name: {string.Join(", ", types.Select(t => $"{t.FullName} [{t.ImageName}]"))}");
            return false;
        }
        type = types[0];
        if (methodName is null) return true;
        var name = methodName;
        methods = [.. type.Methods.Where(m => m.Name == name || m.DisplaySignature == name)];
        if (methods.Count == 0)
        {
            Console.Error.WriteLine($"Method '{methodName}' not found on {type.FullName}. Methods: {string.Join(", ", type.Methods.Select(m => m.Name).Distinct())}");
            return false;
        }
        return true;
    }

    /// <summary>Finds a method by name across all types (for verbs that take only --method), or "Type::Method".</summary>
    public static List<(TypeModel Type, MethodModel Method)> FindMethods(Il2CppWorkspace ws, string spec, string? typeName)
    {
        var sep = spec.IndexOf("::", StringComparison.Ordinal);
        if (sep > 0) { typeName = spec[..sep]; spec = spec[(sep + 2)..]; }
        var types = typeName is null ? ws.Model.Types : ws.FindTypes(typeName);
        return [.. types.SelectMany(t => t.Methods.Where(m => m.Name == spec).Select(m => (t, m)))];
    }

    public static readonly JsonSerializerOptions Json = WorkspaceSerializer.Json;

    public static void WriteJson(object value) => Console.WriteLine(JsonSerializer.Serialize(value, Json));
}
