using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Reaganism.FBI.Textual.Fuzzy;
using Reaganism.FBI.Textual.Fuzzy.Patching;

namespace Reaganism.CDC.Patching;

internal sealed class FilePatcher(FuzzyPatchFile patchFile, string rootDir)
{
    public string OriginalPath => Path.Combine(rootDir, patchFile.OriginalPath?.Span.ToString() ?? throw new InvalidOperationException());

    public string ModifiedPath => Path.Combine(rootDir, patchFile.ModifiedPath?.Span.ToString() ?? throw new InvalidOperationException());

    public List<Patcher.Result> Results { get; private set; } = [];

    private string[]? originalLines;
    private string[]? modifiedLines;

    public void Patch(Patcher.Mode mode)
    {
        originalLines ??= File.ReadAllLines(OriginalPath);

        var patcher = new Patcher(patchFile.Patches, originalLines);
        patcher.Patch(mode);
        Results       = patcher.Results.Where(x => x is not null).ToList()!;
        modifiedLines = patcher.ResultLines;
    }

    public void Save()
    {
        Debug.Assert(modifiedLines is not null);
        File.WriteAllLines(ModifiedPath, modifiedLines);
    }

    public static FilePatcher FromPatchFile(string patchFilePath, string rootDir = "")
    {
        return new FilePatcher(FuzzyPatchFile.FromText(File.ReadAllText(patchFilePath)), rootDir);
    }
}