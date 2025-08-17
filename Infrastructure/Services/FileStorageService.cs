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

    // Constructor يأخذ IConfiguration لتحديد مسار التخزين الأساسي
    public LocalFileStorageService(IConfiguration configuration)
    {
        // إذا لم يتم تحديد المسار في الإعدادات، نستخدم المسار الافتراضي wwwroot/uploads
        _basePath = configuration["FileStorage:LocalPath"]
                    ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        // التأكد من أن المسار موجود، وإذا لم يكن موجوداً يتم إنشاؤه
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    // ===== حفظ ملف =====
    public async Task<string> SaveFileAsync(IFormFile file, string folderName)
    {
        // إنشاء مجلد خاص بالـ folderName داخل المسار الأساسي
        string folderPath = Path.Combine(_basePath, folderName);
        Directory.CreateDirectory(folderPath);

        // إنشاء اسم فريد للملف لتجنب التعارض
        string fileName = $"{Guid.NewGuid()}_{file.FileName}";
        string filePath = Path.Combine(folderPath, fileName);

        // نسخ محتوى الملف إلى المسار النهائي
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // إعادة مسار النسخة بالنسبة للمجلدات (لتخزينه في DB مثلاً)
        return Path.Combine(folderName, fileName).Replace("\\", "/");
    }

    // ===== حذف ملف =====
    public Task DeleteFileAsync(string filePath)
    {
        string fullPath = Path.Combine(_basePath, filePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    // ===== قراءة ملف =====
    public async Task<byte[]> GetFileAsync(string filePath)
    {
        string fullPath = Path.Combine(_basePath, filePath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException(); // إذا لم يوجد الملف

        return await File.ReadAllBytesAsync(fullPath); // إرجاع محتوى الملف كبايتات
    }
}

}
