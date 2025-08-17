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

        /// <summary>
        /// تسجيل مستخدم جديد في النظام.
        /// </summary>
        /// <param name="command">بيانات التسجيل (اسم المستخدم، البريد، كلمة المرور، ...)</param>
        /// <returns>نتيجة النجاح أو الخطأ</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تسجيل الدخول (توليد Access Token + Refresh Token).
        /// </summary>
        /// <param name="command">بيانات تسجيل الدخول (Email / Username و Password)</param>
        /// <returns>توكنات JWT وبيانات المستخدم</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return Unauthorized(new { result.Success, result.ErrorMessage });

            SetRefreshTokenCookie(result.RefreshToken!);

            return Ok(new LoginUserResult
            {
                Success = true,
                AccessToken = result.AccessToken,
                User = result.User
            });
        }

        /// <summary>
        /// تسجيل الخروج وإزالة Refresh Token من الكوكيز.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = GetCurrentUserId();
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(refreshToken))
            {
                var command = new LogoutUserCommand(userId, refreshToken);
                await _mediator.Send(command);
            }

            DeleteRefreshTokenCookie();
            return Ok(new LogoutUserResult { Success = true, Message = "Logged out successfully" });
        }

        /// <summary>
        /// تجديد الـ Access Token باستخدام Refresh Token من الكوكيز (يتطلب صلاحية Admin).
        /// </summary>
        [HttpPost("refresh-token")]
        [Authorize(Roles = "Admin")]
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

            SetRefreshTokenCookie(result.NewRefreshToken!);

            return Ok(new RefreshTokenCommandResult
            {
                Success = true,
                AccessToken = result.AccessToken
            });
        }

        // =============== Helpers ===============

        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? User.FindFirstValue("uid");
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
