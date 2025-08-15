using MediatR;
using AuctionSystem.Application.DTOs;

namespace AuctionSystem.Application.Queries.Projects
{
    public record GetProjectByIdQuery(int Id) : IRequest<ProjectDto?>;
}
