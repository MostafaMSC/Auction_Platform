using System.Security.Claims;

namespace AuctionSystem.Infrastructure.Extensions;

public static class ClaimsIdentityExtensions
{
    public static ClaimsIdentity AddUserTypeClaim(this ClaimsIdentity identity, string type)
    {
        Claim claim = new("userType", type.Trim().ToUpper());
        identity.AddClaim(claim);
        return identity;
    }

    public static IEnumerable<Claim> AddUserTypeClaim(this IEnumerable<Claim> claims, string type)
        => claims.Union([new("userType", type.Trim().ToUpper())]);
}