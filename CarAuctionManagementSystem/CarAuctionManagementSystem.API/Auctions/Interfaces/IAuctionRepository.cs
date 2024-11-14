using CarAuctionManagementSystem.API.Auctions.Models;

namespace CarAuctionManagementSystem.API.Auctions.Interfaces;

public interface IAuctionRepository
{
    Task<Auction> AddAsync(Auction auction);
    Task<Auction?> GetByIdAsync(string id);
    Task<IEnumerable<Auction>> GetAllAsync();
    Task<Auction> UpdateAsync(Auction auction);
}