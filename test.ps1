# Runs the regression suite (xUnit v3 in-process runner). Builds first unless -NoBuild.
param([switch]$NoBuild, [string]$Filter)
Set-Location $PSScriptRoot
if (-not $NoBuild) { & .\build.ps1 tests\Il2CppStudio.Tests\Il2CppStudio.Tests.csproj; if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE } }
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$runArgs = @("tests\Il2CppStudio.Tests\bin\Debug\net11.0\Il2CppStudio.Tests.dll")
if ($Filter) { $runArgs += @("-method", $Filter) }
& dotnet @runArgs
exit $LASTEXITCODE
