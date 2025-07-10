using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using ICSharpCode.Decompiler.Metadata;

namespace Reaganism.CDC.Utilities;

internal static class PathUtil
{
    private static readonly string[] non_srs_directories = [".git", ".vs", ".idea", "bin", "obj"];

    public static string NormalizePath(string path)
    {
        return path.Replace('\\', '/');
    }

    public static string GetOutputPath(string path, MetadataFile metadataFile, string[] embeddedNamespaces)
    {
        // If this is an assembly.
        if (path.EndsWith(".dll"))
        {
            // ReSharper disable once AccessToModifiedClosure - Captured and
            //                                                  used before any
            //                                                  mutations.
            var assemblyReference = metadataFile.AssemblyReferences.SingleOrDefault(x => path.EndsWith(x.Name + ".dll"));
            if (assemblyReference is not null)
            {
                path = Path.Combine(path[..(path.Length - assemblyReference.Name.Length - 5)], assemblyReference.Name + ".dll");
            }
        }

        var rootNamespace = AssemblyUtil.GetAssemblyTitle(metadataFile);
        if (path.StartsWith(rootNamespace))
        {
            path = path[(rootNamespace.Length + 1)..];
        }

        // Short-circuit if there's no directory handling needed.
        if (!path.Contains('.') && !path.Contains('/') && !path.Contains('\\'))
        {
            return path;
        }

        var bestEmbeddedNamespace = default(string);
        foreach (var embeddedNamespace in embeddedNamespaces)
        {
            // Support cases where the file actually has no namespace (in which
            // one may specify the entire file name).
            if (path == embeddedNamespace)
            {
                return path;
            }

            if (path.StartsWith(embeddedNamespace + '.') && (bestEmbeddedNamespace is null || embeddedNamespace.Length > bestEmbeddedNamespace.Length))
            {
                bestEmbeddedNamespace = embeddedNamespace;
            }
        }

        if (bestEmbeddedNamespace is not null)
        {
            path = path.Replace(bestEmbeddedNamespace + '.', bestEmbeddedNamespace + '/');
        }

        path = path.Replace('\\', '/');

        var lastDirectorySeparatorIndex = path.IndexOf('/');
        if (lastDirectorySeparatorIndex < 0)
        {
            lastDirectorySeparatorIndex = path.LastIndexOf('.');
        }

        return new StringBuilder(path)
              .Replace('.', '/', 0, lastDirectorySeparatorIndex)
              .ToString();
    }

    public static bool DeleteEmptyDirectories(string directory)
    {
        return !Directory.Exists(directory) || Recurse(directory);

        static bool Recurse(string directory)
        {
            var allEmpty = true;

            foreach (var subDirectory in Directory.EnumerateDirectories(directory))
            {
                allEmpty &= Recurse(subDirectory);
            }

            if (!allEmpty || Directory.EnumerateFiles(directory).Any())
            {
                return false;
            }

            Directory.Delete(directory);
            return true;
        }
    }

#region File enumeration
    public static IEnumerable<(string fullPath, string relativePath)> EnumerateSourceFiles(string directory)
    {
        return EnumerateFiles(directory)
           .Where(x => !x.relativePath.Split('/', '\\').Any(non_srs_directories.Contains));
    }

    public static IEnumerable<(string fullPath, string relativePath)> EnumerateFiles(string directory)
    {
        return Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
                        .Select(x => (fullPath: x, relativePath: GetRelativePath(directory, x)));
    }

    private static string GetRelativePath(string directory, string fullPath)
    {
        // Sanitization: remove or add directory separator character as needed.
        {
            if (fullPath[^1] == Path.DirectorySeparatorChar)
            {
                fullPath = fullPath[..^1];
            }

            if (directory[^1] != Path.DirectorySeparatorChar)
            {
                directory += Path.DirectorySeparatorChar;
            }
        }

        if (fullPath + Path.DirectorySeparatorChar == directory)
        {
            return string.Empty;
        }

        Debug.Assert(fullPath.StartsWith(directory));
        {
            return fullPath[directory.Length..];
        }
    }
#endregion

#region Directory creation
    public static DirectoryInfo CreateParentDirectory(string path)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(Path.GetDirectoryName(path)));
        return Directory.CreateDirectory(Path.GetDirectoryName(path)!);
    }
#endregion

#region File copying
    public static void Copy(string fromPath, string toPath)
    {
        CreateParentDirectory(toPath);
        File.Copy(fromPath, toPath, true);
    }
#endregion
}