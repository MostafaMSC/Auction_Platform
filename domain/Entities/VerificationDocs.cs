
using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Domain.Entities
{
    public class VerificationDoc : Entity
    {
        public string UserId { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty;
        public VerificationStatus VerificationStatus { get; set; }


        public virtual User User { get; set; } = default!;

    }
}
