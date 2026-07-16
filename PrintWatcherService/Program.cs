using PrintBridge.Common;
using PrintWatcherService;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Logging.AddEventLog(settings =>
{
    settings.SourceName = "PrintWatcherService";
});

builder.Services.Configure<PrinterOptions>(
    builder.Configuration.GetSection(PrinterOptions.SectionName));
builder.Services.Configure<PrintWatcherOptions>(
    builder.Configuration.GetSection(PrintWatcherOptions.SectionName));
builder.Services.Configure<PrintWatcherPaths>(
    builder.Configuration.GetSection(PrintWatcherPaths.SectionName));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();