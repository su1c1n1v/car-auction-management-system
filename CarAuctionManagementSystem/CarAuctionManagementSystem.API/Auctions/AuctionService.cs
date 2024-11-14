using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using CarAuctionManagementSystem.API.Auctions.Interfaces;
using CarAuctionManagementSystem.API.Auctions.Models;
using CarAuctionManagementSystem.API.Vehicles.Interfaces;

namespace CarAuctionManagementSystem.API.Auctions;

public class AuctionService(
    IAuctionRepository auctionRepository,
    ILogger<AuctionService> logger,
    IVehicleRepository vehicleRepository) : IAuctionService
{
    public async Task<Auction> AddAsync(CreateAuctionDto auction)
    {
        logger.LogInformation("Service: Adding new auction");

        var vehicle = await vehicleRepository.GetByIdAsync(auction.VehicleId);

        if (vehicle is null)
        {
            logger.LogWarning("Service: Vehicle not found");
            throw new NullReferenceException("Vehicle not found");
        }

        var existingAuction = await GetAllAsync();

        bool isAuctionForThisVehicle = existingAuction.Any(a =>
            a.Vehicle.Id == vehicle.Id &&
            (a.Status.Id == AuctionStatusEnum.Active ||
             a.Status.Id == AuctionStatusEnum.Pending ||
             a.Status.Id == AuctionStatusEnum.Sold));
        
        if (isAuctionForThisVehicle)
        {
            logger.LogWarning("Service: Auction already exists an active auction for this vehicle");
            throw new BadHttpRequestException("Auction already exists an active auction for this vehicle");
        }

        var result = await auctionRepository.AddAsync(new Auction(vehicle));

        logger.LogInformation("Service: New auction added");

        return result;
    }

    public async Task<Auction> GetByIdAsync(string id)
    {
        logger.LogInformation("Service: Getting auction by id");

        var result = await auctionRepository.GetByIdAsync(id);

        if (result is null)
        {
            logger.LogWarning("Service: Auction not found");
            throw new NullReferenceException("Auction not found");
        }

        logger.LogInformation("Service: Auction found");

        return result;
    }

    public Task<IEnumerable<Auction>> GetAllAsync()
    {
        logger.LogInformation("Service: Getting all auctions");

        var result = auctionRepository.GetAllAsync();

        logger.LogInformation("Service: All auctions found");

        return result;
    }

    public async Task<Auction> StartAsync(string id)
    {
        logger.LogInformation("Service: Updating auction");

        var auction = await GetByIdAsync(id);

        if (auction.Status.Id != AuctionStatusEnum.Pending)
        {
            logger.LogWarning("Service: Auction is not pending");
            throw new BadHttpRequestException("Auction is not pending");
        }

        auction.Start = DateTime.Now;
        auction.Status = new AuctionStatus(AuctionStatusEnum.Active);

        var result = await auctionRepository.UpdateAsync(auction);

        logger.LogInformation("Service: Auction updated");

        return result;
    }

    public async Task<Auction> FinishAsync(string id)
    {
        logger.LogInformation("Service: Finishing auction");

        var auction = await GetByIdAsync(id);

        if (auction.Status.Id != AuctionStatusEnum.Active)
        {
            logger.LogWarning("Service: Auction is not active");
            throw new BadHttpRequestException("Auction is not active");
        }

        auction.End = DateTime.Now;

        auction.Status = auction.Bids.Any()
            ? new AuctionStatus(AuctionStatusEnum.Sold)
            : new AuctionStatus(AuctionStatusEnum.Cancelled);

        var result = await auctionRepository.UpdateAsync(auction);

        logger.LogInformation("Service: Auction finished");

        return result;
    }

    public async Task<Auction> OfferBidAsync(string id, CreateBidDto bid)
    {
        logger.LogInformation("Service: Offering bid");

        var auction = await GetByIdAsync(id);

        if (auction.Status.Id != AuctionStatusEnum.Active)
        {
            logger.LogWarning("Service: Auction is not active");
            throw new BadHttpRequestException("Auction is not active");
        }

        bool isAmountLowerThanStartingBid = bid.Amount < auction.Vehicle.StartingBid;
        bool isAmountLowerThanCurrentHighestBid = auction.Bids.Any() && bid.Amount <= auction.Bids.Max(b => b.Amount);

        if (isAmountLowerThanCurrentHighestBid || isAmountLowerThanStartingBid)
        {
            logger.LogWarning("Service: Bid amount is lower than the current highest bid");
            throw new BadHttpRequestException("Bid amount is lower than the current highest bid");
        }

        auction.Bids.Add(new Bid
        {
            Amount = bid.Amount,
            Username = bid.Username
        });

        var result = await auctionRepository.UpdateAsync(auction);

        logger.LogInformation("Service: Bid offered");

        return result;
    }
}