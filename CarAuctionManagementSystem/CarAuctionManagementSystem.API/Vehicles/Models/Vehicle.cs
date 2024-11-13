using System.Text.Json.Serialization;

namespace CarAuctionManagementSystem.API.Vehicles.Models;

[JsonDerivedType(typeof(Hatchback))]
[JsonDerivedType(typeof(Sedan))]
[JsonDerivedType(typeof(SUV))]
[JsonDerivedType(typeof(Truck))]
public abstract class Vehicle : AuditEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public virtual VehicleType Type { get; init; } = new();
    public int Year { get; set; }
    public double StartingBid { get; set; }
}

public abstract class AuditEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}

public class VehicleType
{
    public VehicleEnumType Id { get; set; }

    public string Name => Id.ToString();
}

public enum VehicleEnumType
{
    Hatchback,
    Sedan,
    SUV,
    Truck
}

public sealed class Truck : Vehicle
{
    public override VehicleType Type => new()
    {
        Id = VehicleEnumType.Truck
    };

    public double LoadCapacity { get; set; }
}

public sealed class SUV : Vehicle
{
    public override VehicleType Type => new()
    {
        Id = VehicleEnumType.SUV
    };

    public int NumberSeats { get; set; }
}

public sealed class Sedan : Vehicle
{
    public override VehicleType Type => new()
    {
        Id = VehicleEnumType.Sedan
    };

    public int NumberDoors { get; set; }
}

public sealed class Hatchback : Vehicle
{
    public override VehicleType Type => new()
    {
        Id = VehicleEnumType.Hatchback
    };

    public int NumberDoors { get; set; }
}