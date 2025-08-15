using AuctionSystem.Domain.Events.AuctionEvents;
using AuctionSystem.Domain.ValueObjects;
using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.Exceptions;

namespace AuctionSystem.Domain.Entities
{
    public class Auction : SoftDeletableEntity
    {
        public int ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;

        public Money StartingPrice { get; private set; }
        public Money CurrentPrice { get; private set; }
        public Money MinPrice { get; private set; }
        public Money TargetPrice { get; private set; }

        public DateTime? StartAt { get; private set; }
        public DateTime? EndAt { get; private set; }
        public TimeSpan MaxDuration { get; private set; }

        public TimeSpan PriceDropInterval { get; private set; }
        public Money PriceDropAmount { get; private set; }

        public AuctionStatus Status { get; private set; } = AuctionStatus.Draft;

        // Winner information
        public int? WinningBidId { get; private set; }
        public Bid? WinningBid { get; private set; }

        public ICollection<Bid> Bids { get; private set; } = new List<Bid>();

        public bool IsActive =>
            Status == AuctionStatus.Active &&
            StartAt.HasValue &&
            EndAt.HasValue &&
            StartAt <= DateTime.UtcNow &&
            EndAt >= DateTime.UtcNow;

        public bool IsExpired =>
            EndAt.HasValue && DateTime.UtcNow > EndAt.Value;

        // Private constructor for EF Core
        private Auction() { }

        public Auction(
            int projectId,
            Money startingPrice,
            Money minPrice,
            Money targetPrice,
            TimeSpan maxDuration,
            TimeSpan priceDropInterval,
            Money priceDropAmount
        )
        {
            ProjectId = projectId;
            StartingPrice = startingPrice ?? throw new ArgumentNullException(nameof(startingPrice));
            CurrentPrice = startingPrice;
            MinPrice = minPrice ?? throw new ArgumentNullException(nameof(minPrice));
            TargetPrice = targetPrice ?? throw new ArgumentNullException(nameof(targetPrice));
            MaxDuration = maxDuration;
            PriceDropInterval = priceDropInterval;
            PriceDropAmount = priceDropAmount ?? throw new ArgumentNullException(nameof(priceDropAmount));

            Status = AuctionStatus.Draft;
        }

        public void StartAuction()
        {
            if (Status != AuctionStatus.Draft)
                throw new DomainException("Auction is already started or closed");

            StartAt = DateTime.UtcNow;
            EndAt = StartAt.Value.Add(MaxDuration);
            Status = AuctionStatus.Active;

            RaiseDomainEvent(new AuctionCreatedEvent(Id, ProjectId, StartAt.Value, EndAt.Value));
        }

        public void DecreasePrice()
        {
            if (!IsActive)
                throw new DomainException("Auction is not active");

            var newPrice = Math.Max(CurrentPrice.Amount - PriceDropAmount.Amount, MinPrice.Amount);
            var oldPrice = CurrentPrice;
            CurrentPrice = new Money(newPrice);

            RaiseDomainEvent(new AuctionPriceDecreasedEvent(Id, oldPrice, CurrentPrice));

            // Auto-close if minimum price reached
            if (CurrentPrice.Amount <= MinPrice.Amount)
            {
                CloseAuction();
            }
        }

        public void CheckForAutoClose()
        {
            if (IsExpired && Status == AuctionStatus.Active)
            {
                CloseAuction();
            }
        }

        public Bid PlaceBid(string sellerId, Money bidAmount)
{
    if (!IsActive)
        throw new DomainException("Auction is not active");

    if (bidAmount.Amount > CurrentPrice.Amount)
        throw new DomainException("Bid amount cannot exceed current auction price");

    if (bidAmount.Amount <= 0)
        throw new DomainException("Bid amount must be positive");

    var bid = new Bid(Id, sellerId, bidAmount);
    Bids.Add(bid);

    RaiseDomainEvent(new BidPlacedEvent(Id, sellerId, bidAmount));

    return bid; // <-- Return the newly created bid
}


        public void CloseAuction()
        {
            if (Status != AuctionStatus.Active)
                throw new DomainException("Only active auctions can be closed");

            Status = AuctionStatus.Completed;
            if (!EndAt.HasValue || DateTime.UtcNow < EndAt.Value)
            {
                EndAt = DateTime.UtcNow;
            }

            RaiseDomainEvent(new AuctionClosedEvent(Id, WinningBidId, WinningBid?.SellerId, CurrentPrice));
        }

        public void SelectWinner(int winningBidId)
        {
            if (Status != AuctionStatus.Completed)
                throw new DomainException("Auction must be completed before selecting a winner");

            var winningBid = Bids.FirstOrDefault(b => b.Id == winningBidId)
                ?? throw new DomainException("Winning bid not found");

            WinningBidId = winningBidId;
            WinningBid = winningBid;

            RaiseDomainEvent(new WinnerSelectedEvent(Id, winningBid.Id, winningBid.SellerId, winningBid.Amount));
        }

        public void CancelAuction()
        {
            if (Status == AuctionStatus.Completed)
                throw new DomainException("Cannot cancel completed auction");

            Status = AuctionStatus.Cancelled;
            EndAt = DateTime.UtcNow;

            RaiseDomainEvent(new AuctionCancelledEvent(Id));
        }

        protected override bool IsDeletable()
        {
            return Status == AuctionStatus.Draft;
        }
    }
}
