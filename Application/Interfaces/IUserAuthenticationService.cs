using AuctionSystem.Application.Queries.Users;

namespace AuctionSystem.Application.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<(bool Succeeded, string ErrorMessage, UserProfileDto? Profile, string? AccessToken, string? RefreshToken)>
            LoginAsync(string email, string password);
    }
}
