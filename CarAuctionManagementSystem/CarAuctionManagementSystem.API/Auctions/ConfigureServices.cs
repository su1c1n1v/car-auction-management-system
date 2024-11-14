using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using CarAuctionManagementSystem.API.Auctions.Interfaces;
using CarAuctionManagementSystem.API.Auctions.Validation;
using FluentValidation;

namespace CarAuctionManagementSystem.API.Auctions;

public static class ConfigureServices
{
    public static IServiceCollection AddAuctionServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IAuctionService, AuctionService>();

        services.AddScoped<IValidator<CreateAuctionDto>, AuctionValidation>();
        services.AddScoped<IValidator<CreateBidDto>, BidValidation>();

        return services;
    }
}