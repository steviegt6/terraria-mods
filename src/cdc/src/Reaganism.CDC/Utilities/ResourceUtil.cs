using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ICSharpCode.Decompiler.Metadata;

namespace Reaganism.CDC.Utilities;

internal static class ResourceUtil
{
    public static void ExtractResource(
        string   projectOutputDirectory,
        string   name,
        Resource resource,
        string   projectDir
    )
    {
        var path = Path.Combine(projectOutputDirectory, projectDir, name);
        PathUtil.CreateParentDirectory(path);

        using var stream = resource.TryOpenStream();
        if (stream is null)
        {
            throw new InvalidOperationException($"Failed to extract resource '{name}'");
        }

        stream.Position = 0;
        using var fileStream = File.Create(path);
        stream.CopyTo(fileStream);
    }

    public static Action ExtractResourceAction(
        string   projectOutputDirectory,
        string   name,
        Resource resource,
        string   projectDir
    )
    {
        return () => ExtractResource(projectOutputDirectory, name, resource, projectDir);
    }

    public static IEnumerable<(string path, Resource resource)> GetResourceFiles(
        MetadataFile metadataFile,
        string[]     embeddedNamespaces
    )
    {
        return metadataFile.Resources.Where(x => x.ResourceType == ResourceType.Embedded)
                           .Select(x => (PathUtil.GetOutputPath(x.Name, metadataFile, embeddedNamespaces), x));
    }
}