using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Events.ProjectEvents;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Domain.Entities
{
    public class Project : SoftDeletableEntity
    {
        public string ProjectTitle { get; private set; } = string.Empty;
        public string ProjectDescription { get; private set; } = string.Empty;
        public string ProjectOwnerId { get; private set; }
        public User ProjectOwner { get; private set; } = null!;
        public int CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
        public string Location { get; private set; } = string.Empty;
        public Money EstimatedBudget { get; private set; } = new Money(0);
        public ProjectStatus Status { get; private set; }

        // Navigation property for auctions
        public ICollection<Auction> Auctions { get; private set; } = new List<Auction>();

        // Private constructor for EF Core
        private Project() { }

        // Public constructor for creating new projects
        public Project(
            string title,
            string description,
            string ownerId,
            int categoryId,
            string location,
            Money estimatedBudget)
        {
            ProjectTitle = title ?? throw new ArgumentNullException(nameof(title));
            ProjectDescription = description ?? throw new ArgumentNullException(nameof(description));
            ProjectOwnerId = ownerId;
            CategoryId = categoryId;
            Location = location ?? throw new ArgumentNullException(nameof(location));
            EstimatedBudget = estimatedBudget ?? throw new ArgumentNullException(nameof(estimatedBudget));
            Status = ProjectStatus.Draft;
        }

        public bool CanEdit() => Status == ProjectStatus.Draft;

        public void Submit()
        {
            if (Status != ProjectStatus.Draft)
                throw new DomainException("Only draft projects can be submitted");

            Status = ProjectStatus.Active; // Changed from Draft to Active
            RaiseDomainEvent(new ProjectSubmittedEvent(Id, ProjectOwnerId));
        }

        public Auction CreateAuction(
            Money startingPrice,
            Money minPrice,
            Money targetPrice,
            TimeSpan maxDuration,
            TimeSpan priceDropInterval,
            Money priceDropAmount)
        {
            if (Status != ProjectStatus.Active)
                throw new DomainException("Only active projects can create auctions");

            // Validate auction parameters
            if (startingPrice.Amount <= 0)
                throw new DomainException("Starting price must be greater than zero");

            if (minPrice.Amount <= 0)
                throw new DomainException("Minimum price must be greater than zero");

            if (startingPrice.Amount < minPrice.Amount)
                throw new DomainException("Starting price cannot be less than minimum price");

            if (maxDuration <= TimeSpan.Zero)
                throw new DomainException("Max duration must be positive");

            if (priceDropInterval <= TimeSpan.Zero)
                throw new DomainException("Price drop interval must be positive");

            if (priceDropAmount.Amount <= 0)
                throw new DomainException("Price drop amount must be positive");

            var auction = new Auction(
                Id,
                startingPrice,
                minPrice,
                targetPrice,
                maxDuration,
                priceDropInterval,
                priceDropAmount
            );

            Auctions.Add(auction);
            Status = ProjectStatus.InAuction;

            RaiseDomainEvent(new ProjectAuctionCreatedEvent(Id, auction.Id));
            return auction;
        }

        public void CompleteAuction()
        {
            if (Status != ProjectStatus.InAuction)
                throw new DomainException("Project must be in auction to complete");

            Status = ProjectStatus.Active;
            RaiseDomainEvent(new ProjectAuctionCompletedEvent(Id));
        }

        protected override bool IsDeletable()
        {
            return Status == ProjectStatus.Draft;
        }
    }
}