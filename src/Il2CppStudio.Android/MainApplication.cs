using System.IO.Compression;
using System.Text;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Util;
using Avalonia;
using Avalonia.Android;
using Il2CppStudio.Gui;
using Il2CppStudio.Gui.ViewModels;

namespace Il2CppStudio.Android;

[Application]
public class MainApplication : AvaloniaAndroidApplication<App>
{
    private static int s_crashHooksRegistered;

    public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public override void OnCreate()
    {
        if (Interlocked.Exchange(ref s_crashHooksRegistered, 1) == 0)
        {
            AndroidEnvironment.UnhandledExceptionRaiser += (_, e) =>
                LogFatalException("AndroidEnvironment.UnhandledExceptionRaiser", e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
                LogFatalException("AppDomain.UnhandledException", e.ExceptionObject as Exception, e.ExceptionObject?.ToString());
        }

        try
        {
            base.OnCreate();
        }
        catch (Exception ex)
        {
            LogFatalException("MainApplication.OnCreate", ex);
            throw;
        }
    }

    private static void LogFatalException(string source, Exception? ex, string? fallback = null)
    {
        try
        {
            var sb = new StringBuilder();
            sb.Append("FATAL [").Append(source).Append("]: ");
            if (ex is not null)
            {
                sb.AppendLine(ex.ToString());
                var depth = 0;
                for (var cur = ex.InnerException; cur is not null; cur = cur.InnerException)
                {
                    depth++;
                    sb.Append("--- InnerException #").Append(depth).Append(" (").Append(cur.GetType().FullName).Append("): ")
                      .AppendLine(cur.Message);
                    if (!string.IsNullOrWhiteSpace(cur.StackTrace))
                        sb.AppendLine(cur.StackTrace);
                }
            }
            else
            {
                sb.AppendLine(fallback ?? "(null exception)");
            }
            Log.Error("Il2CppStudio", sb.ToString());
        }
        catch
        {
            // Never mask the original exception if logging itself fails.
        }
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        if (CacheDir?.AbsolutePath is { Length: > 0 } cacheDir)
            global::System.Environment.SetEnvironmentVariable("TMPDIR", cacheDir);
        Il2CppStudio.Gui.App.InstalledAppScanner = ScanInstalledUnityAppsAsync;
        return base.CustomizeAppBuilder(builder).WithInterFont();
    }

    private Task<IReadOnlyList<InstalledAndroidApp>> ScanInstalledUnityAppsAsync() => Task.Run<IReadOnlyList<InstalledAndroidApp>>(() =>
    {
        var ctx = ApplicationContext ?? this;
        var pm = ctx.PackageManager;
        if (pm is null) return [];

        IList<ApplicationInfo> apps;
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
            apps = pm.GetInstalledApplications(PackageManager.ApplicationInfoFlags.Of(0L));
        else
        {
#pragma warning disable CA1422
            apps = pm.GetInstalledApplications(PackageInfoFlags.MetaData);
#pragma warning restore CA1422
        }

        var selfPkg = ctx.PackageName;
        var results = new List<InstalledAndroidApp>();
        foreach (var ai in apps)
        {
            if (string.IsNullOrWhiteSpace(ai.PackageName)) continue;
            if (string.Equals(ai.PackageName, selfPkg, StringComparison.Ordinal)) continue;

            var baseApk = ai.PublicSourceDir ?? ai.SourceDir;
            if (string.IsNullOrWhiteSpace(baseApk) || !File.Exists(baseApk)) continue;

            var splits = (ai.SplitPublicSourceDirs ?? ai.SplitSourceDirs)?
                .Where(p => !string.IsNullOrWhiteSpace(p) && File.Exists(p))
                .Distinct(StringComparer.Ordinal)
                .ToArray() ?? [];

            var nativeDir = !string.IsNullOrWhiteSpace(ai.NativeLibraryDir) && Directory.Exists(ai.NativeLibraryDir)
                ? ai.NativeLibraryDir
                : null;
            var hasDiskIl2Cpp = nativeDir is not null && File.Exists(Path.Combine(nativeDir, "libil2cpp.so"));

            var hasMetadata = false;
            var hasZipIl2Cpp = false;
            ScanApkCentralDirectory(baseApk, ref hasMetadata, ref hasZipIl2Cpp);
            if (!hasMetadata || (!hasDiskIl2Cpp && !hasZipIl2Cpp))
            {
                foreach (var split in splits)
                {
                    ScanApkCentralDirectory(split, ref hasMetadata, ref hasZipIl2Cpp);
                    if (hasMetadata && (hasDiskIl2Cpp || hasZipIl2Cpp)) break;
                }
            }

            if (!hasMetadata || (!hasDiskIl2Cpp && !hasZipIl2Cpp)) continue;

            var label = ai.LoadLabel(pm);
            if (string.IsNullOrWhiteSpace(label)) label = ai.PackageName;

            string? versionName = null;
            try
            {
                PackageInfo? pi;
                if (OperatingSystem.IsAndroidVersionAtLeast(33))
                    pi = pm.GetPackageInfo(ai.PackageName, PackageManager.PackageInfoFlags.Of(0L));
                else
                {
#pragma warning disable CA1422
                    pi = pm.GetPackageInfo(ai.PackageName, (PackageInfoFlags)0);
#pragma warning restore CA1422
                }
                versionName = pi?.VersionName;
            }
            catch { /* optional metadata */ }

            results.Add(new InstalledAndroidApp(ai.PackageName, label, baseApk, splits, nativeDir, versionName));
        }

        results.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Label, b.Label));
        return results;
    });

    private static void ScanApkCentralDirectory(string apkPath, ref bool hasMetadata, ref bool hasIl2Cpp)
    {
        try
        {
            using var zip = ZipFile.OpenRead(apkPath);
            foreach (var entry in zip.Entries)
            {
                var name = entry.FullName;
                if (name.EndsWith("global-metadata.dat", StringComparison.OrdinalIgnoreCase))
                    hasMetadata = true;
                else if (name.EndsWith("/libil2cpp.so", StringComparison.OrdinalIgnoreCase)
                         || name.Equals("libil2cpp.so", StringComparison.OrdinalIgnoreCase))
                    hasIl2Cpp = true;
                if (hasMetadata && hasIl2Cpp) break;
            }
        }
        catch
        {
            // Ignore unreadable / restricted package archives cleanly.
        }
    }
}
