using CarAuctionManagementSystem.API.Vehicles.DataTransferObject;
using CarAuctionManagementSystem.API.Vehicles.Interfaces;
using CarAuctionManagementSystem.API.Vehicles.Validation;
using FluentValidation;

namespace CarAuctionManagementSystem.API.Vehicles;

public static class ConfigureServices
{
    public static IServiceCollection AddVehicleServices(this IServiceCollection services)
    {
        services.AddSingleton<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleService, VehicleService>();
        
        services.AddScoped<IValidator<CreateVehicleDto>, VehicleValidation>();
        
        return services;
    }
}