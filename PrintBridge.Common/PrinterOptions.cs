namespace PrintBridge.Common;

/// <summary>
/// Shared, non-sensitive printing settings used by any service that prints
/// (PrintWatcherService and EmailFetcherService both reference this).
/// Safe to commit real values — nothing here reveals anything about this
/// machine or its owner.
/// </summary>
public class PrinterOptions
{
    public const string SectionName = "Printer";

    public int MaxRetries { get; set; } = 3;

    public int RetryBackoffSeconds { get; set; } = 10;

    public int PrintTimeoutSeconds { get; set; } = 60;

    /// <summary>
    /// Whitelist of file extensions eligible for printing, e.g. [".pdf", ".docx"].
    /// Empty list = print nothing (fail safe). A misconfigured or accidentally
    /// cleared list should never silently mean "print everything."
    /// </summary>
    public string[] AllowedExtensions { get; set; } = [".pdf"];
}
