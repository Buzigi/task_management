using Serilog;

namespace TaskMAPI.Services.Configurations;

public static class SerilogConfigExtensions
{
    public static void SerilogServices(this IServiceCollection services, IConfiguration config)
    {
        var serilogLogger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .Enrich.FromLogContext()
            .CreateLogger();

        _ = services.AddSingleton(services =>
        {
            var factory = LoggerFactory.Create(logging =>
            {
                logging.AddSerilog(serilogLogger, dispose: true);
            });
            return factory;
        });

        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    }
}
