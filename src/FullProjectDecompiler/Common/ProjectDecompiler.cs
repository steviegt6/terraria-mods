using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.ProjectDecompiler;
using ICSharpCode.Decompiler.CSharp.Transforms;
using ICSharpCode.Decompiler.DebugInfo;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.ILSpyX.PdbProvider;

using Terraria.ModLoader.Core;

namespace Tomat.TML.Mod.FullProjectDecompiler.Common;

internal static class ProjectDecompiler
{
    private sealed class SimpleModProjectFileWriter : IProjectFileWriter
    {
        public void Write(TextWriter target, IProjectInfoProvider project, IEnumerable<ProjectItemInfo> files, MetadataFile module)
        {
            throw new System.NotImplementedException();
        }
    }

    private sealed class ExtendedProjectDecompiler(
        DecompilerSettings  settings,
        IAssemblyResolver   assemblyResolver,
        IDebugInfoProvider? debugInfoProvider
    ) : WholeProjectDecompiler(
        settings,
        assemblyResolver,
        new SimpleModProjectFileWriter(),
        assemblyReferenceClassifier: null,
        debugInfoProvider: debugInfoProvider
    );

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

        var decompiler = new ExtendedProjectDecompiler(decompiler_settings, asmResolver, debug);
    }

    private static CSharpDecompiler CreateDecompiler(ExtendedProjectDecompiler projectDecompiler, DecompilerTypeSystem ts, CancellationToken token)
    {
        var decompiler = new CSharpDecompiler(ts, projectDecompiler.Settings)
        {
            CancellationToken = token,
        };
        {
            decompiler.AstTransforms.Add(new EscapeInvalidIdentifiers());
            decompiler.AstTransforms.Add(new RemoveCLSCompliantAttribute());
        }

        return decompiler;
    }

    private static void ExtractResource(string name, Resource resource, string projectDir)
    {
    }
}