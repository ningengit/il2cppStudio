@echo off
setlocal enabledelayedexpansion
pushd "%~dp0"

echo === Il2CppStudio Release Build (v2.4.0) ===

if exist "dist" (
    echo Cleaning dist\ ...
    rmdir /s /q "dist"
    if !errorlevel! neq 0 goto :fail
)
mkdir "dist"
if !errorlevel! neq 0 goto :fail

for %%R in (win-x64 win-arm64 linux-x64 linux-arm64 osx-x64 osx-arm64) do (
    echo.
    echo --- Publishing [%%R] GUI ---
    dotnet publish "src\Il2CppStudio.Gui\Il2CppStudio.Gui.csproj" -c Release -r %%R --self-contained true -o "dist\%%R"
    if !errorlevel! neq 0 goto :fail

    echo --- Publishing [%%R] CLI ---
    dotnet publish "src\Il2CppStudio.Cli\Il2CppStudio.Cli.csproj" -c Release -r %%R --self-contained true -o "dist\%%R"
    if !errorlevel! neq 0 goto :fail
)

echo.
echo --- Building [android-arm64] APK ---
dotnet build "src\Il2CppStudio.Android\Il2CppStudio.Android.csproj" -c Release
if !errorlevel! neq 0 goto :fail
if not exist "dist\android-arm64\com.il2cppstudio.app-Signed.apk" (
    echo ERROR: Expected APK not found at dist\android-arm64\com.il2cppstudio.app-Signed.apk
    goto :fail
)

echo.
echo === Release Artifacts Summary ===
for %%R in (win-x64 win-arm64) do (
    echo   %%R : dist\%%R\Il2CppStudio.Gui.exe + dist\%%R\il2cppstudio.exe
)
for %%R in (linux-x64 linux-arm64 osx-x64 osx-arm64) do (
    echo   %%R : dist\%%R\Il2CppStudio.Gui + dist\%%R\il2cppstudio
)
for %%F in ("dist\android-arm64\com.il2cppstudio.app-Signed.apk") do (
    echo   android-arm64 : %%~fF ^(%%~zF bytes^)
)
echo.
echo All 7 Release targets succeeded.
popd
endlocal
exit /b 0

:fail
echo.
echo ERROR: Release build failed.
popd
endlocal
exit /b 1
