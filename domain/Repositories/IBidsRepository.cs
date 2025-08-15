using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Domain.Repositories
{
    public interface IBidRepository
    {
        // استرجاع كل العروض
        Task<IEnumerable<Bid>> GetAllAsync();

        // استرجاع عرض معين حسب Id
        Task<Bid?> GetByIdAsync(int id);

        // استرجاع عروض حسب شروط متعددة
        Task<IEnumerable<Bid>> GetBidsAsync(
            int? auctionId = null,
            string? userId = null,
            BidStatus? status = null
        );

        // تحديث عرض موجود
        Task<bool> UpdateBidAsync(Bid bid);

        Task<bool> DeleteBidAsync(int id);

        // ملاحظة: إنشاء العروض يجب أن يتم عبر Auction.PlaceBid() وليس مباشرة
        Task PlaceBidAsync(Bid bid); 
    }
}
