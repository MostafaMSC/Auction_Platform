using Microsoft.AspNetCore.Http;
using AuctionSystem.Application.Interfaces;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AuctionSystem.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;

        public LocalFileStorageService(IConfiguration configuration)
        {
            _basePath = configuration["FileStorage:LocalPath"]
                        ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            string folderPath = Path.Combine(_basePath, folderName);
            Directory.CreateDirectory(folderPath);

            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }

        public Task DeleteFileAsync(string filePath)
        {
            string fullPath = Path.Combine(_basePath, filePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            string fullPath = Path.Combine(_basePath, filePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException();

            return await File.ReadAllBytesAsync(fullPath);
        }
    }
}
