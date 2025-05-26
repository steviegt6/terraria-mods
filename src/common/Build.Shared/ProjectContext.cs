using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Build.Shared;

public readonly record struct ProjectContext(string ProjectDirectory, string ProjectFilePath, Dictionary<string, ProjectFile[]> Paths)
{
    public static ProjectContext Create(string projectDirectory, string[] args)
    {
        if (args.Length <= 1)
        {
            return new ProjectContext(projectDirectory, args[0], []);
        }

        var paths = new Dictionary<string, ProjectFile[]>();
        for (var i = 1; i < args.Length; i++)
        {
            var pathGroup = args[i];
            var groupParts = pathGroup.Split('=');
            if (groupParts.Length != 2)
            {
                continue;
            }

            var groupName = groupParts[0].ToLowerInvariant();
            var groupPaths = groupParts[1].Split(';');

            paths[groupName] = groupPaths.Select(
                x => new ProjectFile(x.Replace('\\', '/'), Path.Combine(projectDirectory, x))
            ).ToArray();
        }
        return new ProjectContext(projectDirectory, args[0], paths);
    }

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

    public IEnumerable<ProjectFile> EnumerateGroup(string name)
    {
        return Paths.TryGetValue(name.ToLowerInvariant(), out var files) ? files : [];
    }
}