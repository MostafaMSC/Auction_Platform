using AuctionSystem.Domain;
using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

public class AuctionAccessMiddleware
{
    private readonly RequestDelegate _next;

    public AuctionAccessMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
    {
        var path = context.Request.Path.Value?.ToLower();

        // فقط نفذ هذا Middleware على مسارات المزادات
        if (path != null && path.StartsWith("/api/auctions"))
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // تحقق من توثيق المستخدم
                    if (user.VerificationStatus != VerificationStatus.Approved)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("User is not verified to access auctions.");
                        return;
                    }
                }
            }
        }

        await _next(context);
    }
}
