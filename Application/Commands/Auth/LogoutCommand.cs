using MediatR;

namespace AuctionSystem.Application.Commands.Auth
{
    public record LogoutUserCommand(string UserId, string? RefreshToken = null) : IRequest<LogoutUserResult>;

    public record LogoutUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "Logged out successfully";
    }
}
