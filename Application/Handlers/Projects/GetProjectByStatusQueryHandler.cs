using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Projects;

namespace AuctionSystem.Application.Handlers.Projects
{
    public class GetProjectsByStatusQueryHandler : IRequestHandler<GetProjectByStateQuery, List<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectsByStatusQueryHandler(IProjectRepository repo)
        {
            _projectRepository = repo;
        }

        public async Task<List<ProjectDto>> Handle(GetProjectByStateQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetByStatusAsync(request.State);

            // تحويل كل مشروع إلى DTO
            var projectDtos = projects.Select(project => new ProjectDto(
                project.Id,
                project.ProjectTitle,
                project.ProjectDescription,
                project.ProjectOwnerId,
                project.Status.ToString(),
                project.EstimatedBudget.Amount
            )).ToList();

            return projectDtos;
        }
    }
}
