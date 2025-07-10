using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Xml;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;

using Reaganism.CDC.Decompilation;

namespace Reaganism.CDC.Utilities;

internal static class ProjectFileUtil
{
    public static void AddEmbeddedLibrary(
        Resource                                   resource,
        string                                     projectOutputDirectory,
        ProjectDecompiler.ExposedProjectDecompiler decompiler,
        DecompilerSettings                         settings,
        List<Action>                               actions,
        string[]                                   embeddedNamespaces
    )
    {
        using var stream = resource.TryOpenStream();
        if (stream is null)
        {
            throw new InvalidOperationException($"Failed to extract resource '{resource.Name}'");
        }

        stream.Position = 0;
        var module = new PEFile(resource.Name, stream, PEStreamOptions.PrefetchEntireImage);
        AddLibrary(module, projectOutputDirectory, decompiler, settings, actions, embeddedNamespaces);
    }

    public static void AddLibrary(
        PEFile                                     module,
        string                                     projectOutputDirectory,
        ProjectDecompiler.ExposedProjectDecompiler decompiler,
        DecompilerSettings                         settings,
        List<Action>                               actions,
        string[]                                   embeddedNamespaces
    )
    {
        var files     = new HashSet<string>();
        var resources = new HashSet<string>();

        ProjectDecompiler.DecompileModule(module, decompiler, actions, files, resources, projectOutputDirectory, settings, embeddedNamespaces);
        actions.Add(
            WriteProjectFile(
                module,
                "Library",
                projectOutputDirectory,
                files,
                resources,
                x =>
                {
                    x.WriteStartElement("ItemGroup");
                    {
                        foreach (var reference in module.AssemblyReferences.OrderBy(y => y.Name))
                        {
                            if (reference.Name == "mscorlib")
                            {
                                continue;
                            }

                            x.WriteStartElement("Reference");
                            {
                                x.WriteAttributeString("Include", reference.Name);
                            }
                            x.WriteEndElement();
                        }
                    }
                    x.WriteEndElement();
                }
            )
        );
    }

    private static Action WriteProjectFile(
        MetadataFile          module,
        string                outputType,
        string                projectOutputDirectory,
        IEnumerable<string>   sources,
        IEnumerable<string>   resources,
        Action<XmlTextWriter> writeSpecificConfig
    )
    {
        var name     = AssemblyUtil.GetAssemblyTitle(module);
        var fileName = name + ".csproj";

        return () =>
        {
            var path = Path.Combine(projectOutputDirectory, name, fileName);
            PathUtil.CreateParentDirectory(path);

            using var sw = new StreamWriter(path);
            using var w  = new XmlTextWriter(sw);
            w.Formatting = Formatting.Indented;
            {
                w.WriteStartElement("Project");
                {
                    w.WriteAttributeString("Sdk", "Microsoft.NET.Sdk");

                    w.WriteStartElement("Import");
                    {
                        w.WriteAttributeString("Project", "../Configuration.targets");
                    }
                    w.WriteEndElement();

                    w.WriteStartElement("PropertyGroup");
                    {
                        w.WriteElementString("OutputType", outputType);
                        w.WriteElementString("Nullable",   "enable");
                        w.WriteElementString("Version",    new AssemblyName(module.FullName).Version!.ToString());

                        var attributes = AssemblyUtil.GetCustomAttributes(module);
                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Key)
                            {
                                case nameof(AssemblyCompanyAttribute):
                                    w.WriteElementString("Company", attribute.Value);
                                    break;

                                case nameof(AssemblyCopyrightAttribute):
                                    w.WriteElementString("Copyright", attribute.Value);
                                    break;
                            }
                        }

                        w.WriteElementString("RootNamespace", module.Name);
                    }
                    w.WriteEndElement();

                    writeSpecificConfig(w);

                    // Resources.
                    w.WriteStartElement("ItemGroup");
                    {
                        foreach (var resource in ApplyWildcards(resources, sources.ToArray()).OrderBy(x => x))
                        {
                            w.WriteStartElement("EmbeddedResource");
                            {
                                w.WriteAttributeString("Include", resource);
                            }
                            w.WriteEndElement();
                        }
                    }
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
            sw.WriteLine();
        };
    }

    public static Action WriteCommonConfigurationFile(string projectOutputDirectory)
    {
        const string file_name = "Configuration.targets";

        return () =>
        {
            var path = Path.Combine(projectOutputDirectory, file_name);
            PathUtil.CreateParentDirectory(path);

            using var sw = new StreamWriter(path);
            using var w  = new XmlTextWriter(sw);
            w.Formatting = Formatting.Indented;
            {
                w.WriteStartElement("Project");
                {
                    w.WriteStartElement("PropertyGroup");
                    {
                        w.WriteElementString("AllowUnsafeBlocks", "true");
                    }
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
            sw.WriteLine();
        };
    }

    public static Action WriteProjectFile(MetadataFile module, string projectOutputDirectory, IEnumerable<string> sources, IEnumerable<string> resources, string[]? decompiledLibraries)
    {
        return WriteProjectFile(
            module,
            "WinExe",
            projectOutputDirectory,
            sources,
            resources,
            w =>
            {
                w.WriteStartElement("ItemGroup");
                {
                    foreach (var reference in module.AssemblyReferences.OrderBy(x => x.Name))
                    {
                        if (reference.Name == "mscorlib")
                        {
                            continue;
                        }

                        if (decompiledLibraries?.Contains(reference.Name) ?? false)
                        {
                            w.WriteStartElement("ProjectReference");
                            {
                                w.WriteAttributeString("Include", $"../{reference.Name}/{reference.Name}.csproj");
                            }
                            w.WriteEndElement();

                            // TODO: Do we necessarily need to preserve this?
                            w.WriteStartElement("EmbeddedResource");
                            {
                                w.WriteAttributeString("Include", $"../{reference.Name}/bin/$(Configuration)/$(TargetFramework)/{reference.Name}.dll");
                            }
                            w.WriteEndElement();
                        }
                        else
                        {
                            w.WriteStartElement("Reference");
                            {
                                w.WriteAttributeString("Include", reference.Name);
                            }
                            w.WriteEndElement();
                        }
                    }
                }
                w.WriteEndElement();
            }
        );
    }

    private static IEnumerable<string> ApplyWildcards(IEnumerable<string> include, string[] exclude)
    {
        var wildPaths = new HashSet<string>();

        // Dumb patch: sort `include` by the amount of `/`s it has (effectively
        // sorting by depth).  This is so we process deeper files first, fixing
        // a bug where we'd only add one wildcard instead of two even if two is
        // the correct behavior because we matched a shallower case that only
        // necessitated a single wildcard.  There are better ways to resolve
        // this, but this is simple.
        include = include.OrderByDescending(x => x.Count(y => y == '/'));

        foreach (var path in include)
        {
            if (wildPaths.Any(path.StartsWith))
            {
                continue;
            }

            var wildPath = path;
            var cards    = "";
            while (wildPath.Contains('/'))
            {
                var parent = wildPath[..wildPath.LastIndexOf('/')];
                if (exclude.Any(x => x.StartsWith(parent)))
                {
                    // Can't use parent as wildcard.
                    break;
                }

                wildPath = parent;
                if (cards.Length < 2)
                {
                    cards += '*';
                }
            }

            if (wildPath != path)
            {
                wildPaths.Add(wildPath);
                yield return $"{wildPath}/{cards}";
            }
            else
            {
                yield return path;
            }
        }
    }
}