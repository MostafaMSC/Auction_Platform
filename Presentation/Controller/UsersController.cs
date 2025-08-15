using MediatR;
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
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

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

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var updatedCommand = command with { UserId = userId };
            var result = await _mediator.Send(updatedCommand);
            return result.Success ? Ok(result) : BadRequest(result);
        }

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

[HttpPost("verify-id")]
[Authorize]
public async Task<IActionResult> VerifyId([FromForm] VerifyUserCommand command)
{
    var userId = GetCurrentUserId();
    if (userId == null) return Unauthorized();

    // Override the UserId from the JWT
    var updatedCommand = command with { UserId = userId };
    
    var result = await _mediator.Send(updatedCommand);
    return result.Success ? Ok(result) : BadRequest(result);
}


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
        [HttpDelete("{id}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> DeleteUser(string id)
            {
                var result = await _mediator.Send(new DeleteUserCommand(id));
                return result ? Ok(new { Success = true, Message = "User deleted" })
                            : NotFound(new { Success = false, Message = "User not found" });
            }


        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("sub") ??
                User.FindFirstValue("uid");
        }
    }

    // Response DTOs
    public record SecureLoginResult
    {
        public bool Success { get; set; } = true;
        public string AccessToken { get; set; } = string.Empty;
        public UserDto User { get; set; } = default!;
    }

    public record RefreshTokenResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string NewRefreshToken { get; set; } = string.Empty;
    }

    public record LogoutResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "Logged out successfully";
    }

    public record UserProfileResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public UserDto? User { get; set; }
    }

    public record UpdateUserResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public UserDto? User { get; set; }
    }

    public record ChangePasswordResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public record RevokeTokensResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "All tokens revoked successfully";
    }

    public record VerifyIdResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public record VerificationStatusResult
    {
        public bool Success { get; set; }
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
        public string? DocumentType { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }
    }
}