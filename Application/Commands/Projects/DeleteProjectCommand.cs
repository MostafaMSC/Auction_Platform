using MediatR;

namespace AuctionSystem.Application.Commands.Projects
{
    public record DeleteProjectCommand(int ProjectId) : IRequest<bool>;
}
