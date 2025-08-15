using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Notifications)
                .Include(u => u.VerificationDocs)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Notifications)
                .Include(u => u.VerificationDocs)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id);
        }

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User?> GetByBidAsync(int bidId)
        {
            return await _context.Users
                .Include(u => u.Bids)
                .FirstOrDefaultAsync(u => u.Bids.Any(b => b.Id == bidId));
        }

        public async Task<User?> GetByProjectAsync(int projectId)
        {
            return await _context.Users
                .Include(u => u.Projects)
                .FirstOrDefaultAsync(u => u.Projects.Any(p => p.Id == projectId));
        }


    }
}
