using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.ILSpyX.PdbProvider;

using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Tomat.TML.Mod.FullProjectDecompiler.Common;

[NoJIT]
internal static class ProjectDecompiler
{
    private const string csproj = "<Project Sdk=\"Microsoft.NET.Sdk\">\n"
                                + "\n"
                                + "    <Import Condition=\"Exists('..\\tModLoader.targets')\" Project=\"..\\tModLoader.targets\"/>\n"
                                + "    <Import Condition=\"Exists('..\\..\\tModLoader.targets')\" Project=\"..\\..\\tModLoader.targets\"/>\n"
                                + "    <Import Condition=\"Exists('..\\..\\..\\tModLoader.targets')\" Project=\"..\\..\\..\\tModLoader.targets\"/>\n"
                                + "\n"
                                + "    <ItemGroup>\n"
                                + "        <Reference Include=\"lib\\**\"/>\n"
                                + "    </ItemGroup>\n"
                                + "\n"
                                + "</Project>";

    private static readonly CSharpFormattingOptions formatting_options;
    private static readonly DecompilerSettings      decompiler_settings;

    static ProjectDecompiler()
    {
        formatting_options = FormattingOptionsFactory.CreateAllman();
        {
            // TODO: Tweak formatting options to my biased tastes.

            formatting_options.IndentationString          = "    ";
            formatting_options.ArrayInitializerWrapping   = Wrapping.WrapAlways;
            formatting_options.ArrayInitializerBraceStyle = BraceStyle.EndOfLine;
        }

        decompiler_settings = new DecompilerSettings(LanguageVersion.Latest)
        {
            RemoveDeadCode          = true,
            CSharpFormattingOptions = formatting_options,

            // tML disabled SwitchExpressions, but we can keep them.
            // SwitchExpressions = false,
        };
    }

    public static void Decompile(LocalMod mod, string dir, DecompiledMod decompiledMod, CancellationToken cancellationToken)
    {
        var asmResolver = new AssemblyResolver(dir);

        using var fs = File.OpenRead(decompiledMod.DllPath);

        var module = new PEFile(decompiledMod.DllPath, fs, PEStreamOptions.PrefetchEntireImage);
        var debug = decompiledMod.PdbPath is not null
            ? DebugInfoUtils.FromFile(module, decompiledMod.PdbPath)
            : null;

        var stagingDir = Path.Combine(dir, "__decompiled");
        {
            Directory.CreateDirectory(stagingDir);
        }

        // Decompile to staging directory.
        Reaganism.CDC.Decompilation.ProjectDecompiler.Decompile(
            decompiledMod.DllPath,
            stagingDir,
            decompiler_settings,
            null,
            null,
            asmResolver,
            debug,
            null,
            false,
            "."
        );

        // Copy root files of the staging directory to the final directory.
        foreach (var file in Directory.EnumerateFiles(stagingDir))
        {
            var fileName = Path.GetFileName(file);
            var destFile = Path.Combine(dir, fileName);

            File.Copy(file, destFile, true);
        }

        // Move the contents of the decompiled directory to the final directory.
        var modSourceDirectory = Directory.EnumerateDirectories(stagingDir).Single();
        {
            foreach (var file in Directory.EnumerateFiles(modSourceDirectory, "*", SearchOption.AllDirectories))
            {
                var relativePath = Path.GetRelativePath(modSourceDirectory, file);
                var destFile     = Path.Combine(dir, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
                File.Copy(file, destFile, true);
            }
        }

        Directory.Delete(stagingDir, true);

        // Let's get rid any generated .csproj files from decompilation.
        var csprojNames = new[]
        {
            // .csproj potentially generated in mod directory.
            Path.Combine(dir, mod.Name + ".csproj"),

            // .csproj potentially generated in ModAssemblies directory.
            Path.Combine(Path.GetDirectoryName(decompiledMod.DllPath)!, Path.GetFileNameWithoutExtension(decompiledMod.DllPath) + ".csproj"),
        };

        foreach (var csprojName in csprojNames)
        {
            File.Delete(csprojName);
        }
    }

    public static void WriteCsproj(LocalMod mod, string dir)
    {
        var csprojName = Path.Combine(dir, mod.Name + ".csproj");

        if (!File.Exists(csprojName))
        {
            File.WriteAllText(csprojName, csproj);
        }
    }
}