using Serilog;

namespace CarAuctionManagementSystem.API.Logging;

public static class ConfigureServices
{
    public static IServiceCollection AddLoggingServices(this IServiceCollection services, ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();

        // Configure Serilog using settings from appsettings.json
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        // Register Serilog in the service collection
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

        return services;
    }
}