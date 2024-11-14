using CarAuctionManagementSystem.API.Vehicles.DataTransferObject;
using CarAuctionManagementSystem.API.Vehicles.Models;
using CarAuctionManagementSystem.API.Vehicles.Validation;

namespace CarAuctionManagementSystem.Test.Vehicles.Validation;

public class VehicleValidationTest
{
    [Theory]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Sedan, 2020, 10000, 4, null, null)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Hatchback, 2020, 10000, 4, null, null)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.SUV, 2020, 10000, null, 4, null)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Truck, 2020, 10000, null, null, 74000)]
    public void VehicleValidation_WhenVehicleIsValid(
        string manufacturer, string model, VehicleEnumType type, int year, double startingBid, int? numberDoors, int? numberSeats,
        int? loadCapacity)
    {
        // Arrange
        var vehicle = new CreateVehicleDto
        {
            Manufacturer = manufacturer,
            Model = model,
            Type = type,
            Year = year,
            StartingBid = startingBid,
            NumberDoors = numberDoors,
            LoadCapacity = loadCapacity,
            NumberSeats = numberSeats
        };

        var validator = new VehicleValidation();

        // Act
        var result = validator.Validate(vehicle);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    // Errors for Manufacturer
    [InlineData(null, "Corolla", VehicleEnumType.Sedan, 2020, 10000)]
    [InlineData("To", "Corolla", VehicleEnumType.Sedan, 2020, 10000)]
    [InlineData("ToyotaToyotaToyotaToyotaToyotaToyotaToyotaToyotaToyo", "Corolla", VehicleEnumType.Sedan, 2020, 10000)]
    // Errors for Model
    [InlineData("Toyota", null, VehicleEnumType.Sedan, 2020, 10000)]
    [InlineData("Toyota", "Co", VehicleEnumType.Sedan, 2020, 10000)]
    [InlineData("Toyota", "CorollaCorollaCorollaCorollaCorollaCorollaCorollaCo", VehicleEnumType.Sedan, 2020, 10000)]
    // Errors for Type
    [InlineData("Toyota", "Corolla", (VehicleEnumType)10, 2020, 10000)]
    // Errors for Year
    [InlineData("Toyota", "Corolla", VehicleEnumType.Sedan, 1930, 10000)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Sedan, 3000, 10000)]
    // Errors for StartingBid
    [InlineData("Toyota", "Corolla", VehicleEnumType.Sedan, 2020, 0)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Sedan, 2020, -1)]
    public void VehicleValidation_WhenVehicleIsInvalid(
        string? manufacturer, string? model, VehicleEnumType? type, int? year, int? startingBid)
    {
        // Arrange
        var vehicle = new CreateVehicleDto
        {
            Manufacturer = manufacturer!,
            Model = model!,
            Type = (VehicleEnumType)type!,
            Year = (int)year!,
            StartingBid = (double)startingBid!,
            NumberDoors = 4
        };

        var validator = new VehicleValidation();

        // Act
        var result = validator.Validate(vehicle);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    // Errors for NumberDoors
    [InlineData("Toyota", "Corolla", VehicleEnumType.Sedan, 2020, 10000, 0, null, null)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Hatchback, 2020, 10000, -1, null, null)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Hatchback, 2020, 10000, 6, null, null)]
    // Errors for NumberSeats
    [InlineData("Toyota", "Corolla", VehicleEnumType.SUV, 2020, 10000, null, 0, null)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.SUV, 2020, 10000, null, -1, null)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.SUV, 2020, 10000, null, 41, null)]
    // Errors for LoadCapacity
    [InlineData("Toyota", "Corolla", VehicleEnumType.Truck, 2020, 10000, null, null, 0)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Truck, 2020, 10000, null, null, -1)]
    [InlineData("Toyota", "Corolla", VehicleEnumType.Truck, 2020, 10000, null, null, 74001)]
    public void VehicleValidation_WhenVehicleTypeIsInvalid(
        string manufacturer, string model, VehicleEnumType type, int year, double startingBid, int? numberDoors, int? numberSeats,
        int? loadCapacity)
    {
        // Arrange
        var vehicle = new CreateVehicleDto
        {
            Manufacturer = manufacturer,
            Model = model,
            Type = type,
            Year = year,
            StartingBid = startingBid,
            NumberDoors = numberDoors,
            NumberSeats = numberSeats,
            LoadCapacity = loadCapacity
        };

        var validator = new VehicleValidation();

        // Act
        var result = validator.Validate(vehicle);

        // Assert
        Assert.False(result.IsValid);
    }
}