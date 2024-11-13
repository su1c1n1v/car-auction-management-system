using CarAuctionManagementSystem.API.Auctions.Interfaces;
using CarAuctionManagementSystem.API.Auctions.Models;

namespace CarAuctionManagementSystem.API.Auctions;

public class AuctionRepository : IAuctionRepository
{
    private readonly List<Auction> _auctions = [];

    public Task<Auction> AddAsync(Auction auction) =>
        Task.Run(() =>
        {
            _auctions.Add(auction);
            return auction;
        });

    public Task<Auction?> GetByIdAsync(string id) =>
        Task.FromResult(_auctions.FirstOrDefault(a => a.Id.ToString() == id));

    public Task<IEnumerable<Auction>> GetAllAsync() =>
        Task.FromResult(_auctions.AsEnumerable());

    public Task<Auction> UpdateAsync(Auction auction) =>
        Task.Run(() =>
        {
            var index = _auctions.FindIndex(a => a.Id == auction.Id);
            _auctions[index] = auction;
            return auction;
        });
}