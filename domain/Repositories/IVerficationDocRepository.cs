using AuctionSystem.Domain.Entities;

namespace AuctionSystem.Domain.Repositories
{
    public interface IVerficationDocRepository
    {
        Task<IEnumerable<VerificationDoc>> GetAllAsync();
        Task<VerificationDoc?> GetByIdAsync(string id);
        Task<IEnumerable<VerificationDoc>> GetByUserIdAsync(string userId);
        Task CreateAsync(VerificationDoc document);
        Task<bool> UpdateAsync(VerificationDoc document);
        Task<bool> DeleteAsync(string id);
    }
}
