# Il2CppStudio

Il2CppStudio is a high-performance C# decompiler and binary analysis tool built specifically for Unity IL2CPP games. Designed to eliminate the need for 4000 different programs just to decompile something, its goal is to instantly give you highly accurate, compilable C# code.

The tool itself is vibe-coded (and I admit it!), so it may have certain bugs and quirks, but it mostly works just fine and operates at blazing speeds.

## Supported Platforms
It supports (or will support) a wide range of platforms, including **WebGL, Android (APK/XAPK), Windows, Linux, and iOS**.

## A Comprehensive List of Repositories the AI Used

* **Il2CppDumper** — Used as a foundational structural reference for parsing binary envelopes (ELF, PE, WebAssembly), resolving metadata pointers, and mapping version-specific struct layouts across metadata v24–v31+.
* **Cpp2IL / WasmDisassembler** — Used as a structural reference for WebAssembly disassembly handling, intermediate representation (IR) design, calling-convention profiles, and IL2CPP metadata inspection patterns.
* **Iced** — High-performance, razor-sharp x86/x64 disassembly and instruction decoding engine utilized for native binary analysis.
* **Capstone** — Multi-architecture disassembly engine bindings leveraged for cross-platform support across ARM, ARMv7, ARM64, and alternative CPU targets.
* **uwdtool (uwdtool-master)** — Provided essential structural insights and reference logic for unpacking Unity WebGL data containers (`unitywebdata`).
* **WDU** (SeriousCache/UABE) — Used for modeling and extracting Unity WebGL metadata container formats.
* **Web Global Metadata Parser** — Clean reference parsing implementation for handling web-packaged `global-metadata.dat` files.
* **Brotli (brotli-master)** — Google's reference compression library, studied for structural layout lookup while implementing high-speed extraction via .NET's built-in `System.IO.Compression.BrotliStream`.
* **Avalonia UI** — The cross-platform XAML-based .NET UI framework powering Il2CppStudio’s modern, hardware-accelerated desktop interface.
* **AsmArm64** - Still needs to test wether this or capstone is faster at arm64 disassembly.

## Getting Started & Prerequisites
* **Runtime:** .NET 11 SDK (with .NET 8.0 fallback).
* **IDE / Editor:** Visual Studio 2026, VS Code, or JetBrains Rider.
* **Local Reference Assets (Optional for Version Profiles):** Il2CppStudio can leverage local Unity Hub editor installations (`C:\Program Files\Unity\Hub\Editor`) to reference exact runtime headers (`libil2cpp`) for specialized builds.

## Building from Source

```bash
git clone https://github.com/ningengit/il2cppStudio.git
cd il2cppStudio
dotnet build -c Release
```

## Testing & Validation
Il2CppStudio includes a robust test harness (`tests/Il2CppStudio.Tests`) utilizing unit and integration tests to guard against regressions across reference scripts. Run the test suite via PowerShell:

```powershell
.\test.ps1
```

## 📜 License
This project is open-source under the MIT License.
