namespace CarAuctionManagementSystem.API.Auctions.DataTransferObject;

public record CreateBidDto
{
    public string Username { get; init; }
    public double Amount { get; init; }
}