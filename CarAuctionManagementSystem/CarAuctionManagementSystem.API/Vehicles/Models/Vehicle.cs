namespace CarAuctionManagementSystem.API.Vehicles.Models;

public abstract class Vehicle
{
    public Guid Id { get; init; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public VehicleType Type { get; init; }
    public short Year { get; set; }
    public double StartingBid { get; set; }
}

public enum VehicleType
{
    Hatchback,
    Sedan,
    SUV,
    Truck
}

public sealed class Truck : Vehicle
{
    public Truck()
    {
        Type = VehicleType.Truck;
    }

    public double LoadCapacity { get; set; }
}

public sealed class SUV : Vehicle
{
    public SUV()
    {
        Type = VehicleType.SUV;
    }

    public sbyte NumberSeats { get; set; }
}

public sealed class Sedan : Vehicle
{
    public Sedan()
    {
        Type = VehicleType.Sedan;
    }

    public sbyte NumberDoors { get; set; }
}

public sealed class Hatchback : Vehicle
{
    public Hatchback()
    {
        Type = VehicleType.Hatchback;
    }

    public sbyte NumberDoors { get; set; }
}