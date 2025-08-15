using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuctionSystem.Infrastructure.Authorizations;

public class ClaimsTransformer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    : IClaimsTransformation
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        if (identity == null || !identity.IsAuthenticated)
        {
            return principal;
        }

        var userId = identity.FindFirst("uid")?.Value;
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
        {
            return principal;
        }

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            var roleEntity = await _roleManager.FindByNameAsync(role);
            if (roleEntity != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                foreach (var claim in roleClaims)
                {
                    identity.AddClaim(claim);
                }
            }
        }

        identity.AddUserTypeClaim(user.AccountType.ToString());

        return principal;
    }
}
