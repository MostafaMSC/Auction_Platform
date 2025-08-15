using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task DeleteFileAsync(string filePath);
        Task<byte[]> GetFileAsync(string filePath);
    }
}
