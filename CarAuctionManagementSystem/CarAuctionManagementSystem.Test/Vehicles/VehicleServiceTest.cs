using CarAuctionManagementSystem.API.Vehicles;
using CarAuctionManagementSystem.API.Vehicles.DataTransferObject;
using CarAuctionManagementSystem.API.Vehicles.Interfaces;
using CarAuctionManagementSystem.API.Vehicles.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarAuctionManagementSystem.Test.Vehicles;

public class VehicleServiceTest
{
    [Fact]
    public async void GetAllAsync_ShouldReturnAllVehicles()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicles = new List<Vehicle>
        {
            new Truck { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 },
            new SUV { Manufacturer = "Toyota", Model = "Camry", Year = 2015 },
            new Hatchback { Manufacturer = "Honda", Model = "Civic", Year = 2018 }
        };

        vehicleRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(vehicles);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Equal(vehicles, result);
    }

    [Fact]
    public async void SearchAsync_ShouldReturnFilteredVehicles()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicles = new List<Vehicle>
        {
            new Truck { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 },
            new SUV { Manufacturer = "Toyota", Model = "Camry", Year = 2015 },
            new Hatchback { Manufacturer = "Honda", Model = "Civic", Year = 2018 }
        };

        vehicleRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(vehicles);

        // Act
        var result = await service.SearchAsync("Toyota", "Corolla", 2010);

        // Assert
        Assert.Single(result);
        Assert.Equal("Toyota", result.First().Manufacturer);
        Assert.Equal("Corolla", result.First().Model);
        Assert.Equal(2010, result.First().Year);
    }

    [Fact]
    public async void GetByIdAsync_ShouldReturnVehicleById()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicle = new Truck { Id = Guid.NewGuid(), Manufacturer = "Toyota", Model = "Corolla", Year = 2010 };

        vehicleRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(vehicle);

        // Act
        var result = await service.GetByIdAsync(vehicle.Id.ToString());

        // Assert
        Assert.Equal(vehicle, result);
    }

    [Fact]
    public async void AddAsync_ShouldReturnTruck()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicle = new CreateVehicleDto
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010,
            LoadCapacity = 1000,
            Type = VehicleEnumType.Truck
        };

        var createdVehicle = new Truck { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 };

        vehicleRepository.Setup(x => x.AddAsync(It.IsAny<Vehicle>())).ReturnsAsync(createdVehicle);

        // Act
        var result = await service.AddAsync(vehicle);

        // Assert
        Assert.Equal(createdVehicle, result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async void AddAsync_ShouldReturnSedan()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicle = new CreateVehicleDto
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010,
            NumberDoors = 4,
            Type = VehicleEnumType.Sedan
        };

        var createdVehicle = new Sedan { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 };

        vehicleRepository.Setup(x => x.AddAsync(It.IsAny<Vehicle>())).ReturnsAsync(createdVehicle);

        // Act
        var result = await service.AddAsync(vehicle);

        // Assert
        Assert.Equal(createdVehicle, result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async void AddAsync_ShouldReturnSUV()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicle = new CreateVehicleDto
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010,
            NumberSeats = 4,
            Type = VehicleEnumType.SUV
        };

        var createdVehicle = new SUV { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 };

        vehicleRepository.Setup(x => x.AddAsync(It.IsAny<Vehicle>())).ReturnsAsync(createdVehicle);

        // Act
        var result = await service.AddAsync(vehicle);

        // Assert
        Assert.Equal(createdVehicle, result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async void AddAsync_ShouldReturnHatchback()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicle = new CreateVehicleDto
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010,
            NumberDoors = 4,
            Type = VehicleEnumType.Hatchback
        };

        var createdVehicle = new Hatchback { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 };

        vehicleRepository.Setup(x => x.AddAsync(It.IsAny<Vehicle>())).ReturnsAsync(createdVehicle);

        // Act
        var result = await service.AddAsync(vehicle);

        // Assert
        Assert.Equal(createdVehicle, result);
        Assert.NotNull(result);
    }

    [Fact]
    public async void AddAsync_ShouldThrowException_WhenVehicleTypeIsInvalid()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var logger = new Mock<ILogger<VehicleService>>();
        var service = new VehicleService(vehicleRepository.Object, logger.Object);

        var vehicle = new CreateVehicleDto
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010,
            LoadCapacity = 1000,
            Type = (VehicleEnumType)10
        };

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(() => service.AddAsync(vehicle));
    }
}