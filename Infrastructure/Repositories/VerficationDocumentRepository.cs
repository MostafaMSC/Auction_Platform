using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Infrastructure.Repositories
{
    public class VerificationDocumentRepository : IVerficationDocRepository
    {
        private readonly ApplicationDbContext _context;

        public VerificationDocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VerificationDoc>> GetAllAsync()
        {
            return await _context.VerificationDocs
                .Include(v => v.UserId)
                .ToListAsync();
        }

        public async Task<VerificationDoc?> GetByIdAsync(string id)
        {
            return await _context.VerificationDocs
                .Include(v => v.UserId)
                .FirstOrDefaultAsync(v => v.Id.ToString() == id);
        }

        public async Task CreateAsync(VerificationDoc verificationDocument)
        {
            await _context.VerificationDocs.AddAsync(verificationDocument);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(VerificationDoc verificationDocument)
        {
            _context.VerificationDocs.Update(verificationDocument);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var verificationDocument = await GetByIdAsync(id);
            if (verificationDocument == null) return false;

            _context.VerificationDocs.Remove(verificationDocument);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<VerificationDoc>> GetByUserIdAsync(string userId)
        {
            return await _context.VerificationDocs
                .Where(v => v.UserId.ToString() == userId.ToString())
                .ToListAsync();
        }

    }
}
