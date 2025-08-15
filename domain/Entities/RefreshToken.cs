using System;

namespace AuctionSystem.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; } // Primary Key
        public string Token { get; set; } = string.Empty; 
        public string UserId { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime ExpiresAt { get; set; } 
        public bool IsRevoked { get; set; } = false; 
        public DateTime? RevokedAt { get; set; } 
        public string? ReplacedByToken { get; set; } 

        // Parameterless constructor for EF Core
        public RefreshToken() { }

        // Optional helper constructor
        public RefreshToken(string userId, int expirationDays)
        {
            UserId = userId;
            Token = GenerateToken();
            ExpiresAt = DateTime.UtcNow.AddDays(expirationDays);
            CreatedAt = DateTime.UtcNow;
        }

        private string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
