using Serilog;

namespace CarAuctionManagementSystem.API.Logging;

public static class ConfigureServices
{
    public static WebApplicationBuilder AddLoggingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

        return builder;
    }
}