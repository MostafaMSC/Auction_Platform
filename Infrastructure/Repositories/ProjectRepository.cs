using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Auctions)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Auctions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await GetByIdAsync(id);
            if (project == null) return false;

            _context.Projects.Remove(project);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Project>> GetByUserIdAsync(string userId)
        {
            var projects = _context.Projects
                .Where(p => p.ProjectOwnerId == userId);
            return await projects.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByStatusAsync(ProjectStatus status)
        {
            var projects = _context.Projects
                .Where(p => p.Status == status);
            return await projects.ToListAsync();
        }
        public async Task<bool> SubmitAsync(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return false;

            try
            {
                project.Submit(); 
                _context.Projects.Update(project);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DomainException)
            {
                return false;
            }
        }

    }
}
