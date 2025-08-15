using AuctionSystem.Application.DTOs;
using AuctionSystem.Domain.Constants;
using MediatR;

namespace AuctionSystem.Application.Commands.Auth
{
    public record RegisterUserCommand(
        string Email, 
        string Password, 
        string ConfirmPassword,
        string Username,
        AccountType AccountType) : IRequest<RegisterUserResult>;

    public record RegisterUserResult(
    bool Success,
    string? ErrorMessage = null,
    string? UserId = null,
    UserDto? User = null
);

}