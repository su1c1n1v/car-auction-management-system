using CarAuctionManagementSystem.API.Vehicles.DataTransferObject;
using CarAuctionManagementSystem.API.Vehicles.Models;

namespace CarAuctionManagementSystem.API.Vehicles.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(string id);
    Task<Vehicle> AddAsync(CreateVehicleDto vehicle);
    Task<IEnumerable<Vehicle>> SearchAsync(string? manufacturer, string? model, int? year);
}