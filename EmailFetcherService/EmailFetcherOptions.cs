namespace EmailFetcherService;

/// <summary>
/// Non-sensitive settings unique to EmailFetcherService.
/// </summary>
public class EmailFetcherOptions
{
    public const string SectionName = "EmailFetcher";

    public int PollIntervalSeconds { get; set; } = 60;

    public string ImapHost { get; set; } = "imap.gmail.com";

    public int ImapPort { get; set; } = 993;
}

/// <summary>
/// Sensitive settings for EmailFetcherService.
/// Never committed — sourced from User Secrets locally, environment
/// variables when deployed.
/// </summary>
public class EmailFetcherCredentials
{
    public const string SectionName = "EmailFetcherCredentials";

    public string EmailAddress { get; set; } = string.Empty;

    public string AppPassword { get; set; } = string.Empty;

    /// <summary>
    /// Whitelist of sender addresses whose attachments will be processed.
    /// Empty list = process nothing (fail safe). Doubles as the mechanism
    /// that excludes unrelated mail (e.g. scanner notifications) from this
    /// inbox, since anything not explicitly listed is simply ignored.
    /// </summary>
    public string[] AllowedSenders { get; set; } = [];

    public string AttachmentDropDirectory { get; set; } = string.Empty;

    public string AttachmentDoneDirectory { get; set; } = string.Empty;
}