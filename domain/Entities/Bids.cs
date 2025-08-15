using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Domain.Entities
{
    public class Bid : Entity
    {
        public int AuctionId { get; set; }
        public Auction Auction { get; set; } = null!;
        public string SellerId { get; set; }
        public User Seller { get; set; } = null!;
        public Money Amount { get; set; } = new Money(0);
        public BidStatus Status { get; set; }
        private Bid() { }

        public Bid(int auctionId, string sellerId, Money amount, string? notes = null)
        {
            AuctionId = auctionId;
            SellerId = sellerId;
            Amount = amount ?? throw new ArgumentNullException(nameof(amount));

        }
    }
        
    }
