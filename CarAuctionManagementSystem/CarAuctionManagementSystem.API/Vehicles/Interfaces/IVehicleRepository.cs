using CarAuctionManagementSystem.API.Vehicles.Models;

namespace CarAuctionManagementSystem.API.Vehicles.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(string id);
    Task<IEnumerable<Vehicle>> GetByManufacturerAsync(string manufacturer);
    Task<IEnumerable<Vehicle>> GetByYearAsync(int year);
    Task<IEnumerable<Vehicle>> GetByModelAsync(string model);
    Task<IEnumerable<Vehicle>> GetByTypeAsync(string type);
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task<Vehicle> UpdateAsync(Vehicle vehicle);
}