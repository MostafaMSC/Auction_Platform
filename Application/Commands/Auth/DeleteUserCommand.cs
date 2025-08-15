using MediatR;

namespace AuctionSystem.Application.Commands.Users
{
    public record DeleteUserCommand(string UserId) : IRequest<bool>;
}
