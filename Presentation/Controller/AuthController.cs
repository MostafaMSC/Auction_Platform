using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AuctionSystem.Application.Commands.Users;
using AuctionSystem.Application.Commands.Auth;
using AuctionSystem.Application.Queries.Users;
using System.Security.Claims;
using AuctionSystem.Application.DTOs;

namespace AuctionSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            Console.WriteLine($"Login attempt: {result.Success}");
            if (!result.Success) return Unauthorized(new { result.Success, result.ErrorMessage });

            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(new SecureLoginResult
            {
                Success = true,
                AccessToken = result.AccessToken,
                User = result.User
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Try to get user ID from token first, if not available, allow logout anyway
            var userId = GetCurrentUserId();

            // Get refresh token from cookie
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            // If we have both userId and refreshToken, try to revoke it
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(refreshToken))
            {
                var command = new LogoutUserCommand(userId, refreshToken);
                await _mediator.Send(command); // Don't block logout if this fails
            }

            // Always delete the cookie and return success
            DeleteRefreshTokenCookie();
            return Ok(new LogoutResult { Success = true, Message = "Logged out successfully" });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized(new { success = false, message = "Refresh token not found" });

            var command = new RefreshTokenCommand(refreshToken);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                Response.Cookies.Delete("refreshToken");
                return Unauthorized(new { result.Success, result.ErrorMessage });
            }

            SetRefreshTokenCookie(result.NewRefreshToken);
            return Ok(new RefreshTokenResult { Success = true, AccessToken = result.AccessToken });
        }

        
        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("sub") ??
                User.FindFirstValue("uid");
        }

        private void SetRefreshTokenCookie(string token)
        {
            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30),
                Path = "/"
            });
        }

        private void DeleteRefreshTokenCookie()
        {
            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            });
        }
    }
}