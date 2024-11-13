using CarAuctionManagementSystem.API.Vehicles.Interfaces;
using CarAuctionManagementSystem.API.Vehicles.Models;

namespace CarAuctionManagementSystem.API.Vehicles;

public class VehicleRepository : IVehicleRepository
{
    private List<Vehicle> _vehicles =
    [
        new Hatchback
        {
            NumberDoors = (sbyte)Random.Shared.Next(2, 4),
            Year = 2019,
            Model = "Corolla",
            Manufacturer = "Toyota",
            StartingBid = Random.Shared.Next(3000, 10000)
        },
        new Sedan
        {
            NumberDoors = (sbyte)Random.Shared.Next(2, 4),
            Year = 2018,
            Model = "Civic",
            Manufacturer = "Honda",
            StartingBid = Random.Shared.Next(3000, 10000)
        },
        new SUV
        {
            NumberSeats = (sbyte)Random.Shared.Next(5, 9),
            Year = 2017,
            Model = "Explorer",
            Manufacturer = "Ford",
            StartingBid = Random.Shared.Next(3000, 10000)
        },
        new Truck
        {
            Year = 2016,
            Model = "Silverado",
            LoadCapacity = Random.Shared.Next(100, 1300),
            Manufacturer = "Chevrolet",
            StartingBid = Random.Shared.Next(3000, 10000)
        }
    ];


    public Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return Task.FromResult(_vehicles.AsEnumerable());
    }

    public Task<Vehicle?> GetByIdAsync(string id)
    {
        return Task.FromResult(_vehicles.FirstOrDefault(v => v.Id.ToString() == id));
    }
    
    public Task<IEnumerable<Vehicle>> GetByManufacturerAsync(string manufacturer)
    {
        return Task.FromResult(_vehicles.Where(v => v.Manufacturer == manufacturer).AsEnumerable());
    }
    
    public Task<IEnumerable<Vehicle>> GetByYearAsync(int year)
    {
        return Task.FromResult(_vehicles.Where(v => v.Year == year).AsEnumerable());
    }
    
    public Task<IEnumerable<Vehicle>> GetByModelAsync(string model)
    {
        return Task.FromResult(_vehicles.Where(v => v.Model == model).AsEnumerable());
    }
    
    public Task<IEnumerable<Vehicle>> GetByTypeAsync(string type)
    {
        return Task.FromResult(_vehicles.Where(v => v.GetType().Name == type).AsEnumerable());
    }

    public Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        return Task.Run(() =>
        {
            _vehicles.Add(vehicle);
            return vehicle;
        });
    }

    public Task<Vehicle> UpdateAsync(Vehicle vehicle)
    {
        var existingVehicle = _vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
        if (existingVehicle == null)
        {
            return Task.FromResult<Vehicle>(null);
        }

        _vehicles.Remove(existingVehicle);
        _vehicles.Add(vehicle);
        return Task.FromResult(vehicle);
    }
}