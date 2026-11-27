using System.Diagnostics;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Workspace;
using Il2CppStudio.Gui.ViewModels;

namespace Il2CppStudio.Tests;

public sealed class DiagnosticsAndStabilityTests
{
    private static DiagnosticCollector Sink() => new(new Serilog.LoggerConfiguration().CreateLogger());

    private static string Target(params string[] relativeParts)
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var path = root is null ? null : Path.Combine([root, "testdata", .. relativeParts]);
        if (path is null || (!Directory.Exists(path) && !File.Exists(path)))
            Assert.Skip($"Test target '{Path.Combine(relativeParts)}' not found.");
        return path!;
    }

    [Fact]
    public void PerformanceMonitorService_SamplesCsvAndCompactsLoh()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cpp-perf-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            using var log = new RuntimeDiagnosticsLog(logDirectory: tmp);
            using var perf = new PerformanceMonitorService(log, tmp);

            perf.IntervalSeconds = 120; // should clamp to 60
            Assert.Equal(60, perf.IntervalSeconds);
            perf.IntervalSeconds = 0; // should clamp to 1
            Assert.Equal(1, perf.IntervalSeconds);

            perf.SetActivity("Decompiling");
            perf.StartCsvLogging();
            Assert.True(perf.IsCsvLogging);

            var s = perf.SampleNow();
            Assert.True(s.ThisProcessMb > 0);
            Assert.True(s.PeakProcessMb >= s.ThisProcessMb);
            Assert.True(s.ManagedHeapMb > 0);
            Assert.True(s.Threads > 0);
            Assert.Equal("Decompiling", s.Activity);

            var (beforeMb, afterMb, freedMb) = perf.ForceGcCompactLoh();
            Assert.True(beforeMb > 0);
            Assert.True(afterMb > 0);
            Assert.True(freedMb >= 0);

            var snap = PerformanceMonitorService.FormatSnapshotText(s);
            Assert.Contains("Activity: Decompiling", snap, StringComparison.Ordinal);

            perf.Dispose();

            var csvFiles = Directory.GetFiles(tmp, "perf-*.csv");
            Assert.Single(csvFiles);
            var lines = File.ReadAllLines(csvFiles[0]);
            Assert.True(lines.Length >= 2);
            Assert.Equal(PerformanceMonitorService.CsvHeader, lines[0]);
            Assert.Equal(21, lines[0].Split(',').Length);
            Assert.Equal(21, lines[1].Split(',').Length);
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Fact]
    public void RuntimeDiagnosticsLog_DeduplicatesFirstChance_CapsBuffer_AndFlushesLog()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cpp-diag-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            using var log = new RuntimeDiagnosticsLog(capacity: 500, logDirectory: tmp);
            log.InstallGlobalHooks();
            log.StartFileLogging();
            log.CaptureFirstChanceExceptions = true;

            for (var i = 0; i < 6; i++)
            {
                try { ThrowRepeatedTestException(i); }
                catch { }
            }

            Assert.True(log.FirstChanceExceptionCount >= 6);
            var fc = log.Snapshot(RuntimeLogFilter.FirstChance);
            var repeated = fc.FirstOrDefault(e => e.ExceptionType == typeof(InvalidOperationException).FullName
                                                  && e.Message.StartsWith("repeat-", StringComparison.Ordinal));
            Assert.NotNull(repeated);
            Assert.Equal(6, repeated.HitCount);

            // Push > 500 unique warnings to verify ring buffer cap.
            for (var i = 0; i < 600; i++)
                log.RecordWarning("TestSource", $"warning-{i}");

            Assert.Equal(600, log.WarningCount);
            Assert.True(log.Snapshot(RuntimeLogFilter.All).Count <= 500);
            Assert.True(log.Snapshot(RuntimeLogFilter.Warnings).Count <= 500);

            log.RecordError("TestErrorSource", "fatal-problem", new ArgumentException("bad"));
            Assert.Single(log.Snapshot(RuntimeLogFilter.Errors));

            var text = log.FormatText(RuntimeLogFilter.Errors);
            Assert.Contains("fatal-problem", text, StringComparison.Ordinal);

            log.Dispose();
            var diagFiles = Directory.GetFiles(tmp, "diag-*.log");
            Assert.Single(diagFiles);
            Assert.Contains("fatal-problem", File.ReadAllText(diagFiles[0]), StringComparison.Ordinal);
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    private static void ThrowRepeatedTestException(int i) =>
        throw new InvalidOperationException($"repeat-{i}");

    [Fact]
    public async Task RapidConcurrentTypeSwitching_ZeroCrashesAndZeroObjectDisposedExceptions()
    {
        var apk = Target("Template il2cpp android", "Baldi's Basics Classic.arm64-v8a.apk");
        var sink = Sink();
        using var log = new RuntimeDiagnosticsLog();
        log.CaptureFirstChanceExceptions = true;
        sink.DiagnosticReported += log.RecordFromDiagnostic;

        using var ws = Il2CppWorkspace.Load(apk, sink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
        WorkspaceSetup.Attach(ws);

        var types = ws.Model.Types
            .Where(t => t.DeclaringType is null && t.Methods.Count > 0 && t.ImageName == "Assembly-CSharp.dll")
            .Take(55)
            .ToList();
        Assert.True(types.Count >= 50);

        CancellationTokenSource? activeCts = null;
        var tasks = new List<Task>(types.Count);
        var errors = new System.Collections.Concurrent.ConcurrentBag<Exception>();

        for (var i = 0; i < types.Count; i++)
        {
            var t = types[i];
            var view = (CodeView)(i % 3);
            var cts = new CancellationTokenSource();
            var prev = Interlocked.Exchange(ref activeCts, cts);
            if (prev is not null)
            {
                try { prev.Cancel(); }
                catch (ObjectDisposedException ex) { errors.Add(ex); }
            }

            tasks.Add(Task.Run(() =>
            {
                try
                {
                    _ = ws.Render(t, null, view, cts.Token);
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
                finally
                {
                    Interlocked.CompareExchange(ref activeCts, null, cts);
                    cts.Dispose();
                }
            }));
        }

        await Task.WhenAll(tasks);
        Assert.Empty(errors);
        Assert.Empty(sink.Query(DiagnosticSeverity.Error));

        // Also stress-test MainViewModel debounced navigation across 50+ rapid selection changes.
        using var vm = new MainViewModel { Ui = new HeadlessUiServices() };
        await vm.LoadFolderAsync(apk);
        vm.SelectedCodeTabIndex = 2; // C#
        var acsRoot = vm.Roots.First(r => r.Assembly?.Name == "Assembly-CSharp");
        acsRoot.IsExpanded = true;
        foreach (var ns in acsRoot.Children)
            ns.IsExpanded = true;
        var typeNodes = acsRoot.Children.SelectMany(ns => ns.Children).Where(n => n.Type is not null).Take(55).ToList();
        Assert.True(typeNodes.Count >= 50);

        foreach (var node in typeNodes)
            vm.SelectedNode = node;

        var finalNode = typeNodes[^1];
        var sw = Stopwatch.StartNew();
        while (sw.Elapsed < TimeSpan.FromSeconds(10))
        {
            if (!vm.IsBusy && vm.CSharpText.Contains(finalNode.Type!.Name, StringComparison.Ordinal))
                break;
            await Task.Delay(50);
        }

        Assert.Contains(finalNode.Type!.Name, vm.CSharpText, StringComparison.Ordinal);
        Assert.Equal(0, vm.RuntimeLog.ErrorCount);
    }

    [Fact]
    public void Unity6000_6_EmitsEnumsCleanly_WithoutClassInheritingInt()
    {
        var paths = new[]
        {
            Target("template il2cpp 6000.6 win64"),
            Target("template il2cpp 6000.6 android", "android baldi build.apk"),
        };

        foreach (var path in paths)
        {
            var sink = Sink();
            using var ws = Il2CppWorkspace.Load(path, sink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
            WorkspaceSetup.Attach(ws);

            var actionType = Assert.Single(ws.FindTypes("InputAction"));
            var actionCs = ws.Render(actionType, null, CodeView.CSharp);
            Assert.Contains("public enum InputAction", actionCs, StringComparison.Ordinal);
            Assert.DoesNotContain("public sealed class InputAction : int", actionCs, StringComparison.Ordinal);

            var bindingType = Assert.Single(ws.FindTypes("InputBinding"));
            var bindingCs = ws.Render(bindingType, null, CodeView.CSharp);
            Assert.Contains("KeyCode secondary = KeyCode.None", bindingCs, StringComparison.Ordinal);

            var mgrType = Assert.Single(ws.FindTypes("InputManager"));
            var mgrCs = ws.Render(mgrType, null, CodeView.CSharp);
            Assert.DoesNotContain("using Unity.IL2CPP.Metadata;", mgrCs, StringComparison.Ordinal);
            Assert.DoesNotContain("__Il2Cpp", mgrCs, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void DuplicateNonMultipleAttributesAndOptionalParameters_AreNormalized()
    {
        var ps4Dir = Target("Unity Game Ps4 template");
        var ps4Sink = Sink();
        using (var ps4 = Il2CppWorkspace.Load(ps4Dir, ps4Sink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true }))
        {
            WorkspaceSetup.Attach(ps4);
            var chest = Assert.Single(ps4.FindTypes("TreasureChestController"));
            var chestCs = ps4.Render(chest, null, CodeView.CSharp);
            Assert.DoesNotContain("[Space]\r\n\t[Space]", chestCs, StringComparison.Ordinal);
            Assert.DoesNotContain("[Space]\n\t[Space]", chestCs, StringComparison.Ordinal);
        }

        var switchDir = Target("2019 game il2cpp switch");
        var swSink = Sink();
        using (var sw = Il2CppWorkspace.Load(switchDir, swSink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true }))
        {
            WorkspaceSetup.Attach(sw);
            var opc = Assert.Single(sw.FindTypes("OpcManager"));
            var opcCs = sw.Render(opc, null, CodeView.CSharp);
            Assert.DoesNotContain("[Space]\r\n\t[Space]", opcCs, StringComparison.Ordinal);
            Assert.DoesNotContain("[Space]\n\t[Space]", opcCs, StringComparison.Ordinal);

            var encount = Assert.Single(sw.FindTypes("Dpr.EncountTools"));
            var setupMethod = Assert.Single(encount.Methods, m => m.Name == "SetupBattleComm");
            var setupCs = sw.Render(encount, setupMethod, CodeView.CSharp);
            Assert.DoesNotContain("int stationIndex2 = -1", setupCs, StringComparison.Ordinal);
            Assert.DoesNotContain("[Optional][DefaultParameterValue(null)][In] ref", setupCs, StringComparison.Ordinal);
        }
    }

    private sealed class HeadlessUiServices : IUiServices
    {
        public Task<string?> PickFolderAsync(string title) => Task.FromResult<string?>(null);
        public Task<string?> PickFileAsync(string title) => Task.FromResult<string?>(null);
        public Task<string?> PickKeysetFileAsync(string title) => Task.FromResult<string?>(null);
        public Task<InstalledAndroidApp?> PickInstalledAppAsync() => Task.FromResult<InstalledAndroidApp?>(null);
        public Task<string?> PromptTextAsync(string title, string message, string watermark, bool maskInput = false) => Task.FromResult<string?>(null);
        public Task<string?> SaveFileAsync(string title, string suggestedName, string extension) => Task.FromResult<string?>(null);
        public Task<string?> SaveTextFileAsync(string title, string suggestedName, string extension, string content) => Task.FromResult<string?>(null);
        public Task SetClipboardAsync(string text) => Task.CompletedTask;
        public Task ShowBootConfigDialogAsync(BootConfigViewModel vm) => Task.CompletedTask;
        public void Post(Action action) => action();
    }
}
