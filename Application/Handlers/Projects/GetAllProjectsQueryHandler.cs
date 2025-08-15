using MediatR;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Projects;
using AuctionSystem.Domain.Repositories;

namespace AuctionSystem.Application.Handlers.Projects
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetAllProjectsQueryHandler(IProjectRepository repo)
        {
            _projectRepository = repo;
        }

        public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllAsync();

            var dtoList = projects.Select(p => new ProjectDto(
                p.Id,
                p.ProjectTitle,
                p.ProjectDescription,
                p.ProjectOwnerId,
                p.Status.ToString(),
                p.EstimatedBudget.Amount
            )).ToList();

            return dtoList;
        }
    }
}
