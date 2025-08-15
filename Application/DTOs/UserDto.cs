using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Application.DTOs
{
    public record UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public AccountType AccountType { get; set; } 
        public VerificationStatus VerificationStatus { get; set; } 
    }
}