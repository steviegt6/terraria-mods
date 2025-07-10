using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using Reaganism.CDC.Utilities;
using Reaganism.FBI.Textual.Fuzzy;
using Reaganism.FBI.Textual.Fuzzy.Diffing;
using Reaganism.FBI.Utilities;

namespace Reaganism.CDC.Diffing;

/// <summary>
///     Handles diffing a project.
/// </summary>
[PublicAPI]
public static class ProjectDiffer
{
    /// <summary>
    ///     Diffs the project as specified in the settings.
    /// </summary>
    /// <param name="settings">The project diff settings.</param>
    [PublicAPI]
    public static void Diff(DifferSettings settings)
    {
        var actions = new List<Action>();

        // Get files that exist in the modified directory and diff them against
        // files in the original directory.
        foreach (var (filePath, relativePath) in PathUtil.EnumerateSourceFiles(settings.ModifiedDirectory))
        {
            if (!File.Exists(Path.Combine(settings.OriginalDirectory, relativePath)))
            {
                // If the file doesn't exist in the original directory, it's
                // new; we can directly copy it to the output directory.
                actions.Add(
                    () =>
                    {
                        var destination = Path.Combine(settings.PatchesDirectory, relativePath);
                        PathUtil.CreateParentDirectory(destination);
                        File.Copy(filePath, destination);
                    }
                );
            }
            else
            {
                // If the file does exist then we need to perform an actual diff
                // on it and generate any applicable patch files.
                settings.GetFileDiffType(relativePath, out var fileDiffType, out var ignore);

                if (ignore)
                {
                    continue;
                }

                switch (fileDiffType)
                {
                    case FileDiffType.TextualDiff:
                        actions.Add(
                            () => DiffFile(settings, relativePath)
                        );
                        break;

                    case FileDiffType.BinaryDiff:
                        actions.Add(
                            () => DiffBinaryFile(settings, relativePath)
                        );
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        ParallelUtil.Execute(actions);

        // Clean up any patch files that are no longer needed.  That is, any
        // patch files that was generated for a file that no longer exists in
        // the current version of the modified directory.  This happens if the
        // modified directory once contained modifications to a file that has
        // since been reverted to its original version or has been deleted
        // outright.
        foreach (var (filePath, relativePath) in PathUtil.EnumerateFiles(settings.PatchesDirectory))
        {
            // Relative path without the ".patch" extension.
            var targetPath = relativePath.EndsWith(".patch") ? relativePath[..^6] : relativePath;

            if (!File.Exists(Path.Combine(settings.ModifiedDirectory, targetPath)) && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        PathUtil.DeleteEmptyDirectories(settings.ModifiedDirectory);

        // Determine what files have been deleted in the modified directory
        // (ones that existed in the source directory but do not exist in the
        // modified directory) and write them to a file.
        var removedFiles = PathUtil.EnumerateSourceFiles(settings.OriginalDirectory)
                                   .Where(x => !File.Exists(Path.Combine(settings.ModifiedDirectory, x.relativePath)))
                                   .Select(x => x.relativePath)
                                   .ToArray();

        var removedFileList = Path.Combine(settings.PatchesDirectory, DifferSettings.REMOVED_FILES_LIST_NAME);
        if (removedFiles.Length > 0)
        {
            File.WriteAllLines(removedFileList, removedFiles);
        }
        else
        {
            if (File.Exists(removedFileList))
            {
                File.Delete(removedFileList);
            }
        }
    }

    private static unsafe void DiffFile(DifferSettings settings, string relativePath)
    {
        // Is this size excessive?
        const long max_file_bytes_for_stack = 1024 * 200;

        Utf16String originalText;
        {
            var originalPath = Path.Combine(settings.OriginalDirectory, relativePath).Replace('\\', '/');
            var originalInfo = new FileInfo(originalPath);
            if (originalInfo.Length <= max_file_bytes_for_stack)
            {
                using var fs = new FileStream(originalPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                var pBytes = (Span<byte>)stackalloc byte[(int)originalInfo.Length];
                var pChars = (Span<char>)stackalloc char[(int)originalInfo.Length];
                _ = fs.Read(pBytes);
                Encoding.UTF8.GetChars(pBytes, pChars);

                originalText = Utf16String.FromSpan(pChars);
            }
            else
            {
                originalText = Utf16String.FromString(File.ReadAllText(originalPath));
            }
        }

        Utf16String modifiedText;
        {
            var modifiedPath = Path.Combine(settings.ModifiedDirectory, relativePath).Replace('\\', '/');
            var modifiedInfo = new FileInfo(modifiedPath);
            if (modifiedInfo.Length <= max_file_bytes_for_stack)
            {
                using var fs = new FileStream(modifiedPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                var pBytes = (Span<byte>)stackalloc byte[(int)modifiedInfo.Length];
                var pChars = (Span<char>)stackalloc char[(int)modifiedInfo.Length];
                _ = fs.Read(pBytes);
                Encoding.UTF8.GetChars(pBytes, pChars);

                modifiedText = Utf16String.FromSpan(pChars);
            }
            else
            {
                modifiedText = Utf16String.FromString(File.ReadAllText(modifiedPath));
            }
        }

        var patches = FuzzyDiffer.DiffTexts(
            new LineMatchedDiffer(),
            SplitText(originalText),
            SplitText(modifiedText)
        ).ToArray();

        var patchPath = Path.Combine(settings.PatchesDirectory, relativePath + ".patch");
        if (patches.Length != 0)
        {
            PathUtil.CreateParentDirectory(patchPath);
            File.WriteAllText(
                patchPath,
                new FuzzyPatchFile(
                    patches,
                    Utf16String.FromString(Path.Combine(settings.OriginalDirectory, relativePath).Replace('\\', '/')),
                    Utf16String.FromString(Path.Combine(settings.ModifiedDirectory, relativePath).Replace('\\', '/'))
                ).ToString(true)
            );
        }
        else
        {
            if (File.Exists(patchPath))
            {
                File.Delete(patchPath);
            }
        }
    }

    // Only checks whether files are the same and copies the binary file to the
    // patches directory if they aren't.  Does not generate any patch files such
    // as xdelta.
    private static void DiffBinaryFile(DifferSettings settings, string relativePath)
    {
        var originalFilePath = Path.Combine(settings.OriginalDirectory, relativePath);
        var modifiedFilePath = Path.Combine(settings.ModifiedDirectory, relativePath);

        var originalFileSize = new FileInfo(originalFilePath).Length;
        var modifiedFileSize = new FileInfo(modifiedFilePath).Length;

        // Quick check: we know they files aren't the same if their sizes are
        // different.
        if (originalFileSize != modifiedFileSize)
        {
            File.Copy(modifiedFilePath, Path.Combine(settings.PatchesDirectory, relativePath));
        }

        // Now we need to actually check whether their bytes are the same.
        var originalFileBytes = File.ReadAllBytes(originalFilePath);
        var modifiedFileBytes = File.ReadAllBytes(modifiedFilePath);

        if (!originalFileBytes.SequenceEqual(modifiedFileBytes))
        {
            File.Copy(modifiedFilePath, Path.Combine(settings.PatchesDirectory, relativePath));
        }
    }

    private static unsafe List<Utf16String> SplitText(Utf16String text)
    {
        var span = text.Span;

        var lineCount = 0;
        for (var i = 0; i < text.Length; i++)
        {
            if (span[i] == '\n')
            {
                lineCount++;
            }
        }

        var ranges = (Span<Range>)stackalloc Range[lineCount];
        if (span.Split(ranges, '\n') != ranges.Length)
        {
            throw new Exception("Line count mismatch");
        }

        var result = new List<Utf16String>(ranges.Length);

        for (var i = 0; i < ranges.Length; i++)
        {
            var (start, length) = ranges[i].GetOffsetAndLength(text.Length);

            if (span[start + length - 1] == '\r')
            {
                length--;
            }
            else if (start + length > 2 && span[start + length - 2] == '\r' && span[start + length - 1] == '\n')
            {
                length -= 2;
            }

            result.Add(text.Slice(start, length));
        }

        return result;
    }
}