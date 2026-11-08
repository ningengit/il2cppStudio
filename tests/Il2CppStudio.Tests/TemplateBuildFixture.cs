using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>
/// Loads the ground-truth IL2CPP build (testdata/template il2cpp) once for all tests that need it, with the
/// lifter attached exactly as the CLI and GUI attach it. Tests skip (not fail) when the test data is absent.
/// </summary>
public sealed class TemplateBuildFixture : IDisposable
{
    public static string? RepositoryRoot { get; } = FindRoot();

    /// <summary>Ground-truth build: first existing candidate (the folder was renamed from "template il2cpp").</summary>
    public static string? TestBuild => RepositoryRoot is null ? null
        : new[] { "template il2cpp 2022 win", "template il2cpp" }.Select(n => Path.Combine(RepositoryRoot, "testdata", n)).FirstOrDefault(Directory.Exists)
          ?? Path.Combine(RepositoryRoot, "testdata", "template il2cpp 2022 win");
    public static string? GroundTruthManaged => TestBuild is null ? null
        : Directory.EnumerateDirectories(TestBuild, "*BackUpThisFolder*").Select(d => Path.Combine(d, "Managed")).FirstOrDefault(Directory.Exists);

    private readonly Lazy<Il2CppWorkspace?> _workspace;
    public DiagnosticCollector Sink { get; } = new(new Serilog.LoggerConfiguration().CreateLogger());

    public TemplateBuildFixture()
    {
        _workspace = new Lazy<Il2CppWorkspace?>(() =>
        {
            if (TestBuild is null || !Directory.Exists(TestBuild)) return null;
            var ws = Il2CppWorkspace.Load(TestBuild, Sink, new WorkspaceOptions { SubstituteBcl = true });
            WorkspaceSetup.Attach(ws);
            return ws;
        });
    }

    /// <summary>The loaded workspace; skips the calling test when the test build is not available.</summary>
    public Il2CppWorkspace Workspace => _workspace.Value ?? throw SkipException();

    private static Exception SkipException()
    {
        Assert.Skip($"Test build not found at '{TestBuild ?? "<no repository root>"}'.");
        return new InvalidOperationException("unreachable");
    }

    private static string? FindRoot()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
            if (File.Exists(Path.Combine(dir.FullName, "Il2CppStudio.slnx"))) return dir.FullName;
        return null;
    }

    public void Dispose()
    {
        if (_workspace.IsValueCreated) _workspace.Value?.Dispose();
    }
}

[CollectionDefinition(Name)]
public sealed class TemplateBuildCollection : ICollectionFixture<TemplateBuildFixture>
{
    public const string Name = "template il2cpp";
}
