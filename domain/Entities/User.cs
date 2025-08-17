using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using AuctionSystem.Domain.Events;
using AuctionSystem.Domain.Events.UserEvents;

namespace AuctionSystem.Domain.Entities
{
    // كلاس يمثل المستخدم داخل النظام
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty; // الاسم الكامل للمستخدم
        public VerificationStatus VerificationStatus { get; set; } // حالة التحقق من المستخدم
        public AccountType AccountType { get; set; } // نوع الحساب (Buyer أو Seller)
        public bool IsDeleted { get; set; } = false; // الحذف الناعم
        public DateTime? DeletedAt { get; set; } // وقت الحذف الناعم

        // دالة لحذف المستخدم بشكل ناعم
        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        // علاقات المستخدم بالمشاريع والمزايدات والوثائق والإشعارات
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public ICollection<VerificationDoc> VerificationDocs { get; set; } = new List<VerificationDoc>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        // تحقق إذا كان المستخدم يمكنه تعديل أو حذف المشروع
        public bool CanEditOrDeleteProject(Project project)
        {
            return project.ProjectOwnerId.ToString() == Id && project.Status == ProjectStatus.Draft;
        }

        // تحقق إذا كان المستخدم يمكنه إنشاء مشروع
        public bool CanCreateProject()
        {
            return AccountType == AccountType.BuyerUser &&
                   VerificationStatus == VerificationStatus.Approved &&
                   !IsDeleted;
        }

        // تحقق إذا كان المستخدم يمكنه تقديم عرض
        public bool CanSubmitBid()
        {
            return AccountType == AccountType.SellerUser &&
                   VerificationStatus == VerificationStatus.Approved &&
                   !IsDeleted;
        }

        // إدارة الأحداث الخاصة بالنطاق (Domain Events)
        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // دالة للموافقة على المستخدم
        public void Approve()
        {
            if (VerificationStatus == VerificationStatus.Approved)
                throw new DomainException("User already approved");

            VerificationStatus = VerificationStatus.Approved;
            RaiseDomainEvent(new UserVerifiedEvent(Id, UserName));
        }

        // دالة لرفض المستخدم مع سبب
        public void Reject(string reason)
        {
            if (VerificationStatus != VerificationStatus.Pending)
                throw new DomainException("Can only reject pending verification");

            VerificationStatus = VerificationStatus.Rejected;
            RaiseDomainEvent(new UserRejectedEvent(Id, reason));
        }

        // مسح كل الأحداث
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
