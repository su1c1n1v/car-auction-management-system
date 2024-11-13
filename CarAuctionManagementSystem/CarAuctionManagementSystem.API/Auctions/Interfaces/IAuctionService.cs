using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using CarAuctionManagementSystem.API.Auctions.Models;

namespace CarAuctionManagementSystem.API.Auctions.Interfaces;

public interface IAuctionService
{
    Task<Auction> GetByIdAsync(string id);
    Task<IEnumerable<Auction>> GetAllAsync();
    Task<Auction> AddAsync(CreateAuctionDto auction);
    Task<Auction> StartAsync(string id);
    Task<Auction> FinishAsync(string id);
    Task<Auction> OfferBidAsync(string id, CreateBidDto bid);
}