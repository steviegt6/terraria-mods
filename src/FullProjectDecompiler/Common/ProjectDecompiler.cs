using System.Diagnostics;
using System.IO;
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

    public static void Decompile(LocalMod mod, string dir, CancellationToken cancellationToken)
    {
        var asmResolver = new AssemblyResolver(dir);
        var modDllPath  = Path.Combine(dir, mod.Name + ".dll");
        {
            Debug.Assert(File.Exists(modDllPath));
        }

        using var fs = File.OpenRead(modDllPath);

        var module = new PEFile(modDllPath, fs, PEStreamOptions.PrefetchEntireImage);
        var debug  = DebugInfoUtils.FromFile(module, Path.Combine(dir, mod.Name + ".pdb"));

        Reaganism.CDC.Decompilation.ProjectDecompiler.Decompile(
            modDllPath,
            Path.Combine(dir, ".."), // temporary fix
            decompiler_settings,
            null,
            null,
            asmResolver,
            debug,
            null,
            false,
            "."
        );

        File.WriteAllText(Path.Combine(dir, mod.Name + ".csproj"), csproj);
    }
}