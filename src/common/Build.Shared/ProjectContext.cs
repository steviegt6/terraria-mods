using System.Collections.Generic;
using System.IO;

namespace Build.Shared;

public readonly record struct ProjectContext(string ProjectDirectory, string ProjectFilePath)
{
    public IEnumerable<ProjectFile> EnumerateProjectFiles()
    {
        foreach (var file in Directory.EnumerateFiles(ProjectDirectory, "*.*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(ProjectDirectory, file);
            {
                relativePath = relativePath.Replace('\\', '/');
            }

            if (relativePath.StartsWith("bin/") || relativePath.StartsWith("obj/") || relativePath.StartsWith('.'))
            {
                continue;
            }

            yield return new ProjectFile(relativePath, file);
        }
    }
}