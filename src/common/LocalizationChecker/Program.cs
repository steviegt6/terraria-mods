using System;
using System.IO;

using Hjson;

namespace LocalizationChecker;

internal static class Program
{
    public static void Main(string[] args)
    {
        foreach (var file in args)
        {
            CheckFile(file);
        }
    }

    private static void CheckFile(string file)
    {
        try
        {
            HjsonValue.Load(Path.GetFullPath(file));
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Failed to validate HJSON file with error: " + e);
            Environment.ExitCode = 1;
        }
    }
}