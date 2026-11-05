Il2CppStudio is a C# decompiler and binary analysis tool built specifically for Unity il2cpp games. Designed to eliminate the need for 4000 programs just to decompile something and immediately get compileable C# code

The tool itself is vibe coded and i admit it, so it may have certain bugs and stuff but it mostly works just fine.

It will support a lot of platforms like Webgl, Android, iOS and soo on

📦 Getting Started & Prerequisites
Runtime: .NET 8.0 / .NET 11+ SDK

IDE / Editor: Visual Studio 2022 or VS Code / Rider

Local Reference Assets (Optional for Version Profiles): Il2CppStudio can leverage local Unity Hub editor installations (C:\Program Files\Unity\Hub\Editor) to reference exact runtime headers (libil2cpp) for specialized builds.

Building from Source

git clone https://github.com/YourUsername/Il2CppStudio.git
cd Il2CppStudio
dotnet build -c Release
🧪 Testing & Validation
Il2CppStudio includes a robust test harness (tests/Il2CppStudio.Tests) utilizing unit and integration tests to guard against regressions across reference scripts. Run the test suite via PowerShell:

PowerShell
.\test.ps1
📜 License
This project is open-source under the MIT License.
