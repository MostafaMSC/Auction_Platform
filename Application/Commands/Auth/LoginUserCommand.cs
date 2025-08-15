using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Users;
using MediatR;

namespace AuctionSystem.Application.Commands.Auth
{
    public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResult>;

    public record LoginUserResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }
}