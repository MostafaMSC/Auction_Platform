using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AuctionSystem.Application.Commands.Users;
using AuctionSystem.Application.Commands.Auth;
using AuctionSystem.Application.Queries.Users;
using AuctionSystem.Application.DTOs;

namespace AuctionSystem.Presentation.Controllers
{
    /// <summary>
    /// Controller لإدارة المستخدمين وحساباتهم
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// استرجاع ملف المستخدم الحالي (Profile)
        /// </summary>
        /// <returns>معلومات المستخدم أو Unauthorized إذا لم يتم تسجيل الدخول</returns>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var query = new GetUserProfileQuery(userId);
            var result = await _mediator.Send(query);

            if (result == null || !result.Success)
                return NotFound(new { result?.Success, result?.ErrorMessage });

            return Ok(result);
        }

        /// <summary>
        /// تحديث معلومات المستخدم الحالي
        /// </summary>
        /// <param name="command">بيانات التحديث</param>
        /// <returns>النتيجة بعد التحديث أو خطأ</returns>
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var updatedCommand = command with { UserId = userId, Email = GetCurrentEmail(), UserName = GetCurrentUserName() };
            var result = await _mediator.Send(updatedCommand);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تغيير كلمة مرور المستخدم الحالي
        /// </summary>
        /// <param name="command">بيانات تغيير كلمة المرور</param>
        /// <returns>نتيجة العملية أو خطأ</returns>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var updatedCommand = command with { UserId = userId };
            var result = await _mediator.Send(updatedCommand);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// التحقق من هوية المستخدم عن طريق رفع مستند
        /// </summary>
        /// <param name="command">بيانات التحقق</param>
        /// <returns>نتيجة التحقق أو خطأ</returns>
        [HttpPost("verify-id")]
        [Authorize]
        public async Task<IActionResult> VerifyId([FromForm] VerifyUserCommand command)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var updatedCommand = command with { UserId = userId };
            var result = await _mediator.Send(updatedCommand);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// استرجاع حالة التحقق من الهوية للمستخدم الحالي
        /// </summary>
        /// <returns>حالة التحقق من الهوية</returns>
        [HttpGet("verification-status")]
        [Authorize]
        public async Task<IActionResult> GetVerificationStatus()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var query = new GetVerificationStatusQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// استرجاع كل المستخدمين (للمسؤول فقط)
        /// </summary>
        /// <returns>قائمة المستخدمين أو NotFound إذا لا يوجد</returns>
        [HttpGet("list")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
                return NotFound(new { Success = false, Message = "No users found" });

            return Ok(result);
        }

        /// <summary>
        /// حذف مستخدم حسب المعرف (للمسؤول فقط)
        /// </summary>
        /// <param name="id">معرف المستخدم</param>
        /// <returns>نجاح العملية أو NotFound إذا لم يكن موجود</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return result ? Ok(new { Success = true, Message = "User deleted" })
                          : NotFound(new { Success = false, Message = "User not found" });
        }

        // ======== Helpers ========
        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   User.FindFirstValue("sub") ??
                   User.FindFirstValue("uid");
        }
        private string? GetCurrentEmail()
{
    return User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
}

private string? GetCurrentUserName()
{
    return User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
}

    }
}
