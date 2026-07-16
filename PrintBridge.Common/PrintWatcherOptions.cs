namespace PrintBridge.Common;

/// <summary>
/// Non-sensitive settings unique to PrintWatcherService (not shared with
/// EmailFetcherService).
/// </summary>
public class PrintWatcherOptions
{
    public const string SectionName = "PrintWatcher";

    public int PollIntervalSeconds { get; set; } = 30;

    /// <summary>
    /// After this many consecutive scan failures, back off to a longer
    /// interval instead of retrying every PollIntervalSeconds.
    /// </summary>
    public int MaxConsecutiveFailuresBeforeBackoff { get; set; } = 5;

    /// <summary>
    /// How long to wait between checks once in backoff mode.
    /// </summary>
    public int BackoffIntervalSeconds { get; set; } = 600;
}

/// <summary>
/// Machine-specific paths for PrintWatcherService only.
/// Never committed — sourced from User Secrets locally, environment
/// variables when deployed.
/// </summary>
public class PrintWatcherPaths
{
    public const string SectionName = "PrintWatcherPaths";

    public string WatchDirectory { get; set; } = string.Empty;

    public string DoneDirectory { get; set; } = string.Empty;

    public string FailedDirectory { get; set; } = string.Empty;
}