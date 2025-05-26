using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Build.Shared;

public static partial class CPreprocessor
{
    private static readonly Regex include_regex = IncludeRegex();

    public static bool IsOutOfDate(string compilationUnitPath, string binaryPath)
    {
        if (!File.Exists(binaryPath))
        {
            return true;
        }

        var compilationUnitTime = GetLatestWriteTime(compilationUnitPath);
        var binaryWriteTime = File.GetLastWriteTimeUtc(binaryPath);
        return compilationUnitTime > binaryWriteTime;
    }

    private static DateTime GetLatestWriteTime(string filePath)
    {
        return GetLatestWriteTimeRecursive(Path.GetFullPath(filePath), []);
    }

    private static DateTime GetLatestWriteTimeRecursive(string filePath, HashSet<string> visitedFiles)
    {
        if (visitedFiles.Contains(filePath))
        {
            return DateTime.MinValue;
        }

        if (!File.Exists(filePath))
        {
            return DateTime.MinValue;
        }

        visitedFiles.Add(filePath);

        var latest = File.GetLastWriteTimeUtc(filePath);

        var lines = File.ReadAllLines(filePath);
        var baseDir = Path.GetDirectoryName(filePath) ?? string.Empty;

        foreach (var line in lines)
        {
            var match = include_regex.Match(line);
            if (!match.Success)
            {
                continue;
            }

            var includePath = match.Groups[1].Value;
            var fullIncludePath = Path.Combine(baseDir, includePath);

            var includeWriteTime = GetLatestWriteTimeRecursive(fullIncludePath, visitedFiles);
            if (includeWriteTime > latest)
            {
                latest = includeWriteTime;
            }
        }

        return latest;
    }

    [GeneratedRegex(
        """
        ^\s*#\s*include\s*"([^"]+)"
        """
    )]
    private static partial Regex IncludeRegex();
}