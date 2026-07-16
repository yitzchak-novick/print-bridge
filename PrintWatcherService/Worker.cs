using Microsoft.Extensions.Options;
using PrintBridge.Common;

namespace PrintWatcherService;

public class Worker(
    ILogger<Worker> logger,
    IOptions<PrintWatcherPaths> paths,
    IOptions<PrintWatcherOptions> options,
    IOptions<PrinterOptions> printerOptions) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var watchPaths = paths.Value;
        var watcherOptions = options.Value;
        var printer = printerOptions.Value;

        if (!ValidateStartupPaths(watchPaths))
        {
            return;
        }

        logger.LogInformation(
            "PrintWatcherService starting. Watching: {dir}, poll interval: {interval}s, allowed extensions: {extensions}",
            watchPaths.WatchDirectory,
            watcherOptions.PollIntervalSeconds,
            string.Join(", ", printer.AllowedExtensions));

        var consecutiveFailures = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var currentInterval = TimeSpan.FromSeconds(watcherOptions.PollIntervalSeconds);

            try
            {
                var files = Directory.GetFiles(watchPaths.WatchDirectory);

                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file);
                    var isAllowed = printer.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);

                    logger.LogInformation(
                        "Found file: {file} (extension: {ext}, allowed: {allowed})",
                        Path.GetFileName(file),
                        extension,
                        isAllowed);
                }

                if (files.Length == 0)
                {
                    logger.LogInformation("No files found in watch directory.");
                }

                consecutiveFailures = 0;
            }
            catch (Exception ex)
            {
                consecutiveFailures++;
                logger.LogError(
                    ex,
                    "Error scanning watch directory (consecutive failure #{count}).",
                    consecutiveFailures);

                if (consecutiveFailures >= watcherOptions.MaxConsecutiveFailuresBeforeBackoff)
                {
                    currentInterval = TimeSpan.FromSeconds(watcherOptions.BackoffIntervalSeconds);
                    logger.LogWarning(
                        "Reached {count} consecutive failures. Backing off to {interval}s between checks.",
                        consecutiveFailures,
                        watcherOptions.BackoffIntervalSeconds);
                }
            }

            await Task.Delay(currentInterval, stoppingToken);
        }
    }

    private bool ValidateStartupPaths(PrintWatcherPaths watchPaths)
    {
        var missing = new List<string>();

        if (string.IsNullOrWhiteSpace(watchPaths.WatchDirectory) || !Directory.Exists(watchPaths.WatchDirectory))
            missing.Add($"WatchDirectory ('{watchPaths.WatchDirectory}')");

        if (string.IsNullOrWhiteSpace(watchPaths.DoneDirectory) || !Directory.Exists(watchPaths.DoneDirectory))
            missing.Add($"DoneDirectory ('{watchPaths.DoneDirectory}')");

        if (string.IsNullOrWhiteSpace(watchPaths.FailedDirectory) || !Directory.Exists(watchPaths.FailedDirectory))
            missing.Add($"FailedDirectory ('{watchPaths.FailedDirectory}')");

        if (missing.Count == 0)
        {
            return true;
        }

        logger.LogError(
            "One or more required directories are missing or not configured: {missing}. " +
            "Set these via User Secrets or environment variables. Service will not start.",
            string.Join("; ", missing));

        return false;
    }
}