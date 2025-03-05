namespace Tomat.TML.Lib.RevisedConfiguration.API.IO;

/// <summary>
///     A config IO result.
/// </summary>
public enum Result
{
    /// <summary>
    ///     Completed without issues.
    /// </summary>
    Success,

    /// <summary>
    ///     There were errors that did not prevent completion.
    /// </summary>
    WarningHadErrors,
    
    /// <summary>
    ///     The config had no mod version.
    /// </summary>
    WarningModVersionMissing,

    /// <summary>
    ///     The config file is missing.
    /// </summary>
    ErrorFileMissing,
    
    /// <summary>
    ///     The config file is irreparably malformed.
    /// </summary>
    ErrorFileBroken,
    
    /// <summary>
    ///     The config file could not be accessed.
    /// </summary>
    ErrorFileInaccessible,
}