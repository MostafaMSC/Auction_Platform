using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetAllAsync()
        {
            return await _context.Bids
                .Include(b => b.Auction)
                .Include(b => b.Seller)
                .ToListAsync();
        }

        public async Task<Bid?> GetByIdAsync(int id)
        {
            return await _context.Bids
                .Include(b => b.Auction)
                .Include(b => b.Seller)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bid>> GetBidsAsync(int? auctionId = null, string? userId = null, BidStatus? status = null)
        {
            var query = _context.Bids
                .AsQueryable();
            if (auctionId.HasValue)
                query = query.Where(b => b.AuctionId == auctionId.Value);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(b => b.SellerId == userId);

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            return await query
                .Include(b => b.Auction)
                .Include(b => b.Seller)
                .ToListAsync();
        }

        // منع إنشاء عرض مباشر خارج Aggregate Root
        public Task PlaceBidAsync(Bid bid)
        {
            throw new InvalidOperationException(
                "Direct creation of bids is not allowed. Use Auction.PlaceBid() via Aggregate Root."
            );
        }

        public async Task<bool> UpdateBidAsync(Bid bid)
        {
            _context.Bids.Update(bid);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBidAsync(int id)
        {
            var bid = await GetByIdAsync(id);
            if (bid == null) return false;

            _context.Bids.Remove(bid);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<Bid>> GetBidsBySellerIdAsync(string sellerId)
{
    if (string.IsNullOrEmpty(sellerId))
        return new List<Bid>();

    return await _context.Bids
        .Where(b => b.SellerId == sellerId)
        .Include(b => b.Auction)
        .Include(b => b.Seller)
        .ToListAsync();
}

    }
}
