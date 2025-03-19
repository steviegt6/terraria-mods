using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using System.Threading.Tasks;

using ICSharpCode.Decompiler.Metadata;

namespace Tomat.TML.Mod.FullProjectDecompiler.Common;

internal sealed class AssemblyResolver : IAssemblyResolver
{
    private readonly Dictionary<string, List<Assembly>> cache = [];

    public AssemblyResolver(string modDir)
    {
        // Add any `lib/` dllReferences the mod includes.
        var libDir = Path.Combine(modDir, "lib");
        if (Directory.Exists(libDir))
        {
            var dllReferences = Directory.GetFiles(libDir, "*.dll", SearchOption.AllDirectories);
            foreach (var dllReference in dllReferences)
            {
                // We don't need to load it into an ALC.
                var asm = Assembly.LoadFile(dllReference);
                if (!cache.TryGetValue(asm.GetName().Name!, out var assemblies))
                {
                    cache.Add(asm.GetName().Name!, assemblies = []);
                }

                assemblies.Add(asm);
            }
        }

        // Add tModLoader and its references.
        var modules = AssemblyLoadContext.Default.Assemblies
                                         .DistinctBy(x => x.ManifestModule.FullyQualifiedName)
                                         .Select(x => x.ManifestModule);
        foreach (var module in modules)
        {
            if (!cache.TryGetValue(module.Assembly.GetName().Name!, out var assemblies))
            {
                cache.Add(module.Assembly.GetName().Name!, assemblies = []);
            }

            assemblies.Add(module.Assembly);
        }
    }

    public MetadataFile? Resolve(IAssemblyReference reference)
    {
        if (!cache.TryGetValue(reference.Name, out var assemblies))
        {
            return null;
        }

        if (assemblies.Count == 1)
        {
            // if (assemblies[0].GetName().Version != reference.Version) { }

            return MakePeFile(assemblies[0]);
        }

        var highestVersion = default(Assembly);
        var exactMatch     = default(Assembly);

        var publicKeyTokenOfName = reference.PublicKeyToken ?? [];

        foreach (var assembly in assemblies)
        {
            var version        = assembly.GetName().Version;
            var publicKeyToken = assembly.GetName().GetPublicKeyToken() ?? [];

            if (version == reference.Version && publicKeyToken.SequenceEqual(publicKeyTokenOfName))
            {
                exactMatch = assembly;
            }
            else if (highestVersion is null || highestVersion.GetName().Version < version)
            {
                highestVersion = assembly;
            }
        }

        var chosen = exactMatch ?? highestVersion;
        return MakePeFile(chosen);

        static PEFile? MakePeFile(Assembly? assembly)
        {
            if (assembly is null)
            {
                return null;
            }

            try
            {
                if (string.IsNullOrEmpty(assembly.Location))
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

            return new PEFile(assembly.Location, PEStreamOptions.PrefetchMetadata);
        }
    }

    public MetadataFile? ResolveModule(MetadataFile mainModule, string moduleName)
    {
        var baseDirectory  = Path.GetDirectoryName(mainModule.FileName)!;
        var moduleFileName = Path.Combine(baseDirectory, moduleName);

        return File.Exists(moduleFileName)
            ? new PEFile(moduleFileName, PEStreamOptions.PrefetchMetadata)
            : null;
    }

    public Task<MetadataFile?> ResolveAsync(IAssemblyReference reference)
    {
        return Task.FromResult(Resolve(reference));
    }

    public Task<MetadataFile?> ResolveModuleAsync(MetadataFile mainModule, string moduleName)
    {
        return Task.FromResult(ResolveModule(mainModule, moduleName));
    }
}