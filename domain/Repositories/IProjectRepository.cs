using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByUserIdAsync(string userId);

        Task<IEnumerable<Project>> GetByStatusAsync(ProjectStatus status);

        Task<Project?> GetByIdAsync(int id);
        Task CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(int id);
        Task<bool> SubmitAsync(int projectId);

    }
}
