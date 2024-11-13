namespace CarAuctionManagementSystem.API.Auctions.DataTransferObject;

public record CreateAuctionDto
{
    public string VehicleId { get; init; }
}