using System.Security.Claims;
using AuctionSystem.Application.Interfaces;
using MediatR;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResult>
    {
        private readonly IJwtTokenService _tokenService;

        public RefreshTokenCommandHandler(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<RefreshTokenCommandResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(request.RefreshToken);
                var userId = principal.FindFirstValue("uid");

                if (string.IsNullOrEmpty(userId))
                {
                    return new RefreshTokenCommandResult
                    {
                        Success = false,
                        ErrorMessage = "Invalid token"
                    };
                }

                var storedRefreshToken = await _tokenService.GetRefreshToken(request.RefreshToken, cancellationToken);
                if (storedRefreshToken == null || !storedRefreshToken.IsActive)
                {
                    return new RefreshTokenCommandResult
                    {
                        Success = false,
                        ErrorMessage = "Invalid or expired refresh token"
                    };
                }

                // Generate new tokens
                var newAccessToken = await _tokenService.GenerateAccessTokenAsync(userId);
                var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(userId, cancellationToken);

                // Revoke old token
                storedRefreshToken.IsRevoked = true;
                storedRefreshToken.RevokedAt = DateTime.UtcNow;
                storedRefreshToken.ReplacedByToken = newRefreshToken;
                await _tokenService.SaveRefreshToken(storedRefreshToken, cancellationToken);

                return new RefreshTokenCommandResult
                {
                    Success = true,
                    AccessToken = newAccessToken,
                    NewRefreshToken = newRefreshToken
                };
            }
            catch (Exception ex)
            {
                return new RefreshTokenCommandResult
                {
                    Success = false,
                    ErrorMessage = "Token refresh failed"
                };
            }
        }
    }