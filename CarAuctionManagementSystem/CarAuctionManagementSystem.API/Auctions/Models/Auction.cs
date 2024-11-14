using CarAuctionManagementSystem.API.Vehicles.Models;

namespace CarAuctionManagementSystem.API.Auctions.Models;

public class Auction(Vehicle vehicle) : AuditEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public Vehicle Vehicle { get; init; } = vehicle;
    public List<Bid> Bids { get; set; } = [];
    public AuctionStatus Status { get; set; } = new(AuctionStatusEnum.Pending);
}

public class AuctionStatus(AuctionStatusEnum status)
{
    public AuctionStatusEnum Id { get; init; } = status;
    public string Name => Id.ToString();
}

public enum AuctionStatusEnum
{
    Pending,
    Active,
    Sold,
    Cancelled
}

public class Bid
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public double Amount { get; set; }
    public string Username { get; set; } = string.Empty;
}