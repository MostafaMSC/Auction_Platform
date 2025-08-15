using MediatR;

namespace AuctionSystem.Application.Commands.Projects
{
    public record SubmitProjectCommand(int ProjectId) : IRequest<SubmitProjectResult>;

    public record SubmitProjectResult(bool Success, string? ErrorMessage = null);
}
