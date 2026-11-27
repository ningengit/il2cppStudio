using System.Diagnostics;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;

namespace Il2CppStudio.Core.Diagnostics;

public sealed record PerformanceSample(
    DateTimeOffset Timestamp,
    double CpuPercent,
    double RamUsedMb,
    double RamTotalMb,
    double RamUsedPercent,
    double DiskBusyPercent,
    double DiskReadMbPerSec,
    double DiskWriteMbPerSec,
    double DiskFreeGb,
    double ThisProcessMb,
    double PeakProcessMb,
    double PrivateBytesMb,
    double ManagedHeapMb,
    double ProcessCpuPercent,
    int GcGen0,
    int GcGen1,
    int GcGen2,
    int Threads,
    long Warnings,
    long Errors,
    long FirstChanceExceptions,
    string Activity);

/// <summary>
/// Cross-platform (Windows, Linux, macOS, Android) process and system performance monitor with
/// configurable sampling interval, daily rotating CSV logging, and LOH-compacting GC helper.
/// Zero external NuGet dependencies.
/// </summary>
public sealed class PerformanceMonitorService : IDisposable
{
    public const string CsvHeader =
        "timestamp,cpu_percent,ram_used_mb,ram_total_mb,ram_used_percent,disk_busy_percent,disk_read_mb_s,disk_write_mb_s,disk_free_gb,this_process_mb,peak_process_mb,managed_heap_mb,process_cpu_percent,gc_gen0,gc_gen1,gc_gen2,threads,warnings,errors,first_chance_exceptions,activity";

    private const double BytesPerMb = 1024.0 * 1024.0;
    private const double BytesPerGb = 1024.0 * 1024.0 * 1024.0;

    private readonly RuntimeDiagnosticsLog? _diagnostics;
    private readonly Process _process;
    private readonly Lock _sampleGate = new();
    private readonly object _csvGate = new();

    private CancellationTokenSource? _loopCts;
    private Task? _loopTask;
    private int _intervalSeconds = 5;
    private string _activity = "Idle";
    private string _logDirectory;
    private bool _isCsvLogging;
    private StreamWriter? _csvWriter;
    private DateTime _csvDate;
    private bool _disposed;

    // Delta state for CPU & I/O probes
    private long _lastWallTimestamp;
    private TimeSpan _lastProcCpuTime;
    private ulong _lastWinIdleTime;
    private ulong _lastWinKernelTime;
    private ulong _lastWinUserTime;
    private ulong _lastIoReadBytes;
    private ulong _lastIoWriteBytes;
    private bool _hasBaseline;

    private long _lastLinuxTotalTicks;
    private long _lastLinuxIdleTicks;
    private long _lastLinuxDiskIoMs;
    private bool _linuxSelfIoDenied;
    private bool _linuxDiskStatsDenied;

    public PerformanceMonitorService(RuntimeDiagnosticsLog? diagnostics = null, string? logDirectory = null)
    {
        _diagnostics = diagnostics;
        _process = Process.GetCurrentProcess();
        _logDirectory = string.IsNullOrWhiteSpace(logDirectory) ? RuntimeDiagnosticsLog.DefaultLogDirectory : logDirectory;
        InitializeBaselines();

        if (Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PERF_LOG") == "1")
            StartCsvLogging();
    }

    public int IntervalSeconds
    {
        get => Volatile.Read(ref _intervalSeconds);
        set => Volatile.Write(ref _intervalSeconds, Math.Clamp(value, 1, 60));
    }

    public string Activity
    {
        get => Volatile.Read(ref _activity);
        set => Volatile.Write(ref _activity, string.IsNullOrWhiteSpace(value) ? "Idle" : value);
    }

    public void SetActivity(string activity) => Activity = activity;

    public bool IsCsvLogging => Volatile.Read(ref _isCsvLogging);
    public string LogDirectory => _logDirectory;
    public PerformanceSample? LatestSample { get; private set; }

    public event Action<PerformanceSample>? Sampled;

    public void Start()
    {
        lock (_sampleGate)
        {
            if (_disposed || _loopCts is not null) return;
            var cts = new CancellationTokenSource();
            _loopCts = cts;
            _loopTask = Task.Run(() => MonitorLoopAsync(cts.Token));
        }
    }

    public void Stop()
    {
        CancellationTokenSource? cts;
        Task? task;
        lock (_sampleGate)
        {
            cts = _loopCts;
            task = _loopTask;
            _loopCts = null;
            _loopTask = null;
        }
        if (cts is not null)
        {
            try { cts.Cancel(); } catch { }
            if (task is not null)
            {
                try { task.Wait(TimeSpan.FromSeconds(1)); } catch { }
            }
            cts.Dispose();
        }
    }

    public void StartCsvLogging(string? directory = null)
    {
        lock (_csvGate)
        {
            if (_disposed) return;
            if (!string.IsNullOrWhiteSpace(directory))
                _logDirectory = directory;
            Directory.CreateDirectory(_logDirectory);
            _isCsvLogging = true;
            EnsureCsvWriterOpenLocked(DateTime.UtcNow.Date);
        }
    }

    public void StopCsvLogging()
    {
        lock (_csvGate)
        {
            _isCsvLogging = false;
            CloseCsvWriterLocked();
        }
    }

    public PerformanceSample SampleNow(bool writeCsvIfEnabled = true)
    {
        PerformanceSample sample;
        lock (_sampleGate)
        {
            sample = CollectSampleLocked();
            LatestSample = sample;
        }

        if (writeCsvIfEnabled && IsCsvLogging)
            WriteCsvRow(sample);

        Sampled?.Invoke(sample);
        return sample;
    }

    public (double BeforeMb, double AfterMb, double FreedMb) ForceGcCompactLoh()
    {
        var before = SampleNow(writeCsvIfEnabled: false).ThisProcessMb;
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);
        var after = SampleNow(writeCsvIfEnabled: false).ThisProcessMb;
        return (before, after, Math.Max(0.0, before - after));
    }

    public static string FormatSnapshotText(PerformanceSample s)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "Timestamp: {0:O}\nActivity: {1}\nSystem CPU: {2:F1}% | Process CPU: {3:F1}% | Threads: {4}\n" +
            "Process Working Set: {5:F1} MB (Peak: {6:F1} MB, Private: {7:F1} MB, Managed Heap: {8:F1} MB)\n" +
            "System RAM: {9:F1} / {10:F1} MB ({11:F1}%)\n" +
            "Disk I/O: Busy {12:F1}% | Read {13:F2} MB/s | Write {14:F2} MB/s | Free {15:F1} GB\n" +
            "GC Collections: Gen0={16}, Gen1={17}, Gen2={18}\n" +
            "Diagnostics: Warnings={19}, Errors={20}, FirstChance={21}",
            s.Timestamp.UtcDateTime,
            s.Activity,
            s.CpuPercent,
            s.ProcessCpuPercent,
            s.Threads,
            s.ThisProcessMb,
            s.PeakProcessMb,
            s.PrivateBytesMb,
            s.ManagedHeapMb,
            s.RamUsedMb,
            s.RamTotalMb,
            s.RamUsedPercent,
            s.DiskBusyPercent,
            s.DiskReadMbPerSec,
            s.DiskWriteMbPerSec,
            s.DiskFreeGb,
            s.GcGen0,
            s.GcGen1,
            s.GcGen2,
            s.Warnings,
            s.Errors,
            s.FirstChanceExceptions);
    }

    private async Task MonitorLoopAsync(CancellationToken ct)
    {
        try { SampleNow(); } catch { }
        while (!ct.IsCancellationRequested)
        {
            var delayMs = Math.Clamp(IntervalSeconds, 1, 60) * 1000;
            try
            {
                await Task.Delay(delayMs, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            if (ct.IsCancellationRequested) break;
            try { SampleNow(); } catch { }
        }
    }

    private void InitializeBaselines()
    {
        _lastWallTimestamp = Stopwatch.GetTimestamp();
        try
        {
            _process.Refresh();
            _lastProcCpuTime = _process.TotalProcessorTime;
        }
        catch
        {
            _lastProcCpuTime = TimeSpan.Zero;
        }

        if (OperatingSystem.IsWindows())
        {
            TryGetWindowsTimes(out _lastWinIdleTime, out _lastWinKernelTime, out _lastWinUserTime);
            TryGetWindowsIo(_process.Handle, out _lastIoReadBytes, out _lastIoWriteBytes);
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsAndroid())
        {
            TryReadLinuxCpuStat(out _lastLinuxTotalTicks, out _lastLinuxIdleTicks);
            TryReadLinuxSelfIo(out _lastIoReadBytes, out _lastIoWriteBytes);
            TryReadLinuxDiskIoMs(out _lastLinuxDiskIoMs);
        }
        _hasBaseline = true;
    }

    private PerformanceSample CollectSampleLocked()
    {
        var now = DateTimeOffset.UtcNow;
        var wallNow = Stopwatch.GetTimestamp();
        var elapsedSec = _hasBaseline
            ? Math.Max(0.001, (double)(wallNow - _lastWallTimestamp) / Stopwatch.Frequency)
            : 1.0;

        double procMb = 0, peakMb = 0, privMb = 0, procCpuPct = 0;
        int threads = 0;
        try
        {
            _process.Refresh();
            procMb = _process.WorkingSet64 / BytesPerMb;
            peakMb = _process.PeakWorkingSet64 / BytesPerMb;
            privMb = _process.PrivateMemorySize64 / BytesPerMb;
            threads = _process.Threads.Count;

            var cpuNow = _process.TotalProcessorTime;
            if (_hasBaseline)
            {
                var cpuDeltaSec = Math.Max(0.0, (cpuNow - _lastProcCpuTime).TotalSeconds);
                procCpuPct = Math.Clamp(cpuDeltaSec / (elapsedSec * Math.Max(1, Environment.ProcessorCount)) * 100.0, 0.0, 100.0);
            }
            _lastProcCpuTime = cpuNow;
        }
        catch { }

        double managedMb = GC.GetTotalMemory(forceFullCollection: false) / BytesPerMb;
        int gen0 = GC.CollectionCount(0);
        int gen1 = GC.CollectionCount(1);
        int gen2 = GC.CollectionCount(2);

        double sysCpuPct = 0;
        double ramUsedMb = 0, ramTotalMb = 0, ramPct = 0;
        double diskReadMbS = 0, diskWriteMbS = 0, diskBusyPct = 0;

        if (OperatingSystem.IsWindows())
        {
            CollectWindowsMetrics(elapsedSec, ref sysCpuPct, ref ramUsedMb, ref ramTotalMb, ref ramPct, ref diskReadMbS, ref diskWriteMbS, ref diskBusyPct);
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsAndroid())
        {
            CollectLinuxMetrics(elapsedSec, ref sysCpuPct, ref ramUsedMb, ref ramTotalMb, ref ramPct, ref diskReadMbS, ref diskWriteMbS, ref diskBusyPct);
        }
        else if (OperatingSystem.IsMacOS())
        {
            CollectMacMetrics(ref ramUsedMb, ref ramTotalMb, ref ramPct, procMb);
        }

        _lastWallTimestamp = wallNow;
        _hasBaseline = true;

        var diskFreeGb = ProbeDiskFreeGb(_logDirectory);
        var warnings = _diagnostics?.WarningCount ?? 0;
        var errors = _diagnostics?.ErrorCount ?? 0;
        var firstChance = _diagnostics?.FirstChanceExceptionCount ?? 0;

        return new PerformanceSample(
            now,
            Math.Round(sysCpuPct, 2),
            Math.Round(ramUsedMb, 2),
            Math.Round(ramTotalMb, 2),
            Math.Round(ramPct, 2),
            Math.Round(diskBusyPct, 2),
            Math.Round(diskReadMbS, 3),
            Math.Round(diskWriteMbS, 3),
            Math.Round(diskFreeGb, 2),
            Math.Round(procMb, 2),
            Math.Round(peakMb, 2),
            Math.Round(privMb, 2),
            Math.Round(managedMb, 2),
            Math.Round(procCpuPct, 2),
            gen0,
            gen1,
            gen2,
            threads,
            warnings,
            errors,
            firstChance,
            Activity);
    }

    private void CollectWindowsMetrics(
        double elapsedSec,
        ref double sysCpuPct,
        ref double ramUsedMb,
        ref double ramTotalMb,
        ref double ramPct,
        ref double diskReadMbS,
        ref double diskWriteMbS,
        ref double diskBusyPct)
    {
        if (TryGetWindowsMemory(out var totalBytes, out var availBytes))
        {
            ramTotalMb = totalBytes / BytesPerMb;
            ramUsedMb = (totalBytes - availBytes) / BytesPerMb;
            ramPct = totalBytes > 0 ? Math.Clamp((double)(totalBytes - availBytes) / totalBytes * 100.0, 0.0, 100.0) : 0.0;
        }

        if (TryGetWindowsTimes(out var idle, out var kernel, out var user))
        {
            if (_hasBaseline)
            {
                var dIdle = idle >= _lastWinIdleTime ? idle - _lastWinIdleTime : 0;
                var dKernel = kernel >= _lastWinKernelTime ? kernel - _lastWinKernelTime : 0;
                var dUser = user >= _lastWinUserTime ? user - _lastWinUserTime : 0;
                var total = dKernel + dUser;
                if (total > 0)
                {
                    var busy = total > dIdle ? total - dIdle : 0;
                    sysCpuPct = Math.Clamp((double)busy / total * 100.0, 0.0, 100.0);
                }
            }
            _lastWinIdleTime = idle;
            _lastWinKernelTime = kernel;
            _lastWinUserTime = user;
        }

        try
        {
            if (TryGetWindowsIo(_process.Handle, out var readBytes, out var writeBytes))
            {
                if (_hasBaseline)
                {
                    var dRead = readBytes >= _lastIoReadBytes ? readBytes - _lastIoReadBytes : 0;
                    var dWrite = writeBytes >= _lastIoWriteBytes ? writeBytes - _lastIoWriteBytes : 0;
                    diskReadMbS = dRead / BytesPerMb / elapsedSec;
                    diskWriteMbS = dWrite / BytesPerMb / elapsedSec;
                    diskBusyPct = (diskReadMbS + diskWriteMbS) > 0.0001
                        ? Math.Clamp((diskReadMbS + diskWriteMbS) / 250.0 * 100.0, 0.1, 100.0)
                        : 0.0;
                }
                _lastIoReadBytes = readBytes;
                _lastIoWriteBytes = writeBytes;
            }
        }
        catch { }
    }

    private void CollectLinuxMetrics(
        double elapsedSec,
        ref double sysCpuPct,
        ref double ramUsedMb,
        ref double ramTotalMb,
        ref double ramPct,
        ref double diskReadMbS,
        ref double diskWriteMbS,
        ref double diskBusyPct)
    {
        if (TryReadLinuxMemInfo(out var totalKb, out var availKb))
        {
            ramTotalMb = totalKb / 1024.0;
            ramUsedMb = Math.Max(0, totalKb - availKb) / 1024.0;
            ramPct = totalKb > 0 ? Math.Clamp((double)(totalKb - availKb) / totalKb * 100.0, 0.0, 100.0) : 0.0;
        }

        if (TryReadLinuxCpuStat(out var totalTicks, out var idleTicks))
        {
            if (_hasBaseline)
            {
                var dTotal = totalTicks - _lastLinuxTotalTicks;
                var dIdle = idleTicks - _lastLinuxIdleTicks;
                if (dTotal > 0)
                    sysCpuPct = Math.Clamp((double)Math.Max(0, dTotal - dIdle) / dTotal * 100.0, 0.0, 100.0);
            }
            _lastLinuxTotalTicks = totalTicks;
            _lastLinuxIdleTicks = idleTicks;
        }

        if (TryReadLinuxSelfIo(out var readBytes, out var writeBytes))
        {
            if (_hasBaseline)
            {
                var dRead = readBytes >= _lastIoReadBytes ? readBytes - _lastIoReadBytes : 0;
                var dWrite = writeBytes >= _lastIoWriteBytes ? writeBytes - _lastIoWriteBytes : 0;
                diskReadMbS = dRead / BytesPerMb / elapsedSec;
                diskWriteMbS = dWrite / BytesPerMb / elapsedSec;
            }
            _lastIoReadBytes = readBytes;
            _lastIoWriteBytes = writeBytes;
        }

        if (TryReadLinuxDiskIoMs(out var ioMs))
        {
            if (_hasBaseline)
            {
                var dIoMs = Math.Max(0, ioMs - _lastLinuxDiskIoMs);
                diskBusyPct = Math.Clamp(dIoMs / (elapsedSec * 1000.0) * 100.0, 0.0, 100.0);
            }
            _lastLinuxDiskIoMs = ioMs;
        }
        else if ((diskReadMbS + diskWriteMbS) > 0.0001)
        {
            diskBusyPct = Math.Clamp((diskReadMbS + diskWriteMbS) / 250.0 * 100.0, 0.1, 100.0);
        }
    }

    private static void CollectMacMetrics(ref double ramUsedMb, ref double ramTotalMb, ref double ramPct, double procMb)
    {
        var gcInfo = GC.GetGCMemoryInfo();
        if (gcInfo.TotalAvailableMemoryBytes > 0)
        {
            ramTotalMb = gcInfo.TotalAvailableMemoryBytes / BytesPerMb;
            ramUsedMb = procMb;
            ramPct = Math.Clamp(procMb / ramTotalMb * 100.0, 0.0, 100.0);
        }
    }

    private static double ProbeDiskFreeGb(string logDir)
    {
        try
        {
            double totalBytes = 0;
            var hasFixed = false;
            foreach (var d in DriveInfo.GetDrives())
            {
                if (d.DriveType != DriveType.Fixed) continue;
                try
                {
                    if (!d.IsReady) continue;
                    totalBytes += d.AvailableFreeSpace;
                    hasFixed = true;
                }
                catch { }
            }
            if (hasFixed) return totalBytes / BytesPerGb;

            var root = Path.GetPathRoot(logDir);
            if (!string.IsNullOrEmpty(root))
            {
                var di = new DriveInfo(root);
                if (di.IsReady) return di.AvailableFreeSpace / BytesPerGb;
            }
        }
        catch { }
        return 0.0;
    }

    // ------------------------------------------------------------------ CSV writer

    private void WriteCsvRow(PerformanceSample s)
    {
        lock (_csvGate)
        {
            if (!_isCsvLogging || _disposed) return;
            try
            {
                var date = s.Timestamp.UtcDateTime.Date;
                EnsureCsvWriterOpenLocked(date);
                var row = string.Create(CultureInfo.InvariantCulture,
                    $"{s.Timestamp.UtcDateTime:O},{s.CpuPercent:F2},{s.RamUsedMb:F2},{s.RamTotalMb:F2},{s.RamUsedPercent:F2},{s.DiskBusyPercent:F2},{s.DiskReadMbPerSec:F3},{s.DiskWriteMbPerSec:F3},{s.DiskFreeGb:F2},{s.ThisProcessMb:F2},{s.PeakProcessMb:F2},{s.ManagedHeapMb:F2},{s.ProcessCpuPercent:F2},{s.GcGen0},{s.GcGen1},{s.GcGen2},{s.Threads},{s.Warnings},{s.Errors},{s.FirstChanceExceptions},{EscapeCsv(s.Activity)}");
                _csvWriter?.WriteLine(row);
                _csvWriter?.Flush();
            }
            catch { }
        }
    }

    private static string EscapeCsv(string value) =>
        value.IndexOfAny([',', '"', '\r', '\n']) >= 0
            ? "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\""
            : value;

    private void EnsureCsvWriterOpenLocked(DateTime dateUtc)
    {
        if (_csvWriter is not null && _csvDate == dateUtc) return;
        CloseCsvWriterLocked();
        Directory.CreateDirectory(_logDirectory);
        var path = Path.Combine(_logDirectory, $"perf-{dateUtc:yyyy-MM-dd}.csv");
        var writeHeader = !File.Exists(path) || new FileInfo(path).Length == 0;
        var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        _csvWriter = new StreamWriter(fs, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)) { AutoFlush = false };
        if (writeHeader)
        {
            _csvWriter.WriteLine(CsvHeader);
            _csvWriter.Flush();
        }
        _csvDate = dateUtc;
    }

    private void CloseCsvWriterLocked()
    {
        try { _csvWriter?.Flush(); } catch { }
        try { _csvWriter?.Dispose(); } catch { }
        _csvWriter = null;
    }

    // ------------------------------------------------------------------ Linux / Android /proc readers

    private static bool TryReadLinuxMemInfo(out long totalKb, out long availKb)
    {
        totalKb = 0;
        availKb = 0;
        if (!File.Exists("/proc/meminfo")) return false;
        try
        {
            long freeKb = 0, buffersKb = 0, cachedKb = 0;
            var hasAvail = false;
            foreach (var line in File.ReadLines("/proc/meminfo"))
            {
                if (line.StartsWith("MemTotal:", StringComparison.Ordinal))
                    totalKb = ParseProcKb(line);
                else if (line.StartsWith("MemAvailable:", StringComparison.Ordinal))
                {
                    availKb = ParseProcKb(line);
                    hasAvail = true;
                }
                else if (line.StartsWith("MemFree:", StringComparison.Ordinal))
                    freeKb = ParseProcKb(line);
                else if (line.StartsWith("Buffers:", StringComparison.Ordinal))
                    buffersKb = ParseProcKb(line);
                else if (line.StartsWith("Cached:", StringComparison.Ordinal))
                    cachedKb = ParseProcKb(line);
            }
            if (!hasAvail) availKb = freeKb + buffersKb + cachedKb;
            return totalKb > 0;
        }
        catch
        {
            return false;
        }
    }

    private static long ParseProcKb(string line)
    {
        var colon = line.IndexOf(':');
        if (colon < 0) return 0;
        var span = line.AsSpan(colon + 1).Trim();
        var space = span.IndexOfAny(' ', '\t');
        if (space > 0) span = span[..space];
        return long.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var val) ? val : 0;
    }

    private static bool TryReadLinuxCpuStat(out long totalTicks, out long idleTicks)
    {
        totalTicks = 0;
        idleTicks = 0;
        if (!File.Exists("/proc/stat")) return false;
        try
        {
            using var sr = new StreamReader("/proc/stat");
            var line = sr.ReadLine();
            if (line is null || !line.StartsWith("cpu ", StringComparison.Ordinal)) return false;
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 5) return false;
            long sum = 0;
            for (var i = 1; i < parts.Length; i++)
            {
                if (long.TryParse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out var v))
                {
                    sum += v;
                    if (i is 4 or 5) idleTicks += v; // idle + iowait
                }
            }
            totalTicks = sum;
            return totalTicks > 0;
        }
        catch
        {
            return false;
        }
    }

    private bool TryReadLinuxSelfIo(out ulong readBytes, out ulong writeBytes)
    {
        readBytes = 0;
        writeBytes = 0;
        if (_linuxSelfIoDenied || !File.Exists("/proc/self/io")) return false;
        try
        {
            foreach (var line in File.ReadLines("/proc/self/io"))
            {
                if (line.StartsWith("read_bytes:", StringComparison.Ordinal))
                    ulong.TryParse(line.AsSpan(11).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out readBytes);
                else if (line.StartsWith("write_bytes:", StringComparison.Ordinal))
                    ulong.TryParse(line.AsSpan(12).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out writeBytes);
            }
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            _linuxSelfIoDenied = true;
            return false;
        }
        catch
        {
            _linuxSelfIoDenied = true;
            return false;
        }
    }

    private bool TryReadLinuxDiskIoMs(out long totalIoMs)
    {
        totalIoMs = 0;
        if (_linuxDiskStatsDenied || !File.Exists("/proc/diskstats")) return false;
        try
        {
            foreach (var line in File.ReadLines("/proc/diskstats"))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 14) continue;
                var dev = parts[2];
                // Count whole-disk block devices only (sdX, nvme0n1, vdX, mmcblk0) to avoid double-counting partitions.
                if (dev.StartsWith("loop", StringComparison.Ordinal) || dev.StartsWith("ram", StringComparison.Ordinal))
                    continue;
                if (dev.StartsWith("sd", StringComparison.Ordinal) && dev.Length > 3 && char.IsDigit(dev[^1])) continue;
                if (dev.StartsWith("vd", StringComparison.Ordinal) && dev.Length > 3 && char.IsDigit(dev[^1])) continue;
                if (dev.Contains('p', StringComparison.Ordinal) && (dev.StartsWith("nvme", StringComparison.Ordinal) || dev.StartsWith("mmcblk", StringComparison.Ordinal))) continue;
                if (long.TryParse(parts[12], NumberStyles.Integer, CultureInfo.InvariantCulture, out var ms))
                    totalIoMs += ms;
            }
            return true;
        }
        catch
        {
            _linuxDiskStatsDenied = true;
            return false;
        }
    }

    // ------------------------------------------------------------------ Windows kernel32 P/Invoke

    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
        public ulong ToUInt64() => ((ulong)dwHighDateTime << 32) | dwLowDateTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IO_COUNTERS
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetSystemTimes(out FILETIME lpIdleTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetProcessIoCounters(IntPtr hProcess, out IO_COUNTERS lpIoCounters);

    private static bool TryGetWindowsMemory(out ulong totalBytes, out ulong availBytes)
    {
        var stat = new MEMORYSTATUSEX { dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>() };
        if (GlobalMemoryStatusEx(ref stat))
        {
            totalBytes = stat.ullTotalPhys;
            availBytes = stat.ullAvailPhys;
            return true;
        }
        totalBytes = 0;
        availBytes = 0;
        return false;
    }

    private static bool TryGetWindowsTimes(out ulong idle, out ulong kernel, out ulong user)
    {
        if (GetSystemTimes(out var fIdle, out var fKernel, out var fUser))
        {
            idle = fIdle.ToUInt64();
            kernel = fKernel.ToUInt64();
            user = fUser.ToUInt64();
            return true;
        }
        idle = 0;
        kernel = 0;
        user = 0;
        return false;
    }

    private static bool TryGetWindowsIo(IntPtr processHandle, out ulong readBytes, out ulong writeBytes)
    {
        if (GetProcessIoCounters(processHandle, out var counters))
        {
            readBytes = counters.ReadTransferCount;
            writeBytes = counters.WriteTransferCount;
            return true;
        }
        readBytes = 0;
        writeBytes = 0;
        return false;
    }

    public void Dispose()
    {
        _disposed = true;
        Stop();
        StopCsvLogging();
        try { _process.Dispose(); } catch { }
    }
}
