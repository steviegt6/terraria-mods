using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.ProjectDecompiler;
using ICSharpCode.Decompiler.CSharp.Transforms;
using ICSharpCode.Decompiler.DebugInfo;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;

using JetBrains.Annotations;

using Reaganism.CDC.Utilities;

namespace Reaganism.CDC.Decompilation;

/// <summary>
///     Provides utilities for decompiling projects.
/// </summary>
[PublicAPI]
public static class ProjectDecompiler
{
    internal sealed class ExposedProjectDecompiler(
        DecompilerSettings  settings,
        IAssemblyResolver   assemblyResolver,
        IDebugInfoProvider? debugInfoProvider,
        IProjectFileWriter? projectFileWriter
    ) : WholeProjectDecompiler(
        settings,
        assemblyResolver,
        projectFileWriter,
        assemblyReferenceClassifier: null,
        debugInfoProvider: debugInfoProvider
    )
    {
        // Expose as a public member.
        public new bool IncludeTypeWhenDecompilingProject(MetadataFile module, TypeDefinitionHandle type)
        {
            return base.IncludeTypeWhenDecompilingProject(module, type);
        }
    }

    /// <summary>
    ///     Decompiles the specified target file (assembly).
    /// </summary>
    /// <param name="targetFile">
    ///     The path to the .NET assembly to decompile.
    /// </param>
    /// <param name="sourceOutputDirectory">
    ///     The directory to write the decompiled source code to.
    /// </param>
    /// <param name="decompilerSettings">
    ///     The settings to use when decompiling the target assembly.
    /// </param>
    /// <param name="decompiledLibraries">
    ///     The libraries also being decompiled.
    /// </param>
    /// <param name="embeddedNamespaces">
    ///     Known parts of embedded resource names that can be treated as
    ///     namespaces mirroring directories.
    /// </param>
    [PublicAPI]
    public static void Decompile(
        string              targetFile,
        string              sourceOutputDirectory,
        DecompilerSettings? decompilerSettings  = null,
        string[]?           decompiledLibraries = null,
        string[]?           embeddedNamespaces  = null,
        IAssemblyResolver?  assemblyResolver    = null,
        IDebugInfoProvider? debugInfoProvider   = null,
        IProjectFileWriter? projectFileWriter   = null,
        bool                deleteOldSource     = true,
        string?             projectName         = null
    )
    {
        if (!File.Exists(targetFile))
        {
            throw new FileNotFoundException(targetFile);
        }

        decompilerSettings ??= new DecompilerSettings(LanguageVersion.Latest)
        {
            CSharpFormattingOptions = FormattingOptionsFactory.CreateAllman(),
        };

        embeddedNamespaces ??= [];

        if (deleteOldSource)
        {
            DeleteOldSource(sourceOutputDirectory);
        }

        var mainModule = MetadataUtil.ReadModule(targetFile);

        var projectDecompiler = new ExposedProjectDecompiler(
            decompilerSettings,
            assemblyResolver ?? new EmbeddedAssemblyResolver(mainModule, mainModule.DetectTargetFrameworkId()),
            debugInfoProvider,
            projectFileWriter
        );

        var actions   = new List<Action>();
        var files     = new HashSet<string>();
        var resources = new HashSet<string>();
        var exclude   = new List<string>();

        if (decompiledLibraries is not null)
        {
            foreach (var library in decompiledLibraries)
            {
                var libraryResource = mainModule.Resources.SingleOrDefault(x => x.Name.EndsWith(library + ".dll"));
                if (libraryResource is not null)
                {
                    ProjectFileUtil.AddEmbeddedLibrary(libraryResource, sourceOutputDirectory, projectDecompiler, decompilerSettings, actions, embeddedNamespaces);
                    exclude.Add(PathUtil.GetOutputPath(libraryResource.Name, mainModule, embeddedNamespaces));
                }
                else
                {
                    var asmRef = mainModule.AssemblyReferences.SingleOrDefault(x => x.Name == library);
                    if (asmRef is not null)
                    {
                        var tempModule = projectDecompiler.AssemblyResolver.ResolveModule(mainModule, asmRef.Name + ".dll");
                        if (tempModule is null)
                        {
                            throw new InvalidOperationException($"Cannot resolve library {library} in the target assembly.");
                        }

                        using var fs = File.OpenRead(tempModule.FileName);
                        {
                            var libModule = new PEFile(asmRef.Name, fs, PEStreamOptions.PrefetchEntireImage);
                            ProjectFileUtil.AddLibrary(libModule, sourceOutputDirectory, projectDecompiler, decompilerSettings, actions, embeddedNamespaces);
                            exclude.Add(PathUtil.GetOutputPath(libModule.Name, mainModule, embeddedNamespaces));
                        }
                    }
                    // TODO: Error message or something.
                    /*else
                    {
                        throw new InvalidOperationException($"Cannot find library {library} in the target assembly.");
                    }*/
                }
            }
        }

        DecompileModule(mainModule, projectDecompiler, actions, files, resources, sourceOutputDirectory, decompilerSettings, embeddedNamespaces, exclude, projectName: projectName);

        actions.Add(ProjectFileUtil.WriteProjectFile(mainModule, sourceOutputDirectory, files, resources, decompiledLibraries));
        actions.Add(ProjectFileUtil.WriteCommonConfigurationFile(sourceOutputDirectory));

        ParallelUtil.Execute(actions);
    }

    private static void DeleteOldSource(string sourceOutputDirectory)
    {
        if (Directory.Exists(sourceOutputDirectory))
        {
            foreach (var dir in Directory.GetDirectories(sourceOutputDirectory))
            {
                Directory.Delete(dir, true);
            }

            foreach (var file in Directory.GetFiles(sourceOutputDirectory))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
        else
        {
            Directory.CreateDirectory(sourceOutputDirectory);
        }
    }

    private static CSharpDecompiler CreateDecompiler(DecompilerTypeSystem typeSystem, DecompilerSettings settings)
    {
        var decompiler = new CSharpDecompiler(typeSystem, settings);
        decompiler.AstTransforms.Add(new EscapeInvalidIdentifiers());
        decompiler.AstTransforms.Add(new RemoveCLSCompliantAttribute());
        return decompiler;
    }

    private static IEnumerable<IGrouping<string, TypeDefinitionHandle>> GetCodeFiles(
        MetadataFile             module,
        ExposedProjectDecompiler decompiler,
        string[]                 embeddedNamespaces
    )
    {
        var metadata = module.Metadata;
        return metadata.GetTopLevelTypeDefinitions()
                       .Where(x => decompiler.IncludeTypeWhenDecompilingProject(module, x))
                       .GroupBy(
                            x =>
                            {
                                var type = metadata.GetTypeDefinition(x);
                                var path = WholeProjectDecompiler.CleanUpDirectoryName(metadata.GetString(type.Name)) + ".cs";
                                if (!string.IsNullOrEmpty(metadata.GetString(type.Namespace)))
                                {
                                    path = Path.Combine(WholeProjectDecompiler.CleanUpPath(metadata.GetString(type.Namespace)), path);
                                }

                                return PathUtil.GetOutputPath(path, module, embeddedNamespaces);
                            },
                            StringComparer.OrdinalIgnoreCase
                        );
    }

    private static void DecompileSourceFile(
        DecompilerTypeSystem                    typeSystem,
        IGrouping<string, TypeDefinitionHandle> src,
        string                                  projectOutputDirectory,
        string                                  projectName,
        DecompilerSettings                      settings,
        string?                                 conditional = null
    )
    {
        var path = Path.Combine(projectOutputDirectory, projectName, src.Key);
        PathUtil.CreateParentDirectory(path);

        using var writer = new StringWriter();
        {
            if (conditional is not null)
            {
                writer.WriteLine("#if " + conditional);
            }

            CreateDecompiler(typeSystem, settings)
               .DecompileTypes(src.ToArray())
               .AcceptVisitor(new CSharpOutputVisitor(writer, settings.CSharpFormattingOptions));

            if (conditional is not null)
            {
                writer.WriteLine("#endif");
            }

            var source = writer.ToString();
            File.WriteAllText(path, source);
        }
    }

    private static Action DecompileSourceFileAction(
        DecompilerTypeSystem                    typeSystem,
        IGrouping<string, TypeDefinitionHandle> src,
        string                                  projectOutputDirectory,
        string                                  projectName,
        DecompilerSettings                      settings,
        string?                                 conditional = null
    )
    {
        return () => DecompileSourceFile(typeSystem, src, projectOutputDirectory, projectName, settings, conditional);
    }

    internal static DecompilerTypeSystem DecompileModule(
        MetadataFile             module,
        ExposedProjectDecompiler decompiler,
        List<Action>             actions,
        HashSet<string>          sourceSet,
        HashSet<string>          resourceSet,
        string                   projectOutputDirectory,
        DecompilerSettings       settings,
        string[]                 embeddedNamespaces,
        List<string>?            exclude     = null,
        string?                  conditional = null,
        string?                  projectName = null
    )
    {
        var projectDirectory = projectName ?? AssemblyUtil.GetAssemblyTitle(module);

        var sources   = GetCodeFiles(module, decompiler, embeddedNamespaces).ToList();
        var resources = ResourceUtil.GetResourceFiles(module, embeddedNamespaces).ToList();

        if (exclude is not null)
        {
            sources.RemoveAll(x => exclude.Contains(x.Key));
            resources.RemoveAll(x => exclude.Contains(x.path));
        }

        var typeSystem = new DecompilerTypeSystem(module, decompiler.AssemblyResolver, settings);
        actions.AddRange(
            sources.Where(x => sourceSet.Add(x.Key))
                   .Select(x => DecompileSourceFileAction(typeSystem, x, projectOutputDirectory, projectDirectory, settings, conditional))
        );

        if (conditional is not null && resources.Any(x => !resourceSet.Contains(x.path)))
        {
            throw new InvalidOperationException($"Conditional resources are not supported (conditional: {conditional}).");
        }

        actions.AddRange(
            resources.Where(x => resourceSet.Add(x.path))
                     .Select(x => ResourceUtil.ExtractResourceAction(projectOutputDirectory, x.path, x.resource, projectDirectory))
        );

        return typeSystem;
    }
}