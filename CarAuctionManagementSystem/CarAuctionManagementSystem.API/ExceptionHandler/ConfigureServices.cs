namespace CarAuctionManagementSystem.API.ExceptionHandler;

public static class ConfigureServices
{
    public static IApplicationBuilder AddExceptionHandleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandleMiddleware>();
    }
}
