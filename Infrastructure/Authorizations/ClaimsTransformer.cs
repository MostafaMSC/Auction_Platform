using AuctionSystem.Domain.Entities;
using AuctionSystem.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuctionSystem.Infrastructure.Authorizations;

// كلاس لتحويل Claims للمستخدمين عند المصادقة
public class ClaimsTransformer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    : IClaimsTransformation
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    // الدالة الأساسية لإضافة claims إضافية للمستخدم بعد المصادقة
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        // إذا لم يكن المستخدم مصادق عليه، نعيد نفس principal
        if (identity == null || !identity.IsAuthenticated)
        {
            return principal;
        }

        // جلب معرف المستخدم من الـ claims
        var userId = identity.FindFirst("uid")?.Value;
        var user = await _userManager.FindByIdAsync(userId!);

        // إذا لم يتم العثور على المستخدم، نعيد نفس principal
        if (user == null)
        {
            return principal;
        }

        // جلب جميع أدوار المستخدم
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            // جلب الكيان الخاص بالدور
            var roleEntity = await _roleManager.FindByNameAsync(role);
            if (roleEntity != null)
            {
                // جلب جميع الـ claims المرتبطة بالدور وإضافتها للمستخدم
                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                foreach (var claim in roleClaims)
                {
                    identity.AddClaim(claim);
                }
            }
        }

        // إضافة claim يوضح نوع الحساب
        identity.AddUserTypeClaim(user.AccountType.ToString());
        
        return principal;
    }
}
