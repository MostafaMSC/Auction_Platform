using AuctionSystem.Application.Interfaces;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionSystem.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public JwtTokenService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> GenerateAccessTokenAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId)
                       ?? throw new Exception("User not found");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.AccountType.ToString()) // Default role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpirationMinutes = int.Parse(_configuration["JwtSettings:DurationInMinutes"] ?? "30");

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId, CancellationToken cancellationToken)
        {
            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "30");

            var refreshToken = new RefreshToken(userId, refreshTokenExpirationDays);

            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return refreshToken.Token;
        }

        public async Task<RefreshToken?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == refreshToken, cancellationToken);
        }

        public async Task SaveRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // مهم: نسمح بالتحقق من التوكن المنتهي
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }
        public async Task<bool> RevokeRefreshTokenAsync(string userId, string refreshToken, CancellationToken cancellationToken)
{
    var token = await GetRefreshToken(refreshToken, cancellationToken);
    if (token == null || token.UserId != userId)
        return false;

    _context.RefreshTokens.Remove(token);
    await _context.SaveChangesAsync(cancellationToken);
    return true;
}

public async Task RevokeAllRefreshTokensAsync(string userId, CancellationToken cancellationToken)
{
    var tokens = _context.RefreshTokens.Where(t => t.UserId == userId);
    _context.RefreshTokens.RemoveRange(tokens);
    await _context.SaveChangesAsync(cancellationToken);
}

    }
}
