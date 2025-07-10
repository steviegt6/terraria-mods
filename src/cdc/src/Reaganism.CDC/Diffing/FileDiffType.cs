using JetBrains.Annotations;

namespace Reaganism.CDC.Diffing;

/// <summary>
///     The type of diff to perform on a file.
/// </summary>
[PublicAPI]
public enum FileDiffType
{
    /// <summary>
    ///     Perform a textual diff on the file.
    /// </summary>
    [PublicAPI]
    TextualDiff,

    /// <summary>
    ///     Perform a binary diff on the file.
    /// </summary>
    /// <remarks>
    ///     Currently this just checks whether files are identical and writes
    ///     the new file to the patches directory if they are not.
    /// </remarks>
    [PublicAPI]
    BinaryDiff,
}