using AuctionSystem.Application.Interfaces;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace AuctionSystem.Infrastructure.Identity
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<User> _userManager;

        public UserRegistrationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool Succeeded, string ErrorMessage, string? UserId)> RegisterUserAsync(
            string username,
            string email,
            string password,
            AccountType accountType)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return (false, "User already exists with this email", null);

            var user = new User
            {
                UserName = username,
                Email = email,
                AccountType = accountType
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)), null);

            await _userManager.AddToRoleAsync(user, accountType.ToString());
            return (true, null, user.Id);
        }
    }
}
