using AuctionSystem.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            // ابحث عن المستخدم أولاً بواسطة البريد
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            // استخدم UserName مع SignInManager
            var result = await _signInManager.PasswordSignInAsync(
                user.Email,  
                password,
                isPersistent: true,
                lockoutOnFailure: false
            );

            return result.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
