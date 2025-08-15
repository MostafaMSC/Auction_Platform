using MediatR;
using AuctionSystem.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Application.Commands.Auth;

namespace AuctionSystem.Application.Handlers.Auth
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, LogoutUserResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _tokenService;

        public LogoutUserCommandHandler(UserManager<User> userManager, IJwtTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LogoutUserResult> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find the user by ID
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return new LogoutUserResult
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Revoke the refresh token using the token service
                if (!string.IsNullOrEmpty(request.RefreshToken))
                {
                    var revoked = await _tokenService.RevokeRefreshTokenAsync(user.Id, request.RefreshToken, cancellationToken);
                    if (!revoked)
                    {
                        return new LogoutUserResult
                        {
                            Success = false,
                            Message = "Failed to revoke refresh token"
                        };
                    }
                }

                // Optionally, you could also revoke all refresh tokens for this user
                // await _tokenService.RevokeAllRefreshTokensAsync(user.Id, cancellationToken);

                return new LogoutUserResult
                {
                    Success = true,
                    Message = "Logged out successfully"
                };
            }
            catch (Exception ex)
            {
                return new LogoutUserResult
                {
                    Success = false,
                    Message = $"Logout failed: {ex.Message}"
                };
            }
        }
    }
}
