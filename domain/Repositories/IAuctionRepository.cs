using AuctionSystem.Domain.Entities;

public interface IAuctionRepository
{
    Task<IEnumerable<Auction>> GetAllAsync();
    Task<Auction?> GetByIdAsync(int id, CancellationToken cancellationToken = default);  // use this
    Task<IEnumerable<Auction>> GetByStatusAsync(AuctionStatus status);
    Task<Auction?> GetByProjectIdAsync(int projectId);
    Task CreateAsync(Auction auction);
    Task<bool> UpdateAsync(Auction auction, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id);
    Task<bool> CloseAuctionAsync(int id);
    Task PlaceBidAsync(Bid bid);
}
