using AuctionSystem.Application.Queries.Users;

namespace AuctionSystem.Application.Commands.Users
{
    public record UpdateProfileResult(
        bool Success,
        string? ErrorMessage,
        UserProfileDto? UpdatedProfile = null
    );
}
