using MediatR;
using AuctionSystem.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Application.Commands.Auth;
using AuctionSystem.Application.DTOs;

namespace AuctionSystem.Application.Handlers.Users
{
    // Login Handler Example
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _tokenService;

    public LoginUserCommandHandler(UserManager<User> userManager, IJwtTokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // جلب المستخدم بواسطة البريد
            var user = await _userManager.FindByEmailAsync(request.Email);
Console.WriteLine(user != null 
    ? $"User found: {user.Email}" 
    : "User not found");

if (user == null)
    return new LoginUserResult
    {
        Success = false,
        ErrorMessage = "Invalid credentials"
    };

var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
Console.WriteLine($"Password valid: {isPasswordValid}");

if (!isPasswordValid)
    return new LoginUserResult
    {
        Success = false,
        ErrorMessage = "Invalid credentials"
    };

            // توليد التوكنات
Console.WriteLine("Generating access token...");
var accessToken = await _tokenService.GenerateAccessTokenAsync(user.Id);
Console.WriteLine($"AccessToken: {accessToken}");

Console.WriteLine("Generating refresh token...");
var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);
Console.WriteLine($"RefreshToken: {refreshToken}");

            // تحديث آخر تسجيل دخول
            await _userManager.UpdateAsync(user);

            return new LoginUserResult
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Username = user.UserName!,
                    AccountType = user.AccountType,
                    VerificationStatus = user.VerificationStatus
                }
            };
        }
        catch (Exception)
        {
            return new LoginUserResult
            {
                Success = false,
                ErrorMessage = "Login failed"
            };
        }
    }
}

}