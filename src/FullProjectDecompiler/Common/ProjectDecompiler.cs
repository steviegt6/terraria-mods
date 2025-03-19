using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.ProjectDecompiler;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.ILSpyX.PdbProvider;

using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Tomat.TML.Mod.FullProjectDecompiler.Common;

[NoJIT]
internal static class ProjectDecompiler
{
    private sealed class SimpleModProjectFileWriter : IProjectFileWriter
    {
        public void Write(TextWriter target, IProjectInfoProvider project, IEnumerable<ProjectItemInfo> files, MetadataFile module)
        {
            throw new System.NotImplementedException();
        }
    }

    private static readonly CSharpFormattingOptions formatting_options;

    private static readonly DecompilerSettings decompiler_settings = new(LanguageVersion.Latest)
    {
        RemoveDeadCode          = true,
        CSharpFormattingOptions = formatting_options,

        // tML disabled SwitchExpressions, but we can keep them.
        // SwitchExpressions = false,
    };

    static ProjectDecompiler()
    {
        formatting_options = FormattingOptionsFactory.CreateAllman();
        {
            // TODO: Tweak formatting options to my biased tastes.

            formatting_options.IndentationString          = "    ";
            formatting_options.ArrayInitializerWrapping   = Wrapping.WrapAlways;
            formatting_options.ArrayInitializerBraceStyle = BraceStyle.EndOfLine;
        }
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
            dir,
            decompiler_settings,
            null,
            null,
            asmResolver,
            debug,
            new SimpleModProjectFileWriter()
        );
    }
}