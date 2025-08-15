using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Projects;

namespace AuctionSystem.Application.Handlers.Projects
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectByIdQueryHandler(IProjectRepository repo)
        {
            _projectRepository = repo;
        }

        public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null) return null;

            return new ProjectDto(
                project.Id,
                project.ProjectTitle,
                project.ProjectDescription,
                project.ProjectOwnerId,
                project.Status.ToString(),
                project.EstimatedBudget.Amount
            );
        }
    }
}
