    using AuctionSystem.Domain.Entities;

    namespace AuctionSystem.Domain.Repositories
    {
        public interface IUserRepository
        {
            Task<IEnumerable<User>> GetAllAsync();
            Task<User?> GetByIdAsync(string id);
            Task<User?> GetByBidAsync(int bidId);
            Task<User?> GetByProjectAsync(int projectId);

            Task CreateAsync(User user);
            Task<bool> UpdateAsync(User user);
            Task<bool> DeleteAsync(string id);
        }
    }
