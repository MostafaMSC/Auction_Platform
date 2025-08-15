using AuctionSystem.Domain.Abstractions;
using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Domain.Entities
{
    public class Notification : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
    }
}
