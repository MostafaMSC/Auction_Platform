using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ApplicationDbContext _context;

        public AuctionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // استرجاع جميع المزادات الغير محذوفة
        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _context.Auctions
                .Where(a => !a.IsDeleted)
                .Include(a => a.Bids)
                .ToListAsync();
        }

        // استرجاع مزاد واحد مع العروض
        public async Task<Auction?> GetByIdAsync(int id)
        {
            return await _context.Auctions
                .Where(a => !a.IsDeleted)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // استرجاع مزادات حسب الحالة
        public async Task<IEnumerable<Auction>> GetByStatusAsync(AuctionStatus status)
        {
            return await _context.Auctions
                .Where(a => !a.IsDeleted && a.Status == status)
                .Include(a => a.Bids)
                .ToListAsync();
        }

        // استرجاع مزاد حسب المشروع
        public async Task<Auction?> GetByProjectIdAsync(int projectId)
        {
            return await _context.Auctions
                .Where(a => !a.IsDeleted && a.ProjectId == projectId)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync();
        }

        // إنشاء مزاد جديد
        public async Task CreateAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
        }

        // تحديث مزاد
public async Task<bool> UpdateAsync(Auction auction, CancellationToken cancellationToken = default)
{
    _context.Auctions.Update(auction);
    return await _context.SaveChangesAsync(cancellationToken) > 0;
}


        // حذف المزاد بطريقة Soft Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var auction = await GetByIdAsync(id);
            if (auction == null) return false;  

            _context.Auctions.Update(auction);
            return await _context.SaveChangesAsync() > 0;
        }

        // إغلاق المزاد
        public async Task<bool> CloseAuctionAsync(int id)
        {
            var auction = await GetByIdAsync(id);
            if (auction == null || auction.Status != AuctionStatus.Active) return false;

            auction.CloseAuction(); // Aggregate Root يرفع Domain Event
            _context.Auctions.Update(auction);

            // حفظ التغييرات (يجب لاحقًا إرسال Domain Events عبر Mediator أو Event Dispatcher)
            return await _context.SaveChangesAsync() > 0;
        }

        // إضافة عرض جديد
        public async Task PlaceBidAsync(Bid bid)
        {
            var auction = await GetByIdAsync(bid.AuctionId);
            if (auction == null) throw new InvalidOperationException("Auction not found");

            auction.PlaceBid(bid.SellerId, bid.Amount); // Aggregate Root يتحكم بالعروض
            _context.Auctions.Update(auction);

            await _context.SaveChangesAsync();

            // لاحقًا يمكن إرسال Domain Events بعد الحفظ
            // await _domainEventDispatcher.DispatchAsync(auction.DomainEvents);
            // auction.ClearDomainEvents();
        }

        public async Task<Auction?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
{
    return await _context.Auctions
        .Where(a => !a.IsDeleted && a.Id == id)
        .Include(a => a.Bids)
        .FirstOrDefaultAsync(cancellationToken);
}

    }
}
