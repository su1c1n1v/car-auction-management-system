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
            Type = new()
            {
                Id = VehicleEnumType.Hatchback
            },
            StartingBid = Random.Shared.Next(3000, 10000)
        },
        new Hatchback
        {
            NumberDoors = (sbyte)Random.Shared.Next(2, 4),
            Year = 2024,
            Model = "Corolla",
            Manufacturer = "Toyota",
            Type = new()
            {
                Id = VehicleEnumType.Hatchback
            },
            StartingBid = Random.Shared.Next(7000, 12000)
        },
        new Hatchback
        {
            NumberDoors = (sbyte)Random.Shared.Next(2, 4),
            Year = 2010,
            Model = "Corolla",
            Manufacturer = "Toyota",
            Type = new()
            {
                Id = VehicleEnumType.Hatchback
            },
            StartingBid = Random.Shared.Next(3000, 6000)
        },
        new Sedan
        {
            NumberDoors = (sbyte)Random.Shared.Next(2, 4),
            Year = 2018,
            Model = "Civic",
            Manufacturer = "Honda",
            Type = new()
            {
                Id = VehicleEnumType.Sedan
            },
            StartingBid = Random.Shared.Next(3000, 10000)
        },
        new Sedan
        {
            NumberDoors = (sbyte)Random.Shared.Next(2, 4),
            Year = 2018,
            Model = "Camry",
            Manufacturer = "Toyota",
            Type = new()
            {
                Id = VehicleEnumType.Sedan
            },
            StartingBid = Random.Shared.Next(3000, 10000)
        },
        new SUV
        {
            NumberSeats = (sbyte)Random.Shared.Next(5, 40),
            Year = 2017,
            Model = "Explorer",
            Manufacturer = "Ford",
            Type = new()
            {
                Id = VehicleEnumType.SUV
            },
            StartingBid = Random.Shared.Next(3000, 10000)
        },
        new Truck
        {
            Year = 2016,
            Model = "Silverado",
            Type = new()
            {
                Id = VehicleEnumType.Truck
            },
            LoadCapacity = Random.Shared.Next(100, 74000),
            Manufacturer = "Chevrolet",
            StartingBid = Random.Shared.Next(3000, 10000),
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
    
    public Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        return Task.Run(() =>
        {
            _vehicles.Add(vehicle);
            return vehicle;
        });
    }
}