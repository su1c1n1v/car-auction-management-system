using CarAuctionManagementSystem.API.Vehicles.Models;

namespace CarAuctionManagementSystem.API.Vehicles.DataTransferObject;

public record CreateVehicleDto
{
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public VehicleEnumType Type { get; set; }
    public int Year { get; set; }
    public double StartingBid { get; set; }
    public int? NumberSeats { get; set; }
    public int? LoadCapacity { get; set; }
    public int? NumberDoors { get; set; }
}