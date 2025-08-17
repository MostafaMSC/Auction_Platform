using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Events.ProjectEvents;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Domain.ValueObjects;
namespace AuctionSystem.Domain.Entities
{
    // كلاس يمثل مشروع داخل النظام
    public class Project : SoftDeletableEntity
    {
        public string ProjectTitle { get; private set; } = string.Empty; // عنوان المشروع
        public string ProjectDescription { get; private set; } = string.Empty; // وصف المشروع
        public string ProjectOwnerId { get; private set; } // معرف مالك المشروع
        public User ProjectOwner { get; private set; } = null!; // مالك المشروع
        public int CategoryId { get; private set; } // معرف التصنيف
        public Category Category { get; private set; } = null!; // التصنيف المرتبط بالمشروع
        public string Location { get; private set; } = string.Empty; // موقع المشروع
        public Money EstimatedBudget { get; private set; } = new Money(0); // الميزانية التقديرية
        public ProjectStatus Status { get; private set; } // حالة المشروع

        public ICollection<Auction> Auctions { get; private set; } = new List<Auction>(); // المزادات المرتبطة بالمشروع

        // منشئ خاص يستخدم EF Core
        private Project() { }

        // منشئ لإنشاء مشروع جديد
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
            Status = ProjectStatus.Draft; // الحالة الافتراضية
        }

        // تحقق إذا كان المشروع قابل للتعديل
        public bool CanEdit() => Status != ProjectStatus.InAuction || Status == ProjectStatus.Active;

        // تقديم المشروع (تغيير الحالة إلى Active)
        public void Submit()
        {
            if (!CanEdit())
                throw new DomainException("Project cannot be submitted its in Action now");

            Status = ProjectStatus.Active; 
            RaiseDomainEvent(new ProjectSubmittedEvent(Id, ProjectOwnerId)); // رفع حدث عند التقديم
        }

        // إنشاء مزاد جديد للمشروع
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

            // تحقق من القيم
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
            Status = ProjectStatus.InAuction; // تغيير الحالة إلى InAuction

            RaiseDomainEvent(new ProjectAuctionCreatedEvent(Id, auction.Id)); // رفع حدث عند إنشاء المزاد
            return auction;
        }

        // إكمال المزاد المرتبط بالمشروع
        public void CompleteAuction()
        {
            if (Status != ProjectStatus.InAuction)
                throw new DomainException("Project must be in auction to complete");

            Status = ProjectStatus.AuctionCompleted;
            RaiseDomainEvent(new ProjectAuctionCompletedEvent(Id)); // رفع حدث عند إكمال المزاد
        }

        // تحديد متى يكون المشروع قابل للحذف
        protected override bool IsDeletable()
        {
            return Status == ProjectStatus.Draft; // يمكن حذف المشروع إذا كان في وضع Draft
        }
    }
}
