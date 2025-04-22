using System;
using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;

namespace TaskMAPI.Services.Configurations;

public static class SerilogConfigExtensions
{
    public static void SerilogServices(this IServiceCollection services, IConfiguration config)
    {
        var options = new CloudWatchSinkOptions
        {
            LogGroupName = "TaskM-API-Logs",
            TextFormatter = new Serilog.Formatting.Compact.RenderedCompactJsonFormatter(),
            MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
        };

        var client = new AmazonCloudWatchLogsClient();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .WriteTo.Console()
            .WriteTo.AmazonCloudWatch(options, client)
            .CreateLogger();
    }
}
