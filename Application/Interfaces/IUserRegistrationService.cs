// Application/Interfaces/IUserRegistrationService.cs
using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Entities;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<(bool Succeeded, string ErrorMessage, string? UserId)> RegisterUserAsync(
            string username,
            string email,
            string password,
            AccountType accountType);
    }
}
