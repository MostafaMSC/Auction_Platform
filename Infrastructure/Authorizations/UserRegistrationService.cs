using AuctionSystem.Application.Interfaces;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace AuctionSystem.Infrastructure.Identity
{
    // خدمة لتسجيل المستخدمين الجدد
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<User> _userManager;

        public UserRegistrationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // تسجيل مستخدم جديد
        public async Task<(bool Succeeded, string ErrorMessage, string? UserId)> RegisterUserAsync(
            string username,
            string email,
            string password,
            AccountType accountType)
        {
            // التحقق من وجود مستخدم بنفس البريد الإلكتروني
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return (false, "User already exists with this email", null);

            // إنشاء كائن مستخدم جديد
            var user = new User
            {
                UserName = username,
                Email = email,
                AccountType = accountType
            };

            // إنشاء المستخدم في قاعدة البيانات مع كلمة المرور
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)), null);

            // إضافة الدور المناسب للمستخدم حسب نوع الحساب
            await _userManager.AddToRoleAsync(user, accountType.ToString());

            // إرجاع نجاح العملية ومعرف المستخدم الجديد
            return (true, null, user.Id);
        }
    }
}
