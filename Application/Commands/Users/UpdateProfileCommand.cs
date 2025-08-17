using AuctionSystem.Application.Commands.Users;
using AuctionSystem.Application.Queries.Users;
using AuctionSystem.Domain.Constants;
using MediatR;

namespace AuctionSystem.Application.Commands.Users
{
    public record UpdateProfileCommand(
        string? UserId,
        string? UserName,
        string? Email,
        AccountType AccountType
    ) : IRequest<UpdateProfileResult>;
    
}
