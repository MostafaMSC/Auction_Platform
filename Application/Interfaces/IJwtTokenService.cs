using System.Security.Claims;
using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Application.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessTokenAsync(string userId);
        Task<string> GenerateRefreshTokenAsync(string userId, CancellationToken cancellationToken);
        Task<RefreshToken?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task SaveRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    
        Task<bool> RevokeRefreshTokenAsync(string userId, string refreshToken, CancellationToken cancellationToken);
        Task RevokeAllRefreshTokensAsync(string userId, CancellationToken cancellationToken);
}

}
