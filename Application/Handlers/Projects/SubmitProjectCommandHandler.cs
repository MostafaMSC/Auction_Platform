using AuctionSystem.Domain.Repositories;
using AuctionSystem.Application.Commands.Projects;
using MediatR;

namespace AuctionSystem.Application.Handlers.Projects
{
    public class SubmitProjectCommandHandler : IRequestHandler<SubmitProjectCommand, SubmitProjectResult>
    {
        private readonly IProjectRepository _projectRepository;

        public SubmitProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<SubmitProjectResult> Handle(SubmitProjectCommand request, CancellationToken cancellationToken)
        {
            var success = await _projectRepository.SubmitAsync(request.ProjectId);
            if (!success)
                return new SubmitProjectResult(false, "Project cannot be submitted or does not exist");

            return new SubmitProjectResult(true);
        }
    }
}
