using AuctionSystem.Domain.Constants;
using Microsoft.AspNetCore.Identity;

public class RoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<List<RoleDto>> GetRolesWithPermissionsAsync()
    {
        var roles = _roleManager.Roles.ToList();
        var result = new List<RoleDto>();

        foreach (var role in roles)
        {
            // Example: fetching permissions from constants
            var permissions = RolePermissions.GetPermissionsForRole(role.Name); // assume this returns List<string>

            result.Add(new RoleDto
            {
                Name = role.Name,
                Permissions = permissions
            });
        }

        return result;
    }
}
