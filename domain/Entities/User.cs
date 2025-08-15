using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using AuctionSystem.Domain.Events;
using AuctionSystem.Domain.Events.UserEvents;

namespace AuctionSystem.Domain.Entities
{
    public class User : IdentityUser 
    {
        public string FullName { get; set; } = string.Empty;
        public VerificationStatus VerificationStatus { get; set; }
        public AccountType AccountType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public ICollection<VerificationDoc> VerificationDocs { get; set; } = new List<VerificationDoc>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public bool CanEditOrDeleteProject(Project project)
        {
            return project.ProjectOwnerId.ToString() == Id && project.Status == ProjectStatus.Draft;
        }

        public bool CanCreateProject()
        {
            return AccountType == AccountType.BuyerUser &&
            VerificationStatus == VerificationStatus.Approved &&
            !IsDeleted;
        }
        public bool CanSubmitBid()
        {
            return AccountType == AccountType.SellerUser &&
                VerificationStatus == VerificationStatus.Approved &&
                !IsDeleted;
        }

        public void Approve()
        {
            if (VerificationStatus == VerificationStatus.Approved)
                throw new DomainException("User already approved");

            VerificationStatus = VerificationStatus.Approved;
            // RaiseDomainEvent(new UserVerifiedEvent(Id));
        }
        public void Reject(string reason)
        {
            if (VerificationStatus != VerificationStatus.Pending)
                throw new DomainException("Can only reject pending verification");
                
            VerificationStatus = VerificationStatus.Rejected;
            // RaiseDomainEvent(new UserRejectedEvent(Id, reason));
        }
}

}
