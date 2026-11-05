Il2CppStudio is a C# decompiler and binary analysis tool built specifically for Unity il2cpp games. Designed to eliminate the need for 4000 programs just to decompile something and immediately get compileable C# code

The tool itself is vibe coded and i admit it, so it may have certain bugs and stuff but it mostly works just fine.

**so here is a comprehensive list of repositories that the AI used**

Cpp2IL / WasmDisassembler — Used as a structural reference for WebAssembly disassembly handling and IL2CPP metadata inspection patterns.

Iced — High-performance, razor-sharp x86/x64 disassembly and instruction decoding engine utilized for native binary analysis.

Capstone & Capstone.NET — Multi-architecture disassembly engine bindings leveraged for cross-platform support across ARM, ARM64, and alternative CPU targets.

uwdtool (uwdtool-master) — Provided essential structural insights and reference logic for unpacking Unity WebGL data containers (unitywebdata).

[WDU Code](https://local/WDU code) — Used for modeling and extracting Unity WebGL metadata container formats.

Web Global Metadata Parser (web-global-metadata-parser-main) — Clean reference parsing implementation for handling web-packaged global-metadata.dat files.

Brotli (brotli-master) — Google's reference compression library, studied for structural layout lookup while implementing high-speed extraction via .NET's built-in System.IO.Compression.BrotliStream.

Avalonia UI — The cross-platform XAML-based .NET UI framework powering Il2CppStudio’s modern, hardware-accelerated desktop interface.

It will support a lot of platforms like Webgl, Android, iOS and soo on

**Getting Started & Prerequisites**
Runtime: .NET 8.0 / .NET 11+ SDK

IDE / Editor: Visual Studio 2022 or VS Code / Rider

Local Reference Assets (Optional for Version Profiles): Il2CppStudio can leverage local Unity Hub editor installations (C:\Program Files\Unity\Hub\Editor) to reference exact runtime headers (libil2cpp) for specialized builds.

**Building from Source**

git clone https://github.com/YourUsername/Il2CppStudio.git
cd Il2CppStudio
dotnet build -c Release

**Testing & Validation**
Il2CppStudio includes a robust test harness (tests/Il2CppStudio.Tests) utilizing unit and integration tests to guard against regressions across reference scripts. Run the test suite via PowerShell:

PowerShell
.\test.ps1
📜 License
This project is open-source under the MIT License.
