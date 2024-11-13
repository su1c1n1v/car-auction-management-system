using CarAuctionManagementSystem.API.Vehicles.DataTransferObject;
using CarAuctionManagementSystem.API.Vehicles.Interfaces;
using CarAuctionManagementSystem.API.Vehicles.Models;

namespace CarAuctionManagementSystem.API.Vehicles;

public class VehicleService(
    IVehicleRepository vehicleRepository,
    ILogger<VehicleService> logger
) : IVehicleService
{
    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        logger.LogInformation("Service: Getting all vehicles");

        var result = await vehicleRepository.GetAllAsync();

        logger.LogInformation("Service: Returning all vehicles");
        return result;
    }

    public async Task<IEnumerable<Vehicle>> SearchAsync(string? manufacturer, string? model, int? year)
    {
        logger.LogInformation("Service: Searching vehicles");
        var allVehicles = await vehicleRepository.GetAllAsync();

        logger.LogDebug("Service: Filtering vehicles Manufacturer: {Manufacturer}, Model: {Model}, Year: {Year}", manufacturer,
            model, year);

        var result = allVehicles
            .Where(vehicle => year is null || vehicle.Year == year)
            .Where(vehicle => model is null || vehicle.Model == model)
            .Where(vehicle => manufacturer is null || vehicle.Manufacturer == manufacturer);

        logger.LogInformation("Service: Returning vehicles");
        return result;
    }

    public async Task<Vehicle?> GetByIdAsync(string id)
    {
        logger.LogInformation("Service: Getting vehicle by id");

        var result = await vehicleRepository.GetByIdAsync(id);

        logger.LogInformation("Service: Returning vehicle by id");
        return result;
    }

    public async Task<Vehicle> AddAsync(CreateVehicleDto vehicle)
    {
        logger.LogInformation("Service: Creating vehicle");

        var result = await vehicleRepository.AddAsync(VehicleConvert(vehicle));

        logger.LogInformation("Service: Returning created vehicle");
        return result;
    }

    private static Vehicle VehicleConvert(CreateVehicleDto vehicle) => vehicle.Type switch
    {
        VehicleEnumType.Hatchback => new Hatchback
        {
            Manufacturer = vehicle.Manufacturer,
            Model = vehicle.Model,
            Year = vehicle.Year,
            StartingBid = vehicle.StartingBid,
            NumberDoors = vehicle.NumberDoors!.Value
        },
        VehicleEnumType.Sedan => new Sedan
        {
            Manufacturer = vehicle.Manufacturer,
            Model = vehicle.Model,
            Year = vehicle.Year,
            StartingBid = vehicle.StartingBid,
            NumberDoors = vehicle.NumberDoors!.Value
        },
        VehicleEnumType.SUV => new SUV
        {
            Manufacturer = vehicle.Manufacturer,
            Model = vehicle.Model,
            Year = vehicle.Year,
            StartingBid = vehicle.StartingBid,
            NumberSeats = vehicle.NumberSeats!.Value
        },
        VehicleEnumType.Truck => new Truck
        {
            Manufacturer = vehicle.Manufacturer,
            Model = vehicle.Model,
            Year = vehicle.Year,
            StartingBid = vehicle.StartingBid,
            LoadCapacity = vehicle.LoadCapacity!.Value
        },
        _ => throw new BadHttpRequestException("Invalid vehicle type. Please provide a valid vehicle type.")
    };
}