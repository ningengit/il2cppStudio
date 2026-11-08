using System.CommandLine;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Cli.Commands;

/// <summary><c>metadata</c>, <c>pe</c> and <c>disasm</c>: structural inspection of the metadata and the native binary.</summary>
internal static class InfoCommands
{
    public static Command Metadata(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("path") { Description = "Game folder, *_Data folder, or global-metadata.dat." };
        var types = new Option<int>("--types") { Description = "Print the first N types with members.", DefaultValueFactory = _ => 0 };
        var filter = new Option<string?>("--filter") { Description = "Only print types whose full name contains this text." };
        var cmd = new Command("metadata", "Parse global-metadata.dat (v27-v31) and report its structure.") { path, types, filter };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            var files = GameFolderLocator.Locate(pr.GetValue(path)!, sink);
            if (files.MetadataPath is null) return 2;
            using var md = GlobalMetadata.Load(files.MetadataPath, sink);
            var model = MetadataModel.Build(md, sink);
            if (pr.GetValue(json))
            {
                CliHost.WriteJson(new { files, model.Statistics, Sections = md.SectionReports, Assemblies = model.Assemblies.Select(a => new { a.Name, a.Version, a.TypeCount, a.References }) });
                return sink.HasErrors ? 1 : 0;
            }
            var s = model.Statistics;
            Console.WriteLine($"File      : {md.Path}");
            Console.WriteLine($"Version   : v{s.Version} ({s.UnityRange})  size {s.FileSize:N0} bytes");
            Console.WriteLine($"Images {s.Images}, assemblies {s.Assemblies}, types {s.Types:N0}, methods {s.Methods:N0}, fields {s.Fields:N0}, params {s.Parameters:N0}");
            Console.WriteLine($"Properties {s.Properties:N0}, events {s.Events:N0}, generic containers {s.GenericContainers:N0}, string literals {s.StringLiterals:N0}, vtable slots {s.VTableSlots:N0}");
            Console.WriteLine();
            Console.WriteLine($"{"Section",-40} {"Offset",10} {"Size",12} {"Elem",5} {"Count",9}  OK");
            foreach (var r in md.SectionReports.OrderBy(r => r.Offset))
                Console.WriteLine($"{r.Name,-40} 0x{r.Offset,8:X} {r.Size,12:N0} {r.ElementSize,5} {r.ElementCount,9:N0}  {(r.InBounds && r.Aligned ? "yes" : "NO")}");
            Console.WriteLine();
            Console.WriteLine("Assemblies:");
            foreach (var a in model.Assemblies.OrderByDescending(a => a.TypeCount))
                Console.WriteLine($"  {a.Name,-45} v{a.Version,-14} {a.TypeCount,6:N0} types");

            var n = pr.GetValue(types);
            var f = pr.GetValue(filter);
            if (n > 0 || f is not null)
            {
                Console.WriteLine();
                var selected = model.Types.Where(t => f is null || t.FullName.Contains(f, StringComparison.OrdinalIgnoreCase));
                foreach (var t in n > 0 ? selected.Take(n) : selected)
                {
                    Console.WriteLine($"[{t.ImageName}] {t.Kind} {t.FullName}{(t.GenericParameters.Count > 0 ? $"<{string.Join(", ", t.GenericParameters)}>" : "")}  token 0x{t.Token:X8}");
                    foreach (var fl in t.Fields) Console.WriteLine($"    field  {fl.Name}");
                    foreach (var p in t.Properties) Console.WriteLine($"    prop   {p.Name} {{ {(p.GetterName is null ? "" : "get; ")}{(p.SetterName is null ? "" : "set; ")}}}");
                    foreach (var m in t.Methods) Console.WriteLine($"    method {m.Access} {(m.IsStatic ? "static " : "")}{m.DisplaySignature}  token 0x{m.Token:X8} slot {(m.Slot == ushort.MaxValue ? "-" : m.Slot)}");
                }
            }
            return sink.HasErrors ? 1 : 0;
        }));
        return cmd;
    }

    public static Command Pe(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("path") { Description = "Game folder or GameAssembly.dll." };
        var cmd = new Command("pe", "Inspect GameAssembly.dll headers, sections, exports and compiler fingerprint.") { path };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            var files = GameFolderLocator.Locate(pr.GetValue(path)!, sink);
            if (files.BinaryPath is null) return 2;
            if (!files.BinaryPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                sink.Error(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatUnsupported, "Only Windows PE (GameAssembly.dll) is supported.", new() { File = files.BinaryPath });
                return 2;
            }
            using var pe = PeBinaryImage.Load(files.BinaryPath, sink);
            var exports = pe.GetExports();
            var imports = pe.GetImports();
            var cv = pe.GetCodeView();
            var rich = RichHeader.TryParse(pe.Data.Span);
            if (pr.GetValue(json))
            {
                CliHost.WriteJson(new { pe.Headers, pe.Architecture, pe.Sections, CodeView = cv, Rich = rich, Imports = imports,
                    Il2CppExports = exports.Count(e => e.Name?.StartsWith("il2cpp_") == true), ExportCount = exports.Count, RuntimeFunctions = pe.GetRuntimeFunctions().Count });
                return sink.HasErrors ? 1 : 0;
            }
            var h = pe.Headers;
            Console.WriteLine($"File        : {pe.Path}");
            Console.WriteLine($"Machine     : {h.Machine} ({pe.Architecture}, {(pe.Is64Bit ? "PE32+" : "PE32")})  image base 0x{h.ImageBase:X}");
            Console.WriteLine($"Linker      : {h.LinkerVersion}   timestamp 0x{h.TimeDateStamp:X8}   subsystem {h.Subsystem}");
            Console.WriteLine($"PDB         : {(cv is null ? "none" : $"{cv.PdbPath}  guid {cv.Guid} age {cv.Age}")}");
            Console.WriteLine($"Exports     : {exports.Count} ({exports.Count(e => e.Name?.StartsWith("il2cpp_") == true)} il2cpp_* API functions)");
            Console.WriteLine($"Functions   : {pe.GetRuntimeFunctions().Count:N0} (.pdata, chained fragments merged)");
            Console.WriteLine($"Imports     : {string.Join(", ", imports.Select(i => i.Name))}");
            Console.WriteLine();
            Console.WriteLine($"{"Section",-10} {"RVA",10} {"VSize",12} {"FileOff",10} {"RawSize",12}  Flags");
            foreach (var sec in pe.Sections)
                Console.WriteLine($"{sec.Name,-10} 0x{sec.Rva,8:X} {sec.VirtualSize,12:N0} 0x{sec.FileOffset,8:X} {sec.RawSize,12:N0}  {(sec.IsReadable ? "R" : "-")}{(sec.IsWritable ? "W" : "-")}{(sec.IsExecutable ? "X" : "-")}");
            if (rich is not null)
            {
                Console.WriteLine();
                Console.WriteLine($"Rich header (checksum {(rich.ChecksumValid ? "valid" : "INVALID")}):");
                foreach (var e in rich.Entries.OrderByDescending(e => e.Count))
                    Console.WriteLine($"  0x{e.ProductId:X4} build {e.BuildNumber,5} x{e.Count,6}  {e.ProductName,-34} {e.ToolsetGuess}");
                Console.WriteLine($"  => LTCG objects present: {rich.Entries.Any(e => e.IsLtcg)} (Unity 'Master' C++ configuration enables LTCG; 'Release' does not)");
            }
            return sink.HasErrors ? 1 : 0;
        }));
        return cmd;
    }

    public static Command Helpers(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("folder-path");
        var unclassified = new Option<int>("--unclassified") { Description = "Also list the N most-called unclassified targets with raw statistics.", DefaultValueFactory = _ => 0 };
        var cmd = new Command("helpers", "Identify IL2CPP runtime helpers (write barrier, object_new, casts, raise…) and show the evidence.") { path, unclassified };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            using var ws = CliHost.Load(pr.GetValue(path)!, sink, substituteBcl: true);
            var map = ws.RuntimeHelpers;
            if (map is null) { Console.Error.WriteLine("No native code available."); return 2; }
            var rows = map.Helpers.OrderBy(h => h.Kind).ThenByDescending(h => h.CallSites).ToList();
            if (pr.GetValue(json)) { CliHost.WriteJson(rows.Select(h => new { Rva = $"0x{h.Va - ws.Binary!.ImageBase:X}", h.Kind, h.CallSites, h.Evidence })); return 0; }
            foreach (var h in rows) Console.WriteLine($"sub_{h.Va - ws.Binary!.ImageBase:X8}  {h.Kind,-16} {h.CallSites,6} sites  {h.Evidence}");
            if (pr.GetValue(unclassified) is var n and > 0)
            {
                Console.WriteLine($"\n{"target",-13} {"sites",6} {"wbar",6} {"int3",6} {"rcxSet",6} {"rdxTI",6} {"leaUse",6} {"raxUse",6}  top next call");
                foreach (var (va, s) in map.Stats.Where(k => map.KindOf(k.Key) == Core.Lifting.Runtime.RuntimeHelperKind.Unknown).OrderByDescending(k => k.Value.Sites).Take(n))
                {
                    var next = s.NextCalls.OrderByDescending(x => x.Value).FirstOrDefault();
                    Console.WriteLine($"sub_{va - ws.Binary!.ImageBase:X8} {s.Sites,6} {s.WriteBarrierShape,6} {s.FollowedByInt3,6} {s.RcxSetJustBefore,6} {s.RdxFromTypeInfo,6} {s.RcxLeaUsage,6} {s.RaxUsedAfter,6}  " +
                        (next.Key == 0 ? "-" : $"sub_{next.Key - ws.Binary.ImageBase:X} x{next.Value}"));
                }
            }
            return 0;
        }));
        return cmd;
    }

    public static Command Disasm(Option<bool> verbose, Option<bool> json)
    {
        var path = new Argument<string>("folder-path");
        var type = new Option<string>("--type") { Required = true };
        var method = new Option<string?>("--method");
        var cmd = new Command("disasm", "Annotated x86-64 disassembly of a method (or all methods of a type).") { path, type, method };
        cmd.SetAction(pr => CliHost.Run(pr.GetValue(verbose), sink =>
        {
            using var ws = CliHost.Load(pr.GetValue(path)!, sink, substituteBcl: true);
            if (!CliHost.TryResolve(ws, pr.GetValue(type)!, pr.GetValue(method), out var t, out var methods)) return 2;
            foreach (var m in methods.Count > 0 ? methods : t.Methods)
            {
                Console.WriteLine($"; ===== {t.FullName}::{m.DisplaySignature}  RVA 0x{ws.GetMethodRva(m):X}");
                Console.WriteLine(ws.DisassembleMethod(m) ?? "; no native code");
            }
            return 0;
        }));
        return cmd;
    }
}
