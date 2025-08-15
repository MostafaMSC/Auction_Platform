namespace AuctionSystem.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(string email, string password);
        Task SignOutAsync();
    }
}
