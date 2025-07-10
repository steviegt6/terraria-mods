using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

using Reaganism.CDC.Diffing;
using Reaganism.CDC.Utilities;
using Reaganism.FBI.Textual.Fuzzy;
using Reaganism.FBI.Textual.Fuzzy.Patching;

namespace Reaganism.CDC.Patching;

/// <summary>
///     Handles patching a project.
/// </summary>
[PublicAPI]
public static class ProjectPatcher
{
    internal sealed class PatcherState
    {
        private readonly ConcurrentBag<FilePatcher> patchResults = [];

        internal IEnumerable<FilePatcher> GetResults()
        {
            return patchResults;
        }

        public void AddPatchResult(FilePatcher patcher)
        {
            patchResults.Add(patcher);
        }

        public CompiledPatchState Compile()
        {
            return new CompiledPatchState(this);
        }
    }

    /// <summary>
    ///     The finalized report of a patch operation.
    /// </summary>
    [PublicAPI]
    public readonly struct CompiledPatchState
    {
        /// <summary>
        ///     The number of successful patches.
        /// </summary>
        [PublicAPI]
        public int Successes { [PublicAPI] get; }

        /// <summary>
        ///     The number of failed patches.
        /// </summary>
        [PublicAPI]
        public int Failures { [PublicAPI] get; }

        /// <summary>
        ///     The number of patches that resulted in an offset warning.
        /// </summary>
        [PublicAPI]
        public int Warnings { [PublicAPI] get; }

        /// <summary>
        ///     The number of exact patches.
        /// </summary>
        [PublicAPI]
        public int Exacts { [PublicAPI] get; }

        /// <summary>
        ///     The number of offset patches.
        /// </summary>
        [PublicAPI]
        public int Offsets { [PublicAPI] get; }

        /// <summary>
        ///     The number of fuzzy patches.
        /// </summary>
        [PublicAPI]
        public int Fuzzies { [PublicAPI] get; }

        /// <summary>
        ///     The results of each patch.
        /// </summary>
        [PublicAPI]
        public PatchResult[] Results { [PublicAPI] get; }

        internal CompiledPatchState(PatcherState state)
        {
            var patchResults = state.GetResults().ToList();
            var allResults   = patchResults.SelectMany(x => x.Results).ToList();
            {
                Successes = allResults.Count(x => x.Success);
                Failures  = allResults.Count(x => !x.Success);
                Warnings  = allResults.Count(x => x.OffsetWarning);
                Exacts    = allResults.Count(x => x.Mode == Patcher.Mode.Exact);
                Offsets   = allResults.Count(x => x.Mode == Patcher.Mode.Offset);
                Fuzzies   = allResults.Count(x => x.Mode == Patcher.Mode.Fuzzy);
            }

            Results = patchResults.Select(x => new PatchResult(x)).ToArray();
        }
    }

    [PublicAPI]
    public readonly struct PatchResult
    {
        [PublicAPI]
        public readonly struct SingleResult
        {
            [PublicAPI]
            public string Header { [PublicAPI] get; }

            [PublicAPI]
            public string Summary { [PublicAPI] get; }

            internal SingleResult(Patcher.Result result)
            {
                Header  = FuzzyPatchHeader.GetHeader(result.Patch, false);
                Summary = result.Summary();
            }
        }

        [PublicAPI]
        public string OriginalPath { [PublicAPI] get; }

        [PublicAPI]
        public string ModifiedPath { [PublicAPI] get; }

        [PublicAPI]
        public SingleResult[] Results { [PublicAPI] get; }

        internal PatchResult(FilePatcher patchResult)
        {
            OriginalPath = patchResult.OriginalPath;
            ModifiedPath = patchResult.ModifiedPath;
            Results      = patchResult.Results.Select(x => new SingleResult(x)).ToArray();
        }
    }

    /// <summary>
    ///     Patches the project as specified in the settings.
    /// </summary>
    /// <param name="settings">The project patch settings.</param>
    /// <returns>A report of the finished patch operation.</returns>
    public static CompiledPatchState Patch(PatcherSettings settings)
    {
        var state = new PatcherState();

        var removedFilesList = Path.Combine(settings.PatchesDirectory, DifferSettings.REMOVED_FILES_LIST_NAME);

        var noCopy   = File.Exists(removedFilesList) ? new HashSet<string>(File.ReadAllLines(removedFilesList)) : [];
        var newFiles = new HashSet<string>();

        var patchActions     = new List<Action>();
        var patchCopyActions = new List<Action>();
        var copyActions      = new List<Action>();

        foreach (var (filePath, relativePath) in PathUtil.EnumerateFiles(settings.PatchesDirectory))
        {
            if (relativePath.EndsWith(".patch"))
            {
                // This is a patch.
                patchActions.Add(
                    () =>
                    {
                        var patcher = PatchFile(settings, filePath, state);
                        newFiles.Add(Path.GetFullPath(PathUtil.NormalizePath(patcher.ModifiedPath)));
                    }
                );

                noCopy.Add(relativePath[..^6]);
            }
            else if (relativePath != removedFilesList)
            {
                // This is copied file (whether it be new or a modified binary
                // file).  Excludes our metadata removed_files.list file.
                var destination = Path.GetFullPath(Path.Combine(settings.ModifiedDirectory, relativePath));
                PathUtil.CreateParentDirectory(destination);

                patchCopyActions.Add(() => File.Copy(filePath, destination));
                newFiles.Add(destination);
            }
        }

        foreach (var (filePath, relativePath) in PathUtil.EnumerateSourceFiles(settings.OriginalDirectory))
        {
            if (noCopy.Contains(relativePath))
            {
                continue;
            }

            var destination = Path.GetFullPath(Path.Combine(settings.ModifiedDirectory, relativePath));
            PathUtil.CreateParentDirectory(destination);
            copyActions.Add(() => File.Copy(filePath, destination, true));
            newFiles.Add(destination);
        }

        ParallelUtil.Execute(patchActions);
        ParallelUtil.Execute(patchCopyActions);
        ParallelUtil.Execute(copyActions);

        foreach (var (filePath, _) in PathUtil.EnumerateSourceFiles(settings.ModifiedDirectory))
        {
            if (!newFiles.Contains(Path.GetFullPath(filePath)) && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        PathUtil.DeleteEmptyDirectories(settings.ModifiedDirectory);

        return state.Compile();
    }

    private static FilePatcher PatchFile(PatcherSettings settings, string patchFilePath, PatcherState state)
    {
        var patcher = FilePatcher.FromPatchFile(patchFilePath);
        state.AddPatchResult(patcher);
        {
            patcher.Patch(settings.Mode);
            PathUtil.CreateParentDirectory(patcher.ModifiedPath);
            patcher.Save();
        }

        return patcher;
    }
}