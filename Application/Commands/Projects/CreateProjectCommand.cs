using AuctionSystem.Domain.ValueObjects;
using MediatR;

namespace AuctionSystem.Application.Commands.Projects
{
    public record CreateProjectCommand(
        string Title,
        string Description,
        string? OwnerId,
        int CategoryId,
        string Location,
        decimal EstimatedBudget
    ) : IRequest<int>;  // Return the new project Id
}
