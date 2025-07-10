using JetBrains.Annotations;

namespace Reaganism.CDC.Diffing;

/// <summary>
///     Determines whether a file is eligible to be diffed.
/// </summary>
[PublicAPI]
public interface IFileDiffTypeProvider
{
    /// <summary>
    ///     Determines whether the file at the given path is diffable and how it
    ///     should be diffed.
    /// </summary>
    /// <param name="filePath">The path of the file.</param>
    /// <param name="diffType">
    ///     The diff type, which may have not yet been determined.
    /// </param>
    /// <param name="ignore">Whether this file should be ignored.</param>
    [PublicAPI]
    void GetFileDiffType(string filePath, ref FileDiffType? diffType, ref bool? ignore);
}