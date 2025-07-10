using System.IO;
using System.Linq;

using JetBrains.Annotations;

using Reaganism.CDC.Diffing;

namespace Reaganism.CDC.Utilities.Extensions;

[PublicAPI]
public static class DifferSettingsExtensions
{
    private sealed class IgnoreCommonDirectoriesProvider : IFileDiffTypeProvider
    {
        private static readonly string[] common_unwanted_directories = [".git", ".vs", ".idea", "bin", "obj"];

        void IFileDiffTypeProvider.GetFileDiffType(string filePath, ref FileDiffType? diffType, ref bool? ignore)
        {
            // TODO: This could be faster if we just check whether the file path
            //       contains any of the common unwanted directories.  This is
            //       not an actual check, though.
            var fileDirs = filePath.Split(Path.DirectorySeparatorChar);
            ignore = common_unwanted_directories.Any(commonDir => fileDirs.Contains(commonDir));
        }
    }

    private sealed class HandleCommonFileTypesProvider : IFileDiffTypeProvider
    {
        private static readonly string[] common_textual_extensions = [".cs", ".csproj", ".resx", "App.config", ".json", ".targets", ".txt", ".bat", ".sh"];
        private static readonly string[] common_binary_extensions  = [".ico", ".png", ".dll"];

        void IFileDiffTypeProvider.GetFileDiffType(string filePath, ref FileDiffType? diffType, ref bool? ignore)
        {
            var extension = Path.GetExtension(filePath);
            var fileName  = Path.GetFileName(filePath);

            if (common_textual_extensions.Contains(extension) || common_textual_extensions.Contains(fileName))
            {
                diffType = FileDiffType.TextualDiff;
            }
            else if (common_binary_extensions.Contains(extension) || common_binary_extensions.Contains(fileName))
            {
                diffType = FileDiffType.BinaryDiff;
            }
        }
    }

    [PublicAPI]
    public static DifferSettings IgnoreCommonDirectories(this DifferSettings settings)
    {
        settings.AddCanDiffFileProvider(new IgnoreCommonDirectoriesProvider());
        return settings;
    }

    public static DifferSettings HandleCommonFileTypes(this DifferSettings settings)
    {
        settings.AddCanDiffFileProvider(new HandleCommonFileTypesProvider());
        return settings;
    }
}