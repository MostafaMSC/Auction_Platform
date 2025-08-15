using MediatR;
using AuctionSystem.Application.DTOs;

namespace AuctionSystem.Application.Queries.Projects
{
    public record GetAllProjectsQuery : IRequest<List<ProjectDto>>;
}
