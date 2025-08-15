using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.ValueObjects;
using AuctionSystem.Application.Commands.Projects;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AuctionSystem.Application.Handlers.Projects
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateProjectCommandHandler(
            IProjectRepository projectRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _projectRepository = projectRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            // استخرج المستخدم الحالي
            var userId = _httpContextAccessor.HttpContext?.User
                             .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not logged in");

            var project = new Project(
                request.Title,
                request.Description,
                userId, // استخدم المستخدم الحالي
                request.CategoryId,
                request.Location,
                new Money(request.EstimatedBudget)
            );

            await _projectRepository.CreateAsync(project);

            return project.Id;
        }
    }
}
