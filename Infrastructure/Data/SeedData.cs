using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedRolesAndUsersAsync(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Create roles based on AccountType enum
        foreach (AccountType accountType in Enum.GetValues(typeof(AccountType)))
        {
            var roleName = accountType.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Admin user
        if (!await userManager.Users.AnyAsync(u => u.UserName == "admin"))
        {
            var admin = new User
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true,
                FullName = "System Admin",
                AccountType = AccountType.Admin,
                VerificationStatus = VerificationStatus.Approved,
                
            };

            var result = await userManager.CreateAsync(admin, "Admin@1234!"); // More complex password
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, AccountType.Admin.ToString());
            }
            else
            {
                // Log errors
                Console.WriteLine($"Admin creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // Seller user
        if (!await userManager.Users.AnyAsync(u => u.UserName == "seller"))
        {
            var seller = new User
            {
                UserName = "seller",
                Email = "seller@example.com",
                EmailConfirmed = true,
                FullName = "Test Seller",
                AccountType = AccountType.SellerUser,
                VerificationStatus = VerificationStatus.Approved
            };

            var result = await userManager.CreateAsync(seller, "Seller@1234!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(seller, AccountType.SellerUser.ToString());
            }
        }

        // Buyer user
        if (!await userManager.Users.AnyAsync(u => u.UserName == "buyer"))
        {
            var buyer = new User
            {
                UserName = "buyer",
                Email = "buyer@example.com",
                EmailConfirmed = true,
                FullName = "Test Buyer",
                AccountType = AccountType.BuyerUser,
                VerificationStatus = VerificationStatus.Approved
            };

            var result = await userManager.CreateAsync(buyer, "Buyer@1234!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(buyer, AccountType.BuyerUser.ToString());
            }
        }
    }
}