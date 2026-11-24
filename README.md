# Il2CppStudio

Il2CppStudio is a high-performance, 100% MIT-licensed C# decompiler and binary analysis tool built specifically for Unity IL2CPP games. Designed to eliminate the need for 4,000 different programs and Python scripts just to decompile something, its goal is to take you from a game binary or console archive straight to readable, compilable C# code. I'd be happy if you'd tell me any bugs or errors you come across so i can fix them.

The tool itself is vibe-coded (and I admit it!), so it won't always output textbook-perfect C# on every edge case yet, but it already compiles massive commercial `Assembly-CSharp` projects with zero errors and operates at blazing speeds.

## Why Il2CppStudio?

- **All-in-One Archive & Console Loading:** You do **not** need external tools like `nstool`, `pkgextract`, or `Il2CppDumper` separately. Just use **File → Load File** (or the CLI) to open `.apk`, `.xapk`, PS4 `.pkg`/`.prx`, or Nintendo Switch `.nsp`/`main` files directly.
- **Bring Your Own Keys (Nintendo Switch):** Il2CppStudio **never** bundles copyrighted console keys or bypasses DRM. When loading a Nintendo Switch `.nsp`, you must provide your own `prod.keys` file (dumped from your own console) via the GUI prompt or the CLI `--keyset` flag.
- **First-Class CLI for AI Agents & Automation:** Alongside the Avalonia desktop GUI, Il2CppStudio ships with a dedicated CLI (`Il2CppStudio.Cli`). This makes it easy for **AI coding agents**, CI pipelines, and reverse-engineering scripts to load builds, inspect metadata/types, dump specific methods, and export full C# projects headlessly.

```bash
# Example: Headless CLI usage (great for AI agents & scripting)
il2cppstudio load "path/to/game" --export "path/to/output"
il2cppstudio load "path/to/game.nsp" --keyset "path/to/prod.keys" --export "path/to/output"
il2cppstudio load "path/to/game.pkg" --passcode "00000000000000000000000000000000"
```

## Current State of the Project

| Area | Status | Details |
| :--- | :--- | :--- |
| **Windows (`x64` / `x86`) & Linux (`x64`)** | Working | Fast metadata loading, native-to-C# lifting, and browsable/exportable C# reconstruction. |
| **Android (`.apk` / `.xapk`)** | Working | Direct archive extraction, automatic Unity version discovery, and 0-error `Assembly-CSharp` compilation on ARM64 builds. |
| **WebGL (`.wasm` + `.data`)** | Working | Decompression (`Brotli`/`Gzip`), `unitywebdata` unpacking, and Wasm32-to-C# lifting. |
| **PlayStation 4 (`.pkg` & `.prx`)** | Working | Native C# `.pkg`/PFS extraction, FSELF-to-ELF64 unpacking (`Il2CppUserAssemblies.prx`), Orbis relocations, and SysV AMD64 lifting. |
| **Nintendo Switch (`main` NSO & `.nsp`)** | Working / Polishing | Commercial Switch `Assembly-CSharp` builds (67,000+ methods) compile with **0 errors**. Direct `.nsp` loading with user-supplied `prod.keys` is actively being polished. |
| **iOS (`.ipa`) & macOS** | Not Yet Supported | Intentionally excluded for now while core platforms are finalized. |
| **Performance & UI Polish** | In Progress | Cold loads take ~1–3s via memory-mapped I/O; `--export-all` optimization (targeting ~20s), on-disk lifted-method caching, and speeding up the GUI script-name search box are currently underway. |
| **Metadata Versions** | v24 – v31+ | Full support for metadata v24 through v31+; pre-v24 support is not yet implemented. |

## Supported Platforms

Currently supported: **Windows (`x64`/`x86`), Linux (`x64`), Android (`.apk`/`.xapk`), WebGL, PlayStation 4 (`.pkg`/`.prx`), and Nintendo Switch (`main` NSO / `.nsp`)**.  
Planned / excluded for now: **iOS** and **macOS**.

## Repositories & Reference Material

Everything in Il2CppStudio is kept **100% MIT-licensed** (zero GPL/AGPL dependencies). The following permissive open-source projects were used directly or studied as structural references:

- **ICSharpCode.Decompiler (ILSpy)** — Powers the C# AST reconstruction and custom decompilation transform pipeline.
- **AsmResolver** — PE/metadata token resolution and stub assembly generation.
- **Il2CppDumper** — Foundational structural reference for binary formats (ELF, PE, NSO, WebAssembly), metadata pointers, and version-specific struct layouts across metadata v24–v31+.
- **Cpp2IL / WasmDisassembler** — Structural reference for WebAssembly disassembly, IR design, calling-convention profiles, and IL2CPP metadata inspection.
- **Iced** — High-performance x86/x64 disassembly and instruction decoding engine.
- **AsmArm64 & Capstone** — Multi-architecture disassembly engines for ARM64 and ARMv7 targets.
- **orbis (`orbis-pkg` / `orbis-pfs`) & ps4-pkg-extractor** — MIT/Apache-2.0 and Unlicense references for native C# PlayStation 4 `.pkg` and PFS extraction.
- **PS5-SELF-TO-ELF** — MIT reference for stripping PlayStation FSELF wrappers back into standard ELF64 binaries.
- **nstool & Ghidra-Switch-Loader** — MIT/ISC references for Nintendo Switch `.nsp`/NCA/NSO container parsing.
- **uwdtool, WDU & Web Global Metadata Parser** — Reference logic for unpacking Unity WebGL `unitywebdata` containers and `global-metadata.dat`.
- **Brotli (`brotli-master`)** — Studied for WebGL compression layout alongside .NET's built-in `System.IO.Compression.BrotliStream`.
- **Avalonia UI** — Cross-platform XAML-based .NET UI framework powering the desktop interface.

## Getting Started & Prerequisites

- **Runtime:** `.NET 11 SDK` (`net11.0`, C# 15).
- **IDE / Editor:** Visual Studio 2026, VS Code, or JetBrains Rider.
- **Local Reference Assets (Optional):** Il2CppStudio can leverage local Unity Hub editor installations (`C:\Program Files\Unity\Hub\Editor`) to reference runtime headers (`libil2cpp`) for specialized builds.

## Building from Source

```bash
git clone https://github.com/ningengit/il2cppStudio.git
cd il2cppStudio
dotnet build -c Release
dotnet publish src\Il2CppStudio.Gui\Il2CppStudio.Gui.csproj -c Release -r win-x64 --self-contained -o dist\win-x64
dotnet publish src\Il2CppStudio.Gui\Il2CppStudio.Gui.csproj -c Release -r win-arm64 --self-contained -o dist\win-arm64
dotnet publish src\Il2CppStudio.Gui\Il2CppStudio.Gui.csproj -c Release -r linux-x64 --self-contained -o dist\linux-x64
dotnet publish src\Il2CppStudio.Gui\Il2CppStudio.Gui.csproj -c Release -r linux-arm64 --self-contained -o dist\linux-arm64
dotnet publish src\Il2CppStudio.Gui\Il2CppStudio.Gui.csproj -c Release -r osx-arm64 --self-contained -o dist\osx-arm64
dotnet publish src\Il2CppStudio.Gui\Il2CppStudio.Gui.csproj -c Release -r osx-x64 --self-contained -o dist\osx-x64

or
cmd /c release.cmd
cmd /c "dotnet test -c Release"
```

To run the desktop GUI directly:

```bash
dotnet run --project src/Il2CppStudio.Gui -c Release
```

## Testing & Validation

Il2CppStudio includes an automated unit and cross-platform integration test suite (`tests/Il2CppStudio.Tests`) guarding against regressions across Windows, Linux, Android, WebGL, PS4, and Switch targets. Run it via PowerShell:

```powershell
.\test.ps1
```

## 📜 License

This project is open-source under the **MIT License**.
