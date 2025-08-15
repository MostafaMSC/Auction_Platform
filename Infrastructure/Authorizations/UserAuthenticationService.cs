using AuctionSystem.Application.Interfaces;
using AuctionSystem.Application.Queries.Users;
using AuctionSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuctionSystem.Infrastructure.Identity
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserAuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool Succeeded, string ErrorMessage, UserProfileDto? Profile, string? AccessToken, string? RefreshToken)>
            LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "Invalid email or password", null, null, null);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
                return (false, "Invalid email or password", null, null, null);

            var profile = new UserProfileDto(
                user.Id,
                user.Email!,
                user.UserName!,
                user.AccountType,
                user.VerificationStatus
            );

            // TODO: Implement real token generation
            return (true, null, profile, "access_token", "refresh_token");
        }
    }
}
