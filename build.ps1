# Sandbox-friendly build wrapper: disables telemetry, the MSBuild server and the shared compiler server
# (named pipes are unavailable in restricted environments), then builds the requested target.
param(
    [string]$Target = "Il2CppStudio.slnx",
    [string]$Configuration = "Debug",
    [switch]$Restore
)
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_NOLOGO = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_CLI_USE_MSBUILD_SERVER = "0"
$env:MSBUILDDISABLENODEREUSE = "1"
Set-Location $PSScriptRoot
[string[]]$buildArgs = @("build", $Target, "-c", $Configuration, "-nr:false", "-m:1", "-p:UseSharedCompilation=false", "-clp:NoSummary;ErrorsOnly")
if (-not $Restore) { $buildArgs += "--no-restore" }
& dotnet @buildArgs 2>&1 |
    Where-Object { $_ -notmatch "MSBuild-Server|MSBuild server|NativeCommandError|CategoryInfo|FullyQualifiedErrorId|In Zeile|^\+ " }
exit $LASTEXITCODE
