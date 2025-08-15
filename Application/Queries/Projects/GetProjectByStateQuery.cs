using MediatR;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Application.Queries.Projects
{
    public record GetProjectByStateQuery(ProjectStatus State) : IRequest<List<ProjectDto>>;
}
