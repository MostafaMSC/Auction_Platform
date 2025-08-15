
using MediatR;

namespace AuctionSystem.Application.Commands.Auth
{
    public record ChangePasswordCommand : IRequest<ChangePasswordCommandResult>
    {
        public string UserId { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public record ChangePasswordCommandResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}