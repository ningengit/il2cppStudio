# Sandbox-friendly build wrapper: disables telemetry, the MSBuild server and the shared compiler server
# (named pipes are unavailable in restricted environments), then builds the requested target.
# Pass -Android to also build/publish an android-arm64 APK when the .NET Android workload is installed.
param(
    [string]$Target = "Il2CppStudio.slnx",
    [string]$Configuration = "Debug",
    [switch]$Restore,
    [switch]$Android
)
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_NOLOGO = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_CLI_USE_MSBUILD_SERVER = "0"
$env:MSBUILDDISABLENODEREUSE = "1"
Set-Location $PSScriptRoot

if (-not $Android) {
    [string[]]$buildArgs = @("build", $Target, "-c", $Configuration, "-nr:false", "-m:1", "-p:UseSharedCompilation=false", "-clp:NoSummary;ErrorsOnly")
    if (-not $Restore) { $buildArgs += "--no-restore" }
    & dotnet @buildArgs 2>&1 |
        Where-Object { $_ -notmatch "MSBuild-Server|MSBuild server|NativeCommandError|CategoryInfo|FullyQualifiedErrorId|In Zeile|^\+ " }
    exit $LASTEXITCODE
}

$packsDir = Join-Path (Split-Path (Get-Command dotnet).Source -Parent) "packs"
$hasAndroidWorkload = (Test-Path $packsDir) -and (Get-ChildItem $packsDir -Filter "Microsoft.Android.Sdk*" -ErrorAction SilentlyContinue | Measure-Object).Count -gt 0
if (-not $hasAndroidWorkload) {
    Write-Host "Android .NET workload is not installed (skipping Android .apk build). Install with: dotnet workload install android"
    exit 0
}

# Android builds require JDK 17+; upgrade JAVA_HOME automatically if an older JDK 8 is active.
if ([string]::IsNullOrWhiteSpace($env:JAVA_HOME) -or $env:JAVA_HOME -match "jdk[-_]?1\.?8|jdk-8") {
    $jdkCandidates = @(
        "C:\Program Files\Android\AndroidStudioLatest\jbr",
        "C:\Program Files\Android\Android Studio\jbr",
        "C:\Program Files\Eclipse Adoptium\jdk-21*",
        "C:\Program Files\Eclipse Adoptium\jdk-17*",
        "C:\Program Files\Java\jdk-21*",
        "C:\Program Files\Java\jdk-17*"
    )
    foreach ($pat in $jdkCandidates) {
        $match = Get-ChildItem $pat -Directory -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($match -and (Test-Path (Join-Path $match.FullName "bin\java.exe"))) {
            $env:JAVA_HOME = $match.FullName
            break
        }
    }
}

$outApkDir = Join-Path $PSScriptRoot "dist\android-arm64"
[string[]]$apkArgs = @(
    "publish", "src\Il2CppStudio.Android\Il2CppStudio.Android.csproj",
    "-c", $Configuration,
    "-r", "android-arm64",
    "-p:AndroidPackageFormat=apk",
    "-p:UseSharedCompilation=false",
    "-nr:false",
    "-m:1",
    "-o", $outApkDir,
    "-clp:NoSummary;ErrorsOnly"
)
if (-not $Restore) { $apkArgs += "--no-restore" }
& dotnet @apkArgs 2>&1 |
    Where-Object { $_ -notmatch "MSBuild-Server|MSBuild server|NativeCommandError|CategoryInfo|FullyQualifiedErrorId|In Zeile|^\+ " }
exit $LASTEXITCODE
