using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

using ICSharpCode.Decompiler.Metadata;

namespace Reaganism.CDC.Decompilation;

internal sealed class EmbeddedAssemblyResolver : IAssemblyResolver
{
    private readonly PEFile                    metadataFile;
    private readonly UniversalAssemblyResolver resolver;

    // It's possible for the cached metadata file to be null if we fail to
    // resolve the assembly.  We keep this anyway since we don't want to keep
    // repeating our resolution logic.
    private readonly Dictionary<string, MetadataFile?> cache = [];

    public EmbeddedAssemblyResolver(PEFile metadataFile, string targetFramework)
    {
        this.metadataFile = metadataFile;

        resolver = new UniversalAssemblyResolver(metadataFile.FileName, true, targetFramework, streamOptions: PEStreamOptions.PrefetchMetadata);
        {
            if (Path.GetDirectoryName(metadataFile.FileName) is not { } dirName || string.IsNullOrWhiteSpace(dirName))
            {
                // TODO: This doesn't necessarily require an error.
                throw new InvalidOperationException($"Cannot resolve embedded assemblies because given metadata file has no directory or one without a directory name: {metadataFile.FileName}");
            }

            resolver.AddSearchDirectory(dirName);
        }
    }

    MetadataFile? IAssemblyResolver.Resolve(IAssemblyReference reference)
    {
        lock (this)
        {
            if (cache.TryGetValue(reference.FullName, out var module))
            {
                return module;
            }

            // Assume the actual name is just the name of the assembly with a
            // ".dll" extension.  This is somewhat naive but acceptable for most
            // use cases.  If this isn't correct, one can implement their own
            // resolution logic on top of this by wrapping.
            var assemblyFileName = reference.Name + ".dll";

            // Find the expected file within the resources of this PE file.
            // Search specifically for embedded resources since that's the only
            // case we're expecting to cover (in relation to .NET assemblies).
            // TODO: Handle cases where there are multiple resources with the
            //       same file name?
            // TODO: Split paths to search for the exact file name rather than
            //       just whether the resolved one ends with the expected name?
            var resource = metadataFile.Resources.Where(x => x.ResourceType is ResourceType.Embedded).SingleOrDefault(x => x.Name.EndsWith(assemblyFileName));
            {
                Debug.Assert(resource?.Name.EndsWith(assemblyFileName) ?? true);
            }

            if (resource is not null)
            {
                if (resource.TryOpenStream() is { } stream)
                {
                    module = new PEFile(resource.Name, stream);
                }
                else
                {
                    // TODO: This doesn't necessarily require an error.
                    throw new InvalidOperationException($"Failed to open stream for embedded assembly: {resource.Name}");
                }
            }

            // If we haven't found the assembly as an embedded resource, fall
            // back to regular resolution behavior.
            module ??= resolver.Resolve(reference);

            return cache[reference.FullName] = module;
        }
    }

    MetadataFile? IAssemblyResolver.ResolveModule(MetadataFile mainModule, string moduleName)
    {
        // TODO: Should we search embedded resources in this variant?
        return resolver.ResolveModule(mainModule, moduleName);
    }

    Task<MetadataFile?> IAssemblyResolver.ResolveAsync(IAssemblyReference reference)
    {
        return Task.FromResult(((IAssemblyResolver)this).Resolve(reference));
    }

    Task<MetadataFile?> IAssemblyResolver.ResolveModuleAsync(MetadataFile mainModule, string moduleName)
    {
        return Task.FromResult(((IAssemblyResolver)this).ResolveModule(mainModule, moduleName));
    }
}