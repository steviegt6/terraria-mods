namespace Tomat.TML.Lib.RevisedConfiguration.API.IO;

/// <summary>
///     The kind of file IO result.
/// </summary>
public enum ResultKind
{
    /// <summary>
    ///     Indicates that something was successful.
    /// </summary>
    Success,
    
    /// <summary>
    ///     Indicates that something went from but did not have to fail.
    /// </summary>
    Warning,
    
    /// <summary>
    ///     Indicates a failure.
    /// </summary>
    Error,
}