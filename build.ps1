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

# Detect local Android Studio SDK if ANDROID_HOME is unset or missing on disk.
if ([string]::IsNullOrWhiteSpace($env:ANDROID_HOME) -or -not (Test-Path $env:ANDROID_HOME)) {
    $sdkCandidate = Join-Path $env:LOCALAPPDATA "Android\Sdk"
    if (Test-Path $sdkCandidate) {
        $env:ANDROID_HOME = $sdkCandidate
    }
}

# .NET 11 Android requires JDK 21+; upgrade JAVA_HOME automatically if unset, missing, or < 21.
$needsJdk = [string]::IsNullOrWhiteSpace($env:JAVA_HOME) -or
    -not (Test-Path (Join-Path $env:JAVA_HOME "bin\java.exe")) -or
    ($env:JAVA_HOME -match "jdk[-_]?1\.?8|jdk-8|jdk-11|jdk-17|jdk-18|jdk-19|jdk-20")
if (-not $needsJdk) {
    $verOut = (& (Join-Path $env:JAVA_HOME "bin\java.exe") -version 2>&1) -join "`n"
    if ($verOut -match 'version "(\d+)') {
        if ([int]$matches[1] -lt 21) { $needsJdk = $true }
    }
}
if ($needsJdk) {
    $jdkCandidates = @(
        "C:\Program Files\Android\Android Studio\jbr",
        "C:\Program Files\Android\AndroidStudioLatest\jbr",
        "C:\Program Files\Eclipse Adoptium\jdk-25*",
        "C:\Program Files\Eclipse Adoptium\jdk-21*",
        "C:\Program Files\Java\jdk-25*",
        "C:\Program Files\Java\jdk-21*"
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
$androidAssets = Join-Path $PSScriptRoot "src\Il2CppStudio.Android\obj\project.assets.json"
[string[]]$apkArgs = @(
    "publish", "src\Il2CppStudio.Android\Il2CppStudio.Android.csproj",
    "-c", $Configuration,
    "-f", "net11.0-android",
    "-r", "android-arm64",
    "-p:AndroidPackageFormat=apk",
    "-p:UseSharedCompilation=false",
    "-nr:false",
    "-m:1",
    "-o", $outApkDir,
    "-clp:NoSummary;ErrorsOnly"
)
if (-not [string]::IsNullOrWhiteSpace($env:ANDROID_HOME) -and (Test-Path $env:ANDROID_HOME)) {
    $apkArgs += "-p:AndroidSdkDirectory=$env:ANDROID_HOME"
}
if (-not [string]::IsNullOrWhiteSpace($env:JAVA_HOME) -and (Test-Path $env:JAVA_HOME)) {
    $apkArgs += "-p:JavaSdkDirectory=$env:JAVA_HOME"
}
if (-not $Restore -and (Test-Path $androidAssets)) { $apkArgs += "--no-restore" }
& dotnet @apkArgs 2>&1 |
    Where-Object { $_ -notmatch "MSBuild-Server|MSBuild server|NativeCommandError|CategoryInfo|FullyQualifiedErrorId|In Zeile|^\+ " }
exit $LASTEXITCODE
