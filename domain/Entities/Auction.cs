    using AuctionSystem.Domain.Events.AuctionEvents;
    using AuctionSystem.Domain.Events;
    using AuctionSystem.Domain.ValueObjects;
    using AuctionSystem.Domain.Abstractions;
    using AuctionSystem.Domain.Exceptions;
    using AuctionSystem.Domain.Constants;
// Auction: يمثل المزاد في النظام
// يحتوي على أسعار المزاد، مدة المزاد، العروض، والحالة الحالية للمزاد
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

            // ===== Auction Methods =====

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

                var scheduledNewPrice = Math.Max(CurrentPrice.Amount - PriceDropAmount.Amount, MinPrice.Amount);

                if (scheduledNewPrice < CurrentPrice.Amount)
                {
                    var oldPrice = CurrentPrice;
                    CurrentPrice = new Money(scheduledNewPrice);
                    RaiseDomainEvent(new AuctionPriceDecreasedEvent(Id, oldPrice, CurrentPrice));
                }

                if (CurrentPrice.Amount <= MinPrice.Amount)
                {
                    CloseAuction();
                }
            }

            public void CheckForAutoClose()
            {
                if (Status != AuctionStatus.Active)
                    return;

                if (IsExpired)
                {
                    CloseAuction();
                    return;
                }

                if (CurrentPrice.Amount <= TargetPrice.Amount)
                {
                    var winningBid = Bids
                        .Where(b => b.Amount.Amount >= TargetPrice.Amount)
                        .OrderBy(b => b.CreatedAt)
                        .FirstOrDefault();

                    if (winningBid != null)
                    {
                        WinningBidId = winningBid.Id;
                        WinningBid = winningBid;
                    }

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

                if (bidAmount.Amount < MinPrice.Amount)
                    throw new DomainException($"Bid amount cannot be less than minimum price of {MinPrice.Amount}");

                var bid = new Bid(Id, sellerId, bidAmount);
                Bids.Add(bid);

                var oldPrice = CurrentPrice;
                CurrentPrice = bidAmount;

                if (bidAmount.Amount < oldPrice.Amount)
                    RaiseDomainEvent(new AuctionPriceDecreasedEvent(Id, oldPrice, CurrentPrice));

                // إشعار لتقديم العرض
                RaiseDomainEvent(new NotificationCreatedEvent(
                    Guid.Parse(sellerId),
                    "تم تقديم عرضك",
                    $"تم تسجيل عرضك بقيمة {bidAmount.Amount} على المزاد رقم {Id}."
                ));

                if (bidAmount.Amount >= TargetPrice.Amount)
                {
                    WinningBidId = bid.Id;
                    WinningBid = bid;
                    CloseAuction();
                }

                return bid;
            }

            public void CloseAuction()
            {
                if (Status != AuctionStatus.Active)
                    throw new DomainException("Only active auctions can be closed");

                Status = AuctionStatus.Completed;
                if (!EndAt.HasValue || DateTime.UtcNow < EndAt.Value)
                    EndAt = DateTime.UtcNow;

                Project?.CompleteAuction();

                RaiseDomainEvent(new AuctionClosedEvent(Id, WinningBidId, WinningBid?.SellerId, CurrentPrice));

                // إشعارات لجميع المشاركين
                foreach (var bid in Bids)
                {
                    RaiseDomainEvent(new NotificationCreatedEvent(
                        Guid.Parse(bid.SellerId),
                        "تم إغلاق المزاد",
                        $"المزاد رقم {Id} قد تم إغلاقه."
                    ));
                }
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

                // إشعار للفائز
                RaiseDomainEvent(new NotificationCreatedEvent(
                    Guid.Parse(winningBid.SellerId),
                    "لقد فزت بالمزاد!",
                    $"تهانينا! عرضك بقيمة {winningBid.Amount.Amount} فاز بالمزاد رقم {Id}."
                ));
            }

            public void CancelAuction()
            {
                if (Status == AuctionStatus.Completed)
                    throw new DomainException("Cannot cancel completed auction");

                Status = AuctionStatus.Cancelled;
                EndAt = DateTime.UtcNow;

                RaiseDomainEvent(new AuctionCancelledEvent(Id));
            }
    // ------------------ Update Methods ------------------ //

    public void UpdateStartingPrice(Money newStartingPrice)
    {
        if (newStartingPrice.Amount <= 0)
            throw new DomainException("Starting price must be positive.");

        StartingPrice = newStartingPrice;
    }

    public void UpdateStartDate(DateTime newStart)
    {
        StartAt = newStart;
    }

    public void UpdateEndDate(DateTime newEnd)
    {
        EndAt = newEnd;
    }

    public void UpdateMinPrice(Money newMinPrice)
    {
        MinPrice = newMinPrice;
    }

    public void UpdateTargetPrice(Money newTargetPrice)
    {
        TargetPrice = newTargetPrice;
    }
    // داخل Auction class

    public void UpdateProjectId(int projectId)
    {
        ProjectId = projectId;
    }

    public void UpdateMaxDuration(TimeSpan maxDuration)
    {
        MaxDuration = maxDuration;
    }

    public void UpdatePriceDropInterval(TimeSpan interval)
    {
        PriceDropInterval = interval;
    }

    public void UpdatePriceDropAmount(Money amount)
    {
        PriceDropAmount = amount ?? throw new ArgumentNullException(nameof(amount));
    }

            protected override bool IsDeletable()
            {
                return Status == AuctionStatus.Draft;
            }
        }
    }
