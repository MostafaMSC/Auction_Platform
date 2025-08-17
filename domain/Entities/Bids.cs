using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Domain.Entities
{
    // كلاس يمثل عرض مقدم على مزاد
    public class Bid : Entity
    {
        public int AuctionId { get; set; } // رقم المزاد المرتبط بالعرض
        public Auction Auction { get; set; } = null!; // كائن المزاد نفسه

        public string SellerId { get; set; } // رقم أو معرف البائع
        public User Seller { get; set; } = null!; // كائن البائع

        public Money Amount { get; set; } = new Money(0); // قيمة العرض
        public BidStatus Status { get; set; } // حالة العرض (مقبول، مرفوض، إلخ)

        // constructor فارغ خاص بـ EF Core
        private Bid() { }

        // إنشاء عرض جديد
        public Bid(int auctionId, string sellerId, Money amount, string? notes = null)
        {
            AuctionId = auctionId; // ربط العرض بالمزاد
            SellerId = sellerId; // ربط العرض بالبائع
            Amount = amount ?? throw new ArgumentNullException(nameof(amount)); // قيمة العرض يجب أن تكون موجودة
        }
    }
}
