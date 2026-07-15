namespace PrintBridge.Common;

/// <summary>
/// Non-sensitive settings unique to PrintWatcherService (not shared with
/// EmailFetcherService).
/// </summary>
public class PrintWatcherOptions
{
    public const string SectionName = "PrintWatcher";

    public int PollIntervalSeconds { get; set; } = 30;
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