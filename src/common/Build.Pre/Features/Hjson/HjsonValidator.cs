using System;
using System.Text.RegularExpressions;

using Build.Shared;

using Hjson;

namespace Build.Pre.Features.Hjson;

internal sealed partial class HjsonValidator : BuildTask
{
    private static readonly Regex error_regex = LineColumnErrorMessageRegex();

    public override void Run(ProjectContext ctx)
    {
        foreach (var (relativePath, fullPath) in ctx.EnumerateProjectFiles())
        {
            if (!relativePath.StartsWith("Localization/") || !relativePath.EndsWith(".hjson"))
            {
                continue;
            }

            ValidateHjsonFile(fullPath);
        }
    }

    private static void ValidateHjsonFile(string filePath)
    {
        try
        {
            HjsonValue.Load(filePath);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(GetRichErrorMessage(filePath, e.Message));
            Environment.ExitCode = 1;
        }
    }

    private static string GetRichErrorMessage(string filePath, string message)
    {
        // extract line and column information from message
        var match = error_regex.Match(message);
        if (!match.Success)
        {
            return $"{filePath}: error HJSON: {message}";
        }

        var line = match.Groups[2].Value;
        var column = match.Groups[3].Value;
        return $"{filePath}({line},{column}): error HJSON: {match.Groups[1].Value}";
    }

    [GeneratedRegex(@"(.*?) At line (.*?), column (.*?) \((.*?)\)")]
    private static partial Regex LineColumnErrorMessageRegex();
}